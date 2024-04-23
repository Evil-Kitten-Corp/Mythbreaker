using System;
using UnityEngine;

namespace Abilities
{
    public abstract class UIMenu : MonoBehaviour
    {
        public vThirdPersonCamera mainCamera;

        private void Start()
        {
            OnEnable();
        }

        private void OnEnable()
        {
            mainCamera.lockCamera = true;
            Cursor.visible = true;
            Time.timeScale = 0f;
            Initialise();
        }

        private void OnDisable()
        {
            mainCamera.lockCamera = false;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

        protected virtual void Initialise()
        {
            
        }
    }
}