using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonScript : MonoBehaviour {

    public MainManagerScript MMS;
    private VRInteractiveItem m_VRInteractiveItem;

    private void OnEnable()
    {
        m_VRInteractiveItem = GetComponent<VRInteractiveItem>();

        m_VRInteractiveItem.OnClick += HandleClick;
    }


    private void HandleClick(RaycastHit hit)
    {
        switch (transform.name)
        {
            case "Start":
                MMS.currMenu = MainManagerScript.Menu.GAME;
                break;
            case "Edit":
                PlayerPrefs.SetString("StageName", "Test");
                SceneManager.LoadScene("gameScene");
                break;
            case "Option":

                break;

            case "Scenario":
                MMS.currMenu = MainManagerScript.Menu.SCENARIO;
                break;
            case "Challenge":
                MMS.currMenu = MainManagerScript.Menu.CHALLENGE;
                break;
        }
    }
}
