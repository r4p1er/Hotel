using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reporting.Domain.DataObjects;
using Reporting.Domain.Interfaces;

namespace Reporting.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IEnumerable<ReportDTO>> GetAll()
    {
        return await _reportService.GetAll();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<ReportDTO> GetById(Guid id)
    {
        return await _reportService.GetById(id);
    }

    [HttpPost]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<ActionResult<ReportDTO>> CreateReport(ReportData data)
    {
        var report = await _reportService.CreateReport(data);
        
        return CreatedAtAction(nameof(GetById), new { id = report.Id }, report);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task DeleteReport(Guid id)
    {
        await _reportService.DeleteReport(id);
    }
}