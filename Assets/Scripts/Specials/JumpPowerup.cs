namespace FinalScripts.Specials
{
    public class JumpPowerup : Powerup
    {
        public int addedJumpForce;
        public float timeout = 10f;
        
        private void Start()
        {
            OnPickup += JumpHigher;
        }

        private void JumpHigher()
        {
            GetComponent<AttributesManager>().AddTemporaryBuff(addedJumpForce, timeout);
        }
    }
}