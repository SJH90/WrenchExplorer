using UnityEngine;
using System.Collections;

public class FlipInsideOut : MonoBehaviour
{
    public int[] Tris = new int[100];
    int temp;
    
    void Start()
    {
        Tris = this.GetComponent<MeshFilter>().mesh.triangles;
        for (int i = 0; i < 100; i++)
        {
            if (Tris[3 * i] != null)
            {
                temp = Tris[3 * i + 1];
                Tris[3 * i + 1] = Tris[3 * i + 2];
                Tris[3 * i + 2] = temp;
            }
        }
        this.GetComponent<MeshFilter>().mesh.triangles = Tris;
    }
}