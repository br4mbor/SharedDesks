using System.ServiceModel;
using Abb.Euopc.SharedDesks.Domain.Common;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Common;
using Abb.Euopc.SharedDesks.Infrastructure.Options;
using MailServiceReference;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Abb.Euopc.SharedDesks.Infrastructure.Services;

internal sealed class EmailNotificationService : IEmailNotificationService
{
    private readonly EmailNotificationServiceOptions _options;
    private readonly ILogger<EmailNotificationService> _logger;

    public EmailNotificationService(IOptions<EmailNotificationServiceOptions> options, ILogger<EmailNotificationService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public Task SendNotificationAsync(EmailMessage message) => SendNotificationsAsync(new[] { message });

    public async Task SendNotificationsAsync(IEnumerable<EmailMessage> messages)
    {
        if (messages is null)
        {
            _logger.LogError($"{nameof(EmailNotificationService)}: Notification messages null.");

            return;
        }

        ServiceSoapClient? client = null;

        try
        {
            client = new ServiceSoapClient(ServiceSoapClient.EndpointConfiguration.ServiceSoap);

            if (client.State == CommunicationState.Faulted)
            {
                client.Abort();
            }

            await client.OpenAsync();

            if (client.State == CommunicationState.Opened)
            {
                foreach (var message in messages)
                {
                    if (string.IsNullOrEmpty(message.Cc))
                    {
                        await client.SendMailToAddrAsync(_options.FromAddress, message.To, message.Subject, message.Message);
                    }
                    else
                    {
                        await client.SendMailToAddrCCAsync(_options.FromAddress, message.To, message.Cc, message.Subject, message.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(EmailNotificationService)}: An error has occurred while sending notification.");
        }
        finally
        {
            if (client is not null)
            {
                if (client.State != CommunicationState.Closed)
                {
                    client.Abort();
                }

                await client.CloseAsync();
            }
        }
    }
}
