using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class OnHandPokeUnityEvents : MonoBehaviour
{
    [Header("Unity Events")]
    public UnityEvent onPokeStart;   // Event triggered when the object is poked
    public UnityEvent onPokeEnd;    // Event triggered when the poke ends

    private PokeInteractable pokeInteractable;

    void Awake()
    {
        // Get the PokeInteractable component
        pokeInteractable = GetComponent<PokeInteractable>();

        // Subscribe to poke events
        pokeInteractable.WhenPointerEventRaised += HandlePointerEvent;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        pokeInteractable.WhenPointerEventRaised -= HandlePointerEvent;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        // Check the event type and invoke UnityEvents
        if (evt.Type == PointerEventType.Select)
        {
            onPokeStart?.Invoke();
            Debug.Log("Poke Started");
        }
        else if (evt.Type == PointerEventType.Unselect)
        {
            onPokeEnd?.Invoke();
            Debug.Log("Poke Ended");
        }
    }
}
