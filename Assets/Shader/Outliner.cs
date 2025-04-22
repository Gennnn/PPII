using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Outliner : MonoBehaviour {
    public Shader edgeShader;
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
    [Range(-100.0f, 100.0f)]
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
    [Range(0.0f, 1f)]
    public float borderThickness = 0.1f;

    public Color borderColor = Color.black;

    private Material edgeMat;

    void Start() {
        edgeMat ??= new Material(edgeShader);
        edgeMat.hideFlags = HideFlags.HideAndDontSave;
        gameObject.GetComponent<SkinnedMeshRenderer>().material = edgeMat;
        
     
    }

    private void Update()
    {
        edgeMat.SetVector("_OutlineColor", borderColor);
        edgeMat.SetFloat("_OutlineWidth", borderThickness);
        edgeMat.SetFloat("_LineWidth", lineWidth);
        edgeMat.SetFloat("_CellSizeX", cellSizeX);
        edgeMat.SetFloat("_CellSizeY", cellSizeY);
        edgeMat.SetFloat("_MoveSpeed", moveSpeed);
        edgeMat.SetFloat("_XSlant", xSlant);
        edgeMat.SetFloat("_YSlant", ySlant);
        edgeMat.SetFloat("_MaximumLuminance", maximumLuminance);
        edgeMat.SetFloat("_MinimumLuminance", minimumLuminance);
        edgeMat.SetInt("_CurvedXOffset", curvedXOffset == true ? 1 : 0);
        edgeMat.SetInt("_CurvedYOffset", curvedYOffset == true ? 1 : 0);
        edgeMat.SetFloat("_CurveMagnitudeX", curvatureXMagnitude);
        edgeMat.SetFloat("_CurveMagnitudeY", curvatureYMagnitude);
        edgeMat.SetFloat("_CurveFrequencyX", curvatureXFrequency);
        edgeMat.SetFloat("_CurveFrequencyY", curvatureYFrequency);
        edgeMat.SetFloat("_PulseFrequency", pulseFrequency);
        edgeMat.SetVector("_GlowColor", glowColor);
        edgeMat.SetVector("_BackingColor", backingColor);
        edgeMat.SetFloat("_RotationX", rotationX);
        edgeMat.SetFloat("_RotationY", rotationY);
        edgeMat.SetFloat("_Perspective", perspective);
        edgeMat.SetInt("_UseDepth", useDepth == true ? 1 : 0);
        edgeMat.SetInt("_InvertTimeForX", invertTimeX == true ? 1 : 0);
        edgeMat.SetInt("_InvertTimeForY", invertTimeY == true ? 1 : 0);
        edgeMat.SetFloat("_DepthAggression", depthAggression);
        edgeMat.SetFloat("_GridXScale", gridScaleX);
        edgeMat.SetFloat("_GridYScale", gridScaleY);
        edgeMat.SetFloat("_GridXOffset", gridOffsetX);
        edgeMat.SetFloat("_GridYOffset", gridOffsetY);
        edgeMat.SetFloat("_GlowIntensity", glowIntensity);
        edgeMat.SetFloat("_GlowFalloff", glowFalloff);
    }


}
