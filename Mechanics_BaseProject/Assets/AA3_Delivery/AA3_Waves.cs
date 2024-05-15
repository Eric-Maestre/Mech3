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
    public void Update(float dt)
    {
        Random rnd = new Random();
        for(int i = 0; i < points.Length; i++)
        {
            points[i].position = points[i].originalposition;
            points[i].position.y = rnd.Next(100) * 0.01f;
        }
    }

    public void Debug()
    {
        if(points != null)
        foreach (var item in points)
        {
            item.originalposition.Print(0.05f);
            item.position.Print(0.05f);
        }
    }
}
