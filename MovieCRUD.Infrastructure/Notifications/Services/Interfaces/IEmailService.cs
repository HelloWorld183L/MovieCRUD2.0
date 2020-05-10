using MovieCRUD.Infrastructure.Notifications.Messages;

namespace MovieCRUD.Infrastructure.Notifications.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailMessage message);
    }
}
