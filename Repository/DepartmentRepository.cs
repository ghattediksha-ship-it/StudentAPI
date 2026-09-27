using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using DepartmentAPI.Models;

namespace DepartmentAPI.Repository
{
    public class DepartmentRepository
    {
        private readonly string _connectionString;

        // Stored procedure names
        private readonly string _getAllDepartments;
        private readonly string _getDepartmentById;
        private readonly string _addDepartment;
        private readonly string _updateDepartment;
        private readonly string _deleteDepartment;


        // Constructor
        public DepartmentRepository(IConfiguration config)
        {
            // Get connection string from appsettings.json
            _connectionString =
                config.GetConnectionString("CollegeDB")
                ?? throw new InvalidOperationException(
                    "Connection string 'CollegeDB' not found.");


            // Get stored procedure names from appsettings.json
            _getAllDepartments =
                config["StoredProcedures:Department:GetAllDepartments"]
                ?? throw new InvalidOperationException(
                    "GetAllDepartment procedure name is missing.");

            _getDepartmentById =
                config["StoredProcedures:Department:GetDepartmentById"]
                ?? throw new InvalidOperationException(
                    "GetDepartmentById procedure name is missing.");

            _addDepartment =
                config["StoredProcedures:Department:AddDepartment"]
                ?? throw new InvalidOperationException(
                    "AddDepartment procedure name is missing.");

            _updateDepartment =
                config["StoredProcedures:Department:UpdateDepartment"]
                ?? throw new InvalidOperationException(
                    "UpdateDepartment procedure name is missing.");

            _deleteDepartment =
                config["StoredProcedures:Department:DeleteDepartment"]
                ?? throw new InvalidOperationException(
                    "DeleteDepartment procedure name is missing.");
        }


        // Database connection
        private IDbConnection Connection =>
            new SqlConnection(_connectionString);


        // 1. GET ALL DEPARTMENTS
        public async Task<List<Department>> GetAll()
        {
            using IDbConnection db = Connection;

            try
            {
                var result = await db.QueryAsync<Department>(
                    _getAllDepartments,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch
            {
                throw;
            }
        }


        // 2. GET DEPARTMENT BY ID
        public async Task<Department?> GetById(int id)
        {
            using IDbConnection db = Connection;

            try
            {
                var result =
                    await db.QueryFirstOrDefaultAsync<Department>(
                        _getDepartmentById,
                        new
                        {
                            DepartmentID = id
                        },
                        commandType: CommandType.StoredProcedure);

                return result;
            }
            catch
            {
                throw;
            }
        }


        // 3. ADD DEPARTMENT
        public async Task<Department> Add(
            Department department)
        {
            using IDbConnection db = Connection;

            try
            {
                var id = await db.QuerySingleAsync<int>(
                    _addDepartment,
                    new
                    {
                        DepartmentName =
                            department.DepartmentName,

                        CreatedBy =
                            department.CreatedBy
                    },
                    commandType: CommandType.StoredProcedure);


                // Get complete record from database
                var newDepartment =
                    await db.QueryFirstOrDefaultAsync<Department>(
                        _getDepartmentById,
                        new
                        {
                            DepartmentID = id
                        },
                        commandType: CommandType.StoredProcedure);

                return newDepartment!;
            }
            catch
            {
                throw;
            }
        }


        // 4. UPDATE DEPARTMENT
        public async Task<Department?> Update(
            int id,
            Department department)
        {
            using IDbConnection db = Connection;

            try
            {
                // Check whether department exists
                var existingDepartment =
                    await db.QueryFirstOrDefaultAsync<Department>(
                        _getDepartmentById,
                        new
                        {
                            DepartmentID = id
                        },
                        commandType: CommandType.StoredProcedure);

                if (existingDepartment == null)
                {
                    return null;
                }


                // Update department
                await db.ExecuteAsync(
                    _updateDepartment,
                    new
                    {
                        DepartmentID = id,
                        DepartmentName =
                            department.DepartmentName
                    },
                    commandType: CommandType.StoredProcedure);


                // Update returned object
                existingDepartment.DepartmentName =
                    department.DepartmentName;

                return existingDepartment;
            }
            catch
            {
                throw;
            }
        }


        // 5. DELETE DEPARTMENT
        public async Task<bool> Delete(int id)
        {
            using IDbConnection db = Connection;

            try
            {
                int rowsAffected =
                    await db.QuerySingleAsync<int>(
                        _deleteDepartment,
                        new
                        {
                            DepartmentID = id
                        },
                        commandType: CommandType.StoredProcedure);

                return rowsAffected > 0;
            }
            catch
            {
                throw;
            }
        }
    }
}