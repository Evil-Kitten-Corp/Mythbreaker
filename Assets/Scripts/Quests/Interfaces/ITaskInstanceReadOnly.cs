using Quests.Definitions.Tasks;

namespace Quests.Interfaces
{
    public interface ITaskInstanceReadOnly 
    {
        string Id { get; }
        string Title { get; }
        string Description { get; }
        ITaskDefinition Definition { get; }
        TaskStatus Status { get; }

        string Save ();
    }
}