using System;
using UnityEngine;
namespace WebOverlay;
public sealed class OverlayPump : MonoBehaviour
{
    private readonly WebOverlayPlugin owner;
    public OverlayPump(IntPtr pointer) : base(pointer) { owner = WebOverlayPlugin.Instance; }
    public void Update() => owner?.Tick();
    public void LateUpdate() => owner?.LateTick();
    public void OnDestroy() => owner?.Shutdown();
}
