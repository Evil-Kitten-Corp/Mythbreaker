using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Quests.Utilities
{
    public abstract class SortableListBase 
    {
        protected readonly ReorderableList List;
        protected Editor Editor;

        private readonly SerializedObject _serializedObject;
        protected readonly SerializedProperty SerializedProp;

        protected SortableListBase(Editor editor, string property, string title) 
        {
            if (editor == null) 
            {
                Debug.LogError("Editor cannot be null");
                return;
            }
            
            SerializedProp = editor.serializedObject.FindProperty(property);
            _serializedObject = editor.serializedObject;

            if (SerializedProp == null) 
            {
                Debug.LogErrorFormat("Could not find property {0}", property);
                return;
            }

            List = new ReorderableList(
                _serializedObject, 
                SerializedProp, 
                true, true, true, true)
            {
                drawHeaderCallback = rect => 
                {  
                    EditorGUI.LabelField(rect, title);
                }
            };
        }

        public void Update() 
        {
            _serializedObject.Update();

            List?.DoLayoutList();

            _serializedObject.ApplyModifiedProperties();
        }
    }
}