using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gongguligi : MonoBehaviour {
    SpringJoint[] springs;
    Punch[] punchs;
    Rigidbody[] rigids;
    public Transform head;
    HingeJoint[] hingeJoints;
    // Use this for initialization
    void Start () {
        rigids = GetComponentsInChildren<Rigidbody>();
        springs = GetComponentsInChildren<SpringJoint>();
        punchs = GetComponentsInChildren<Punch>();
        hingeJoints = GetComponentsInChildren<HingeJoint>();
    }
    
	// Update is called once per frame
	void Update () {
        float h=Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        foreach (var hinge in hingeJoints)
        {
            Rigidbody rigid = hinge.GetComponent<Rigidbody>();
            
            //rigid.AddTorque(hinge. * h * 800);
            rigid.AddTorque(hinge.connectedBody.transform.rotation * 
                Vector3.Cross(hinge.connectedAnchor, Vector3.forward) *
                hinge.axis.x *
                h * 2000);
            /*
            print(hinge.connectedBody.transform.rotation*
                Vector3.Cross(hinge.connectedAnchor, Vector3.forward) *
                hinge.axis.x *
                -h * 800);
                */
            rigid.AddTorque(hinge.connectedBody.transform.rotation * hinge.connectedAnchor.normalized * hinge.axis.x * -v * 800);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            foreach (var punch in punchs)
            {
                punch.punch();
            }
        }
        //Vector3 springForce= spring.currentForce;
    }
}
