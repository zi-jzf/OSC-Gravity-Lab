using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;
using OscJack;

[VFXBinder("ZIG SIM/Gravity")] // テクニカルアーティスト主導設計！
public class ZigSimGravityBinder : VFXBinderBase
{
    [VFXPropertyBinding("System.Vector3")]
    public ExposedProperty gravityProperty = "OSCGravity";

    //風や修復など、全てをコントロールするための「大元の割合（0.0〜1.0）」
    [VFXPropertyBinding("System.Single")]
    public ExposedProperty collapseRatioProperty = "CollapseRatio";

    [Header("OSC Settings")]
    public int port = 9000;
    public string oscAddress = "/ZIGSIM/ipad/gravity";

    [Header("Physics Setting")]
    public float gravityMultiplier = 30.0f;
    public bool isLandscape = true;

    [Header("Tilt Tuning (角度の閾値)")]
    public float neutralThreshold = 0.1f;
    public float collapseThreshold = 1.0f;

    [Header("Axis Mapping")]
    public bool applyX = true;
    public bool applyZ = true;
    public bool invertX = false;
    public bool invertZ = false;

    OscServer server;
    float currentRawX = 0f;
    float currentRawZ = 0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        server = OscMaster.GetSharedServer(port);
        server.MessageDispatcher.AddCallback(oscAddress, OnReceiveGravity);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (server != null) server.MessageDispatcher.RemoveCallback(oscAddress, OnReceiveGravity);
    }

    void OnReceiveGravity(string address, OscDataHandle data)
    {
        float rawX = data.GetElementAsFloat(0);
        float rawY = data.GetElementAsFloat(1);
        float rawZ = data.GetElementAsFloat(2);

        currentRawX = isLandscape ? rawY : rawX;
        currentRawZ = rawZ;
    }

    public override bool IsValid(VisualEffect component)
    {
        return component.HasVector3(gravityProperty) && component.HasFloat(collapseRatioProperty);
    }

    public override void UpdateBinding(VisualEffect component)
    {
        float finalRawX = applyX ? (invertX ? -currentRawX : currentRawX) : 0f;
        float finalRawZ = applyZ ? (invertZ ? -currentRawZ : currentRawZ) : 0f;

        float currentTiltMagnitude = new Vector2(finalRawX, finalRawZ).magnitude;

        // C#の仕事は「崩壊率(0.0〜1.0)」を計算することだけ
        float collapseRatio = Mathf.InverseLerp(neutralThreshold, collapseThreshold, currentTiltMagnitude);

        float finalY = -collapseRatio;
        Vector3 targetGravity = new Vector3(finalRawX, finalY, finalRawZ) * (collapseRatio * gravityMultiplier);

        // VFX Graphへ送信
        component.SetVector3(gravityProperty, targetGravity);
        component.SetFloat(collapseRatioProperty, collapseRatio);
    }
}