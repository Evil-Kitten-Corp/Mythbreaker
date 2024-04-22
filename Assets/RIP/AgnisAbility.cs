using System;
using LT.Shi;
using UnityEngine;

public class AgnisAbility : TAbility
{
    public TPlayer player;
    public int maxStacks;
    public GameObject abilityPrefab;
    internal int currentStacks;

    public event Action OnStackChange;

    private void Start()
    {
        player.OnPerformAttack += AddStack;
    }
    
    private void AddStack()
    {
        if (currentStacks < maxStacks)
        {
            currentStacks++;
            OnStackChange?.Invoke();
        }
    }

    public override void Use()
    {
        for (int i = 0; i < currentStacks; i++)
        {
            var obj = Instantiate(abilityPrefab, player.transform.position + Vector3.forward + Vector3.up, 
                Quaternion.identity);
            
            obj.GetComponent<Missile>().SetTarget(FindObjectOfType<Target>());
        }

        currentStacks = 0;
        OnStackChange?.Invoke();
        
        base.Use();
    }
}