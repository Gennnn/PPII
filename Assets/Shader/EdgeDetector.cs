using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EdgeDetector : MonoBehaviour {
    public Shader edgeShader;
    
    public Color borderColor;

    [Range(0.0f, 0.1f)]
    public float depthEdgeThresh = 0.01f;

    private Material edgeMat;

    void Start() {
        edgeMat ??= new Material(edgeShader);
        edgeMat.hideFlags = HideFlags.HideAndDontSave;
        
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        edgeMat.SetColor("_BorderColor", borderColor);
        edgeMat.SetFloat("_DepthThreshold", depthEdgeThresh);
        Graphics.Blit(source, destination, edgeMat);
    }
}
