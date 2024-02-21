using BrunoMikoski.ScriptableObjectCollections;
using UnityEngine;
using System.Collections.Generic;

namespace Collections.Resources.Abilities
{
    [CreateAssetMenu(menuName = "ScriptableObject Collection/Collections/Create AbilityCollection", fileName = "AbilityCollection", order = 0)]
    public class AbilityCollection : ScriptableObjectCollection<Ability>
    {
    }
}
