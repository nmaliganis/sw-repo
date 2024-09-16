namespace sw.interprocess.api.Commanding.PackageCheckers
{
    public interface IPackageChecker
    {
        void Check(byte[] package);
        bool IsValidPackage(string packageMessage);
    }
}