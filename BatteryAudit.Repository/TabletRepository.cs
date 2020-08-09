namespace BatteryAudit.Repository
{
    using BatteryAudit.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class TabletRepository : ITabletRepository
    {
        private readonly IEnumerable<TabletReading> tabletInfos;

        public TabletRepository()
        {
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"Data\battery.json");
            string json = File.ReadAllText(filePath);
            tabletInfos = JsonConvert.DeserializeObject<IEnumerable<TabletReading>>(json);
        }

        public IEnumerable<TabletReading> GetTabletData(string serialNumber)
        {
            return tabletInfos.Where(x => x.SerialNumber.Equals(serialNumber));
        }

        public IEnumerable<TabletReading> GetTabletDataForAcadamy(int acadamyId)
        {
            return tabletInfos.Where(x => x.AcademyId.Equals(acadamyId));
        }
    }
}
