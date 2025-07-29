namespace WazeCredit.Data.Repository;

public interface IUnitOfWork : IDisposable
{
    ICreditApplicationRepository CreditApplication { get; }

    void Save();
}