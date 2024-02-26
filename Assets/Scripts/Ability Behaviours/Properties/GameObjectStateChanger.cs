using System;
using System.Collections;
using UnityEngine;

namespace Ability_Behaviours.Properties
{
    [Serializable]
    public class GameObjectStateChanger: IAbilityBehaviour
    {
        public GameObject gameObject;

        public IEnumerator Apply()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            yield break;
        }

        public void Unapply()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}