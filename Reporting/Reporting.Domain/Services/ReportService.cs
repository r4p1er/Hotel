using System.Text.Json;
using System.Text.Json.Nodes;
using FluentValidation;
using Hotel.Shared.Exceptions;
using Hotel.Shared.MessageContracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Reporting.Domain.DataObjects;
using Reporting.Domain.Entities;
using Reporting.Domain.Interfaces;

namespace Reporting.Domain.Services;

public class ReportService(IReportsRepository repository,
    IValidator<ReportData> validator,
    IRequestClient<SelectBookingTickets> bookingClient,
    IRequestClient<SelectRoomNames> managingClient) : IReportService
{
    public async Task<IEnumerable<ReportDTO>> GetAll()
    {
        return await repository.FindAll().Select(x => new ReportDTO(x)).ToListAsync();
    }

    public async Task<ReportDTO> GetById(Guid id)
    {
        var report = await repository.FindByIdAsync(id);

        if (report == null) throw new NotFoundException("Report not found");

        return new ReportDTO(report);
    }

    public async Task<ReportDTO> CreateReport(ReportData data)
    {
        await validator.ValidateAndThrowAsync(data);

        var report = new Report
        {
            Id = Guid.NewGuid(),
            Summary = data.Summary,
            From = data.From,
            To = data.To
        };

        var bookingResponse = await bookingClient.GetResponse<BookingTicketsResult>(new SelectBookingTickets
        {
            From = data.From,
            To = data.To
        });
        
        var roomsId = bookingResponse.Message.BookingTickets.Select(x => x.RoomId).ToList();

        var managingResponse = await managingClient.GetResponse<RoomNamesResult>(new SelectRoomNames
        {
            Guids = roomsId
        });
        
        var result = new List<JsonObject>();

        foreach (var ticket in bookingResponse.Message.BookingTickets)
        {
            var item = new JsonObject();
            item["Name"] = managingResponse.Message.Rooms.FirstOrDefault(x => x.Id == ticket.RoomId)!.Name;
            var date = data.From;

            while (date <= data.To)
            {
                item[date.ToString("dd.MM.yy")] =
                    ticket.From.Date <= date.Date && date.Date <= ticket.To.Date ? "Забронирован" : "Свободен";
                date = date.AddDays(1);
            }
            
            result.Add(item);
        }

        report.Data = JsonSerializer.Serialize(result);
        await repository.AddReportAsync(report);

        return new ReportDTO(report);
    }

    public async Task DeleteReport(Guid id)
    {
        var report = await repository.FindByIdAsync(id);

        if (report == null) throw new NotFoundException("Report not found");

        await repository.RemoveReportAsync(report);
    }
}