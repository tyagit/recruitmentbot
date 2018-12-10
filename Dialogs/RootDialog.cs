using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.FormFlow;
using System.Threading;
using System.Collections.Generic;
using System.Web;
using AdaptiveCards;
using Newtonsoft.Json.Linq;
using RecruitmentQnA.Services;
using Newtonsoft.Json;

namespace RecruitmentQnA.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string ERROR =
            "Sorry, I didn't understand that.";
        private readonly IUserToAgent _userToAgent;

        public RootDialog(IUserToAgent userToAgent)
        {
            _userToAgent = userToAgent;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            string text = string.IsNullOrEmpty(activity.Text) ? string.Empty : activity.Text.ToLower().Trim();

            IMessageActivity nextMessage = null;

            if (!string.IsNullOrEmpty(text))//user typed a message
            {
                nextMessage = await GetMessageFromText(context, activity, text);
            }

            if (nextMessage == null)//user typed a message but not recognized by bot or user used a card to reply
            {
                if (!string.IsNullOrEmpty(text))//user typed a message
                {
                    activity.Text = text;
                    await context.Forward(new RecruitmentQnADialog(), RecruitmentQnADialog.ResumeAfterSimpleQnADialog, context.Activity, CancellationToken.None);
                    return;
                }
                var jObjectValue = activity.Value as JObject;
                var cardProvider = jObjectValue["nextcard"].ToString();

                if (cardProvider == "Qna")
                {
                    var datatext = jObjectValue["text"].ToString();
                    if (!string.IsNullOrEmpty(datatext))//qna with a question using card reply
                    {
                        nextMessage = await GetMessageFromText(context, activity, datatext.ToLower());
                        if (nextMessage == null)
                        {
                            activity.Text = datatext;
                            await context.Forward(new RecruitmentQnADialog(), RecruitmentQnADialog.ResumeAfterSimpleQnADialog, context.Activity, CancellationToken.None);
                            return;
                        }
                    }
                    else
                        nextMessage = await GetNextCardMessage(context, activity);
                }
                else if (cardProvider == "Options")
                {
                    var InterviewLocationChoice = Convert.ToString(jObjectValue["InterviewLocationChoice"]);
                    if (!string.IsNullOrEmpty(InterviewLocationChoice))
                    {
                        jObjectValue["nextcard"] = jObjectValue["InterviewLocationChoice"];
                        activity.Value = jObjectValue;
                        nextMessage = await GetNextCardMessage(context, activity);
                    }
                    else
                        nextMessage = await GetNextCardMessage(context, activity);
                }
                else
                    nextMessage = await GetNextCardMessage(context, activity);
            }

            await context.PostAsync(nextMessage);
            context.Wait(MessageReceivedAsync);
        }

        private async Task<IMessageActivity> GetMessageFromText(IDialogContext context, Activity activity, string text)
        {
            IMessageActivity nextMessage = null;

            if (text == "reset"
                     || text == "start over"
                     || text == "restart")
            {
                context.PrivateConversationData.Clear();
                nextMessage = await GetCard(activity, "Welcome", context);
            }
            else if(text == "connect with agent")
            {
                Attachment cardAttachment = null;
                var msg = "";
                var agent = await _userToAgent.IntitiateConversationWithAgentAsync(activity, default(CancellationToken));
                if (agent == null)
                {
                    //await context.PostAsync("All our customer care representatives are busy at the moment. Please try after some time.");
                    msg = "All our customer care representatives are busy at the moment. Please try after some time.";                    
                }
                else
                {
                    msg = "Hello, how can I help you?";
                }
                cardAttachment = await CardService.GetCardAttachment("NormalMessage", new object[] { msg, context });
                nextMessage = GetCardReply(activity, cardAttachment);
            }
            else
            {
                //TODO: help
            }

            return nextMessage;
        }

        private async Task<IMessageActivity> GetCard(Activity activity, string cardName, IDialogContext context)
        {
            var cardText = await CardService.GetCardAttachment(cardName, new object[] { context }); //await CardService.GetCardText(cardName);
            return GetCardReply(activity, cardText);
        }

        private async Task<IMessageActivity> GetNextCardMessage(IDialogContext context, Activity activity)
        {
            var resultInfo = await new CardService().GetNextCardText(context, activity);
            if (!string.IsNullOrEmpty(resultInfo.ErrorMessage))
            {
                var cardText = await CardService.GetCardAttachment("NormalMessage", new object[] { resultInfo.ErrorMessage, context });
                return GetCardReply(activity, cardText);
                //return activity.CreateReply(resultInfo.ErrorMessage);
            }
            return GetCardReply(activity, resultInfo.CardText);
        }

        public static Activity GetCardReply(Activity activity, Attachment cardText)
        {
            var reply = activity.CreateReply();
            reply.Attachments.Add(cardText);
            return reply;
        }
    }
}