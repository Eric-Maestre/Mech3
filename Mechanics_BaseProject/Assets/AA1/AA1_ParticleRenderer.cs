using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AA1_ParticleRenderer : MonoBehaviour
{
    public Material particleMaterial;
    public Mesh particleMesh;

    public Transform cannon;
    public Transform cascadeA;
    public Transform cascadeB;

    public AA1_ParticleSystem system;
    void Update()
    {
        system.settingsCascade.PointA = cascadeA.ToCustomPosition();
        system.settingsCascade.PointB = cascadeB.ToCustomPosition();
        system.settingsCannon.Start = cannon.ToCustomPosition();
        system.settingsCannon.Direction = cannon.ToCustomForward();

        AA1_ParticleSystem.Particle[] particles = system.Update(Time.deltaTime);


        RenderParams rp = new RenderParams(particleMaterial);
        rp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        rp.receiveShadows = true;
        Matrix4x4[] instData = new Matrix4x4[particles.Length];
        for (int i = 0; i < particles.Length; ++i)
        {
            instData[i] = Matrix4x4.TRS(particles[i].position.ToUnity(), Quaternion.identity, Vector3.one * particles[i].size);
        }
        Graphics.RenderMeshInstanced(rp, particleMesh, 0, instData);
    }
    private void OnDrawGizmosSelected()
    {
        system.Debug();
    }
}
