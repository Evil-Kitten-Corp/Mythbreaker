using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class PopupScreen : MonoBehaviour
    {
        public UnityEvent OnAccept;
        public UnityEvent OnDecline;
        
        public GameObject popupScreen;

        public void CallPopup()
        {
            popupScreen.SetActive(true);
        }

        public void AcceptPopup()
        {
            OnAccept?.Invoke();
        }
        
        public void DeclinePopup()
        {
            OnDecline?.Invoke();
        }

        public void HelperExitApplication()
        {
            Application.Quit();
        }
    }
}