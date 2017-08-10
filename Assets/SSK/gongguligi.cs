using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gongguligi : MonoBehaviour {
    public CharacterJoint[] springs;
    Punch[] punchs;
    //Rigidbody[] rigids;
    public Transform head;
    HingeJoint[] hingeJoints;
    float ch;
    float cv;

    // Use this for initialization
    void Start () {
        //rigids = GetComponentsInChildren<Rigidbody>();
        //springs = GetComponentsInChildren<SpringJoint>();
        punchs = GetComponentsInChildren<Punch>();
        hingeJoints = GetComponentsInChildren<HingeJoint>();
        ch = 10000;
        cv = 800;
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
                h * ch);
            /*
            print(hinge.connectedBody.transform.rotation*
                Vector3.Cross(hinge.connectedAnchor, Vector3.forward) *
                hinge.axis.x *
                -h * 800);
                */
            rigid.AddTorque(hinge.connectedBody.transform.rotation * hinge.connectedAnchor.normalized * hinge.axis.x * -v * cv);
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
