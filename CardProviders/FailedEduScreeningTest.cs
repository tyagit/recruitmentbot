using System.Threading.Tasks;
using RecruitmentQnA.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Bot.Builder.Dialogs;

namespace RecruitmentQnA.Bot.CardProviders
{
    public class FailedEduScreeningTest : CardProvider
    {
        public override string CardName => "FailedEduScreeningTest";

        public override async Task<CardResult> GetCardResult(UserProfile userData, JObject messageValue, string messageText, IDialogContext context)
        {
            return new CardResult() { CardText = await base.GetCardAttachment(new object[] { context }) };
        }
    }
}