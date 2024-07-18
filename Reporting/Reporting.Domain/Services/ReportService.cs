using System.Text.Json;
using System.Text.Json.Nodes;
using FluentValidation;
using Hotel.Shared.Exceptions;
using Hotel.Shared.Interfaces;
using Hotel.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Reporting.Domain.DataObjects;
using Reporting.Domain.Entities;
using Reporting.Domain.Interfaces;

namespace Reporting.Domain.Services;

public class ReportService(
    IReportsRepository repository,
    IValidator<ReportData> validator,
    IRabbitService rabbit) : IReportService
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

        var bookingResponse = await rabbit.RequestMessageAsync(new RabbitMessage
        {
            Id = Guid.NewGuid(),
            Name = "BookingDataRequired",
            Receiver = "Booking",
            Data = JsonSerializer.Serialize(new { From = data.From, To = data.To })
        });

        if (bookingResponse == null) throw new ArgumentNullException(nameof(RabbitMessage));

        var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var bookingList = JsonSerializer.Deserialize<List<BookingData>>(bookingResponse.Data, serializerOptions);
        var roomsId = bookingList!.Select(x => x.RoomId).ToList();

        var managingResponse = await rabbit.RequestMessageAsync(new RabbitMessage
        {
            Id = Guid.NewGuid(),
            Name = "RoomsNameRequired",
            Receiver = "Managing",
            Data = JsonSerializer.Serialize(roomsId)
        });
        
        if (managingResponse == null) throw new ArgumentNullException(nameof(RabbitMessage));

        var roomList = JsonSerializer.Deserialize<List<RoomData>>(managingResponse.Data, serializerOptions);
        var result = new List<JsonObject>();

        foreach (var bookingData in bookingList!)
        {
            var item = new JsonObject();
            item["Name"] = roomList!.FirstOrDefault(x => x.Id == bookingData.RoomId)!.Name;
            var date = data.From;

            while (date <= data.To)
            {
                item[date.ToString("dd.MM.yy")] =
                    bookingData.From.Date <= date.Date && date.Date <= bookingData.To.Date ? "Забронирован" : "Свободен";
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