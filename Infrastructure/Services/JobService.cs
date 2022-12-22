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
public class JobService
{
    private readonly DataContext _context; 

    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IMapper _mapper;

    public JobService(DataContext context, IWebHostEnvironment env, IMapper mapper)
    {
            _mapper = mapper;
        _context = context;
        _hostEnvironment = env;
    }
    

    public async Task<Response<List<AddJobDto>>> GetJobs()
    {
        var list = _mapper.Map<List<AddJobDto>>(await _context.Jobs.ToListAsync());


        return new Response<List<AddJobDto>>(list);
            
    }
    
        
       

    public async Task<Response<AddJobDto>> InsertJob(AddJobDto job)
    {
        var newJob = _mapper.Map<Job>(job);
  
         _context.Jobs.Add(newJob);

         await _context.SaveChangesAsync();
         job.JobId = newJob.JobId;

         return new Response<AddJobDto>(job);
    }
        public async Task<Response<AddJobDto>> UpdateJob(AddJobDto job)
        {
            var find = await _context.Jobs.FindAsync(job.JobId);
            find.JobName = job.JobName;
            find.MinSalary = job.MinSalary;
            find.MaxSalary = job.MaxSalary;

            await _context.SaveChangesAsync();

            return new Response<AddJobDto>(job);
        }
      public async Task<Response<string>> DeleteJob(int id)
        {      
        var find = await _context.Jobs.FindAsync(id);
        _context.Jobs.Remove(find);
        await _context.SaveChangesAsync();
           if(find.JobId > 0 )  return new Response<string>("Job deleted successfully");

        
               return new Response<string>(HttpStatusCode.BadRequest, "Job not found");
        }
}
    
