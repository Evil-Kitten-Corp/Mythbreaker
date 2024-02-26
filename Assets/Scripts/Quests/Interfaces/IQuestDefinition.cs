using System.Collections.Generic;

namespace Quests.Interfaces
{
    public interface IQuestDefinition 
    {
        string Id { get; }
        string Title { get; }
        string Description { get; }
        IReadOnlyList<ITaskDefinition> Tasks { get; }
    }
}