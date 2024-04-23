using Minimalist.Bar.Quantity;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    public QuantityBhv health;
    public int attack;

    public Animator anim;

    [Header("Debug")] 
    public bool hungerDebuff;

    public void TakeDamage(int amount, bool crit = false)
    {
        if (hungerDebuff)
        {
            health.Amount -= amount * 2;
        }
        else
        {
            health.Amount -= amount;
        }
        
        Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
        
        switch (crit)
        {
            case false:
                DamagePopUpGenerator.current.CreatePopUp(transform.position + randomness, amount.ToString(), Color.yellow);
                break;
            case true:
                DamagePopUpGenerator.current.CreatePopUp(transform.position + randomness, amount.ToString(), Color.cyan);
                break;
        }

        if (anim != null)
        {
            anim.SetTrigger("Hit");
        }
    }

    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<AttributesManager>();
        if(atm != null)
        {
            atm.TakeDamage(attack);
        }
    }
}