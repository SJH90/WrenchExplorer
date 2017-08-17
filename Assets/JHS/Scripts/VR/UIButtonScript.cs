﻿using System.Collections;
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
                MMS.targetPosition = MMS.t_GameMenu.position;
                break;
            case "Edit":
                PlayerPrefs.SetString("StageName", "Test");
                SceneManager.LoadScene("gameScene");
                break;
            case "Option":

                break;

            case "Scenario":
                MMS.targetPosition = MMS.t_ScenarioMenu.position;
                break;
            case "Challenge":
                MMS.targetPosition = MMS.t_ChallengeMenu.position;
                break;
        }
    }
}
