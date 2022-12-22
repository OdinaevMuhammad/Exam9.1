using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class JobHistoryController
{
    private JobHistoryService _JobHistoryService;
    public JobHistoryController(JobHistoryService JobHistoryService)
    {
        _JobHistoryService = JobHistoryService;
    }
    

    [HttpGet("GetJobHistory")]
    public async Task<Response<List<GetJobHistory>>> GetJobHistorys()
    {
        return  await _JobHistoryService.GetJobHistory();
    }
       [HttpPost("InsertJobHistory")]
    public async Task<Response<AddJobHistory>> InsertJobHistory(AddJobHistory JobHistory)
    {
        return await _JobHistoryService.InsertJobHistory(JobHistory);
    }

    [HttpPut("UpdateJobHistory")]
    public async Task<Response<AddJobHistory>> Update(AddJobHistory JobHistory)
    {
        return await _JobHistoryService.UpdateJobHistory(JobHistory);
    }
    [HttpDelete("DeleteJobHistory")]
    public async Task<Response<string>> DeleteJobHistory(int employeeid)
    {
        return await _JobHistoryService.DeleteJobHistory(employeeid);
    }
}