using System.Collections;
using System.Collections.Generic;
using Oculus.VoiceSDK.UX;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Renderer m_Renderer;
    private Camera targetCamera;
    private float lastInteracted = 0f;
    private bool interacting = false;

    private void Update()
    {
        lastInteracted += Time.deltaTime;

        if (lastInteracted > 5f)
        {
            if (interacting)
            {
                lastInteracted = 0f;
            }
            else
            {
                m_Renderer.enabled = true;
            }
        }
    }

    void LateUpdate()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera != null)
        {
            transform.forward = targetCamera.transform.forward;
        }
    }

    public void Interacted()
    {
        lastInteracted = 0f;
        m_Renderer.enabled = false;
    }

    public void Grabbed()
    {
        lastInteracted = 0f;
        m_Renderer.enabled = false;
        interacting = true;
    }

    public void Released()
    {
        interacting = false;
    }
}
