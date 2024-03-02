using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SigtyFriend;

namespace SightyFriend;

public sealed class SightyFriendPlugin : IDalamudPlugin
{
    public string Name => "Sighty Friend";
    private DalamudPluginInterface PluginInterface { get; init; }
    private ICommandManager CommandManager { get; init; }
    public Configuration Configuration { get; init; }
    public SightyFriendPlugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
        [RequiredVersion("1.0")] ICommandManager commandManager)
    {
        PluginInterface = pluginInterface;
        CommandManager = commandManager;

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);

        CommandManager.AddHandler("/test", new CommandInfo(OnCommand)
        {
            HelpMessage = "A useful message to display in /xlhelp"
        });

    }

    public void Dispose()
    {
        CommandManager.RemoveHandler("/test");
    }

    private void OnCommand(string command, string args)
    {
    }
}
