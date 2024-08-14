using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hotel.Reporting.Domain.DataObjects;
using Hotel.Reporting.Domain.Interfaces;

namespace Hotel.Reporting.Api.Controllers;

/// <summary>
/// Web API контроллер отчетов
/// </summary>
/// <param name="reportService">Сервис для рбаоты с отчетами</param>
[ApiController]
[Route("api/[controller]")]
public class ReportsController(IReportService reportService) : ControllerBase
{
    /// <summary>
    /// Получить все отчеты
    /// </summary>
    /// <returns>Коллекция с DTO отчетов</returns>
    [HttpGet]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IEnumerable<ReportDTO>> GetAll()
    {
        return await reportService.GetAll();
    }

    /// <summary>
    /// Получить отчет по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор отчета</param>
    /// <returns>DTO отчета</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<ReportDTO> GetById(Guid id)
    {
        return await reportService.GetById(id);
    }

    /// <summary>
    /// Создать новый отчет
    /// </summary>
    /// <param name="data">Данные для создания нового отчета</param>
    /// <returns>DTO созданного отчета, обернутый в CreatedAtAction</returns>
    [HttpPost]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<ActionResult<ReportDTO>> CreateReport(ReportData data)
    {
        var report = await reportService.CreateReport(data);
        
        return CreatedAtAction(nameof(GetById), new { id = report.Id }, report);
    }

    /// <summary>
    /// Удалить отчет по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор отчета</param>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task DeleteReport(Guid id)
    {
        await reportService.DeleteReport(id);
    }
}