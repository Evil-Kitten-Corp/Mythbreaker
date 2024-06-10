using UnityEngine;

namespace FinalScripts
{
    public class TriggerCutscene: MonoBehaviour
    {
        public GameObject boss;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AttributesManager.OnBossMeet?.Invoke();
                GetComponent<AudioSource>().Play();
                boss.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}