using SmartSwitchWeb.Database;
using System;
using System.Collections.Generic;

namespace SmartSwitchWeb.Data
{
    
    public enum DeviceStatus {Offline, Online, Unknown, RunningScheduled,RunningManuel, StoppedManual }
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
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Guid { get; private set; }
        public string PicturePath { get;} 
        public DateTime LastOnline { get;private set;}
        public DeviceStatus Status { get; private set; }
        public static List<Device> DeviceList
        {
            get
            {
                if (_deviceList == null)
                {
                    using (DeviceContext context = new DeviceContext())
                    {
                        List<Device> _list;
                        _list = context.GetAll();
                        Console.WriteLine(_list.ToString());
                        Console.WriteLine(_list.Count);
                        _deviceList = _list;
                        
                        
                        return _list;
                    }
                }
                return _deviceList;
            }
        }
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
        public static void addToList(Device device)
        {
            _deviceList.Add(device);
        }
    }
}
