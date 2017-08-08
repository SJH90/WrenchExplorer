using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour {
    Rigidbody rigid;
    //SpringJoint spring;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        //spring = GetComponent<SpringJoint>();

    }
    float force = 50000;
	// Update is called once per frame
	void Update () {
		
	}
    private void FixedUpdate()
    {
        if (true)
        {

        }
    }
    public void punch()
    {
        print("punch!");
        rigid.AddForce(force* transform.forward);
    }
}
