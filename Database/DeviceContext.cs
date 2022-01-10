using SmartSwitchWeb.Data;
using System.Collections.Generic;
using System.Linq;

namespace SmartSwitchWeb.Database
{
    public class DeviceContext : Database<Device>
    {
        public Device GetDevice(string guid) { return DbSet.SingleOrDefault(d => d.Guid == guid); }
    }
}
