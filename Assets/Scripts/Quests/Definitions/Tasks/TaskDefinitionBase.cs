using System;
using Quests.Interfaces;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quests.Definitions.Tasks
{
    public abstract class TaskDefinitionBase :
        ScriptableObject,
#if UNITY_EDITOR
        ITaskDefinition,
        ISetupEditor
#else
        ITaskDefinition
#endif
    {
        [FormerlySerializedAs("_id")] [ReadOnly] [SerializeField] private string id;

        [FormerlySerializedAs("_parent")] [ReadOnly] [SerializeField] private QuestDefinitionBase parent;

        [FormerlySerializedAs("_description")] [TextArea] [SerializeField] private string description;

        public string Id => id;
        public string Title => name;
        public string Description => description;
        public IQuestDefinition Parent => parent;

#if UNITY_EDITOR
        public void SetupEditor()
        {
            name = "Untitled Task";
            id = Guid.NewGuid().ToString();
        }

        public void SetParent(QuestDefinitionBase parent)
        {
            this.parent = parent;
        }
#endif
    }
}