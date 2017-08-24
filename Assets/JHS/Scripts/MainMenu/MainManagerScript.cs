using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManagerScript : MonoBehaviour {

    public enum Menu
    {
        MAIN,
        GAME,
        SCENARIO,
        CHALLENGE,
        OPTION
    }
    public Transform t_MainMenu;
    public Transform t_GameMenu;
    public Transform t_ScenarioMenu;
    public Transform t_ChallengeMenu;
    public Transform t_Option;
    public Menu currMenu;
    public Menu befMenu;

    public Transform Head;
    public Vector3 targetPosition;

    public VRInput m_VRInput;

    private void OnEnable()
    {
        befMenu = Menu.MAIN;
        currMenu = Menu.MAIN;

        m_VRInput = Head.GetComponentInChildren<VRInput>();

        m_VRInput.OnCancel += HandleCancel;
    }


    private void Update()
    {
        if(currMenu != befMenu)
        {
            switch (currMenu)
            {
                case Menu.MAIN:
                    targetPosition = t_MainMenu.position;
                    break;
                case Menu.GAME:
                    targetPosition = t_GameMenu.position;
                    break;
                case Menu.SCENARIO:
                    targetPosition = t_ScenarioMenu.position;
                    break;
                case Menu.CHALLENGE:
                    targetPosition = t_ChallengeMenu.position;
                    break;
            }
            befMenu = currMenu;
        }

        if (Vector3.Distance(Head.transform.position, targetPosition) > 0.1f)
        {
            Head.transform.position = Vector3.Lerp(Head.transform.position, targetPosition, 0.2f);
        }
    }

    public void HandleCancel()
    {
        print(currMenu);
        switch (currMenu)
        {
            case Menu.MAIN:
                
                break;
            case Menu.GAME:
                currMenu = Menu.MAIN;
                break;
            case Menu.SCENARIO:
                currMenu = Menu.GAME;
                break;
            case Menu.CHALLENGE:
                currMenu = Menu.GAME;
                break;
        }
        print(currMenu);
    }
}
