using MovieCRUD.Authentication.Notifications.Messages;

namespace MovieCRUD.Authentication.Notifications.Interfaces
{
    public interface ISmsService
    {
        void SendMessage(SmsMessage message);
    }
}
