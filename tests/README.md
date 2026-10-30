# Local IL2CPP checks

Requires .NET 10 SDK, the target game's generated BepInEx/interop assemblies and WebView2 runtime. From the repository root:

```powershell
dotnet build WebOverlay/WebOverlay.csproj -c Release
dotnet run --project tools/Probe/Probe.csproj -c Release -- channels
dotnet build tests/RuntimeChecks.csproj -c Release
```

Use `-p:SPTPath='<game>'` on builds to override the default 48484 installation. Install the production staged folder first, then temporarily copy **only** `tests/bin/Release/net10.0/WebOverlay.RuntimeChecks.dll` into `BepInEx/plugins/Anvil-WebOverlay`. Start server and client using an appropriate test account. The parent SPT-BE `scripts/README.md` documents local process helpers and log capture.

Expected BepInEx/LogOutput.log results: injected IL2CPP Update executes; a transparent-configured WebView2 page posts `ready` back on Unity's main thread. The upstream channels probe reports 12 passing messaging checks. Stop the client and remove WebOverlay.RuntimeChecks.dll afterward. These checks do not modify inventory or quests.

The runtime harness loads HTML but does not call Show: it verifies initialization/messaging, **not visible transparency**, overlay positioning, cursor ownership or raid rendering. Test those interactively in windowed/borderless mode. Keep logs/build outputs local; commit test source here.
