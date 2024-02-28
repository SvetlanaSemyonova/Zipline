using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class patheditor : Editor
{

    PathCreator creator;
    Path Path;
    private void OnSceneGUI()
    {
        Draw();
       Input();

    }

    void Input()
    {
        Event guievent = Event.current;
        Vector2 mousepos = HandleUtility.GUIPointToWorldRay(guievent.mousePosition).origin;

        if(guievent.type==EventType.MouseDown &&guievent.button==0)
        {
            Undo.RecordObject(creator, "Add Segment");
            Path.addsegment(mousepos);

        }

    }

    void Draw()
    {
        for (int i = 0; i < Path.numsegment; i++)
        {
            Vector2[] points = Path.GetPointInSegment(i);
            Handles.color = Color.black;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.red, null, 2);
        }
        Handles.color = Color.green;
        for (int i = 0; i < Path.numpoints; i++)
        {

            Vector2 newpos = Handles.FreeMoveHandle(Path[i], Quaternion.identity, .1f, Vector2.zero, Handles.CylinderHandleCap);

           
            if(Path[i]!=newpos)
            {
                Undo.RecordObject(creator, "Move Point");
                Path.movepoint(i, newpos);


            }
        }


    }

     void OnEnable()
    {
        creator = (PathCreator)target;
        if(creator.Path==null)
        {
            creator.CreatePath();

        }

        Path = creator.Path;

    }
}
