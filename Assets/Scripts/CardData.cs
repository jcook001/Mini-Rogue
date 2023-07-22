using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    None,
    XP,
    ArmorPoint,
    HP,
    Food,
    Gold,
    Treasure,
    FirePotion,
    FrostPotion,
    PoisonPotion,
    HealingPotion,
    HolyPotion,
    PerceptionPotion,
    AnyPotion,
    RefreshSkills,
    Blessing,
    CurePoison,
    CureCurse,
    Choice,
    DropFloor
}

[System.Serializable]
public struct TradeOption
{
    public ResourceCost Costs;
    public List<ResourceReward> Rewards;
}

[System.Serializable]
public struct ResourceCost
{
    //0 Quantitiy means equals floor
    public List<ResourceType> ResourceType;
    public List<int> Quantity;
}

[System.Serializable]
public struct ResourceReward
{
    public List<ResourceType> ResourceType;
    //0 Quantitiy means equals floor
    public List<int> Quantity;
    public List<ResourceType> Choice;
}

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
    public enum DamageEffects
    {
        None,
        Curse,
        Poison,
        Blindness,
        IgnoreArmor,
        Weaken,
        Regeneration,
        Fall
    }

    public string[] MonsterName = new string[4];
    public int[] OnePlayerHealth = new int[4];
    public int[] TwoPlayerHealth = new int[4];
    public int[] DamageValue = new int[4];
    public DamageEffects[] DamageEffect = new DamageEffects[4];
    public DamageEffects[] DamageEffect2 = new DamageEffects[4];
    public int[] RewardValue = new int[4];
    public ResourceType[] RewardEffect = new ResourceType[4];

    //Trap

    //Tomb

    //Bonfire

    //Merchant
    public List<TradeOption> TradeOptions = new List<TradeOption>();

    //Treasure

    //Shrine

    //Reference
}
