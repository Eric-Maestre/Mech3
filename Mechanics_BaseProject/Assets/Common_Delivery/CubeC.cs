using System;

[System.Serializable]
public struct CubeC
{
    #region FIELDS
    public Vector3C position;
    public Vector3C scale;
    public Vector3C euler;
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
        return false;
    }
    #endregion

    #region FUNCTIONS
    #endregion

}