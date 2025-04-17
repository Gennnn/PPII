using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class TiledImage : MonoBehaviour
{
    public Shader tileShader;

    public Vector2 direction;
    public float scale = 1f;
    [Range (0f, 360f)]
    public float rotationDegrees;

    public Texture2D tiledTexture;

    private Material tileMat;

    private void OnEnable()
    {
        tileMat ??= new Material(tileShader);
        tileMat.hideFlags = HideFlags.HideAndDontSave;
        gameObject.GetComponent<Renderer>().material = tileMat;
        
        //gameObject.GetComponent<MeshRenderer>().material ??= gridMat;
        

    }
    
    private void Update()
    {
        tileMat.SetTexture("_TiledTexture", tiledTexture);
        tileMat.SetVector("_Direction", direction);
        tileMat.SetFloat("_Scale", scale);
        tileMat.SetFloat("_RotationDegrees", rotationDegrees);
    }


}
