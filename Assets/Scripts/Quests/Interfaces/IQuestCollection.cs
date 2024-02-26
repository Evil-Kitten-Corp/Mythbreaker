using System.Collections.Generic;

namespace Quests.Interfaces
{
    public interface IQuestCollection 
    {
        IQuestInstance Add (IQuestDefinition quest);
        IQuestInstance Add (ITaskDefinition task);

        IQuestInstance Get (IQuestDefinition definition);
        IQuestInstance Get (ITaskDefinition definition);

        string Save();
        void Load(string save);
        List<IQuestInstance> GetAll();
    }
}