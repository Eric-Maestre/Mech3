using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityToCustom
{
    public static Vector3 ToUnity(this Vector3C v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
    public static Vector3C ToCustom(this Vector3 v)
    {
        return new Vector3C(v.x, v.y, v.z);
    }
    public static Color ToColor(this Vector3C v)
    {
        return new Color(v.r, v.g, v.b);
    }
    public static Vector3C ToCustom(this Color v)
    {
        return new Vector3C(v.r, v.g, v.b);
    }
    public static Vector3C ToCustomPosition(this Transform transform)
    {
        return ToCustom(transform.position);
    }
    public static Vector3C ToCustomRight(this Transform transform)
    {
        return ToCustom(transform.right);
    }
    public static Vector3C ToCustomUp(this Transform transform)
    {
        return ToCustom(transform.up);
    }
    public static Vector3C ToCustomForward(this Transform transform)
    {
        return ToCustom(transform.forward);
    }
    public static LineC ToCustom(this Ray v)
    {
        return new LineC(v.origin.ToCustom(), v.direction.ToCustom());
    }
    public static Ray ToUnity(this LineC v)
    {
        return new Ray(v.origin.ToUnity(), v.direction.ToUnity());
    }
    public static PlaneC ToCustom(this Plane v)
    {
        return new PlaneC(v.normal.ToCustom(), v.distance);
    }
    public static Plane ToUnity(this PlaneC v)
    {
        return new Plane(v.position.ToUnity(), v.normal.ToUnity());
    }
}
