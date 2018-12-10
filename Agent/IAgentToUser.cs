using Microsoft.Bot.Connector;
using System.Threading;
using System.Threading.Tasks;

namespace RecruitmentQnA
{
    public interface IAgentToUser
    {
        Task SendToUserAsync(Activity message, CancellationToken cancellationToken);
    }
}
