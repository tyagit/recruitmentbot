using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace RecruitmentQnA.Dialogs
{
    /// <summary>
    /// test dialog used to capture phone no and validate it while giving maximum 3 attempts to user to provide valid phone no
    /// </summary>
    [Serializable]
    public class PhoneNoDialog : IDialog<string>
    {
        private int attempts = 3;

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Please enter your phone no");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            
            if ((message.Text != null) && (message.Text.Trim().Length > 9))
            {
                context.Done(message.Text);
            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I'm sorry, this seems to be an invalid no. What is your phone no?");

                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));
                }
            }
        }
    }
}