namespace CoworkingApp.BusinessLogic.Adapters
{
    public class MySqlAdapter : ISqlAdapter
    {
        public string GetCurrentDateTime() => "NOW()";
        public string GetDatePart(string column) => $"DATE({column})";
        public string GetMonthPart(string column) => $"MONTH({column})";
        public string GetYearPart(string column) => $"YEAR({column})";
        public string GetTopN(int n) => string.Empty;
        public string GetLimitN(int n) => $"LIMIT {n}";
    }
}