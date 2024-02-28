using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class path
{
    [SerializeField, HideInInspector] List<Vector2> points;

    public path(Vector2 center)
    {
        points = new List<Vector2>
        {
            center + Vector2.left,
            center + (Vector2.left + Vector2.up) * .5f,
            center + (Vector2.right + Vector2.down) * .5f,
            center + Vector2.right
        };
    }

    public Vector2 this[int i]
    {
        get { return points[i]; }
    }

    public int numpoints
    {
        get { return points.Count; }
    }

    public int numsegment
    {
        get { return (points.Count - 4) / 3 + 1; }
    }

    public void addsegment(Vector2 ancorpos)
    {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + ancorpos) * .5f);
        points.Add(ancorpos);
    }

    public Vector2[] GetPointInSegment(int i)
    {
        return new Vector2[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3] };
    }

    public void movepoint(int i, Vector2 pos)
    {
        points[i] = pos;
    }
}