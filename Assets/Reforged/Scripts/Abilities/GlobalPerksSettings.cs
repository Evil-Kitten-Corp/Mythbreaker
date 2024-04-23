using System;
using System.Collections.Generic;
using System.Linq;
using Minimalist.Bar.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Abilities
{
    [RequireComponent(typeof(Player))]
    public class GlobalPerksSettings : Singleton<GlobalPerksSettings>
    {
        public List<Perk> allPerks;
        public List<AbilityData> discoveredPerks;

        public AbilityData GetAbilityByGod(string god)
        {
            return allPerks.First(x => x.ability.god == god).ability;
        }

        public Perk GetRandomAbility()
        {
            List<Perk> unowned = allPerks.FindAll(x => x.owned == false);
            int i = Random.Range(0, unowned.Count);

            return unowned[i];
        }

        public void SetAbility(bool owned, AbilityData ability)
        {
            allPerks.First(x => x.ability == ability).owned = owned;
        }
    }

    [Serializable]
    public class Perk
    {
        public AbilityData ability;
        public MonoBehaviour associated;
        public bool owned;
    }
}