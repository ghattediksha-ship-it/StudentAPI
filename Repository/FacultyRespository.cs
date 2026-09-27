using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using FacultyAPI.Models;

namespace FacultyAPI.Repository
{
    public class FacultyRepository
    {
        // Connection string
        private readonly string _connectionString;

        // Stored procedure names
        private readonly string _getAllFaculty;
        private readonly string _getFacultyById;
        private readonly string _addFaculty;
        private readonly string _updateFaculty;
        private readonly string _deleteFaculty;


        // Constructor
        public FacultyRepository(IConfiguration config)
        {
            // Get connection string from appsettings.json
            _connectionString =
                config.GetConnectionString("CollegeDB")
                ?? throw new InvalidOperationException(
                    "Connection string 'CollegeDB' not found.");


            // Get stored procedure names from appsettings.json
            _getAllFaculty =
                config["StoredProcedures:Faculty:GetAllFaculty"]
                ?? throw new InvalidOperationException(
                    "GetAllFaculty procedure name is missing.");

            _getFacultyById =
                config["StoredProcedures:Faculty:GetFacultyById"]
                ?? throw new InvalidOperationException(
                    "GetFacultyById procedure name is missing.");

            _addFaculty =
                config["StoredProcedures:Faculty:AddFaculty"]
                ?? throw new InvalidOperationException(
                    "AddFaculty procedure name is missing.");

            _updateFaculty =
                config["StoredProcedures:Faculty:UpdateFaculty"]
                ?? throw new InvalidOperationException(
                    "UpdateFaculty procedure name is missing.");

            _deleteFaculty =
                config["StoredProcedures:Faculty:DeleteFaculty"]
                ?? throw new InvalidOperationException(
                    "DeleteFaculty procedure name is missing.");
        }


        // Create SQL connection
        private IDbConnection Connection =>
            new SqlConnection(_connectionString);


        // =========================================================
        // 1. GET ALL FACULTY
        // =========================================================
        public async Task<List<Faculty>> GetAll()
        {
            try
            {
                using IDbConnection db = Connection;

                var result = await db.QueryAsync<Faculty>(
                    _getAllFaculty,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while getting all faculties.",
                    ex);
            }
        }


        // =========================================================
        // 2. GET FACULTY BY ID
        // =========================================================
        public async Task<Faculty?> GetById(int id)
        {
            try
            {
                using IDbConnection db = Connection;

                var result =
                    await db.QueryFirstOrDefaultAsync<Faculty>(
                        _getFacultyById,
                        new
                        {
                            FacultyId = id
                        },
                        commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while getting faculty by ID.",
                    ex);
            }
        }


        // =========================================================
        // 3. ADD FACULTY
        // =========================================================
        public async Task<Faculty?> Add(Faculty faculty)
        {
            try
            {
                using IDbConnection db = Connection;

                var id = await db.QuerySingleAsync<int>(
                    _addFaculty,
                    new
                    {
                        faculty.FacultyName,
                        faculty.Email,
                        faculty.Salary,
                        faculty.DepartmentName,
                        faculty.Phone,
                        faculty.CreatedBy
                    },
                    commandType: CommandType.StoredProcedure);


                // Get complete record from database
                var newFaculty =
                    await db.QueryFirstOrDefaultAsync<Faculty>(
                        _getFacultyById,
                        new
                        {
                            FacultyId = id
                        },
                        commandType: CommandType.StoredProcedure);

                return newFaculty;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while adding faculty.",
                    ex);
            }
        }


        // =========================================================
        // 4. UPDATE FACULTY
        // =========================================================
        public async Task<Faculty?> Update(
            int id,
            Faculty faculty)
        {
            try
            {
                using IDbConnection db = Connection;

                // Check whether faculty exists
                var existingFaculty =
                    await db.QueryFirstOrDefaultAsync<Faculty>(
                        _getFacultyById,
                        new
                        {
                            FacultyId = id
                        },
                        commandType: CommandType.StoredProcedure);

                if (existingFaculty == null)
                {
                    return null;
                }


                // Update faculty
                await db.ExecuteAsync(
                    _updateFaculty,
                    new
                    {
                        FacultyId = id,
                        faculty.FacultyName,
                        faculty.Email,
                        faculty.Salary,
                        faculty.DepartmentName,
                        faculty.Phone,
                        faculty.ModifiedBy
                    },
                    commandType: CommandType.StoredProcedure);


                // Get updated record from database
                var updatedFaculty =
                    await db.QueryFirstOrDefaultAsync<Faculty>(
                        _getFacultyById,
                        new
                        {
                            FacultyId = id
                        },
                        commandType: CommandType.StoredProcedure);

                return updatedFaculty;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while updating faculty.",
                    ex);
            }
        }


        // =========================================================
        // 5. SOFT DELETE FACULTY
        // =========================================================
        public async Task<bool> Delete(int id)
        {
            try
            {
                using IDbConnection db = Connection;

                int rowsAffected =
                    await db.QuerySingleAsync<int>(
                        _deleteFaculty,
                        new
                        {
                            FacultyId = id
                        },
                        commandType: CommandType.StoredProcedure);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while deleting faculty.",
                    ex);
            }
        }
    }
}