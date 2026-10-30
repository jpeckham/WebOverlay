using BepInEx;
using BepInEx.Unity.IL2CPP;
using UnityEngine;
using WebOverlay;

namespace WebOverlayRuntimeChecks;

[BepInPlugin("com.jpeckham.weboverlay.runtimechecks", "WebOverlay Runtime Checks", "1.0.0")]
[BepInDependency("com.anvil.weboverlay", "1.11.0")]
public class RuntimeChecks : BasePlugin
{
    internal static RuntimeChecks Instance;
    private IWebOverlay overlay;
    private bool started;
    private int mainThread;
    public override void Load() { Instance = this; AddComponent<CheckPump>(); }
    internal void Tick()
    {
        if (started) return;
        started = true;
        mainThread = Environment.CurrentManagedThreadId;
        Log.LogInfo("PASS: injected IL2CPP Update executed");
        overlay = WebOverlays.Create("SPT BE overlay check", new OverlayOptions { Transparent = true, DispatchOnMainThread = true });
        if (overlay == null) { Log.LogError("FAIL: overlay creation returned null"); return; }
        overlay.Failed += () => Log.LogError("FAIL: " + overlay.FailureMessage);
        overlay.MessageReceived += message => {
            if (message == "ready" && Environment.CurrentManagedThreadId == mainThread)
                Log.LogInfo("PASS: transparent WebView2 page and message dispatch on Unity thread");
            else Log.LogError("FAIL: unexpected message or thread");
            overlay.Dispose();
        };
        overlay.LoadHtml("<!doctype html><html><body style='background:transparent'><script>window.chrome.webview.postMessage('ready')</script></body></html>");
    }
}
public sealed class CheckPump : MonoBehaviour
{
    public CheckPump(IntPtr pointer) : base(pointer) { }
    public void Update() => RuntimeChecks.Instance.Tick();
}
