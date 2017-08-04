using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlaneScript : MonoBehaviour {

    public VRInteractiveItem m_VRInteractiveItem;
    public EditManagerScript m_EMScript;
    public GameObject previewObject;
    EditManagerScript m_EditManager;

    private void OnEnable()
    {
        m_VRInteractiveItem = transform.GetComponent<VRInteractiveItem>();
        m_EMScript = transform.root.GetComponent<EditManagerScript>();
        m_EditManager = GetComponentInParent<EditManagerScript>();
        m_VRInteractiveItem.OnEnter += HandleEnter;
        m_VRInteractiveItem.OnOver += HandleOver;
        m_VRInteractiveItem.OnExit += HandleExit;
        m_VRInteractiveItem.OnClick += HandleClick;
    }
    

    public void HandleEnter()
    {
        if(m_EMScript.selectedObjectprev != null)
            previewObject = Instantiate(m_EMScript.selectedObjectprev);
    }

    public void HandleOver()
    {
        if(previewObject != null)
        {
            previewObject.transform.position = transform.position;
            previewObject.transform.rotation = transform.root.rotation * m_EMScript.selectedObjectRotate;
        }
    }

    public void HandleExit()
    {
        if(previewObject != null)
            Destroy(previewObject);
    }

    public void HandleClick()
    {
        Destroy(previewObject);
        m_EditManager.AddObject(transform);
    }

}
