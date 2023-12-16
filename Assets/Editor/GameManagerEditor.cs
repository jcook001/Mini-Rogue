using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GameManager gameManager = (GameManager)target;

        SerializedProperty property = serializedObject.GetIterator();
        bool next = property.NextVisible(true); // Start with the first property

        while (next)
        {
            if (property.name == "DebugOverrideDungeonCards")
            {
                EditorGUILayout.PropertyField(property, true); // Draw the DebugOverrideDungeonCards field

                // Draw the CardData dropdown
                string[] guids = AssetDatabase.FindAssets("t:CardData");
                gameManager.allCardData = guids.Select(g => AssetDatabase.LoadAssetAtPath<CardData>(AssetDatabase.GUIDToAssetPath(g))).ToList();
                string[] options = gameManager.allCardData.Select(cardData => cardData != null ? cardData.name : "None").ToArray();
                gameManager.selectedCardIndex = EditorGUILayout.Popup("Debug Override Card", gameManager.selectedCardIndex, options);
            }
            else
            {
                EditorGUILayout.PropertyField(property, true); // Draw all other fields automatically
            }

            next = property.NextVisible(false); // Move to next property
        }

        serializedObject.ApplyModifiedProperties();
    }
}
