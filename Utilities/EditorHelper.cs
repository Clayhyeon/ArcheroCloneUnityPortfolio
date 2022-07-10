using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorHelper
{
    public static void DrawCenterLabel(string label, int size, Color color, FontStyle fontStyle = FontStyle.Normal)
    {
        var style = new GUIStyle
        {
            padding =
            {
                top = 10,
                bottom = 10
            },
            fontSize = size,
            fontStyle = fontStyle,
            normal =
            {
                textColor = color
            }
        };

        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(label, style);
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }
}