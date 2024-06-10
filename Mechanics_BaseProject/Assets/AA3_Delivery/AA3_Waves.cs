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

            //Calculate 1rst wave:
            k = (2 * MathF.PI) / wavesSettings[0].frequency;
            points[i].position.x = points[i].originalposition.x + wavesSettings[0].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[0].direction) + elapsedTime) + wavesSettings[0].phase) * wavesSettings[0].direction.x;
            points[i].position.z = points[i].originalposition.z + wavesSettings[0].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[0].direction) + elapsedTime) + wavesSettings[0].phase) * wavesSettings[0].direction.z;
            points[i].position.y = wavesSettings[0].amplitude * MathF.Sin(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[0].direction) + elapsedTime) + wavesSettings[0].phase);

            //Add the rest of the waves:
            for (int j = 1; j < wavesSettings.Length; j++)
            {
                k = (2 * MathF.PI) / wavesSettings[j].frequency;
                points[i].position.x += points[i].originalposition.x + wavesSettings[j].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[j].direction) + elapsedTime) + wavesSettings[j].phase) * wavesSettings[j].direction.x;
                points[i].position.z += points[i].originalposition.z + wavesSettings[1].amplitude * k * MathF.Cos(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[j].direction) + elapsedTime) + wavesSettings[j].phase) * wavesSettings[j].direction.z;
                points[i].position.y += wavesSettings[j].amplitude * MathF.Sin(k * (Vector3C.Dot(points[i].originalposition, wavesSettings[j].direction) + elapsedTime) + wavesSettings[j].phase);

            }

            //Get wave height to calculate how deep has the buoy gone
            waveHeight = GetWaveHeight();
            submergenceDepth = waveHeight - (buoy.position.y - buoy.radius);
            //Adjust submergence depth so values don't break the calculations
            if (submergenceDepth < 0)
            {
                submergenceDepth = 0;
            }
            if (submergenceDepth > buoy.radius)
            {
                submergenceDepth = buoy.radius;
            }
            //Calculate volume underwater to get the force to be applied to the buoy
            volumeUnderwater = ((MathF.PI * (MathF.Pow(submergenceDepth, 2)) / 3)) * ((3 * buoy.radius) - submergenceDepth);
            flotabilityForce = volumeUnderwater * submergenceDepth * buoySettings.gravity;
            //Substract the force applied by the buoy's mass to get the total force
            netForce = flotabilityForce - (buoySettings.mass * buoySettings.gravity);
            //Taking the buoyancy coefficient into account to regulate the force applied
            netForce *= buoySettings.buoyancyCoefficient;
            //a = F/m
            buoyAcceleration = netForce / buoySettings.mass;
            //V = Vo + a*t
            buoySettings.buoyVelocity = buoySettings.buoyVelocity + buoyAcceleration * dt;

            //Update position with velocity depending on the time
            buoy.position.y += buoySettings.buoyVelocity * dt;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///Bueno, de los comentarios y c�digo anterior, ya que est�s, dime si falta algo muy evidente, porfa.
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Todos los comentarios de abajo a partir de aqu� los borrar� evidentemente
            //Esto de aqu� era para comprobar si se calculaba bien la altura de la ola, y parece que no.
            //buoy.position.y = waveHeight;
            //Si hago "UnityEngine.Debug.Log(waveHeight);", lo que se imprime como altura m�xima coincide
            //con la altura m�xima que alcanza la boya si uso "buoy.position.y = waveHeight;", entonces el problema est� en la funci�n

        }
    }

    public float GetWaveHeight()
    {
        float height = 0.0f;
        float k;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///Esto era un intento de hacer lo mismo que hemos hecho con las otras olas para calcularlas para ver si era esto,
        ///pero no cambia nada, s�lo que la altura es a�n mayor (aunque no se "rompe" nada de c�digo nin unity).
        ///No s� si es porque no est� bien o porque �ste es el bueno, ya que suma todas las olas y el otro s�lo cuenta la �ltima
        ///que entre en el "foreach".
        ///En cualquier caso, algo falla. Si se te ocurre algo super r�pido en 5 minutos o menos lo agradezco, si no,
        ///tampoco te entretengas, creo que esto que hay ser� suficiente.
        //k = (float)(2.0f * MathF.PI / wavesSettings[0].frequency);
        //height = (float)(wavesSettings[0].amplitude * k * MathF.Sin(Vector3C.Dot(buoy.position, wavesSettings[0].direction) + elapsedTime) + wavesSettings[0].phase);
        //for (int i = 1; i < wavesSettings.Length; i++)
        //{
        //    k = (float)(2.0f * MathF.PI / wavesSettings[i].frequency);
        //    height += (float)(wavesSettings[i].amplitude * k * MathF.Sin(Vector3C.Dot(buoy.position, wavesSettings[i].direction) + elapsedTime) + wavesSettings[i].phase);
        //}
        foreach (WavesSettings ws in wavesSettings)
        {
            k = (float)(2.0f * MathF.PI / ws.frequency);
            height = (float)(ws.amplitude * k * MathF.Sin(Vector3C.Dot(buoy.position, ws.direction) + elapsedTime) + ws.phase);
        }

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
