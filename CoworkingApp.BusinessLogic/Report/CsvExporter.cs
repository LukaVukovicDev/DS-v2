using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace CoworkingApp.BusinessLogic.Report
{
    public class CsvExporter
    {
        public string Export(List<ReportItem> report, string folderPath = null, string fileName = null)
        {
            folderPath = string.IsNullOrWhiteSpace(folderPath)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reports")
                : folderPath;

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            fileName = string.IsNullOrWhiteSpace(fileName)
                ? $"report_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.csv"
                : fileName;

            var path = Path.Combine(folderPath, fileName);

            using (var writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("FullName,Email,MembershipType,TotalReservations,TotalHours,UniqueResources,UniqueLocations");

                foreach (var r in report)
                {
                    writer.WriteLine(string.Join(",",
                        Escape(r.FullName),
                        Escape(r.Email),
                        Escape(r.MembershipTypeName),
                        r.TotalReservations.ToString(CultureInfo.InvariantCulture),
                        r.TotalHours.ToString("0.##", CultureInfo.InvariantCulture),
                        r.UniqueResources.ToString(CultureInfo.InvariantCulture),
                        r.UniqueLocations.ToString(CultureInfo.InvariantCulture)));
                }
            }

            return path;
        }

        private static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            var escaped = value.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
    }
}
