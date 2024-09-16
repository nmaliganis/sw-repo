using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders;
using sw.interprocess.api.Commanding.PackageRepository;
using sw.interprocess.api.Helpers.Exceptions.Commands;
using System.Collections.Generic;

namespace sw.interprocess.api.Commanding.Commands.Inbound
{
    public sealed class WmInboundCommandBuilderRepository
    {
        private readonly Dictionary<string, IWmInboundCommandBuilder> _cmdBuilders;


        private WmInboundCommandBuilderRepository()
        {
            _cmdBuilders = new Dictionary<string, IWmInboundCommandBuilder>()
                               {
                                   {WmPackageRepository.GetPackageRepository.GpsMessageCode,
                                    new GpsPackageDetectedInboundCommandBuilder()},
                                  {WmPackageRepository.GetPackageRepository.GpioMessageCode,
                                    new GpioPackageDetectedInboundCommandBuilder()},
                                  {WmPackageRepository.GetPackageRepository.BinaryUltrasonicMessageCode,
                                    new BinaryUltrasonicPackageDetectedInboundCommandBuilder()},
                                  {WmPackageRepository.GetPackageRepository.BinaryUltrasonicMultiMessageCode,
                                    new BinaryUltrasonicMultiPackageDetectedInboundCommandBuilder()},
                                  {WmPackageRepository.GetPackageRepository.UltrasonicMessageCode,
                                    new UltrasonicPackageDetectedInboundCommandBuilder()},
                               };
        }

        public static WmInboundCommandBuilderRepository GetCommandBuilderRepository { get; } = new WmInboundCommandBuilderRepository();

        public IWmInboundCommandBuilder this
            [string index]
        {
            get
            {
                try
                {
                    return _cmdBuilders[index];
                }
                catch (KeyNotFoundException)
                {
                    throw new WmCommandNotFoundException();
                }
            }
        }
    }
}
