using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways] //affects editor too, always run

public class CameraRig : MonoBehaviour
{
    [SerializeField] Transform followTransform;
    [SerializeField] float armLength;
    [SerializeField] Transform camTrans;
    [SerializeField] Transform camArm;
    [SerializeField] float turnSpeed;

    [SerializeField] [Range(0,1)] float followDamping;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        camTrans.position = camArm.position - camTrans.forward * armLength;

        transform.position = Vector3.Lerp(transform.position, followTransform.position, (1-followDamping)*Time.deltaTime*20);
    }
    public void AddYawInput(float amt)
    {
        transform.Rotate(Vector3.up, amt * Time.deltaTime * turnSpeed);
    }
}
