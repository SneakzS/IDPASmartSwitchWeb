namespace SmartSwitchWeb {
    public enum RPIFlag
        {
            Enforce = 1 << 0,

            IsEnabled = 1 << 1,

            IsUIConnected = 1 << 2,

            ProviderClientOK = 1 << 3
        }
}