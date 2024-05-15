using System;

[System.Serializable]
public struct PlaneC
{
    #region FIELDS
    public Vector3C position;
    public Vector3C normal;
    #endregion

    #region PROPIERTIES
    #endregion

    #region CONSTRUCTORS
    public PlaneC(Vector3C position, Vector3C normal)
    {
        this.position = position;
        this.normal = normal;
        this.normal.Normalize();
    }
    public PlaneC(float A, float B, float C, float D)
    {
        PlaneC temp = new PlaneC(new(A, B, C), D);
        this.position = temp.position;
        this.normal = temp.normal;
    }
    public PlaneC(Vector3C n, float D)
    {
        this.position = new Vector3C();
        this.normal = new Vector3C();
    }
    public PlaneC(Vector3C A, Vector3C B, Vector3C C)
    {
        Vector3C AB = B - A;
        AB.Normalize();
        Vector3C AC = C - A;
        AC.Normalize();
        Vector3C cross = Vector3C.Cross(AB, AC);
        cross.Normalize();
        this.position = A;
        this.normal = cross;
    }
    #endregion

    #region OPERATORS
    #endregion

    #region METHODS
    /// <summary>
    /// ax + by + cz + d = 0
    /// </summary>
    /// <returns></returns>
    
    public (float A, float B, float C, float D) ToEquation()
    {
        normal.Normalize();
        float D = 0f - Vector3C.Dot(normal, position);
        return (normal.x, normal.y, normal.z, D);
    }
    #endregion

    #region FUNCTIONS
    public Vector3C NearestPoint(Vector3C point)
    {
        return new Vector3C();
    }
    public Vector3C Intersection(LineC line)
    {
        line.direction.Normalize();
        var diff = line.origin - position;
        var prod1 = Vector3C.Dot(diff, normal);
        var prod2 = Vector3C.Dot(line.direction, normal);
        var distance = prod1 / prod2;
        return line.origin - line.direction * distance;
    }
    public float FrontOrBehind(Vector3C point)
    {
        return Vector3C.Dot(normal, point - position);
    }
    public (Vector3C position, Vector3C velocity) Collision(Vector3C position, Vector3C positionLast, Vector3C velocity, float restitutionCoeficcient = 1)
    {

        if (FrontOrBehind(position) < 0)
        {
            position = Intersection(LineC.FromTwoPoints(positionLast, position));
            velocity = Vector3C.Reflect(velocity.normalized, normal) * velocity.magnitude * restitutionCoeficcient;
        }
        return (position, velocity);
    }
    #endregion

}