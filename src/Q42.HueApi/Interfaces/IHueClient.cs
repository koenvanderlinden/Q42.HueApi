﻿using Q42.HueApi.Models;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  /// <summary>
  /// Hue Client for interaction with the bridge
  /// </summary>
  public interface IHueClient
  {
    /// <summary>
    /// Base address url
    /// </summary>
    string ApiBase { get; }

    /// <summary>
    /// Initialize the client with your app key
    /// </summary>
    /// <param name="appKey"></param>
    void Initialize(string appKey);

    /// <summary>
    /// Register your <paramref name="appName"/> and <paramref name="appKey"/> at the Hue Bridge.
    /// </summary>
    /// <param name="appKey">Secret key for your app. Must be at least 10 characters.</param>
    /// <param name="appName">The name of your app or device.</param>
    /// <returns><c>true</c> if success, <c>false</c> if the link button hasn't been pressed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="appName"/> or <paramref name="appKey"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="appName"/> or <paramref name="appKey"/> aren't long enough, are empty or contains spaces.</exception>
    Task<bool> RegisterAsync(string appName, string appKey);

    /// <summary>
    /// Set the next Hue color
    /// </summary>
    /// <param name="lightList"></param>
    /// <returns></returns>
    Task<HueResults> SetNextHueColorAsync(IEnumerable<string> lightList = null);

    /// <summary>
    /// Asynchronously gets all lights registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="Light"/>s registered with the bridge.</returns>
    Task<IEnumerable<Light>> GetLightsAsync();

    /// <summary>
    /// Asynchronously gets all lights registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="WhiteList"/>s registered with the bridge.</returns>
    Task<IEnumerable<WhiteList>> GetWhiteListAsync();

    /// <summary>
    /// Asynchronously retrieves an individual light.
    /// </summary>
    /// <param name="id">The light's Id.</param>
    /// <returns>The <see cref="Light"/> if found, <c>null</c> if not.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="id"/> is empty or a blank string.</exception>
    Task<Light> GetLightAsync(string id);

    /// <summary>
    /// Sets the light name
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<HueResults> SetLightNameAsync(string id, string name);

    /// <summary>
    /// Get bridge info
    /// </summary>
    /// <returns></returns>
    Task<Bridge> GetBridgeAsync();

    /// <summary>
    /// Update bridge config
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    Task<HueResults> UpdateBridgeConfigAsync(BridgeConfigUpdate update);

    /// <summary>
    /// Deletes a whitelist entry
    /// </summary>
    /// <returns></returns>
    Task<bool> DeleteWhiteListEntryAsync(string entry);


    /// <summary>
    /// Send a raw string / json command
    /// </summary>
    /// <param name="command">json</param>
    /// <param name="lightList">if null, send to all lights</param>
    /// <returns></returns>
    Task<HueResults> SendCommandRawAsync(string command, IEnumerable<string> lightList = null);

    /// <summary>
    /// Send a light command
    /// </summary>
    /// <param name="command">Compose a new lightCommand()</param>
    /// <param name="lightList">if null, send to all lights</param>
    /// <returns></returns>
    Task<HueResults> SendCommandAsync(LightCommand command, IEnumerable<string> lightList = null);

    /// <summary>
    /// Send command to a group
    /// </summary>
    /// <param name="command"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    Task<HueResults> SendGroupCommandAsync(LightCommand command, string group = "0");

    /// <summary>
    /// Creates a group for a set of lights
    /// </summary>
    /// <param name="lights"></param>
    /// <param name="name">Group Name (optional)</param>
    /// <returns></returns>
    Task<string> CreateGroupAsync(IEnumerable<string> lights, string name = null);

    /// <summary>
    /// Deletes a single group
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    Task<HueResults> DeleteGroupAsync(string groupId);

    /// <summary>
    /// Get all groups
    /// </summary>
    /// <returns></returns>
    Task<List<Group>> GetGroupsAsync();

    /// <summary>
    /// Get the state of a single group
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Group> GetGroupAsync(string id);

    /// <summary>
    /// Update a group
    /// </summary>
    /// <param name="id">Group ID</param>
    /// <param name="lights">List of light IDs</param>
    /// <param name="name">Group Name (optional)</param>
    /// <returns></returns>
    Task<HueResults> UpdateGroupAsync(string id, List<string> lights, string name = null);

    /// <summary>
    /// Start searching for new lights
    /// </summary>
    /// <returns></returns>
    Task<HueResults> SearchNewLightsAsync();

    /// <summary>
    /// Gets a list of lights that were discovered the last time a search for new lights was performed.
    /// </summary>
    /// <returns></returns>
    Task<List<Light>> GetNewLightsAsync();

    #region Schedules

    /// <summary>
    /// Get all schedules
    /// </summary>
    /// <returns></returns>
    Task<List<Schedule>> GetSchedulesAsync();


    /// <summary>
    /// Get a single schedule
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Schedule> GetScheduleAsync(string id);


    /// <summary>
    /// Create a schedule
    /// </summary>
    /// <param name="schedule"></param>
    /// <returns></returns>
    Task<string> CreateScheduleAsync(Schedule schedule);


    /// <summary>
    /// Update a schedule
    /// </summary>
    /// <param name="id"></param>
    /// <param name="schedule"></param>
    /// <returns></returns>
    Task<HueResults> UpdateScheduleAsync(string id, Schedule schedule);


    /// <summary>
    /// Delete a schedule
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<HueResults> DeleteScheduleAsync(string id);


    #endregion

    #region Info

    Task<IEnumerable<string>> GetTimeZonesAsync();

    #endregion

    #region Scenes

    Task<IEnumerable<Scene>> GetScenesAsync();
    Task<HueResults> CreateOrUpdateSceneAsync(string id, string name, IEnumerable<string> lights);
    Task<HueResults> ModifySceneAsync(string sceneId, string lightId, LightCommand command);
    #endregion

    #region Rules

    Task<IEnumerable<Rule>> GetRulesAsync();
    Task<Rule> GetRuleAsync(string id);
    Task<HueResults> DeleteRule(string id);
    Task<string> CreateRule(string name, IEnumerable<RuleCondition> conditions, IEnumerable<RuleAction> actions);
    Task<HueResults> UpdateRule(string id, string name, IEnumerable<RuleCondition> conditions, IEnumerable<RuleAction> actions);

    #endregion

    #region Sensors

    Task<IEnumerable<Sensor>> GetSensorsAsync();
    Task<string> CreateSensorAsync(Sensor sensor);
    Task<HueResults> FindNewSensorsAsync();
    Task<List<Sensor>> GetNewSensorsAsync();
    Task<Sensor> GetSensorAsync(string id);
    Task<HueResults> UpdateSensorAsync(string id, string newName);
    Task<HueResults> ChangeSensorConfigAsync(string id, SensorConfig config);
    Task<HueResults> ChangeSensorStateAsync(string id, SensorState state);

    #endregion
  }
}
