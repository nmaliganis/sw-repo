using sw.interprocess.api.Models.Messages;

namespace sw.interprocess.api.Commanding.PackageExtractor
{
    public interface IPackageExtractor
    {
        WasteMessage ExtractPackages(string packageMessage);
    }
}