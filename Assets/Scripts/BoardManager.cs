using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; } // Static property to access the instance

    //Player 1 board
    public GameObject P1Board;
    public GameObject P1XPCube;
    public GameObject P1ArmorCube;
    public GameObject P1HPCube;
    public GameObject P1FoodCube;
    public GameObject P1GoldCube;
    public GameObject P1PotionCube1;
    public GameObject P1PotionCube2;

    private List<GameObject> P1XPTrack = new List<GameObject>();
    private List<GameObject> P1ArmorTrack = new List<GameObject>();
    private List<GameObject> P1HPTrack = new List<GameObject>();
    private List<GameObject> P1FoodTrack = new List<GameObject>();
    private List<GameObject> P1GoldTrack = new List<GameObject>();
    private GameObject P1PotionNoneTop;
    private GameObject P1PotionNoneBottom;
    private GameObject P1PotionFire;
    private GameObject P1PotionFrost;
    private GameObject P1PotionPoison;
    private GameObject P1PotionHealing;
    private GameObject P1PotionHoly;
    private GameObject P1PotionPerception;

    //Player 2 board
    public GameObject P2Board;
    public GameObject P2XPCube;
    public GameObject P2ArmorCube;
    public GameObject P2HPCube;
    public GameObject P2FoodCube;
    public GameObject P2GoldCube;
    public GameObject P2PotionCube1;
    public GameObject P2PotionCube2;

    private List<GameObject> P2XPTrack = new List<GameObject>();
    private List<GameObject> P2ArmorTrack = new List<GameObject>();
    private List<GameObject> P2HPTrack = new List<GameObject>();
    private List<GameObject> P2FoodTrack = new List<GameObject>();
    private List<GameObject> P2GoldTrack = new List<GameObject>();
    private GameObject P2PotionNoneTop;
    private GameObject P2PotionNoneBottom;
    private GameObject P2PotionFire;
    private GameObject P2PotionFrost;
    private GameObject P2PotionPoison;
    private GameObject P2PotionHealing;
    private GameObject P2PotionHoly;
    private GameObject P2PotionPerception;

    //Dungeon board
    public GameObject dungeonBoard;
    public GameObject monsterHPCube;
    public GameObject dungeonFloorCube;

    private List<GameObject> dungeonFloorTrack = new List<GameObject>();
    private List<GameObject> towerFloorTrack = new List<GameObject>();
    private List<GameObject> dungeonMonsterHPTrack = new List<GameObject>();
    private List<GameObject> towerMonsterHPTrack = new List<GameObject>();

    private void Awake()
    {
        // Singleton pattern for easy access
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Add each empty gameobject to the appropriate track list
        foreach(Transform child in P1Board.transform)
        {
            if (child.name.Contains("Armor"))
            {
                P1ArmorTrack.Add(child.gameObject);
            }
            else if (child.name.Contains("Food"))
            {
                P1FoodTrack.Add(child.gameObject);
            }
            else if (child.name.Contains("HP"))
            {
                P1HPTrack.Add(child.gameObject);
            }
            else if (child.name.Contains("XP"))
            {
                P1XPTrack.Add(child.gameObject);
            }
            else if (child.name.Contains("Gold"))
            {
                P1GoldTrack.Add(child.gameObject);
            }
            else if (child.name.Contains("Fire"))
            {
                P1PotionFire = child.gameObject;
            }
            else if (child.name.Contains("Frost"))
            {
                P1PotionFrost = child.gameObject;
            }
            else if (child.name.Contains("Poison"))
            {
                P1PotionPoison = child.gameObject;
            }
            else if (child.name.Contains("Healing"))
            {
                P1PotionHealing = child.gameObject;
            }
            else if (child.name.Contains("Holy"))
            {
                P1PotionHoly = child.gameObject;
            }
            else if (child.name.Contains("Perception"))
            {
                P1PotionPerception = child.gameObject;
            }
            else if (child.name.Contains("Top"))
            {
                P1PotionNoneTop = child.gameObject;
            }
            else if (child.name.Contains("Bottom"))
            {
                P1PotionNoneBottom = child.gameObject;
            }
        }

        //If there's 2 players add all p2 board parts
        if (GameManager.Instance.playerCount > 1)
        {
            foreach (Transform child in P2Board.transform)
            {
                if (child.name.Contains("Armor"))
                {
                    P2ArmorTrack.Add(child.gameObject);
                }
                else if (child.name.Contains("Food"))
                {
                    P2FoodTrack.Add(child.gameObject);
                }
                else if (child.name.Contains("HP"))
                {
                    P2HPTrack.Add(child.gameObject);
                }
                else if (child.name.Contains("XP"))
                {
                    P2XPTrack.Add(child.gameObject);
                }
                else if (child.name.Contains("Gold"))
                {
                    P2GoldTrack.Add(child.gameObject);
                }
                else if (child.name.Contains("Fire"))
                {
                    P2PotionFire = child.gameObject;
                }
                else if (child.name.Contains("Frost"))
                {
                    P2PotionFrost = child.gameObject;
                }
                else if (child.name.Contains("Poison"))
                {
                    P2PotionPoison = child.gameObject;
                }
                else if (child.name.Contains("Healing"))
                {
                    P2PotionHealing = child.gameObject;
                }
                else if (child.name.Contains("Holy"))
                {
                    P2PotionHoly = child.gameObject;
                }
                else if (child.name.Contains("Perception"))
                {
                    P2PotionPerception = child.gameObject;
                }
                else if (child.name.Contains("Top"))
                {
                    P2PotionNoneTop = child.gameObject;
                }
                else if (child.name.Contains("Bottom"))
                {
                    P2PotionNoneBottom = child.gameObject;
                }
            }
        }

        foreach (Transform child in dungeonBoard.transform)
        {
            if (child.name.Contains("DungeonFloor"))
            {
                dungeonFloorTrack.Add(child.gameObject);
            }
            else if (child.name.Contains("TowerFloor"))
            {
                towerFloorTrack.Add(child.gameObject);
            }
            else if (child.name.Contains("DungeonHP"))
            {
                dungeonMonsterHPTrack.Add(child.gameObject);
            }
            else if (child.name.Contains("TowerHP"))
            {
                towerMonsterHPTrack.Add(child.gameObject);
            }
        }
    }

    public void UpdatePlayerStat(int playerIndex, Player.stat statToUpdate, int amount)
    {
        switch (statToUpdate)
        {
            case Player.stat.XP:
                break;
        }
    }

    public void SetUpPlayerStats(Player player, int playerIndex)
    {

        //set up XP
        (playerIndex == 1? P1XPCube:P2XPCube).transform.position = (playerIndex == 1 ? P1XPTrack[(player.XP)] : P2XPTrack[(player.XP)]).transform.position;
        (playerIndex == 1 ? P1XPCube : P2XPCube).transform.rotation = (playerIndex == 1 ? P1XPTrack[(player.XP)] : P2XPTrack[(player.XP)]).transform.rotation;

        //set up Armor
        (playerIndex == 1 ? P1ArmorCube : P2ArmorCube).transform.position = (playerIndex == 1 ? P1ArmorTrack[(player.Armor)] : P2ArmorTrack[(player.Armor)]).transform.position;
        (playerIndex == 1 ? P1ArmorCube : P2ArmorCube).transform.rotation = (playerIndex == 1 ? P1ArmorTrack[(player.Armor)] : P2ArmorTrack[(player.Armor)]).transform.rotation;

        //set up HP
        (playerIndex == 1 ? P1HPCube : P2HPCube).transform.position = (playerIndex == 1 ? P1HPTrack[(player.HP)] : P2HPTrack[(player.HP)]).transform.position;
        (playerIndex == 1 ? P1HPCube : P2HPCube).transform.rotation = (playerIndex == 1 ? P1HPTrack[(player.HP)] : P2HPTrack[(player.HP)]).transform.rotation;

        //set up food
        (playerIndex == 1 ? P1FoodCube : P2FoodCube).transform.position = (playerIndex == 1 ? P1FoodTrack[(player.XP)] : P2FoodTrack[(player.XP)]).transform.position;
        (playerIndex == 1 ? P1FoodCube : P2FoodCube).transform.rotation = (playerIndex == 1 ? P1FoodTrack[(player.XP)] : P2FoodTrack[(player.XP)]).transform.rotation;

        //set up gold
        (playerIndex == 1 ? P1GoldCube : P2GoldCube).transform.position = (playerIndex == 1 ? P1GoldTrack[(player.XP)] : P2GoldTrack[(player.XP)]).transform.position;
        (playerIndex == 1 ? P1GoldCube : P2GoldCube).transform.rotation = (playerIndex == 1 ? P1GoldTrack[(player.XP)] : P2GoldTrack[(player.XP)]).transform.rotation;

        //potion 1
        switch (player.potion1)
        {
            case potion.None:
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.position = (playerIndex == 1 ? P1PotionNoneTop : P2PotionNoneTop).transform.position;
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.rotation = (playerIndex == 1 ? P1PotionNoneTop : P2PotionNoneTop).transform.rotation;
                break;
            case potion.Fire:
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.position = (playerIndex == 1 ? P1PotionFire : P2PotionFire).transform.position;
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.rotation = (playerIndex == 1 ? P1PotionFire : P2PotionFire).transform.rotation;
                break;
            case potion.Frost:
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.position = (playerIndex == 1 ? P1PotionFrost : P2PotionFrost).transform.position;
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.rotation = (playerIndex == 1 ? P1PotionFrost : P2PotionFrost).transform.rotation;
                break;
            case potion.Poison:
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.position = (playerIndex == 1 ? P1PotionPoison : P2PotionPoison).transform.position;
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.rotation = (playerIndex == 1 ? P1PotionPoison : P2PotionPoison).transform.rotation;
                break;
            case potion.Healing:
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.position = (playerIndex == 1 ? P1PotionHealing : P2PotionHealing).transform.position;
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.rotation = (playerIndex == 1 ? P1PotionHealing : P2PotionHealing).transform.rotation;
                break;
            case potion.Holy:
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.position = (playerIndex == 1 ? P1PotionHoly : P2PotionHoly).transform.position;
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.rotation = (playerIndex == 1 ? P1PotionHoly : P2PotionHoly).transform.rotation;
                break;
            case potion.Perception:
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.position = (playerIndex == 1 ? P1PotionPerception : P2PotionPerception).transform.position;
                (playerIndex == 1 ? P1PotionCube1 : P2PotionCube1).transform.rotation = (playerIndex == 1 ? P1PotionPerception : P2PotionPerception).transform.rotation;
                break;
        }

        //potion 2
        switch (player.potion2)
        {
            case potion.None:
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.position = (playerIndex == 1 ? P1PotionNoneBottom : P2PotionNoneBottom).transform.position;
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.rotation = (playerIndex == 1 ? P1PotionNoneBottom : P2PotionNoneBottom).transform.rotation;
                break;
            case potion.Fire:
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.position = (playerIndex == 1 ? P1PotionFire : P2PotionFire).transform.position;
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.rotation = (playerIndex == 1 ? P1PotionFire : P2PotionFire).transform.rotation;
                break;
            case potion.Frost:
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.position = (playerIndex == 1 ? P1PotionFrost : P2PotionFrost).transform.position;
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.rotation = (playerIndex == 1 ? P1PotionFrost : P2PotionFrost).transform.rotation;
                break;
            case potion.Poison:
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.position = (playerIndex == 1 ? P1PotionPoison : P2PotionPoison).transform.position;
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.rotation = (playerIndex == 1 ? P1PotionPoison : P2PotionPoison).transform.rotation;
                break;
            case potion.Healing:
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.position = (playerIndex == 1 ? P1PotionHealing : P2PotionHealing).transform.position;
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.rotation = (playerIndex == 1 ? P1PotionHealing : P2PotionHealing).transform.rotation;
                break;
            case potion.Holy:
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.position = (playerIndex == 1 ? P1PotionHoly : P2PotionHoly).transform.position;
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.rotation = (playerIndex == 1 ? P1PotionHoly : P2PotionHoly).transform.rotation;
                break;
            case potion.Perception:
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.position = (playerIndex == 1 ? P1PotionPerception : P2PotionPerception).transform.position;
                (playerIndex == 1 ? P1PotionCube2 : P2PotionCube2).transform.rotation = (playerIndex == 1 ? P1PotionPerception : P2PotionPerception).transform.rotation;
                break;
        }
    }

    public void SetUpDungeonBoard(Options.level level)
    {
        switch (level)
        {
            case Options.level.soloDungeon:
            case Options.level.coopDungeon:
                //Set the board position
                dungeonBoard.transform.localPosition = new Vector3(0.0026f, 0.01624f, 0f);

                //place floor cube
                dungeonFloorCube.transform.position = dungeonFloorTrack[0].transform.position;
                dungeonFloorCube.transform.rotation = dungeonFloorTrack[0].transform.rotation;

                //place monter hp cube
                monsterHPCube.transform.position = dungeonMonsterHPTrack[0].transform.position;
                monsterHPCube.transform.rotation = dungeonMonsterHPTrack[0].transform.rotation;
                break;

            case Options.level.soloTower:
            case Options.level.coopTower:
                dungeonBoard.transform.localPosition = new Vector3(-0.0026f, -0.01372f, 0f);
                //flip the board
                dungeonBoard.transform.Rotate(180, 0, 180);

                //place floor cube
                dungeonFloorCube.transform.position = towerFloorTrack[0].transform.position;
                dungeonFloorCube.transform.rotation = towerFloorTrack[0].transform.rotation;

                //place monter hp cube
                monsterHPCube.transform.position = towerMonsterHPTrack[0].transform.position;
                monsterHPCube.transform.rotation = towerMonsterHPTrack[0].transform.rotation;
                break;
        }

    }
}
