using Characters.Combat.Info;
using Characters.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Characters.Combat.Hitbox
{
    [CustomPropertyDrawer(typeof(HitInfoBase), true)]
    public class HitInfoBasePropertyDrawer : PropertyDrawer
    {
        protected float LineHeight;
        private int _lines;
        private Rect _pos;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * _lines + EditorGUIUtility.standardVerticalSpacing * 
                (_lines - 1);
        }

        private bool _generalFoldoutGroup;
        protected bool ForcesFoldoutGroup = false;
        protected bool StunFoldoutGroup = false;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _lines = 0;
            _pos = position;
            EditorGUI.BeginProperty(position, label, property);
            LineHeight = EditorGUIUtility.singleLineHeight;
            DrawProperty(ref position, property);
            EditorGUI.EndProperty();
        }

        protected float GetLineY()
        {
            return _pos.min.y + _lines++ * LineHeight;
        }

        public virtual void DrawProperty(ref Rect position, SerializedProperty property)
        {
            // GENERAL //
            _generalFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, GetLineY(), 
                    position.width, LineHeight), _generalFoldoutGroup, new GUIContent("General"));
            
            if (_generalFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                DrawGeneralGroup(ref position, property);
                EditorGUI.indentLevel--;
            }
            
            EditorGUI.EndFoldoutHeaderGroup();
        }

        protected virtual void DrawGeneralGroup(ref Rect position, SerializedProperty property)
        {
            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                property.FindPropertyRelative("ID"), true);
        }
    }
}