using UnityEngine;
using UnityEngine.Events;

public class StartAction : MonoBehaviour
{
    public UnityEvent onEnter;  // Event to trigger on Enter
    public UnityEvent onExit;   // Event to trigger on Exit

    public void Start()
    {
        onEnter.Invoke();
    }

    public void Exit()
    {
        onExit.Invoke();
    }
}
