using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    public Transform[] transforms;
    private Transform[] originalTransforms;

    // Start is called before the first frame update
    void Start()
    {
        originalTransforms = new Transform[transforms.Length];

        for (int i = 0; i < transforms.Length; i++)
        {
            print("original position: " + transforms[i].position);
            originalTransforms[i] = transforms[i].transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ResetPositions();
        }
    }

    private void ResetPositions()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            print("resetting position: " + originalTransforms[i].transform.position);
            transforms[i].transform.SetPositionAndRotation(originalTransforms[i].transform.position, originalTransforms[i].rotation);
        }
    }
}
