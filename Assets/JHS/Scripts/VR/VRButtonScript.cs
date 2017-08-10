using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButtonScript : MonoBehaviour {

    [SerializeField]
    private VRInteractiveItem m_VRInteractiveItem;

    [SerializeField]
    private Material mOriginal;
    [SerializeField]
    private Material mMagenta;


    private void OnEnable()
    {
        m_VRInteractiveItem.OnEnter += HandleEnter;
        m_VRInteractiveItem.OnExit += HandleExit;
    }

    private void HandleEnter()
    {
        gameObject.GetComponent<MeshRenderer>().material = mMagenta;
    }

    private void HandleExit()
    {
        gameObject.GetComponent<MeshRenderer>().material = mOriginal;
    }
}
