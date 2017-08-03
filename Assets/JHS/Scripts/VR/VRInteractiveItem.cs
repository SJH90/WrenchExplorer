using UnityEngine;
using System.Collections;
using System;

public class VRInteractiveItem : MonoBehaviour {
    
    public event Action OnEnter;
    public event Action OnOver;
    public event Action OnExit;

    public event Action OnClick;
    public event Action OnCancel;
    
    public void Enter()
    {
        if (OnEnter != null)
            OnEnter();
    }
    public void Over()
    {
        if (OnOver != null)
            OnOver();
    }
    public void Exit()
    {
        if (OnExit != null)
            OnExit();
    }
    public void Click()
    {
        if (OnClick != null)
            OnClick();
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
