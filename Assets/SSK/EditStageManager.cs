﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  EditStageManager : MonoBehaviour {

    public GameObject selectedObject;
    public GameObject selectedObjectprev;
    public Quaternion selectedObjectRotate;
    public ObjectManager m_CurrentObject;
    public string StageName;
    public GameObject ter;
    int StageInt;
    [SerializeField]
    GameObject[] prefabs;
    [SerializeField]
    GameObject[] prefabsPrev;
    [SerializeField]
    Transform cam;
    bool isEdit;
    int ObjectSize;
    [SerializeField]
    GameObject Root;

    [SerializeField]
    List<Rigidbody> wheels;

    public bool IsEdit
    {
        get
        {
            return isEdit;
        }
    }
    private void Awake()
    {
        
        selectedObject = prefabs[0];
        selectedObjectprev = prefabsPrev[0];
        selectedObjectRotate = Quaternion.Euler(new Vector3(0, 0, 0));
        StageName=PlayerPrefs.GetString("StageName","Test");
        StageInt = -1;
        isEdit = true;
        ObjectSize = 1;
        TerrainData td1 = Resources.Load<TerrainData>("Terrains/BK Basic Terrain Data");
        Terrain.DestroyObject(ter);
        ter = Terrain.CreateTerrainGameObject(td1);
        ter.transform.position = new Vector3(-250, -10.002f, -250);
        TerrainData td2= Resources.Load<TerrainData>("Terrains/" + StageName);
        ter = Terrain.CreateTerrainGameObject(td2);
        ter.transform.position = new Vector3(-250, -10, -250);
        wheels = new List<Rigidbody>();
        
    }
    float upPosition=5;
    float fowardPosition=5;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
            debugMode();
        if (isEdit)
        {
            EditKeyInput();
            EditStageRotate();
            //cam.transform.position = Vector3.zero;
        }
        else
        {
            StageKeyInput();
            cam.transform.position = Root.transform.position + Root.transform.up * upPosition + Root.transform.forward * -fowardPosition;
        }
    }
    private void EditKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartStage();
        }
        if (Input.GetKey(KeyCode.T))
        {
            camMoveFront();
        }
        if (Input.GetKey(KeyCode.G))
        {
            camMoveBack();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ObejctChange(0);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ObejctChange(1);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ObejctChange(2);

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedObject = null;
            selectedObjectprev = null;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            selectedObjectRotate *= Quaternion.Euler(new Vector3(0, 0, 90));

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            selectedObjectRotate *= Quaternion.Euler(new Vector3(90, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveObject(StageName);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadObject(StageName);
        }
    }
    void StageKeyInput()
    {
        StageMove();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartEdit();
            
        }
    }
    private void EditStageRotate()
    {
        float speed = 5f;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        transform.RotateAround(transform.position, Vector3.up, -h * speed);
        transform.RotateAround(transform.position, Vector3.right, -v * speed);
        
    }
    void StageMove()
    {
        float speed = 50f;
        

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        foreach (var current in wheels)
        {
            //current.AddTorque(-Root.transform.right * v * speed);
            current.AddTorque(Root.transform.up * h * 10);
            current.AddTorque(Root.transform.right * v * speed);
            //current.AddTorque(abs(current.transform.up) * v * speed);
            //print(abs(current.transform.up));
            
        }
    }
    public void AddObject(Transform SelectedObject, Vector3 position)
    {
        if (selectedObject != null && SelectedObject.tag !="Wheel")
        {
            GameObject newObj = Instantiate(selectedObject, SelectedObject.root);
            if (newObj.tag == "Wheel")
            {
                wheels.Add(newObj.GetComponent<Rigidbody>());
            }
            ObjectSize++;
            newObj.name = selectedObject.name + ObjectSize;

            newObj.transform.position = position;
            newObj.transform.rotation = SelectedObject.root.rotation * selectedObjectRotate;
        }
        else if (selectedObject != null && SelectedObject.tag == "Wheel")
        {

        }
        else
        {
            if (!SelectedObject.GetComponent<ObjectManager>().isRoot)
            {
                if (SelectedObject.tag == "Wheel")
                {
                    wheels.Remove(SelectedObject.GetComponent<Rigidbody>());
                }
                Destroy(SelectedObject.gameObject);
                ObjectSize--;
            }
        }
    }
    void SaveObject(string name)
    {
        Quaternion temp = transform.rotation;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        ObjectManager[] objectList;
        objectList = GetComponentsInChildren<ObjectManager>();
        ObjectListData[] objListData = new ObjectListData[objectList.Length];
        //print(objectList.Length);
        for(int i = 0; i < objectList.Length; i++)
        {
            if(objectList[i].tag == "Cube")
                objListData[i] = new ObjectListData(objectList[i], 0);
            if (objectList[i].tag == "Wheel")
                objListData[i] = new ObjectListData(objectList[i], 1);
            if (objectList[i].tag == "FixedWheel")
                objListData[i] = new ObjectListData(objectList[i], 2);
        }
        StageObjectData data = new StageObjectData(StageInt, name, objListData);
        DataManager.SaveToFile(data, name);
        transform.rotation = temp;

    }
    void LoadObject(string name)
    {
        
        transform.rotation = Quaternion.Euler(Vector3.zero);
        foreach (Transform child in transform)
        {
            child.name = "Destroy";
            Destroy(child.gameObject);
        }
        wheels.Clear();
        StageObjectData loaded = DataManager.LoadToFile<StageObjectData>(name);
        StageInt = loaded.number;
        for(int i = 0; i < loaded.objectSize; i++)
        {
            //if(loaded.objectList[i].type==1)
            GameObject newObj= loaded.objectList[i].DataToObject(prefabs[loaded.objectList[i].type],gameObject.transform);
            newObj.GetComponent<Rigidbody>().maxAngularVelocity = float.PositiveInfinity;
            if (newObj.GetComponent<ObjectManager>().isRoot)
            {
                Root = newObj;
            }
            if (newObj.tag == "Wheel" || newObj.tag == "FixedWheel")
            {
                wheels.Add(newObj.GetComponent<Rigidbody>());
            }
        }
    }
    public void ObejctChange(int index)
    {
        selectedObject = prefabs[index];
        selectedObjectprev = prefabsPrev[index];
        if (m_CurrentObject != null)
        {
            m_CurrentObject.Refresh();
        }
    }
    void camMoveFront()
    {
        //print(cam.position.z - transform.position.z);
        if(Mathf.Abs(cam.position.z - transform.position.z) >1)
            cam.position -= Vector3.forward * 0.1f ;
    }
    void camMoveBack()
    {
        if (Mathf.Abs(cam.position.z - transform.position.z) < 20)
            cam.position -= Vector3.back * 0.1f;
    }
    void StartStage()
    {
        Quaternion temp = transform.rotation;
        transform.rotation = Quaternion.Euler(Vector3.zero); 
        Rigidbody[] rigids = GetComponentsInChildren<Rigidbody>();
        SaveObject(StageName + "temp");
        connectJoint();
        foreach (var rigid in rigids)
        {
            rigid.isKinematic = false;
            rigid.useGravity = true;
        }
        cam.transform.position = Root.transform.position +new Vector3(0, 0, -5);
        isEdit = false;
        foreach(var com in GetComponentsInChildren<ObjectManager>())
        {
            com.StartStageInit();
        }
    }
    void StartEdit()
    {
        cam.transform.position = Vector3.zero;
        LoadObject(StageName + "temp");
        isEdit = true;
        foreach (var com in GetComponentsInChildren<ObjectManager>())
        {
            com.StartEditInit();
        }
    }
    bool isDebug;
    void debugMode()
    {
        ObjectManager[] objectList;
        objectList = GetComponentsInChildren<ObjectManager>();
        isDebug = !isDebug;
        foreach (var currentObject in objectList)
        {
            if (currentObject.gameObject.tag == "Cube")
            {
                Rigidbody rigid= currentObject.GetComponent<Rigidbody>();
                if (isDebug) {
                    rigid.constraints = RigidbodyConstraints.FreezeAll;
                    
                }
                else
                {
                    rigid.constraints = RigidbodyConstraints.None;
                }
                
            }
        }
    }

    Vector3 abs(Vector3 vec)
    {
        vec.x = Mathf.Abs(vec.x);
        vec.y = Mathf.Abs(vec.y);
        vec.z = Mathf.Abs(vec.z);
        return vec;
    }

    void connectJoint()
    {
        ObjectManager[] objectList;
        objectList = GetComponentsInChildren<ObjectManager>();
        foreach(var currentObject in objectList) {  
            RaycastHit hit;
            Ray ray = new Ray(currentObject.transform.position, currentObject.transform.up);

            if (Physics.Raycast(ray, out hit, 1.1f))
            {
                if(currentObject.gameObject.tag == "Cube"){
                    FixedJoint fj = currentObject.gameObject.AddComponent<FixedJoint>();
                    fj.connectedBody = hit.rigidbody;
                }
                if(currentObject.gameObject.tag == "Wheel" && hit.rigidbody.gameObject.tag=="Cube")
                {
                    CharacterJoint cj=hit.rigidbody.gameObject.AddComponent<CharacterJoint>();
                    cj.connectedBody = currentObject.GetComponent<Rigidbody>();
                    cj.autoConfigureConnectedAnchor = false;
                    cj.connectedAnchor = Vector3.zero;
                    cj.anchor = hit.point;
                    cj.anchor = hit.transform.InverseTransformPoint(currentObject.transform.position);
                    
                    cj.axis = hit.transform.InverseTransformVector(abs(hit.transform.position- hit.point)).normalized;
                    cj.swingAxis = hit.transform.InverseTransformVector(Root.transform.up).normalized;
                    SoftJointLimit cjLtl = cj.lowTwistLimit;
                    SoftJointLimit cjHtl = cj.highTwistLimit;
                    cjLtl.limit = -177;
                    cjLtl.contactDistance = 500;
                    cjHtl.limit = 177;
                    cjHtl.contactDistance = 500;
                    SoftJointLimit cjS1l = cj.swing1Limit;
                    SoftJointLimit cjS2l = cj.swing2Limit;
                    cjS1l.limit = 20;
                    cjS2l.limit = 0;
                    cjS1l.contactDistance = 0;
                    cjS2l.contactDistance = 0;
                    cj.lowTwistLimit = cjLtl;
                    cj.highTwistLimit = cjHtl;
                    cj.swing1Limit = cjS1l;
                    cj.swing2Limit = cjS2l;
                }
                if (currentObject.gameObject.tag == "FixedWheel" && hit.rigidbody.gameObject.tag == "Cube")
                {

                    HingeJoint fj = currentObject.gameObject.AddComponent<HingeJoint>();
                    fj.connectedBody = hit.rigidbody;
                    fj.axis = Vector3.up;
                    
                }

            }
        }
    }
}
