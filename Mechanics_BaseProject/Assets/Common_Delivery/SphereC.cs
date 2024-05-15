using System;

[System.Serializable]
public struct SphereC
{
    #region FIELDS
    public Vector3C position;
    public float radius;
    #endregion

    #region PROPIERTIES
    #endregion

    #region CONSTRUCTORS
    #endregion

    #region OPERATORS
    #endregion

    #region METHODS
    public bool IsInside(Vector3C point)
    {
        return Vector3C.Distance(point, position) < radius;
    }

    //float sphIntersect(in vec3 ro, in vec3 rd, in vec4 sph)
    //{
    //    vec3 oc = ro - sph.xyz;
    //    float b = dot(oc, rd);
    //    float c = dot(oc, oc) - sph.w * sph.w;
    //    float h = b * b - c;
    //    if (h < 0.0) return -1.0;
    //    return -b - sqrt(h);
    //}
    public Vector3C Intersection(LineC line)
    {
        Vector3C dif = line.origin - position;
        float dot = Vector3C.Dot(dif, line.direction);
        float dist = Vector3C.Dot(dif, dif) - radius * radius;
        float h = dot * dot - dist;
        if(h < 0)
        {
            return line.origin;
        }
        return line.origin + line.direction * (-dot - (float)Math.Sqrt(h));
    }

    public (Vector3C position, Vector3C velocity) Collision(Vector3C position, Vector3C positionLast, Vector3C velocity, float restitutionCoeficcient = 1)
    {

        if (IsInside(position))
        {
            Vector3C collision = Intersection(LineC.FromTwoPoints(positionLast, position));
            PlaneC plane = new PlaneC(collision, (collision - this.position).normalized);
            return plane.Collision(position, positionLast, velocity, restitutionCoeficcient);
        }
        return (position, velocity);
    }
    #endregion

    #region FUNCTIONS
    #endregion

}