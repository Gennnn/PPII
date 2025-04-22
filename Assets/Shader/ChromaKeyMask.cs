using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ChromaKeyMask : MonoBehaviour {
    public Shader chromaShader;

    public Color chromaColor;
    [Range(0.0f, 0.1f)]
    public float threshold = 0.01f;

    private Material chromaMat;

    void Start() {
        chromaMat ??= new Material(chromaShader);
        chromaMat.hideFlags = HideFlags.HideAndDontSave;

        Camera cam = GetComponent<Camera>();
        cam.backgroundColor = new Color(chromaColor.r, chromaColor.g, chromaColor.b, 1.0f);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.SetRenderTarget(destination);
        GL.Clear(true, true, chromaColor);
        chromaMat.SetColor("_ChromaColor", chromaColor);
        chromaMat.SetFloat("_Threshold", threshold);
        Graphics.Blit(source, destination, chromaMat);
    }
}
