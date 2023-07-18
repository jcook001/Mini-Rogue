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
    Treasure
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
}

[System.Serializable]
public struct ResourceReward
{
    public ResourceType ResourceType;
    public int Quantity;
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

    //TODO merge this with resource type
    public enum RewardEffects
    {
        XP,
        ArmorPoint,
        HP,
        Food,
        Gold,
        Treasure
    }

    public string[] MonsterName = new string[4];
    public int[] OnePlayerHealth = new int[4];
    public int[] TwoPlayerHealth = new int[4];
    public int[] DamageValue = new int[4];
    public DamageEffects[] DamageEffect = new DamageEffects[4];
    public DamageEffects[] DamageEffect2 = new DamageEffects[4];
    public int[] RewardValue = new int[4];
    public RewardEffects[] RewardEffect = new RewardEffects[4];

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

    public enum ShopReward
    {
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

    public List<TradeOption> TradeOptions = new List<TradeOption>();

    //Treasure

    //Shrine

    //Reference
}
