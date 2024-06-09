using UnityEngine;

public class EquipmentMenuController : MonoBehaviour
{
    public GameObject equipmentMenuPanel; // Reference to the EquipmentMenuPanel
    public GameObject pauseMenuPanel; // Reference to the PauseMenuPanel
    public GameObject[] abilityButtons; // References to the ability buttons
    public GameObject[] abilityImages; // References to the ability images

    private bool isPaused = false;
    private bool isEquipmentMenuActive = false;
    private bool[] isButtonEnabled = new bool[10]; // Array to track if a button can be selected

    void Start()
    {
        // Initialize all buttons to be enabled
        for (int i = 0; i < isButtonEnabled.Length; i++)
        {
            isButtonEnabled[i] = true;
        }
    }

    void Update()
    {
        // Toggle equipment menu and pause/resume game when the C key is pressed
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isEquipmentMenuActive)
            {
                HideEquipmentMenu();
            }
            else
            {
                ShowEquipmentMenu();
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume game time
        Cursor.lockState = CursorLockMode.None; // Ensure cursor is unlocked
        Cursor.visible = true; // Ensure cursor is visible
        isPaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause game time
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true; // Show cursor
        isPaused = true;
    }

    public void ShowEquipmentMenu()
    {
        equipmentMenuPanel.SetActive(true);
        PauseGame(); // Pause the game and show the cursor
        isEquipmentMenuActive = true;
    }

    public void HideEquipmentMenu()
    {
        equipmentMenuPanel.SetActive(false);
        if (pauseMenuPanel.activeSelf)
        {
            // If the pause menu is active, don't resume the game time but ensure cursor state
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            ResumeGame(); // Resume the game and hide the cursor
        }
        isEquipmentMenuActive = false;
    }

    public void OnAbilityButtonClick(int index)
    {
        if (index >= 0 && index < abilityImages.Length && isButtonEnabled[index])
        {
            // Toggle the button and its corresponding button
            isButtonEnabled[index] = !isButtonEnabled[index];
            isButtonEnabled[(index % 2 == 0) ? index + 1 : index - 1] = !isButtonEnabled[index];

            // Toggle the image visibility
            abilityImages[index].SetActive(isButtonEnabled[index]);
            abilityImages[(index % 2 == 0) ? index + 1 : index - 1].SetActive(!isButtonEnabled[index]);
        }
    }

    // Method for close button to call
    public void OnCloseButtonClick()
    {
        HideEquipmentMenu();
    }
}
