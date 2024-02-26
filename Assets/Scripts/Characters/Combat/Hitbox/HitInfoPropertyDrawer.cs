using Characters.Combat.Info;
using Characters.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Characters.Combat.Hitbox
{
    [CustomPropertyDrawer(typeof(HitInfo), true)]
    public class HitInfoPropertyDrawer : HitInfoBasePropertyDrawer
    {
        private bool _damageFoldoutGroup;
        
        public override void DrawProperty(ref Rect position, SerializedProperty property)
        {
            base.DrawProperty(ref position, property);

            // FORCES //
            ForcesFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, GetLineY(), 
                    position.width, LineHeight), ForcesFoldoutGroup, new GUIContent("Forces"));
            
            if (ForcesFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                DrawForcesGroup(ref position, property);
                EditorGUI.indentLevel--;
            }
            
            EditorGUI.EndFoldoutHeaderGroup();

            // STUN //
            StunFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, GetLineY(), 
                    position.width, LineHeight), StunFoldoutGroup, new GUIContent("Stun"));
            
            if (StunFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                DrawStunGroup(position, property);
                EditorGUI.indentLevel--;
            }
            
            EditorGUI.EndFoldoutHeaderGroup();
            
            // DAMAGE //
            _damageFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, GetLineY(), 
                    position.width, LineHeight), _damageFoldoutGroup, new GUIContent("Damage"));
            
            if (_damageFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                DrawDamageGroup(position, property);
                EditorGUI.indentLevel--;
            }
            
            EditorGUI.EndFoldoutHeaderGroup();
        }

        protected virtual void DrawDamageGroup(Rect position, SerializedProperty property)
        {
            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                property.FindPropertyRelative("damageOnHit"), true);
        }

        protected override void DrawGeneralGroup(ref Rect position, SerializedProperty property)
        {
            base.DrawGeneralGroup(ref position, property);
            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                property.FindPropertyRelative("causesTumble"), true);
            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                property.FindPropertyRelative("knockdown"), true);
        }

        protected virtual void DrawStunGroup(Rect position, SerializedProperty property)
        {
        }

        protected virtual void DrawForcesGroup(ref Rect position, SerializedProperty property)
        {
            switch((HitboxForceType)property.FindPropertyRelative("forceType").enumValueIndex)
            {
                case HitboxForceType.SET:
                    EditorGUI.LabelField(new Rect(position.x, GetLineY(), 100, LineHeight), 
                        new GUIContent("Force"));
                    EditorGUI.PropertyField(new Rect(position.x+140, GetLineY(), position.width-140, 
                            LineHeight), property.FindPropertyRelative("opponentForce"),
                        GUIContent.none, true);
                    break;
                default:
                    EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                        property.FindPropertyRelative("forceIncludeYForce"), true);
                    EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                        property.FindPropertyRelative("opponentForceMultiplier"), true);
                    EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                        property.FindPropertyRelative("opponentMinMagnitude"), true);
                    EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                        property.FindPropertyRelative("opponentMaxMagnitude"), true);
                    break;
            }
            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                property.FindPropertyRelative("groundBounces"), true);
            
            if (property.FindPropertyRelative("groundBounces").boolValue)
            {
                EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                    property.FindPropertyRelative("groundBounceForce"), true);
            }

            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                property.FindPropertyRelative("wallBounces"), true);
            
            if (property.FindPropertyRelative("wallBounces").boolValue)
            {
                EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, LineHeight), 
                    property.FindPropertyRelative("wallBounceForce"), true);
            }
        }
    }
}