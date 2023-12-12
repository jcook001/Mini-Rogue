using UnityEngine;

public class Die : MonoBehaviour
{
    private Rigidbody rb;
    private bool isRolling = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DiceManager.Instance.itsTimeToRoll)
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
                Debug.Log(this.name + " Die value: " + value);
                GameManager.Instance.DieRolled(value); // Notify the GameManager
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
}
