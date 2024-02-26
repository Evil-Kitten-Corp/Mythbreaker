namespace Quests.Interfaces
{
    public interface IQuestDatabase 
    {
        void Setup ();
        IQuestDefinition GetQuest (string id);
    }
}