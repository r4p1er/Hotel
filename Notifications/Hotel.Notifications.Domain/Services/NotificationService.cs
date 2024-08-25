using Hotel.Notifications.Domain.Abstractions;
using Hotel.Notifications.Domain.Models;
using Hotel.Shared.MessageContracts;
using MassTransit;
using MimeKit;

namespace Hotel.Notifications.Domain.Services;

/// <inheritdoc cref="INotificationService"/>
public class NotificationService(
    IEmailService email, 
    IRequestClient<SelectUserData> identityClient,
    IRequestClient<SelectRoomNames> managingClient,
    NotificationServiceOptions options) : INotificationService
{
    /// <summary>
    /// Подготовить основу электронного пиьсма
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Электронное письмо</returns>
    private async Task<MimeMessage> PrepareMessage(Guid userId)
    {
        var identityResponse = await identityClient.GetResponse<UserDataResult>(new SelectUserData
        {
            Id = userId
        });
        
        var targetMailboxName = identityResponse.Message.Name;
        var targetEmail = identityResponse.Message.Email;
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(options.MailboxName, options.Email));
        message.To.Add(new MailboxAddress(targetMailboxName, targetEmail));

        return message;
    }

    /// <summary>
    /// Получить название номера отеля по его идентификатору
    /// </summary>
    /// <param name="roomId">Идентификатор номера отеля</param>
    /// <returns>Название номера отеля</returns>
    private async Task<string> GetRoomName(Guid roomId)
    {
        var managingResponse = await managingClient.GetResponse<RoomNamesResult>(new SelectRoomNames
        {
            Guids = new List<Guid> { roomId }
        });
        
        return managingResponse.Message.Rooms.First().Name;
    }
    
    /// <inheritdoc cref="INotificationService.TicketCreateNotify"/>
    public async Task TicketCreateNotify(BookingTicketCreated publishedEvent)
    {
        var roomName = await GetRoomName(publishedEvent.RoomId);

        var message = await PrepareMessage(publishedEvent.UserId);
        message.Subject = "Booking ticket created";
        message.Body = new TextPart("plain")
        {
            Text =
                $"You have successfully created booking ticket. Room: {roomName}, from {publishedEvent.From} to {publishedEvent.To}, price: {publishedEvent.Price}. Booking ticket identifier: {publishedEvent.Id}"
        };

        await email.SendEmailAsync(message);
    }

    /// <inheritdoc cref="INotificationService.TicketCancelNotify"/>
    public async Task TicketCancelNotify(BookingTicketCanceled publishedEvent)
    {
        var roomName = await GetRoomName(publishedEvent.RoomId);

        var message = await PrepareMessage(publishedEvent.UserId);
        message.Subject = "Booking ticket canceled";
        message.Body = new TextPart("plain")
        {
            Text =
                $"Your booking ticket has been canceled. Room: {roomName}. Booking ticket identifier: {publishedEvent.Id}"
        };

        await email.SendEmailAsync(message);
    }

    /// <inheritdoc cref="INotificationService.StatusChangeNotify"/>
    public async Task StatusChangeNotify(ConfirmationStatusChanged publishedEvent)
    {
        var roomName = await GetRoomName(publishedEvent.RoomId);
        var statusString = publishedEvent.ConfirmationStatus ? "confirmed" : "not confirmed";

        var message = await PrepareMessage(publishedEvent.UserId);
        message.Subject = "Booking ticket confirmation status changed";
        message.Body = new TextPart("plain")
        {
            Text =
                $"Confirmation status of the booking ticket changed: {statusString}. Room: {roomName}. Booking ticket identifier: {publishedEvent.Id}"
        };

        await email.SendEmailAsync(message);
    }
}