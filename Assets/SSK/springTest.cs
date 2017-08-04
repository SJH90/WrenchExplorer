using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class springTest : MonoBehaviour {
    public SpringJoint spring;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        spring.axis = new Vector3(1, 1, 0);
        print(spring.axis);
        if (Input.GetKey(KeyCode.X))
        {
            spring.connectedBody.AddForce(Vector3.down * 1000);
        }
	}
    private void FixedUpdate()
    {
        spring.connectedBody.velocity = Vector3.Dot(spring.connectedBody.velocity, -transform.up)*-transform.up;
    }
}
