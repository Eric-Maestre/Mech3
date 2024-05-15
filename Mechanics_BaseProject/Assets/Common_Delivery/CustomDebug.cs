using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomDebug
{
    public static void Print(this Vector3C obj, float radius = 1)
    {
#if UNITY_EDITOR
        obj.ToUnity().Print(radius);
#endif
    }
    public static void Print(this Vector3 obj, float radius = 1)
    {
#if UNITY_EDITOR
        Debug.DrawRay(obj, Vector3.up * radius, Color.green);
        Debug.DrawRay(obj, Vector3.right * radius, Color.red);
        Debug.DrawRay(obj, Vector3.forward * radius, Color.blue);
#endif
    }
    public static void Print(this LineC obj, Vector3C color, bool infinite = true)
    {
#if UNITY_EDITOR
        obj.origin.ToUnity().Print();
        (obj.origin.ToUnity() + obj.direction.ToUnity()).Print(0.5f);
        if(infinite )
        {
            Debug.DrawRay(obj.origin.ToUnity() - obj.direction.ToUnity() * 1000, obj.direction.ToUnity() * 2000, color.ToColor());
            Debug.DrawRay(obj.origin.ToUnity(), obj.direction.ToUnity());
        }
        else
        {
            Debug.DrawRay(obj.origin.ToUnity(), obj.direction.ToUnity(), color.ToColor());
        }
#endif
    }
    public static void Print(this Ray obj, Vector3C color, bool infinite = false)
    {
#if UNITY_EDITOR
        obj.ToCustom().Print(color, infinite);
#endif
    }
    public static void Print(this PlaneC obj, Vector3C color, float scale = 1)
    {
#if UNITY_EDITOR
        Color tempColor = Gizmos.color;
        Matrix4x4 tempMatrix = Gizmos.matrix;
        Color newColor = color.ToColor();
        newColor.a = 0.25f;
        Gizmos.color = newColor;
        Gizmos.matrix = Matrix4x4.TRS(obj.position.ToUnity(), Quaternion.LookRotation(obj.normal.ToUnity()), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new(scale, scale, 0));
        newColor.a = 0.25f;
        Gizmos.color = newColor;
        Gizmos.DrawCube(Vector3.zero, new(scale, scale, 0));
        Gizmos.matrix = tempMatrix;
        Gizmos.color = tempColor;
#endif
    }
    public static void Print(this Plane obj, Vector3C color, float scale = 1)
    {
#if UNITY_EDITOR
        obj.ToCustom().Print(color, scale);
#endif
    }
    public static void Print(this SphereC obj, Vector3C color)
    {
#if UNITY_EDITOR
        Color tempColor = Gizmos.color;
        Matrix4x4 tempMatrix = Gizmos.matrix;
        Color newColor = color.ToColor();
        Gizmos.color = newColor;
        Gizmos.DrawWireSphere(obj.position.ToUnity(), obj.radius);
        newColor.a = 0.5f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(obj.position.ToUnity(), obj.radius);
        Gizmos.color = tempColor;
#endif
    }
    public static void Print(this CapsuleC obj, Vector3C color)
    {
#if UNITY_EDITOR
        Color tempColor = Gizmos.color;
        Matrix4x4 tempMatrix = Gizmos.matrix;
        Color newColor = color.ToColor();
        newColor.a = 0.5f;
        Gizmos.color = newColor;
        Debug.DrawLine(obj.positionA.ToUnity(), obj.positionB.ToUnity(), newColor);
        Debug.DrawLine(obj.positionA.ToUnity() + Vector3.up * obj.radius, obj.positionB.ToUnity() + Vector3.up * obj.radius, newColor);
        Debug.DrawLine(obj.positionA.ToUnity() - Vector3.up * obj.radius, obj.positionB.ToUnity() - Vector3.up * obj.radius, newColor);
        Debug.DrawLine(obj.positionA.ToUnity() + Vector3.right * obj.radius, obj.positionB.ToUnity() + Vector3.right * obj.radius, newColor);
        Debug.DrawLine(obj.positionA.ToUnity() - Vector3.right * obj.radius, obj.positionB.ToUnity() - Vector3.right * obj.radius, newColor);
        Debug.DrawLine(obj.positionA.ToUnity() + Vector3.forward * obj.radius, obj.positionB.ToUnity() + Vector3.forward * obj.radius, newColor);
        Debug.DrawLine(obj.positionA.ToUnity() - Vector3.forward * obj.radius, obj.positionB.ToUnity() - Vector3.forward * obj.radius, newColor);
        Gizmos.DrawWireSphere(obj.positionA.ToUnity(), obj.radius);
        Gizmos.DrawWireSphere(obj.positionB.ToUnity(), obj.radius);
        Gizmos.color = tempColor;
#endif
    }
}
