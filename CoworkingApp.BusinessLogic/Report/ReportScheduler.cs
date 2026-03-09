using System;
using System.IO;
using System.Timers;

namespace CoworkingApp.BusinessLogic.Report
{
    public class ReportScheduler
    {
        private readonly Timer _timer;
        private readonly ReportService _reportService;
        private readonly CsvExporter _exporter;
        private readonly string _folderPath;
        private readonly double _intervalMs;

        public string LastGeneratedFilePath { get; private set; }

        public ReportScheduler(double intervalMs = 86400000, string folderPath = null)
        {
            _reportService = new ReportService();
            _exporter = new CsvExporter();
            _intervalMs = intervalMs;
            _folderPath = string.IsNullOrWhiteSpace(folderPath)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reports")
                : folderPath;

            _timer = new Timer(intervalMs);
            _timer.Elapsed += GenerateReport;
            _timer.AutoReset = true;
        }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();

        private void GenerateReport(object sender, ElapsedEventArgs e)
        {
            var to = DateTime.Now;
            var from = to.AddMilliseconds(-_intervalMs);
            var report = _reportService.GetReportByDateRange(from, to);
            var fileName = $"report_{from:yyyy_MM_dd_HH_mm}_{to:yyyy_MM_dd_HH_mm}.csv";
            LastGeneratedFilePath = _exporter.Export(report, _folderPath, fileName);
        }
    }
}
