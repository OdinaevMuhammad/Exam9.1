using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class JobTimeHistoryController
{
    private JobTimeHistoryService _JobTimeHistoryService;
    public JobTimeHistoryController(JobTimeHistoryService jobtimeHistoryService)
    {
        _JobTimeHistoryService = jobtimeHistoryService;
    }
    
     [HttpGet("GetAverage")]
    public  Task<Response<TimeSpan>> GetAverage(int id, DateTime start, DateTime end)
    {
        return   _JobTimeHistoryService.GetAvgById(id,start,end);
    }

    [HttpGet("GetJobTimeHistory")]
    public async Task<Response<List<GetJobTimeHistory>>> GetJobTimeHistorys()
    {
        return  await _JobTimeHistoryService.GetJobTimeHistory();
    }
       [HttpPost("InsertJobTimeHistory")]
    public async Task<Response<AddJobTimeHistory>> InsertJobTimeHistory(AddJobTimeHistory jobTimeHistory)
    {
        return await _JobTimeHistoryService.InsertJobTimeHistory(jobTimeHistory);
    }

    [HttpPut("UpdateJobTimeHistory")]
    public async Task<Response<AddJobTimeHistory>> Update(AddJobTimeHistory jobTimeHistory)
    {
        return await _JobTimeHistoryService.UpdateJobTimeHistory(jobTimeHistory);
    }
    [HttpDelete("DeleteJobTimeHistory")]
    public async Task<Response<string>> DeleteJobTimeHistory(int id)
    {
        return await _JobTimeHistoryService.DeleteJobTimeHistory(id);
    }
}