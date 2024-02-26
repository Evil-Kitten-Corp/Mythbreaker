using UnityEngine;

namespace Quests.Utilities
{
    public class SettingsObject : ScriptableObject 
    {
        public void Setup() 
        {
            OnSetup();
        }

        protected virtual void OnSetup() 
        {
        }
    }
}