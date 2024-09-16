namespace sw.infrastructure.UnitOfWorks;

public interface IUnitOfWork
{
    void Commit();
    void Close();
}