using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using RecruitmentQnA.Dialogs;
using RecruitmentQnA.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using static RecruitmentQnA.Utilities;

namespace RecruitmentQnA
{
    //[BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //to show user that bot is typing
                //var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                //Activity isTypingReply = activity.CreateReply();
                //isTypingReply.Type = ActivityTypes.Typing;
                //isTypingReply.Text = null;
                //await connector.Conversations.ReplyToActivityAsync(isTypingReply);


                if (!string.IsNullOrEmpty(activity.Text))
                {
                    var userLanguage = TranslationHandler.DetectLanguage(activity);

                    //save user's LanguageCode to Azure Storage
                    var message = activity as IMessageActivity;

                    try
                    {
                        using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
                        {
                            var botDataStore = scope.Resolve<IBotDataStore<BotData>>();
                            var key = new AddressKey()
                            {
                                BotId = message.Recipient.Id,
                                ChannelId = message.ChannelId,
                                UserId = message.From.Id,
                                ConversationId = message.Conversation.Id,
                                ServiceUrl = message.ServiceUrl
                            };

                            var userData = await botDataStore.LoadAsync(key, BotStoreType.BotUserData, CancellationToken.None);

                            var storedLanguageCode = userData.GetProperty<string>(StringConstants.UserLanguageKey);

                            //update user's language in Azure Storage
                            if (storedLanguageCode != userLanguage)
                            {
                                userData.SetProperty(StringConstants.UserLanguageKey, userLanguage);
                                await botDataStore.SaveAsync(key, BotStoreType.BotUserData, userData, CancellationToken.None);
                                await botDataStore.FlushAsync(key, CancellationToken.None);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    
                    activity.Text = TranslationHandler.TranslateTextToDefaultLanguage(activity, userLanguage);                    
                }
                //await Conversation.SendAsync(activity, MakeRootDialog);
                //await Conversation.SendAsync(activity, () => new RootDialog());
                //await Conversation.SendAsync(activity, () => new Dialogs.SimpleQnADialog());
                await SendAsync(activity, (scope) => new RootDialog(scope.Resolve<IUserToAgent>()));
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)// && message.ChannelId != "directline")
            {
                MicrosoftAppCredentials.TrustServiceUrl(message.ServiceUrl);
                IConversationUpdateActivity iConversationUpdated = message as IConversationUpdateActivity;
                if (iConversationUpdated != null)
                {
                    ConnectorClient connector = new ConnectorClient(new System.Uri(message.ServiceUrl));
                    foreach (var member in iConversationUpdated.MembersAdded ?? System.Array.Empty<ChannelAccount>())
                    {
                        if (member.Id == iConversationUpdated.Recipient.Id && message.ChannelId != "directline")
                        {
                            var cardText = await CardService.GetCardAttachment("FirstWelcome");
                            var reply = RootDialog.GetCardReply(message, cardText);
                            
                            await connector.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }
            else if (message.Type == ActivityTypes.Event)
            {
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
                {
                    var cancellationToken = default(CancellationToken);
                    var agentService = scope.Resolve<IAgentService>();
                    switch (message.AsEventActivity().Name)
                    {
                        case "connect":
                            await agentService.RegisterAgentAsync(message, cancellationToken);
                            break;
                        case "disconnect":
                            await agentService.UnregisterAgentAsync(message, cancellationToken);
                            break;
                        case "stopConversation":
                            await StopConversation(agentService, message, cancellationToken);
                            await agentService.RegisterAgentAsync(message, cancellationToken);
                            break;
                        default:
                            break;
                    }
                }
            }
            return null;
        }

        private async Task StopConversation(IAgentService agentService, Activity agentActivity, CancellationToken cancellationToken)
        {
            var user = await agentService.GetUserInConversationAsync(agentActivity, cancellationToken);
            var agentReply = agentActivity.CreateReply();
            if (user == null)
            {
                agentReply.Text = "Hey! You were not talking to anyone.";
                await SendToConversationAsync(agentReply);
                return;
            }

            var userActivity = user.ConversationReference.GetPostToBotMessage();
            await agentService.StopAgentUserConversationAsync(
                userActivity,
                agentActivity,
                cancellationToken);

            var userReply = userActivity.CreateReply();
            userReply.Text = "You have been disconnected from our representative.";
            await SendToConversationAsync(userReply);
            userReply.Text = "But we can still talk :)";
            await SendToConversationAsync(userReply);

            agentReply.Text = "You have stopped the conversation.";
            await SendToConversationAsync(agentReply);
        }

        private async Task SendAsync(IMessageActivity toBot, Func<ILifetimeScope, IDialog<object>> MakeRoot, CancellationToken token = default(CancellationToken))
        {
            using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, toBot))
            {
                DialogModule_MakeRoot.Register(scope, () => MakeRoot(scope));
                var task = scope.Resolve<IPostToBot>();
                await task.PostAsync(toBot, token);
            }
        }
    }
}