using UnityEngine;
using System.Collections;
using System;

public class VRInteractiveItem : MonoBehaviour {
    
    public event Action OnEnter;
    public event Action<RaycastHit> OnOver;
    public event Action OnExit;

    public event Action<RaycastHit> OnClick;
    public event Action OnCancel;
    
    public void Enter()
    {
        if (OnEnter != null)
            OnEnter();
    }
    public void Over(RaycastHit hit)
    {
        if (OnOver != null)
            OnOver.DynamicInvoke(hit);
    }
    public void Exit()
    {
        if (OnExit != null)
            OnExit();
    }
    public void Click(RaycastHit hit)
    {
        if (OnClick != null)
            OnClick(hit);
    }
    public void Cancel()
    {
        if (OnCancel != null)
            OnCancel();
    }

    private void OnDestroy()
    {
        OnEnter = null;
        OnOver = null;
        OnExit = null;

        OnClick = null;
        OnCancel = null;
    }
}
