using UnityEngine;
using System.Collections;


public class VREyeRaycast : MonoBehaviour {

    

    [SerializeField]    private VRInput m_VRInput;
    [SerializeField]    private Transform m_Camera;
    [SerializeField]    private Transform m_Reticle;

    private VRInteractiveItem m_CurrentInteractiveItem;
    




    // Use this for initialization
    void Awake   ()
    {
        m_VRInput.OnSwipe += HandleSwipe;
        m_VRInput.OnClick += HandleClick;
        m_VRInput.OnCancel += HandleCancel;
    }
	
	// Update is called once per frame
	void Update () {
        EyeRaycast();
	}
    
    private void EyeRaycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(m_Camera.position, m_Camera.forward);

        VRInteractiveItem interactible;

        if (Physics.Raycast(ray, out hit, 500f))
        {
            m_Reticle.position = Vector3.Lerp(Camera.main.transform.position, hit.point, 0.9f);
            m_Reticle.localScale = new Vector3(0.01f, 0.01f, 0.01f) * Vector3.Distance(Camera.main.transform.position, m_Reticle.position);
            
            interactible = hit.collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object
        }
        else
        {
            interactible = null;
        }

        if (m_CurrentInteractiveItem != interactible)
        {

            if (m_CurrentInteractiveItem != null)
                m_CurrentInteractiveItem.Exit();

            m_CurrentInteractiveItem = interactible;

            if (m_CurrentInteractiveItem != null)
                m_CurrentInteractiveItem.Enter();
                
        }
        else
        {
            if (m_CurrentInteractiveItem != null)
                m_CurrentInteractiveItem.Over();
        }
    }

    void HandleClick()
    {

        if (m_CurrentInteractiveItem != null)
        {
            m_CurrentInteractiveItem.Click();
        }


    }
    void HandleCancel()
    {
        if (m_CurrentInteractiveItem != null)
            m_CurrentInteractiveItem.Cancel();
    }

    void HandleSwipe(VRInput.SwipeDirection swipe)
    {
        //GameManager.Instance.InputSwipe(swipe);
    }
    
}
