using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtitleSystem : MonoBehaviour
{
    public Subtitle[] lines;
    public bool skippableLines = true;
    public bool skippableScene = true;
    public string nextSceneName;

    public AudioSource audioPlayer;
    public TMP_Text subtitle;
    public TMP_Text skip;
    public Image skipBar;
    public float skipDurationTrigger;

    private int _currentLine = -1;
    private Coroutine _skipCoroutine;

    void Start()
    {
        skip.gameObject.SetActive(false);
        PlayNext();
    }

    void Update()
    {
        if (skippableScene && Input.GetKey(KeyCode.Space))
        {
            if (_skipCoroutine == null)
            {
                _skipCoroutine = StartCoroutine(HandleSkipScene());
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (_skipCoroutine != null)
            {
                StopCoroutine(_skipCoroutine);
                _skipCoroutine = null;
                skip.gameObject.SetActive(false);
                skipBar.fillAmount = 0;
            }
        }

        if (skippableLines && Input.GetMouseButtonDown(1))
        {
            PlayNext();
        }
    }

    IEnumerator HandleSkipScene()
    {
        skip.gameObject.SetActive(true);
        float elapsedTime = 0;

        while (elapsedTime < skipDurationTrigger)
        {
            elapsedTime += Time.deltaTime;
            skipBar.fillAmount = elapsedTime / skipDurationTrigger;
            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }

    void PlayNext()
    {
        audioPlayer.Stop();

        if (_currentLine > -1)
        {
            StopCoroutine(WaitForSubtitleToEnd(lines[_currentLine].duration));
        }
        
        _currentLine++;
        
        if (_currentLine >= lines.Length)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
            return;
        }

        Subtitle currentSubtitle = lines[_currentLine];
        subtitle.text = currentSubtitle.text;
        
        if (currentSubtitle.voiceline != null)
        {
            audioPlayer.clip = currentSubtitle.voiceline;
            audioPlayer.Play();
        }

        StartCoroutine(WaitForSubtitleToEnd(currentSubtitle.duration));
    }

    IEnumerator WaitForSubtitleToEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        PlayNext();
    }
}

[System.Serializable]
public struct Subtitle
{
    public string text;
    public AudioClip voiceline;
    public float duration;
}
