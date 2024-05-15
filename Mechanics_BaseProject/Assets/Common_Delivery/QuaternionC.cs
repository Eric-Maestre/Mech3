using System;

[System.Serializable]
public struct QuaternionC
{
    #region FIELDS
    public Vector3C vector;
    public float rotation;
    #endregion

    #region PROPIERTIES

    #endregion

    #region CONSTRUCTORS
    public QuaternionC(Vector3C _vector, float _rotation)
    {
        this.vector = _vector;
        this.rotation = _rotation;
    }
    #endregion

    #region OPERATORS
    public static QuaternionC operator *(QuaternionC a, QuaternionC b)
    {

        QuaternionC multipliedQuaternionC = new QuaternionC();
        return new QuaternionC(multipliedQuaternionC.vector, multipliedQuaternionC.rotation);
    }
    #endregion

    #region METHODS

    #endregion

    #region FUNCTIONS

    #endregion
}