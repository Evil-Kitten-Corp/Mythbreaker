using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public GameObject controlsPanel;
    public GameObject creditsPanel;
    public Image fadePanel;
    public float fadeDuration = 1f;

    bool hasFaded = false;

    void Start()
    {
        // Start the fade-in effect when the game starts
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        if (!hasFaded)
        {
            hasFaded = true;

            // Start with fully opaque
            fadePanel.color = new Color(0f, 0f, 0f, 1f);

            // Fade out
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                float t = elapsedTime / fadeDuration;
                fadePanel.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, t)); // Fade out from 100% to 0%
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            fadePanel.color = new Color(0f, 0f, 0f, 0f); // Ensure the panel is fully transparent
            fadePanel.gameObject.SetActive(false); // Disable the fade panel
        }
    }

    public void StartGame()
    {
        StartCoroutine(FadeAndLoadScene("Combat"));
    }

    public void ShowControls()
    {
        StartCoroutine(FadeAndSetActive(controlsPanel, true));
    }

    public void HideControls()
    {
        StartCoroutine(FadeAndSetActive(controlsPanel, false));
    }

    public void ShowCredits()
    {
        StartCoroutine(FadeAndSetActive(creditsPanel, true));
    }

    public void HideCredits()
    {
        StartCoroutine(FadeAndSetActive(creditsPanel, false));
    }

    public void QuitGame()
    {
        StartCoroutine(FadeAndQuit());
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return FadeOut();
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeAndSetActive(GameObject panel, bool setActive)
    {
        yield return FadeOut();
        panel.SetActive(setActive);
        yield return FadeIn();
    }

    IEnumerator FadeAndQuit()
    {
        yield return FadeOut();
        Application.Quit();
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            fadePanel.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 1f, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadePanel.color = new Color(0f, 0f, 0f, 1f);
    }

    IEnumerator FadeIn()
    {
        fadePanel.gameObject.SetActive(true); // Enable the fade panel
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            fadePanel.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadePanel.color = new Color(0f, 0f, 0f, 0f);
    }
}
