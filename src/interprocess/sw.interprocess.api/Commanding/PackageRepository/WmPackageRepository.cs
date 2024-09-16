namespace sw.interprocess.api.Commanding.PackageRepository
{
    public sealed class WmPackageRepository
    {
        private WmPackageRepository()
        {
        }
        public static WmPackageRepository GetPackageRepository { get; } = new WmPackageRepository();


        #region Command Definition

        public int EncodingCode => 1252;

        #endregion

        #region Command Lengths

        public int HeaderImeiLength => 8;


        #endregion

        #region Command Offsets
        public byte HeaderImeiOffset => 0;
        public string GpsMessageCode { get; set; } = "$21";
        public string GpioMessageCode { get; set; } = "$5";
        public string BinaryUltrasonicMessageCode { get; set; } = "$16";
        public string BinaryUltrasonicMultiMessageCode { get; set; } = "$17";
        public string UltrasonicMessageCode { get; set; } = "$15";

        #endregion
    }
}
