using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class AA3_MeshRenderer : MonoBehaviour
{
    MeshFilter mf;
    [Min(1)]
    public float width;
    [Min(1)]
    public float height;
    [Min(2)]
    public int xSize;
    [Min(2)]
    public int ySize;
    public Mesh mesh;
    public AA3_Waves waves;
    private void Start()
    {
        mf = GetComponent<MeshFilter>();
        mesh = Create();
        mf.sharedMesh = mesh;
    }
    private void Update()
    {
        waves.Update(Time.deltaTime);
        Vector3[] vertices = new Vector3[waves.points.Length];
        for (int i = 0; i < waves.points.Length; i++)
        {
            vertices[i] = waves.points[i].position.ToUnity();
        }
        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();
        //mesh.MarkModified();
        //mesh.UploadMeshData(true);
    }

    public Mesh Create()
    {
        Mesh newmesh = new Mesh();
        Vector3[] vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        newmesh.name = "Procedural Grid";
        waves.points = new AA3_Waves.Vertex[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x * width / xSize - width * 0.5f, 0, y * height / ySize - height * 0.5f);
                waves.points[i] = new AA3_Waves.Vertex(vertices[i].ToCustom());
            }
        }
        newmesh.vertices = vertices;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        newmesh.triangles = triangles;
        newmesh.RecalculateNormals();
        newmesh.MarkDynamic();
        return newmesh;
    }

    private void OnDrawGizmosSelected()
    {
        waves.Debug();
    }
}
