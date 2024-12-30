using UnityEngine;

public class PositionTool : MonoBehaviour {
    public Transform targetTransform;
    void Update() {
        transform.position = targetTransform.position;
    }
}