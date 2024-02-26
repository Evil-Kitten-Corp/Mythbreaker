using System;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.ScriptableObjectCollections;
using BrunoMikoski.ServicesLocation;
using UnityEditor;
using UnityEngine;
using TinyScript;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
    using UnityEditorInternal;
#endif

namespace TinyScript 
{
    [CreateAssetMenu(fileName = "Loot Table", menuName = "Tools/Loot Table")]
    public class CustomLootDrop : ScriptableObject
    {
        [FormerlySerializedAs("GuaranteedLootTable")] public CustomDropChangeItem[] guaranteedLootTable = 
            Array.Empty<CustomDropChangeItem>();
        
        [FormerlySerializedAs("OneItemFromList")] public CustomDropChangeItem[] oneItemFromList = 
            new CustomDropChangeItem[1];
        
        [FormerlySerializedAs("WeightToNoDrop")] public float weightToNoDrop = 100;

        // Return List of Guaranteed Drop 
        public List<RarityChooser> GetGuaranteedLoot()
        {
            List<RarityChooser> lootList = new List<RarityChooser>();

            foreach (var t in guaranteedLootTable)
            {
                // Adds the drawn number of items to drop
                int count = Random.Range(t.minCountItem, t.maxCountItem);
                for (int j = 0; j < count; j++)
                {
                    lootList.Add(t.drop);
                }
            }
            
            var inv = ServiceLocator.Instance.GetInstance<Inventory.Inventory>();
            return inv.upgrades.Any(x => lootList.Any(y => y == x)) ? null : lootList;
        }

        // Return List of Optional Drop 
        public List<RarityChooser> GetRandomLoot(int changeCount)
        {
            List<RarityChooser> lootList = new List<RarityChooser>();
            float totalWeight = weightToNoDrop;

            // Executes a function a specified number of times
            for (int j = 0; j < changeCount; j++)
            {
                // They add up the entire weight of the items
                totalWeight += oneItemFromList.Sum(t => t.drop.Rarity.GetChance());

                float value = Random.Range(0, totalWeight);
                float timedValue = 0;

                foreach (var t in oneItemFromList)
                {
                    // If timed_value is greater than value, it means this item has been drawn
                    timedValue += t.drop.Rarity.GetChance();
                    
                    if (timedValue > value)
                    {
                        int count = Random.Range(t.minCountItem, t.maxCountItem + 1);
                        
                        for (int c = 0; c < count; c++)
                        {
                            var inv = ServiceLocator.Instance.GetInstance<Inventory.Inventory>();
                            
                            if (!inv.upgrades.Contains(t.drop))
                            {
                                lootList.Add(t.drop);
                            }
                        }
                        
                        break;
                    }
                }
            }

            return lootList;
        }


        /*public void SpawnDrop(Transform _position, int _count, float _range)
        {
            List<IRarityChooser> guaranteed = GetGuaranteedLoot();
            List<IRarityChooser> randomLoot = GetRandomLoot(_count);

            for (int i = 0; i < guaranteed.Count; i++)
            {
                Instantiate(guaranteed[i], new Vector3(_position.position.x + Random.Range(-_range, _range), _position.position.y, _position.position.z + Random.Range(-_range, _range)), Quaternion.identity);
            }

            for (int i = 0; i < randomLoot.Count; i++)
            {
                Instantiate(randomLoot[i], new Vector3(_position.position.x + Random.Range(-_range, _range), _position.position.y, _position.position.z + Random.Range(-_range, _range)), Quaternion.identity);
            }
        }*/
    }

    /* --------------------- */
    // Drop Item Change Class
    /* --------------------- */

    [Serializable]
    public class CustomDropChangeItem
    {
        //public float Weight;
        //public IRarityChooser Drop;

        [FormerlySerializedAs("Drop")] 
        [SOCItemEditorOptions(DrawType = DrawType.AsReference)] public RarityChooser drop;
        [FormerlySerializedAs("MinCountItem")] public int minCountItem;
        [FormerlySerializedAs("MaxCountItem")] public int maxCountItem;
    }
}

    /* --------------------- */
    // Custom Editor & Property Draw (look)
    /* --------------------- */


