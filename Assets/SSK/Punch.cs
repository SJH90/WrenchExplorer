using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour {
    Rigidbody rigid;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void punch()
    {
        print("punch!");
        rigid.AddForce(100 * Vector3.forward);
    }
}
