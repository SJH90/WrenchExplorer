using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  EditStageManager : MonoBehaviour {

    public GameObject selectedObject;
    public GameObject selectedObjectprev;
    public Quaternion selectedObjectRotate;
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
    }
    void StageKeyInput()
    {
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
    public void AddObject(Transform SelectedObject, Vector3 position)
    {
        if (selectedObject != null)
        {
            GameObject newObj = Instantiate(selectedObject, SelectedObject.root);
            ObjectSize++;
            newObj.name = selectedObject.name + ObjectSize;

            newObj.transform.position = position;
            newObj.transform.rotation = SelectedObject.root.rotation * selectedObjectRotate;
        }
        else
        {
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
            objListData[i] = new ObjectListData(objectList[i], 0);
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
        StageObjectData loaded = DataManager.LoadToFile<StageObjectData>(name);
        StageInt = loaded.number;
        for(int i = 0; i < loaded.objectSize; i++)
        {
            loaded.objectList[i].DataToObject(prefabs[loaded.objectList[i].type],gameObject.transform);
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
        connectJoint();
        foreach (var rigid in rigids)
        {
            rigid.isKinematic = false;
            rigid.useGravity = true;
        }
        
        isEdit = false;
    }
    void StartEdit()
    {
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
                FixedJoint fj = currentObject.gameObject.AddComponent<FixedJoint>();
                fj.connectedBody=hit.rigidbody;
            }
        }
    }
}
