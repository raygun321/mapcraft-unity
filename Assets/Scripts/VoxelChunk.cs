using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelChunk : MonoBehaviour {

    public CameraControl camControl;

    [Range(1, 200)]
    public int resolution = 16;

    [Range(1, 200)]
    public int size = 16;

    private Mesh mesh;
    private int currentResolution;
    private bool[] voxels;

    private void OnEnable()
    {
        if(camControl != null)
        {
            camControl.AddVisibleTarget(transform);
        }

        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "Chunk Mesh";
            GetComponent<MeshFilter>().mesh = mesh;
        }
        Refresh();
    }

    public void Refresh()
    {
        if (resolution != currentResolution)
        {
            CreateGrid();
        }
    }

    private void CreateGrid()
    {
        currentResolution = resolution;
        mesh.Clear();

        int ySkip = resolution + 1;
        int zSkip = ySkip * ySkip;
        int yzSkip = ySkip + zSkip;

        voxels = new bool[resolution * resolution * resolution];

        Vector3[] vertices = new Vector3[voxels.Length * 24];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[resolution * resolution * resolution * 12 * 3];

        float stepSize = (float)size / (float)resolution;

        for(int v = 0, z = 0; z < resolution; z++)
        {
            for(int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++, v += 24)
                {
                    vertices[v]     = new Vector3(x * stepSize, y * stepSize, z * stepSize);
                    vertices[v + 1] = new Vector3(x * stepSize, y * stepSize, z * stepSize);
                    vertices[v + 2] = new Vector3(x * stepSize, y * stepSize, z * stepSize);
                    vertices[v + 3] = new Vector3(x * stepSize, y * stepSize, z * stepSize);
                }
            }
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;

        for (int t = 0, v = 0, z = 0; z < resolution; z++, v += resolution + 1)
        {
            for (int y = 0; y < resolution; y++, v++)
            {
                for (int x = 0; x < resolution; x++, v++, t += 36)
                {

                    AddQuad(triangles, t, v, v + 1, v + ySkip + 1, v + ySkip);
                    AddQuad(triangles, t + 6, v + zSkip, v + yzSkip, v + yzSkip + 1, v + zSkip + 1);
                    AddQuad(triangles, t + 12, v, v + zSkip, v + zSkip + 1, v + 1);
                    AddQuad(triangles, t + 18, v + ySkip + 1, v + yzSkip + 1, v + yzSkip, v + ySkip);
                    AddQuad(triangles, t + 24, v + ySkip, v + yzSkip, v + zSkip, v);
                    AddQuad(triangles, t + 30, v + 1, v + zSkip + 1, v + yzSkip + 1, v + ySkip + 1);
                }
            }
        }
        mesh.triangles = triangles;
    }

    // b c
    // a d
    private void AddQuad(int[] triangles, int t, int a, int b, int c, int d) {
        Debug.Log("AddQuad t:" + t + " a:" + a + " b:" + b + " c:" + c + " d:" + d);

        triangles[t] = a;
        triangles[t+1] = d;
        triangles[t+2] = b;
        triangles[t+3] = b;
        triangles[t+4] = d;
        triangles[t+5] = c;
    }
}
