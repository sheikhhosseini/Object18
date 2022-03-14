
namespace Core.Shared.Email;

public interface IMailSender
{
    void Send(string to, string subject, string body);
}