using System;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Devices;

public class CreateDeviceResourceParameters
{
  [Required]
  [StringLength(250)]
  public string Imei { get; set; }

  [Required]
  [StringLength(250)]
  public string SerialNumber { get; set; }

  [Required]
  public string IpAddress { get; set; }

  // Device Model
  [Required]
  public long DeviceModelId { get; set; }

  // Simcard
  [Required]
  public string PhoneNumber { get; set; }
}