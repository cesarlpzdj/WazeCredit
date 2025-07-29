using WazeCredit.Models;

namespace WazeCredit.Service;

public class CreditApprovedHigh : ICreditApproved
{
    public double GetCreditApproved(CreditApplication creditApplication)
    {
        // Calculate approval limit.
        // Hard coded to 30% of salary for demo purposes.

        return creditApplication.Salary * 0.3;
    }
}