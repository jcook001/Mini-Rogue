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
    DropFloor,
    Combat
}

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

[System.Serializable]
public struct TrapResult
{
    public string[] TrapNames;
    public ResourceType[] FailureResourceType;
    //0 Quantitiy means equals floor
    public int[] FailureQuantity;
    public DamageEffects[] FailureResourceType2;
    public ResourceType[] SuccessResourceType;
    //0 Quantitiy means equals floor
    public int[] SuccessQuantity;
}

[System.Serializable]
public struct TrapResult_Depths
{
    public string[] TrapNames;
    public ResourceType[] FailureResourceType;
    //0 Quantitiy means equals floor
    public int[] FailureQuantity;
    public DamageEffects[] FailureResourceType2;
    public ResourceType[] SuccessResourceType;
    //0 Quantitiy means equals floor
    public int[] SuccessQuantity;
}

[System.Serializable]
public struct TreasureRewards
{
    public int BasicGoldReward;
    public int IncreasedGoldReward;
    public ResourceReward[] Reward;
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
        Trap_Depths,
        Tomb,
        Bonfire,
        Merchant,
        Treasure,
        Treasure_Depths,
        Shrine,
        Reference
    }
    public CardType cardType;
    public GameObject model;

    //Item
    
    //Monster + Boss
    public string[] MonsterName = new string[4];
    public int[] OnePlayerHealth = new int[4];
    public int[] TwoPlayerHealth = new int[4];
    public int[] DamageValue = new int[4];
    public DamageEffects[] DamageEffect = new DamageEffects[4];
    public DamageEffects[] DamageEffect2 = new DamageEffects[4];
    public int[] RewardValue = new int[4];
    public ResourceType[] RewardEffect = new ResourceType[4];

    //Trap
    public TrapResult TrapResult = new TrapResult
    {
        TrapNames = new string[3],
        FailureResourceType = new ResourceType[3],
        FailureQuantity = new int[3],
        FailureResourceType2 = new DamageEffects[3],
        SuccessResourceType = new ResourceType[3],
        SuccessQuantity = new int[3]
    };

    //Trap_Depths
    public TrapResult_Depths TrapResult_Depths = new TrapResult_Depths
    {
        TrapNames = new string[4],
        FailureResourceType = new ResourceType[4],
        FailureQuantity = new int[4],
        FailureResourceType2 = new DamageEffects[4],
        SuccessResourceType = new ResourceType[4],
        SuccessQuantity = new int[4]
    };

    //Tomb

    //Bonfire

    //Merchant
    public List<TradeOption> TradeOptions = new List<TradeOption>();

    //Treasure
    public TreasureRewards TreasureRewards = new TreasureRewards
    {
        Reward = new ResourceReward[6]
    };

    //Shrine

    //Reference
}
