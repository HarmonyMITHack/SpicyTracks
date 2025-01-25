using UnityEngine;
using UnityEngine.Events;

public class StartAction : MonoBehaviour
{
    public UnityEvent onEnter;  // Event to trigger on Enter
    public UnityEvent onExit;   // Event to trigger on Exit

    public void CallStart()
    {
        onEnter.Invoke();
    }

    public void CallExit()
    {
        onExit.Invoke();
    }
}
