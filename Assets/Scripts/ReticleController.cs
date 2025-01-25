using Oculus.Interaction;
using UnityEngine;

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
                Debug.Log("Hit interactable: " + hitInfo.collider.name);
                StartAction startAction = hitInfo.collider.gameObject.GetComponent<StartAction>();
                if (startAction != null)
                {
                    startAction.Start();
                }
            }
        }
        else
        {
            // No hit, so extend the ray to the far clip plane
            lastKnownPosition = ray.origin + ray.direction * CameraFacing.farClipPlane * 0.95f;
            lastKnownDistance = CameraFacing.farClipPlane * 0.95f;
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
