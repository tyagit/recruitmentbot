using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Linq;
using RecruitmentQnA.Bot.CardProviders;
using RecruitmentQnA.Models;
using RecruitmentQnA.Dialogs;
using System.Threading;
using Newtonsoft.Json.Linq;
using RecruitmentQnA.DAL;

namespace RecruitmentQnA.Services
{
    public class CardService
    {
        public const string UserDataKey = "UserData";

        private static Lazy<List<CardProvider>> _cardHandlers = new Lazy<List<CardProvider>>(() =>
        {
            return Assembly.GetCallingAssembly().DefinedTypes
                .Where(t => (typeof(CardProvider) != t && typeof(CardProvider).IsAssignableFrom(t) && !t.IsAbstract))
                .Select(t => (CardProvider)Activator.CreateInstance(t))
                .ToList();
        });

        public async Task<CardResult> GetNextCardText(IDialogContext context, Activity activity)
        {
            var botdata = context.PrivateConversationData;
            var userInput = activity.Text;
            UserProfile userProfileData = null;

            if (botdata.ContainsKey(UserDataKey))
                userProfileData = botdata.GetValue<UserProfile>(UserDataKey);

            var jObjectValue = activity.Value as JObject;

            var cardProvider = Convert.ToString(jObjectValue["nextcard"]);

            if (userProfileData == null)
                userProfileData = new UserProfile();

            if (cardProvider == "ShowInterviewDate")
                userProfileData.InterviewDate = CommonDAL.getInterviewDate();

            if (cardProvider != "")
            {
                Type cardProviderType = Type.GetType("RecruitmentQnA.Bot.CardProviders." + cardProvider);
                if (cardProviderType != null)
                {
                    object classInstance = Activator.CreateInstance(cardProviderType, null);
                    MethodInfo methodInfo = cardProviderType.GetMethod("GetCardResult");
                    object[] parametersArray = new object[] { userProfileData, jObjectValue, activity.Text, context };

                    dynamic cardResult = methodInfo.Invoke(classInstance, parametersArray);
                    CardResult r = cardResult.Result;

                    if (string.IsNullOrEmpty(r.ErrorMessage))
                    {
                        var action = Convert.ToString(jObjectValue["action"]);
                        if (!string.IsNullOrEmpty(action))
                        {
                            string validationErr = ValidateInfo(jObjectValue);
                            if (string.IsNullOrEmpty(validationErr))
                                userProfileData = AssignValuesToUser(userProfileData, jObjectValue);
                            else
                                r.ErrorMessage = validationErr;

                            if (action.ToLower() == "saveindb" && string.IsNullOrEmpty(r.ErrorMessage))
                            {
                                int result = UserProfileDAL.SaveUserProfile(userProfileData);
                                if (result == 0)
                                    r.ErrorMessage = "I'm sorry, I couldn't save your record. Please contact helpdesk/support for more information.";
                            }
                            botdata.SetValue(UserDataKey, userProfileData);
                        }
                    }

                    return r;
                }
                else
                    new CardResult() { ErrorMessage = "I'm sorry, I don't understand.  Please rephrase, or use the Card to respond." };
            }
            return new CardResult() { ErrorMessage = "I'm sorry, I don't understand.  Please rephrase, or use the Card to respond." };
        }

        private string ValidateInfo(JObject jObjectValue)
        {
            var FromCard = Convert.ToString(jObjectValue["fromcard"]);
            string err = string.Empty;
            switch (FromCard)
            {
                case "JoinUs":
                    err = ValidateJoinUs(jObjectValue);
                    break;
                default:
                    break;
            }
            return err;
        }

        private string ValidateJoinUs(JObject jObjectValue)
        {
            string err = string.Empty;

            var FirstName = Convert.ToString(jObjectValue["First Name"]);
            var Email = Convert.ToString(jObjectValue["Email"]);
            var Phone = Convert.ToString(jObjectValue["Phone"]);

            if (string.IsNullOrEmpty(FirstName))
                return "Please enter first name";
            if (string.IsNullOrEmpty(Email))
                return "Please enter email.";
            if (string.IsNullOrEmpty(Phone))
                return "Please enter phone.";

            return err;
        }

        private static UserProfile AssignValuesToUser(UserProfile userProfileData, JObject jObjectValue)
        {
            var FirstName = Convert.ToString(jObjectValue["First Name"]);
            var LastName = Convert.ToString(jObjectValue["Last Name"]);
            var Email = Convert.ToString(jObjectValue["Email"]);
            var Phone = Convert.ToString(jObjectValue["Phone"]);
            var FromCard = Convert.ToString(jObjectValue["fromcard"]);
            var InterviewDate = Convert.ToString(jObjectValue["Date"]);
            var JoinUsSourceInfo = Convert.ToString(jObjectValue["JoinUsSourceInfo"]);
            var AvailableStartDate = Convert.ToString(jObjectValue["AvailableStartDate"]);
            var ResignationDate = Convert.ToString(jObjectValue["ResignationDate"]);
            var Account = Convert.ToString(jObjectValue["Account"]);
            var datajson = jObjectValue;
            datajson.Properties()
                .Where(attr => (attr.Name == "First Name" || attr.Name == "Last Name" || attr.Name == "Email" || attr.Name == "Phone"
                || attr.Name == "fromcard" || attr.Name == "Date" || attr.Name == "JoinUsSourceInfo" || attr.Name == "AvailableStartDate"
                || attr.Name == "ResignationDate" || attr.Name == "Account"
                || attr.Name == "action" || attr.Name == "nextcard"))
                .ToList()
                .ForEach(attr => attr.Remove());

            if (userProfileData.DataJSon == null) userProfileData.DataJSon = "{}";
            var temp = JObject.Parse(userProfileData.DataJSon);
            temp.Merge(datajson, new JsonMergeSettings()
            { MergeArrayHandling = MergeArrayHandling.Union });

            userProfileData.DataJSon = temp.ToString();
            if (!string.IsNullOrEmpty(FirstName))
                userProfileData.FirstName = FirstName;
            if (!string.IsNullOrEmpty(LastName))
                userProfileData.LastName = LastName;
            if (!string.IsNullOrEmpty(Email))
                userProfileData.Email = Email;
            if (!string.IsNullOrEmpty(Phone))
                userProfileData.Phone = Phone;
            if (!string.IsNullOrEmpty(FromCard))
                userProfileData.FromCard = FromCard;
            if (!string.IsNullOrEmpty(InterviewDate))
                userProfileData.InterviewDate = InterviewDate;
            if (!string.IsNullOrEmpty(JoinUsSourceInfo))
                userProfileData.JoinUsSourceInfo = JoinUsSourceInfo;
            if (!string.IsNullOrEmpty(AvailableStartDate))
                userProfileData.AvailableStartDate = AvailableStartDate;
            if (!string.IsNullOrEmpty(ResignationDate))
                userProfileData.ResignationDate = ResignationDate;
            if (!string.IsNullOrEmpty(Account))
                userProfileData.Account = Account;

            return userProfileData;
        }

        public static async Task<Attachment> GetCardAttachment(string cardName, object[] parameters = null)
        {
            Attachment attachment = new Attachment();
            attachment = (Attachment)typeof(BotCardAttachments).GetMethod(cardName).Invoke(null, parameters);
            return attachment;
        }
    }
}