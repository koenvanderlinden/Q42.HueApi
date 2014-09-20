using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Remote
{
  /// <summary>
  /// Partial HueRemoteClient, contains requests to the /config/ url
  /// </summary>
  public partial class HueRemoteClient
  {
    /// <summary>
    /// Get bridge info
    /// </summary>
    /// <returns></returns>
    public async Task<Bridge> GetBridgeAsync()
    {     
      HttpClient client = new HttpClient();

      string stringResult = await GetMessage(ApiBridgeBase);
      BridgeState jsonResult = DeserializeResult<BridgeState>(stringResult);

      return new Bridge(jsonResult);
    }

  }
}