#if UNITY_EDITOR

    /* --------------------- */
    // Custom Editor
    /* --------------------- */

    [CustomEditor(typeof(CustomLootDrop))]
    public class CustomLootDropEditor : Editor
    {
        // Guaranteed
        SerializedProperty _guaranteedList;
        ReorderableList _reorderableGuaranteed;
        // Change
        SerializedProperty _changeToGetList;
        ReorderableList _reorderableChange;

        CustomLootDrop _ld;

        private void OnEnable()
        {
            /* GUARANTEED */
            _guaranteedList = serializedObject.FindProperty("GuaranteedLootTable");
            _reorderableGuaranteed = new ReorderableList(serializedObject, _guaranteedList, true, 
                true, true, true)
            {
                // Functions
                drawElementCallback = DrawGuaranteedListItems,
                drawHeaderCallback = DrawHeaderGuaranteed
            };

            /* Change */
            _changeToGetList = serializedObject.FindProperty("OneItemFromList");
            _reorderableChange = new ReorderableList(serializedObject, _changeToGetList, true, 
                true, true, true);
            // Functions
            _reorderableChange.drawElementCallback += DrawChangeListItems;
            _reorderableChange.drawHeaderCallback += DrawHeaderChange;

            _ld = target as CustomLootDrop;
        }
    


        void DrawHeaderGuaranteed(Rect rect) { EditorGUI.LabelField(rect, "Guaranteed Loot Table"); }
        void DrawHeaderChange(Rect rect) { EditorGUI.LabelField(rect, "Change To Get Loot Table"); }

        void DrawGuaranteedListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            CustomLootDrop loot = (CustomLootDrop)target;
            _reorderableGuaranteed.elementHeight = 42;

            SerializedProperty element = _reorderableGuaranteed.serializedProperty.GetArrayElementAtIndex(index);


            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), element, GUIContent.none);

            //LootDrop loot = (LootDrop)target;
    }

        void DrawChangeListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            _reorderableChange.elementHeight = 42;

            SerializedProperty element = _reorderableChange.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), element, GUIContent.none);
        }

        public override void OnInspectorGUI()
        {

            EditorUtility.SetDirty(target);
            CustomLootDrop loot = (CustomLootDrop)target;
            EditorGUILayout.BeginVertical("box");

            EditorGUI.indentLevel = 0;

            // Loot Name
            GUIStyle myStyle = new GUIStyle();
            myStyle.normal.textColor = GUI.color;
            myStyle.alignment = TextAnchor.UpperCenter;
            myStyle.fontStyle = FontStyle.Bold;
            int ti = myStyle.fontSize;

            EditorGUILayout.LabelField($"Loot Table", myStyle);


            myStyle.fontStyle = FontStyle.Italic;
            myStyle.fontSize = 20;

            EditorGUILayout.LabelField($",,{loot.name}''", myStyle);

            EditorGUILayout.Space(10);

            EditorGUI.BeginChangeCheck();


        /* Fixed by D9Construct */
            serializedObject.Update();
            ValidateGuaranteedList(loot);
            _reorderableGuaranteed.DoLayoutList();
            ValidateOneItemFromList(loot);
            _reorderableChange.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                for (int index = 0; index < loot.oneItemFromList.Length; index++)
                {
                    SerializedProperty oifElement = _reorderableChange.serializedProperty.GetArrayElementAtIndex(index);
                    
                    loot.oneItemFromList[index].drop = (RarityChooser)oifElement.FindPropertyRelative("Drop")
                        .objectReferenceValue;
                    
                    loot.oneItemFromList[index].minCountItem = oifElement.FindPropertyRelative("MinCountItem")
                        .intValue;
                    
                    loot.oneItemFromList[index].maxCountItem = oifElement.FindPropertyRelative("MaxCountItem")
                        .intValue;
                }
                
                for (int index = 0; index < loot.guaranteedLootTable.Length; index++)
                {
                    SerializedProperty guaranteedElement = _reorderableGuaranteed.serializedProperty
                        .GetArrayElementAtIndex(index);
                    
                    loot.guaranteedLootTable[index].drop = (RarityChooser)guaranteedElement.FindPropertyRelative("Drop")
                        .objectReferenceValue;
                    
                    loot.guaranteedLootTable[index].minCountItem = guaranteedElement.FindPropertyRelative("MinCountItem")
                        .intValue;
                    
                    loot.guaranteedLootTable[index].maxCountItem = guaranteedElement.FindPropertyRelative("MaxCountItem")
                        .intValue;
                }
            }

        // Nothing Weight
        loot.weightToNoDrop = EditorGUILayout.FloatField("No Drop Weight", loot.weightToNoDrop);

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            Rect r = EditorGUILayout.BeginVertical("box");
            myStyle.fontStyle = FontStyle.Bold;
            myStyle.fontSize = 20;

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField($"Drop Change", myStyle);

            float totalWeight = loot.weightToNoDrop;
            float guaranteedHeight = 0;

            if (loot.oneItemFromList != null)
            {
                totalWeight += loot.oneItemFromList.Where(t => t.drop != null)
                    .Sum(t => t.drop.Rarity.GetChance());
            }

            
            var oldColor = GUI.backgroundColor;

            if (0 < loot.guaranteedLootTable.Length) { guaranteedHeight += 10; }

            /* Guaranteed */
            GUI.backgroundColor = Color.green;
            
            for (int i = 0; i < loot.guaranteedLootTable.Length; i++)
            {
                guaranteedHeight += 25;
                
                string tmpString = loot.guaranteedLootTable[i].drop == null ? 
                    " --- No Drop Object --- " : loot.guaranteedLootTable[i].drop.name;
                
                EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i), r.width - 10, 20), 
                    1, $"{tmpString} [{loot.guaranteedLootTable[i].minCountItem}-" +
                       $"{loot.guaranteedLootTable[i].maxCountItem}]  " +
                       $" -   Guaranteed");
            }
            GUI.backgroundColor = oldColor;

            /* Not Guaranteed */
            if (loot.oneItemFromList != null)
                for (int i = 0; i < loot.oneItemFromList.Length; i++)
                {
                    string tmpString = loot.oneItemFromList[i].drop == null ? 
                        "!!! No Drop Object Attackhment !!!" : loot.oneItemFromList[i].drop.name;

                    var scriptableObjectCollectionItem = loot.oneItemFromList[i].drop;
                    
                    if (scriptableObjectCollectionItem != null &&
                        scriptableObjectCollectionItem.Rarity.GetChance() / totalWeight < 0)
                    {
                        GUI.backgroundColor = Color.red;
                        EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i) + guaranteedHeight,
                            r.width - 10, 20), 1, "Error");
                    }
                    else
                    {
                        var objectCollectionItem = loot.oneItemFromList[i].drop;
                        if (objectCollectionItem != null)
                            EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i) + guaranteedHeight,
                                    r.width - 10, 20), objectCollectionItem.Rarity.GetChance() / totalWeight,
                                $"{tmpString} " +
                                $"[{loot.oneItemFromList[i].minCountItem}-{loot.oneItemFromList[i].maxCountItem}]   -   " +
                                $"{(objectCollectionItem.Rarity.GetChance() / totalWeight * 100):f2}%");
                    }

                    GUI.backgroundColor = oldColor;
                }

            GUI.backgroundColor = Color.gray;
            
            if (loot.oneItemFromList != null)
            {
                EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * loot.oneItemFromList.Length + 10) 
                                                        + guaranteedHeight, r.width - 10, 20), 
                    loot.weightToNoDrop / totalWeight,
                    $"Nothing Additional   -   {(loot.weightToNoDrop / totalWeight * 100):f2}%");
                GUI.backgroundColor = oldColor;

                EditorGUILayout.Space(25 * loot.oneItemFromList.Length + 45 + guaranteedHeight);
            }

            EditorGUILayout.EndVertical();
        }

        void ValidateOneItemFromList(CustomLootDrop loot)
        {
            bool countError = false;
            bool prefabError = false;
            const bool weightError = false;

            foreach (var t in loot.oneItemFromList)
            {
                if (t.drop == null) { prefabError = true; }
                if (t.minCountItem <= 0) { countError = true; }
                if (t.minCountItem > t.maxCountItem) { countError = true; } 
                
                //if (scriptableObjectCollectionItem != null && scriptableObjectCollectionItem.Rarity.GetChance()
                //< 0) { _weightError = true; }
            }
            
            if (prefabError) { EditorGUILayout.HelpBox("One of the List Items does not have " +
                                                       "''Item To Drop'' assigned, which will cause an error " +
                                                       "if it is drawn", MessageType.Error, true); }
            
            if (countError) { EditorGUILayout.HelpBox("One of the List Items has an incorrect number of " +
                                                      "items, which will result in items not appearing when " +
                                                      "drawn", MessageType.Warning, true); }
        }
        
        void ValidateGuaranteedList(CustomLootDrop loot)
        {
            bool countError = false;
            bool prefabError = false;
            const bool weightError = false;

            foreach (var t in loot.guaranteedLootTable)
            {
                if (t.drop == null) { prefabError = true; }
                if (t.minCountItem <= 0) { countError = true; }
                if (t.minCountItem > t.maxCountItem) { countError = true; }
                //if (loot.GuaranteedLootTable[index].Drop.Rarity.GetChance() < 0) { _weightError = true; }
            }
            
            if (prefabError) { EditorGUILayout.HelpBox("One of the List Items does not have " +
                                                       "''Item To Drop'' assigned, which will cause an error " +
                                                       "if it is drawn", MessageType.Error, true); }
            
            if (countError) { EditorGUILayout.HelpBox("One of the List Items has an incorrect number of " +
                                                      "items, which will result in items not appearing when" +
                                                      " drawn", MessageType.Warning, true); }
        }
    }

    /* --------------------- */
    // Custom Property Draw
    /* --------------------- */

    [CustomPropertyDrawer(typeof(CustomDropChangeItem))]
    public class CustomDropChangeItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var oldColor = GUI.backgroundColor;
            EditorGUI.BeginProperty(position, label, property);
            //GUI.backgroundColor = Color.red;

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // var weightRectLabel = new Rect(position.x, position.y, 55, 18);
            // var weightRect = new Rect(position.x, position.y + 20, 55, 18);
            //
            // EditorGUI.LabelField(weightRectLabel, "Weight");
            // if(property.FindPropertyRelative("Weight").floatValue < 0) { GUI.backgroundColor = Color.red; }
            // EditorGUI.PropertyField(weightRect, property.FindPropertyRelative("Weight"), GUIContent.none);
            // GUI.backgroundColor = _oldColor;

            var itemRectLabel = new Rect(position.x + 60, position.y, position.width - 60, 18);
            var itemRect = new Rect(position.x + 60, position.y + 20, position.width - 60 - 75, 18);

            EditorGUI.LabelField(itemRectLabel, "Item To Drop");
            if(property.FindPropertyRelative("Drop").objectReferenceValue == null) { GUI.backgroundColor = Color.red; }
            EditorGUI.PropertyField(itemRect, property.FindPropertyRelative("Drop"), GUIContent.none);
            GUI.backgroundColor = oldColor;

            var minMaxRectLabel = new Rect(position.x + position.width - 70, position.y, 70, 18);

            var minRect = new Rect(position.x + position.width - 70, position.y + 20, 30, 18);
            var minMaxRect = new Rect(position.x + position.width - 39, position.y + 20, 9, 18);
            var maxRect = new Rect(position.x + position.width - 30, position.y + 20, 30, 18);

            if(property.FindPropertyRelative("MinCountItem").intValue < 0) { GUI.backgroundColor = Color.red; }
            if (property.FindPropertyRelative("MaxCountItem").intValue < property.FindPropertyRelative("MinCountItem")
                    .intValue) { GUI.backgroundColor = Color.red; }

            EditorGUI.LabelField(minMaxRectLabel, "Min  -  Max");
            EditorGUI.PropertyField(minRect, property.FindPropertyRelative("MinCountItem"), GUIContent.none);
            EditorGUI.LabelField(minMaxRect, "-");
            EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("MaxCountItem"), GUIContent.none); 
            GUI.backgroundColor = oldColor;

            EditorGUI.EndProperty(); 
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 40;
        }
    }

#endif