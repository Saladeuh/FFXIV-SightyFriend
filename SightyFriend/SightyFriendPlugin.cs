using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SigtyFriend;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.Game.Object;

namespace SightyFriend;

public sealed unsafe partial class SightyFriendPlugin : IDalamudPlugin
{
  public string Name => "Sighty Friend";
  private DalamudPluginInterface PluginInterface { get; init; }
  public IClientState clientState { get; private set; }
  public ICondition condition { get; set; }
  private ICommandManager CommandManager { get; init; }
  public Configuration Configuration { get; init; }
  public IFramework framework { get; set; }
  private IGameInteropProvider gameInteropProvider { get; }

  public SightyFriendPlugin(
    IClientState clientState,
      [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
      [RequiredVersion("1.0")] ICommandManager commandManager,
      ICondition condition,
      IFramework framework
,
      Configuration configuration,
      IGameInteropProvider gameInteropProvider)
  {
    PluginInterface = pluginInterface;
    this.clientState = clientState;
    this.condition = condition;
    CommandManager = commandManager;
    this.framework = framework;
    this.gameInteropProvider = gameInteropProvider;
    Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
    Configuration.Initialize(PluginInterface);
    framework.Update += OnFrameworkUpdate;
    Configuration = configuration;
    this._SetPositionHook = gameInteropProvider.HookFromAddress<SetPosition>(
   (nint)GameObject.Addresses.SetPosition.Value,
   DetourSetPosition);
    this._SetPositionHook.Enable();
  }

  public void Dispose()
  {
    this.framework.Update -= OnFrameworkUpdate;
    this._SetPositionHook.Disable();
  }
}
