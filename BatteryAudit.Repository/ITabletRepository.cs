namespace BatteryAudit.Repository
{
    using BatteryAudit.Models;
    using System.Collections.Generic;

    public interface ITabletRepository
    {
        IEnumerable<TabletReading> GetTabletData(string serialNumber);
        IEnumerable<TabletReading> GetTabletDataForAcadamy(int acadamyId);
    }
}
