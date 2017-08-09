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
        if (m_EMScript != null)
        {
            m_EMScript.m_CurrentObject = this;
            if (m_EMScript.selectedObjectprev != null && m_EMScript.IsEdit && gameObject.tag != "Wheel")
            {
                previewObject = Instantiate(m_EMScript.selectedObjectprev, transform.parent);
                previewObject.transform.rotation = transform.root.rotation * m_EMScript.selectedObjectRotate;
            }
        }

    }

    public void HandleOver(RaycastHit hit)
    {
        //print(gameObject + "Click!");
        if (m_EMScript.IsEdit && gameObject.tag != "Wheel")
        {
            Vector3 view = getAddPosition(hit);
            if (previewObject != null)
            {
                previewObject.transform.position = transform.position + transform.TransformDirection(view);
                previewObject.transform.rotation = transform.root.rotation * m_EMScript.selectedObjectRotate;
            }
        }
    }
    public void Refresh()
    {
        Destroy(previewObject);
        previewObject = Instantiate(m_EMScript.selectedObjectprev, transform.parent);
    }
    public void HandleExit()
    {
        if (previewObject != null)
            Destroy(previewObject);
        if(m_EMScript != null)
            m_EMScript.m_CurrentObject = null;
    }

    public void HandleClick(RaycastHit hit)
    {
        //print(gameObject + "Click!");
        if (m_EMScript.IsEdit)
        {
            Vector3 view = getAddPosition(hit);
            Destroy(previewObject);
            m_EMScript.AddObject(transform, transform.position + transform.TransformDirection(view));
           
        }
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
