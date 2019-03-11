using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Common.RuntimeHelper
{
    public static class ColliderHelper
    {

        public static void DrawSphereColliderBounds(ref List<GameObject> lineContainers, SphereCollider sc)
        {
            int angle = 360;

            for (int i = 0; i < lineContainers.Count; i++)
            {
                lineContainers[i].transform.localPosition = sc.center;
            }

            lineContainers[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
            lineContainers[1].transform.localRotation = Quaternion.Euler(90f, 0, 0);
            lineContainers[2].transform.localRotation = Quaternion.Euler(0, 90f, 0);
            lineContainers[3].transform.localRotation = Quaternion.Euler(0, 0, 90f);

            lineContainers[0].DrawCircle(sc.radius, angle);
            lineContainers[1].DrawCircle(sc.radius, angle);
            lineContainers[2].DrawCircle(sc.radius, angle);
            lineContainers[3].DrawCircle(sc.radius, angle);
        }


        public static void DrawCapsuleColliderBounds(ref List<GameObject> lineContainers, CapsuleCollider cc)
        {
            int angleX = 360;
            int angleY = 360;
            int angleZ = 360;

            for (int i = 0; i < lineContainers.Count; i++)
            {
                lineContainers[i].transform.localPosition = cc.center;
            }

            if (cc.radius * 2.0f < cc.height)
            {
                float modify = (cc.height * 0.5f) - cc.radius;

                switch (cc.direction)
                {
                    case 0:
                        angleY = 180;
                        angleZ = 180;
                        Vector3 axisXpositive = new Vector3(cc.center.x + modify, cc.center.y, cc.center.z);
                        Vector3 axisXnegative = new Vector3(cc.center.x - modify, cc.center.y, cc.center.z);
                        lineContainers[0].transform.localPosition = axisXpositive;
                        lineContainers[1].transform.localPosition = axisXpositive;
                        lineContainers[2].transform.localPosition = axisXpositive;

                        lineContainers[0].transform.localRotation = Quaternion.Euler(0, 0, 90f);
                        lineContainers[1].transform.localRotation = Quaternion.Euler(-90f, -90f, 90f);
                        lineContainers[2].transform.localRotation = Quaternion.Euler(0, 0, 0);

                        lineContainers[3].transform.localPosition = axisXnegative;
                        lineContainers[4].transform.localPosition = axisXnegative;
                        lineContainers[5].transform.localPosition = axisXnegative;

                        lineContainers[3].transform.localRotation = Quaternion.Euler(0, 0, 90f);
                        lineContainers[4].transform.localRotation = Quaternion.Euler(0, 180f, 0);
                        lineContainers[5].transform.localRotation = Quaternion.Euler(90f, 180f, 0);

                        float distance = Vector3.Distance(axisXpositive, axisXnegative);

                        lineContainers[6].DrawLine(new Vector3(distance * 0.5f, -cc.radius, 0), new Vector3(distance * -0.5f, -cc.radius, 0));
                        lineContainers[7].DrawLine(new Vector3(distance * 0.5f, +cc.radius, 0), new Vector3(distance * -0.5f, +cc.radius, 0));
                        lineContainers[8].DrawLine(new Vector3(distance * 0.5f, 0, -cc.radius), new Vector3(distance * -0.5f, 0, -cc.radius));
                        lineContainers[9].DrawLine(new Vector3(distance * 0.5f, 0, +cc.radius), new Vector3(distance * -0.5f, 0, +cc.radius));

                        break;

                    case 1:
                        angleX = 180;
                        angleZ = 180;
                        Vector3 axisYpositive = new Vector3(cc.center.x, cc.center.y + modify, cc.center.z);
                        Vector3 axisYnegative = new Vector3(cc.center.x, cc.center.y - modify, cc.center.z);
                        lineContainers[0].transform.localPosition = axisYpositive;
                        lineContainers[1].transform.localPosition = axisYpositive;
                        lineContainers[2].transform.localPosition = axisYpositive;

                        lineContainers[0].transform.localRotation = Quaternion.Euler(180f, -90f, -90f);
                        lineContainers[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        lineContainers[2].transform.localRotation = Quaternion.Euler(0, 0, 90);

                        lineContainers[3].transform.localPosition = axisYnegative;
                        lineContainers[4].transform.localPosition = axisYnegative;
                        lineContainers[5].transform.localPosition = axisYnegative;

                        lineContainers[3].transform.localRotation = Quaternion.Euler(180f, 90f, 90f);
                        lineContainers[4].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        lineContainers[5].transform.localRotation = Quaternion.Euler(0, 0, -90);

                        distance = Vector3.Distance(axisYpositive, axisYnegative);

                        lineContainers[6].DrawLine(new Vector3(-cc.radius, distance * 0.5f, 0), new Vector3(-cc.radius, distance * -0.5f, 0));
                        lineContainers[7].DrawLine(new Vector3(+cc.radius, distance * 0.5f, 0), new Vector3(+cc.radius, distance * -0.5f, 0));
                        lineContainers[8].DrawLine(new Vector3(0, distance * 0.5f, -cc.radius), new Vector3(0, distance * -0.5f, -cc.radius));
                        lineContainers[9].DrawLine(new Vector3(0, distance * 0.5f, +cc.radius), new Vector3(0, distance * -0.5f, +cc.radius));

                        break;

                    case 2:
                        angleX = 180;
                        angleY = 180;
                        Vector3 axisZpositive = new Vector3(cc.center.x, cc.center.y, cc.center.z + modify);
                        Vector3 axisZnegative = new Vector3(cc.center.x, cc.center.y, cc.center.z - modify);
                        lineContainers[0].transform.localPosition = axisZpositive;
                        lineContainers[1].transform.localPosition = axisZpositive;
                        lineContainers[2].transform.localPosition = axisZpositive;

                        lineContainers[0].transform.localRotation = Quaternion.Euler(90f, 0, 90f);
                        lineContainers[1].transform.localRotation = Quaternion.Euler(0, 90f, 180f);
                        lineContainers[2].transform.localRotation = Quaternion.Euler(90f, 0, 0);

                        lineContainers[3].transform.localPosition = axisZnegative;
                        lineContainers[4].transform.localPosition = axisZnegative;
                        lineContainers[5].transform.localPosition = axisZnegative;

                        lineContainers[3].transform.localRotation = Quaternion.Euler(-90f, 0, 90f);
                        lineContainers[4].transform.localRotation = Quaternion.Euler(0, 90f, 0);
                        lineContainers[5].transform.localRotation = Quaternion.Euler(90f, 0, 180f);
                        
                        distance = Vector3.Distance(axisZpositive, axisZnegative);

                        lineContainers[6].DrawLine(new Vector3(-cc.radius, 0, distance * 0.5f), new Vector3(-cc.radius, 0, distance * -0.5f));
                        lineContainers[7].DrawLine(new Vector3(+cc.radius, 0, distance * 0.5f), new Vector3(+cc.radius, 0, distance * -0.5f));
                        lineContainers[8].DrawLine(new Vector3(0, -cc.radius, distance * 0.5f), new Vector3(0, -cc.radius, distance * -0.5f));
                        lineContainers[9].DrawLine(new Vector3(0, +cc.radius, distance * 0.5f), new Vector3(0, +cc.radius, distance * -0.5f));

                        break;
                }

                lineContainers[3].DrawCircle(cc.radius, angleX);
                lineContainers[4].DrawCircle(cc.radius, angleY);
                lineContainers[5].DrawCircle(cc.radius, angleZ);
            }
            else
            {
                for (int i = 3; i < 10; i++)
                {
                    lineContainers[i].SetActive(false);
                }                

                lineContainers[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
                lineContainers[1].transform.localRotation = Quaternion.Euler(90f, 0, 0);
                lineContainers[2].transform.localRotation = Quaternion.Euler(0, 0, 90f);
            }

            lineContainers[0].DrawCircle(cc.radius, angleX);
            lineContainers[1].DrawCircle(cc.radius, angleY);
            lineContainers[2].DrawCircle(cc.radius, angleZ);
        }

        public static void DrawBoxColliderBounds(ref List<GameObject> lineContainers, BoxCollider bc)
        {
            Vector3[] vertices = GetBoxColliderLocalVertexPositions(bc.center, bc.size);

            for (int i = 0; i < lineContainers.Count; i++)
            {
                lineContainers[i].transform.localPosition = bc.center;
            }

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

        public static Vector3[] GetBoxColliderLocalVertexPositions(Vector3 center, Vector3 size)
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

        public static void ClearLineContainers(ref List<GameObject> lineContainers)
        {
            foreach (GameObject container in lineContainers)
            {
                Object.DestroyImmediate(container);
            }

            lineContainers.Clear();
        }

        public static void CreateSCLineContainers(ref List<GameObject> lineContainers)
        {
            ClearLineContainers(ref lineContainers);

            lineContainers.Add(new GameObject { name = "ROM_cc_circle_0" });
            lineContainers.Add(new GameObject { name = "ROM_cc_circle_1" });
            lineContainers.Add(new GameObject { name = "ROM_cc_circle_2" });
            lineContainers.Add(new GameObject { name = "ROM_cc_circle_3" });            
        }

        public static void CreateCCLineContainers(ref List<GameObject> lineContainers)
        {
            ClearLineContainers(ref lineContainers);

            lineContainers.Add(new GameObject { name = "ROM_cc_circle_0" });
            lineContainers.Add(new GameObject { name = "ROM_cc_circle_1" });
            lineContainers.Add(new GameObject { name = "ROM_cc_circle_2" });
            lineContainers.Add(new GameObject { name = "ROM_cc_circle_3" });
            lineContainers.Add(new GameObject { name = "ROM_cc_circle_4" });
            lineContainers.Add(new GameObject { name = "ROM_cc_circle_5" });
            lineContainers.Add(new GameObject { name = "ROM_cc_line_0" });
            lineContainers.Add(new GameObject { name = "ROM_cc_line_1" });
            lineContainers.Add(new GameObject { name = "ROM_cc_line_2" });
            lineContainers.Add(new GameObject { name = "ROM_cc_line_3" });
        }

        public static void CreateBCLineContainers(ref List<GameObject> lineContainers)
        {
            ClearLineContainers(ref lineContainers);

            lineContainers.Add(new GameObject { name = "ROM_bc_line_0" });
            lineContainers.Add(new GameObject { name = "ROM_bc_line_1" });
            lineContainers.Add(new GameObject { name = "ROM_bc_line_2" });
            lineContainers.Add(new GameObject { name = "ROM_bc_line_3" });
            lineContainers.Add(new GameObject { name = "ROM_bc_line_4" });
            lineContainers.Add(new GameObject { name = "ROM_bc_line_5" });
            lineContainers.Add(new GameObject { name = "ROM_bc_line_6" });
            lineContainers.Add(new GameObject { name = "ROM_bc_line_7" });
            lineContainers.Add(new GameObject { name = "ROM_bc_line_8" });
            lineContainers.Add(new GameObject { name = "ROM_bc_line_9" });
            lineContainers.Add(new GameObject { name = "ROM_bc_line_10" });
            lineContainers.Add(new GameObject { name = "ROM_bc_line_11" });
        }

        public static void AddLineRendererToContainers(ref List<GameObject> lineContainers, float lineWidth, Color lineColor)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");

            Material lineMaterial = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            lineMaterial.SetInt(ShaderPropertyID._SrcBlend, 5);
            lineMaterial.SetInt(ShaderPropertyID._DstBlend, 10);

            foreach (GameObject container in lineContainers)
            {
                LineRenderer lineRenderer = container.AddOrGetComponent<LineRenderer>();

                lineRenderer.material = lineMaterial;
                lineRenderer.useWorldSpace = false;
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
        }

        public static void DisableContainers(ref List<GameObject> lineContainers)
        {
            foreach (GameObject container in lineContainers)
            {
                container.SetActive(false);
            }
        }

    }
}
