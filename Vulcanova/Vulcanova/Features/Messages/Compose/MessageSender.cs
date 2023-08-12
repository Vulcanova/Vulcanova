using Vulcanova.Core.Uonet;
using Vulcanova.Features.Auth;
using Vulcanova.Uonet.Api.Common.Models;
using Vulcanova.Uonet.Api.MessageBox;

namespace Vulcanova.Features.Messages.Compose;

public class MessageSender : IMessageSender
{
    private readonly IApiClientFactory _apiClientFactory;
    private readonly IAccountRepository _accountRepository;

    public MessageSender(IAccountRepository accountRepository, IApiClientFactory apiClientFactory)
    {
        _accountRepository = accountRepository;
        _apiClientFactory = apiClientFactory;
    }

    public async Task SendMessageAsync(int accountId, AddressBookEntry recipient, string subject, string message,
        Guid? threadKey)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);

        var apiClient = await _apiClientFactory.GetAuthenticatedAsync(account);

        var isNewThread = threadKey == null;
        threadKey ??= Guid.NewGuid();

        var request = new SendMessageRequest
        {
            Id = Guid.NewGuid(),
            GlobalKey = isNewThread ? threadKey.Value : Guid.NewGuid(),
            Partition = account.Partition,
            ThreadKey = threadKey.Value,
            Subject = subject,
            Content = PrepareMessageForSending(message),
            Status = 1,
            Owner = recipient.MessageBoxId,
            DateSent = DateTimeInfo.FromDateTime(DateTime.Now),
            DateRead = null,
            Sender = new SendMessageRequestCorrespondent
            {
                Id = "0",
                Partition = account.Partition,
                Owner = recipient.MessageBoxId,
                GlobalKey = recipient.MessageBoxId,
                HasRead = 0
            },
            Receiver = new List<SendMessageRequestCorrespondent>
            {
                new SendMessageRequestCorrespondent
                {
                    Id = $"{recipient.MessageBoxId}-{recipient.Id}",
                    Partition = account.Partition,
                    Owner = recipient.MessageBoxId,
                    GlobalKey = recipient.Id,
                    HasRead = 0
                }
            },
            Attachments = new List<Attachment>()
        };

        await apiClient.PostAsync(SendMessageRequest.ApiEndpoint, request);
    }

    private static string PrepareMessageForSending(string message) =>
        string.Join(string.Empty, message.Split(Environment.NewLine)
            .Select(line => line != string.Empty ? $"<p>{line}</p>" : "<br>"));
}