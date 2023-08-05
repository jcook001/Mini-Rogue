using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    private GameObject options_Manager;
    private Options options;

    public float cardThickness = 50.0f;
    public float piecePlacementHeight = 0.29f;
    public bool DebugShowDungeonCards = false;
    public GameObject[] CardPoints = new GameObject[9];
    public GameObject[] DungeonCardsTest = new GameObject[1]; //TODO remove?
    public CardData placeholderCard;
    public List<CardData> DungeonDeck = new List<CardData>();
    private List<CardData> UsedDungeonDeck = new List<CardData>();
    public List<CardData> BossDeck = new List<CardData>();
    private List<CardData> UsedBossDeck = new List<CardData>();
    public CardData FinalBoss;

    public TextMeshProUGUI debugOverlay;
    public TextMeshProUGUI debugGamePrompts;
    public TMP_FontAsset fontAsset; //Debug

    private Player P1 = new Player();
    private Player P2 = new Player();

    private int playerCount = 1;
    public GameObject P1_Piece; //TODO make player choose their piece so this can be private 
    public GameObject P2_Piece; //TODO make player choose their piece so this can be private 
    private int P1_Location = 1;
    private int P2_Location = 1;
    private int playerTurn = 1;
    private GameObject activePiece;

    public float moveSpeed = 1f;
    public float moveHeight = 1f;
    private bool isPieceMoving = false;

    /*Icon Map
     * a = Armor
     * b = Ignore Armor
     * c = Curse
     * e = Player Dice 3
     * f = Food
     * g = Gold
     * h = HP/Health
     * i = Merchant
     * k = Perception Potion
     * l = Blindness
     * n = Heal 3
     * p = Poison
     * q = Player Dice (1) Miss
     * r = Player Dice 4
     * s = Skill Check
     * t = Player Dice 5 (crit)
     * v = Nothing happens
     * w = Player Dice 2
     * x = Feat
     * y = Player Dice 6 (crit)
     * B = Blessing
     * C = Curse
     * D = Dungeon Dice
     * F = Fall/Drop
     * G = Circle?
     * H = Health Potion
     * I = Item
     * J = Square border?
     * K = Refresh Skills
     * L = Lore card
     * N = Heal 2
     * P = Poison potion
     * Q = Heal 1
     * R = Perk Point
     * V = Tick
     * W = Weaken
     * X = Exchange
     * Y = Heal 4
     * ( = Crossed Swords (XP)
     * ) = Potion
     * = = Room Card (Door)
     * _ = Floor
     * 1 = Enemy Dice 1
     * 2 = Enemy Dice 2
     * 3 = Enemy Dice 3
     * 4 = Enemy Dice 4
     * 5 = Enemy Dice 5
     * 6 = Enemy Dice 6
     * 7 = Fire potion
     * 8 = Ice Potion
     * 9 = Holy Potion
     * 0 = Dice Blank
    */

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        //Limit the framrate in Unity Editor so that GPU usage is reduced
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Application.targetFrameRate = 60;
        }
