using System;

[System.Serializable]
public struct Vector3C
{
    #region FIELDS
    public float x;
    public float y;
    public float z;
    #endregion

    #region PROPIERTIES
    public float r { get => x; set => x = value; }
    public float g { get => y; set => y = value; }
    public float b { get => z; set => z = value; }
    public float magnitude { get { return (float)Math.Sqrt(x * x + y * y + z * z); } }
    public Vector3C normalized
    {
        get
        {
            var temp = new Vector3C(x, y, z);
            temp.Normalize();
            return temp;
        }
    }

    public static Vector3C zero { get { return new Vector3C(0, 0, 0); } }
    public static Vector3C one { get { return new Vector3C(1, 1, 1); } }
    public static Vector3C right { get { return new Vector3C(1, 0, 0); } }
    public static Vector3C up { get { return new Vector3C(0, 1, 0); } }
    public static Vector3C forward { get { return new Vector3C(0, 0, 1); } }

    public static Vector3C black { get { return new Vector3C(0, 0, 0); } }
    public static Vector3C white { get { return new Vector3C(1, 1, 1); } }
    public static Vector3C red { get { return new Vector3C(1, 0, 0); } }
    public static Vector3C green { get { return new Vector3C(0, 1, 0); } }
    public static Vector3C blue { get { return new Vector3C(0, 0, 1); } }
    #endregion

    #region CONSTRUCTORS
    public Vector3C(float x = 0, float y = 0, float z = 0)
    {
        this.x = x; this.y = y; this.z = z;
    }
    #endregion

    #region OPERATORS
    public static Vector3C operator -(Vector3C a)
    {
        return new Vector3C(-a.x, -a.y, -a.z);
    }
    public static Vector3C operator +(Vector3C a, Vector3C b)
    {
        return new Vector3C(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static Vector3C operator -(Vector3C a, Vector3C b)
    {
        return a + -b;
    }
    public static Vector3C operator *(Vector3C a, Vector3C b)
    {
        return new Vector3C(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public static Vector3C operator /(Vector3C a, Vector3C b)
    {
        return new Vector3C(a.x / b.x, a.y / b.y, a.z / b.z);
    }
    public static Vector3C operator *(Vector3C a, float b)
    {
        return new Vector3C(a.x * b, a.y * b, a.z * b);
    }
    public static Vector3C operator /(Vector3C a, float b)
    {
        return new Vector3C(a.x / b, a.y / b, a.z / b);
    }
    #endregion

    #region METHODS
    public void Normalize()
    {
        float m = this.magnitude;
        this.x /= m;
        this.y /= m;
        this.z /= m;
    }
    #endregion

    #region FUNCTIONS
    public static float Dot(Vector3C a, Vector3C b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
    public static Vector3C Cross(Vector3C a, Vector3C b)
    {
        return new Vector3C(a.y * b.z - a.z * a.y, a.z * b.x - a.x * a.z, a.x * b.y - a.y * a.x);
    }
    public static float Distance(Vector3C a, Vector3C b)
    {
        return (a - b).magnitude;
    }

    public static Vector3C Lerp(Vector3C a, Vector3C b, float t)
    {
        return new Vector3C(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
    }
    public static Vector3C Reflect(Vector3C direction, Vector3C normal)
    {
        float num = -2f * Dot(normal, direction);
        return new Vector3C(num * normal.x + direction.x, num * normal.y + direction.y, num * normal.z + direction.z);
    }
    #endregion

}