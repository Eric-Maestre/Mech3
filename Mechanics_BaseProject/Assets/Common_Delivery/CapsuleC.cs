using System;

[System.Serializable]
public struct CapsuleC
{
    #region FIELDS
    public Vector3C positionA;
    public Vector3C positionB;
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
        return Vector3C.Distance(NearestCenterPoint(point), point) <= radius;
    }
    public Vector3C NearestCenterPoint(Vector3C point)
    {
        LineC ab = LineC.FromTwoPoints(positionA, positionB);
        LineC ba = LineC.FromTwoPoints(positionB, positionA);

        float abd = ab.TangentDistanceToOrigin(point);
        float bad = ba.TangentDistanceToOrigin(point);

        if(abd > 0 && bad > 0)
        {
            return ab.PointGivenDistance(abd);
        }
        else if(abd < 0)
        {
            return positionA;
        }
        else
        {
            return positionB;
        }
        return new Vector3C();
    }
    public Vector3C Intersection(LineC line)
    {
        Vector3C ba = positionB - positionA;
        Vector3C oa = line.origin - positionA;

        float baba = Vector3C.Dot(ba, ba);
        float bard = Vector3C.Dot(ba, line.direction);
        float baoa = Vector3C.Dot(ba, oa);
        float rdoa = Vector3C.Dot(line.direction, oa);
        float oaoa = Vector3C.Dot(oa, oa);

        float a = baba - bard * bard;
        float b = baba * rdoa - baoa * bard;
        float c = baba * oaoa - baoa * baoa - radius * radius * baba;
        float h = b * b - a * c;
        if (h >= 0.0)
        {
            float t = (-b - (float)Math.Sqrt(h)) / a;
            float y = baoa + t * bard;
            // body
            if (y > 0.0 && y < baba) return line.origin + line.direction * (t);
            // caps
            Vector3C oc = (y <= 0.0) ? oa : line.origin - positionB;
            b = Vector3C.Dot(line.direction, oc);
            c = Vector3C.Dot(oc, oc) - radius * radius;
            h = b * b - c;
            if (h > 0.0) return line.origin + line.direction * (-b - (float)Math.Sqrt(h));
        }
        //return -1.0;
        return new Vector3C();
    }
    public (Vector3C position, Vector3C velocity) Collision(Vector3C position, Vector3C positionLast, Vector3C velocity, float restitutionCoeficcient = 1)
    {

        if (IsInside(position))
        {
            Vector3C collision = Intersection(LineC.FromTwoPoints(positionLast, position));
            PlaneC plane = new PlaneC(collision, (collision - NearestCenterPoint(collision)).normalized);
            return plane.Collision(position, positionLast, velocity, restitutionCoeficcient);
        }
        return (position, velocity);
    }
    #endregion

    #region FUNCTIONS
    #endregion

}