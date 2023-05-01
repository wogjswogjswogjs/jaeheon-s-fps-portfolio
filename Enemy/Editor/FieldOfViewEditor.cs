using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StateController))]
public class FieldOfViewEditor : Editor
{
    Vector3 DirFromAngle(Transform transform, float angleInDegrees, bool anglesGlobal)
    {
        if (!anglesGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0.0f,
            Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnSceneGUI()
    {
        
        
    }
}
