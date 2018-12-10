using Microsoft.Bot.Connector;
using System.Threading;
using System.Threading.Tasks;

namespace RecruitmentQnA
{
    public interface IUserToAgent
    {
        Task<bool> AgentTransferRequiredAsync(Activity message, CancellationToken cancellationToken);

        Task SendToAgentAsync(Activity message, CancellationToken cancellationToken);

        Task<Agent> IntitiateConversationWithAgentAsync(Activity message, CancellationToken cancellationToken);
    }
}
