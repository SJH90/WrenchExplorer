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
    EditManagerScript m_EMScript;

    private void Awake()
    {
        selectedObject = Obj1;
        selectedObjectprev = Obj1pre;
        selectedObjectRotate = Quaternion.Euler(new Vector3(0, 0, 0));
        m_EMScript = GetComponent<EditManagerScript>();
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
    public void AddObject(Transform SelectedObject)
    {
        if (m_EMScript.selectedObject != null)
        {
            GameObject newObj = Instantiate(m_EMScript.selectedObject, SelectedObject.root);
            newObj.transform.position = SelectedObject.position;
            newObj.transform.rotation = SelectedObject.root.rotation * m_EMScript.selectedObjectRotate;
        }
        else
        {
            if (!SelectedObject.parent.GetComponent<ObjectScript>().isRoot)
                Destroy(SelectedObject.parent.gameObject);
        }
    }
}
