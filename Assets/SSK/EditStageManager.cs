using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  EditStageManager : MonoBehaviour {

    public GameObject selectedObject;
    public GameObject selectedObjectprev;
    public Quaternion selectedObjectRotate;
    public ObjectManager m_CurrentObject;
    string StageName;
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
        
        StageName = "Test";
        StageInt = -1;
        isEdit = true;
        ObjectSize = 1;
        wheels = new List<Rigidbody>();
    }

    private void Update()
    {
        if (isEdit)
        {
            EditKeyInput();
            EditStageRotate();
        }
        else
        {
            StageKeyInput();
            cam.transform.position = Root.transform.position + new Vector3(0, 5, -5);
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
        float speed = 1000f;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        foreach (var current in wheels)
        {
            current.AddTorque(Root.transform.right * v * speed);
            current.AddTorque(Root.transform.up * h);
        }
    }
    public void AddObject(Transform SelectedObject, Vector3 position)
    {
        //print(SelectedObject);
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
            if (newObj.GetComponent<ObjectManager>().isRoot)
            {
                Root = newObj;
            }
            if (newObj.tag == "Wheel")
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
        if(cam.position.x - transform.position.x > 1)
            cam.position -= Vector3.left * 0.1f ;
    }
    void camMoveBack()
    {
        if (cam.position.x - transform.position.x < 20)
            cam.position -= Vector3.right * 0.1f;
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
    }
    void StartEdit()
    {
        cam.transform.position = new Vector3(0, 5, -5);
        LoadObject(StageName + "temp");
        isEdit = true;
    }
    /*
    void connectJoint()
    {
        ObjectManager[] objectList;
        objectList = GetComponentsInChildren<ObjectManager>();
        for(int i = 0; i < objectList.Length-1; i++)
        {
            for (int j = i+1; j < objectList.Length; j++)
            {
                if((objectList[i].Position - objectList[j].Position).magnitude <= 1.1f)
                {
                    FixedJoint fj= objectList[i].gameObject.AddComponent<FixedJoint>();
                    fj.connectedBody = objectList[j].GetComponent<Rigidbody>();
                }
            }
        }
    }
    */
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
                //print(currentObject.gameObject.tag+ hit.rigidbody.gameObject.tag);
                if(currentObject.gameObject.tag == "Wheel" && hit.rigidbody.gameObject.tag=="Cube")
                {

                    //HingeJoint fj = currentObject.gameObject.AddComponent<HingeJoint>();
                    //fj.connectedBody = hit.rigidbody;
                    //fj.axis = Vector3.up;
                    
                    CharacterJoint cj=hit.rigidbody.gameObject.AddComponent<CharacterJoint>();
                    cj.connectedBody = currentObject.GetComponent<Rigidbody>();
                    cj.autoConfigureConnectedAnchor = false;
                    cj.connectedAnchor = Vector3.zero;
                    cj.anchor = hit.point;
                    cj.anchor = hit.transform.InverseTransformPoint(currentObject.transform.position);
                    //cj.axis = hit.transform.InverseTransformPoint(hit.point);
                    cj.axis = hit.transform.InverseTransformVector(Root.transform.right).normalized;
                    cj.swingAxis = hit.transform.InverseTransformVector(Root.transform.right).normalized;
                    SoftJointLimit cjLtl = cj.lowTwistLimit;
                    SoftJointLimit cjHtl = cj.highTwistLimit;
                    cjLtl.limit = -177;
                    cjLtl.contactDistance = 100000;
                    cjHtl.limit = 177;
                    cjHtl.contactDistance = 100000;
                    SoftJointLimit cjS1l = cj.swing1Limit;
                    SoftJointLimit cjS2l = cj.swing2Limit;
                    cjS1l.limit = 30;
                    cjS2l.limit = 0;
                    cjS1l.contactDistance = 100000;
                    cjS2l.contactDistance = 0;
                    cj.lowTwistLimit = cjLtl;
                    cj.highTwistLimit = cjHtl;
                    cj.swing1Limit = cjS1l;
                    cj.swing2Limit = cjS2l;

                    //cj.swingAxis = Vector3.forward;
                }
            }
        }
    }
}
