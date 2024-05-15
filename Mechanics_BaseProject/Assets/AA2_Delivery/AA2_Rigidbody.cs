using static AA1_ParticleSystem;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AA2_Rigidbody
{
    [System.Serializable]
    public struct Settings
    {
        public uint nCubos;
        public Vector3C gravity;
        public float bounce;
    }
    public Settings settings;

    [System.Serializable]
    public struct SettingsCollision
    {
        public PlaneC[] planes;
    }
    public SettingsCollision settingsCollision;


    [System.Serializable]
    public struct CubeRigidbody
    {
        public Vector3C position;
        public Vector3C lastPosition;
        public Vector3C lastPosition2;
        public Vector3C size;
        public Vector3C euler;
        public float density;
        public CubeRigidbody(Vector3C _position, Vector3C _lastPosition, Vector3C _lastPosition2, Vector3C _size, Vector3C _euler, float _density)
        {
            position = _position;
            lastPosition = _lastPosition;
            lastPosition2 = _lastPosition2;
            size = _size;
            euler = _euler;
            density = _density;
        }
    }
    public CubeRigidbody crb = new CubeRigidbody(Vector3C.zero, Vector3C.zero, Vector3C.zero, new(.1f,.1f,.1f), Vector3C.zero, 100);
    
    public void Update(float dt)
    {
        crb.lastPosition2 = crb.lastPosition;
        crb.lastPosition = crb.position;
        float volume = crb.size.x * crb.size.y * crb.size.z;
        Vector3C force;
        force = settings.gravity * crb.density * volume;
        crb.position += force * dt;
        //VERLET:
        //crb.position = crb.lastPosition*2 - crb.lastPosition2 + settings.gravity * Mathf.Pow(dt, 2);
        
            //cubeRList[i].AddForce(settings.gravity * dt);
            //cubeRList[i].lastPosition = particles[i].position;
            //cubeRList[i].position += cubeRList[i].acceleration * dt;
            

            //for (int j = 0; j < settingsCollision.planes.Length; ++j)
            //{
            //    Vector3C distanceVector = particles[i].position - settingsCollision.planes[j].NearestPoint(particles[i].position);
            //    float distance = distanceVector.magnitude;
            //    float factor = particles[i].size;
            //    bool collision = false;
            //    bool passed = false;
            //    if (distance <= factor * 2)
            //        passed = true;
            //    if (distance <= particles[i].size + factor)
            //        collision = true;
            //    if (collision)
            //    {
            //        int counter = 2;
            //        while (passed)
            //        {
            //            particles[i].position = particles[i].lastPosition;
            //            particles[i].position += particles[i].acceleration * dt / counter;
            //            distanceVector = particles[i].position - settingsCollision.planes[j].NearestPoint(particles[i].position);
            //            distance = distanceVector.magnitude;

            //            if (distance > 0)
            //                passed = false;
            //            else
            //                counter *= 2;
            //        }
            //        //Calcular componente normal
            //        float vnMagnitude = Vector3C.Dot(particles[i].acceleration, settingsCollision.planes[j].normal);
            //        Vector3C vn = settingsCollision.planes[j].normal * vnMagnitude;
            //        //Calcular componente tangencial
            //        Vector3C vt = particles[i].acceleration - vn;
            //        //Calcular nueva velocidad
            //        Vector3C newVelocity = -vn + vt;

            //        particles[i].AddForce(-(particles[i].acceleration));
            //        particles[i].AddForce(newVelocity * settings.bounce);


            //        collision = false;

            //    }
            //}
    }

    public void Debug()
    {
        foreach (var item in settingsCollision.planes)
        {
            item.Print(Vector3C.red);
        }
    }
}
