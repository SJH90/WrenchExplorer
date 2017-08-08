using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorUIScript : MonoBehaviour {

    public GameObject stage;
    public EditStageManager esmScript;

    private void OnEnable()
    {
        esmScript = stage.GetComponent<EditStageManager>();
    }


}
