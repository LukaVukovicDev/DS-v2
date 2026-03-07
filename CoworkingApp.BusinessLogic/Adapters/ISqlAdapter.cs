using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Adapters
{
    public interface ISqlAdapter
    {
        string GetCurrentDateTime();
        string GetDatePart(string column);
        string GetMonthPart(string column);
        string GetYearPart(string column);
        string GetTopN(int n);
        string GetLimitN(int n);
    }
}
