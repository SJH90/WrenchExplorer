using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour {

    public GameObject view;
    Camera cam;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = view.transform.position + view.transform.forward * -10 + view.transform.up * 5;
        transform.LookAt(view.transform);

    }
}
