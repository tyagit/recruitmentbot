using Microsoft.Bot.Connector;

namespace RecruitmentQnA.Models
{
    public class CardResult
    {
        public string ErrorMessage { get; set; }
        public Attachment CardText { get; set; }
    }
}