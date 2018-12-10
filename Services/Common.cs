using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace RecruitmentQnA
{
    public class Common
    {
       
        public static string AnaBotConnection = ConfigurationManager.AppSettings["dbcon"];//"";
        public static List<string> sessionStart = new List<string>(new string[] { "restart", "reset", "start over" });
        public static string annaIcon = "data:image/png;base64," + "sampleimageiconinbyteformat";
        
        /// <summary>
        /// creates adaptive card with chat bot avatar
        /// reply: context activity for which reply is to be created
        /// text: Message text to be displayed in adaptive card 
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Activity formatMessage(Activity reply, string text, string actions)
        {
            string description = text;
            string imageURL = Common.annaIcon;

            var Items = new List<CardElement>()
            {
                new ColumnSet()
                {
                    Columns = new List<Column>()
                    {
                        new Column()
                        {
                            Size = ColumnSize.Auto,
                            Items = new List<CardElement>()
                            {
                                new Image()
                                {
                                    Url = imageURL, Size = ImageSize.Small, Style = ImageStyle.Person
                                }
                            }
                        },
                        new Column()
                        {
                            Size = "280",
                            Items = new List<CardElement>()
                            {
                                new TextBlock()
                                {
                                    Text = description,
                                    Size = TextSize.Normal,
                                    Wrap = true
                                }
                            }
                        }
                    }
                }
            };

            var card = new AdaptiveCard();
            card.Body = Items;

            if (!string.IsNullOrEmpty(actions))
            {
                string[] actionArr = actions.Split('~');
                foreach (var action in actionArr)
                {
                    JObject jObject = JObject.Parse(action);
                    string title = (string)jObject.SelectToken("title");
                    string data = (string)jObject.SelectToken("jsondata");
                    string type = (string)jObject.SelectToken("type");
                    if (type == "submit")
                    {
                        var act = new SubmitAction()
                        {
                            Title = title,
                            DataJson = data//"{ \"Type\": \"Recursive\" }"
                        };
                        card.Actions.Add(act);
                    }
                }
            }

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            reply.Attachments.Add(attachment);
            return reply;
        }

        public static AdaptiveCard GetSingleMessageCard(string desc, IDialogContext context)
        {

            desc = desc.ToUserLocale(context);

            string imageURL = Common.annaIcon;

            var card = new AdaptiveCard()
            {
                Body = new List<CardElement>()
                {
                    new ColumnSet()
                    {
                        Columns = new List<Column>()
                        {
                            new Column()
                            {
                                Size = ColumnSize.Auto,
                                Items = new List<CardElement>()
                                {
                                    new Image()
                                    {
                                        Url = imageURL,
                                        Size = ImageSize.Small, Style = ImageStyle.Person
                                    }
                                }
                            },
                            new Column()
                            {
                                Size = "280",
                                Items = new List<CardElement>()
                                {
                                    new TextBlock()
                                    {
                                        Text = desc,
                                        Size = TextSize.Normal,
                                        Wrap = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            return card;
        }
    }
      
}