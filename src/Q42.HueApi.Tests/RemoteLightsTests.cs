using Microsoft.VisualStudio.TestTools.UnitTesting;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Remote;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class RemoteLightsTests
  {
    private IHueRemoteClient _client;

    [TestInitialize]
    public void Initialize()
    {
      string accessToken = ConfigurationManager.AppSettings["accessToken"].ToString();      

      _client = new HueRemoteClient(accessToken);
    }

    [TestMethod]
    public async Task GetLightsAsyncTest()
    {
      //Search for all lights
      var lights = await _client.GetLightsAsync();

      Assert.IsNotNull(lights, "should not be null");
      Assert.IsTrue(lights.Count()>0, "should have more than 0 lights");
    }

    [TestMethod]
    public async Task SendCommandAsync()
    {
      //Create command
      var command = new LightCommand();
      command.TurnOff();
      command.SetColor("#225566");

      List<string> lights = new List<string>()
      {
        "1"
      };

      //Send Command
      //await _client.SendCommandAsync(command); //to group
      var result = await _client.SendCommandAsync(command, lights);

      Assert.IsNotNull(result, "should not be null");      

    }

  }
}
