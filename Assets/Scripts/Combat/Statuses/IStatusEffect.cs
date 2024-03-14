namespace Combat.Statuses
{
    public interface IStatusEffect
    {
        public Status status { get; }
        
        public void Update();
    }

    public enum Status
    {
        Ignite,
        Stun
    }
}