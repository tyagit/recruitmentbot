﻿using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using System.Threading;
using System.Threading.Tasks;

namespace RecruitmentQnA
{
    public class UserToAgentScorable : ScorableBase<IActivity, bool, double>
    {
        private readonly IUserToAgent _userToAgent;

        public UserToAgentScorable(IUserToAgent userToAgent)
        {
            _userToAgent = userToAgent;
        }
        protected override Task DoneAsync(IActivity item, bool state, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected override double GetScore(IActivity item, bool state)
        {
            return state ? 1.0 : 0;
        }

        protected override bool HasScore(IActivity item, bool state)
        {
            return state;
        }

        protected override async Task PostAsync(IActivity item, bool state, CancellationToken token)
        {
            await _userToAgent.SendToAgentAsync(item as Activity, token);
        }

        protected override async Task<bool> PrepareAsync(IActivity item, CancellationToken token)
        {
            return await _userToAgent.AgentTransferRequiredAsync(item as Activity, token);
        }
    }
}
