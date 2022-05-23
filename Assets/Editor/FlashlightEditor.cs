using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Flashlight))]
public class FlashlightEditor : Editor
{
    private void OnSceneGUI() {

        Flashlight flashlight = (Flashlight)target;
        /*
        Handles.color = Color.white;
        
        Handles.DrawWireArc(flashlight.transform.position, Vector3.up, Vector3.forward, 360, flashlight.viewRadius);

        /*Vector3 viewAngle1 = flashlight.DirectionFromAngle(-flashlight.viewAngle/2, false);
        
        Vector3 viewAngle2 = flashlight.DirectionFromAngle(flashlight.viewAngle/2, false);
        
        */
        
        Handles.color = Color.blue;
        if (flashlight.IsOn) {
            Handles.DrawLine(flashlight.transform.position, flashlight.hit.point);
        }

        /*
        Handles.DrawLine(flashlight.transform.position, flashlight.transform.position + viewAngle1 * flashlight.viewRadius);
        Handles.DrawLine(flashlight.transform.position, flashlight.transform.position + viewAngle2 * flashlight.viewRadius);

        if (flashlight.CanSeePlayer())
        {
            Handles.color = Color.green;
            Handles.DrawLine(flashlight.transform.position, flashlight.GetLastKnownTargetPosition());

        }*/
    }

}
