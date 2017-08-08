using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditStageManager : MonoBehaviour {

    // Use this for initialization

    [SerializeField]
    private GameObject Obj1;
    [SerializeField]
    private GameObject Obj1pre;
    [SerializeField]
    private GameObject Obj2;
    [SerializeField]
    private GameObject Obj2pre;

    public GameObject selectedObject;
    public GameObject selectedObjectprev;
    public Quaternion selectedObjectRotate;
    EditStageManager m_EMScript;
    string StageName;
    int StageInt;
    public GameObject[] prefabs;
    private void Awake()
    {
        selectedObject = Obj1;
        selectedObjectprev = Obj1pre;
        selectedObjectRotate = Quaternion.Euler(new Vector3(0, 0, 0));
        m_EMScript = GetComponent<EditStageManager>();
        StageName = "Test";
        StageInt = -1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedObject = Obj1;
            selectedObjectprev = Obj1pre;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedObject = Obj2;
            selectedObjectprev = Obj2pre;
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
            SaveObject();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadObject("Test");
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
            newObj.transform.position = SelectedObject.position+ transform.TransformDirection(view);
            newObj.transform.rotation = SelectedObject.root.rotation * m_EMScript.selectedObjectRotate;
        }
        else
        {
            print(SelectedObject);
            if (!SelectedObject.GetComponent<ObjectManager>().isRoot)
                Destroy(SelectedObject.gameObject);
        }
    }
    void SaveObject()
    {
        Quaternion temp = transform.rotation;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        ObjectManager[] objectList;
        objectList = GetComponentsInChildren<ObjectManager>();
        ObjectListData[] objListData = new ObjectListData[objectList.Length];
        for(int i = 0; i < objectList.Length; i++)
        {
            objListData[i] = new ObjectListData(objectList[i],0);
        }
        StageObjectData data = new StageObjectData(StageInt,StageName, objListData);
        DataManager.SaveToFile(data, StageName);
        transform.rotation = temp;

    }
    void LoadObject(string name)
    {

        /*
        Transform[] children=GetComponentsInChildren<Transform>();
        foreach(var child in children) {
            if (child.IsChildOf(transform))
                Destroy(child);
        }
        */
        transform.rotation = Quaternion.Euler(Vector3.zero);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        StageObjectData loaded = DataManager.LoadToFile<StageObjectData>(name);
        StageInt = loaded.number;
        StageName = loaded.turreinName;
        print(loaded.objectSize);
        for(int i = 0; i < loaded.objectSize; i++)
        {
            print(loaded.objectList[i].DataToObject(prefabs[loaded.objectList[i].type],gameObject.transform));

        }


    }
}
