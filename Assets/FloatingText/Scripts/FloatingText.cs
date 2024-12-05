using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class FloatingText : MonoBehaviour {
    public TextMesh textMesh;
    public float LifeTime = 1;
    public bool FadeEnd = false;
    public Color TextColor = Color.white;
    public bool LockLookCameraRotate = true;
    public Transform camera;

    private float alpha = 1;
    private float timeTemp = 0;

    private void Awake() {
        textMesh = this.GetComponent<TextMesh>();
    }

    void Start() {
        timeTemp = Time.time;
        GameObject.Destroy(this.gameObject, LifeTime);
        if (camera == null) {
            camera = LazyPan.Cond.Instance.Get<Transform>(LazyPan.Cond.Instance.GetCameraEntity(), LazyPan.Label.CAMERA);
        }
    }

    public void SetText(string text) {
        if (textMesh)
            textMesh.text = text;
    }

    public void SetColor(Color color) {
        if (textMesh) {
            TextColor = color;
        }
    }

    void Update() {
        if (FadeEnd) {
            if (Time.time >= ((timeTemp + LifeTime) - 1)) {
                alpha = 1.0f - (Time.time - ((timeTemp + LifeTime) - 1));
            }
        }

        textMesh.color = new Color(TextColor.r, TextColor.g, TextColor.b, alpha);

        if (camera != null) {
            this.transform.localScale = Vector3.one;
            if (!LockLookCameraRotate) {
                // 计算物体到相机的方向
                Vector3 direction = (this.transform.position - camera.position).normalized;

                // 计算物体的旋转，使其面朝相机
                Quaternion rota = Quaternion.LookRotation(direction);

                // 将物体的X轴与相机的X轴对齐
                // 获取当前物体的旋转，调整其X轴为相机的X轴
                Vector3 currentRotation = rota.eulerAngles;
                this.transform.rotation = Quaternion.Euler(currentRotation.x, camera.rotation.eulerAngles.y, currentRotation.z);
            }
        }
    }
}