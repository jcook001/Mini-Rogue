using UnityEngine;
using System.Collections.Generic;

public class DiceManagerTemp : MonoBehaviour
{
    public List<Rigidbody> diceRigidbodies = new List<Rigidbody>(); // Array to hold the rigidbodies of the dice
    public Transform tableTransform; // Transform of the table object
    public float pickUpHeight = 1.0f; // Height above the table to hold the dice
    public float throwForce = 10.0f; // Force applied when throwing the dice
    public float smoothTime = 0.2f; // Smoothing time for the movement

    private Vector3[] dicePositions; // Target positions for the dice
    private Vector3[] velocity; // Velocities for smooth damp movement
    private bool holdingDice = false; // Flag to check if the dice are being held
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        dicePositions = new Vector3[diceRigidbodies.Count];
        velocity = new Vector3[diceRigidbodies.Count];
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPosition = tableTransform.position + Vector3.up * pickUpHeight;
        targetPosition.x = ray.origin.x;
        targetPosition.z = ray.origin.z;

        if (Input.GetMouseButtonDown(0))
        {
            holdingDice = true;
            for (int i = 0; i < diceRigidbodies.Count; i++)
            {
                diceRigidbodies[i].isKinematic = true; // Stop physics while moving the dice
                // Calculate non-intersecting positions for the dice
                dicePositions[i] = targetPosition + (Random.insideUnitSphere * 0.5f);
                dicePositions[i].y = targetPosition.y; // Ensure all dice are at the same height
            }
        }

        if (holdingDice)
        {
            for (int i = 0; i < diceRigidbodies.Count; i++)
            {
                // Smoothly move the dice to the target position
                diceRigidbodies[i].position = Vector3.SmoothDamp(diceRigidbodies[i].position, dicePositions[i], ref velocity[i], smoothTime);
            }
        }

        if (Input.GetMouseButtonUp(0) && holdingDice)
        {
            holdingDice = false;
            foreach (var dieRigidbody in diceRigidbodies)
            {
                dieRigidbody.isKinematic = false; // Re-enable physics
                Vector3 throwDirection = (ray.direction + Vector3.up).normalized; // Toss up and in the direction of the ray
                dieRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse); // Apply throw force
            }
        }
    }
}
