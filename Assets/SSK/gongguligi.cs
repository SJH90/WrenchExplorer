using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gongguligi : MonoBehaviour {
    SpringJoint spring;
    Punch punch;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        spring=GetComponentInChildren<SpringJoint>();
        punch = GetComponentInChildren<Punch>();
    }
    Rigidbody rigid;
	// Update is called once per frame
	void Update () {
        float h=Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rigid.AddTorque(Vector3.back * h*200);
        rigid.AddTorque(Vector3.right * v*200);
        if (Input.GetKey(KeyCode.Z))
        {
            punch.punch();
        }
        Vector3 springForce= spring.currentForce;
    }
}
