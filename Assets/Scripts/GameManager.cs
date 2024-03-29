using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

//TODO re-enable this later
#pragma warning disable 0414 //disables unused variable warning

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Static property to access the instance

    //Game pieces
    public GameObject P1_Piece; //TODO make player choose their piece so this can be private 
    public GameObject P2_Piece; //TODO make player choose their piece so this can be private 
    public GameObject monsterBoard;
    public GameObject[] player1PlayerDice;
    public GameObject player1MonsterDice;
    public GameObject player1PoisonDice;
    public GameObject player1CurseDice;
    public GameObject[] player2PlayerDice;
    public GameObject player2MonsterDice;
    public GameObject player2PoisonDice;
    public GameObject player2CurseDice;

    public float cardThickness = 50.0f;
    public float piecePlacementHeight = 0.29f;
    public GameObject[] cardPoints = new GameObject[9];
    public CardData placeholderCard;
    public List<CardData> dungeonDeck = new List<CardData>();
    private List<CardData> usedDungeonDeck = new List<CardData>();
    public List<CardData> bossDeck = new List<CardData>();
    private List<CardData> usedBossDeck = new List<CardData>();
    public CardData finalBoss;

    private Player P1 = new Player();
    private Player P2 = new Player();
    private Player activePlayer;

    public float moveSpeed = 1f;
    public float moveHeight = 1f;
    private bool isPieceMoving = false;

    public bool isAnyCardZoomed = false;
    public bool isAnyCardZooming = false;
    public bool isAnyCardFlipping = false;
    private Quaternion faceUpCardRotation = Quaternion.Euler(180, 180, 0);
    private Quaternion faceDownCardRotation = Quaternion.Euler(180, 180, 180);
    private GameObject zoomedcardParent = null;
    public GameObject zoomedCard = null;

    //Gameplay
    private bool hasGameStarted = false;
    private int currentFloor = 0;
    private int currentRoom = 0;
    private int activePlayerIndex = 1; //starts from 1
    private GameObject activePiece;
    private int P1_Location = 0;
    private int P2_Location = 0;

    //animations and transitions
    WaitForSeconds PieceMoveDelay = new WaitForSeconds(1);

    public delegate void DieRollComplete(int value, Die.DieType diceType);
    public event DieRollComplete OnDieRollComplete;
    List<GameObject> diceToRoll = new List<GameObject>();
    private int diceRolled = 0;
    private List<int> rollResults = new List<int>();
    private List<Die.DieType> rollResultsDieType = new List<Die.DieType>();
    private bool awaitingDieChoice = true;
    private int monsterDieRollResult = 0;

    //DEBUG
    public bool DebugShowPlayerStats = false;
    public bool DebugShowDungeonCards = false;
    public bool DebugShowFontSymbols = false;
    public bool DebugRollAllDice = false;
    public bool DebugOverrideDungeonCards;
    [HideInInspector]
    public List<CardData> allCardData; // List to hold all CardData objects
    [HideInInspector]
    public int selectedCardIndex = 0; // Index of the selected card

    public TextMeshProUGUI debugOverlay;
    public TextMeshProUGUI debugGamePrompts;
    public TextMeshProUGUI floorStatusPrompts;
    public TMP_FontAsset fontAsset;

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
#if UNITY_EDITOR
        //Limit the framerate in Unity Editor so that GPU usage is reduced
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Application.targetFrameRate = 60;
        }
