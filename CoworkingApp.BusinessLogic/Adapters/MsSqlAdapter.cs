namespace CoworkingApp.BusinessLogic.Adapters
{
    public class MsSqlAdapter : ISqlAdapter
    {
        public string GetCurrentDateTime() => "GETDATE()";
        public string GetDatePart(string column) => $"CAST({column} AS DATE)";
        public string GetMonthPart(string column) => $"MONTH({column})";
        public string GetYearPart(string column) => $"YEAR({column})";
        public string GetTopN(int n) => $"TOP {n}";
        public string GetLimitN(int n) => string.Empty;
    }
}
