using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gongguligi : MonoBehaviour {

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
    }
    Rigidbody rigid;
	// Update is called once per frame
	void Update () {
        float h=Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rigid.AddTorque(Vector3.back * h);
        rigid.AddTorque(Vector3.right * v);
    }
}
