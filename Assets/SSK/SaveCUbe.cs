using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCUbe : MonoBehaviour {
    public bool isSave;
    StageObjectData saveDatas;
    StageObjectData loadDatas;
    // Use this for initialization
    void Start () {
        saveDatas.objectList = new ObjectListData[10];
        for(int i = 0; i < 10; i++)
        {
            saveDatas.objectList[i].position = Vector3.up * i;
            saveDatas.objectList[i].rotation = Vector3.down * i;
            saveDatas.objectList[i].essential = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (isSave)
            //print(DataManager.objectListSave(saveDatas,"test"));
            DataManager.SaveToFile(saveDatas, "test");
        else
        {
            loadDatas = DataManager.LoadToFile<StageObjectData>("test");
            for (int i = 0; i < 10; i++)
            {
                print(i + " | " + loadDatas.objectList[i].position + " | " + loadDatas.objectList[i].rotation + " | " + (loadDatas.objectList[i].essential));

            }
        }
    }
}
