namespace Characters.Combat.Info
{
    public class HurtInfoBase
    {
        public HitInfoBase HitInfo;

        public HurtInfoBase()
        {

        }

        public HurtInfoBase(HitInfoBase hitInfo)
        {
            this.HitInfo = hitInfo;
        }
    }
}