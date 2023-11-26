using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public List<Rigidbody> diceRigidbodies = new List<Rigidbody>();  // List to hold the rigidbodies of the dice
    public Transform tableTransform; // Transform of the table object
    public float pickUpHeight = 1.0f; // Height above the table to hold the dice
    public float throwForce = 10.0f; // Force applied when throwing the dice
    public float smoothTime = 0.2f; // Smoothing time for the movement

    private Vector3[] dicePositions; // Target positions for the dice
    private Vector3[] velocity; // Velocities for smooth damp movement
    private bool isDragging = false; // Flag to check if the dice are being held
    private Camera mainCamera;

    private bool itsTimeToRoll = false;

    private void Start()
    {
        mainCamera = Camera.main;
        dicePositions = new Vector3[diceRigidbodies.Count];
        velocity = new Vector3[diceRigidbodies.Count];
    }

    private void Update()
    {
        if (itsTimeToRoll)
        {
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
        }

        itsTimeToRoll = true;
    }
}
