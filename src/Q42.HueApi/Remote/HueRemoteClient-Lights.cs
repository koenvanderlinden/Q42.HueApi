using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Q42.HueApi.Extensions;
using Newtonsoft.Json;
using Q42.HueApi.Models.Groups;
using System.Dynamic;
using Q42.HueApi.Models;

namespace Q42.HueApi.Remote
{
  /// <summary>
  /// Partial HueClient, contains requests to the /lights/ url
  /// </summary>
  public partial class HueRemoteClient
  {
    /// <summary>
    /// Asynchronously gets all lights registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="Light"/>s registered with the bridge.</returns>
    public async Task<IEnumerable<Light>> GetLightsAsync()
    {      
      Bridge bridge = await GetBridgeAsync().ConfigureAwait(false);
      return bridge.Lights;
    }
    
    private dynamic GetLightCommand(LightCommand command, string lightId)
    {
      string jsonCommand = JsonConvert.SerializeObject(command, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      dynamic clipCommand = new ExpandoObject();
      clipCommand.clipCommand = new
      {
        //change state of light
        url = string.Format("/api/0/lights/{0}/state", lightId),
        method = "PUT",
        body = jsonCommand
      };

      return clipCommand;
    }

    /// <summary>
    /// Send a lightCommand to a list of lights
    /// </summary>
    /// <param name="command"></param>
    /// <param name="lightList">if null, send command to all lights</param>
    /// <returns></returns>
    public async Task<HueRemoteResult> SendCommandAsync(LightCommand command, IEnumerable<string> lightList = null)
    {
      if (command == null)
        throw new ArgumentNullException("command");

      if (lightList == null || !lightList.Any())
      {
        //Group 0 always contains all the lights
        throw new NotImplementedException("Not yet implemented");
        //return await SendGroupCommandAsync(command).ConfigureAwait(false);
      }
      else
      {
        HueRemoteResult result = new HueRemoteResult(); 
                
        await lightList.ForEachAsync(_parallelRequests, async (lightId) =>
        {
          HttpClient client = new HttpClient();

          var clipCommand = GetLightCommand(command, lightId);
          HueRemoteResult hueRemoteResult = await PostMessage(clipCommand);
          string test = string.Format("result: {0}", hueRemoteResult.Message);          
        }).ConfigureAwait(false);

        return result;
      }
    }   
  }
}
