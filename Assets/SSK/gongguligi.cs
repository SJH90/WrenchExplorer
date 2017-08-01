using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gongguligi : MonoBehaviour {
    SpringJoint spring;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        spring=GetComponentInChildren<SpringJoint>();
    }
    Rigidbody rigid;
	// Update is called once per frame
	void Update () {
        float h=Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rigid.AddTorque(Vector3.back * h*2);
        rigid.AddTorque(Vector3.right * v*2);
        Vector3 springForce= spring.currentForce;
    }
}
