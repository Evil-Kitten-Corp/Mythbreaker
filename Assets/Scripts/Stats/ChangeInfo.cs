namespace Stats
{
    public struct ChangeInfo
    {
        public float Delta;
        public float Current;
        public bool IsChanged => Delta != 0;
    }
}