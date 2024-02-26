using System.Collections.Generic;
using Quests.Definitions;
using Quests.Definitions.Tasks;
using UnityEditor;

namespace Quests.Utilities
{
    public class SortableListTasks: SortableListBase 
    {
        private static TypesToMenu<TaskDefinitionBase> _taskTypes;

        private static TypesToMenu<TaskDefinitionBase> TaskTypes =>
            _taskTypes ??= new TypesToMenu<TaskDefinitionBase>();

        public SortableListTasks(Editor editor, string property, QuestDefinitionBase parent, 
            List<TaskDefinitionBase> tasks, string title) : base(editor, property, title) 
        {
            Editor = editor;

            var soPrinter = new ScriptableObjectListPrinter(SerializedProp);
            var taskCrud = new NestedDataCrud<TaskDefinitionBase>(parent, tasks, TaskTypes);
            
            taskCrud.BindOnCreate((task) =>
            {
                task.SetParent(parent);
            });

            List.drawElementCallback = soPrinter.DrawScriptableObject;
            List.elementHeightCallback = soPrinter.GetHeight;

            List.onAddDropdownCallback = taskCrud.ShowMenu;
            List.onRemoveCallback = taskCrud.DeleteItem;
        }
    }
}