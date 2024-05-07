using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using System.Numerics;
using SightyFriend.API;

namespace SightyFriend;

public sealed unsafe partial class SightyFriendPlugin : IDalamudPlugin
{
  public string Name => "Sighty Friend";
  private ApiClient apiClient { get; set; }
  private DalamudPluginInterface PluginInterface { get; init; }
  public IChatGui Chat { get; }
  public IClientState clientState { get; private set; }
  public ICondition condition { get; set; }
  private ICommandManager CommandManager { get; init; }
  public IFramework framework { get; set; }
  private IGameInteropProvider gameInteropProvider { get; }
  private WalkableSegment? currentWalkingSegment = null;
  public SightyFriendPlugin(
    IChatGui chat,
    IClientState clientState,
      [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
      [RequiredVersion("1.0")] ICommandManager commandManager,
      ICondition condition,
      IFramework framework,
      IGameInteropProvider gameInteropProvider)
  {
    PluginInterface = pluginInterface;
    this.Chat = chat;
    this.clientState = clientState;
    this.condition = condition;
    CommandManager = commandManager;
    this.framework = framework;
    this.gameInteropProvider = gameInteropProvider;
    framework.Update += OnFrameworkUpdate;
    apiClient = new(chat);
    this._SetPositionHook = gameInteropProvider.HookFromAddress<SetPosition>(
   (nint)GameObject.Addresses.SetPosition.Value,
   DetourSetPosition);
    this._SetPositionHook.Enable();
    clientState.TerritoryChanged += SendSegments;
    clientState.Logout += onLogout;
  }

  private void onLogout()
  {
    SendSegments();
  }

  private void SendSegments(ushort obj = 0)
  {
    apiClient.PostSegments(segments);
    segments.Clear();
  }

  public void Dispose()
  {
    this.framework.Update -= OnFrameworkUpdate;
    this._SetPositionHook?.Disable();
    clientState.TerritoryChanged -= SendSegments;
    clientState.Logout -= onLogout;
  }
}
