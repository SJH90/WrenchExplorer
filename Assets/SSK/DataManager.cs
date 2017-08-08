using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2017-07-28 SSK
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

//2017-07-28 SSK
//ObjectData Class

[Serializable]
public struct ObjectListData
{
    public SerializableVector3 position;
    public SerializableVector3 rotation;
    public bool essential;
    public int type;
    public bool root;
    public ObjectListData(ObjectManager obj,int Type)
    {
        this.position = obj.Position;
        this.rotation = obj.Rotation;
        this.essential = obj.isEssential;
        this.type = Type;
        this.root = obj.isRoot;
    }
    public GameObject DataToObject(GameObject prefabs,Transform parent)
    {
        GameObject newObject = MonoBehaviour.Instantiate(prefabs,parent);
        newObject.transform.position = this.position;
        newObject.transform.rotation = Quaternion.Euler(this.rotation);
        
        ObjectManager newManager = newObject.GetComponent<ObjectManager>();

        /*
        Joint joint;
        switch (this.jointType)
        {
            case 1:
                joint = newObject.AddComponent<FixedJoint>();
                joint.connectedBody = parent.Find(connectedBody).GetComponent<Rigidbody>();
                
                break;
            case 2:
                joint = newObject.AddComponent<HingeJoint>();
                joint.connectedBody = parent.Find(connectedBody).GetComponent<Rigidbody>();
                break;
        }
        */

        newManager.isRoot = this.root;
        newManager.isEssential = this.essential;
        return newObject;
    }

}

//2017-07-28 SSK
//StageData Class
[Serializable]
public struct StageObjectData
{
    //스테이지넘버
    public int number;
    //터레인 데이터 이름
    public string turreinName;
    public int objectSize;
    //오브젝트 리스트
    public ObjectListData[] objectList;

    public StageObjectData(int number,string turreinName, ObjectListData[] objectList)
    {
        this.number = number;
        this.turreinName = turreinName;
        this.objectList = objectList;
        this.objectSize = objectList.Length;
    }
}
//2017-08-01 SSK
//UserMadeOjectData Class
[Serializable]
public class UserMadeOjectData
{
    //public int objectSize;
    public ObjectListData[] objectList;
    public KeyValuePair<int, int>[] jointList;
}


//2017-07-28 SSK
//SerializableVector3 Class
//From http://answers.unity3d.com/questions/956047/serialize-quaternion-or-vector3.html
[System.Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}]", x, y, z);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}
/// <summary>
/// Since unity doesn't flag the Quaternion as serializable, we
/// need to create our own version. This one will automatically convert
/// between Quaternion and SerializableQuaternion
/// </summary>
[System.Serializable]
public struct SerializableQuaternion
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// w component
    /// </summary>
    public float w;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    /// <param name="rW"></param>
    public SerializableQuaternion(float rX, float rY, float rZ, float rW)
    {
        x = rX;
        y = rY;
        z = rZ;
        w = rW;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
    }

    /// <summary>
    /// Automatic conversion from SerializableQuaternion to Quaternion
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Quaternion(SerializableQuaternion rValue)
    {
        return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
    }

    /// <summary>
    /// Automatic conversion from Quaternion to SerializableQuaternion
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableQuaternion(Quaternion rValue)
    {
        return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
    }
}

//2017-07-28 SSK
//dataManager Calss
public static class DataManager  {
    //SaveToFile
    //data오브젝트를 FileName파일에 쓴다
    public static string SaveToFile(object data,string fileName)
    {
        
        StreamWriter fileStream = new StreamWriter(fileName);
        string json = JsonUtility.ToJson(data);
        fileStream.Write (json);
        fileStream.Close();
        return json;
    }
    //LoadToFile
    //FileName파일데이터를 Templete형식의 데이터로 읽어온다.
    public static Templete LoadToFile<Templete>(string fileName)
    {

        StreamReader fileStream = new StreamReader(fileName);
        string json = fileStream.ReadLine();
        Templete loadedData = JsonUtility.FromJson<Templete>(json);
        fileStream.Close();
        return loadedData;
    }

    
}