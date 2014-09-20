using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Extensions;
using Q42.HueApi.Interfaces;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using Q42.HueApi.Models.Groups;
using Q42.HueApi.Models;
using Newtonsoft.Json.Serialization;

namespace Q42.HueApi.Remote
{
  /// <summary>
  /// Responsible for communicating with the bridge via Meethue portal
  /// </summary>
  public partial class HueRemoteClient : IHueRemoteClient
  {
    private readonly string _apiEndPoint = "https://www.meethue.com/api/v3";        
    private readonly int _parallelRequests = 5;

    private readonly string _accessToken;

    public string ApiBridgeBase
    {
      get
      {
        // /api/v3/bridge/?token=
        return string.Format("{0}/bridge/?token={1}", _apiEndPoint, _accessToken);
      }
    }

    public string ApiMessageBase
    {
      get
      {
        // /api/v3/bridge/sendmessage?token=
        return string.Format("{0}/bridge/sendmessage?token={1}", _apiEndPoint, _accessToken);
      }
    }

    /// <summary>
    /// Initialize with remote client using accesstoken
    /// </summary>
    /// <param name="ip"></param>
    public HueRemoteClient(string accessToken)
    {
      if (accessToken == null)
        throw new ArgumentNullException("accessToken");

      _accessToken = accessToken;

      //IsInitialized = true;
    }   

    ///// <summary>
    ///// Check if the HueClient is initialized
    ///// </summary>
    //private void CheckInitialized()
    //{
    //  if (!IsInitialized)
    //    throw new InvalidOperationException("HueClient is not initialized. First call RegisterAsync or Initialize.");
    //}

    /// <summary>
    /// Deserialization helper that can also check for errors
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    private static T DeserializeResult<T>(string json) where T : class
    {
      try
      {
        T objResult = JsonConvert.DeserializeObject<T>(json);

        return objResult;

      }
      catch (Exception ex)
      {
        var defaultResult = DeserializeDefaultHueResult(json);

        //We expect an actual object, it was unsuccesful, show error why
        if (defaultResult.HasErrors())
          throw new Exception(defaultResult.Errors.First().Error.Description);
      }

      return null;
    }


    /// <summary>
    /// Checks if the json contains errors
    /// </summary>
    /// <param name="json"></param>
    private static HueResults DeserializeDefaultHueResult(string json)
    {
      HueResults result = null;

      try
      {
        result = JsonConvert.DeserializeObject<HueResults>(json);
      }
      catch (JsonSerializationException ex)
      {
        //Ignore JsonSerializationException
      }

      return result;
    }

    private async Task<HueRemoteResult> PostMessage(dynamic command)
    {
      string jsonMessage = JsonConvert.SerializeObject(command);

      HttpClient client = new HttpClient();
      HttpContent content = new StringContent("clipmessage=" + jsonMessage, Encoding.UTF8, "application/x-www-form-urlencoded");

      var result = await client.PostAsync(ApiMessageBase, content).ConfigureAwait(false);
      string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return JsonConvert.DeserializeObject<HueRemoteResult>(jsonResult);      
    }

    private async Task<string> GetMessage(string url)
    {
      HttpClient client = new HttpClient();

      //var result = await client.GetAsync(url).ConfigureAwait(false);
      //return await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return await client.GetStringAsync(url).ConfigureAwait(false);
      

      //success will show: "{\"code\":200,\"message\":\"ok\",\"result\":\"ok\"}"
      //failure could be like: "{\"code\":109,\"message\":\"I don\\u0027t know that token.\",\"result\":\"error\"}"
      //or "{\"code\":113,\"message\":\"Invalid JSON.\",\"result\":\"error\"}"
    }



  }
}
