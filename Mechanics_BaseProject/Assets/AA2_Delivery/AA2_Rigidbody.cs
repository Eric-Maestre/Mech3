using static AA1_ParticleSystem;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.UIElements;
using Microsoft.Win32.SafeHandles;
//obligatori utilitzar quaternions per a aquesta activitat
//permetré excepcionalment utilitzar la classe System.Numerics.Quaternion, NO la UnityEngine.Quaternion
//https://learn.microsoft.com/en-us/dotnet/api/system.numerics.quaternion?view=net-8.0
//Aquells que creïn la seva pròpia estructura de quaternion dins de la carpeta Common_Delivery rebran
//un punt extra en la nota final d'aquesta activitat
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
        public Vector3C[] vertex;
        public Vector3C lastPosition;
        public Vector3C lastPosition2;
        public Vector3C size;
        public Vector3C euler;
        public Vector3C aVelocity;
        public float density;
        public CubeRigidbody(Vector3C _position, Vector3C _lastPosition, Vector3C _lastPosition2, Vector3C _size, Vector3C _euler, Vector3C _aVeclocity, float _density)
        {
            position = _position;
            vertex = new Vector3C[8];
            vertex[0] = position - new Vector3C(position.x / 2, position.y / 2, position.z / 2);
            vertex[1] = position - new Vector3C(-position.x / 2, position.y / 2, position.z / 2);
            vertex[2] = position - new Vector3C(position.x / 2, -position.y / 2, position.z / 2);
            vertex[3] = position - new Vector3C(-position.x / 2, -position.y / 2, position.z / 2);
            vertex[4] = position - new Vector3C(position.x / 2, position.y / 2, -position.z / 2);
            vertex[5] = position - new Vector3C(-position.x / 2, position.y / 2, -position.z / 2);
            vertex[6] = position - new Vector3C(position.x / 2, -position.y / 2, -position.z / 2);
            vertex[7] = position - new Vector3C(-position.x / 2, -position.y / 2, -position.z / 2);
            lastPosition = _lastPosition;
            lastPosition2 = _lastPosition2;
            size = _size;
            euler = _euler;
            aVelocity = _aVeclocity;
            density = _density;
        }
    }
    public CubeRigidbody crb = new CubeRigidbody(Vector3C.zero, Vector3C.zero, Vector3C.zero, new(.1f, .1f, .1f), Vector3C.zero, Vector3C.zero, 100);

    public void GetOthersRigidbodysArray(AA2_Rigidbody[] allRigidbodies)
    {
        AA2_Rigidbody[] othersRigidbodys = new AA2_Rigidbody[allRigidbodies.Length - 1];
        int index = 0;
        for (int i = 0; i < allRigidbodies.Length; i++)
        {
            if (allRigidbodies[i] != this)
            {
                othersRigidbodys[index++] = allRigidbodies[i];
            }
        }
        // Aquest array conté els altres rigidbodys amb els quals podreu interactuar.
    }
    //L'array othersRigidbodys us retorna els altres cossos rígids en l'escena per poder implementar les col·lisions.

    Vector3C force;

    public void Update(float dt)
    {
        crb.lastPosition2 = crb.lastPosition;
        crb.lastPosition = crb.position;
        float volume = crb.size.x * crb.size.y * crb.size.z;
        force = settings.gravity * crb.density * volume;

        //VERLET:
        //crb.position = crb.lastPosition*2 - crb.lastPosition2 + settings.gravity * Mathf.Pow(dt, 2);


        for (int j = 0; j < settingsCollision.planes.Length; ++j)
        {
            Vector3C distanceVector = crb.position - settingsCollision.planes[j].NearestPoint(crb.position);
            float distance = distanceVector.magnitude;
            float factor = crb.size.x;
            bool collision = false;
            bool passed = false;
            if (distance <= factor * 2)
                passed = true;
            if (distance <= crb.size.x/* + factor*/)
                collision = true;
            if (collision)
            {
                UnityEngine.Debug.Log("Collision");
                int counter = 2;
                while (passed)
                {
                    crb.position = crb.lastPosition;
                    //crb.position += crb.acceleration * dt / counter;
                    crb.position += settings.gravity * dt / counter;
                    distanceVector = crb.position - settingsCollision.planes[j].NearestPoint(crb.position);
                    distance = distanceVector.magnitude;

                    if (distance > 0)
                        passed = false;
                    else
                        counter *= 2;
                }
                //Calcular componente normal
                Vector3C direction = (crb.position - crb.lastPosition).normalized;
                float vnMagnitude = Vector3C.Dot(direction, settingsCollision.planes[j].normal);
                //float vnMagnitude = Vector3C.Dot(particles[i].acceleration, settingsCollision.planes[j].normal);
                Vector3C vn = settingsCollision.planes[j].normal * vnMagnitude;
                //Calcular componente tangencial
                Vector3C vt = direction - vn;
                //Vector3C vt = particles[i].acceleration - vn;
                //Calcular nueva velocidad
                Vector3C newVelocity = -vn + vt;

                force = new Vector3C(0, 0, 0);
                force = new Vector3C(0, 10, 0);
                //crb.position += newVelocity * force * dt;
                //particles[i].AddForce(-(particles[i].acceleration));
                //particles[i].AddForce(newVelocity * settings.bounce);

                collision = false;

            }
        }
        crb.position += force * dt;
        crb.euler += crb.aVelocity * dt;
    }
    public void MatrixRotation(Vector3C rotEuler)
    {
        for (int i = 0; i < crb.vertex.Length; i++)
        {
            crb.vertex[i] *= rotEuler * Mathf.PI / 180;
        }
    }

    public void Debug()
    {
        foreach (var item in settingsCollision.planes)
        {
            item.Print(Vector3C.red);
        }
    }
}
