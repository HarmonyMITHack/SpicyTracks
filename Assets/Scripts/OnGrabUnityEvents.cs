using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class OnGrabUnityEvents : MonoBehaviour
{
    [Header("Unity Events")]
    public UnityEvent onGrab;   // Event triggered when the object is grabbed
    public UnityEvent onRelease; // Event triggered when the object is released

    private GrabInteractable grabInteractable;

    void Awake()
    {
        // Get the GrabInteractable component
        grabInteractable = GetComponent<GrabInteractable>();

        // Subscribe to grab events
        grabInteractable.WhenPointerEventRaised += HandlePointerEvent;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        grabInteractable.WhenPointerEventRaised -= HandlePointerEvent;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        // Check the event type and invoke UnityEvents
        if (evt.Type == PointerEventType.Select)
        {
            onGrab?.Invoke();
            Debug.Log("Grabbed");
        }
        else if (evt.Type == PointerEventType.Unselect)
        {
            onRelease?.Invoke();
        }
    }
}