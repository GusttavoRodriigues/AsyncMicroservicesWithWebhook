using EmailNotificationWebhook.Consumer;
using EmailNotificationWebhook.Service;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpClient();

//Configurando Rabbit MQ

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<WebhookConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        config.Host("rabbitmq://localhost", c =>
        {
            c.Username("guest");
            c.Password("guest");
        });
        config.ReceiveEndpoint("email-webhook-queue", e =>
        {
            e.ConfigureConsumer<WebhookConsumer>(context);
        });
    });
});

var app = builder.Build();

app.MapPost("/email-webhook", ([FromBody] EmailDTO email, IEmailService emailService) =>
{
    string result = emailService.SendEmail(email);
    return Task.FromResult(result);

});

app.Run();
