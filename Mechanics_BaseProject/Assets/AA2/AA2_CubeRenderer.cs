using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AA2_CubeRenderer : MonoBehaviour
{
    public Material cubeMaterial;
    public Mesh cubeMesh;

    public AA2_Rigidbody[] rigidbodies;
    void Update()
    {
        foreach (AA2_Rigidbody rb in rigidbodies)
        {
            rb.Update(Time.deltaTime);
        }

        RenderParams rp = new RenderParams(cubeMaterial);
        rp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        rp.receiveShadows = true;
        Matrix4x4[] instData = new Matrix4x4[rigidbodies.Length];
        for (int i = 0; i < rigidbodies.Length; ++i)
        {
            instData[i] = Matrix4x4.TRS(rigidbodies[i].crb.position.ToUnity(), Quaternion.Euler(rigidbodies[i].crb.euler.ToUnity()), rigidbodies[i].crb.size.ToUnity());
        }
        Graphics.RenderMeshInstanced(rp, cubeMesh, 0, instData);
    }
    private void OnDrawGizmosSelected()
    {
        foreach (AA2_Rigidbody rb in rigidbodies)
        {
            rb.Debug();
        }
    }
}
