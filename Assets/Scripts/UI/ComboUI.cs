using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    public GameObject UIparent;
    public TMP_Text number;
    private readonly List<Image> _inputBuffer = new();
    private int _currentInput;
    public GameObject inputPrefab;
    public Image timer;
    public GameObject horizontalLayout;
    public WeaponAndAttackManager attackManager;
    
    private Coroutine _timerCoroutine;
    
    void Start()
    {
        StartTimer();
        
        attackManager.LightAttackAllowed += delegate
        {
            if (MythUIElement.IsInUI) return;
            LogInput("L");
            _currentInput++;
            number.text = _currentInput.ToString();
            RestartTimer();
        };
        
        attackManager.HeavyAttackAllowed += delegate
        {
            if (MythUIElement.IsInUI) return;
            LogInput("R");
            _currentInput++;
            number.text = _currentInput.ToString();
            RestartTimer();
        };
    }

    private void Update()
    {
        UIparent.SetActive(!MythUIElement.IsInUI);
    }

    void StartTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine); 
        }
        _timerCoroutine = StartCoroutine(Timer(1.5f));
    }

    void RestartTimer()
    {
        StopCoroutine(_timerCoroutine); // Stop the existing coroutine
        timer.fillAmount = 1f; // Reset fill amount to max
        StartTimer(); // Start a new coroutine
    }

    IEnumerator Timer(float time)
    {
        float currentTime = time;
        
        while (currentTime > 0f)
        {
            currentTime -= Time.deltaTime; // Reduce the current time by deltaTime
            float fillAmount = currentTime / time; // Calculate the fill amount based on remaining time
            timer.fillAmount = fillAmount; // Set the fill amount of the image
            yield return null; 
        }
        
        timer.fillAmount = 0f;
        ResetCombo();
    }

    public void ResetCombo()
    {
        _inputBuffer.Clear();
        
        for (int i = 0; i < horizontalLayout.transform.childCount; i++)
        {
            Destroy(horizontalLayout.transform.GetChild(i).gameObject);
        }
        
        _currentInput = 0;
        number.text = "";
    }

    void LogInput(string what)
    {
        if (_inputBuffer.Count > 5)
        {
            _inputBuffer.Clear();

            for (int i = 0; i < horizontalLayout.transform.childCount; i++)
            {
                Destroy(horizontalLayout.transform.GetChild(i).gameObject);
            }
        }

        var go = Instantiate(inputPrefab, horizontalLayout.transform);

        _inputBuffer.Add(go.GetComponent<Image>());
        go.GetComponentInChildren<TMP_Text>().text = what;
    }
}
