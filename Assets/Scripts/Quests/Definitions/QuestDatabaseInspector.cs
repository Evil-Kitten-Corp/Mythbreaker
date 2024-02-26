using Quests.Utilities;
using UnityEditor;

namespace Quests.Definitions
{
    [CustomEditor(typeof(QuestDatabase))]
    public class QuestDatabaseInspector : Editor 
    {
        private SortableListQuestDefinitions _itemList;

        private void OnEnable() 
        {
            _itemList = new SortableListQuestDefinitions(this, "questDefinitions", "Definitions");
        }

        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            _itemList.Update();
        }
    }
}