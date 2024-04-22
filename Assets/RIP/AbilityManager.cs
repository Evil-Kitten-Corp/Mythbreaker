using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Base;
using BrunoMikoski.ServicesLocation;
using JetBrains.Annotations;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public TAbilityPreviewerAOE abilityPreviewerAoe;
    public TAbilityPreviewerSingle abilityPreviewerSingle;
    
    [SerializedDictionary("Ability Slot", "Key")]
    public SerializedDictionary<AbilitySlot, KeyCode> keycodeMapper;
    
    [SerializedDictionary("Ability", "Owns?")]
    public SerializedDictionary<TAbility, bool> abilityMapper;

    private  Dictionary<AbilitySlot, TAbility> _slotMapper = new();

    private void Awake()
    {
        ServiceLocator.Instance.RegisterInstance(this);
    }

    private void Start()
    {
        foreach (var kvp in abilityMapper)
        {
            kvp.Key.isActive = kvp.Value;
        }
    }

    private void Update()
    {
        foreach (var ab 
                 in from kvp 
                 in keycodeMapper.TakeWhile(kvp => 
                     !(GetAbilityInSlot(kvp.Key) == null | !GetAbilityInSlot(kvp.Key).CanUse)) 
                 where Input.GetKeyDown(kvp.Value) 
                 select GetAbilityInSlot(kvp.Key))
        {
            if (ab != null && ab.casting is AbilityCastBehaviour.PreviewerAOE)
            {
                abilityPreviewerAoe.AddDelegate(ab, AbilitySender);
                abilityPreviewerAoe.Activate();
            }
            else if (ab != null && ab.casting is AbilityCastBehaviour.PreviewerSingle)
            {
                abilityPreviewerSingle.AddDelegate(ab, AbilitySender);
                abilityPreviewerSingle.Activate();
            }
            else if (ab != null && ab.casting is AbilityCastBehaviour.Routine)
            {
                StartCoroutine(ab.Use(true));
            }
            else if (ab != null)
            {
                ab.Use();
            }
        }
    }

    private void AbilitySender(TAbility a, TEnemy enemy)
    {
        a.Use(enemy);
    }

    private void AbilitySender(TAbility a, Vector3 vector)
    {
        a.Use(vector);
    }

    public void EquipAbility(TAbility ability, AbilitySlot slot)
    {
        if (_slotMapper.ContainsKey(slot))
        {
            _slotMapper[slot] = ability;
        }
        else
        {
            _slotMapper.TryAdd(slot, ability);
        }
    }

    public void UnequipAbility(AbilitySlot slot)
    {
        if (_slotMapper.ContainsKey(slot))
        {
            _slotMapper.Remove(slot);
        }
    }

    [CanBeNull]
    public TAbility GetAbilityInSlot(AbilitySlot slot)
    {
        if (_slotMapper.ContainsKey(slot) && _slotMapper[slot] != null)
        {
            return _slotMapper[slot];
        }
        
        return null;
    }

    public KeyCode GetAbilityKey(TAbility ab)
    {
        AbilitySlot? slot = null;
        
        if (_slotMapper.ContainsValue(ab))
        {
            slot = _slotMapper.First(x => x.Value == ab).Key;
        }

        if (slot != null)
        {
            return keycodeMapper[(AbilitySlot)slot];
        }
        
        throw new InvalidOperationException();
    }
}

public enum AbilityCastBehaviour
{
    Quick,
    PreviewerAOE,
    PreviewerSingle,
    Routine
}
