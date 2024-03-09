using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static Player;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; } // Static property to access the instance

    //Player 1 board
    public GameObject P1Board;
    public GameObject P1CharacterCard;
    public GameObject P1StanceCard;
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
    public GameObject P2CharacterCard;
    public GameObject P2StanceCard;
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
        if (Options.Instance.playerCount > 1)
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

    private IEnumerator MovePiece(GameObject piece, GameObject target)
    {
        piece.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        float moveSpeed = 1;
        float moveHeight = 1;

        //increase the height of the target so the object doesn't fall through and offset the card from the centre by a random amount
        Vector3 targetPosition = target.transform.position + new Vector3(0f, 0.29f, 0f);
        float journey = 0f;

        Vector3 startPosition = piece.transform.position;

        Quaternion startRotation = piece.transform.rotation;

        //Stand the piece upright and randomly change the direction it's facing slightly
        Vector3 newRotation = target.transform.rotation.eulerAngles;

        while (journey <= 1f)
        {
            journey += Time.deltaTime * moveSpeed;

            // Apply an easing function for non-linear movement
            float curve = Mathf.Sin(journey * Mathf.PI / 2); // Example of Ease-Out function

            // Determine the current position along the path
            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, curve);

            // Determine the current rotation along the path
            Quaternion currentRot = Quaternion.Slerp(startRotation, Quaternion.Euler(newRotation), curve);

            // Calculate the height of the arc at this point in the journey
            float arc = moveHeight * Mathf.Sin(journey * Mathf.PI);

            // Apply the arc height to the current position
            currentPos.y += arc;

            piece.transform.position = currentPos;
            piece.transform.rotation = currentRot;

            yield return null;
        }

        //Prevent piece falling out of it's hole in the board
        piece.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        piece.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        piece.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        // Ensure the piece ends exactly at the target
        piece.transform.position = targetPosition;
        piece.transform.rotation = Quaternion.Euler(newRotation);
        yield return null;
    }

    public void SetUpPlayerStats(Player player, int playerIndex)
    {
        //set up XP
        StartCoroutine(MovePiece((playerIndex == 1 ? P1XPCube : P2XPCube), (playerIndex == 1 ? P1XPTrack[(player.XP)] : P2XPTrack[(player.XP)])));

        //set up Armor
        StartCoroutine(MovePiece((playerIndex == 1 ? P1ArmorCube : P2ArmorCube), (playerIndex == 1 ? P1ArmorTrack[(player.Armor)] : P2ArmorTrack[(player.Armor)])));

        //set up HP
        StartCoroutine(MovePiece((playerIndex == 1 ? P1HPCube : P2HPCube), (playerIndex == 1 ? P1HPTrack[(player.HP)] : P2HPTrack[(player.HP)])));

        //set up food
        StartCoroutine(MovePiece((playerIndex == 1 ? P1FoodCube : P2FoodCube), (playerIndex == 1 ? P1FoodTrack[(player.Food)] : P2FoodTrack[(player.Food)])));

        //set up gold
        StartCoroutine(MovePiece((playerIndex == 1 ? P1GoldCube : P2GoldCube), (playerIndex == 1 ? P1GoldTrack[(player.Gold)] : P2GoldTrack[(player.Gold)])));

        //potion 1
        switch (player.potion1)
        {
            case potion.None:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube1 : P2PotionCube1), (playerIndex == 1 ? P1PotionNoneTop : P2PotionNoneTop)));
                break;
            case potion.Fire:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube1 : P2PotionCube1), (playerIndex == 1 ? P1PotionFire : P2PotionFire)));
                break;
            case potion.Frost:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube1 : P2PotionCube1), (playerIndex == 1 ? P1PotionFrost : P2PotionFrost)));
                break;
            case potion.Poison:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube1 : P2PotionCube1), (playerIndex == 1 ? P1PotionPoison : P2PotionPoison)));
                break;
            case potion.Healing:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube1 : P2PotionCube1), (playerIndex == 1 ? P1PotionHealing : P2PotionHealing)));
                break;
            case potion.Holy:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube1 : P2PotionCube1), (playerIndex == 1 ? P1PotionHoly : P2PotionHoly)));
                break;
            case potion.Perception:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube1 : P2PotionCube1), (playerIndex == 1 ? P1PotionPerception : P2PotionPerception)));
                break;
        }

        //potion 2
        StartCoroutine(MovePiece((playerIndex == 1 ? P1XPCube : P2XPCube), (playerIndex == 1 ? P1XPTrack[(player.XP)] : P2XPTrack[(player.XP)])));
        switch (player.potion2)
        {
            case potion.None:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube2 : P2PotionCube2), (playerIndex == 1 ? P1PotionNoneBottom : P2PotionNoneBottom)));
                break;
            case potion.Fire:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube2 : P2PotionCube2), (playerIndex == 1 ? P1PotionFire : P2PotionFire)));
                break;
            case potion.Frost:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube2 : P2PotionCube2), (playerIndex == 1 ? P1PotionFrost : P2PotionFrost)));
                break;
            case potion.Poison:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube2 : P2PotionCube2), (playerIndex == 1 ? P1PotionPoison : P2PotionPoison)));
                break;
            case potion.Healing:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube2 : P2PotionCube2), (playerIndex == 1 ? P1PotionHealing : P2PotionHealing)));
                break;
            case potion.Holy:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube2 : P2PotionCube2), (playerIndex == 1 ? P1PotionHoly : P2PotionHoly)));
                break;
            case potion.Perception:
                StartCoroutine(MovePiece((playerIndex == 1 ? P1PotionCube2 : P2PotionCube2), (playerIndex == 1 ? P1PotionPerception : P2PotionPerception)));
                break;
        }
    }

    public void SetUpDungeonBoard(Options.level level)
    {
        switch (level)
        {
            case Options.level.Dungeon:
                //Set the board position
                dungeonBoard.transform.localPosition = new Vector3(0.0026f, 0.01624f, 0f);

                //place floor cube
                StartCoroutine(MovePiece(dungeonFloorCube, dungeonFloorTrack[0]));

                //place monter hp cube
                StartCoroutine(MovePiece(monsterHPCube, dungeonMonsterHPTrack[0]));
                break;

            case Options.level.Tower:
                dungeonBoard.transform.localPosition = new Vector3(-0.0026f, -0.01372f, 0f);
                //flip the board
                dungeonBoard.transform.Rotate(180, 0, 180);

                //place floor cube
                StartCoroutine(MovePiece(dungeonFloorCube, towerFloorTrack[0]));

                //place monter hp cube
                StartCoroutine(MovePiece(monsterHPCube, towerMonsterHPTrack[0]));
                break;
        }

    }

    public void SetUpPlayerBoards(Options.gameType gameType)
    {
        switch (gameType)
        {
            case Options.gameType.Standard:
                Destroy(P2Board);
                Destroy(P2StanceCard);
                P1StanceCard.transform.position = P2CharacterCard.transform.position;
                Destroy(P2CharacterCard);
                Destroy(P2XPCube);
                Destroy(P2ArmorCube);
                Destroy(P2HPCube);
                Destroy(P2FoodCube);
                Destroy(P2GoldCube);
                Destroy(P2PotionCube1);
                Destroy(P2PotionCube2);
                break;
            case Options.gameType.Coop:
            case Options.gameType.Competitive:
                P2Board.transform.localPosition = new Vector3(-3.74f, 0, 2.53f);
                break;
            case Options.gameType.Campaign:
                //flip the board
                P2Board.transform.rotation = Quaternion.Euler(new Vector3(-180, 0, 0));
                P2Board.transform.localPosition = new Vector3(-3.74f, -0.021f, 2.53f);
                Destroy(P2StanceCard);
                P1StanceCard.transform.position = P2CharacterCard.transform.position;
                Destroy(P2CharacterCard);
                Destroy(P2XPCube);
                Destroy(P2ArmorCube);
                Destroy(P2HPCube);
                Destroy(P2FoodCube);
                Destroy(P2GoldCube);
                Destroy(P2PotionCube1);
                Destroy(P2PotionCube2);
                break;
        }


    }

    public void SetUpCharacterCards(int playerCount)
    {
        //Always set up player 1 card
        Vector3 scale = P1CharacterCard.transform.localScale;
        Transform parent = P1CharacterCard.transform.parent;
        Vector3 position = P1CharacterCard.transform.position;
        Quaternion rotation = P1CharacterCard.transform.rotation;
        GameObject newCharacterCard = Instantiate(Options.Instance.characterCards[Options.Instance.P1CharacterChoice], position, rotation, parent);
        newCharacterCard.transform.localScale = scale;
        Destroy(P1CharacterCard);
        P1CharacterCard = newCharacterCard;

        //If 2 players set up player 2 card
        if (playerCount == 2)
        {
            scale = P2CharacterCard.transform.localScale;
            parent = P2CharacterCard.transform.parent;
            position = P2CharacterCard.transform.position;
            rotation = P2CharacterCard.transform.rotation;
            newCharacterCard = Instantiate(Options.Instance.characterCards[Options.Instance.P2CharacterChoice], position, rotation, parent);
            newCharacterCard.transform.localScale = scale;
            Destroy(P2CharacterCard);
            P2CharacterCard = newCharacterCard;
        }
        else
        {
            //or destroy player 2 card
            Destroy(P2CharacterCard);
        }
            
    }
}
