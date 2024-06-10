using Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class AbilityRepresenter: MonoBehaviour
    {
        public Button button;
        public AbilityData ability;
        public Image @lock;

        private void Start()
        {
            button.image.sprite = ability.icon;

            if (!button.IsInteractable())
            {
                @lock.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            @lock.gameObject.SetActive(!button.IsInteractable());
        }
    }
}