using System;
using UnityEngine;

public class Die : MonoBehaviour
{
    private Rigidbody rb;
    private bool isRolling = false;
    public bool canRoll = false;

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

        rolledValue = bestValue;
        return dieUpVectors[Array.IndexOf(dieValues, bestValue)];
    }

    public void PointRolledFaceToCamera(GameObject die)
    {
        int rolledValue;
        Vector3 rolledFaceUpVector = GetRolledFaceUpVector(out rolledValue);

        // Ensure the rolled value is upwards
        die.transform.up = rolledFaceUpVector;

        // Rotate the die around its up axis to face the camera
        Vector3 cameraDirectionFlat = (Camera.main.transform.position - die.transform.position).normalized;
        cameraDirectionFlat.y = 0; // Ignore vertical component

        // Find the forward direction for the die, relative to the camera
        Vector3 dieForwardTowardsCamera = Vector3.Cross(die.transform.up, cameraDirectionFlat).normalized;
        Quaternion finalRotation = Quaternion.LookRotation(dieForwardTowardsCamera, die.transform.up);

        // Apply the rotation
        die.transform.rotation = finalRotation;
    }


}
