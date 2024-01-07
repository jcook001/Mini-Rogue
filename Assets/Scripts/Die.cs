using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    private Rigidbody rb;
    private bool isRolling = false;
    public bool canRoll = false;

    //Temporary buttons for crits
    GameObject canvasObj;
    Canvas canvas;
    RectTransform canvasRect;
    public GameObject buttonPrefab;
    private float buttonOffset = 0.5f;

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

        // Add and set up the Canvas component
        canvasObj = new GameObject("DiceCanvas");
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();

        // Set the size and position of the canvas
        canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(1, 1);  // Set the size of the canvas (adjust as needed)
        canvasRect.localPosition = Vector3.zero;  // Center the canvas on the dice
        canvasRect.localEulerAngles = Vector3.zero;  // Reset rotation (will align with camera later)
        canvasRect.localScale = new Vector3(0.001f, 0.001f, 0.001f);
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

    public void PointRolledFaceToCamera(GameObject die) //points dice to camera
    {
        int targetFace;
        GetRolledFaceUpVector(out targetFace);

        Camera camera = Camera.main;

        Vector3[] dieFaceOrientations = {
        Vector3.forward, // 1
        Vector3.right,   // 2
        Vector3.down,     // 3
        Vector3.up,      // 4
        Vector3.left,    // 5
        Vector3.back    // 6
        };

        // Ensure the target face is valid
        if (targetFace < 1 || targetFace > 6)
        {
            Debug.LogError("Invalid target face value. Must be between 1 and 6.");
            return;
        }

        // Determine the direction from the die to the camera
        Vector3 toCamera = camera.transform.position - transform.position;
        toCamera.Normalize(); // Ensure it's a unit vector

        // Determine the direction that should be facing towards the camera
        Vector3 targetFaceDirection = transform.TransformDirection(dieFaceOrientations[targetFace - 1]);

        // Calculate a rotation that points 'targetFaceDirection' along 'toCamera'
        Quaternion targetRotation = Quaternion.FromToRotation(targetFaceDirection, toCamera);

        // Apply the rotation
        die.transform.rotation = targetRotation * transform.rotation;
    }

    public void OrientDieFaceUpwards(int targetFace)
    {
        Vector3[] dieFaceOrientations = {
        Vector3.back,    // 1
        Vector3.left,    // 2
        Vector3.down,     // 3
        Vector3.up,      // 4
        Vector3.right,   // 5
        Vector3.forward // 6
    };

        // Ensure the target face is valid
        if (targetFace < 1 || targetFace > 6)
        {
            Debug.LogError("Invalid target face value. Must be between 1 and 6.");
            return;
        }

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z);

        // Calculate the rotation to the target face
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, dieFaceOrientations[targetFace - 1]);

        // Apply the rotation to the die
        transform.rotation = targetRotation;
    }

    public void CreateButtons()
    {
        //Create a canvas on the die
        GameObject DieCanvas = Instantiate(canvasObj, transform);

        Debug.Log(DieCanvas.transform.localRotation.eulerAngles);
        DieCanvas.transform.rotation = Quaternion.LookRotation(DieCanvas.transform.position - Camera.main.transform.position);
        //DieCanvas.transform.rotation = Camera.main.transform.rotation;
        Debug.Log(DieCanvas.transform.localRotation.eulerAngles);

        //DieCanvas.transform.LookAt(Camera.main.transform);
        DieCanvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        Debug.Log(DieCanvas.transform.localRotation.eulerAngles);

        // Calculate positions for the buttons
        Vector3 abovePosition = this.transform.position + new Vector3(0, GetDieSize().y / 2 + buttonOffset, 0);
        Vector3 belowPosition = this.transform.position - new Vector3(0, GetDieSize().y / 2 + buttonOffset, 0);

        // Instantiate buttons
        GameObject buttonAbove = Instantiate(buttonPrefab, abovePosition, Quaternion.identity, DieCanvas.transform);
        GameObject buttonBelow = Instantiate(buttonPrefab, belowPosition, Quaternion.identity, DieCanvas.transform);

        // Orient buttons to face camera - adjust this in the Update method of the buttons or right here
        // ...

        // Attach onClick functionality for the buttons
        // buttonAbove.GetComponent<Button>().onClick.AddListener(() => AddToRoll());
        // buttonBelow.GetComponent<Button>().onClick.AddListener(() => SubtractFromRoll());

        if(buttonAbove.GetComponent<Button>() == null)
        {
            Debug.LogError("shit");
        }
        buttonAbove.GetComponent<Button>().onClick.AddListener(Test);
        buttonBelow.GetComponent<Button>().onClick.AddListener(Test);
    }

    public void Test()
    {
        Debug.LogWarning("button has been pressed");
    }

    Vector3 GetDieSize()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            return renderer.bounds.size;
        }
        else
        {
            Debug.LogError("No MeshRenderer found on the die.");
            return Vector3.zero;
        }
    }
}
