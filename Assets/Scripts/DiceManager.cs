using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance { get; private set; } // Static property to access the instance

    public List<Rigidbody> diceRigidbodies = new List<Rigidbody>();  // List to hold the rigidbodies of the dice
    public Transform tableTransform; // Transform of the table object
    public float pickUpHeight = 1.0f; // Height above the table to hold the dice
    public float throwForce = 10.0f; // Force applied when throwing the dice
    public float smoothTime = 0.2f; // Smoothing time for the movement

    private Vector3[] dicePositions; // Target positions for the dice
    private Vector3[] velocity; // Velocities for smooth damp movement
    private bool isDragging = false; // Flag to check if the dice are being held
    private Camera mainCamera;

    public bool itsTimeToRoll = false;

    public GameObject card; // Assign the card game object in the inspector
    private int cardLayer; // To store the layer number of the card

    //Dice Grid Display
    public int rows = 2;
    public int columns = 3;
    public float diceSpacing = 1.0f; // Space between dice
    public GameObject gridObject;

    void Awake()
    {
        // Singleton pattern for easy access
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        dicePositions = new Vector3[diceRigidbodies.Count];
        velocity = new Vector3[diceRigidbodies.Count];
        cardLayer = LayerMask.NameToLayer("Card");

    }

    private void Update()
    {
        if (itsTimeToRoll)
        {
            if (GameManager.Instance.isAnyCardZoomed)
            {
                // Check every frame to see if the dice is behind the card
                CheckDiceBehindCard();
            }

            // Cast a ray from the camera to the mouse position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            // Calculate a plane at the pick-up height above the table
            Plane plane = new Plane(Vector3.up, tableTransform.position + Vector3.up * pickUpHeight);

            Vector3 targetPosition = tableTransform.position + Vector3.up * pickUpHeight;
            targetPosition.x = ray.origin.x;
            targetPosition.z = ray.origin.z;

            // Check if the ray hits the plane
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                targetPosition = new Vector3(hitPoint.x, tableTransform.position.y + pickUpHeight, hitPoint.z);

                if (Input.GetMouseButtonDown(0))
                {
                    // Start dragging
                    isDragging = true;
                    // Calculate non-intersecting positions for the dice only when dragging starts
                    for (int i = 0; i < diceRigidbodies.Count; i++)
                    {
                        diceRigidbodies[i].isKinematic = true; // Stop physics while moving the dice
                        Vector3 diceOffset = Random.insideUnitSphere * 1.0f;
                        diceOffset.y = 0; // Prevent vertical offset to maintain pickUpHeight
                        dicePositions[i] = targetPosition + diceOffset;
                    }
                }

                if (isDragging)
                {
                    for (int i = 0; i < diceRigidbodies.Count; i++)
                    {
                        // Move the dice along with the mouse without changing their relative positions
                        Vector3 newPosition = targetPosition + (dicePositions[i] - dicePositions[0]);
                        diceRigidbodies[i].MovePosition(Vector3.SmoothDamp(diceRigidbodies[i].position, newPosition, ref velocity[i], smoothTime));
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                isDragging = false;
                foreach (var dieRigidbody in diceRigidbodies)
                {
                    dieRigidbody.isKinematic = false; // Re-enable physics
                    Vector3 throwDirection = (ray.direction + Vector3.up).normalized; // Toss up and in the direction of the ray
                    dieRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse); // Apply throw force
                }
            }
        }

    }

    public void RollDice(List<GameObject> diceToRoll)
    {
        diceRigidbodies.Clear();
        for (int i = 0; i < diceToRoll.Count; i++)
        {
            diceRigidbodies.Add(diceToRoll[i].GetComponent<Rigidbody>());
            diceToRoll[i].GetComponent<Die>().canRoll = true;
        }

        itsTimeToRoll = true;
    }

    void CheckDiceBehindCard()
    {
        bool anyDiceBehindCard = false;
        card = GameManager.Instance.zoomedCard;

        foreach (Rigidbody rigidbody in diceRigidbodies)
        {
            Transform die = rigidbody.gameObject.transform;
            RaycastHit hit;
            // Cast a ray from the camera towards the die
            if (Physics.Raycast(Camera.main.transform.position, (die.position - Camera.main.transform.position).normalized, out hit))
            {
                // Check if the ray hit the card layer
                if (hit.transform.gameObject.layer == cardLayer && hit.transform == card.transform)
                {
                    card = hit.transform.gameObject;
                    anyDiceBehindCard = true;
                    break; // Exit the loop as soon as one die is found behind the card
                }
            }
        }

        // Change the card material transparency only if any dice are behind it
        if (anyDiceBehindCard)
        {
            card.GetComponent<CardAnims>().FadeOut();

        }
        else
        {
            card.GetComponent<CardAnims>().FadeIn();
        }
    }

    //TODO Probably going to need to make this account for different screen sizes?
    public void ArrangeDiceInGrid(List<GameObject> dice)
    {
        for (int i = 0; i < dice.Count; i++)
        {
            int row = i / columns;
            int column = i % columns;

            Vector3 localPosition = new Vector3(
                column * diceSpacing,
                0,
                row * diceSpacing
            );

            dice[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            dice[i].transform.SetParent(gridObject.transform, false);
            dice[i].transform.localPosition = localPosition;
            dice[i].GetComponent<Die>().PointRolledFaceToCamera(dice[i]);


            //Debug.Log("Dice " + dice[i].name + " position set");
        }
    }
}
