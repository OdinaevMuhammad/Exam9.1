using System;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController
{
    private EmployeeService _employeeService;
    public EmployeeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    

    [HttpGet("GetEmployee")]
    public async Task<Response<List<GetEmployee>>> GetEmployees()
    {
        return  await _employeeService.GetEmployees();
    }
       [HttpGet("GetEmployeeByName")]
    public async Task<Response<List<GetEmployee>>> GetEmployeeByName(string name)
    {
        return  await _employeeService.GetEmployeeByName(name);
    }
           [HttpGet("GetEmployeeByPhoneNumber")]
    public async Task<Response<List<GetEmployee>>> GetEmployeeByPhoneNumber(string phoneNumber)
    {
        return  await _employeeService.GetEmployeeByPhoneNumber(phoneNumber);
    }
       [HttpPost("InsertEmployee")]
    public async Task<Response<AddEmployee>> InsertEmployee([FromForm] AddEmployee employee)
    {
        return await _employeeService.InsertEmployee(employee);
    }

    [HttpPut("UpdateEmployee")]
    public async Task<Response<GetEmployee>> Update([FromForm] AddEmployee employee)
    {
        return await _employeeService.UpdateEmployee(employee);
    }
    [HttpDelete("DeleteEmployee")]
    public async Task<Response<string>> DeleteEmployee(int id)
    {
        return await _employeeService.DeleteEmployee(id);
    }
      [HttpGet("GetEmployeeById")]
    public async Task<Response<List<GetEmployee>>> GetEmployeeById(int id)
    {
        return  await _employeeService.GetEmployeeById(id);
    }
}