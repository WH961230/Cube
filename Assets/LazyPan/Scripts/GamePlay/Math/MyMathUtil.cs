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

        public static void CircleLineRenderer(LineRenderer lineRenderer, Vector3 center, float _radius, int numSegments, float roundDir) {
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
            } else {
                // 设置范围旋转
                float deltaRotation = (_radius / 10f)  * Time.deltaTime; // 当前帧旋转的角度

                Vector3[] positions = new Vector3[lineRenderer.positionCount];
                lineRenderer.GetPositions(positions); // 获取当前的所有点位置

                // 创建一个旋转四元数，绕Y轴旋转
                Quaternion rotation = Quaternion.Euler(0, roundDir * deltaRotation, 0); // 绕Y轴旋转

                // 旋转每个点
                for (int i = 0; i < positions.Length; i++) {
                    positions[i] = rotation * (positions[i] - center) + center; // 旋转点并保持相对位置
                }

                lineRenderer.SetPositions(positions); // 更新LineRenderer的位置
            }
        }

        public static void ClearCircleRenderer(LineRenderer lineRenderer) {
            lineRenderer.positionCount = 0;
        }

        #endregion
    }
}