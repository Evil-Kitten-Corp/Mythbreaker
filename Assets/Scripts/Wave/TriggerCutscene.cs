using UnityEngine;

namespace FinalScripts
{
    public class TriggerCutscene: MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AttributesManager.OnBossMeet?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}