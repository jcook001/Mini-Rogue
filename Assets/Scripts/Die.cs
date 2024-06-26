using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    private Rigidbody rb;
    private bool isRolling = false;
    public bool canRoll = false;
    public int lastRolledValue = 0;
    public int currentRolledValue = 0;

    //Temporary buttons for crits
    GameObject canvasObj;
    Canvas canvas;
    RectTransform canvasRect;
    public GameObject buttonPrefab;
    private float buttonOffsetX = 30.0f;
    private float buttonOffsetY = 33.0f;
    private float buttonOffsetZ = 30.0f;
    List<GameObject> activeButtons = new List<GameObject>();

    GameObject activeCanvas;

    //Dice Face Rotations
    public Vector3 Face1 = new Vector3(270, 180 ,0);
    public Vector3 Face2 = new Vector3(0, 90, 90);
    public Vector3 Face3 = new Vector3(180, 270, 0);
    public Vector3 Face4 = new Vector3(0, 270, 0);
    public Vector3 Face5 = new Vector3(180, 90, 90);
    public Vector3 Face6 = new Vector3(90, 0, 0);

    Vector3[] dieVectors = {
            Vector3.up,    // 4
            Vector3.down,  // 3
            Vector3.left,  // 5 //poison //curse
            Vector3.right, // 2 //poison //curse
            Vector3.forward, // 1 //poison //curse
            Vector3.back    // 6
        };

    public enum DieType
    {
        Player,
        Monster,
        Poison,
        Curse
    }

    public DieType type;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if(type == DieType.Monster)
        {
            // Add and set up the Canvas component
            canvasObj = new GameObject("DiceCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();

            // Set the size and position of the canvas
            canvasRect = canvas.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(100, 100);  // Set the size of the canvas (adjust as needed)
            canvasRect.localPosition = Vector3.zero;  // Center the canvas on the dice
            canvasRect.localEulerAngles = Vector3.zero;  // Reset rotation (will align with camera later)
            canvasRect.localScale = new Vector3(0.001f, 0.001f, 0.001f);

            //Need to finda a way to set the canvas as a child of the die but still maintain the correct canvas position
            //canvas.transform.SetParent(transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(DiceManager.Instance.itsTimeToRoll && canRoll)
        {
            // Check if the die is moving or rotating
            if (!rb.IsSleeping() && !isRolling)
            {
                // The die started rolling
                isRolling = true;
            }
            else if (rb.IsSleeping() && isRolling)
            {
                // The die stopped rolling
                isRolling = false;
                int value = GetDieValue();
                // Do something with the value
                Debug.Log(this.name + " Die value: " + value + " (1,2 and 5 are poison/curse)");
                GameManager.Instance.DieRolled(value, type); // Notify the GameManager
                canRoll = false;
            }
        }
    }

    public void OnMouseDown()
    {
        //TODO only do this at the appropriate time
        GameManager.Instance.DieClicked(currentRolledValue);
        DestroyButtons();
    }

    private int GetDieValue()
    {
        // The local up vectors for each face of the die, assuming a standard d6 die
        //I don't know why these are different here than in GetRolledFaceUpVector() /shrug
        Vector3[] dieUpVectors = {
            Vector3.up,    // 4
            Vector3.down,  // 3
            Vector3.left,  // 5 //poison //curse
            Vector3.right, // 2 //poison //curse
            Vector3.forward, // 1 //poison //curse
            Vector3.back    // 6
        };

        // The value that each vector represents
        int[] dieValues = { 4, 3, 5, 2, 1, 6 };

        float maxDot = -Mathf.Infinity;
        int bestValue = 0;

        for (int i = 0; i < dieUpVectors.Length; i++)
        {
            // Convert the die's local up vector to world space
            Vector3 worldUpVector = transform.TransformDirection(dieUpVectors[i]);
            // Calculate the dot product with the world up
            float dot = Vector3.Dot(worldUpVector, Vector3.up);

            if (dot > maxDot)
            {
                maxDot = dot;
                bestValue = dieValues[i];
            }
        }

        lastRolledValue = bestValue;
        currentRolledValue = bestValue;
        return bestValue;
    }

    private Vector3 GetRolledFaceUpVector(out int rolledValue)
    {
        // The local up vectors for each face of the die, assuming a standard d6 die
        //I don't know why these are different here than in GetDieVale() /shrug
        Vector3[] dieUpVectors = {
            Vector3.up,    // 3
            Vector3.down,  // 4
            Vector3.left,  // 2 //poison //curse
            Vector3.right, // 5 //poison //curse
            Vector3.forward, // 6 
            Vector3.back    // 1 //poison //curse
        };

        // The value that each vector represents
        int[] dieValues = { 3, 4, 2, 5, 6, 1 };

        float maxDot = -Mathf.Infinity;
        int bestValue = 0;

        for (int i = 0; i < dieUpVectors.Length; i++)
        {
            // Convert the die's local up vector to world space
            Vector3 worldUpVector = transform.TransformDirection(dieUpVectors[i]);
            // Calculate the dot product with the world up
            float dot = Vector3.Dot(worldUpVector, Vector3.up);

            if (dot > maxDot)
            {
                maxDot = dot;
                bestValue = dieValues[i];
            }
        }

        rolledValue = bestValue;
        return dieUpVectors[Array.IndexOf(dieValues, bestValue)];
    }

    public Quaternion RolledFaceToCameraRotation(Vector3 lookFromPosition, int overrideFace = 0)
    {
        Quaternion rotationToCamera = Quaternion.Euler(Vector3.zero);
        int targetFace;
        if(overrideFace == 0)
        {
            GetRolledFaceUpVector(out targetFace);
        }
        else
        {
            targetFace = overrideFace;
        }

        
        Camera camera = Camera.main;

        Vector3[] dieFaceOrientations = {
        Vector3.back, // 1
        Vector3.left,   // 2
        Vector3.up,     // 3
        Vector3.down,      // 4
        Vector3.right,    // 5
        Vector3.forward    // 6
        };

        // Ensure the target face is valid
        if (targetFace < 1 || targetFace > 6)
        {
            Debug.LogError("Invalid target face value. Must be between 1 and 6.");
            return rotationToCamera;
        }

        // Determine the direction from the die to the camera
        Vector3 toCamera = camera.transform.position - lookFromPosition;
        toCamera.Normalize(); // Ensure it's a unit vector

        // Determine the direction that should be facing towards the camera
        Vector3 targetFaceDirection = transform.TransformDirection(dieFaceOrientations[targetFace - 1]);

        // Calculate a rotation that points 'targetFaceDirection' along 'toCamera'
        Quaternion targetRotation = Quaternion.FromToRotation(targetFaceDirection, toCamera);

        // Return the desired rotation
        rotationToCamera = targetRotation * transform.rotation;

        return rotationToCamera;
    }

    public IEnumerator CreateButtons(int monsterDieResult)
    {
        //Create a canvas on the die
        activeCanvas = Instantiate(canvasObj, transform);

        yield return null;
        // Orient buttons to face camera - adjust this in the Update method of the buttons or right here
        activeCanvas.transform.rotation = Camera.main.transform.localRotation;

        yield return null;

        // Instantiate buttons
        if(monsterDieResult != 6)
        {
            GameObject buttonAbove = Instantiate(buttonPrefab, activeCanvas.transform.TransformPoint(Vector3.zero), activeCanvas.transform.rotation, activeCanvas.transform);
            buttonAbove.transform.localPosition = new Vector3(buttonOffsetX, buttonOffsetY, buttonOffsetZ);
            buttonAbove.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+1";
            buttonAbove.GetComponent<Button>().onClick.AddListener(AddOne);
            activeButtons.Add(buttonAbove);
        }

        if(monsterDieResult != 1)
        {
            GameObject buttonBelow = Instantiate(buttonPrefab, activeCanvas.transform.TransformPoint(Vector3.zero), activeCanvas.transform.rotation, activeCanvas.transform);
            buttonBelow.transform.localPosition = new Vector3(buttonOffsetX, -buttonOffsetY, buttonOffsetZ);
            buttonBelow.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-1";
            buttonBelow.GetComponent<Button>().onClick.AddListener(SubtractOne);
            activeButtons.Add(buttonBelow);
        }

        GameObject buttonMiddle = Instantiate(buttonPrefab, activeCanvas.transform.TransformPoint(Vector3.zero), activeCanvas.transform.rotation, activeCanvas.transform);
        buttonMiddle.transform.localPosition = new Vector3(buttonOffsetX, 0, buttonOffsetZ);
        buttonMiddle.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "=";
        buttonMiddle.GetComponent<Button>().onClick.AddListener(NoChange);
        activeButtons.Add(buttonMiddle);
        
    }

    public void DestroyButtons()
    {
        foreach(GameObject g in activeButtons)
        {
            Destroy(g);
        }
    }

    public void AddOne()
    {
        StartCoroutine(AdjustDieRollResult(1));
        currentRolledValue = lastRolledValue + 1;
    }

    public void NoChange()
    {
        StartCoroutine(AdjustDieRollResult(0));
        currentRolledValue = lastRolledValue;
    }

    public void SubtractOne()
    {
        StartCoroutine(AdjustDieRollResult(-1));
        currentRolledValue = lastRolledValue - 1;
    }

    private IEnumerator AdjustDieRollResult(int adjustAmount)
    {
        //calculate new value
        int newMonsterDieResult = lastRolledValue + adjustAmount;
        GameManager.Instance.UpdateDieResult(DieType.Monster, newMonsterDieResult);

        //unparent canvas
        activeCanvas.transform.SetParent(this.gameObject.transform.parent, true);
        //rotate die
        yield return StartCoroutine(DiceManager.Instance.SmoothRotateDie(this.gameObject, newMonsterDieResult, Camera.main.gameObject.transform, 1.0f));
        //reparent canvas
        activeCanvas.transform.SetParent(this.gameObject.transform, true);

        yield return null;
    }
}
