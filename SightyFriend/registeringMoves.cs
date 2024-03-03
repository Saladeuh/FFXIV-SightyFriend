using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game.Object;

namespace SightyFriend;
public unsafe sealed partial class SightyFriendPlugin
{
  public float lastRotation { get; private set; }
  public Vector3 lastPosion { get; private set; }

  public void OnFrameworkUpdate(IFramework _)
  {
    if (clientState.LocalPlayer != null)
    {
      var rotation = this.clientState.LocalPlayer.Rotation;
      if (rotation != lastRotation && !condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.InFlight])
      {

        lastRotation = rotation;
      }
    }
  }
  private delegate void SetPosition(GameObject* self, float x, float y, float z);
  private readonly Hook<SetPosition>? _SetPositionHook;

  private void DetourSetPosition(GameObject* self, float x, float y, float z)
  {
    try
    {
      if (self->GetObjectID() == ((uint)clientState.LocalPlayer.ObjectId))
      {
        lastPosion = clientState.LocalPlayer.Position;
      }
    }
    catch (Exception _) { }
  }
}
