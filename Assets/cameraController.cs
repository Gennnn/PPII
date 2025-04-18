using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int horizontalSens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    [SerializeField] float sprintingFOVChange = 1.25f;
    [SerializeField] float defaultFOV = 75f;
    [SerializeField] float tweenSpeed = 0.25f;

    float rotX;

    playerController playerController;

    Dictionary<Camera, Tween> fovAdjustmentTweens = new Dictionary<Camera, Tween>();
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Camera cam))
            {
                cam.fieldOfView = defaultFOV;
                fovAdjustmentTweens.Add(cam, null);
            }
        }

        playerController = GetComponent<playerController>();
        playerController.sprintChangeEvent.AddListener(AdjustCameraFOV);
    }

    private void OnDestroy()
    {
        playerController.sprintChangeEvent.RemoveListener(AdjustCameraFOV);
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        if (invertY )
        {
            rotX += mouseY;
        } else
        {
            rotX -= mouseY;
        }
        
        rotX = Mathf.Clamp( rotX, lockVertMin, lockVertMax );
        //transform.localRotation = Quaternion.Euler(rotX, 0, 0 );
        foreach (Transform child in transform)
        {
            child.localRotation = Quaternion.Euler(rotX, 0, 0);
            
        }
        transform.Rotate(Vector3.up * mouseX * horizontalSens);
    }

    void AdjustCameraFOV(bool isSprinting)
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Camera cam))
            {
                if (fovAdjustmentTweens[cam] != null)
                {
                    fovAdjustmentTweens[cam].Kill(false);                  
                }
                fovAdjustmentTweens[cam] = cam.DOFieldOfView(isSprinting ? defaultFOV * sprintingFOVChange : defaultFOV, tweenSpeed).OnComplete( ()=>
                {
                    fovAdjustmentTweens[cam] = null;
                });
            }
        }
    }
}
