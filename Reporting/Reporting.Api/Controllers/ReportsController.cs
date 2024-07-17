using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reporting.Domain.DataObjects;
using Reporting.Domain.Interfaces;

namespace Reporting.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(IReportService reportService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IEnumerable<ReportDTO>> GetAll()
    {
        return await reportService.GetAll();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<ReportDTO> GetById(Guid id)
    {
        return await reportService.GetById(id);
    }

    [HttpPost]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<ActionResult<ReportDTO>> CreateReport(ReportData data)
    {
        var report = await reportService.CreateReport(data);
        
        return CreatedAtAction(nameof(GetById), new { id = report.Id }, report);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task DeleteReport(Guid id)
    {
        await reportService.DeleteReport(id);
    }
}