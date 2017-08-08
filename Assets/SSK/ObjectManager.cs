using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {
    public bool isEssential;
    public bool isRoot;
    public VRInteractiveItem m_VRInteractiveItem;
    public EditStageManager m_EMScript;
    public GameObject previewObject;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }
    public Vector3 Rotation
    {
        get
        {
            return transform.rotation.eulerAngles;
        }
    }
    private void OnEnable()
    {
        m_VRInteractiveItem = transform.GetComponent<VRInteractiveItem>();
        m_EMScript = transform.root.GetComponent<EditStageManager>();
        m_VRInteractiveItem.OnEnter += HandleEnter;
        m_VRInteractiveItem.OnOver += HandleOver;
        m_VRInteractiveItem.OnExit += HandleExit;
        m_VRInteractiveItem.OnClick += HandleClick;
    }


    public void HandleEnter()
    {
        
        if (m_EMScript.selectedObjectprev != null)
            previewObject = Instantiate(m_EMScript.selectedObjectprev,transform.parent);
    }

    public void HandleOver(RaycastHit hit)
    {
        /*
        float cosX = Vector3.Dot(transform.forward.normalized, (-m_Camera.transform.forward).normalized);
        float angleX= Mathf.Acos(cosX);
        float cosY = Vector3.Dot(transform.up.normalized, m_Camera.transform.up.normalized);
        float angleY = Mathf.Acos(cosY);
        angleX *=Mathf.Rad2Deg;
        angleY *= Mathf.Rad2Deg;
        
        print(angleX+","+angleY);
        */

        Vector3 view = getAddPosition(hit);
        if (previewObject != null)
        {
            //previewObject.transform.position = transform.position+ transform.TransformPoint(view/2);
            previewObject.transform.position = transform.position + transform.TransformDirection(view);
            //previewObject.transform.rotation = transform.root.rotation * m_EMScript.selectedObjectRotate;
        }
    }

    public void HandleExit()
    {
        if (previewObject != null)
            Destroy(previewObject);
    }

    public void HandleClick(RaycastHit hit)
    {
        Vector3 view = getAddPosition(hit);
        Destroy(previewObject);
        m_EMScript.AddObject(transform, view);
    }
    Vector3 getAddPosition(RaycastHit hit)
    {
        Vector3 view = transform.InverseTransformPoint(hit.point);
        view.x = Mathf.Floor((int)(view.x * 2.00001f));
        view.y = Mathf.Floor((int)(view.y * 2.00001f));
        view.z = Mathf.Floor((int)(view.z * 2.00001f));
        return view;
    }
}
