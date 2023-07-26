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
    Combat,
    Special,
    Curse,
    Poison,
    Blindness,
    IgnoreArmor,
    Weaken,
    Regeneration,
    Lore,
    SavedFeat,
    SavedCurePoison,
    SavedCureCurse,
    Uncurable,
    Success,
    RewardsCard,
    TakeCard
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
    public List<int> ChoiceQuantity;
    public List<ResourceType> SecondaryEffect;
}

[System.Serializable]
public struct TollResourceCost
{
    //0 Quantitiy means equals floor
    public ResourceType Resource1;
    public int Quantity1;
    public ResourceType Resource2;
    public int Quantity2;
}

[System.Serializable]
public struct TrapResult
{
    public string[] TrapNames;
    public ResourceType[] FailureResourceType;
    //0 Quantitiy means equals floor
    public int[] FailureQuantity;
    public ResourceType[] FailureResourceType2;
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
    public ResourceType[] FailureResourceType2;
    public ResourceType[] SuccessResourceType;
    //0 Quantitiy means equals floor
    public int[] SuccessQuantity;
}

[System.Serializable]
public struct TombRewards
{
    public bool RestoreToTenHealth;
    public string[] EventNames;
    public ResourceReward[] Reward;
}

[System.Serializable]
public struct BonfireRewards
{
    public string[] EventNames;
    public ResourceReward[] Reward;
}

[System.Serializable]
public struct TreasureRewards
{
    public int BasicGoldReward;
    public int IncreasedGoldReward;
    public ResourceReward[] Reward;
}

[System.Serializable]
public struct MonsterBanditStats
{
    public TollResourceCost[] Toll;
    public int[] OnePlayerHealth;
    public int[] TwoPlayerHealth;
    public int[] DamageValue;
    public ResourceType[] DamageEffect;
    public ResourceReward[] Reward;
}

[System.Serializable]
public struct ItemStats
{
    public ResourceCost KeepCost;
    public ResourceCost LeaveCost;
    public ResourceReward[] Reward;
}

[System.Serializable]
public struct MonsterStats
{
    public string[] MonsterName;
    public int[] OnePlayerHealth;
    public int[] TwoPlayerHealth;
    public int[] DamageValue;
    public ResourceType[] DamageEffect;
    public ResourceType[] DamageEffect2;
    public int[] RewardValue;
    public ResourceType[] RewardEffect;
}

[System.Serializable]
public struct BossStats
{
    public int[] OnePlayerHealth;
    public int[] TwoPlayerHealth;
    public int[] DamageValue;
    public ResourceType[] DamageEffect;
    public int[] DamageEffectValue;
    public ResourceType[] DamageEffect2;
    public ResourceType[] RewardEffect;
    public int[] RewardValue;
    public ResourceType[] RewardEffect2;
}

[System.Serializable]
public struct BossFinalStats
{
    public int[] OnePlayerHealth;
    public int[] TwoPlayerHealth;
    public int[] DamageValue;
    public ResourceType[] DamageEffect;
    public int[] DamageEffectValue;
    public ResourceType[] DamageEffect2;
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
        Reference,
        Monster_Bandit,
        Lore,
        Boss_Final
    }
    public CardType cardType;
    public GameObject model;

    //Item
    public ItemStats ItemStats = new ItemStats
    {
        Reward = new ResourceReward[6]
    };

    //Monster
    public MonsterStats MonsterStats = new MonsterStats
    {
        MonsterName = new string[4],
        OnePlayerHealth = new int[4],
        TwoPlayerHealth = new int[4],
        DamageValue = new int[4],
        DamageEffect = new ResourceType[4],
        DamageEffect2 = new ResourceType[4],
        RewardValue = new int[4],
        RewardEffect = new ResourceType[4]
    };

    //Boss
    public BossStats BossStats = new BossStats
    {
        OnePlayerHealth = new int[3],
        TwoPlayerHealth = new int[3],
        DamageValue = new int[3],
        DamageEffect = new ResourceType[3],
        DamageEffectValue = new int[3],
        DamageEffect2 = new ResourceType[3],
        RewardEffect = new ResourceType[3],
        RewardValue = new int[3],
        RewardEffect2 = new ResourceType[3]
    };

    //Trap
    public TrapResult TrapResult = new TrapResult
    {
        TrapNames = new string[3],
        FailureResourceType = new ResourceType[3],
        FailureQuantity = new int[3],
        FailureResourceType2 = new ResourceType[3],
        SuccessResourceType = new ResourceType[3],
        SuccessQuantity = new int[3]
    };

    //Trap_Depths
    public TrapResult_Depths TrapResult_Depths = new TrapResult_Depths
    {
        TrapNames = new string[4],
        FailureResourceType = new ResourceType[4],
        FailureQuantity = new int[4],
        FailureResourceType2 = new ResourceType[4],
        SuccessResourceType = new ResourceType[4],
        SuccessQuantity = new int[4]
    };

    ////Shrine + Tomb
    public TombRewards TombRewards = new TombRewards
    {
        RestoreToTenHealth = false,
        EventNames = new string[6],
        Reward = new ResourceReward[6]
    };

    //Bonfire
    public BonfireRewards BonfireRewards = new BonfireRewards
    {
        EventNames = new string[3],
        Reward = new ResourceReward[3]
    };

    //Merchant
    public List<TradeOption> TradeOptions = new List<TradeOption>();

    //Treasure
    public TreasureRewards TreasureRewards = new TreasureRewards
    {
        Reward = new ResourceReward[6]
    };

    //Reference

    //Bandit
    public MonsterBanditStats MonsterBanditStats = new MonsterBanditStats
    {
        Toll = new TollResourceCost[4],
        OnePlayerHealth = new int[4],
        TwoPlayerHealth = new int[4],
        DamageValue = new int[4],
        DamageEffect = new ResourceType[4],
        Reward = new ResourceReward[4]
    };

    //Boss_Final
    public BossFinalStats BossFinalStats = new BossFinalStats
    {
        OnePlayerHealth = new int[2],
        TwoPlayerHealth = new int[2],
        DamageValue = new int[2],
        DamageEffect = new ResourceType[2],
        DamageEffectValue = new int[2],
        DamageEffect2 = new ResourceType[2]
    };

}
