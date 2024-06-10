using FinalScripts;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EquipmentManager : MythUIElement
    {
        public KeyCode key;
        public GameObject equipmentMenuPanel;
        public GameObject pauseMenuPanel;
        
        public Button[] slots;
        public AbilityRepresenter[] abilities;
        public Image[] slotImages;  
        
        private bool _isPaused = false;
        private bool _isEquipmentMenuActive = false;

        private int _selectedSlotIndex = -1;
        private PlayerInv _playerInv;
        
        void Start()
        {
            _playerInv = FindObjectOfType<PlayerInv>();
            
            for (int i = 0; i < slots.Length; i++)
            {
                int index = i; // Local copy of the index for the lambda closure
                slots[i].onClick.AddListener(() => OnSlotButtonClick(index));
            }

            for (int i = 0; i < abilities.Length; i++)
            {
                int index = i; // Local copy of the index for the lambda closure
                abilities[i].button.onClick.AddListener(() => OnAbilityButtonClick(index));
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
        
        public void OnCloseButtonClick()
        {
            HideEquipmentMenu();
        }

        void OnSlotButtonClick(int index)
        {
            _selectedSlotIndex = index;
            Debug.Log("Slot " + index + " selected. Now select an ability.");
        }

        void OnAbilityButtonClick(int index)
        {
            Debug.Log("Ability button clicked, index: " + index);

            if (_selectedSlotIndex != -1)
            {
                Sprite abilityImage = abilities[index].ability.icon;
                Debug.Log("Selected slot index: " + _selectedSlotIndex);

                // Check if the selected ability exists
                if (abilities[index].ability == null)
                {
                    Debug.LogError("Selected ability is null for index: " + index);
                    return;
                }

                _playerInv.SetAbilityOnSlot(abilities[index].ability, _selectedSlotIndex);

                if (abilityImage != null)
                {
                    slotImages[_selectedSlotIndex].sprite = abilityImage;
                    Debug.Log("Ability " + index + " set to slot " + _selectedSlotIndex + ".");
                    _selectedSlotIndex = -1;
                }
            }
            else
            {
                Debug.Log("Select a slot first.");
            }
        }
    }
}