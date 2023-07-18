using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardData))]
public class CardDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        CardData cardData = (CardData)target;
        
        // Always draw the card name field
        cardData.cardName = EditorGUILayout.TextField("Card Name", cardData.cardName);
        // Always draw the model field.
        cardData.model = (GameObject)EditorGUILayout.ObjectField("Model", cardData.model, typeof(GameObject), false);
        // Always draw the card type field.
        cardData.cardType = (CardData.CardType)EditorGUILayout.EnumPopup("Card Type", cardData.cardType);

        // Depending on the card type, draw different fields.
        switch (cardData.cardType)
        {
            case CardData.CardType.Item:
                // Draw fields relevant to item cards.
                break;

            case CardData.CardType.Monster:
                // Draw fields relevant to monster cards.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MonsterName"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnePlayerHealth"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TwoPlayerHealth"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DamageValue"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DamageEffect"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DamageEffect2"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("RewardValue"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("RewardEffect"), true);
                break;

            case CardData.CardType.Boss:
                // Draw fields relevant to boss cards.
                break;

            case CardData.CardType.Trap:
                // Draw fields relevant to trap cards.
                break;

            case CardData.CardType.Tomb:
                // Draw fields relevant to tomb cards.
                break;

            case CardData.CardType.Bonfire:
                // Draw fields relevant to bonfire cards.
                break;

            case CardData.CardType.Merchant:
                // Draw fields relevant to merchant cards.
                break;

            case CardData.CardType.Treasure:
                // Draw fields relevant to treasure cards.
                break;

            case CardData.CardType.Shrine:
                // Draw fields relevant to shrine cards.
                break;

            case CardData.CardType.Reference:
                // Draw fields relevant to reference cards.
                break;
        }

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}
