using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MythUIElement
{
    public KeyCode key;
    
    public GameObject pauseMenuPanel; // Reference to the PauseMenuPanel
    public GameObject controlsImage; // Reference to the ControlsImage
    public GameObject optionsImage; // Reference to the OptionsImage

    private bool _isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        controlsImage.SetActive(false); // Hide the controls image if it's shown
        optionsImage.SetActive(false); // Hide the options image if it's shown
        Time.timeScale = 1f; // Resume game time
        _isPaused = false;
        IsInUI = false;
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Pause game time
        _isPaused = true;
        IsInUI = true;
    }

    public void QuitGame()
    {
        // Add functionality to quit the game or load the main menu
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowControls()
    {
        // Show the controls image
        controlsImage.SetActive(true);
    }

    public void HideControls()
    {
        // Hide the controls image
        controlsImage.SetActive(false);
    }

    public void ShowOptions()
    {
        // Show the options image
        optionsImage.SetActive(true);
    }

    public void HideOptions()
    {
        // Hide the options image
        optionsImage.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Resume game time before changing the scene
        SceneManager.LoadScene("MainMenu");
    }

    public void EnableEffects()
    {
        Debug.Log("EnableEffects method called."); // Debug log
        GameObject[] effectsParents = GameObject.FindGameObjectsWithTag("Effects");

        foreach (GameObject parent in effectsParents)
        {
            EnableChildren(parent);
        }
    }

    private void EnableChildren(GameObject parent)
    {
        if (parent != null)
        {
            foreach (Transform child in parent.transform)
            {
                child.gameObject.SetActive(true);
                Debug.Log("Enabled child object: " + child.gameObject.name); // Debug log
            }
        }
    }

    public void DisableEffects()
    {
        Debug.Log("DisableEffects method called."); // Debug log
        GameObject[] effectsParents = GameObject.FindGameObjectsWithTag("Effects");

        foreach (GameObject parent in effectsParents)
        {
            DisableChildren(parent);
        }
    }

    private void DisableChildren(GameObject parent)
    {
        if (parent != null)
        {
            foreach (Transform child in parent.transform)
            {
                child.gameObject.SetActive(false);
                Debug.Log("Disabled child object: " + child.gameObject.name); // Debug log
            }
        }
    }
}
