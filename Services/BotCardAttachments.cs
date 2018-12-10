using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web;

namespace RecruitmentQnA.Services
{
    public class BotCardAttachments
    {
        public static Attachment FirstWelcome()
        {
            string description = "Hi, I'm Ana. Let's get started.\n You can always start over by typing \"Restart\" or \"Reset\"";

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
                                        Text = description,
                                        Size = TextSize.Normal,
                                        Wrap = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            card.Actions.Add(new SubmitAction()
            {
                Title = "Answer Questions",
                DataJson = JObject.FromObject(new { fromcard = "FirstWelcome", nextcard = "AnswerQuestions" }).ToString()
            });

            card.Actions.Add(new SubmitAction()
            {
                Title = "Join Us",
                DataJson = JObject.FromObject(new { nextcard = "JoinUs" }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment Welcome(IDialogContext context)
        {
            string description = "Hi, I'm Ana. Let's get started.\n You can always start over by typing \"Restart\" or \"Reset\"";

            var card = Common.GetSingleMessageCard(description, context);
            card.Actions.Add(new SubmitAction()
            {
                Title = "Answer Questions".ToUserLocale(context),
                DataJson = JObject.FromObject(new { fromcard = "FirstWelcome", nextcard = "AnswerQuestions" }).ToString()
            });

            card.Actions.Add(new SubmitAction()
            {
                Title = "Join Us".ToUserLocale(context),
                DataJson = JObject.FromObject(new { nextcard = "JoinUs" }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment AnswerQuestions(IDialogContext context)
        {
            string description = "I can answer questions typed into the IM window or click on one of the featured options below.";

            var card = Common.GetSingleMessageCard(description, context);
            card.Actions.Add(new SubmitAction()
            {
                Title = "Open positions".ToUserLocale(context),
                DataJson = JObject.FromObject(new { fromcard = "AnswerQuestions", nextcard = "Qna", text = "OpenPositions" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Applying".ToUserLocale(context),
                DataJson = JObject.FromObject(new { fromcard = "AnswerQuestions", nextcard = "Qna", text = "Applying" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Interviews".ToUserLocale(context),
                DataJson = JObject.FromObject(new { fromcard = "AnswerQuestions", nextcard = "ApplicationGUAUS" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Locations".ToUserLocale(context),
                DataJson = JObject.FromObject(new { fromcard = "AnswerQuestions", nextcard = "Qna", text = "OfficeLocations" }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment JoinUs(IDialogContext context)
        {
            var card = new AdaptiveCard()
            {
                Body = new List<CardElement>()
                {
                    new TextBlock() {
                        Text = "Please Share your detail for contact:".ToUserLocale(context),
                        Size = TextSize.Large,
                        Weight = TextWeight.Bolder
                    },
                    new TextBlock() { Text = "First Name".ToUserLocale(context) },
                    new TextInput()
                    {
                        Id = "First Name",
                        Placeholder = "Please enter your first name".ToUserLocale(context),
                        Style = TextInputStyle.Text
                    },
                    new TextBlock() { Text = "Last Name".ToUserLocale(context) },
                    new TextInput()
                    {
                        Id = "Last Name",
                        Placeholder = "Please enter your last name".ToUserLocale(context),
                        Style = TextInputStyle.Text
                    },
                    new TextBlock() { Text = "Email".ToUserLocale(context) },
                    new TextInput()
                    {
                        Id = "Email",
                        Placeholder = "Please enter your email".ToUserLocale(context),
                        Style = TextInputStyle.Text
                    },
                    new TextBlock() { Text = "Phone".ToUserLocale(context) },
                    new TextInput()
                    {
                        Id = "Phone",
                        Placeholder = "Please enter your Phone".ToUserLocale(context),
                        Style = TextInputStyle.Text
                    },
                    new TextBlock(){ Text = "From where have you heard about us?".ToUserLocale(context) },
                    new ChoiceSet()
                    {
                        Choices = new List<Choice>()
                        {
                            new Choice()
                            {
                                Title = "Web Search".ToUserLocale(context),
                                Value = "Web",
                                IsSelected = true
                            },
                            new Choice()
                            {
                                Title = "Social Media".ToUserLocale(context),
                                Value = "Social Media"
                            },
                            new Choice()
                            {
                                Title = "Recruiting Event".ToUserLocale(context),
                                Value = "Recruiting Event"
                            }
                        },
                        Style = ChoiceInputStyle.Compact,
                        Id = "JoinUsSourceInfo"
                    }
                },
                Actions = new List<ActionBase>()
                {
                    new SubmitAction()
                    {
                        Title = "Submit".ToUserLocale(context),
                        DataJson = JObject.FromObject(new { action = "saveInDb", nextcard = "ThankAfterJoinUs", fromcard = "JoinUs"}).ToString()
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

        public static Attachment ReHireEmployeeDetails(string message, IDialogContext context)
        {
            var card = Common.GetSingleMessageCard(message, context);

            card.Body.Add(
                    new TextBlock()
                    {
                        Text = "First Name".ToUserLocale(context)
                    });
            card.Body.Add(
                    new TextInput()
                    {
                        Id = "First Name",
                        Placeholder = "Please enter your first name".ToUserLocale(context),
                        Style = TextInputStyle.Text
                    });
            card.Body.Add(
            new TextBlock()
            {
                Text = "Last Name".ToUserLocale(context)
            });
            card.Body.Add(
                    new TextInput()
                    {
                        Id = "Last Name",
                        Placeholder = "Please enter your last name".ToUserLocale(context),
                        Style = TextInputStyle.Text
                    });
            card.Body.Add(new TextBlock() { Text = "Email".ToUserLocale(context) });
            card.Body.Add(new TextInput()
            {
                Id = "Email",
                Placeholder = "Please enter your email".ToUserLocale(context),
                Style = TextInputStyle.Text
            });
            card.Body.Add(new TextBlock() { Text = "Phone".ToUserLocale(context) });
            card.Body.Add(new TextInput()
            {
                Id = "Phone",
                Placeholder = "Please enter your Phone".ToUserLocale(context),
                Style = TextInputStyle.Text
            });
            card.Body.Add(new TextBlock() { Text = "Resignation Date".ToUserLocale(context) });
            card.Body.Add(new DateInput()
            {
                Id = "ResignationDate",
                Placeholder = "dd-MM-yyyy"
            });
            card.Body.Add(new TextBlock() { Text = "Account".ToUserLocale(context) });
            card.Body.Add(new TextInput()
            {
                Id = "Account",
                Placeholder = "Please enter your account".ToUserLocale(context),
                Style = TextInputStyle.Text
            });

            card.Actions.Add(new SubmitAction()
            {
                Title = "Submit".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "saveInDb",
                    nextcard = "ThanksAfterRehireEmpDtls",
                    fromcard = "ReHireEmployeeDetails"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ThanksAfterRehireEmpDtls(IDialogContext context)
        {
            string description = "Thank you, we will have our HR team confirm your re-hire eligiblity within the next 48 hours. Kindly keep your lines open.";

            var card = Common.GetSingleMessageCard(description, context);

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ThankAfterJoinUs(IDialogContext context)
        {
            string description = "Thank you for sharing the information. We will get in touch with you shortly.";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Restart".ToUserLocale(context),
                DataJson = JObject.FromObject(new { nextcard = "Qna", text = "Restart" }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment Thankyou(IDialogContext context)
        {
            string description = "Thank you for sharing the information.\n You can always start over by typing \"Restart\" or \"Reset\"";

            var card = Common.GetSingleMessageCard(description, context);

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment NormalMessage(string message, IDialogContext context)
        {
            var card = Common.GetSingleMessageCard(message, context);

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment GeneralHelpDepartments(string message, IDialogContext context)
        {
            var card = Common.GetSingleMessageCard(message, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Human Resources",
                Speak = "Human Resources",
                DataJson = JObject.FromObject(new { nextcard = "Qna", text = "Hello" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Payroll",
                Speak = "Payroll",
                DataJson = JObject.FromObject(new { nextcard = "Qna", text = "Hello" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Talent Acquisition",
                Speak = "Talent Acquisition",
                DataJson = JObject.FromObject(new { nextcard = "Qna", text = "Hello" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Workforce",
                Speak = "Workforce",
                DataJson = JObject.FromObject(new { nextcard = "Qna", text = "Hello" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Others",
                Speak = "Others",
                DataJson = JObject.FromObject(new { nextcard = "Qna", text = "Hello" }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment OfficeLocations(string message, IDialogContext context)
        {
            var card = Common.GetSingleMessageCard(message, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Taguig City, Philippines",
                Speak = "Taguig City, Philippines",
                DataJson = JObject.FromObject(new { nextcard = "Qna", text = "Location of Philippines" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Guatemala City, Guatemala",
                Speak = "Guatemala City, Guatemala",
                DataJson = JObject.FromObject(new { nextcard = "GeneralLocations" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Salt Lake City, Utah USA",
                Speak = "Salt Lake City, Utah USA",
                DataJson = JObject.FromObject(new { nextcard = "GeneralLocations" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Miramar, Florida USA",
                Speak = "Miramar, Florida USA",
                DataJson = JObject.FromObject(new { nextcard = "GeneralLocations" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Mcgregor, Texas USA",
                Speak = "Mcgregor, Texas USA",
                DataJson = JObject.FromObject(new { nextcard = "GeneralLocations" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Tucson East, ArizonaUSA",
                Speak = "Tucson East, ArizonaUSA",
                DataJson = JObject.FromObject(new { nextcard = "GeneralLocations" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Tucson South, Arizona USA",
                Speak = "Tucson South, Arizona USA",
                DataJson = JObject.FromObject(new { nextcard = "GeneralLocations" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Tucson North, Arizona USA",
                Speak = "Tucson North, Arizona USA",
                DataJson = JObject.FromObject(new { nextcard = "GeneralLocations" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Tahlequah, Oklahoma USA",
                Speak = "Tahlequah, Oklahoma USA",
                DataJson = JObject.FromObject(new { nextcard = "GeneralLocations" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Las Vegas, Nevada USA",
                Speak = "Las Vegas, Nevada USA",
                DataJson = JObject.FromObject(new { nextcard = "GeneralLocations" }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "Twin Falls, Idaho USA",
                Speak = "Twin Falls, Idaho USA",
                DataJson = JObject.FromObject(new { nextcard = "GeneralLocations" }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment InterviewLocations(string message, IDialogContext context)
        {
            var card = Common.GetSingleMessageCard(message, context);

            card.Body.Add(new ChoiceSet()
            {
                Choices = new List<Choice>()
                        {
                            new Choice()
                            {
                                Title = "Taguig City, Philippines",
                                Value = "ApplicationTagPH"
                            },
                            new Choice()
                            {
                                Title = "Guatemala City, Guatemala",
                                Value = "ApplicationGUAUS"
                            },
                            new Choice()
                            {
                                Title = "Salt Lake City, Utah USA",
                                Value = "ApplicationGUAUS"
                            },
                            new Choice()
                            {
                                Title = "Miramar, Florida USA",
                                Value = "ApplicationGUAUS"
                            },
                            new Choice()
                            {
                                Title = "Mcgregor, Texas USA",
                                Value = "ApplicationGUAUS"
                            },
                            new Choice()
                            {
                                Title = "Tucson East, ArizonaUSA",
                                Value = "ApplicationGUAUS"
                            },
                            new Choice()
                            {
                                Title = "Tucson South, Arizona USA",
                                Value = "ApplicationGUAUS"
                            },
                            new Choice()
                            {
                                Title = "Tucson North, Arizona USA",
                                Value = "ApplicationGUAUS"
                            },
                            new Choice()
                            {
                                Title = "Tahlequah, Oklahoma USA",
                                Value = "ApplicationGUAUS"
                            },
                            new Choice()
                            {
                                Title = "Las Vegas, Nevada USA",
                                Value = "ApplicationGUAUS"
                            },
                            new Choice()
                            {
                                Title = "Twin Falls, Idaho USA",
                                Value = "ApplicationGUAUS"
                            }
                        },
                Style = ChoiceInputStyle.Compact,
                Id = "InterviewLocationChoice"
            });

            card.Actions.Add(new SubmitAction()
            {
                Title = "Ok".ToUserLocale(context),
                DataJson = JObject.FromObject(new { nextcard = "Options" }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ApplicationTagPH(IDialogContext context)
        {
            string description = "I would be happy to schedule you for an interview with one of our Recruiters, do you wish to proceed?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Yes".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ApplicationScreeningAge",
                    fromcard = "ApplicationTagPH",
                    applicationtagph = "yes"
                }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "No".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    fromcard = "ApplicationTagPH",
                    nextcard = "DeclineInterviewProcess",
                    applicationtagph = "no"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ApplicationGUAUS(IDialogContext context)
        {
            string description = "Our Recruiters would be glad to contact you, please fill your details and we will reach out to you within the next 24-48 hours.";
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
                                        Text = description.ToUserLocale(context),
                                        Size = TextSize.Normal,
                                        Wrap = true
                                    }
                                }
                            }
                        }
                    },
                    new ColumnSet()
                    {
                        Columns = new List<Column>()
                        {
                            new Column()
                            {
                                Items = new List<CardElement>()
                                {
                                    new TextBlock()
                                    {
                                        Text = "First Name".ToUserLocale(context)
                                    },
                                    new TextInput()
                                    {
                                        Id = "First Name",
                                        Placeholder = "Please enter your first name".ToUserLocale(context),
                                        IsRequired = true
                                    },
                                    new TextBlock()
                                    {
                                        Text = "Last Name".ToUserLocale(context)
                                    },
                                    new TextInput()
                                    {
                                        Id = "Last Name",
                                        Placeholder = "Please enter your last name".ToUserLocale(context),
                                        IsRequired = true
                                    },
                                    new TextBlock()
                                    {
                                        Text = "Phone".ToUserLocale(context)
                                    },
                                    new TextInput()
                                    {
                                        Id = "Phone",
                                        Placeholder = "Please enter your phone".ToUserLocale(context),
                                        IsRequired = true
                                    }
                                }
                            }
                        }
                    }
                },
                Actions = new List<ActionBase>()
                {
                    new SubmitAction()
                    {
                        Title = "Submit".ToUserLocale(context),
                        DataJson = JObject.FromObject(new { action = "saveInDb", nextcard = "ThankAfterApplicationInfo", fromcard = "ApplicationGUAUS" }).ToString()
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

        public static Attachment ApplicationScreeningAge(IDialogContext context)
        {
            string description = "Thank you. Are you 18 years old or above?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Yes".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ApplicationScreeningEdu",
                    fromcard = "ApplicationScreeningAge",
                    applicationscreeningage = "yes"
                }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "No".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "FailedAgeScreeningTest",
                    fromcard = "ApplicationScreeningAge",
                    applicationscreeningage = "no"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ApplicationScreeningEdu(IDialogContext context)
        {
            string description = "Are you a Highschool or Vocational or College Graduate?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Yes".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ApplicationScreeningBpoExp",
                    fromcard = "ApplicationScreeningEdu",
                    applicationscreeningedu = "yes"
                }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "No".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "FailedEduScreeningTest",
                    fromcard = "ApplicationScreeningEdu",
                    applicationscreeningedu = "no"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ApplicationScreeningBpoExp(IDialogContext context)
        {
            string description = "Do you have a Call Center experience?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Yes".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ApplicationScreeningShiftSch",
                    fromcard = "ApplicationScreeningBpoExp",
                    applicationscreeningbpoexp = "yes"
                }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "No".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ApplicationScreeningShiftSch",
                    fromcard = "ApplicationScreeningBpoExp",
                    applicationscreeningbpoexp = "no"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ApplicationScreeningShiftSch(IDialogContext context)
        {
            string description = "Are you flexible to work in night shifts or shifting schedules?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Yes".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ApplicationScreeningNightShift",
                    fromcard = "ApplicationScreeningShiftSch",
                    applicationscreeningshiftsch = "yes"
                }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "No".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "FailedShiftSchScreeningTest",
                    fromcard = "ApplicationScreeningShiftSch",
                    applicationscreeningshiftsch = "no"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ApplicationScreeningNightShift(IDialogContext context)
        {
            string description = "Most of our job vacancies requires a night shift and shifting schedules. Do you wish to proceed?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Yes".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ApplicationScreeningHolWeekend",
                    fromcard = "ApplicationScreeningNightShift",
                    applicationscreeningnightshift = "yes"
                }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "No".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "FailedShiftSchScreeningTest",
                    fromcard = "ApplicationScreeningNightShift",
                    applicationscreeningnightshift = "no"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ApplicationScreeningHolWeekend(IDialogContext context)
        {
            string description = "Are you amenable to working on holidays or weekends?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Yes".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ApplicationScreeningLoc",
                    fromcard = "ApplicationScreeningHolWeekend",
                    applicationscreeningholweekend = "yes"
                }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "No".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "FailedHolWeekendScreeningTest",
                    fromcard = "ApplicationScreeningHolWeekend",
                    applicationscreeningholweekend = "no"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ApplicationScreeningLoc(IDialogContext context)
        {
            string description = "Are you amenable to work in Bonifacio Global City, Taguig?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Yes".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ApplicationScreeningAmenability",
                    fromcard = "ApplicationScreeningLoc",
                    applicationscreeningloc = "yes"
                }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "No".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "FailedLocScreeningTest",
                    fromcard = "ApplicationScreeningLoc",
                    applicationscreeningloc = "no"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ApplicationScreeningAmenability(IDialogContext context)
        {
            string description = "Will you be able to start immediately?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Yes".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ThankAndGetPersonalInfoForInterview",
                    fromcard = "ApplicationScreeningAmenability",
                    applicationscreeningamenability = "yes"
                }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "No".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "AskAvailableStartDate",
                    fromcard = "ApplicationScreeningAmenability",
                    applicationscreeningamenability = "no"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ThankAndGetPersonalInfoForInterview(IDialogContext context)
        {
            string description = "Thank you for your confirming these information. Please provide below details to schedule your interview.";

            var card = Common.GetSingleMessageCard(description, context);

            card.Body.Add(
                    new TextBlock()
                    {
                        Text = "First Name".ToUserLocale(context)
                    });
            card.Body.Add(
                    new TextInput()
                    {
                        Id = "First Name",
                        Placeholder = "Please enter your first name".ToUserLocale(context),
                        Style = TextInputStyle.Text
                    });
            card.Body.Add(
            new TextBlock()
            {
                Text = "Last Name".ToUserLocale(context)
            });
            card.Body.Add(
                    new TextInput()
                    {
                        Id = "Last Name",
                        Placeholder = "Please enter your last name".ToUserLocale(context),
                        Style = TextInputStyle.Text
                    });
            card.Body.Add(new TextBlock() { Text = "Email".ToUserLocale(context) });
            card.Body.Add(new TextInput()
            {
                Id = "Email",
                Placeholder = "Please enter your email".ToUserLocale(context),
                Style = TextInputStyle.Text
            });
            card.Body.Add(new TextBlock() { Text = "Phone".ToUserLocale(context) });
            card.Body.Add(new TextInput()
            {
                Id = "Phone",
                Placeholder = "Please enter your Phone".ToUserLocale(context),
                Style = TextInputStyle.Text
            });
            card.Body.Add(new TextBlock() { Text = "From where have you heard about us?".ToUserLocale(context) });
            card.Body.Add(new ChoiceSet()
            {
                Choices = new List<Choice>()
                        {
                            new Choice()
                            {
                                Title = "Web Search".ToUserLocale(context),
                                Value = "Web",
                                IsSelected = true
                            },
                            new Choice()
                            {
                                Title = "Social Media".ToUserLocale(context),
                                Value = "Social Media"
                            },
                            new Choice()
                            {
                                Title = "Recruiting Event".ToUserLocale(context),
                                Value = "Recruiting Event"
                            }
                        },
                Style = ChoiceInputStyle.Compact,
                Id = "JoinUsSourceInfo"
            });

            card.Actions.Add(new SubmitAction()
            {
                Title = "Ok".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "saveInDb",
                    nextcard = "ShowInterviewDate",
                    fromcard = "ThankAndGetPersonalInfoForInterview"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment AskAvailableStartDate(IDialogContext context)
        {
            string description = "When will you be available to start?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Body.Add(new DateInput()
            {
                Id = "AvailableStartDate",
                Placeholder = "yyyy-MM-dd",
                Min = System.DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")
            });

            card.Actions.Add(new SubmitAction()
            {
                Title = "Confirm".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ThankAndGetPersonalInfoForInterview",
                    fromcard = "AskAvailableStartDate"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ShowInterviewDate(string interviewDate, IDialogContext context)
        {
            string description = "Thank you for your confirming these information. Your interview schedule is on " + interviewDate + " at 10am. " +
                "Our office is located at at 11th Floor, Bonifacio One Technology Tower, along Rizal Drive, Bonifacio Global City. Kindly bring an updated resume and valid ID.";

            var card = Common.GetSingleMessageCard(description, context);

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment FailedLocScreeningTest(IDialogContext context)
        {
            string description = "Our main office is located in Bonifacio Global City, Taguig. Do you wish to proceed?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Yes".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ApplicationScreeningAmenability",
                    fromcard = "FailedLocScreeningTest",
                    failedlocscreeningtest = "yes"
                }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "No".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "FailedShiftSchScreeningTest",
                    fromcard = "FailedLocScreeningTest",
                    failedlocscreeningtest = "no"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment FailedHolWeekendScreeningTest(IDialogContext context)
        {
            string description = "Most of our job vacancies require you to work on holidays and weekends. Do you wish to proceed?";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Yes".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "ApplicationScreeningLoc",
                    fromcard = "FailedHolWeekendScreeningTest",
                    failedholweekendscreeningtest = "yes"
                }).ToString()
            });
            card.Actions.Add(new SubmitAction()
            {
                Title = "No".ToUserLocale(context),
                DataJson = JObject.FromObject(new
                {
                    action = "save",
                    nextcard = "FailedShiftSchScreeningTest",
                    fromcard = "FailedHolWeekendScreeningTest",
                    failedholweekendscreeningtest = "no"
                }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment FailedShiftSchScreeningTest(IDialogContext context)
        {
            string description = "Thank you for your time. We hope to hear from you again soon.";

            var card = Common.GetSingleMessageCard(description, context);

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment FailedEduScreeningTest(IDialogContext context)
        {
            string description = "You must have a 4-year Highschool/2-years Vocational/4-year College Diploma to qualify for a job position.";
            var card = Common.GetSingleMessageCard(description, context);

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment FailedAgeScreeningTest(IDialogContext context)
        {
            string description = "You must be at least 18 years old to qualify for a position. Kindly re-apply on a later time.";

            var card = Common.GetSingleMessageCard(description, context);

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment DeclinePHInterviewProcess(IDialogContext context)
        {
            string description = "If you wish to apply, you may visit us at sample address, Bonifacio Global City.";

            var card = Common.GetSingleMessageCard(description, context);

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment ThankAfterApplicationInfo(IDialogContext context)
        {
            string description = "Thank you for sharing the information. We will reach out to you within the next 24-48 hours.";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Restart".ToUserLocale(context),
                DataJson = JObject.FromObject(new { nextcard = "Qna", text = "Restart" }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment DeclineInterviewProcess(IDialogContext context)
        {
            string description = "If you wish to apply , you may visit us at sampleaddress.";

            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new SubmitAction()
            {
                Title = "Restart".ToUserLocale(context),
                DataJson = JObject.FromObject(new { nextcard = "Qna", text = "Restart" }).ToString()
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment GeneralLocations(IDialogContext context)
        {
            string description = "You can visit our website to know about global locations";
            var card = Common.GetSingleMessageCard(description, context);

            card.Actions.Add(new OpenUrlAction()
            {
                Title = "Global Locations".ToUserLocale(context),
                Url = "http://www.google.com"
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public static Attachment Careers(string message, IDialogContext context)
        {
            var card = Common.GetSingleMessageCard(message, context);

            card.Actions.Add(new OpenUrlAction()
            {
                Title = "Careers".ToUserLocale(context),
                Url = "https://www.google.com/en-us/"
            });

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }
    }
}