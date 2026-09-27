using DepartmentAPI.Repository;
using FacultyAPI.Repository;
using StudentAPI.Repository;
using SubjectAPI.Repository;
using UserAPI.Repository;
using AttendanceAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register StudentRepository
builder.Services.AddScoped<StudentRepository>();

// Register DeaprtmentRepository
builder.Services.AddScoped<DepartmentRepository>();

builder.Services.AddScoped<FacultyRepository>();

builder.Services.AddScoped<SubjectRepository>(); 

builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<AttendanceRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


