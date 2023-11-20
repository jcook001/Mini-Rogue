using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

//TODO re-enable this later
#pragma warning disable 0414 //disables unused variable warning

public class GameManager : MonoBehaviour
{
    private GameObject options_Manager;
    private Options options;

    //Game pieces
    public GameObject P1_Piece; //TODO make player choose their piece so this can be private 
    public GameObject P2_Piece; //TODO make player choose their piece so this can be private 
    public GameObject monsterBoard;
    public GameObject playerTracker;

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

    public float moveSpeed = 1f;
    public float moveHeight = 1f;
    private bool isPieceMoving = false;

    public bool isAnyCardZoomed = false;
    public bool isAnyCardZooming = false;
    public bool isAnyCardFlipping = false;
    private Quaternion faceUpCardRotation = Quaternion.Euler(180, 180, 0);
    private Quaternion faceDownCardRotation = Quaternion.Euler(180, 180, 180);
    private GameObject zoomedcardParent = null;

    //Gameplay
    private bool hasGameStarted = false;
    private int currentFloor = 0;
    private int currentRoom = 0;
    private int playerCount = 1;
    private int playerTurn = 1;
    private GameObject activePiece;
    private int P1_Location = 0;
    private int P2_Location = 0;

    //DEBUG
    public bool DebugShowDungeonCards = false;
    public bool DebugShowFontSymbols = false;

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

        if (DebugShowFontSymbols)
        {
            debugOverlay.text = PrintAllChars();
        }

        //Place the player pieces on the first card
        debugGamePrompts.text = "Press the start game button to begin!";

        //TODO Assign this properly in UI
        activePiece = P1_Piece;

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
            else
            {
                newDungeonCard = Instantiate(dungeonDeck[randomCardInt].model, cardPoints[i].transform);
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
                cardRenderer.bounds.size.z / cardLocalScale.z
            );

            RectTransform rectTransform = canvasObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(cardSize.x, cardSize.z); // Set the size of the canvas to match the card's size
            rectTransform.localScale = new Vector3(
                1f / cardLocalScale.x,
                1f / cardLocalScale.z,
                1f
                );

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
        if(playerCount == 2)
        {
            if (!isPieceMoving)
            {
                //update the active player turn
                if (playerTurn == 1)
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
        card.transform.parent = null;

        Vector3 startPos = card.transform.position;
        Quaternion startRot = card.transform.rotation;

        Vector3 endPos = new Vector3(0, 5.4f, -3.35f);
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
        yield return null;
    }

    public void InitialFloorSetup()
    {
        //Check if game has started!
        if (hasGameStarted) {  return; }
        hasGameStarted = true;

        //Check chosen game mode
        if (options == null)
        {
            //DEBUG we haven't started from the main menu
        }
        else if (options.gameTypeDropdown.value == 0)
        {
            //The Dungeon has been selected but this is the default
        }
        else if (options.gameTypeDropdown.value == 1)
        {
            //The Tower has been selected, so flip the gameboard
            monsterBoard.transform.Rotate(180, 0, 0);
        }

        //Place a card in each card slot 1-8
        //TODO add animation for game start
        DealCards();

        //Create a boss deck in slot 9
        CreateBossPile();

        //Set Current floor and room to 1
        currentFloor = 1;
        currentRoom = 1;
        UpdateFloor();

        //Place appropriate game pieces on card 1
        if (options == null || options.gameTypeDropdown.value == 0 || options.gameTypeDropdown.value == 1)
        {
            debugGamePrompts.text = "";
            //Move player 1 piece to the first card
            MoveActivePieceToCard(cardPoints[0].transform.GetChild(0).gameObject);
            
            //TODO update the card to be moved to at the start of the piece movement
            //P1_Location = Array.IndexOf(cardPoints, P1_Piece.transform.parent.gameObject);
            //Debug.Log("player 1 location is " + P1_Location);
        }
        else if (options.gameTypeDropdown.value > 1)
        {
            //The Tower has been selected, so flip the gameboard
            debugGamePrompts.text = "There's no support for multiplayer yet :(";
        }

        StartCoroutine(DoTurn());
    }

    private IEnumerator DoTurn()
    {
        //wait for pieces to stop moving
        while (isPieceMoving) 
        {
            yield return new WaitForSeconds(1);
        }
        //extra pause?

        //zoom card
        StartCoroutine(SmoothZoomCard(cardPoints[P1_Location].transform.GetChild(0).gameObject));

        //wait for button press

        //Do button press action
        yield return null;
    }

    private void UpdateFloor()
    {
        floorStatusPrompts.text = "test";
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
