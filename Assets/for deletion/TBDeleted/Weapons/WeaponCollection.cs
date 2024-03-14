using BrunoMikoski.ScriptableObjectCollections;
using UnityEngine;
using System.Collections.Generic;

namespace Collections.Resources.Weapons
{
    [CreateAssetMenu(menuName = "ScriptableObject Collection/Collections/Create WeaponCollection", fileName = "WeaponCollection", order = 0)]
    public class WeaponCollection : ScriptableObjectCollection<Weapon>
    {
    }
}
