using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class DeathScreen : MythUIElement
    {
        public GameObject deathScreen;

        private void Start()
        {
            deathScreen.SetActive(false);
            AttributesManager.OnDie += OnDeath;
        }

        private void OnDeath()
        {
            deathScreen.SetActive(true);
            Time.timeScale = 0;
            IsInUI = true;
        }

        public void Restart()
        {
            deathScreen.SetActive(false);
            Time.timeScale = 1;
            IsInUI = false;
            SceneManager.LoadScene("Combat");
        }

        public void ReturnToMain()
        {
            IsInUI = false;
            SceneManager.LoadScene("MainMenu");
        }
    }
}