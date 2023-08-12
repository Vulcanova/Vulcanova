namespace Vulcanova.Features.Messages.Compose;

public interface IMessageSender
{
    Task SendMessageAsync(int accountId, AddressBookEntry recipient, string subject, string message, Guid? threadKey);
}