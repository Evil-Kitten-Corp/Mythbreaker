namespace Quests.Interfaces
{
    public interface ITaskDefinition 
    {
        string Id { get; }
        string Title { get; }
        string Description { get; }
        IQuestDefinition Parent { get; }
    }
}