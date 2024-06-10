namespace FinalScripts.Specials
{
    public class HealthPowerup : Powerup
    {
        private void Start()
        {
            OnPickup += Heal;
        }

        private void Heal()
        {
            GetComponent<AttributesManager>().health.Amount = 1000;
        }
    }
}