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
            FindObjectOfType<AttributesManager>().health.Amount += 30;
            Destroy(gameObject);
        }
    }
}