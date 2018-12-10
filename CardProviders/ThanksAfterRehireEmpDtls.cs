using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json.Linq;
using RecruitmentQnA.Models;
using System.Threading.Tasks;

namespace RecruitmentQnA.Bot.CardProviders
{
    public class ThanksAfterRehireEmpDtls : CardProvider
    {
        public override string CardName => "ThanksAfterRehireEmpDtls";

        public override async Task<CardResult> GetCardResult(UserProfile userData, JObject messageValue, string messageText, IDialogContext context)
        {
            return new CardResult() { CardText = await base.GetCardAttachment(new object[] { context }) };
        }
    }
}