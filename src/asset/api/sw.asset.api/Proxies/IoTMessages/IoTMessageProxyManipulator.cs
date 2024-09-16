using sw.asset.api.Proxies.Base;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.ResourceParameters.Events;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.asset.contracts.V1.EventProcessors;
using sw.asset.model.Assets.Containers;
using sw.azure.messaging.Commanding;
using sw.azure.messaging.Commanding.Events;
using sw.azure.messaging.Commanding.Listeners;
using sw.azure.messaging.Models.IoT;
using sw.infrastructure.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace sw.asset.api.Proxies.IoTMessages;

/// <summary>
/// Class : MemberRegisteredProxyManipulator
/// </summary>
public class IoTMessageProxyManipulator : BaseProxyManipulator, IIoTMessageProxyManipulator,
  IIoTGpsNotificationActionListener, IIoTUltrasonicNotificationActionListener, IIoTDigitalNotificationActionListener
{
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly IServiceProvider _service;
	private readonly IMediator _mediator;

	/// <summary>
	/// Property : Configuration
	/// </summary>
	public IConfiguration Configuration { get; }

	/// <summary>
	/// Constructor : IoTMessageProxyManipulator
	/// </summary>
	/// <param name="configuration"></param>
	/// <param name="scopeFactory"></param>
	/// <param name="service"></param>
	/// /// <param name="mediator"></param>
	public IoTMessageProxyManipulator(IConfiguration configuration,
		IServiceScopeFactory scopeFactory,
		IServiceProvider service, IMediator mediator)
		: base(scopeFactory)
	{
		Configuration = configuration;

		_scopeFactory = scopeFactory;
		_service = service;
		_mediator = mediator;
	}

	/// <summary>
	/// Method : ProxyInitializer
	/// </summary>
	public void ProxyInitializer()
	{
		IoTMessageCommander.Instance.Attach((IIoTGpsNotificationActionListener)this);
		IoTMessageCommander.Instance.Attach((IIoTUltrasonicNotificationActionListener)this);
		IoTMessageCommander.Instance.Attach((IIoTDigitalNotificationActionListener)this);
		Log.Information($"********************************************************\r\n");
		Log.Information($"Register on IIoTGpsNotificationActionListener - IIoTUltrasonicNotificationActionListener - IIoTDigitalNotificationActionListener");
		Log.Information($"********************************************************\r\n");

		AnsiConsole.Write(new Rule("[lightsalmon3_1]ProxyInitializer - Start[/]"));
		AnsiConsole.WriteLine();
		AnsiConsole.Markup("[bold yellow on blue]Register on IIoTGpsNotificationActionListener - IIoTUltrasonicNotificationActionListener - IIoTDigitalNotificationActionListener[/]");
		AnsiConsole.WriteLine();
		AnsiConsole.Write(new Rule("[lightsalmon3_1]ProxyInitializer - End[/]"));
	}

	/// <summary>
	/// Method : Update for IIoTGpsNotificationActionListener Listener - 18
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public async void Update(object sender, IoTGpsMessageEventArgs e)
	{
		Log.Information($"********************************************************\r\n");
		Log.Information($"GPS Id:{e.IsEnabled}\r\n");
		Log.Information($"********************************************************\r\n");


		AnsiConsole.Write(new Rule("[plum2]Update IoTGpsMessageEventArgs - Start[/]"));
		AnsiConsole.WriteLine();
		AnsiConsole.Markup($"[bold yellow on blue] GPS Event for Device Imei: {e.Imei}, at {e.Timestamp} and Values Lat: {e.PayloadGps.Latitude} Lon: {e.PayloadGps.Longitude} [/]");
		AnsiConsole.WriteLine();

		try
		{
			using var scope = _scopeFactory.CreateScope();
			var updateContainerProcessor = scope.ServiceProvider.GetRequiredService<IUpdateContainerProcessor>();

			var createEventHistoryProcessor = scope.ServiceProvider.GetRequiredService<ICreateEventHistoryProcessor>();

			Log.Information(
			  $"IoTGpsMessageEventArgs: {DateTime.UtcNow}" +
			  "--IoTMessageProxyManipulator--  @Complete@ [UpdateContainerProcessor]. " +
			  "Message: Just Before UpdateContainerWithMeasurementsAsync");
			var responseUpdateMeasurement = await updateContainerProcessor.UpdateContainerWithMeasurementsForMotionAsync(new UpdateContainerMeasurementsCommand(
			  "18",
			  e.Imei,
			  new ContainerModificationMeasurementsUiModel
			  {
				  Latitude = e.PayloadGps.Latitude,
				  Longitude = e.PayloadGps.Longitude,
				  Altitude = e.PayloadGps.Altitude,
				  Speed = e.PayloadGps.Speed,
				  Direction = e.PayloadGps.Direction,
				  FixMode = e.PayloadGps.FixMode,
				  Hdop = e.PayloadGps.Hdop,
				  SatellitesUsed = e.PayloadGps.SatellitesUsed,
				  Recorded = e.PayloadGps.Recorded,
			  }));
			Log.Information(
			  $"IoTGpsMessageEventArgs: {DateTime.UtcNow}" +
			  "--IoTMessageProxyManipulator--  @Complete@ [UpdateContainerProcessor]. " +
			  "Message: Just After UpdateContainerWithMeasurementsAsync");
			/*-----------------------------------------------------------------------------------------------------*/
			/*-----------------------------------------------------------------------------------------------------*/
			Log.Information(
			  $"IoTGpsMessageEventArgs: {DateTime.UtcNow}" +
			  "--IoTMessageProxyManipulator--  @Complete@ [CreateEventHistoryProcessor]. " +
			  "Message: Just Before CreateEventHistoryAsync");
            Thread.Sleep(1000);
            var responseCreateEventHistory = await createEventHistoryProcessor.CreateEventHistoryAsync(e.Imei, 
				new CreateEventHistoryCommand(18, e.Imei, new CreateEventHistoryResourceParameters()
			{
				EventValue = 0.0,
				Recorded = e.PayloadGps.Recorded,
				Received = e.Timestamp,
				EventValueJson = JsonConvert.SerializeObject(e.PayloadGps, new JsonSerializerSettings { Formatting = Formatting.None })
			}
			));

			if (responseCreateEventHistory.IsSuccess())
			{
				AnsiConsole.Markup(
					$"[bold black on white] SUCCESS_EVENT_HISTORY_CREATION[/]");
				AnsiConsole.WriteLine();
			}
			else
			{
				AnsiConsole.Markup(
					$"[bold black on white] FAILED_MEASUREMENT_MODIFICATION_ON_MOTION[/]");
				AnsiConsole.WriteLine();

				AnsiConsole.Markup(
					$"[bold black on white] {responseUpdateMeasurement.BrokenRules.FirstOrDefault()!.Rule}[/]");
				AnsiConsole.WriteLine();
			}

			Log.Information(
			  $"IoTGpsMessageEventArgs: {DateTime.UtcNow}" +
			  "--IoTMessageProxyManipulator--  @Complete@ [CreateEventHistoryProcessor]. " +
			  "Message: Just After CreateEventHistoryAsync");
		}
		catch (Exception ex)
		{
			AnsiConsole.WriteException(ex);
			Log.Fatal(
			  $"IoTGpsMessageEventArgs:  {DateTime.UtcNow}" +
			  "--IoTMessageProxyManipulator--  @Complete@ [UpdateContainerProcessor]. " +
			  $"Error: {ex.Message}  ");
			return;
		}

		AnsiConsole.Write(new Rule("[red]Update IoTGpsMessageEventArgs - End[/]"));
	}

	/// <summary>
	/// Method : Update for IIoTUltrasonicNotificationActionListener Listener - 68
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public async void Update(object sender, IoTUltrasonicMessageEventArgs e)
	{
		Log.Information($"********************************************************\r\n");
		Log.Information($"Ultrasonic Id:{e.IsEnabled}\r\n");
		Log.Information($"********************************************************\r\n");

		AnsiConsole.Write(new Rule("[indianred1]Update IoTUltrasonicMessageEventArgs - Start[/]"));
		AnsiConsole.WriteLine();
		AnsiConsole.Markup($"[bold green on black] Ultrasonic Event for Device Imei: {e.Imei}, at {e.Timestamp} and Values : {e.PayloadUltrasonic.Count}[/]");
		AnsiConsole.WriteLine();

		IDictionary<int, List<IoTUltrasonic>> ultrasonicExtractedRanges = new Dictionary<int, List<IoTUltrasonic>>();

		int payloadIndex = e.PayloadUltrasonic.Count / 6;

		if (payloadIndex >= 1)
		{
			for (int i = 0; i < payloadIndex; i++)
			{
				ultrasonicExtractedRanges.Add(i, e.PayloadUltrasonic.GetRange(6 * i, 6));
			}
		}
		else
		{
			AnsiConsole.Write(new Rule("[indianred1]Update IoTUltrasonicMessageEventArgs - Exiting - End[/]"));
			return;
		}

		IList<IoTUltrasonic> ultrasonicCalculatedRanges = new List<IoTUltrasonic>();

		if (ultrasonicExtractedRanges.Keys.Count >= 0)
		{
			foreach (var key in ultrasonicExtractedRanges.Keys)
			{
				IoTUltrasonic ultrasonicToBeCalculated = new IoTUltrasonic()
				{
					Imei = ultrasonicExtractedRanges[key].FirstOrDefault()!.Imei,
					Recorded = ultrasonicExtractedRanges[key].FirstOrDefault()!.Recorded,
					Status = ultrasonicExtractedRanges[key].FirstOrDefault()!.Status,
				};

				List<double> ranges = new List<double>();

				foreach (var ioTUltrasonic in ultrasonicExtractedRanges[key])
				{
					if (!ioTUltrasonic.IsNull() && ioTUltrasonic.Range != 0)
						ranges.Add(Convert.ToDouble(ioTUltrasonic.Range));
				}

				ultrasonicToBeCalculated.Range = CalculateRangeMedian(ranges);

				ultrasonicCalculatedRanges.Add(ultrasonicToBeCalculated);
			}
		}
		else
		{
			AnsiConsole.Write(new Rule("[indianred1]Update IoTUltrasonicMessageEventArgs - Exiting - End[/]"));
			return;
		}

		if (ultrasonicCalculatedRanges.Count <= 0)
		{
			AnsiConsole.Write(new Rule("[indianred1]Update IoTUltrasonicMessageEventArgs - Exiting - End[/]"));
			return;
		}

		foreach (var ultrasonicCalculatedRange in ultrasonicCalculatedRanges)
		{
			try
			{
				using var scope = _scopeFactory.CreateScope();
				var updateContainerProcessor = scope.ServiceProvider.GetRequiredService<IUpdateContainerProcessor>();
				var sensorRepository = scope.ServiceProvider.GetRequiredService<ISensorRepository>();
				var containerRepository = scope.ServiceProvider.GetRequiredService<IContainerRepository>();
				var createEventHistoryProcessor = scope.ServiceProvider.GetRequiredService<ICreateEventHistoryProcessor>();

				Log.Information(
				  $"IoTUltrasonicMessageEventArgs: {DateTime.UtcNow}" +
				  "--IoTMessageProxyManipulator--  @Complete@ [UpdateContainerProcessor]. " +
				  "Message: Just Before UpdateContainerWithMeasurementsAsync");
				var responseUpdateMeasurement = await updateContainerProcessor.UpdateContainerWithMeasurementsForUltrasonicAsync(new UpdateContainerMeasurementsCommand(
				  "68",
				  e.Imei,
				  new ContainerModificationMeasurementsUiModel
				  {
					  Range = Convert.ToDouble(ultrasonicCalculatedRange.Range),
					  Status = ultrasonicCalculatedRange.Status,
					  Temperature = ultrasonicCalculatedRange.Temperature,
					  Recorded = ultrasonicCalculatedRange.Recorded
				  }));

				if (responseUpdateMeasurement.IsSuccess())
				{
					AnsiConsole.Markup(
						$"[bold black on white] SUCCESS_MEASUREMENT_MODIFICATION_ON_ULTRASOUND[/]");
					AnsiConsole.WriteLine();
                    ultrasonicCalculatedRange.Level = responseUpdateMeasurement.Model.Level;
                }

				Log.Information(
				  $"IoTUltrasonicMessageEventArgs: {DateTime.UtcNow}" +
				  "--IoTMessageProxyManipulator--  @Complete@ [UpdateContainerProcessor]. " +
				  "Message: Just After UpdateContainerWithMeasurementsAsync");

				/*-----------------------------------------------------------------------------------------------------*/
				/*-----------------------------------------------------------------------------------------------------*/

				var sensorToBeFetched = sensorRepository.FindByDeviceImei(e.Imei);

				if (sensorToBeFetched.IsNull() || sensorToBeFetched.Count <= 0)
				{
					Log.Fatal(
					  $"IoTUltrasonicMessageEventArgs:  {DateTime.UtcNow}" +
					  "--IoTMessageProxyManipulator--  @Complete@ [UpdateContainerProcessor]. " +
					  $"Error: Sensor or Container is not binded with Device with Imei {e.Imei}.");

					AnsiConsole.Write(new Rule("[indianred1]Update IoTUltrasonicMessageEventArgs - Exiting - End[/]"));
					return;
				}

				Container containerToBeFetched = containerRepository.FindBy(sensorToBeFetched.FirstOrDefault()!.Asset.Id);

				if (containerToBeFetched.IsNull())
				{
					AnsiConsole.Write(new Rule("[indianred1]Update IoTUltrasonicMessageEventArgs - Exiting - End[/]"));
					return;
				}

				/*-----------------------------------------------------------------------------------------------------*/
				/*-----------------------------------------------------------------------------------------------------*/

				Log.Information(
				  $"IoTUltrasonicMessageEventArgs: {DateTime.UtcNow}" +
				  "--IoTMessageProxyManipulator--  @Complete@ [CreateEventHistoryProcessor]. " +
				  "Message: Just Before CreateEventHistoryAsync");

				if (containerToBeFetched.IsNull())
				{
					AnsiConsole.Write(new Rule("[indianred1]Update IoTUltrasonicMessageEventArgs - Exiting - End[/]"));
					return;
				}
				Thread.Sleep(1000);
				var responseCreateEventHistory = await createEventHistoryProcessor.CreateEventHistoryAsync(e.Imei,
					new CreateEventHistoryCommand(68, e.Imei, new CreateEventHistoryResourceParameters()
					{
						EventValue = containerToBeFetched.Level,
						Recorded = ultrasonicCalculatedRange.Recorded,
						Received = e.Timestamp,
						EventValueJson = JsonConvert.SerializeObject(ultrasonicCalculatedRange, new JsonSerializerSettings { Formatting = Formatting.None })
					}
				));


				if (responseCreateEventHistory.IsSuccess())
				{
					AnsiConsole.Markup(
						$"[bold black on white] SUCCESS_EVENT_HISTORY_CREATION_ON_ULTRASOUND[/]");
					AnsiConsole.WriteLine();
				}
				else
				{
					AnsiConsole.WriteLine();
					AnsiConsole.Markup(
						$"[bold white on red] {responseCreateEventHistory.BrokenRules.FirstOrDefault()!.Rule}[/]");
					AnsiConsole.WriteLine();
					AnsiConsole.Markup(
						$"[bold white on red] sw[/]");
					AnsiConsole.WriteLine();
				}

				Log.Information(
				  $"IoTUltrasonicMessageEventArgs: {DateTime.UtcNow}" +
				  "--IoTMessageProxyManipulator--  @Complete@ [CreateEventHistoryProcessor]. " +
				  "Message: Just After CreateEventHistoryAsync");
			}
			catch (Exception ex)
			{
				Log.Fatal(
				  $"IoTUltrasonicMessageEventArgs:  {DateTime.UtcNow}" +
				  "--IoTMessageProxyManipulator--  @Complete@ [UpdateContainerProcessor]. " +
				  $"Error: {ex.Message}  ");
				return;
			}
		}

		AnsiConsole.Write(new Rule("[indianred1]Update IoTUltrasonicMessageEventArgs - End[/]"));
	}

	/// <summary>
	/// Method : Update for IIoTDigitalNotificationActionListener Listener - 1
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public async void Update(object sender, IoTDigitalMessageEventArgs e)
	{
		Log.Information($"********************************************************\r\n");
		Log.Information($"Digital Id:{e.IsEnabled}\r\n");
		Log.Information($"********************************************************\r\n");

		AnsiConsole.Write(new Rule("[khaki1]Update IoTDigitalMessageEventArgs - Start[/]"));
		AnsiConsole.WriteLine();
		AnsiConsole.Markup($"[bold black on white] Ultrasonic Event for Device Imei: {e.Imei}, at {e.Timestamp} and Values : {e.PayloadDigital.FirstOrDefault()?.NewValue}[/]");
		AnsiConsole.WriteLine();

		try
		{
			using var scope = _scopeFactory.CreateScope();
			var updateContainerProcessor = scope.ServiceProvider.GetRequiredService<IUpdateContainerProcessor>();

			var createEventHistoryProcessor = scope.ServiceProvider.GetRequiredService<ICreateEventHistoryProcessor>();

			Log.Information(
			  $"IoTUltrasonicMessageEventArgs: {DateTime.UtcNow}" +
			  "--IoTMessageProxyManipulator--  @Complete@ [UpdateContainerProcessor]. " +
			  "Message: Just Before UpdateContainerWithMeasurementsAsync");
			Log.Information(
			  $"IoTUltrasonicMessageEventArgs: {DateTime.UtcNow}" +
			  "--IoTMessageProxyManipulator--  @Complete@ [UpdateContainerProcessor]. " +
			  "Message: Just After UpdateContainerWithMeasurementsAsync");

			/*-----------------------------------------------------------------------------------------------------*/
			/*-----------------------------------------------------------------------------------------------------*/

			Log.Information(
			  $"IoTUltrasonicMessageEventArgs: {DateTime.UtcNow}" +
			  "--IoTMessageProxyManipulator--  @Complete@ [CreateEventHistoryProcessor]. " +
			  "Message: Just Before CreateEventHistoryAsync");
            Thread.Sleep(1000);
            var responseCreateEventHistory = await createEventHistoryProcessor.CreateEventHistoryAsync(e.Imei, 
				new CreateEventHistoryCommand(1, e.Imei, new CreateEventHistoryResourceParameters()
			{
				EventValue = (double)e.PayloadDigital.Last().PinNumber,
				Recorded = e.PayloadDigital.Last().Recorded,
				Received = e.Timestamp,
				EventValueJson = JsonConvert.SerializeObject(e.PayloadDigital, new JsonSerializerSettings { Formatting = Formatting.None })
			}
			));

			if (responseCreateEventHistory.IsSuccess())
			{
				AnsiConsole.Markup(
					$"[bold black on white] SUCCESS_EVENT_HISTORY_CREATION[/]");
				AnsiConsole.WriteLine();
			}

			Log.Information(
			  $"IoTUltrasonicMessageEventArgs: {DateTime.UtcNow}" +
			  "--IoTMessageProxyManipulator--  @Complete@ [CreateEventHistoryProcessor]. " +
			  "Message: Just After CreateEventHistoryAsync");
		}
		catch (Exception ex)
		{
			Log.Fatal(
			  $"IoTUltrasonicMessageEventArgs:  {DateTime.UtcNow}" +
			  "--IoTMessageProxyManipulator--  @Complete@ [UpdateContainerProcessor]. " +
			  $"Error: {ex.Message}  ");
			return;
		}

		AnsiConsole.Write(new Rule("[khaki1]Update IoTDigitalMessageEventArgs - End[/]"));
	}

	private double CalculateRangeMedian(List<double> ranges)
	{
		ranges.Sort();

		double median;
		int count = ranges.Count;

		if (count % 2 == 0)
		{
			int middleIndex1 = count / 2 - 1;
			int middleIndex2 = count / 2;
			median = Convert.ToDouble((ranges[middleIndex1] + ranges[middleIndex2]) / 2.0);
		}
		else
		{
			int middleIndex = count / 2;
			median = Convert.ToDouble(ranges[middleIndex]);
		}

		return median;
	}

} // Class : IoTMessageProxyManipulator