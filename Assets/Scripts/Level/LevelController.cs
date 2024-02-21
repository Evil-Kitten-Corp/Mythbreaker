using ExtEvents;
using TinyScript;
using UnityEngine;

namespace Level
{
    public class LevelController : MonoBehaviour
    {
        public CustomLootDrop levelEndRewards;
        public ExtEvent onLevelComplete;

        public void CompleteLevel()
        {
            onLevelComplete.Invoke();
        }
    }
}