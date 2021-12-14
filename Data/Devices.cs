using SmartSwitchWeb.Database;
using System;
using System.Collections.Generic;

namespace SmartSwitchWeb.Data
{
    
    public enum DeviceStatus {Offline, Online, Unknown, RunningScheduled,RunningManual, StoppedManual }
    public class Device
    {
        public int DeviceID { get; set; }
        private static List<Device> _deviceList { get; set; }
            /*
        {
            get
            {
                if (DeviceList == null)
                {
                    using (DeviceContext context = new DeviceContext())
                    {
                        List<Device> _list;
                        _list = context.GetAll();
                        return _list;
                    }
                }
                else
                {
                    return DeviceList;
                }
            }
        }*/
        public string Name { get; set; }
        public string Description { get;  set; }
        public string Guid { get; set; }
        public string PicturePath { get;} 
        public DateTime LastOnline { get; set;}
        public DeviceStatus Status { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public Device() {}
        public Device(string name, string description,string guid, DeviceStatus status )
        {
            Name = name;
            Description = description;
            Guid = guid;
            Status = status;
            LastOnline = DateTime.Now;
        }
        public void SetGuid(string guid) { Guid = guid; }
        public void SetStatus(DeviceStatus status) { Status = status; LastOnline = DateTime.Now; }
        public static Device GetDevice(string guid)
        {
            return _deviceList.Find(x => x.Guid == guid);
        }
    }
}
