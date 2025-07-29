using WazeCredit.Models;

namespace WazeCredit.Service;

public class CreditApprovedLow : ICreditApproved
{
    public double GetCreditApproved(CreditApplication creditApplication)
    {
        // Calculate approval limit.
        // Hard coded to 50% of salary for demo purposes.

        return creditApplication.Salary * 0.5;
    }
}