using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class Test : MonoBehaviour {

    Rigidbody rigid;
    HingeJoint hinge;
    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
        hinge = GetComponent<HingeJoint>();
	}
	
	// Update is called once per frame
	void Update () {
        //rigid.AddTorque(transform.up * 100);
        float speed = 1000f;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rigid.AddTorque(Vector3.up * h);
        rigid.AddTorque(Vector3.back * v*speed);
    }
    
}
