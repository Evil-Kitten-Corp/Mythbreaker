using BrunoMikoski.ScriptableObjectCollections;
using TypeReferences;
using UnityEngine;
using Weapon_Behaviours;

namespace Collections.Resources.Weapons
{
    public partial class Weapon : ScriptableObjectCollectionItem
    {
        public GameObject prefab;
        
        [Inherits(typeof(IWeaponBehaviour))]
        [SerializeField] private TypeReference weaponBehaviour;
    }
}
