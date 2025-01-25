using Oculus.Interaction;
using UnityEngine;
using UnityEngine.UI;

public interface IRayInteractable
{
    void OnRayHit(RaycastHit hit);
}

public class ReticleController : MonoBehaviour
{
    [SerializeField]
    private GameObject crosshair; // The GameObject of the reticle

    [SerializeField]
    private Camera CameraFacing;

    private Vector3 originalScale;
    private Vector3 currentScale;
    private Vector3 lastKnownPosition;
    private float lastKnownDistance;

    private RaycastHit hitInfo;
    private RayInteractable lastHitInteractable = null; // Track the last hit interactable object

    void Start()
    {
        originalScale = transform.localScale;
        currentScale = originalScale;
        lastKnownPosition = transform.position;
        lastKnownDistance = CameraFacing.farClipPlane * 0.55f;
    }

    private void Update()
    {
        if (crosshair == null)
        {
            Debug.LogError("Crosshair is not assigned.");
            return;
        }

        // Perform the custom raycast
        Ray ray = new Ray(CameraFacing.transform.position, CameraFacing.transform.forward);
        if (Physics.Raycast(ray, out hitInfo))
        {
            // If a collision is detected, update the last known position and distance
            lastKnownPosition = hitInfo.point;
            lastKnownDistance = hitInfo.distance;

            // Check if the hit object is interactable
            RayInteractable interactable = hitInfo.collider.gameObject.GetComponent<RayInteractable>();
            if (interactable != null)
            {
                // If we hit a new interactable object, handle the CallStart
                if (lastHitInteractable != interactable)
                {
                    // Call CallStop on the last interactable (if any)
                    if (lastHitInteractable != null)
                    {
                        StartAction lastStartAction = lastHitInteractable.GetComponent<StartAction>();
                        if (lastStartAction != null)
                        {
                            crosshair.GetComponent<Image>().enabled = false;
                            lastStartAction.CallExit(); // Stop the previous interaction
                        }
                    }

                    // Call CallStart on the new interactable
                    //Debug.Log("Hit interactable: " + hitInfo.collider.name);
                    StartAction startAction = hitInfo.collider.gameObject.GetComponent<StartAction>();
                    if (startAction != null)
                    {
                        crosshair.GetComponent<Image>().enabled = true;
                        startAction.CallStart();
                    }

                    // Update the last hit interactable

                    lastHitInteractable = interactable;
                }
            }
        }
        else
        {
            // No hit, so extend the ray to the far clip plane
            lastKnownPosition = ray.origin + ray.direction * CameraFacing.farClipPlane * 0.95f;
            lastKnownDistance = CameraFacing.farClipPlane * 0.95f;

            // If we were previously hitting an interactable, call CallStop
            if (lastHitInteractable != null)
            {
                StartAction stopAction = lastHitInteractable.GetComponent<StartAction>();
                if (stopAction != null)
                {
                    crosshair.GetComponent<Image>().enabled = false;
                    stopAction.CallExit(); // Stop interaction if raycast moves away from the object
                }

                lastHitInteractable = null; // Reset the interactable tracker
            }
        }

        // Non-linear scaling factor to reduce "slipping" effect
        if (lastKnownDistance < 10)
        {
            lastKnownDistance *= 1 + 5 * Mathf.Exp(-lastKnownDistance);
        }

        // Smoothly adjust the scale to reduce abrupt changes
        float smoothFactor = 1f; // Adjust the smoothness as needed
        currentScale = Vector3.Lerp(currentScale, originalScale * lastKnownDistance, smoothFactor);
        transform.localScale = currentScale;

        // Smoothly transition to the new reticle position
        transform.position = Vector3.Lerp(transform.position, lastKnownPosition, smoothFactor);

        // Ensure the reticle is oriented towards the camera for consistent visibility
        transform.LookAt(CameraFacing.transform.position);
        transform.Rotate(0f, 180f, 0f);
    }
}
