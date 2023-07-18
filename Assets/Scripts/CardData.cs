using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public enum CardType
    {
        Item,
        Monster,
        Boss,
        Trap,
        Tomb,
        Bonfire,
        Merchant,
        Treasure,
        Shrine,
        Reference
    }
    public CardType cardType;
    public GameObject model;

    //Item
    
    //Monster + Boss
    public enum DamageEffect
    {
        Curse,
        Poison,
        Blindness,
        IgnoreArmor,
        Weaken,
        Regeneration,
        Fall
    }

    public int[] OnePlayerHealth = new int[4];
    public int[] TwoPlayerHealth = new int[4];
    public int[] Damage = new int[4];
    public DamageEffect[] damageEffects = new DamageEffect[4];

    //Trap

    //Tomb

    //Bonfire

    //Merchant

    //Treasure

    //Shrine

    //Reference
}
