using System.Collections.Generic;
using System.Linq;
using Quests.Interfaces;
using UnityEngine;

namespace Quests.Definitions
{
    [CreateAssetMenu(menuName = "Tools/Quest Journal/Database", fileName = "QuestDatabase")]
    public class QuestDatabase : ScriptableObject, IQuestDatabase 
    {
        private Dictionary<string, IQuestDefinition> _idToQuest;

        [HideInInspector]
        public List<QuestDefinitionBase> questDefinitions = new();

        public void Setup () 
        {
            _idToQuest = questDefinitions.ToDictionary((quest) => quest.Id, (quest) => quest as IQuestDefinition);
        }

        public IQuestDefinition GetQuest (string id) 
        {
            return _idToQuest[id];
        }
    }
}