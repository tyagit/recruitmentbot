
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using RecruitmentQnA.Models;
using RecruitmentQnA.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentQnA.Bot.CardProviders
{
    public abstract class CardProvider
    {
        public abstract string CardName { get; }
        
        public abstract Task<CardResult> GetCardResult(UserProfile userData, JObject value, string messageText, IDialogContext context);

        protected async Task<Attachment> GetCardAttachment(object[] parameters = null)
        {
            Attachment cardJson = await CardService.GetCardAttachment(CardName, parameters);
            //if (string.IsNullOrEmpty(cardJson))
            //    return string.Empty;

            //if (parameters == null)
            //    return cardJson;

            //foreach (var replaceKvp in replaceInfo)            
            //     cardJson = cardJson.Replace(replaceKvp.Key, replaceKvp.Value);
            
            return cardJson;
        }
    }
}