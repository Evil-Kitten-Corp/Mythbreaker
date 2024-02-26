using System;
using System.Collections.Generic;
using Quests.Definitions.Tasks;
using Quests.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quests.Definitions
{
    public abstract class QuestDefinitionBase: ScriptableObject,
#if UNITY_EDITOR
        IQuestDefinition,
        ISetupEditor
#else
        IQuestDefinition
#endif
    {
        [FormerlySerializedAs("_id")]
        [HideInInspector]
        [SerializeField]
        private string id;

        [FormerlySerializedAs("_description")]
        [TextArea]
        [SerializeField]
        private string description;

        [FormerlySerializedAs("_tasks")]
        [HideInInspector]
        [SerializeField]
        public List<TaskDefinitionBase> tasks = new();

        public string Id => id;
        public string Title => name;
        public string Description => description;
        public IReadOnlyList<ITaskDefinition> Tasks => tasks;

#if UNITY_EDITOR
        public void SetupEditor () 
        {
            name = "Untitled Quest";
            id = Guid.NewGuid().ToString();
        }
#endif
    }
}