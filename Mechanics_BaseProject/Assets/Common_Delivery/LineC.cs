using System;

[System.Serializable]
public struct LineC
{
    #region FIELDS
    public Vector3C origin;
    public Vector3C direction;
    #endregion

    #region PROPIERTIES
    #endregion

    #region CONSTRUCTORS
    public LineC(Vector3C origin, Vector3C direction)
    {
        this.origin = origin;
        this.direction = direction;
        this.direction.Normalize();
    }
    #endregion

    #region OPERATORS
    #endregion

    #region METHODS
    public float TangentDistanceToOrigin(Vector3C point)
    {
        Vector3C originPoint = point - origin;
        return Vector3C.Dot(originPoint, direction);
    }
    public Vector3C PointGivenDistance(float distance)
    {
        return origin + direction * distance;
    }
    public Vector3C NearestPoint(Vector3C point)
    {
        return PointGivenDistance(TangentDistanceToOrigin(point));
    }
    #endregion

    #region FUNCTIONS
    public static LineC FromTwoPoints(Vector3C start, Vector3C end)
    {
        return new LineC(start, end - start);
    }
    #endregion

}