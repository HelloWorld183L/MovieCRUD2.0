using MovieCRUD.Infrastructure.Notifications.Messages;

namespace MovieCRUD.Infrastructure.Notifications.Services.Interfaces
{
    public interface ISmsService
    {
        void SendMessage(SmsMessage message);
    }
}
