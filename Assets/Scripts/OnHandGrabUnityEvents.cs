using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Events;

public class OnHandGrabUnityEvents : MonoBehaviour
{
    [Header("Unity Events")]
    public UnityEvent onGrab;   // Event triggered when the object is grabbed
    public UnityEvent onRelease; // Event triggered when the object is released

    private HandGrabInteractable handGrabInteractable;

    void Awake()
    {
        // Get the GrabInteractable component
        handGrabInteractable = GetComponent<HandGrabInteractable>();

        // Subscribe to grab events
        handGrabInteractable.WhenPointerEventRaised += HandlePointerEvent;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        handGrabInteractable.WhenPointerEventRaised -= HandlePointerEvent;
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