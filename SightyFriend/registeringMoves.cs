using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using SightyFriend.API;

namespace SightyFriend;
public sealed unsafe partial class SightyFriendPlugin
{
  public float lastRotation { get; private set; }
  public Vector3 lastPosion { get; private set; }
  private List<WalkableSegment> segments = [];
  // when rotation change: end current segment
  public void OnFrameworkUpdate(IFramework _)
  {
    if (clientState.LocalPlayer != null)
    {
      var rotation = this.clientState.LocalPlayer.Rotation;
      if (rotation != lastRotation && !condition[ConditionFlag.InFlight])
      {
        if (currentWalkingSegment != null)
        {
          EndSegment();
        }
        lastRotation = rotation;
      }
    }
  }

  private void EndSegment()
  {
    currentWalkingSegment.EndPoint = clientState.LocalPlayer.Position;
    segments.Add(currentWalkingSegment);
    Chat.Print(segments.Count.ToString());
    currentWalkingSegment = null;
  }

  private delegate void SetPosition(GameObject* self, float x, float y, float z);
  private readonly Hook<SetPosition>? _SetPositionHook;

  private void DetourSetPosition(GameObject* self, float x, float y, float z)
  {
    try
    {
      lastPosion = clientState.LocalPlayer!.Position;
      // if player start move, set start of a new segment if don't exist
      if (clientState.LocalPlayer!=null && self->GetObjectID() == ((uint)clientState.LocalPlayer!.ObjectId!) && currentWalkingSegment == null && !condition[ConditionFlag.InFlight])
      {
        currentWalkingSegment = new WalkableSegment();
        currentWalkingSegment.map = "api/maps/1" + clientState.TerritoryType;

        currentWalkingSegment.FirstPoint = lastPosion;
      }
    }
    catch (Exception) { }
    this._SetPositionHook.Original(self, x, y, z);
  }
}
