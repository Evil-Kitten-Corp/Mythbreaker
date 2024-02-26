using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ability_Behaviours.Properties
{
    [Serializable]
    public class TimedAbility: IAbilityBehaviour
    {
        public float maxTime;
        public Image image;

        private float _currentTime;
        
        public IEnumerator Apply()
        {
            image.fillAmount = 1;
            image.gameObject.SetActive(true);
            
            while (_currentTime > 0f)
            {
                float fillRatio = Mathf.Clamp01(_currentTime / maxTime);
                image.fillAmount = fillRatio;

                _currentTime -= Time.deltaTime;

                yield return null;
            }

            image.fillAmount = 0f;
        }

        public void Unapply()
        {
            image.gameObject.SetActive(false);
        }
    }
}