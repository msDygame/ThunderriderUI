using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DrawPolygon : MonoBehaviour
{

    public float scale = 0;
    public int polygon = 0;
    public float[] value;
    public float maxValue = 0;

    private List<Vector3> v3List = new List<Vector3>();
    private Vector3[] v3Array;

    void Start()
    {

        v3List.Add(new Vector3(0, 0, 0));
        for (int i = 0; i < polygon; i++)
        {
            float angle = i * Mathf.PI * 2 / polygon;
            Vector3 pos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
            v3List.Add(pos);
        }
        v3Array = v3List.ToArray();
//      Update2();
    }

    void Update()
    {

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v3Temp = new Vector3[polygon + 1];
        v3Temp[0] = new Vector3(0, 0, 0);

        mesh.Clear();
        for (int i = 0; i < polygon; i++)
        {
            v3Temp[i + 1] = v3Array[i + 1] * Mathf.Clamp01(value[i] / maxValue) * scale;
        }
        mesh.vertices = v3Temp;
        mesh.triangles = ComparePolygonTriangles(polygon);
    }

    private int[] ComparePolygonTriangles(int num)
    {

        List<int> triangleList = new List<int>();
        int[] triangleElements;
        for (int i = 0; i < num - 1; i++)
        {
            triangleList.Add(0);
            for (int j = 0; j < 2; j++)
            {
                triangleList.Add(i + j + 1);
            }
        }
        triangleList.Add(0);
        triangleList.Add(num);
        triangleList.Add(1);
        triangleElements = triangleList.ToArray();
        return triangleElements;
    }
}
