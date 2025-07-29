namespace WazeCredit.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        CreditApplication = new CreditApplicationRepository(_dbContext);
    }
    public ICreditApplicationRepository CreditApplication { get; private set; }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}