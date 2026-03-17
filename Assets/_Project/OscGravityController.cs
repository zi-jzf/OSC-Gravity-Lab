using UnityEngine;
using OscJack;

public class OscGravityController : MonoBehaviour
{
    [Header("Gravity Power")]
    public float gravityMultiplier = 5.0f;

    [Header("Orientation (持ち方)")]
    [Tooltip("iPadを横向きで使う場合はチェックを入れる")]
    public bool isLandscape = true;

    [Header("Axis Mapping")]
    public bool applyX = true;
    public bool applyY = false; // 重力は基本的にY軸(下方向)には常に固定の重力をかけることが多いので、一旦false推奨
    public bool applyZ = true;

    [Header("Axis Invert (逆向きに動く場合はチェック)")]
    public bool invertX = false;
    public bool invertY = false;
    public bool invertZ = false;

    OscServer server;
    Vector3 targetGravity = new Vector3(0, -9.81f, 0);

    void Start()
    {
        server = new OscServer(9000);
        server.MessageDispatcher.AddCallback("/ZIGSIM/ipad/gravity", OnReceiveGravity);
    }

    void OnReceiveGravity(string address, OscDataHandle data)
    {
        float rawX = data.GetElementAsFloat(0);
        float rawY = data.GetElementAsFloat(1);
        float rawZ = data.GetElementAsFloat(2);

        // 【ここが90度回転の魔法】
        // 横向きモードなら、XとYのデータを入れ替える！
        float mappedX = isLandscape ? rawY : rawX;
        float mappedY = isLandscape ? rawX : rawY;
        float mappedZ = rawZ;

        // インスペクターの反転設定を適用
        float finalX = applyX ? (invertX ? -mappedX : mappedX) : 0f;
        float finalY = applyY ? (invertY ? -mappedY : mappedY) : 0f; // Y軸は基本の下向き重力を残しておく
        float finalZ = applyZ ? (invertZ ? -mappedZ : mappedZ) : 0f;

        // Unityの重力ベクトルに変換（Y軸とZ軸の意味合いをUnityの3D空間に合わせる）
        targetGravity = new Vector3(finalX, finalY, finalZ) * gravityMultiplier;
    }

    void Update()
    {
        Physics.gravity = Vector3.Lerp(Physics.gravity, targetGravity, 0.2f); // Lerpで少し動きを滑らか(ヌルッと)させました！
    }

    void OnDestroy()
    {
        server?.Dispose();
    }
}