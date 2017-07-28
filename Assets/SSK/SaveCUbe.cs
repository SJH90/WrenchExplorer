using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCUbe : MonoBehaviour {
    public bool isSave;
    ObjectListData[] saveDatas;
    ObjectListData[] loadDatas;

    // Use this for initialization
    void Start () {
        saveDatas = new ObjectListData[10];
        for(int i = 0; i < 10; i++)
        {
            saveDatas[i].position = Vector3.up * i;
            saveDatas[i].rotation = Vector3.down * i;
            saveDatas[i].essential = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (isSave)
            DataManager.objectListSave(saveDatas,"test");
        else
        {
            loadDatas = DataManager.obejctListLoad("test");
            for (int i = 0; i < 10; i++)
            {
                print(i + " | " + loadDatas[i].position +" | " + loadDatas[i].rotation + " | " + (loadDatas[i].essential));

            }
        }
    }
}
