using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using RecruitmentQnA.Services;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentQnA.Dialogs
{
    [Serializable]
    //[QnAMaker("subkey", "kbid")]
    public class RecruitmentQnADialog : QnAMakerDialog
    {
        public RecruitmentQnADialog() : base(new QnAMakerService(new QnAMakerAttribute
            (ConfigurationManager.AppSettings["QnASubscriptionKey"],
            ConfigurationManager.AppSettings["QnAKnowledgebaseId"], 
            "No good match in FAQ")))
        { }
        /// <summary>
        /// overrides RespondFromQnAMakerResultAsync from QnAMakerDialog to create reply with bot avatar
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            var answer = result.Answers.First().Answer;
            Activity reply = ((Activity)context.Activity).CreateReply();
            var card = "NormalMessage";
            if (answer.Contains("\"card\""))
            {
                var jObjectValue = JObject.Parse(answer);
                card = jObjectValue["card"].ToString();
                answer = jObjectValue["answer"].ToString();
            }

            Attachment cardAttachment = await CardService.GetCardAttachment(card, new object[] { answer, context });
            reply.Attachments.Add(cardAttachment);
            await context.PostAsync(reply);
        }
        
        public static async Task ResumeAfterSimpleQnADialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }
    }
}