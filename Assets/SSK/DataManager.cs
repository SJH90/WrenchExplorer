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
    //오브젝트 리스트
    ObjectListData[] objectList;

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


//2017-07-28 SSK
//dataManager Calss
public static class DataManager  {
    public static void objectListSave(ObjectListData[] data,string fileName)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(fileName, FileMode.Create);
        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static ObjectListData[] obejctListLoad(string fileName)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(fileName, FileMode.Open);
        ObjectListData[] loadedData;
        loadedData=(ObjectListData[])binaryFormatter.Deserialize(fileStream);
        fileStream.Close();
        return loadedData;
    }
}
