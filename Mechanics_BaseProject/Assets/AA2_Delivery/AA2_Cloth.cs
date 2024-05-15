using System;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class AA2_Cloth
{
    [System.Serializable]
    public struct Settings
    {
        public Vector3C gravity;
        [Min(1)]
        public float width;
        [Min(1)]
        public float height;
        [Min(2)]
        public int xPartSize;
        [Min(2)]
        public int yPartSize;
    }
    public Settings settings;
    [System.Serializable]
    public struct ClothSettings
    {
        [Header("Structural Sring")]
        public float structuralElasticCoef;
        public float structuralDampCoef;
        public float structuralSpring;
        [Header("")]
        public float structuralMaxL; //distancia maxima entre vertices contiguos
        [Header("Shear Sring")]
        public float shearElasticCoef;
        public float shearDampCoef;
        public float shearSpring;
        [Header("")]
        public float shearMaxL;
        [Header("Bending Sring")]
        public float bendingElasticCoef;
        public float bendingDampCoef;
        public float bendingSpring;
        [Header("")]
        public float bendingMaxL;
    }
    public ClothSettings clothSettings;

    [System.Serializable]
    public struct SettingsCollision
    {
        public SphereC sphere;
    }
    public SettingsCollision settingsCollision;
    public struct Vertex
    {
        public Vector3C lastPosition;
        public Vector3C actualPosition;
        public Vector3C velocity;
        public Vertex(Vector3C _position)
        {
            this.actualPosition = _position;
            this.lastPosition = _position;
            this.velocity = new Vector3C(0, 0, 0);
        }

        public void Euler(Vector3C force, float dt)
        {
            lastPosition = actualPosition;
            velocity += force * dt;
            actualPosition += velocity * dt;
        }
    }
    public Vertex[] points;
    public void Update(float dt)
    {
        int xVertices = settings.xPartSize + 1;

        Vector3C[] structuralForces = new Vector3C[points.Length];
        Vector3C[] shearForces = new Vector3C[points.Length];
        Vector3C[] bendingForces = new Vector3C[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            //STRUCTURAL VERTICAL
            if(i > xVertices-1)
            {
                float structMagnitudeY = (points[i - xVertices].actualPosition - points[i].actualPosition).magnitude
                                        - clothSettings.structuralSpring;

                structMagnitudeY *= clothSettings.structuralMaxL
                    * (points[i - xVertices].actualPosition - points[i].actualPosition).magnitude;

                Vector3C structForceVector = (points[i - xVertices].actualPosition
                    - points[i].actualPosition).normalized * structMagnitudeY * clothSettings.structuralElasticCoef;
                
                Vector3C structDampForceVector = (points[i].velocity
                    - points[i - xVertices].velocity) * clothSettings.structuralDampCoef;
                structuralForces[i] += structForceVector;
                structuralForces[i] += -structDampForceVector;
                structuralForces[i - xVertices] += -structForceVector;
            }
            //STRUCTURAL HORIZONTAL
            if (i%(xVertices) != 0)
            {
                float structMagnitudeX = (points[i - 1].actualPosition - points[i].actualPosition).magnitude
                                        - clothSettings.structuralSpring;

                structMagnitudeX *= clothSettings.structuralMaxL
                    * (points[i - 1].actualPosition - points[i].actualPosition).magnitude;

                Vector3C structForceVector = (points[i - 1].actualPosition
                    - points[i].actualPosition).normalized * structMagnitudeX * clothSettings.structuralElasticCoef;
                Vector3C structDampForceVector = (points[i].velocity
                    - points[i - 1].velocity) * clothSettings.structuralDampCoef;
                structuralForces[i] += structForceVector;
                structuralForces[i] += -structDampForceVector;
                structuralForces[i - 1] += -structForceVector;
            }
            //SHEAR DIAGONAL IZQ-ABJ/DER-ARR
            if (i > xVertices && i % (xVertices) != 0)
            {
                float structMagnitudeDiagonalAsc = (points[i - xVertices + 1].actualPosition - points[i].actualPosition).magnitude
                    - clothSettings.shearSpring;

                structMagnitudeDiagonalAsc *= clothSettings.shearMaxL
                    * (points[i - xVertices + 1].actualPosition - points[i].actualPosition).magnitude;

                Vector3C shearForceVector = (points[i - xVertices + 1].actualPosition
                    - points[i].actualPosition).normalized * structMagnitudeDiagonalAsc * clothSettings.shearElasticCoef;
                Vector3C shearDampForceVector = (points[i].velocity
                    - points[i - xVertices + 1].velocity) * clothSettings.shearDampCoef;
                shearForces[i] += shearForceVector;
                shearForces[i] += -shearDampForceVector;
                shearForces[i - xVertices + 1] += -shearForceVector;
            }
            ////SHEAR DIAGONAL DER-ABJ/IZQ-ARR
            if (i > xVertices - 1 && i % (xVertices) != 0)
            {
                float structMagnitudeDiagonalDesc = (points[i - xVertices - 1].actualPosition - points[i].actualPosition).magnitude
                    - clothSettings.shearSpring;

                structMagnitudeDiagonalDesc *= clothSettings.shearMaxL
                    * (points[i - xVertices - 1].actualPosition - points[i].actualPosition).magnitude;

                Vector3C shearForceVector = (points[i - xVertices - 1].actualPosition
                    - points[i].actualPosition).normalized * structMagnitudeDiagonalDesc * clothSettings.shearElasticCoef;
                Vector3C shearDampForceVector = (points[i].velocity
                    - points[i - xVertices - 1].velocity) * clothSettings.shearDampCoef;
                shearForces[i] += shearForceVector;
                shearForces[i] += -shearDampForceVector;
                shearForces[i - xVertices - 1] += -shearForceVector;
            }

            //BENDING VERTICAL
            if (i > ((xVertices*2)-1))
            {
                float bendingMagnitudeY = (points[i - (xVertices * 2)].actualPosition - points[i].actualPosition).magnitude
                                        - clothSettings.bendingSpring;

                bendingMagnitudeY *= clothSettings.bendingMaxL
                    * (points[i - (xVertices * 2)].actualPosition - points[i].actualPosition).magnitude;

                Vector3C bendingForceVector = (points[i - (xVertices * 2)].actualPosition
                    - points[i].actualPosition).normalized * bendingMagnitudeY * clothSettings.bendingElasticCoef;

                Vector3C bendingDampForceVector = (points[i].velocity
                    - points[i - (xVertices * 2)].velocity) * clothSettings.bendingDampCoef;
                bendingForces[i] += bendingForceVector;
                bendingForces[i] += -bendingDampForceVector;
                bendingForces[i - (xVertices * 2)] += -bendingForceVector;
            }
            //BENDING HORIZONTAL
            if (i % xVertices != 0 && i > 1)
            {
                float bendingMagnitudeX = (points[i - 2].actualPosition - points[i].actualPosition).magnitude
                                       - clothSettings.bendingSpring;

                bendingMagnitudeX *= clothSettings.bendingMaxL
                    * (points[i - 2].actualPosition - points[i].actualPosition).magnitude;

                Vector3C bendingForceVector = (points[i - 2].actualPosition
                    - points[i].actualPosition).normalized * bendingMagnitudeX * clothSettings.bendingElasticCoef;
                Vector3C bendingDampForceVector = (points[i].velocity
                    - points[i - 2].velocity) * clothSettings.bendingDampCoef;
                bendingForces[i] += bendingForceVector;
                bendingForces[i] += -bendingDampForceVector;
                bendingForces[i - 2] += -bendingForceVector;
            }
        }
        for (int i = 0; i < points.Length; i++)
        {
            if (i != 0 && i != xVertices - 1)
                points[i].Euler(settings.gravity + structuralForces[i] + shearForces[i] + bendingForces[i], dt);

            //Collision

            //V, vlocidad que llevo al colisionar
            //me quedo, pierdo la fuera en y pero conservo en x hasta que la Ff me fren completamente
            //V = Vx + Vy, Vx = V - Vy. Vy proyeccion sobre la normal

            //Deteccion de la colision
            bool entered = settingsCollision.sphere.IsInside(points[i].actualPosition);
            if(entered)
            {
                //Calcular Vy
                Vector3C normal = points[i].actualPosition - settingsCollision.sphere.position;
                points[i].actualPosition = settingsCollision.sphere.position +
                    normal.normalized * settingsCollision.sphere.radius;
                float vyMagnitude = Vector3C.Dot(normal.normalized, points[i].velocity);
                Vector3C vy = normal * vyMagnitude;

                //Calcular Vx
                Vector3C vx = points[i].velocity - vy;

                //Nueva velocidad
                points[i].velocity = vx;
            }

        }

    }

    public void Debug()
    {
        settingsCollision.sphere.Print(Vector3C.blue);

        if (points != null)
            foreach (var item in points)
            {
                item.lastPosition.Print(0.05f);
                item.actualPosition.Print(0.05f);
            }
    }
}
