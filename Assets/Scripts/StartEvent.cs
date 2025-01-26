using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartEvent : MonoBehaviour
{
    public UnityEvent onStart;

    // Start is called before the first frame update
    void Start()
    {
        onStart.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
