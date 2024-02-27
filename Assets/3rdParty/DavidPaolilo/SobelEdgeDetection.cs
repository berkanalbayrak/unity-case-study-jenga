using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class SobelEdgeDetection : MonoBehaviour
{
    [SerializeField] private Shader shader;

    [SerializeField] private float thickness = 1;
    [SerializeField] private float depthMultiplier = 1;
    [SerializeField] private float depthBias = 1;
    [SerializeField] private float normalMultiplier = 1;
    [SerializeField] private float normalBias = 10;
    [SerializeField] private Color color = Color.black;

    private Material material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material == null)
            material = new Material(shader);

        material.SetFloat("_OutlineThickness", thickness);
        material.SetFloat("_OutlineDepthMultiplier", depthMultiplier);
        material.SetFloat("_OutlineDepthBias", depthBias);
        material.SetFloat("_OutlineNormalMultiplier", normalMultiplier);
        material.SetFloat("_OutlineNormalBias", normalBias);
        material.SetColor("_OutlineColor", color);

        Graphics.Blit(source, destination, material);
    }
}
