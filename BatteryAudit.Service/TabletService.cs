namespace BatteryAudit.Service
{
    using BatteryAudit.Models;
    using BatteryAudit.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TabletService : ITabletService
    {
        private readonly ITabletRepository _tabletRepository;
        public TabletService(ITabletRepository tabletRepository)
        {
            _tabletRepository = tabletRepository;
        }

        public TabletUsage GetTabletBatteryUsage(string serialNumber)
        {
            //tablet found or not found to be verified
            var tabletReadings = _tabletRepository.GetTabletData(serialNumber);
            return new TabletUsage()
            {
                SerialNumber = serialNumber,
                Average = tabletReadings?.Count() > 1 ? CalculateTabletAverageUsagePerDay(tabletReadings.OrderBy(x => x.Timestamp)) : "Unknown"
            };
        }

        public IEnumerable<TabletUsage> GetAcadamyTabletsBatteryUsage(int acadamyId)
        {
            //acadamy found or not found to be verified
            //Assuming proper relaions exists between acadamy and tablet tables
            var tabletsInfo = _tabletRepository.GetTabletDataForAcadamy(acadamyId);
            List<TabletUsage> tabletUsages = null;
            if (tabletsInfo?.Count() > 0)
            {
                tabletUsages = new List<TabletUsage>();
                foreach (var tabletReadings in tabletsInfo.GroupBy(x => x.SerialNumber))
                {
                    tabletUsages.Add(new TabletUsage()
                    {
                        SerialNumber = tabletReadings.Key,
                        Average = tabletReadings?.Count() > 1 ? CalculateTabletAverageUsagePerDay(tabletReadings.OrderBy(x => x.Timestamp)) : "Unknown"
                    });
                }
            }
            return tabletUsages;
        }

        private static string CalculateTabletAverageUsagePerDay(IEnumerable<TabletReading> tabletReadings)
        {
            decimal usage = 0;
            DateTime startDateReading = tabletReadings.FirstOrDefault()?.Timestamp ?? default; //Assuming Timestamp is available all the time
            DateTime endDateReading = default;
            for (int i = 1; i < tabletReadings.Count(); i++)
            {
                TabletReading current = tabletReadings.ElementAtOrDefault(i - 1);
                TabletReading next = tabletReadings.ElementAtOrDefault(i);
                usage += current.BatteryLevel - next.BatteryLevel > 0 ? current.BatteryLevel - next.BatteryLevel : 0;
                endDateReading = current.BatteryLevel - next.BatteryLevel > 0 ? next.Timestamp : endDateReading;
            }
            //Just for the data representation while seeing the results in swagger considered avaerage as string
            return endDateReading == default ? "0" : Math.Round(usage * 24 * 60 * 100 / (Math.Round(Convert.ToDecimal((endDateReading - startDateReading).TotalMinutes), MidpointRounding.AwayFromZero)), 2, MidpointRounding.ToEven).ToString();
        }
    }
}
