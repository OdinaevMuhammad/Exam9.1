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
public class JobHistoryService
{
     private readonly DataContext _context; 

    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IMapper _mapper;

    public JobHistoryService(DataContext context, IWebHostEnvironment env, IMapper mapper)
    {
            _mapper = mapper;
        _context = context;
        _hostEnvironment = env;
    }
    

    public async Task<Response<List<GetJobHistory>>> GetJobHistory()
    {
        var list = await _context.JobHistories.Select(c => new GetJobHistory()
        {
            EmployeeId = c.EmployeeId,
            JobName = c.Job.JobName,
            StartDate = c.StartDate,
            EndDate = c.EndDate
        }).ToListAsync();

        return new Response<List<GetJobHistory>>(list);
            
    }
    
        
       

    public async Task<Response<AddJobHistory>> InsertJobHistory(AddJobHistory jobhistory)
    {
        var newJobHistory = _mapper.Map<JobHistory>(jobhistory);
     
         _context.JobHistories.Add(newJobHistory);

         await _context.SaveChangesAsync();

         return new Response<AddJobHistory>(jobhistory);
    }
        public async Task<Response<AddJobHistory>> UpdateJobHistory(AddJobHistory jobhistory)
        {
            var find = await _context.JobHistories.FindAsync(jobhistory.EmployeeId);
             find.EmployeeId = jobhistory.EmployeeId;
            find.StartDate = jobhistory.StartDate;
            find.EndDate = jobhistory.EndDate;
            find.JobId = jobhistory.JobId;

            await _context.SaveChangesAsync();

            return new Response<AddJobHistory>(jobhistory);
        }
      public async Task<Response<string>> DeleteJobHistory(int id)
        {      
        var find = await _context.JobHistories.FindAsync(id);
        _context.JobHistories.Remove(find);
        await _context.SaveChangesAsync();
           if(find.EmployeeId > 0 )  return new Response<string>("JobHistory deleted successfully");

        
               return new Response<string>(HttpStatusCode.BadRequest, "JobHistory not found");
        }
}
    
