using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
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
    Perceptionpotion,
    AnyPotion,
    RefreshSkills,
    Blessing,
    CurePoison,
    CureCurse
}

[System.Serializable]
public struct TradeOption
{
    public List<ResourceCost> Costs;
    public List<ResourceReward> Rewards;
}

[System.Serializable]
public struct ResourceCost
{
    public ResourceType ResourceType;
    public int Quantity;
    public bool MultiplyCostByFloor;
}

[System.Serializable]
public struct ResourceReward
{
    public ResourceType ResourceType;
    public int Quantity;
    public bool MultiplyCostByFloor;
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
    public enum ShopCost
    {
        XP,
        ArmorPoint,
        HP,
        Food,
        Gold,
        Treasure
    }

    public List<TradeOption> TradeOptions = new List<TradeOption>();

    //Treasure

    //Shrine

    //Reference
}
