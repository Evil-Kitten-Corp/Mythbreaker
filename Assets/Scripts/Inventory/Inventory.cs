using System.Collections.Generic;
using BrunoMikoski.ServicesLocation;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        public List<RarityChooser> upgrades;

        private void Awake()
        {
            ServiceLocator.Instance.RegisterInstance(this);
        }
    }
}