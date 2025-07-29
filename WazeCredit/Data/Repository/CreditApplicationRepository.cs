using WazeCredit.Models;

namespace WazeCredit.Data.Repository;

public class CreditApplicationRepository : Repository<CreditApplication>, ICreditApplicationRepository
{
    private readonly AppDbContext _dbContext;
    public CreditApplicationRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(CreditApplication creditApplication)
    {
        _dbContext.CreditApplications.Update(creditApplication);
    }
}