using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Plugin.Services;
using Newtonsoft.Json;
namespace SightyFriend.API;

class ApiClient
{
  private const string POST_SEGMENT_URI = "api/walkable_segments";
  private const string GET_MAP_URI = "api/maps";
  public IChatGui Chat { get; }

  private HttpClient HttpClient;
  public ApiClient(IChatGui chat)
  {
    this.Chat = chat;
    this.HttpClient = new()
    {
      BaseAddress = new Uri("http://ffxiv-sighty-friends.saladecorp.fr/"),
    };
  }
  public List<Map>? Maps { get; set; }
  public List<WalkableSegment>? CurrentMapSegments { get; private set; }

  public async Task RetrieveMaps()
  {
    Task.Run(async () =>
    {
      try
      {
        Maps = await this.HttpClient.GetFromJsonAsync<List<Map>>(GET_MAP_URI);
      }
      catch (Exception _) { Maps = null; }
    }
    );
  }
  public async Task RetrieveSegmentFromMap(int mapId)
  {
    Task.Run(async () =>
    {
      try
      {
        CurrentMapSegments = await this.HttpClient.GetFromJsonAsync<List<WalkableSegment>>($"api/maps/{mapId}/WalkableSegments");
      }
      catch (Exception _) { CurrentMapSegments = null; }
    }
  );
  }
  public async Task PostSegment(WalkableSegment walkableSegment)
  {
    Task.Run(async () =>
    {
      try
      {
        string json = JsonConvert.SerializeObject(walkableSegment);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //Chat.Print("sending...");
        HttpResponseMessage response = await this.HttpClient.PostAsync(POST_SEGMENT_URI, content);
        //Chat.Print(await response.Content.ReadAsStringAsync());
        return response.IsSuccessStatusCode;
      }
      catch (Exception e) {
        Chat.PrintError($"SightyFriend error: {e.Message}");
        return false; }
    }
  );
  }
  public async Task PostSegments(List<WalkableSegment> walkableSegments)
  {
    foreach (WalkableSegment segment in walkableSegments)
    {
      PostSegment(segment);
    }
    Chat.Print($"Sighty Friend: {walkableSegments.Count()} segments sent to database");
  }
}
