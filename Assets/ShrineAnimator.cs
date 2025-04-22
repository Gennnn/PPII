using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineAnimator : MonoBehaviour
{
    [SerializeField] Renderer shrineBase;
    [SerializeField] Renderer shrineMain;
    [SerializeField] Renderer ringOne;
    [SerializeField] Renderer ringTwo;

    public float mainRotSpeed = 5.0f;
    public float ringOneRotSpeed = 5.0f;
    public float ringTwoRotSpeed = 5.0f;
    void Start()
    {
        
    }

    void Update()
    {
        //Quaternion rot = Quaternion.Euler(shrineMain.transform.rotation.eulerAngles.x, shrineMain.transform.rotation.eulerAngles.y + (mainRotSpeed*Time.deltaTime), shrineMain.transform.rotation.eulerAngles.z);
        shrineMain.transform.Rotate(Vector3.up * mainRotSpeed * Time.deltaTime, Space.World);
        ringOne.transform.Rotate(Vector3.up * ringOneRotSpeed * Time.deltaTime, Space.Self);
        ringOne.transform.Rotate(Vector3.forward * ringOneRotSpeed * Time.deltaTime, Space.Self);
        ringTwo.transform.Rotate(Vector3.up * ringTwoRotSpeed * Time.deltaTime, Space.Self);
        ringTwo.transform.Rotate(Vector3.forward * ringTwoRotSpeed * Time.deltaTime, Space.Self);
    }
}
