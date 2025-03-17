using MassTransit;
using Shared.DTOs;

namespace EmailNotificationWebhook.Consumer
{
    public class WebhookConsumer(HttpClient client) : IConsumer<EmailDTO>
    {
        public async Task Consume(ConsumeContext<EmailDTO> context)
        {
            var result = await client.PostAsJsonAsync("https://localhost:7157/email-webhook",
                        new EmailDTO(context.Message.Title, context.Message.Content));

            result.EnsureSuccessStatusCode();
        }
    }
}
