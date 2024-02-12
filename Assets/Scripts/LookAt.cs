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
        //transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
        //transform.LookAt(target.transform, Vector3.up);
        //PointFaceAtTarget(this.transform, FaceTo, target.transform.position);
        //PointFaceAtTargetWithCorrectUpMatchRotation(this.transform, FaceTo, target.transform);
        //PointFaceAtTargetWithCorrectUp(this.transform, FaceTo, target.transform);
        //this.transform.rotation = CalculateTargetRotationWithCorrectUp(this.transform, FaceTo, this.transform.position, target.transform);
        //this.transform.rotation = CalculateRotationForTargetMatchRotation(this.transform, FaceTo, this.transform.position, target.transform);
    }

    public void OnMouseDown()
    {
        //Correct up
        //this.transform.rotation = CalculateRotationForTargetMatchRotation(this.gameObject.transform, FaceTo, this.transform.position, target.transform);
        //StartCoroutine(DiceManager.Instance.MoveDieToPosition(this.gameObject, moveToTarget.transform.position, CalculateRotationForTargetMatchRotation(this.gameObject.transform, FaceTo, moveToTarget.transform.position, target.transform)));

        //correct face
        //StartCoroutine(DiceManager.Instance.MoveDieToPosition(this.gameObject, moveToTarget.transform.position, AngleToPointFaceAtTarget(this.gameObject.transform, FaceTo, moveToTarget.transform.position, target.transform.position)));
        //this.transform.rotation = AngleToPointFaceAtTarget(this.gameObject.transform, FaceTo, this.transform.position, target.transform.position);

        //better movement correct face
        //StartCoroutine(DiceManager.Instance.SmoothSyncMoveRotate(this.gameObject, moveToTarget.transform.position, AngleToPointFaceAtTarget(this.gameObject.transform, FaceTo, moveToTarget.transform.position, target.transform.position)));

        //better movement correct face correct up
        //StartCoroutine(DiceManager.Instance.SmoothSyncMoveRotate(this.gameObject, moveToTarget.transform.position, CalculateRotationForTargetMatchRotation(this.gameObject.transform, FaceTo, moveToTarget.transform.position, target.transform)));

        //better movement and calculate end rotation every frame?
        //StartCoroutine(SmoothSyncMoveRotate(this.gameObject, moveToTarget.transform.position));

        //Check if there is any distance to move
        if(this.gameObject.transform.position == moveToTarget.transform.position)
        {
            StartCoroutine(SmoothRotate(this.gameObject, 1.0f));
        }
        else
        {
            StartCoroutine(SmoothSyncMoveRotate(this.gameObject, moveToTarget.transform.position));
        }
        //better movement and calculate end rotation every frame
    }

    void PointLeftAtTarget(Transform objectTransform, Vector3 targetPosition)
    {
        // Calculate the desired direction from the object to the target
        Vector3 desiredDirection = (targetPosition - objectTransform.position).normalized;

        // Calculate the rotation needed to align the object's left vector with the desired direction
        Quaternion targetRotation = Quaternion.FromToRotation(-objectTransform.right, desiredDirection) * objectTransform.rotation;

        // Apply the rotation
        objectTransform.rotation = targetRotation;
    }

    void PointFaceAtTarget(Transform diceTransform, int faceNumber, Vector3 targetPosition)
    {
        Vector3 faceDirection;

        // Map the face number to the corresponding local vector
        switch (faceNumber)
        {
            case 1: faceDirection = diceTransform.forward; break; // Assuming 1 maps to the forward face
            case 2: faceDirection = diceTransform.right; break; // Assuming 2 maps to the back face
            case 3: faceDirection = -diceTransform.up; break; // Assuming 3 maps to the up face
            case 4: faceDirection = diceTransform.up; break; // Assuming 4 maps to the down face
            case 5: faceDirection = -diceTransform.right; break; // Assuming 5 maps to the left face
            case 6: faceDirection = -diceTransform.forward; break; // Assuming 6 maps to the right face
            default: throw new ArgumentException("Invalid face number");
        }

        // Calculate the desired direction from the dice to the target
        Vector3 desiredDirection = (targetPosition - diceTransform.position).normalized;

        // Calculate the rotation needed to align the specified face with the desired direction
        Quaternion targetRotation = Quaternion.FromToRotation(faceDirection, desiredDirection) * diceTransform.rotation;

        // Apply the rotation
        diceTransform.rotation = targetRotation;
    }

    Quaternion AngleToPointFaceAtTarget(Transform diceTransform, int faceNumber, Vector3 lookFrom, Vector3 targetPosition)
    {
        Vector3 faceDirection;

        // Map the face number to the corresponding local vector
        switch (faceNumber)
        {
            case 1: faceDirection = diceTransform.forward; break; // Assuming 1 maps to the forward face
            case 2: faceDirection = diceTransform.right; break; // Assuming 2 maps to the back face
            case 3: faceDirection = -diceTransform.up; break; // Assuming 3 maps to the up face
            case 4: faceDirection = diceTransform.up; break; // Assuming 4 maps to the down face
            case 5: faceDirection = -diceTransform.right; break; // Assuming 5 maps to the left face
            case 6: faceDirection = -diceTransform.forward; break; // Assuming 6 maps to the right face
            default: throw new ArgumentException("Invalid face number");
        }

        // Calculate the desired direction from the dice to the target
        Vector3 desiredDirection = (targetPosition - lookFrom).normalized;

        // Calculate the rotation needed to align the specified face with the desired direction
        Quaternion targetRotation = Quaternion.FromToRotation(faceDirection, desiredDirection) * diceTransform.rotation;

        // Return the rotation
        return targetRotation;
    }

    void PointFaceAtTargetWithCorrectUpMatchRotation(Transform diceTransform, int faceNumber, Transform target)
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
        Vector3 toTarget = (target.position - diceTransform.position).normalized;
        Quaternion faceToTargetRotation = Quaternion.FromToRotation(faceDirection, toTarget);

        // Step 2: Align the die's 'up' direction
        // Rotate the die's up direction into world space, then rotate it towards the target's up
        Vector3 worldFaceUpDirection = faceToTargetRotation * faceUpDirection;
        Quaternion upAlignmentRotation = Quaternion.FromToRotation(worldFaceUpDirection, target.up);

        // Combine the two rotations and apply to the dice
        Quaternion targetRotation = upAlignmentRotation * faceToTargetRotation * diceTransform.rotation;
        diceTransform.rotation = targetRotation;
    }

    //Often fails to point up correctly
    void PointFaceAtTargetWithCorrectUp(Transform diceTransform, int faceNumber, Transform target)
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
        Vector3 toTarget = (target.position - diceTransform.position).normalized;
        Quaternion faceToTargetRotation = Quaternion.FromToRotation(faceDirection, toTarget);
        diceTransform.rotation = faceToTargetRotation * diceTransform.rotation;

        // Step 2: Align the die's 'up' direction
        // Compute the axis to rotate around
        Vector3 rotationAxis = toTarget;
        // Compute the angle needed for the up direction to align with the target's up
        Vector3 currentUp = diceTransform.up;
        float angle = Vector3.SignedAngle(currentUp, target.up, rotationAxis);

        // Apply the additional rotation around the axis
        diceTransform.Rotate(rotationAxis, angle, Space.World);
    }

    //Often fails to point up correctly
    public Quaternion CalculateTargetRotationWithCorrectUp(Transform diceTransform, int faceNumber, Vector3 viewPosition, Transform target)
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

        // Step 1: Align the face with the target from the viewing position
        Vector3 toTarget = (target.position - viewPosition).normalized;
        Quaternion faceToTargetRotation = Quaternion.FromToRotation(faceDirection, toTarget);
        Quaternion initialRotation = faceToTargetRotation * diceTransform.rotation;

        // Step 2: Align the die's 'up' direction
        Vector3 rotationAxis = toTarget;
        Vector3 currentUp = initialRotation * faceUpDirection;
        float angle = Vector3.SignedAngle(currentUp, target.up, rotationAxis);

        // Compute the final rotation
        Quaternion finalRotation = Quaternion.AngleAxis(angle, rotationAxis) * initialRotation;

        return finalRotation;
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

    private IEnumerator SmoothMoveAndRotate(GameObject objectToMove, Vector3 targetLocation, Quaternion targetRotation, float duration = 1f)
    {
        Vector3 startPos = objectToMove.transform.position;
        Quaternion startRot = objectToMove.transform.rotation;

        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float percentageComplete = (Time.time - startTime) / duration;

            // Move the object to the interpolated position
            objectToMove.transform.position = Vector3.Lerp(startPos, targetLocation, percentageComplete);

            // Rotate the object to the interpolated rotation
            objectToMove.transform.rotation = Quaternion.Slerp(startRot, targetRotation, percentageComplete);

            yield return null;
        }

        // Ensure that object reaches the final position and rotation
        objectToMove.transform.position = targetLocation;
        objectToMove.transform.rotation = targetRotation;
    }

    public Quaternion CalculateRotation(GameObject objectToRotate, int faceNumber, Vector3 viewFromPosition, GameObject lookAtTarget)
    {
        Vector3 faceDirection;
        Vector3 faceUpDirection;

        // Map the face number to the corresponding local vector and its up vector
        switch (faceNumber)
        {
            case 1:
                faceDirection = objectToRotate.transform.forward;
                faceUpDirection = objectToRotate.transform.up;
                break;
            case 2:
                faceDirection = objectToRotate.transform.right;
                faceUpDirection = objectToRotate.transform.up;
                break;
            case 3:
                faceDirection = -objectToRotate.transform.up;
                faceUpDirection = objectToRotate.transform.right;
                break;
            case 4:
                faceDirection = objectToRotate.transform.up;
                faceUpDirection = objectToRotate.transform.right;
                break;
            case 5:
                faceDirection = -objectToRotate.transform.right;
                faceUpDirection = objectToRotate.transform.up;
                break;
            case 6:
                faceDirection = -objectToRotate.transform.forward;
                faceUpDirection = objectToRotate.transform.up;
                break;
            default:
                throw new ArgumentException("Invalid face number");
        }

        Vector3 toTarget = (lookAtTarget.transform.position - viewFromPosition).normalized;
        Quaternion faceToTargetRotation = Quaternion.FromToRotation(faceDirection, toTarget);



        return faceToTargetRotation;
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
