using UnityEngine;

public class AdjustPitchOnShake : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource for the sound
    public Transform fryingPan;     // The frying pan object

    public float basePitch = 1.0f;         // The default pitch when not moving
    public float maxOscillation = 0.5f;   // Maximum oscillation amplitude
    public float sensitivity = 1.0f;      // Sensitivity to movement
    public float oscillationSpeed = 2.0f; // Speed of pitch oscillation
    public float smoothReturnSpeed = 2.0f; // Speed of returning to base pitch when movement stops

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private float oscillationOffset;
    private float movementIntensity;

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
            return;
        }

        if (fryingPan == null)
        {
            Debug.LogError("Frying Pan is not assigned!");
            return;
        }

        // Initialize position and rotation
        lastPosition = fryingPan.position;
        lastRotation = fryingPan.rotation;

        // Play the audio on loop
        audioSource.loop = true;
        audioSource.pitch = basePitch;
        audioSource.Play();
    }

    void Update()
    {
        // Calculate linear velocity
        Vector3 velocity = (fryingPan.position - lastPosition) / Time.deltaTime;

        // Calculate angular velocity
        Quaternion deltaRotation = fryingPan.rotation * Quaternion.Inverse(lastRotation);
        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);
        float angularVelocity = angle / Time.deltaTime;

        // Calculate current movement intensity
        float currentIntensity = velocity.magnitude + angularVelocity * sensitivity;

        // Smoothly adjust movement intensity
        movementIntensity = Mathf.Lerp(movementIntensity, currentIntensity, Time.deltaTime * 10f);

        // Update oscillation offset based on movement intensity
        oscillationOffset += Time.deltaTime * oscillationSpeed;

        // Calculate oscillation effect
        float oscillation = Mathf.Sin(oscillationOffset) * Mathf.Min(movementIntensity * maxOscillation, maxOscillation);

        // Smoothly adjust pitch
        float targetPitch = basePitch + oscillation;
        audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, Time.deltaTime * 10f);

        // Gradually return pitch to base pitch when movement stops
        if (movementIntensity < 0.01f)
        {
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, basePitch, Time.deltaTime * smoothReturnSpeed);
        }

        // Update last position and rotation
        lastPosition = fryingPan.position;
        lastRotation = fryingPan.rotation;
    }
}
