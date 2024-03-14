using Base;
using BrunoMikoski.ServicesLocation;
using ExtEvents;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "New Ability Info", fileName = "AbilityInfo", order = 0)]
    public class AbilityInfo : ScriptableObject
    {
        public Sprite icon;
        public string abilityName;
        public float cooldown;
        [TextArea] public string description;
        [SerializeReference] public IAbilityWrapper abilityBehaviour;

        [HideInInspector] public ExtEvent<AbilityInfo> onAbilityEquipped = new();
        [HideInInspector] public ExtEvent<AbilityInfo> onAbilityUnequipped = new();

        public AbilityBase Add()
        {
            PlayerCharacter pc = ServiceLocator.Instance.GetInstance<PlayerCharacter>();
            var component = abilityBehaviour.ToAbilityBehaviour(pc.abilityHolder);
            component.AbilityInfo = this;
            return component as AbilityBase;
        }

        public void Equip()
        {
            onAbilityEquipped.Invoke(this);
        }

        public void Unequip()
        {
            onAbilityUnequipped.Invoke(this);
        }
    }
}