using DefaultNamespace;
using UnityEngine;

public class EquipmentMenuController : MythUIElement
{
    public KeyCode key;
    
    public GameObject equipmentMenuPanel; // Reference to the EquipmentMenuPanel
    public GameObject pauseMenuPanel; // Reference to the PauseMenuPanel
    public GameObject[] abilityButtons; // References to the ability buttons
    public GameObject[] abilityImages; // References to the ability images

    private bool _isPaused = false;
    private bool _isEquipmentMenuActive = false;
    private readonly bool[] _isButtonEnabled = new bool[10]; // Array to track if a button can be selected

    void Start()
    {
        for (int i = 0; i < _isButtonEnabled.Length; i++)
        {
            _isButtonEnabled[i] = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (_isEquipmentMenuActive)
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
        _isPaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
        _isPaused = true;
    }

    public void ShowEquipmentMenu()
    {
        equipmentMenuPanel.SetActive(true);
        IsInUI = true;
        PauseGame();
        _isEquipmentMenuActive = true;
    }

    public void HideEquipmentMenu()
    {
        equipmentMenuPanel.SetActive(false);
        IsInUI = false;
        
        if (pauseMenuPanel.activeSelf)
        {
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            ResumeGame(); 
        }
        
        _isEquipmentMenuActive = false;
    }

    public void OnAbilityButtonClick(int index)
    {
        if (index >= 0 && index < abilityImages.Length && _isButtonEnabled[index])
        {
            _isButtonEnabled[index] = !_isButtonEnabled[index];
            _isButtonEnabled[(index % 2 == 0) ? index + 1 : index - 1] = !_isButtonEnabled[index];
            
            abilityImages[index].SetActive(_isButtonEnabled[index]);
            abilityImages[(index % 2 == 0) ? index + 1 : index - 1].SetActive(!_isButtonEnabled[index]);
        }
    }

    public void OnCloseButtonClick()
    {
        HideEquipmentMenu();
    }
}
