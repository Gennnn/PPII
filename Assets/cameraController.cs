using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int verticalSens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float rotX;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        transform.Rotate(Vector3.up * mouseX * verticalSens * Time.deltaTime);
    }
}