#endif

        //Get the options from the Main Menu
        //if there's no options manager to be found just use default values
        options_Manager = GameObject.Find("Options_Manager");
        if (options_Manager)
        {
            GameObject.Find("Options_Manager").TryGetComponent<Options>(out options);

            //Set P1 class
            P1.SetClass(options.characters[options.P1CharacterChoice]);
        }
        else
        {
            //set a sensible default
            P1.SetClass("Crusader");
        }

        //Place a card in each card slot 1-8
        DealCards();

        //Create a boss deck in slot 9
        CreateBossPile();

        //Show character stats in debug overlay
        debugOverlay.text =
            "Chosen Character: " + P1.Character + "\n" +
            "<#656868><font=\"Icons SDF\">a</font> Armor: </color>" + P1.Armor + "\n" +
            "<#c84d4a><font=\"Icons SDF\">h</font> HP: </color>" + P1.HP + "\n" +
            "<#2a737c><font=\"Icons SDF\">(</font> XP: </color>" + P1.XP + "\n" +
            "<#7d5641><font=\"Icons SDF\">f</font> Food: </color>" + P1.Food + "\n" +
            "<#ce982c><font=\"Icons SDF\">g</font> Gold: </color>" + P1.Gold + "\n" +
            "<#231f20><font=\"Icons SDF\">)</font> Potion 1: </color>" + P1.potion1 + "\n" +
            "<#231f20><font=\"Icons SDF\">)</font> Potion 2: </color>" + P1.potion2 + "\n" +
            "<#00bc62><font=\"Icons SDF\">p</font> Poisoned: </color>" + P1.poisoned + "\n" +
            "<#6600bf><font=\"Icons SDF\">c</font> Cursed: </color>" + P1.cursed + "\n";

        //Place the player pieces on the first card
        debugGamePrompts.text = "Select a location to place your player token";

        //TODO Assign this properly in UI
        activePiece = P1_Piece;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DealCards()
    {
        for(int i = 0; i < CardPoints.Length - 1; i++)
        {
            //select a random card to deal
            int randomCardInt = UnityEngine.Random.Range(0, DungeonDeck.Count - 1);
            //create the card from the model specified in CardData
            GameObject newDungeonCard;
            if (DungeonDeck[randomCardInt].model == null)
            {
                newDungeonCard = Instantiate(placeholderCard.model, CardPoints[i].transform);
            }
            else
            {
                newDungeonCard = Instantiate(DungeonDeck[randomCardInt].model, CardPoints[i].transform);
            }

            if (DebugShowDungeonCards)
            {
                newDungeonCard.transform.rotation = Quaternion.Euler(180, 180, 0);
            }
            else
            {
                newDungeonCard.transform.rotation = Quaternion.Euler(180, 180, 180);
            }

            newDungeonCard.transform.localScale = new Vector3(25, cardThickness, 25);

            //Add that card to the used cards list
            UsedDungeonDeck.Add(DungeonDeck[randomCardInt]);
            //remove that card from the deck
            DungeonDeck.RemoveAt(randomCardInt);

            //add components so the card can be interacted with
            newDungeonCard.AddComponent<CardAnims>();
            newDungeonCard.AddComponent<BoxCollider>();
            newDungeonCard.GetComponent<BoxCollider>().size = new Vector3(0.05f, 0.0003f, 0.07f);
        }
    }

    private void CreateBossPile()
    {
        GameObject pileCard;
        if (FinalBoss.model == null)
        {
            pileCard = Instantiate(placeholderCard.model, CardPoints[8].transform);
        }
        else
        {
            pileCard = Instantiate(BossDeck[0].model, CardPoints[8].transform);
        }

        pileCard.transform.rotation = Quaternion.Euler(180, 180, 180);
        
        pileCard.transform.localScale = new Vector3(25, cardThickness * (4 - UsedBossDeck.Count), 25);
        
        pileCard.AddComponent<CardAnims>();
        pileCard.AddComponent<BoxCollider>();
        pileCard.GetComponent<BoxCollider>().size = new Vector3(0.05f, 0.0003f, 0.07f);
    }

    public void MoveActivePieceToCard(GameObject card)
    {
        if (!isPieceMoving)
        {
            StartCoroutine(SmoothMoveRandomPos(activePiece, card));

            //update the active player turn
            if(playerTurn == 1)
            {
                playerTurn = 2;
                activePiece = P2_Piece;
            }
            else
            {
                playerTurn = 1;
                activePiece = P1_Piece;
            }
        }
    }

    private IEnumerator SmoothMove(GameObject piece, GameObject target)
    {
        //increase the height of the target so the object doesn't fall through.
        Vector3 targetPosition = target.transform.position + new Vector3(0, 0.2f, 0);
        isPieceMoving = true;
        float journey = 0f;

        Vector3 startPosition = piece.transform.position;

        while (journey <= 1f)
        {
            journey += Time.deltaTime * moveSpeed;

            // Here you can apply an easing function for non-linear movement
            float curve = Mathf.Sin(journey * Mathf.PI / 2); // Example of Ease-Out function

            // Determine the current position along the path
            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, curve);

            // Calculate the height of the arc at this point in the journey
            float arc = moveHeight * Mathf.Sin(journey * Mathf.PI);

            // Apply the arc height to the current position
            currentPos.y += arc;

            piece.transform.position = currentPos;

            yield return null;
        }

        // Ensure the piece ends exactly at the target
        piece.transform.position = targetPosition;
        isPieceMoving = false;
    }

    private IEnumerator SmoothMoveRandomPos(GameObject piece, GameObject card)
    {
        BoxCollider collider = card.GetComponent<BoxCollider>();
        Vector3 size = collider.size;
        float offsetMod = 15.0f;
        float randomX = UnityEngine.Random.Range((-size.x * offsetMod) / 2, (size.x *offsetMod) / 2);
        float randomZ = UnityEngine.Random.Range((-size.z * offsetMod) / 2, (size.z * offsetMod) / 2);

        //increase the height of the target so the object doesn't fall through and offset the card from the centre by a random amount
        Vector3 targetPosition = card.transform.position + new Vector3(randomX, piecePlacementHeight, randomZ);
        isPieceMoving = true;
        float journey = 0f;

        Vector3 startPosition = piece.transform.position;

        Quaternion startRotation = piece.transform.rotation;

        //Stand the piece upright and randomly change the direction it's facing slightly
        Vector3 newRotation = new Vector3(0,UnityEngine.Random.Range(startRotation.y -60 ,startRotation.y + 60),0);

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

        // Ensure the piece ends exactly at the target
        piece.transform.position = targetPosition;
        piece.transform.rotation = Quaternion.Euler(newRotation);
        isPieceMoving = false;
    }

    private void InitialFloorSetup()
    {
        //Shuffle Dungeon Cards

        //Deal 8 Cards into slots 1-8 face down

        //Shuffle and instantiate Monster Deck face down + increase Y size to match height * cards

        //Set Current floor and room to 1
    }

    //Debug functions
    string PrintAllChars()
    {
        string allCharacters = "";

        // Iterate over all characters in the font asset.
        for (int i = 0; i < fontAsset.characterTable.Count; i++)
        {
            // Get the Unicode representation of the character.
            uint unicode = fontAsset.characterTable[i].unicode;

            // Convert the Unicode to a string.
            string characterString = char.ConvertFromUtf32((int)unicode);

            allCharacters += characterString;
        }

        allCharacters += " \n<font=\"Icons SDF\">";

        for (int i = 0; i < fontAsset.characterTable.Count; i++)
        {
            // Get the Unicode representation of the character.
            uint unicode = fontAsset.characterTable[i].unicode;

            // Convert the Unicode to a string.
            string characterString = char.ConvertFromUtf32((int)unicode);

            allCharacters += characterString;
        }
        
        return allCharacters;
    }
}
