using System.Collections.Generic;
using Quests.Definitions.Tasks;
using Quests.Interfaces;
using Quests.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quests.Definitions
{
    [CreateAssetMenu(fileName = "QuestJournalSettings", menuName = "Fluid/Quest Journal/Settings")]
    public class QuestJournalSettings: SettingsBase<QuestJournalSettings>
    {
        [FormerlySerializedAs("_database")] [SerializeField]
        private QuestDatabase database;

        [FormerlySerializedAs("_startingQuests")] [SerializeField]
        private List<QuestDefinitionBase> startingQuests;

        [FormerlySerializedAs("_debugQuests")]
        [Header("Debug")]

        [Tooltip("Quests started automatically while using the Unity editor mode (excluded from runtime)")]
        [SerializeField]
        private List<QuestDebugEntry> debugQuests;

        [FormerlySerializedAs("_debugTasks")]
        [Tooltip("Automatically starts the corresponding quest and sets the task position while using the editor")]
        [SerializeField]
        private List<TaskDefinition> debugTasks;

        public IQuestDatabase Database => database;
        public List<QuestDefinitionBase> StartingQuests => startingQuests;
        public List<QuestDebugEntry> DebugQuests => debugQuests;
        public List<TaskDefinition> DebugTasks => debugTasks;
    }
}