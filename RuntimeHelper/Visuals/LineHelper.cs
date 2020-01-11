using Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace RuntimeHelper.Visuals
{
    public static class LineHelper
    {
        public const float MinLineWidth = 0.004f;

        public static void SetLineWidth(this List<GameObject> lineContainers, float maxSizeValue)
        {
            float lineWidth = Mathf.Clamp(maxSizeValue / 500f, MinLineWidth, 0.1f);

            foreach (GameObject container in lineContainers)
            {
                LineRenderer lineRenderer = container.GetComponent<LineRenderer>();

                lineRenderer.startWidth = lineWidth;
                lineRenderer.endWidth = lineWidth;
            }
        }

        public static void AddLineRendererToContainer(this GameObject linecontainer, float lineWidth, Color lineColor, bool useWorldSpace)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");

            Material lineMaterial = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            lineMaterial.SetInt(ShaderPropertyID._SrcBlend, 5);
            lineMaterial.SetInt(ShaderPropertyID._DstBlend, 10);

            LineRenderer lineRenderer = linecontainer.GetOrAddComponent<LineRenderer>();

            lineRenderer.material = lineMaterial;
            lineRenderer.useWorldSpace = useWorldSpace;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;
            lineRenderer.receiveShadows = false;
            lineRenderer.loop = false;
            lineRenderer.textureMode = LineTextureMode.Stretch;
            lineRenderer.alignment = LineAlignment.View;
            lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }

        public static void AddLineRendererToContainers(this List<GameObject> lineContainers, float lineWidth, Color lineColor, bool useWorldSpace)
        {
            foreach (GameObject lineContainer in lineContainers)
            {
                lineContainer.AddLineRendererToContainer(lineWidth, lineColor, useWorldSpace);
            }
        }        

        public static void DrawLine(this GameObject lineContainer, Vector3 fromPosition, Vector3 toPosition)
        {
            LineRenderer lineRenderer = lineContainer.GetComponent<LineRenderer>();            

            lineRenderer.positionCount = 2;            

            lineRenderer.SetPosition(0, fromPosition);
            lineRenderer.SetPosition(1, toPosition);
        }

        public static void DrawRectangle(this List<GameObject> lineContainers, RectTransform rectTransform)
        {
            LineRenderer lineRenderer = lineContainers[0].GetComponent<LineRenderer>();

            Vector3[] vertices = new Vector3[4];

            rectTransform.GetLocalCorners(vertices);

            lineRenderer.positionCount = 4;
            lineRenderer.loop = true;

            for (int i = 0; i < 4; i++)
            {                
                lineRenderer.SetPosition(i, vertices[i]);
            }            
        }

        public static void DrawBox(this List<GameObject> lineContainers, Vector3 center, Vector3 size)
        {
            Vector3[] vertices = GetBoxLocalCorners(center, size);

            lineContainers[0].DrawLine(vertices[0], vertices[1]);
            lineContainers[1].DrawLine(vertices[0], vertices[3]);
            lineContainers[2].DrawLine(vertices[0], vertices[4]);

            lineContainers[3].DrawLine(vertices[2], vertices[1]);
            lineContainers[4].DrawLine(vertices[2], vertices[3]);
            lineContainers[5].DrawLine(vertices[2], vertices[6]);

            lineContainers[6].DrawLine(vertices[5], vertices[1]);
            lineContainers[7].DrawLine(vertices[5], vertices[4]);
            lineContainers[8].DrawLine(vertices[5], vertices[6]);

            lineContainers[9].DrawLine(vertices[7], vertices[3]);
            lineContainers[10].DrawLine(vertices[7], vertices[4]);
            lineContainers[11].DrawLine(vertices[7], vertices[6]);
        }

        public static void DrawCircle(this GameObject lineContainer, float radius, int angle)
        { 
            LineRenderer lineRenderer = lineContainer.GetComponent<LineRenderer>();            

            lineRenderer.positionCount = angle + 1;

            var points = new Vector3[lineRenderer.positionCount];

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                var rad = Mathf.Deg2Rad * (i * angle / angle);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
            }

            lineRenderer.SetPositions(points);            
        }
        
        private static Vector3[] GetBoxLocalCorners(Vector3 center, Vector3 size)
        {
            Vector3[] vertices = new Vector3[8];

            vertices[0] = center + new Vector3(-size.x, -size.y, -size.z) * 0.5f;
            vertices[1] = center + new Vector3(size.x, -size.y, -size.z) * 0.5f;
            vertices[2] = center + new Vector3(size.x, -size.y, size.z) * 0.5f;
            vertices[3] = center + new Vector3(-size.x, -size.y, size.z) * 0.5f;
            vertices[4] = center + new Vector3(-size.x, size.y, -size.z) * 0.5f;
            vertices[5] = center + new Vector3(size.x, size.y, -size.z) * 0.5f;
            vertices[6] = center + new Vector3(size.x, size.y, size.z) * 0.5f;
            vertices[7] = center + new Vector3(-size.x, size.y, size.z) * 0.5f;

            return vertices;
        }

        public static Vector3 CompensateSizefromScale(Vector3 size, Vector3 scale)
        {
            float diffX = size.x / scale.x;
            float diffY = size.y / scale.y;
            float diffZ = size.z / scale.z;

            return new Vector3(size.x * diffX, size.y * diffY, size.z * diffZ);
        }
    }
}
