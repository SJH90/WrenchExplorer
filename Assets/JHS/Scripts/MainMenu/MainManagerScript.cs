using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManagerScript : MonoBehaviour {

    public Transform t_MainMenu;
    public Transform t_GameMenu;
    public Transform t_ScenarioMenu;
    public Transform t_ChallengeMenu;
    public Transform t_Option;

    public Transform Head;
    public Vector3 targetPosition;

    private void Update()
    {
        if (Vector3.Distance(Head.transform.position, targetPosition) > 0.1f)
        {
            Head.transform.position = Vector3.Lerp(Head.transform.position, targetPosition, 0.2f);
        }
    }
}
