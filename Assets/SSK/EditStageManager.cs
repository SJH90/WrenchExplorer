using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditStageManager : MonoBehaviour {

    // Use this for initialization
    /*
    [SerializeField]
    private GameObject Obj1;
    [SerializeField]
    private GameObject Obj1pre;
    [SerializeField]
    private GameObject Obj2;
    [SerializeField]
    private GameObject Obj2pre;
    */

    public GameObject selectedObject;
    public GameObject selectedObjectprev;
    public Quaternion selectedObjectRotate;
    EditStageManager m_EMScript;
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
    private void Awake()
    {
        selectedObject = prefabs[0];
        selectedObjectprev = prefabsPrev[0];
        selectedObjectRotate = Quaternion.Euler(new Vector3(0, 0, 0));
        m_EMScript = GetComponent<EditStageManager>();
        StageName = "Test";
        StageInt = -1;
        isEdit = true;
        ObjectSize = 1;
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            jointTest();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isEdit)
                StartStage();
            else
                StartEdit();
        }
        if (Input.GetKey(KeyCode.T))
        {
            camMoveFront();
        }
        if (Input.GetKey(KeyCode.G))
        {
            camMoveBack();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedObject = prefabs[0];
            selectedObjectprev = prefabsPrev[0];
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedObject = prefabs[1];
            selectedObjectprev = prefabsPrev[1];
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedObject = null;
            selectedObjectprev = null;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            selectedObjectRotate *= Quaternion.Euler(new Vector3(0, 0, 90));
        }
        if (Input.GetKeyDown(KeyCode.G))
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
        StageRotate();
    }

    private void StageRotate()
    {
        float speed = 5f;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        transform.RotateAround(transform.position, Vector3.up, -h * speed);
        transform.RotateAround(transform.position, Vector3.right, -v * speed);
    }
    public void AddObject(Transform SelectedObject, Vector3 view)
    {
        if (m_EMScript.selectedObject != null)
        {
            GameObject newObj = Instantiate(m_EMScript.selectedObject, SelectedObject.root);
            ObjectSize++;
            newObj.name=selectedObject.name + ObjectSize;

            newObj.transform.position = SelectedObject.position+ transform.TransformDirection(view);
            newObj.transform.rotation = SelectedObject.root.rotation * m_EMScript.selectedObjectRotate;
            FixedJoint newJoint = newObj.AddComponent<FixedJoint>();
            newJoint.connectedBody = SelectedObject.GetComponent<Rigidbody>();
        }
        else
        {
            //print(SelectedObject);
            if (!SelectedObject.GetComponent<ObjectManager>().isRoot)
                Destroy(SelectedObject.gameObject);
        }
    }
    void SaveObject(string name)
    {
        Quaternion temp = transform.rotation;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        ObjectManager[] objectList;
        objectList = GetComponentsInChildren<ObjectManager>();
        ObjectListData[] objListData = new ObjectListData[objectList.Length];
        for(int i = 0; i < objectList.Length; i++)
        {
            Joint joint = objectList[i].GetComponent<Joint>();
            if (joint == null)
                objListData[i] = new ObjectListData(objectList[i], 0, objectList[i].name);
            else
                objListData[i] = new ObjectListData(objectList[i],0, objectList[i].name,1, joint.connectedBody.name);
            //print(objListData[i]+i.ToString());
        }
        StageObjectData data = new StageObjectData(StageInt, name, objListData);
        print(DataManager.SaveToFile(data, name));
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
        StageObjectData loaded = DataManager.LoadToFile<StageObjectData>(name);
        StageInt = loaded.number;
        //StageName = loaded.turreinName;
        
        print(loaded.objectSize);
        for(int i = 0; i < loaded.objectSize; i++)
        {
            loaded.objectList[i].DataToObject(prefabs[loaded.objectList[i].type],gameObject.transform);
        }
        print("done!");


    }
    void jointTest()
    {
        Joint[] joint = transform.GetComponentsInChildren<Joint>();
        foreach (var joint1 in joint)
        {
            try
            {

                print(joint1.connectedBody.name);
            }
            catch (Exception e)
            {
                print(e);
            }
        }
    }
    public void ObejctChange(int index)
    {
        selectedObject = prefabs[index];
        selectedObjectprev = prefabsPrev[index];

    }
    void camMoveFront()
    {
        if(cam.position.z - transform.position.z > 1)
            cam.position -= Vector3.forward * 0.1f ;
    }
    void camMoveBack()
    {
        if (cam.position.z - transform.position.z < 20)
            cam.position -= Vector3.back * 0.1f;
    }
    void StartStage()
    {
        Quaternion temp = transform.rotation;
        transform.rotation = Quaternion.Euler(Vector3.zero); 
        Rigidbody[] rigids = GetComponentsInChildren<Rigidbody>();
        SaveObject(StageName + "temp");
        foreach (var rigid in rigids)
        {
            rigid.isKinematic = false;
            rigid.useGravity = true;
        }
        
        isEdit = false;
    }
    void StartEdit()
    {
        /*
        Quaternion temp = transform.rotation;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        Rigidbody[] rigids = GetComponentsInChildren<Rigidbody>();
        foreach (var rigid in rigids)
        {
            rigid.isKinematic = true;
            rigid.useGravity = false;
        }
        */
        LoadObject(StageName + "temp");
        isEdit = true;
    }
}
