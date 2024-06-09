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

    private bool _hasFaded;
    private SaveLoadJsonFormatter _saveSys;

    void Start()
    {
        StartCoroutine(FadeInOut());
        _saveSys = FindObjectOfType<SaveLoadJsonFormatter>();
    }

    IEnumerator FadeInOut()
    {
        if (!_hasFaded)
        {
            _hasFaded = true;

            fadePanel.color = new Color(0f, 0f, 0f, 1f);

            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                float t = elapsedTime / fadeDuration;
                fadePanel.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, t)); 
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            fadePanel.color = new Color(0f, 0f, 0f, 0f); 
            fadePanel.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        //try to get the save data info
        _saveSys.LoadGame(out int wave);
        
        Debug.Log(wave);
        
        //if wave = 0, start from cutscene
        StartCoroutine(wave == 0 ? FadeAndLoadScene("Cutscene") : FadeAndLoadScene("Loading"));
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
        fadePanel.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            fadePanel.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadePanel.color = new Color(0f, 0f, 0f, 0f);

        if (fadePanel.color.a <= 0f)
        {
            fadePanel.gameObject.SetActive(false);
        }
    }

}
