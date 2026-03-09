using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using CoworkingApp.BusinessLogic.Database;

namespace CoworkingApp.BusinessLogic.Report
{
    public class ReportService
    {
        private readonly IDbConnection _connection;

        public ReportService()
        {
            _connection = DatabaseConnection.GetInstance(null).GetConnection();
        }

        public List<ReportItem> GetMonthlyReport()
        {
            var now = DateTime.Now;
            var from = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
            var to = from.AddMonths(1);
            return GetReportByDateRange(from, to);
        }

        public List<ReportItem> GetReportByDateRange(DateTime from, DateTime to)
        {
            const string sql = @"
                SELECT
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    mt.Name AS MembershipTypeName,
                    r.StartDateTime,
                    r.EndDateTime,
                    r.ResourceId,
                    res.LocationId
                FROM Reservations r
                INNER JOIN Users u ON r.UserId = u.Id
                LEFT JOIN MembershipTypes mt ON u.MembershipTypeId = mt.Id
                INNER JOIN Resources res ON r.ResourceId = res.Id
                WHERE r.StartDateTime >= @From
                  AND r.StartDateTime < @To
                  AND r.Status <> 'Cancelled'";

            var rows = _connection.Query(sql, new { From = from, To = to });
            var grouped = new Dictionary<string, AggregationBucket>();

            foreach (var row in rows)
            {
                string key = row.Email != null ? (string)row.Email : $"{row.FirstName}_{row.LastName}";

                if (!grouped.ContainsKey(key))
                {
                    grouped[key] = new AggregationBucket
                    {
                        Item = new ReportItem
                        {
                            FirstName = row.FirstName,
                            LastName = row.LastName,
                            Email = row.Email,
                            MembershipTypeName = row.MembershipTypeName,
                            TotalReservations = 0,
                            TotalHours = 0,
                            UniqueResources = 0,
                            UniqueLocations = 0
                        }
                    };
                }

                var bucket = grouped[key];
                bucket.Item.TotalReservations++;

                DateTime start = row.StartDateTime;
                DateTime end = row.EndDateTime;
                bucket.Item.TotalHours += (end - start).TotalHours;
                bucket.ResourceIds.Add((int)row.ResourceId);
                bucket.LocationIds.Add((int)row.LocationId);
            }

            foreach (var bucket in grouped.Values)
            {
                bucket.Item.UniqueResources = bucket.ResourceIds.Count;
                bucket.Item.UniqueLocations = bucket.LocationIds.Count;
            }

            return grouped.Values
                .Select(x => x.Item)
                .OrderByDescending(x => x.TotalHours)
                .ThenBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToList();
        }

        private class AggregationBucket
        {
            public ReportItem Item { get; set; }
            public HashSet<int> ResourceIds { get; } = new HashSet<int>();
            public HashSet<int> LocationIds { get; } = new HashSet<int>();
        }
    }
}
