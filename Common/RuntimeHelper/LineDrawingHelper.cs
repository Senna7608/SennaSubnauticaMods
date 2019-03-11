using UnityEngine;

namespace Common.RuntimeHelper
{
    public static class LineDrawingHelper
    {
        public static void DrawCircle(this GameObject container, float radius, int angle)
        {
            LineRenderer lineRenderer = container.GetComponent<LineRenderer>();

            lineRenderer.positionCount = angle + 1;

            var points = new Vector3[lineRenderer.positionCount];

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                var rad = Mathf.Deg2Rad * (i * angle / angle);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
            }

            lineRenderer.SetPositions(points);

            container.SetActive(true);
        }

        public static void DrawLine(this GameObject container, Vector3 fromPos, Vector3 toPos)
        {
            LineRenderer lineRenderer = container.GetComponent<LineRenderer>();

            lineRenderer.positionCount = 2;

            lineRenderer.SetPosition(0, fromPos);
            lineRenderer.SetPosition(1, toPos);

            container.SetActive(true);
        }
    }
}
