using Base;
using BrunoMikoski.ServicesLocation;
using UnityEngine;
using UnityEngine.EventSystems;

public class TSlot : MonoBehaviour, IDropHandler
{
    public AbilitySlot slot;
    public TAbility debugAbility;

    public virtual void Start()
    {
        ServiceLocator.Instance.GetInstance<AbilityManager>().EquipAbility(debugAbility, slot);
    }

    public void OnDrop(PointerEventData eventData)
    {
    }
}