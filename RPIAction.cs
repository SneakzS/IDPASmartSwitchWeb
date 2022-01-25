namespace SmartSwitchWeb
{
    public enum RPIAction
    {
        SetWorkload = 1,
        GetWorkloads = 2,
        DeleteWorkload = 3,
        SetFlags = 4,
        GetFlags = 5,
        Helo = 6,
        GetWorkloadEvents = 7,
        GetSensorSamples = 8,
        GetLogEntries = 9,

        NotifyError = 101,
        NotifyWorkloadCreated = 102,
        NotifyNoContent = 103,
        NotifyWorkloadEvents = 104,
        NotifyWorkloads = 105,
        NotifySensorSamples       = 106,
        NotifyLogEntries = 107,
        
    }

    public static class RPIActionExtensions
    {
        public static int ToID(this RPIAction a)
        {
            return (int)a;
        }
    }
}