namespace Lykke.Service.PushNotifications.Contract.Commands
{
    public class WakeUpNotificationCommand
    {
        public string Tag { get; set; }
        public string Message { get; set; }
    }
}
