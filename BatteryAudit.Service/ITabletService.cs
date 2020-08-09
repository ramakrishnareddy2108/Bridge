namespace BatteryAudit.Service
{
    using BatteryAudit.Models;
    using System.Collections.Generic;

    public interface ITabletService
    {
        TabletUsage GetTabletBatteryUsage(string serialNumber);
        IEnumerable<TabletUsage> GetAcadamyTabletsBatteryUsage(int acadamyId);
    }
}
