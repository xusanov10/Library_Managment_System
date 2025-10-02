using AutoMapper;
using Libray_Managment_System.DtoModels.ReportModels;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.ReportService;

public class ReportService : IReportService
{
    private readonly LibraryManagmentSystemContext _context;
    private readonly IMapper _mapper;

    public ReportService(LibraryManagmentSystemContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReportResponseDto>> GetAllAsync()
    {
        var reports = await _context.Reports.ToListAsync();
        return _mapper.Map<IEnumerable<ReportResponseDto>>(reports);
    }

    public async Task<ReportResponseDto?> GetByIdAsync(int id)
    {
        var report = await _context.Reports.FindAsync(id);
        return report == null ? null : _mapper.Map<ReportResponseDto>(report);
    }

    public async Task<ReportResponseDto> CreateAsync(ReportCreateDto dto)
    {
        var report = _mapper.Map<Report>(dto);
        report.Generatedat = DateTime.UtcNow;
        _context.Reports.Add(report);
        await _context.SaveChangesAsync();
        return _mapper.Map<ReportResponseDto>(report);
    }

    public async Task<ReportResponseDto?> UpdateAsync(int id, ReportUpdateDto dto)
    {
        var report = await _context.Reports.FindAsync(id);
        if (report == null) return null;

        _mapper.Map(dto, report);
        await _context.SaveChangesAsync();
        return _mapper.Map<ReportResponseDto>(report);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var report = await _context.Reports.FindAsync(id);
        if (report == null) return false;

        _context.Reports.Remove(report);
        await _context.SaveChangesAsync();
        return true;
    }
}