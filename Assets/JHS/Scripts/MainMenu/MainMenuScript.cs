using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

    public GameObject Head;
    public Transform t_MainMenu;
    public Transform t_GameMenu;
    public Transform t_ScenarioMenu;
    public Transform t_ChallengeMenu;
    

    public bool isup = true;

    private void OnEnable()
    {
        m_VRInteractivrItem.OnClick += HandleClick;
    }

    void HandleClick(RaycastHit hit)
    {

    }

    void moveTo(string name)
    {
        switch (name)
        {
            case "main":

                break;
            case "game":
                break;
            case "scenario":
                break;
            case "challenge":
                break;
        }
    }
}
