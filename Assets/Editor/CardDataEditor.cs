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
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemStats"), true);
                break;

            case CardData.CardType.Monster:
                // Draw fields relevant to monster cards.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MonsterStats"), true);
                break;

            case CardData.CardType.Boss:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BossStats"), true);
                // Draw fields relevant to boss cards.
                break;

            case CardData.CardType.Trap:
                // Draw fields relevant to trap cards.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TrapResult"), true);
                break;
            case CardData.CardType.Trap_Depths:
                // Draw fields relevant to trap cards.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TrapResult_Depths"), true);
                break;

            case CardData.CardType.Tomb:
                // Draw fields relevant to tomb cards.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TombRewards"), true);
                break;

            case CardData.CardType.Bonfire:
                // Draw fields relevant to bonfire cards.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BonfireRewards"), true);
                break;

            case CardData.CardType.Merchant:
                // Draw fields relevant to merchant cards.
                EditorGUILayout.HelpBox("Floor costs should be entered as 0", MessageType.Info);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TradeOptions"), true);
                break;

            case CardData.CardType.Treasure:
                // Draw fields relevant to treasure cards.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TreasureRewards"), true);
                break;

            case CardData.CardType.Treasure_Depths:
                // Draw fields relevant to treasure cards.
                break;

            case CardData.CardType.Shrine:
                // Draw fields relevant to shrine cards.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TombRewards"), true);
                break;

            case CardData.CardType.Reference:
                // Draw fields relevant to reference cards.
                break;

            case CardData.CardType.Monster_Bandit:
                // Draw fields relevant to bandit cards.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MonsterBanditStats"), true);
                break;

            case CardData.CardType.Boss_Final:
                // Draw fields relevant to bandit cards.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BossFinalStats"), true);
                break;
        }

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();

        //If data has been changed, mark it as changed
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
    }
}
