﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MageQuitMod
{
    public static class Renderer
    {
        public static GUIStyle StringStyle { get; set; } = new GUIStyle(GUI.skin.label);

        public static Color Color
        {
            get { return GUI.color; }
            set { GUI.color = value; }
        }


        public static void DrawDot(Vector2 position, Color color)
        {
            Color = color;
            DrawDot(position);
        }

        public static void DrawDot(Vector2 position)
        {
            DrawBox(position, new Vector2(1, 1), true);
        }

        public static void DrawLine(Vector2 from, Vector2 to, Color color)
        {
            Color = color;
            DrawLine(from, to);
        }
        public static void DrawLine(Vector2 from, Vector2 to)
        {
            var angle = Vector2.SignedAngle(from, to);
            GUIUtility.RotateAroundPivot(angle, from);
            DrawBox(from, Vector2.right * (from - to).magnitude, false);
            GUIUtility.RotateAroundPivot(-angle, from);
        }

        public static void DrawBox(Vector2 position, Vector2 size, Color color, bool centered = true)
        {
            Color = color;
            DrawBox(position, size, centered);
        }
        public static void DrawBox(Vector2 position, Vector2 size, bool centered = true)
        {
            var upperLeft = centered ? position - size / 2f : position;
            GUI.DrawTexture(new Rect(position, size), Texture2D.whiteTexture, ScaleMode.StretchToFill);
        }

        public static void DrawString(Vector2 position, string label, Color color, bool centered = true)
        {
            Color = color;
            DrawString(position, label, centered);
        }
        public static void DrawString(Vector2 position, string label, bool centered = true)
        {
            var content = new GUIContent(label);
            var size = StringStyle.CalcSize(content);
            var upperLeft = centered ? position - size / 2f : position;
            GUI.Label(new Rect(upperLeft, size), content);
        }
    }
}
