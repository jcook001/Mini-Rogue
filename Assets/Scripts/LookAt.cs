using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LookAt : MonoBehaviour
{
    public GameObject target;
    public GameObject moveToTarget;
    public int FaceTo = 1;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        //better movement and calculate end rotation every frame
        //Check if there is any distance to move
        if (this.gameObject.transform.position == moveToTarget.transform.position)
        {
            StartCoroutine(SmoothRotate(this.gameObject, 1.0f));
        }
        else
        {
            StartCoroutine(SmoothSyncMoveRotate(this.gameObject, moveToTarget.transform.position));
        }
    }

    public Quaternion CalculateRotationForTargetMatchRotation(Transform diceTransform, int faceNumber, Vector3 viewPosition, Transform target)
    {
        Vector3 faceDirection;
        Vector3 faceUpDirection;

        // Map the face number to the corresponding local vector and its up vector
        switch (faceNumber)
        {
            case 1:
                faceDirection = diceTransform.forward;
                faceUpDirection = diceTransform.up;
                break;
            case 2:
                faceDirection = diceTransform.right;
                faceUpDirection = diceTransform.up;
                break;
            case 3:
                faceDirection = -diceTransform.up;
                faceUpDirection = diceTransform.right;
                break;
            case 4:
                faceDirection = diceTransform.up;
                faceUpDirection = diceTransform.right;
                break;
            case 5:
                faceDirection = -diceTransform.right;
                faceUpDirection = diceTransform.up;
                break;
            case 6:
                faceDirection = -diceTransform.forward;
                faceUpDirection = diceTransform.up;
                break;
            default:
                throw new ArgumentException("Invalid face number");
        }

        // Step 1: Align the face with the target
        Vector3 toTarget = (target.position - viewPosition).normalized;
        Quaternion faceToTargetRotation = Quaternion.FromToRotation(faceDirection, toTarget);

        // Step 2: Align the die's 'up' direction
        Vector3 worldFaceUpDirection = faceToTargetRotation * faceUpDirection;
        Quaternion upAlignmentRotation = Quaternion.FromToRotation(worldFaceUpDirection, target.up);

        // Combine the two rotations
        Quaternion targetRotation = upAlignmentRotation * faceToTargetRotation * diceTransform.rotation;

        return targetRotation;
    }

    //Use this when rotating and moving a distance
    public IEnumerator SmoothSyncMoveRotate(GameObject die, Vector3 targetLocation)
    {
        Vector3 startPos = die.transform.position;
        Quaternion startRot = die.transform.rotation;

        Vector3 endPos = targetLocation;
        //Quaternion endRot = targetRotation;

        float journeyLength = Vector3.Distance(startPos, endPos);
        float speed = 10f;  // Set to desired speed value
        float startTime = Time.time;

        float distanceCovered = 0;

        while (distanceCovered < journeyLength)
        {
            float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted * speed / journeyLength;

            // Move the card to the interpolated position
            die.transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);

            // Rotate the card to the interpolated rotation
            die.transform.rotation = Quaternion.Slerp(startRot, CalculateRotationForTargetMatchRotation(this.gameObject.transform, FaceTo, moveToTarget.transform.position, target.transform), percentageComplete);

            distanceCovered = (die.transform.position - startPos).magnitude;

            yield return null;
        }

        // Just to ensure that card reaches the final position and rotation
        die.transform.position = endPos;
        die.transform.rotation = CalculateRotationForTargetMatchRotation(this.gameObject.transform, FaceTo, moveToTarget.transform.position, target.transform);
        yield return null;
    }

    //Use this when rotating in place
    public IEnumerator SmoothRotate(GameObject die, float duration)
    {
        Quaternion startRot = die.transform.rotation;

        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float percentageComplete = (Time.time - startTime) / duration;

            // Rotate the die to the interpolated rotation
            die.transform.rotation = Quaternion.Slerp(startRot, CalculateRotationForTargetMatchRotation(this.gameObject.transform, FaceTo, moveToTarget.transform.position, target.transform), percentageComplete);

            yield return null;
        }

        // Ensure the die reaches the final rotation
        die.transform.rotation = CalculateRotationForTargetMatchRotation(this.gameObject.transform, FaceTo, moveToTarget.transform.position, target.transform);
    }
}
