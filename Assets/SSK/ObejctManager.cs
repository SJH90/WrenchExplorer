using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObejctManager : MonoBehaviour {
    public bool isEssential;
    public bool isRoot;
    public VRInteractiveItem m_VRInteractiveItem;
    public EditStageManager m_EMScript;
    public GameObject previewObject;
    Camera m_Camera;

    private void Start()
    {
        m_Camera = Camera.main;
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
            previewObject = Instantiate(m_EMScript.selectedObjectprev);
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
        Vector3 view=transform.InverseTransformPoint(hit.point);
        print(view);
        view.x = Mathf.Round(view.x);
        view.y = Mathf.Round(view.y);
        view.z = Mathf.Round(view.z);
        print(view);
        if (previewObject != null)
        {
            previewObject.transform.position = transform.position+ view;
            previewObject.transform.rotation = transform.root.rotation * m_EMScript.selectedObjectRotate;
        }
    }

    public void HandleExit()
    {
        if (previewObject != null)
            Destroy(previewObject);
    }

    public void HandleClick(RaycastHit hit)
    {
        Destroy(previewObject);
        m_EMScript.AddObject(transform);
    }
}
