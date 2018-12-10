namespace RecruitmentQnA
{
    public interface IAgentProvider
    {
        Agent GetNextAvailableAgent();
        bool AddAgent(Agent agent);
        Agent RemoveAgent(Agent agent);
    }
}
