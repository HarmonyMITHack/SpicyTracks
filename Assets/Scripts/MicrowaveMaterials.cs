using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveMaterials : MonoBehaviour
{
    public Material recordMaterial;
    public Material restartMaterial;
    public Renderer m_Renderer;

    public void MaterialToRecord()
    {
        m_Renderer.material = recordMaterial;
    }

    public void MaterialToRestart()
    {
        m_Renderer.material = restartMaterial;
    }
}
