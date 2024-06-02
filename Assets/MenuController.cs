using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject controlsPanel; // Reference to the ControlsPanel
    public GameObject creditsPanel; // Reference to the CreditsPanel

    // This function will be called when the Start Game button is clicked
    public void StartGame()
    {
        // Replace "Combat" with the name of your game scene
        SceneManager.LoadScene("Combat");
    }

    // This function will be called when the Controls button is clicked
    public void ShowControls()
    {
        // Show the controls panel
        controlsPanel.SetActive(true);
    }

    // This function will be called when the Close button on the Controls panel is clicked
    public void HideControls()
    {
        // Hide the controls panel
        controlsPanel.SetActive(false);
    }

    // This function will be called when the Credits button is clicked
    public void ShowCredits()
    {
        // Show the credits panel
        creditsPanel.SetActive(true);
    }

    // This function will be called when the Close button on the Credits panel is clicked
    public void HideCredits()
    {
        // Hide the credits panel
        creditsPanel.SetActive(false);
    }

    // This function will be called when the Quit Game button is clicked
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
