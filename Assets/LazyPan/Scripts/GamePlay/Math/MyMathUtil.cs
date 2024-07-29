using UnityEngine;

namespace LazyPan {
    public class MyMathUtil {
        public static int ComputeRoot(float a, float b, float c, out float root1, out float root2) {
            var num = b * b - 4 * a * c;
            if (num < 0) {
                root1 = Mathf.Infinity;
                root2 = -root1;
                return 0;
            }

            root1 = (-b + Mathf.Sqrt(num)) / (2 * a);
            root2 = (-b - Mathf.Sqrt(num)) / (2 * a);
            return num > 0 ? 2 : 1;
        }

        #region 画线

        public static void CircleLineRenderer(LineRenderer lineRenderer, Vector3 center, float _radius, int numSegments) {
            if (lineRenderer.positionCount == 0) {
                lineRenderer.positionCount = numSegments + 1;
                float angleStep = 360f / numSegments;
                Vector3[] positions = new Vector3[numSegments + 1]; // 缓存位置数组
                for (int i = 0; i <= numSegments; i++) {
                    float angle = i * angleStep;
                    float x = Mathf.Sin(Mathf.Deg2Rad * angle) * _radius;
                    float z = Mathf.Cos(Mathf.Deg2Rad * angle) * _radius;
                    positions[i] = center + new Vector3(x, 0.5f, z);
                }

                lineRenderer.SetPositions(positions);
            }
        }

        public static void ClearCircleRenderer(LineRenderer lineRenderer) {
            lineRenderer.positionCount = 0;
        }

        #endregion
    }
}