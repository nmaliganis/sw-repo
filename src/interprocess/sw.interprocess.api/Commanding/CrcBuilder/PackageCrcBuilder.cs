using System;
using System.Collections.Generic;
using System.Linq;

namespace sw.interprocess.api.Commanding.CrcBuilder
{
  public class PackageCrcBuilder : IPackageCrcBuilder
  {

    public byte Build(byte[] package)
    {
      var packageToBeCheckedForCrc = new byte[package.Length - 1];
      Array.Copy(package,
        0, 
        packageToBeCheckedForCrc,
        0, 
        package.Length - 1
        );
      return BuildCrc(packageToBeCheckedForCrc);
    }

    private byte BuildCrc(IEnumerable<byte> package)
    {
      return package.Aggregate<byte, byte>(0x00, (current, t) => CheckCrc(current, t));
    }


    private byte CheckCrc(byte oldByte, byte newByte)
    {
      var shiftReg = oldByte;

      for (var j = 0; j < 8; j++)
      {
        var dataBit = (byte)((newByte >> j) & 0x01);
        var srLsb = (byte)(shiftReg & 0x01);
        var fbBit = (byte)((dataBit ^ srLsb) & 0x01);
        shiftReg = (byte)(shiftReg >> 1);
        if (fbBit == 0x01)
          shiftReg = (byte)(shiftReg ^ 0x8c);
      }
      return (shiftReg);
    }
  }
}