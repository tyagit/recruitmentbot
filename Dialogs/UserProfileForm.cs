using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using RecruitmentQnA.DAL;
using RecruitmentQnA.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RecruitmentQnA.Dialogs
{
    [Serializable]
    public class UserProfileForm
    {
        public string FirstName;
        public string LastName;
        public string PhoneNo;
        public string EmailId;

        public static IForm<UserProfileForm> BuildForm()
        {
            OnCompletionAsyncDelegate<UserProfileForm> callQnA = async (context, state) =>
            {
                //await context.Forward(new RecruitmentQnADialog(), onQnACompletion, state, CancellationToken.None);
                //
            };
            return new FormBuilder<UserProfileForm>()
                .OnCompletion(callQnA)
                .Build();
        }
        private static async Task onQnACompletion(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }
        public static Attachment GetUserProfileForm()
        {
            var card = new AdaptiveCard()
            {
                Body = new List<CardElement>()
                {
                    new TextBlock() {
                        Text = "Please Share your detail for contact:",
                        Size = TextSize.Large,
                        Weight = TextWeight.Bolder
                    },
                    new TextBlock() { Text = "First Name" },
                    new TextInput()
                    {
                        Id = "First Name",
                        Speak = "<s>Please enter your first name</s>",
                        Placeholder = "Please enter your first name",
                        Style = TextInputStyle.Text
                    },
                    new TextBlock() { Text = "Last Name" },
                    new TextInput()
                    {
                        Id = "Last Name",
                        Speak = "<s>Please enter your last name</s>",
                        Placeholder = "Please enter your last name",
                        Style = TextInputStyle.Text
                    },
                    new TextBlock() { Text = "Email" },
                    new TextInput()
                    {
                        Id = "Email",
                        Speak = "<s>Please enter your email</s>",
                        Placeholder = "Please enter your email",
                        Style = TextInputStyle.Text
                    },
                    new TextBlock() { Text = "Phone" },
                    new TextInput()
                    {
                        Id = "Phone",
                        Speak = "<s>Please enter your phone</s>",
                        Placeholder = "Please enter your Phone",
                        Style = TextInputStyle.Text
                    },
                },
                Actions = new List<ActionBase>()
                {
                    new SubmitAction()
                    {
                        Title = "Submit",
                        Speak = "<s>Submit</s>",
                        DataJson = JObject.FromObject(new { datatype = "userprofile"}).ToString()
                    }
                }
            };
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }
        public static int SaveUserProfile(string jsondata)
        {
            JObject jObject = JObject.Parse(jsondata);
            
            UserProfile user = new UserProfile();
            user.FirstName = jObject["First Name"].ToString();
            user.LastName = jObject["Last Name"].ToString();
            user.Email = jObject["Email"].ToString();
            user.Phone = jObject["Phone"].ToString();
            return UserProfileDAL.SaveUserProfile(user);            
        }

    }
}