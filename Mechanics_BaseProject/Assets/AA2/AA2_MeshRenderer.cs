using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
public class AA2_MeshRenderer : MonoBehaviour
{
    MeshFilter mf;
    public Mesh mesh;
    public AA2_Cloth cloth;
    private void Start()
    {
        mf = GetComponent<MeshFilter>();
        mesh = Create();
        mf.sharedMesh = mesh;
    }
    private void Update()
    {
        cloth.Update(Time.deltaTime);
        Vector3[] vertices = new Vector3[cloth.points.Length];
        for (int i = 0; i < cloth.points.Length; i++)
        {
            vertices[i] = cloth.points[i].actualPosition.ToUnity();
        }
        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();
        //mesh.MarkModified();
        //mesh.UploadMeshData(true);
    }

    public Mesh Create()
    {
        Mesh newmesh = new Mesh();
        Vector3[] vertices = new Vector3[(cloth.settings.xPartSize + 1) * (cloth.settings.yPartSize + 1)];
        newmesh.name = "Procedural Grid";
        cloth.points = new AA2_Cloth.Vertex[(cloth.settings.xPartSize + 1) * (cloth.settings.yPartSize + 1)];
        for (int i = 0, y = 0; y <= cloth.settings.yPartSize; y++)
        {
            for (int x = 0; x <= cloth.settings.xPartSize; x++, i++)
            {
                vertices[i] = new Vector3(x * cloth.settings.width / cloth.settings.xPartSize - cloth.settings.width * 0.5f, 0, y * cloth.settings.height / cloth.settings.yPartSize - cloth.settings.height * 0.5f);
                cloth.points[i] = new AA2_Cloth.Vertex(vertices[i].ToCustom());
            }
        }
        newmesh.vertices = vertices;

        int[] triangles = new int[cloth.settings.xPartSize * cloth.settings.yPartSize * 6];
        for (int ti = 0, vi = 0, y = 0; y < cloth.settings.yPartSize; y++, vi++)
        {
            for (int x = 0; x < cloth.settings.xPartSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + cloth.settings.xPartSize + 1;
                triangles[ti + 5] = vi + cloth.settings.xPartSize + 2;
            }
        }
        newmesh.triangles = triangles;
        newmesh.RecalculateNormals();
        newmesh.MarkDynamic();
        return newmesh;
    }

    private void OnDrawGizmosSelected()
    {
        cloth.Debug();
    }
}
