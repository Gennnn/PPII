using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class ScrollingGridTex : MonoBehaviour
{
    public Shader gridShader;
    
    [Header("Grid 1 Settings")]
    [Range(0.0f, 1.0f)]
    public float lineWidth = 0.2f;

    [Range(0.0f, 1.0f)]
    public float cellSizeX = 0.1f;

    [Range(0.0f, 1.0f)]
    public float cellSizeY = 0.1f;

    [Range(0.0f, 5.0f)]
    public float moveSpeed = 0.1f;

    public bool invertTimeX = false;
    public bool invertTimeY = false;

    public float xSlant = 0.0f;
    public float ySlant = 0.0f;

    [Range(0.0f, 1.0f)]
    public float maximumLuminance = 0.33f;

    [Range(0.0f, 1.0f)]
    public float minimumLuminance = 0.0f;

    public bool curvedXOffset = false;

    public bool curvedYOffset = false;

    [Range(-1.0f, 1.0f)]
    public float curvatureXMagnitude = 0.0f;
    [Range(-1.0f, 1.0f)]
    public float curvatureYMagnitude = 0.0f;
    [Range (-100.0f, 100.0f)]
    public float curvatureXFrequency = 5.0f;
    [Range(-100.0f, 100.0f)]
    public float curvatureYFrequency = 5.0f;

    [Range(0.0f, 10.0f)]
    public float pulseFrequency = 0.8f;

    public Color glowColor = Color.white;
    public Color backingColor = Color.black;

    [Range(-360.0f, 360.0f)]
    public float rotationX = 0.0f;

    [Range(-360.0f, 360.0f)]
    public float rotationY = 0.0f;

    [Range(0.0f, 2.0f)]
    public float perspective = 1.0f;

    public bool useDepth = true;
    [Range(0.0f, 1.0f)]
    public float depthAggression = 0.0f;

    [Range(0.0f, 1.0f)]
    public float gridScaleX = 1.0f;
    [Range(0.0f, 1.0f)]
    public float gridScaleY = 1.0f;

    [Range(-10.0f, 10.0f)]
    public float gridOffsetX = 0.0f;
    [Range(-10.0f, 10.0f)]
    public float gridOffsetY = 0.0f;

    [Range(0.0f, 5.0f)]
    public float glowIntensity = 1.0f;
    [Range(0.01f, 0.5f)]
    public float glowFalloff = 0.1f;

    private Material gridMat;
    public Material depthOnlyMat;

    private void OnEnable()
    {
        gridMat ??= new Material(gridShader);
        gridMat.hideFlags = HideFlags.HideAndDontSave;
        gameObject.GetComponent<Renderer>().material = gridMat;
        
        //gameObject.GetComponent<MeshRenderer>().material ??= gridMat;
        

    }
    
    private void Update()
    {
        
        gridMat.SetFloat("_LineWidth", lineWidth);
        gridMat.SetFloat("_CellSizeX", cellSizeX);
        gridMat.SetFloat("_CellSizeY", cellSizeY);
        gridMat.SetFloat("_MoveSpeed", moveSpeed);
        gridMat.SetFloat("_XSlant", xSlant);
        gridMat.SetFloat("_YSlant", ySlant);
        gridMat.SetFloat("_MaximumLuminance", maximumLuminance);
        gridMat.SetFloat("_MinimumLuminance", minimumLuminance);
        gridMat.SetInt("_CurvedXOffset", curvedXOffset == true ? 1 : 0);
        gridMat.SetInt("_CurvedYOffset", curvedYOffset == true ? 1 : 0);
        gridMat.SetFloat("_CurveMagnitudeX", curvatureXMagnitude);
        gridMat.SetFloat("_CurveMagnitudeY", curvatureYMagnitude);
        gridMat.SetFloat("_CurveFrequencyX", curvatureXFrequency);
        gridMat.SetFloat("_CurveFrequencyY", curvatureYFrequency);
        gridMat.SetFloat("_PulseFrequency", pulseFrequency);
        gridMat.SetVector("_GlowColor", glowColor);
        gridMat.SetVector("_BackingColor", backingColor);
        gridMat.SetFloat("_RotationX", rotationX);
        gridMat.SetFloat("_RotationY", rotationY);
        gridMat.SetFloat("_Perspective", perspective);
        gridMat.SetInt("_UseDepth", useDepth == true ? 1 : 0);
        gridMat.SetInt("_InvertTimeForX", invertTimeX == true ? 1 : 0);
        gridMat.SetInt("_InvertTimeForY", invertTimeY == true ? 1 : 0);
        gridMat.SetFloat("_DepthAggression", depthAggression);
        gridMat.SetFloat("_GridXScale", gridScaleX);
        gridMat.SetFloat("_GridYScale", gridScaleY);
        gridMat.SetFloat("_GridXOffset", gridOffsetX);
        gridMat.SetFloat("_GridYOffset", gridOffsetY);
        gridMat.SetFloat("_GlowIntensity", glowIntensity);
        gridMat.SetFloat("_GlowFalloff", glowFalloff);
    }


}
