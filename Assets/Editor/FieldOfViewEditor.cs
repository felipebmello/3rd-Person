using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Events;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI() 
    {

        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);

        Vector3 viewAngle1 = fov.DirectionFromAngle(-fov.viewAngle/2, false);
        
        Vector3 viewAngle2 = fov.DirectionFromAngle(fov.viewAngle/2, false);
        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle1 * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle2 * fov.viewRadius);


        if (fov.CanSeeTarget())
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.GetLastKnownTargetPosition());

        }
    }


}
