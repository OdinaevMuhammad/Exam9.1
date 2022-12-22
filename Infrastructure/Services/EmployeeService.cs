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

public class EmployeeService
{
    private readonly DataContext _context; 

    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IMapper _mapper;

    public EmployeeService(DataContext context, IWebHostEnvironment env, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
        _hostEnvironment = env;
    }
    public async Task<Response<List<GetEmployee>>> GetEmployees()
    {
        var list =  (
            from e in _context.Employees
            join t in _context.JobTimeHistories on e.EmployeeId equals t.EmployeeId
            join j in _context.Jobs on e.JobId equals j.JobId
            select new GetEmployee
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                ProfileImage = e.ProfileImage,
                HireDate = e.HireDate,
                Salary = e.Salary,
                JobName = j.JobName,
                StartJobTime = t.StartJobTime,
                TimeOfBeingLate = t.TimeOfBeingLate

            }
        ).ToList();
        return new Response<List<GetEmployee>>( list);
    }
   

       public async Task<Response<AddEmployee>> InsertEmployee(AddEmployee employee)
    {
        if (employee.ProfileImage == null) return null;

        var path = Path.Combine(_hostEnvironment.WebRootPath, "images");
        if (Directory.Exists(path) == false) Directory.CreateDirectory(path);
         var filepath = Path.Combine(_hostEnvironment.WebRootPath, "images", employee.ProfileImage.FileName);
        using (var stream = File.Create(filepath))
        {
            await employee.ProfileImage.CopyToAsync(stream);
        }
         var list = _mapper.Map<Employee>(employee);
        _context.Employees.Add(list);

        await _context.SaveChangesAsync();

      
         employee.EmployeeId = list.EmployeeId;
        return new Response<AddEmployee>(employee);
    }
    public async Task<Response<GetEmployee>> UpdateEmployee(AddEmployee employee)
    {

        var find = await _context.Employees.FindAsync(employee.EmployeeId);
        find.FirstName = employee.FirstName;
        find.LastName = employee.LastName;
        find.Email = employee.Email;
        find.PhoneNumber = employee.PhoneNumber;
        find.Salary = employee.Salary;
        find.JobId = employee.JobId;
        find.HireDate = employee.HireDate;

        if (employee.ProfileImage != null)
        {
            find.ProfileImage = await UpdateFile(employee.ProfileImage, find.ProfileImage);
        }
            await _context.SaveChangesAsync();

        var response = _mapper.Map<GetEmployee>(find);
   
        return new Response<GetEmployee>(response);
    }
    public async Task<string> UpdateFile(IFormFile file, string oldFileName)
    {
        var filepath = Path.Combine(_hostEnvironment.WebRootPath, "images", oldFileName);
        if (File.Exists(filepath) == true) File.Delete(filepath);

        var newFilepath = Path.Combine(_hostEnvironment.WebRootPath, "images", file.FileName);
        using (var stream = new FileStream(newFilepath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return file.FileName;

    }
    public async Task<Response<string>> DeleteEmployee(int id)
    {
        var find = await _context.Employees.FindAsync(id);
        _context.Employees.Remove(find);
        await _context.SaveChangesAsync();
        if (find.EmployeeId > 0) return new Response<string>("Employee deleted successfully");


        return new Response<string>(HttpStatusCode.BadRequest, "Employee not found");
    }
        public async Task<Response<List<GetEmployee>>> GetEmployeeById(int id)
    {
        var find = await _context.Employees.FindAsync(id);
  
        await _context.SaveChangesAsync();
        var list =  (
            from e in _context.Employees
            join t in _context.JobTimeHistories on e.EmployeeId equals t.EmployeeId
            join j in _context.Jobs on e.JobId equals j.JobId
            select new GetEmployee
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                ProfileImage = e.ProfileImage,
                HireDate = e.HireDate,
                Salary = e.Salary,
                JobName = j.JobName,
                StartJobTime = t.StartJobTime,
                TimeOfBeingLate = t.TimeOfBeingLate

            }
        ).ToList();
        if (find.EmployeeId > 0) return new Response<List<GetEmployee>>(list);

        else{return new Response<List<GetEmployee>>(HttpStatusCode.BadRequest, "EmployeeId not  found");}


    }
    public async Task<Response<List<GetEmployee>>> GetEmployeeByName(string name)
    {
            var find = await (from e in _context.Employees
            where e.FirstName.Equals(name)
            select new GetEmployee()
            {
                     EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                ProfileImage = e.ProfileImage,
                HireDate = e.HireDate,
                Salary = e.Salary
            }).ToListAsync();

            return new Response<List<GetEmployee>>(find);
    }
      public async Task<Response<List<GetEmployee>>> GetEmployeeByPhoneNumber(string phoneNumber)
    {
            var find = await (from e in _context.Employees
            where e.PhoneNumber.Equals(phoneNumber)
            select new GetEmployee()
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                ProfileImage = e.ProfileImage,
                HireDate = e.HireDate,
                Salary = e.Salary
            }).ToListAsync();

            return new Response<List<GetEmployee>>(find);
    }
}