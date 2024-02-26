namespace Quests.Interfaces
{
    public interface ITaskInstance : ITaskInstanceReadOnly 
    {
        void Begin ();
        void Complete ();
        void ClearStatus ();

        void Load (string save);
    }
}