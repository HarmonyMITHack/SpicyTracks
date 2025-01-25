using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VelocityTracker : MonoBehaviour
{
    public float Magnitude { get; private set; }
    public float magnitudeThreshold = 1f;
    private Vector3 lastPosition;
    //private float timeAtThreshold = 0f;
    private Rigidbody rb;

    public UnityEvent onThresholdReached, onThresholdEnded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
    }

    private void Fixed() 
    {
        Magnitude = rb.velocity.magnitude;
        //Magnitude = ((transform.position - lastPosition) / Time.fixedDeltaTime).magnitude;

        Debug.Log("Magnitude: " + Magnitude);
        Debug.Log("Transform Magnitude: " + ((transform.position - lastPosition) / Time.fixedDeltaTime).magnitude);

        lastPosition = transform.position;

        /*if (Magnitude > magnitudeThreshold)
        {
            timeAtThreshold += Time.fixedDeltaTime;

            if (timeAtThreshold > 0.2f)
            {
                onThresholdReached.Invoke();
            }
        }
        else
        {
            timeAtThreshold = 0f;
            onThresholdEnded.Invoke();
        }*/
    }
}
