using Quests.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quests.Definitions
{
    [System.Serializable]
    public class QuestDebugEntry 
    {
        [SerializeField]
        private QuestDefinitionBase definition;

        [FormerlySerializedAs("_markComplete")] [SerializeField]
        private bool markComplete;

        public IQuestDefinition Definition => definition;
        public bool MarkComplete => markComplete;
    }
}