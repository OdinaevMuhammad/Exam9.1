using System.IO.Enumeration;
namespace Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Domain.Dtos;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using System.Net;
using Domain.Wrapper;
public class JobTimeHistoryService
{
    private readonly DataContext _context;

    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IMapper _mapper;

    public JobTimeHistoryService(DataContext context, IWebHostEnvironment env, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
        _hostEnvironment = env;
    }

    public async Task<Response<TimeSpan>> GetAvgById(int id, DateTime start, DateTime end)
    {
        var list = (from j in _context.JobTimeHistories
                    where j.EmployeeId == id && start <= j.TimeOfBeingLate && j.TimeOfBeingLate >= end
                    select new GetAvg()
                    {
                        TimeOfBeingLate = j.TimeOfBeingLate.TimeOfDay
                    }).ToList();
      
            TimeSpan total = default(TimeSpan);

            var sortedDates = list.OrderBy(x => x.TimeOfBeingLate);

            foreach (var dateTime in sortedDates)
            {
                total += dateTime.TimeOfBeingLate;
            }
            var time = TimeSpan.FromMilliseconds(total.TotalMilliseconds / sortedDates.Count());
            
            return new Response<TimeSpan>(time);
        }





    


    public async Task<Response<List<GetJobTimeHistory>>> GetJobTimeHistory()
    {
        var list = await _context.JobTimeHistories.Select(c => new GetJobTimeHistory()
        {
            JobTimeHistoryId = c.JobTimeHistoryId,
            EmployeeId = c.EmployeeId,
            FirstName = c.Employee.FirstName,
            LastName = c.Employee.LastName,
            StartJobTime = c.StartJobTime,
            TimeOfBeingLate = c.TimeOfBeingLate
        }).ToListAsync();



        return new Response<List<GetJobTimeHistory>>(list);

    }




    public async Task<Response<AddJobTimeHistory>> InsertJobTimeHistory(AddJobTimeHistory jobTimeHistory)
    {
        var newJobTimeHistory = _mapper.Map<JobTimeHistory>(jobTimeHistory);

        _context.JobTimeHistories.Add(newJobTimeHistory);




        await _context.SaveChangesAsync();


        return new Response<AddJobTimeHistory>(jobTimeHistory);


    }
    public async Task<Response<AddJobTimeHistory>> UpdateJobTimeHistory(AddJobTimeHistory jobTimeHistory)
    {
        var find = await _context.JobTimeHistories.FindAsync(jobTimeHistory.EmployeeId);
        find.EmployeeId = jobTimeHistory.EmployeeId;
        find.StartJobTime = jobTimeHistory.StartJobTime;
        find.TimeOfBeingLate = jobTimeHistory.TimeOfBeingLate;

        await _context.SaveChangesAsync();

        return new Response<AddJobTimeHistory>(jobTimeHistory);
    }
    public async Task<Response<string>> DeleteJobTimeHistory(int id)
    {
        var find = await _context.JobTimeHistories.FindAsync(id);
        _context.JobTimeHistories.Remove(find);
        await _context.SaveChangesAsync();
        if (find.JobTimeHistoryId > 0) return new Response<string>("JobTimeHistory deleted successfully");


        return new Response<string>(HttpStatusCode.BadRequest, "JobTimeHistory not found");
    }
}

