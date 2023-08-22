using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DrawGiz : MonoBehaviour
{
    public float range;
    public Color color;
    public enum type
    {
        Rect,Sphere
    };

    public type Type;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        switch (Type)
        {
            case type.Rect:

                Gizmos.DrawCube(transform.position, new Vector3(range, range, range));
                break;

            case type.Sphere:
                Gizmos.DrawSphere(transform.position, range);
                break;
        }
    }
}
