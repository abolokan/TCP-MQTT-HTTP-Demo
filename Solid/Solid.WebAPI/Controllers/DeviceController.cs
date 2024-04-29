using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Solid.Application.Interfaces;
using Solid.Application.LiveDeviceHandlers;
using Solid.Common.Exceptions;
using Solid.Common.Utils;
using Solid.WebAPI.AppSettings;
using Solid.WebAPI.Utils;
using System.Net;

namespace Solid.WebAPI.Controllers;

[ApiController]
[Route("devices")]
public class DeviceController : ControllerBase
{
    private readonly ILogger<DeviceController> _logger;
    private readonly ISensorService _sensorService;
    private readonly IFileManager _fileManager;
    private readonly ICryptoService _cryptoService;
    private readonly RequiredHtppHeaders _requiredHtppHeaders;

    public DeviceController(
        IOptions<RequiredHtppHeaders> requiredHtppHeadersOptions,
        ILogger<DeviceController> logger,
        ISensorService sensorService,
        IFileManager fileManager,
        ICryptoService cryptoService)
    {
        _requiredHtppHeaders = requiredHtppHeadersOptions.Value;
        _logger = logger;
        _sensorService = sensorService;
        _fileManager = fileManager;
        _cryptoService = cryptoService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var headers = Request.Headers;
        if (headers[_requiredHtppHeaders.ESP8266[0]].Any())
        {
            HeaderUtils.ValidateHeaders(Request.Headers, _requiredHtppHeaders.ESP8266);
        }

        if (headers[_requiredHtppHeaders.ESP32[0]].Any())
        {
            HeaderUtils.ValidateHeaders(Request.Headers, _requiredHtppHeaders.ESP32);
        }

        string macAddress = headers[_requiredHtppHeaders.ESP8266[0]].FirstOrDefault() ?? headers[_requiredHtppHeaders.ESP32[0]].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(macAddress))
        {
            throw new BadHttpRequestException($"MacAddres is not found");
        }

        var sensor = await _sensorService.GetByDeviceIdAsync(macAddress, cancellationToken);

        if (sensor == null)
        {
            throw new BadHttpRequestException("Unkown device requested");
        }

        if (!sensor.Type.HasValue)
        {
            throw new ApiException("Sensor doesn't have valid 'type' field", HttpStatusCode.ExpectationFailed);
        }

        // Did the device happen to send it's current FW version? Only works for ESP32
        if (headers[_requiredHtppHeaders.ESP32[0]].Any())
        {
            // add some handler to define semver.coerc method            
            var currentVersion = VersionUtils.CoerceVersion(headers["x-esp32-version"][0]);

            // Determine latest version
            var latestVersion = VersionUtils.CoerceVersion(sensor.FwVersion);

            // If both are valid version numbers
            if (VersionUtils.IsValid(latestVersion) && VersionUtils.IsValid(currentVersion))
            {
                // if the current version is at least the same as the latest known version
                if (VersionUtils.IsSatisfied(currentVersion, latestVersion))
                {
                    _logger.LogInformation($"Found semvers, NO update necessary. Current version is {currentVersion}, latest version is {latestVersion}");
                    throw new NoUpdateException("No update necessary");
                }
                else
                {
                    _logger.LogInformation("Found semvers, and update is necessary. Current version is " + VersionUtils.CoerceVersion(currentVersion) + ", latest version is " + latestVersion);
                }
            }
        }

        // Note that sensor.type is retrieved from the sensor_type table, not from the sensor table.
        _logger.LogInformation($" > Found sensor {sensor.DeviceId} with type {sensor.Type}");

        if (await _fileManager.IsFileExistsAsync(sensor.FileName))
        {
            byte[] binary = await _fileManager.GetFileDataAsync(sensor.FileName);

            string hex = _cryptoService.Hex(binary);

            Response.Headers.Add("x-MD5", hex);
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{sensor.FileName}\"");

            Response.ContentLength = binary.Length;
            Response.ContentType = "application/octet-stream";

            await Response.Body.WriteAsync(binary, 0, binary.Length);

            return new EmptyResult();
        }
        else
        {
            throw new NoUpdateException("No update found");
        }
    }

    [HttpGet("status/{deviceId}")]
    public async Task<ActionResult<string>> GetStatusAsync(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            throw new BadHttpRequestException("Missing deviceId");
        }

        // Find the device in connection list
        var connection = DeviceConnections.TryGetDevice(deviceId);
        if (connection != null)
        {
            return await Task.FromResult("Device is online");
        }
        else
        {
            throw new BadHttpRequestException($"Device '${deviceId}' is not connected");
        }
    }

    [HttpPut("requestupdate/{deviceId}")]
    public async Task<ActionResult<string>> RequestUpdateAsync(string deviceId)
    {
        _logger.LogInformation($" > Request For Update From IP: {Request.Headers["x-real-ip"]}");

        if (string.IsNullOrWhiteSpace(deviceId))
        {
            throw new BadHttpRequestException("Missing deviceId");
        }

        var diviceCorrected = deviceId.ToLower();

        _logger.LogInformation("> DeviceID: {0} ", diviceCorrected);

        var device = DeviceConnections.TryGetDevice(diviceCorrected);

        device.RequestUpdate();

        _logger.LogInformation("> Update requested", diviceCorrected);

        return await Task.FromResult("OK. Request for update sent.");
    }

    [HttpPost("command/{deviceId}/{command}")]
    public async Task<ActionResult<string>> CommandAsync(string deviceId, string command)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            throw new BadHttpRequestException("Missing deviceId");
        }

        if (string.IsNullOrWhiteSpace(command))
        {
            throw new BadHttpRequestException("Missing command");
        }

        var diviceCorrected = deviceId.ToLower();

        _logger.LogInformation("> DeviceID: {0} ", diviceCorrected);

        var device = DeviceConnections.TryGetDevice(diviceCorrected);

        device.SendCommand(command);

        _logger.LogInformation("> Command sent");

        return await Task.FromResult("OK. Request for command sent.");
    }
}
