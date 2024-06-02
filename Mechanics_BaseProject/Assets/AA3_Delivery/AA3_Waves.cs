using System;
using System.Threading.Tasks;

[System.Serializable]
public class AA3_Waves
{
    [System.Serializable]
    public struct Settings
    {

    }
    public Settings settings;
    [System.Serializable]
    public struct WavesSettings
    {
        public float amplitude;//Amplitud de la ola
        public float frequency;//Frecuencia de la ola
        public float phase;//Fase inicial de la ola
        public Vector3C direction;//Direcci�n de la ola
        public float speed;//Velocidad de propagaci�n de la ola
    }
    public WavesSettings[] wavesSettings;
    public struct Vertex
    {
        public Vector3C originalposition;
        public Vector3C position;
        public Vertex(Vector3C _position)
        {
            this.position = _position;
            this.originalposition = _position;
        }
    }
    public Vertex[] points;
    [System.Serializable]
    public struct BuoySettings
    {
        public float buoyancyCoefficient;
        public float buoyVelocity;
        public float mass;
        public float waterDensity;
        public float gravity;
    }
    public BuoySettings buoySettings;
    public SphereC buoy;

    private float elapsedTime;//Tiempo acumulado
    public AA3_Waves()
    {
        elapsedTime = 0.0f;
    }
    float submergenceDepth;
    float volumeUnderwater;
    float flotabilityForce;
    float netForce;
    float buoyAcceleration;
    float k;
    public void Update(float dt)
    {
        Random rnd = new Random();
        elapsedTime += dt;
        float waveHeight;
        for (int i = 0; i < points.Length; i++)
        {
            
            points[i].position = points[i].originalposition;
            points[i].position.y = rnd.Next(100) * 0.01f;
            //k = 2PI/lambda
            //lambda = frecuencia
            //X = Xo + A * k * cos(k*(Xo * W + t)+ Phi) * Wx
            //Z = Zo + A * k * cos(k*(Xo * W + t)+ Phi) * Wz
            //Y = A * sen(k*(Xo * W + t)+ Phi)
            k= (2 * MathF.PI) / wavesSettings[0].frequency;
            points[i].position.x = points[i].originalposition.x + wavesSettings[0].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[0].direction) + elapsedTime) + wavesSettings[0].phase) * wavesSettings[0].direction.x;
            points[i].position.z = points[i].originalposition.z + wavesSettings[0].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[0].direction) + elapsedTime) + wavesSettings[0].phase) * wavesSettings[0].direction.z;
            points[i].position.y = wavesSettings[0].amplitude * MathF.Sin(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[0].direction) + elapsedTime) + wavesSettings[0].phase);

            points[i].position.x += points[i].originalposition.x + wavesSettings[1].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[1].direction) + elapsedTime) + wavesSettings[1].phase) * wavesSettings[1].direction.x;
            points[i].position.z += points[i].originalposition.z + wavesSettings[1].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[1].direction) + elapsedTime) + wavesSettings[1].phase) * wavesSettings[1].direction.z;
            points[i].position.y += wavesSettings[1].amplitude * MathF.Sin(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[1].direction) + elapsedTime) + wavesSettings[1].phase);

            points[i].position.x += points[i].originalposition.x + wavesSettings[2].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[2].direction) + elapsedTime) + wavesSettings[2].phase) * wavesSettings[2].direction.x;
            points[i].position.z += points[i].originalposition.z + wavesSettings[2].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[2].direction) + elapsedTime) + wavesSettings[2].phase) * wavesSettings[2].direction.z;
            points[i].position.y += wavesSettings[2].amplitude * MathF.Sin(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[2].direction) + elapsedTime) + wavesSettings[2].phase);

            points[i].position.x += points[i].originalposition.x + wavesSettings[3].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[3].direction) + elapsedTime) + wavesSettings[3].phase) * wavesSettings[3].direction.x;
            points[i].position.z += points[i].originalposition.z + wavesSettings[3].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[3].direction) + elapsedTime) + wavesSettings[3].phase) * wavesSettings[3].direction.z;
            points[i].position.y += wavesSettings[3].amplitude * MathF.Sin(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[3].direction) + elapsedTime) + wavesSettings[3].phase);
            waveHeight = GetWaveHeight(buoy.position.x, buoy.position.z);
            submergenceDepth = waveHeight - (buoy.position.y - buoy.radius);
            if (submergenceDepth < 0)
            {
                submergenceDepth = 0;
            }
            volumeUnderwater = ((MathF.PI * MathF.Pow(volumeUnderwater, 2)) / 3) * ((3 * buoy.radius) - volumeUnderwater);
            if (volumeUnderwater < 0)
            {
                volumeUnderwater = 0;
            }

            


            flotabilityForce = volumeUnderwater * submergenceDepth * buoySettings.gravity;
            netForce = flotabilityForce - (buoySettings.mass * buoySettings.gravity * 0);
            buoyAcceleration = netForce / buoySettings.mass;
            buoySettings.buoyVelocity = buoySettings.buoyVelocity + buoyAcceleration * MathF.Pow(elapsedTime, 2);
            buoy.position.y += buoySettings.buoyVelocity;
            
        }
        float a = points[8].position.y;
        waveHeight = GetWaveHeight(buoy.position.x, buoy.position.z);
        UnityEngine.Debug.Log(waveHeight);
    }
    //public float GetWaveHeight(float x, float z)
    //{
    //    float height = 0;
    //    for (int i = 0; i < wavesSettings.Length; i++)
    //    {
    //        float k = (2 * MathF.PI) / wavesSettings[i].frequency;
    //        float waveFactor = k * (Vector3C.Dot(new Vector3C(x, 0, z), wavesSettings[i].direction) + elapsedTime * wavesSettings[i].speed) + wavesSettings[i].phase;
    //        height += wavesSettings[i].amplitude * MathF.Sin(waveFactor);
    //    }
    //    UnityEngine.Debug.Log(height);
    //    return height;
    //}
    public float GetWaveHeight(float x, float z)
    {
        float height = 0;
        for (int i = 0; i < wavesSettings.Length; i++)
        {
            float k = (2 * MathF.PI) / wavesSettings[i].frequency;
            points[i].position.y = wavesSettings[0].amplitude * MathF.Sin(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[0].direction) + elapsedTime) + wavesSettings[0].phase);
            float waveFactor = k * (Vector3C.Dot(new Vector3C(x, 0, z), wavesSettings[i].direction) + elapsedTime * wavesSettings[i].speed) + wavesSettings[i].phase;
            height += wavesSettings[i].amplitude * MathF.Sin(waveFactor);
        }
        //UnityEngine.Debug.Log(height);
        return height;
    }
    public void Debug()
    {
        buoy.Print(Vector3C.blue);
        if(points != null)
        foreach (var item in points)
        {
            item.originalposition.Print(0.05f);
            item.position.Print(0.05f);
        }
    }
}
