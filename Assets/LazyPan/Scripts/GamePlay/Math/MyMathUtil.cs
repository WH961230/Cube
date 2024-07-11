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
    }
}