#endif

        //Get the options from the Main Menu
        //if there's no options manager to be found just use default values

        //Set P1 class
        P1.SetClass(Options.Instance.characters[Options.Instance.P1CharacterChoice]);

        if (Options.Instance.playerCount == 2)
        {
            P2.SetClass(Options.Instance.characters[Options.Instance.P2CharacterChoice]);
        }

        if (DebugShowPlayerStats)
        {
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
                "<#00bc62><font=\"Icons SDF\">p</font> Poisoned: </color>" + P1.isPoisoned + "\n" +
                "<#6600bf><font=\"Icons SDF\">c</font> Cursed: </color>" + P1.isCursed + "\n";
        }
        else
        {
            debugOverlay.enabled = false;
        }

        if (DebugShowFontSymbols)
        {
            debugOverlay.text = PrintAllChars();
        }

        debugGamePrompts.text = "Press the start game button to begin!";

        //TODO Assign player piece properly in UI
        activePiece = P1_Piece;

        //Give all dice a type
        AssignDiceTypes();

        // Subscribe to the dice roll event
        OnDieRollComplete += HandleDieRollComplete;

        //subscribe to die click event
        //OnDieResultClicked += HandleDieClicked;

        //Set up chosen character cards
        BoardManager.Instance.SetUpCharacterCards(Options.Instance.playerCount);

        BoardManager.Instance.SetUpPlayerBoards(Options.Instance.chosenGameType);

    }

    private void AssignDiceTypes()
    {
        foreach(GameObject die in player1PlayerDice)
        {
            Die DieComponent = die.GetComponent<Die>();
            DieComponent.type = Die.DieType.Player;
        }

        player1MonsterDice.GetComponent<Die>().type = Die.DieType.Monster;
        player1PoisonDice.GetComponent<Die>().type = Die.DieType.Poison;
        player1CurseDice.GetComponent<Die>().type = Die.DieType.Curse;

        foreach (GameObject die in player2PlayerDice)
        {
            Die DieComponent = die.GetComponent<Die>();
            DieComponent.type = Die.DieType.Player;
        }

        player2MonsterDice.GetComponent<Die>().type = Die.DieType.Monster;
        player2PoisonDice.GetComponent<Die>().type = Die.DieType.Poison;
        player2CurseDice.GetComponent<Die>().type = Die.DieType.Curse;
    }

    private void DealCards()
    {
        for(int i = 0; i < cardPoints.Length - 1; i++)
        {
            //select a random card to deal
            int randomCardInt = UnityEngine.Random.Range(0, dungeonDeck.Count - 1);
            //create the card from the model specified in CardData
            GameObject newDungeonCard;
            if (dungeonDeck[randomCardInt].model == null)
            {
                newDungeonCard = Instantiate(placeholderCard.model, cardPoints[i].transform);
                Debug.LogError("there's no model for " + dungeonDeck[randomCardInt].model.name);
            }
            else if (DebugOverrideDungeonCards)
            {
                newDungeonCard = Instantiate(allCardData[selectedCardIndex].model, cardPoints[i].transform);
                newDungeonCard.AddComponent<Card>();
                newDungeonCard.GetComponent<Card>().cardData = allCardData[selectedCardIndex];
            }
            else
            {
                newDungeonCard = Instantiate(dungeonDeck[randomCardInt].model, cardPoints[i].transform);
                newDungeonCard.AddComponent<Card>();
                newDungeonCard.GetComponent<Card>().cardData = dungeonDeck[randomCardInt];
            }

            newDungeonCard.AddComponent<CardAnims>();

            if (DebugShowDungeonCards)
            {
                newDungeonCard.transform.rotation = faceUpCardRotation;
                newDungeonCard.GetComponent<CardAnims>().isFaceUp = true;
            }
            else
            {
                newDungeonCard.transform.rotation = faceDownCardRotation;
            }

            newDungeonCard.transform.localScale = new Vector3(25, cardThickness, 25);

            //Add that card to the used cards list
            usedDungeonDeck.Add(dungeonDeck[randomCardInt]);
            //remove that card from the deck
            dungeonDeck.RemoveAt(randomCardInt);

            //add components so the card can be interacted with
            newDungeonCard.AddComponent<BoxCollider>();
            newDungeonCard.GetComponent<BoxCollider>().size = new Vector3(0.05f, 0.0003f, 0.07f);
            newDungeonCard.AddComponent<Rigidbody>();
            newDungeonCard.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            newDungeonCard.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

            // Add a Canvas to the new card
            GameObject canvasObject = new GameObject("CardCanvas");
            canvasObject.transform.SetParent(newDungeonCard.transform, false);
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            MeshRenderer cardRenderer = newDungeonCard.GetComponent<MeshRenderer>();
            // Calculate the size of the card taking into account its local scale
            Vector3 cardLocalScale = newDungeonCard.transform.localScale;
            Vector3 cardSize = new Vector3(
                cardRenderer.bounds.size.x / cardLocalScale.x,
                cardRenderer.bounds.size.y / cardLocalScale.y,
                cardRenderer.bounds.size.z / cardLocalScale.z );

            RectTransform rectTransform = canvasObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(cardSize.x, cardSize.z); // Set the size of the canvas to match the card's size
            rectTransform.localScale = new Vector3(
                1f / cardLocalScale.x,
                1f / cardLocalScale.z,
                1f );

            // Calculate a small offset to raise the canvas just above the card surface
            float heightOffset = cardSize.y * -0.5f - 0.0001f; // Raise by half the card's height plus a small additional amount

            // Now set the local position of the canvas
            rectTransform.localPosition = new Vector3(0, heightOffset, 0);
            rectTransform.localEulerAngles = new Vector3(-90, 0, 0); // Adjust as needed
            rectTransform.localScale = Vector3.one;

            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 10; // Adjust this to match the actual size of your card

            GraphicRaycaster raycaster = canvasObject.AddComponent<GraphicRaycaster>();

            //for each child object
            foreach (Transform child in newDungeonCard.transform)
            {
                if (child.name.StartsWith("CardType"))
                {
                    CreateCardButton(0.20f, 0.20f, child, canvasObject, cardLocalScale, newDungeonCard);

                }
                else if (child.name.StartsWith("SubCardType"))
                {
                    CreateCardButton(0.3f, 0.1f, child, canvasObject, cardLocalScale, newDungeonCard);

                }
                else if (child.name.StartsWith("Option"))
                {
                    CreateCardButton(0.4f, 0.087f, child, canvasObject, cardLocalScale, newDungeonCard);

                }
                else if (child.name.StartsWith("CardCanvas"))
                {

                }
                else
                {
                    Debug.LogError("Unknown object " + child.name + " on card " + newDungeonCard.name);
                }
            }

            int layerCard = LayerMask.NameToLayer("Card");
            newDungeonCard.layer = layerCard;
        }
    }

    //TODO rename variables here so it's more readable out of context
    private void CreateCardButton(float width, float height, Transform child, GameObject canvasObject, Vector3 cardLocalScale, GameObject newDungeonCard)
    {
        //Add a Button to the Canvas
        GameObject buttonObject = new GameObject("CardButton");
        buttonObject.transform.SetParent(canvasObject.transform, false);

        // Add an Image component first, which will automatically add a RectTransform
        Image buttonImage = buttonObject.AddComponent<Image>();
        buttonImage.color = new Color(1, 1, 1, 0.5f); // Set the colour to white with 0 alpha for transparency

        // Now, add the Button component
        Button button = buttonObject.AddComponent<Button>();

        // Assign the empty GameObject that serves as the anchor point for the button
        Transform buttonAnchor = child;

        RectTransform buttonRectTransform = button.GetComponent<RectTransform>();

        // Convert the position of the empty GameObject to the local space of the Canvas
        Vector2 anchorPositionOnCanvas = canvasObject.transform.InverseTransformPoint(buttonAnchor.position);

        // Now set the position of the button
        buttonRectTransform.sizeDelta = new Vector2(width / cardLocalScale.x, height / cardLocalScale.z); // Adjust the size as needed

        // Since the Canvas might be at a different height, we only use the x and z coordinates (which corresponds to width and length)
        buttonRectTransform.anchoredPosition = new Vector2(anchorPositionOnCanvas.x, anchorPositionOnCanvas.y);
        // Adjust the local z position (height) of the button as necessary to avoid intersection with the card
        buttonRectTransform.localPosition = new Vector3(buttonRectTransform.localPosition.x, buttonRectTransform.localPosition.y, 0);

        // Optionally, adjust the scale if needed to match the empty's scale
        buttonRectTransform.localScale = buttonAnchor.localScale;

        //Set up the button interaction
        button.onClick.AddListener(() => { Debug.Log(child.name + " clicked! On card " + newDungeonCard.name); });
    }

    private void CreateBossPile()
    {
        GameObject pileCard;
        if (finalBoss.model == null)
        {
            pileCard = Instantiate(placeholderCard.model, cardPoints[8].transform);
        }
        else
        {
            pileCard = Instantiate(bossDeck[0].model, cardPoints[8].transform);
        }

        pileCard.transform.rotation = Quaternion.Euler(180, 180, 180);
        
        pileCard.transform.localScale = new Vector3(25, cardThickness * (4 - usedBossDeck.Count), 25);
        
        pileCard.AddComponent<CardAnims>();
        pileCard.AddComponent<BoxCollider>();
        pileCard.GetComponent<BoxCollider>().size = new Vector3(0.05f, 0.0003f, 0.07f);
        pileCard.AddComponent<Rigidbody>();
        pileCard.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        pileCard.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    public void MoveActivePieceToCard(GameObject card)
    {
        StartCoroutine(SmoothMoveRandomPos(activePiece, card));
    }

    public void FlipCard(GameObject card, float raiseHeight, float flipTime)
    {
        StartCoroutine(SmoothCardFlip(card, raiseHeight, flipTime));
    }

    public void ZoomIn(GameObject card)
    {
        if (card.GetComponent<CardAnims>().isFaceUp)
        {
            StartCoroutine(SmoothZoomCard(card));
        }
    }

    public void ZoomOut(GameObject card)
    {
        if (!card.GetComponent<CardAnims>().isZoomed) { return; }
        StartCoroutine(SmoothRetractCard(card));
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

    private IEnumerator SmoothCardFlip(GameObject card, float raiseHeight, float flipTime)
    {
        if (card.GetComponent<CardAnims>().isZoomed) { yield break; }
        if (card.GetComponent <CardAnims>().isFlipping) { yield break; }
        if (isAnyCardZoomed) { yield break; }
        if (isAnyCardZooming) { yield break; }
        isAnyCardFlipping = true;
        card.GetComponent<CardAnims>().isFlipping = true;
        Vector3 originalPosition = card.transform.position;
        Quaternion originalRotation = card.transform.rotation;
        Quaternion flippedRotation = originalRotation * Quaternion.Euler(0, 0, 180);
        float elapsedTime = 0f;
        float finalRaiseHeight = 0.1f;
        card.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        while (elapsedTime < flipTime)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the percentage of the flip completed
            float t = elapsedTime / flipTime;

            // Modifying the height calculation for a smoother drop at the end of the flip
            float heightOffset = (Mathf.Sin(t * Mathf.PI) * raiseHeight) + (t * finalRaiseHeight);
            Vector3 raisedPosition = originalPosition + Vector3.up * heightOffset;

            card.transform.position = Vector3.Lerp(originalPosition, raisedPosition, t);

            // Rotate the card, starting roughly in the middle of the movement
            if (t > 0.25f)
            {
                card.transform.rotation = Quaternion.Lerp(originalRotation, flippedRotation, (t - 0.25f) * 2);
            }

            yield return null;
        }

        if (card.GetComponent<CardAnims>().isFaceUp)
        {
            card.GetComponent<CardAnims>().isFaceUp = false;
        }
        else
        {
            card.GetComponent<CardAnims>().isFaceUp = true;
        }

        // Snap to final position and rotation for accuracy
        card.transform.position = originalPosition;
        card.transform.rotation = flippedRotation;
        isAnyCardFlipping = false;
        card.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        card.GetComponent<CardAnims>().isFlipping = false;
    }

    private IEnumerator SmoothZoomCard(GameObject card)
    {
        if (isAnyCardZoomed) { yield break; } // don't zoom a card if one is already zoomed
        if (isAnyCardZooming) { yield break; }// or if one is being zoomed
        if (isAnyCardFlipping) { yield break; }
        isAnyCardZooming = true;
        card.GetComponent<CardAnims>().isZoomed = true;
        card.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        zoomedcardParent = card.transform.parent.gameObject;
        zoomedCard = card;
        card.transform.parent = null;

        Vector3 startPos = card.transform.position;
        Quaternion startRot = card.transform.rotation;

        Vector3 endPos = new Vector3(-0.25f, 4.73f, -4.26f);
        Quaternion endRot = Quaternion.Euler(-18, 0, 180);

        float journeyLength = Vector3.Distance(startPos, endPos);
        float speed = 10f;  // Set to desired speed value
        float startTime = Time.time;

        float distanceCovered = 0;

        while (distanceCovered < journeyLength)
        {
            float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted * speed / journeyLength;

            // Move the card to the interpolated position
            card.transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);

            // Rotate the card to the interpolated rotation
            card.transform.rotation = Quaternion.Slerp(startRot, endRot, percentageComplete);

            distanceCovered = (card.transform.position - startPos).magnitude;

            yield return null;
        }

        // Just to ensure that card reaches the final position and rotation
        card.transform.position = endPos;
        card.transform.rotation = endRot;
        yield return null;
        isAnyCardZooming = false;
        isAnyCardZoomed = true;
        yield return null;
    }

    private IEnumerator SmoothRetractCard(GameObject card)
    {
        if (!isAnyCardZoomed) { yield break; } // don't zoom out a card if one isn't already zoomed
        if (isAnyCardZooming) { yield break; }// or if one is being zoomed
        isAnyCardZooming = true;
        card.GetComponent<CardAnims>().isZoomed = false;
        card.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        card.transform.parent = zoomedcardParent.transform;

        Vector3 startPos = card.transform.position;
        Quaternion startRot = card.transform.rotation;

        Vector3 endPos = card.transform.parent.position;
        Quaternion endRot = faceUpCardRotation;

        float journeyLength = Vector3.Distance(startPos, endPos);
        float speed = 10f;  // Set to desired speed value
        float startTime = Time.time;

        float distanceCovered = 0;

        while (distanceCovered < journeyLength)
        {
            float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted * speed / journeyLength;

            // Move the card to the interpolated position
            card.transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);

            // Rotate the card to the interpolated rotation
            card.transform.rotation = Quaternion.Slerp(startRot, endRot, percentageComplete);

            distanceCovered = (card.transform.position - startPos).magnitude;

            yield return null;
        }

        // Just to ensure that card reaches the final position and rotation
        card.transform.position = endPos;
        card.transform.rotation = endRot;
        yield return null;
        isAnyCardZooming = false;
        isAnyCardZoomed = false;
        zoomedCard = null;
        yield return null;
    }

    public void InitialFloorSetup()
    {
        //Check if game has started!
        if (hasGameStarted) { hasGameStarted = false; SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); return; }
        hasGameStarted = true;

        //Place a card in each card slot 1-8
        //TODO add animation for game start
        DealCards();

        //Create a boss deck in slot 9
        CreateBossPile();

        //Set Current floor and room to 1
        currentFloor = 1;
        currentRoom = 1;
        UpdateFloor();

        debugGamePrompts.text = "";
        //Move player 1 piece to the first card
        MoveActivePieceToCard(cardPoints[0].transform.GetChild(0).gameObject);

        P1_Location = 0;

        //Set up player board(s)
        BoardManager.Instance.SetUpPlayerStats(P1, 1);

        if (Options.Instance.playerCount == 2)
        {
            BoardManager.Instance.SetUpPlayerStats(P2, 2);
        }

        //Set up dungeon board
        if (Options.Instance)
        {
            BoardManager.Instance.SetUpDungeonBoard(Options.Instance.chosenLevel);
        }
        else //set up solo dungeon if we started from game board scene
        {
            BoardManager.Instance.SetUpDungeonBoard(Options.level.Dungeon);
        }
        
        StartCoroutine(DoTurn());
    }

    private IEnumerator DoTurn()
    {
        //wait for pieces to stop moving
        while (isPieceMoving) 
        {
            yield return PieceMoveDelay;
        }

        //Swap the active player
        SwitchActivePlayer();

        //Save the active card
        GameObject activeCard = cardPoints[P1_Location].transform.GetChild(0).gameObject;

        //zoom card
        StartCoroutine(SmoothZoomCard(activeCard));

        //Wait for the card to zoom in
        while (isAnyCardZooming)
        {
            yield return PieceMoveDelay;
        }

        //Check the card, what do we need to do?
        CardData activeCardData = activeCard.GetComponent<Card>().cardData;
        debugGamePrompts.text = activeCardData.cardType.ToString();

        switch (activeCardData.cardType)
        {
            //choice
            case CardData.CardType.Merchant:
            case CardData.CardType.Bonfire:
                //make choice
                break;

            //choice then roll
            case CardData.CardType.Shrine:
                //make choice
                break;

            //roll then choice
            case CardData.CardType.Item:
                break;

            //choice then fight
            case CardData.CardType.Monster_Bandit:
                //make choice
                break;

            //Fight
            case CardData.CardType.Monster:
            case CardData.CardType.Boss:
                //Combat!
                break;

            //Roll
            case CardData.CardType.Tomb:
            case CardData.CardType.Trap:
            case CardData.CardType.Trap_Depths:
            case CardData.CardType.Treasure:
            case CardData.CardType.Treasure_Depths:
                //roll
                yield return StartCoroutine(HandleCardRoll(activeCardData));
                break;
        }

        //Debug.LogWarning("All dice have been rolled! (or there's no dice to roll!)");

        //Do button press action
        yield return null;
    }

    private void ChooseDiceAndRoll(CardData.CardType cardType)
    {
        diceToRoll.Clear();
        //Check what dice are required by card

        //Debug
        if (DebugRollAllDice)
        {
            foreach(GameObject dice in activePlayerIndex == 1 ? player1PlayerDice : player2PlayerDice)
            {
                diceToRoll.Add(dice);
            }
            diceToRoll.Add(activePlayerIndex == 1 ? player1PoisonDice : player2PoisonDice);
            diceToRoll.Add(activePlayerIndex == 1 ? player1CurseDice : player2CurseDice);
            diceToRoll.Add(activePlayerIndex == 1 ? player1MonsterDice : player2MonsterDice);

            DiceManager.Instance.RollDice(diceToRoll);
            return;
        }

        switch (cardType)
        {
            //Player Dice + Poison + Curse
            case CardData.CardType.Item:
            case CardData.CardType.Tomb:
            case CardData.CardType.Trap:
            case CardData.CardType.Trap_Depths:
            case CardData.CardType.Treasure:
            case CardData.CardType.Treasure_Depths:
            case CardData.CardType.Monster:
            case CardData.CardType.Boss:
            case CardData.CardType.Monster_Bandit:
                //Check dice suitable for active player
                //what level is player
                for(int i = 0; i < (activePlayerIndex == 1 ? P1.level : P2.level); i++)
                {
                    diceToRoll.Add(activePlayerIndex == 1 ? player1PlayerDice[i] : player2PlayerDice[i]);
                }
                //is player poisoned
                if (activePlayerIndex == 1 ? P1.isPoisoned : P2.isPoisoned)
                {
                    diceToRoll.Add(activePlayerIndex == 1 ? player1PoisonDice : player2PoisonDice);
                }
                //is player isCursed
                if (activePlayerIndex == 1 ? P1.isCursed : P2.isCursed)
                {
                    diceToRoll.Add(activePlayerIndex == 1 ? player1CurseDice : player2CurseDice);
                }
                //add monster dice
                diceToRoll.Add(activePlayerIndex == 1 ? player1MonsterDice : player2MonsterDice);
                break;
            //Monster Dice
            case CardData.CardType.Shrine:
                diceToRoll.Add(activePlayerIndex == 1 ? player1MonsterDice : player2MonsterDice);
                break;
            
        }

        DiceManager.Instance.RollDice(diceToRoll);
    }

    // Call this from the Die when it finishes rolling
    public void DieRolled(int value, Die.DieType diceType)
    {
        OnDieRollComplete?.Invoke(value, diceType);
    }

    // Function that gets called when a die roll is complete
    private void HandleDieRollComplete(int value, Die.DieType diceType)
    {
        rollResults.Add(value);
        rollResultsDieType.Add(diceType);
        diceRolled++;

        if (diceRolled >= diceToRoll.Count)
        {
            // All dice have rolled
        }
    }

    public void DieClicked(int value)
    {
        //record the chosen roll value
        monsterDieRollResult = value;
        awaitingDieChoice = false;
    }

    private IEnumerator WaitForDieChoice()
    {
        while (awaitingDieChoice)
        {
            yield return null;
        }

        Debug.Log("Die has been clicked");
    }

    private IEnumerator HandleCardRoll(CardData card)
    {
        bool critRolled = false;
        int monsterResult = 0;
        bool cursedRoll = false;
        ChooseDiceAndRoll(card.cardType);

        // wait for dice to be rolled
        while (diceRolled < diceToRoll.Count)
        {
            yield return null;
        }

        //check for curse die first
        foreach (var (result, i) in rollResults.WithIndex())
        {
            if ((rollResultsDieType[i] == Die.DieType.Curse && result == 3) ||
                (rollResultsDieType[i] == Die.DieType.Curse && result == 1) ||
                (rollResultsDieType[i] == Die.DieType.Curse && result == 6))
            {
                //-1 to all dice results
                cursedRoll = true;
                Debug.Log("You're cursed!");
                //show this to the player
                //TODO Broadcast the cursed roll
            }

        }

        //Determine if special die rolls do something
        foreach (var (result, i) in rollResults.WithIndex())
        {
            Debug.LogWarning(rollResultsDieType[i] + " die result is: " +  result);
            //did the player roll a crit?
            if (cursedRoll)
            {
                if (result == 6 && rollResultsDieType[i] == Die.DieType.Player)
                {
                    critRolled = true;
                    Debug.Log("it's a cursed crit!");
                }
            }
            else
            {
                if ((result == 5 && rollResultsDieType[i] == Die.DieType.Player) ||
                    (result == 6 && rollResultsDieType[i] == Die.DieType.Player))
                {
                    critRolled = true;
                    Debug.Log("it's a crit!");
                }
            }

            //TODO if there's a crit, shake the die and have the word crit rise out of it
            //+ similar for curse


            //record the monster die roll
            if (rollResultsDieType[i] == Die.DieType.Monster)
            {
                monsterResult = result;
            }

            //Check if the player has been poisoned
            //TODO if there's poison, shake the die and have the word poison rise out of it

            if (rollResultsDieType[i] == Die.DieType.Poison)
            {
                //-1 health
                activePlayer.HP -= 1;

                //TODO broadcast this to the player
            }
        }

        yield return (StartCoroutine(DiceManager.Instance.ArrangeDiceInGrid(diceToRoll)));

        switch (card.cardType)
        {
            case CardData.CardType.Tomb:
                
                if (critRolled)
                {
                    StartCoroutine(player1MonsterDice.GetComponent<Die>().CreateButtons(monsterResult));
                }
                break;
        }

        StartCoroutine(WaitForDieChoice());

        //die result has been confirmed
        //Tidy up dice
        foreach (GameObject die in diceToRoll)
        {
            die.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        //Tidy up zoomed card
        SmoothRetractCard(zoomedCard);

        //Do the card effect and update player boards

        yield return null;
    }

    /// <summary>
    /// Will update the die result of the specified die to the value specified or an appropriate value for curing poision/curse
    /// Does not currently update player dice values
    /// </summary>
    /// <param name="dieType"></param>
    /// <param name="conditionCured"></param>
    /// <param name="newValue"></param>
    public void UpdateDieResult(Die.DieType dieType, bool conditionCured = false)
    {
        if(dieType == Die.DieType.Monster || dieType == Die.DieType.Player)
        {
            Debug.LogError("Wrong die type for die adjustment!");
            return;
        }

        foreach (var (result, i) in rollResultsDieType.WithIndex())
        {
            if (result == dieType)
            {
                if(dieType == Die.DieType.Curse || dieType == Die.DieType.Poison)
                {
                    if (conditionCured)
                    {
                        //change value to either 3, 4 or 6
                        List<int> curedInts = new List<int>();
                        curedInts.Add(3); curedInts.Add(4); curedInts.Add(6);
                        rollResults[i] = curedInts[UnityEngine.Random.Range(0, curedInts.Count - 1)];
                    }
                    else
                    {
                        //change value to either 1, 2 or 5
                        List<int> curedInts = new List<int>();
                        curedInts.Add(1); curedInts.Add(2); curedInts.Add(5);
                        rollResults[i] = curedInts[UnityEngine.Random.Range(0, curedInts.Count - 1)];
                    }
                }
            }

        }
    }

    public void UpdateDieResult(Die.DieType dieType, int newValue = 0)
    {
        if (dieType == Die.DieType.Curse || dieType == Die.DieType.Poison)
        {
            Debug.LogError("Wrong die type for die adjustment!");
            return;
        }

        foreach (var (result, i) in rollResultsDieType.WithIndex())
        {
            if (result == dieType)
            {
                if (dieType == Die.DieType.Monster)
                {
                    rollResults[i] = newValue;
                }
                else if (dieType == Die.DieType.Player)
                {
                    Debug.LogError("UpdateDieResult() is not currently set up to modify player dice results");
                }
            }

        }
    }

    public void SwitchActivePlayer()
    {
        if (Options.Instance.playerCount > 1)
        {
            activePlayer = activePlayer == P1 ? P2 : P1;
        }
        else
        {
            activePlayer = P1;
        }
    }

    private void UpdateFloor()
    {
        floorStatusPrompts.text = "Floor: " + currentFloor + "   Room: " + currentRoom;
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

/// <summary>
/// Allows foreach iterations to easily track iteration count:
/// foreach (var (item, index) in collection.WithIndex())
/// </summary>
public static class IEnumerableExtensions
{
    // Found here https://stackoverflow.com/questions/43021/how-do-you-get-the-index-of-the-current-iteration-of-a-foreach-loop
    //TODO have a go at understanding how this works :sweatsmile:
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
       => self.Select((item, index) => (item, index));
}
