using Quests.Utilities;
using UnityEditor;

namespace Quests.Definitions
{
    [CustomEditor(typeof(QuestDefinitionBase), true)]
    public class QuestDefinitionInspector: Editor 
    {
        private SortableListTasks _taskList;

        private void OnEnable ()
        {
            var quest = target as QuestDefinitionBase;
            if (quest != null) _taskList = new SortableListTasks(this, "_tasks", quest, quest.tasks, "Tasks");
        }

        public override void OnInspectorGUI () {
            target.name = EditorGUILayout.TextField("Title", target.name);
            _taskList.Update();

            // Listed last to prevent textarea pointer from bugging out (Unity bug)
            base.OnInspectorGUI();
        }
    }
}