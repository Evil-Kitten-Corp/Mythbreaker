using System.Collections;
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
            FindObjectOfType<AttributesManager>().OnDie += () => StartCoroutine(OnDeath());
        }

        private IEnumerator OnDeath()
        {
            yield return new WaitForSeconds(6f);
            deathScreen.SetActive(true);
            Time.timeScale = 0;
            IsInUI = true;
        }

        public void Restart()
        {
            deathScreen.SetActive(false);
            Time.timeScale = 1;
            IsInUI = false;
            SceneManager.LoadScene("Combat Test");
        }

        public void ReturnToMain()
        {
            IsInUI = false;
            SceneManager.LoadScene("MainMenu");
        }
    }
}