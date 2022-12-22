using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class JobController
{
    private JobService _JobService;
    public JobController(JobService JobService)
    {
        _JobService = JobService;
    }
    

    [HttpGet("GetJob")]
    public async Task<Response<List<AddJobDto>>> GetJob()
    {
        return  await _JobService.GetJobs();
    }
       [HttpPost("InsertJob")]
    public async Task<Response<AddJobDto>> InsertJob(AddJobDto Job)
    {
        return await _JobService.InsertJob(Job);
    }

    [HttpPut("UpdateJob")]
    public async Task<Response<AddJobDto>> UpdateJob(AddJobDto Job)
    {
        return await _JobService.UpdateJob(Job);
    }
    [HttpDelete("DeleteJob")]
    public async Task<Response<string>> DeleteJob(int id)
    {
        return await _JobService.DeleteJob(id);
    }
}