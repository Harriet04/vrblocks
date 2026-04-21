using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public float rotateSpeed = 1.0f;

    public Material SkyboxMaterial;

    void Start()
    {
        // Change the global scene skybox to chosen material
        RenderSettings.skybox = SkyboxMaterial;

    }
    void Update()
    {
        // Use RenderSettings to access the active skybox material
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);

    }
}