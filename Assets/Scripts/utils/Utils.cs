using UnityEngine;

namespace utils {
    public static class Utils {
        public static Vector3 GetRandomDirection() {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
        }
    }
}