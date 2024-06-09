using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class DeathScreen : MythUIElement
    {
        public GameObject deathScreen;
        public AttributesManager attributes;

        private void Start()
        {
            deathScreen.SetActive(false);
            attributes.OnDie += OnDeath;
        }

        private void OnDeath()
        {
            deathScreen.SetActive(true);
            IsInUI = true;
        }

        public void Restart()
        {
            deathScreen.SetActive(false);
            IsInUI = false;
        }

        public void ReturnToMain()
        {
            IsInUI = false;
            SceneManager.LoadScene("MainMenu");
        }
    }
}