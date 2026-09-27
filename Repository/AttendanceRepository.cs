using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using AttendanceAPI.Models;

namespace AttendanceAPI.Repository
{
    public class AttendanceRepository
    {
        // Connection string
        private readonly string _connectionString;

        // Constructor
        public AttendanceRepository(IConfiguration config)
        {
            _connectionString =
                config.GetConnectionString("CollegeDB")
                ?? throw new InvalidOperationException(
                    "Connection string 'CollegeDB' not found.");
        }

        // Create SQL connection
        private IDbConnection Connection =>
            new SqlConnection(_connectionString);


        // =========================================================
        // 1. GET ALL ATTENDANCE
        // =========================================================
        public async Task<List<Attendance>> GetAll()
        {
            try
            {
                using IDbConnection db = Connection;

                var result = await db.QueryAsync<Attendance>(
                    "GetAllAttendance",
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        // =========================================================
        // 2. GET ATTENDANCE BY STUDENT ID
        // =========================================================
        public async Task<Attendance?> GetByStudentId(int studentId)
        {
            try
            {
                using IDbConnection db = Connection;

                var result =
                    await db.QueryFirstOrDefaultAsync<Attendance>(
                        "GetByStudentId",
                        new
                        {
                            StudentID = studentId
                        },
                        commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // =========================================================
        // 3. ADD ATTENDANCE
        // =========================================================
        public async Task<Attendance> Add(Attendance attendance)
        {
            try
            {
                using IDbConnection db = Connection;

                await db.ExecuteAsync(
                    "AddAttendance",
                    new
                    {
                        attendance.StudentID,
                        attendance.SubjectID,
                        attendance.AttendanceDate,
                        attendance.Status
                    },
                    commandType: CommandType.StoredProcedure);

                return attendance;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // =========================================================
        // 4. UPDATE ATTENDANCE
        // =========================================================
        public async Task<Attendance?> Update(
            int studentId,
            Attendance attendance)
        {
            try
            {
                using IDbConnection db = Connection;

                // First check whether attendance exists
                var existingAttendance =
                    await db.QueryFirstOrDefaultAsync<Attendance>(
                        "GetByStudentId",
                        new
                        {
                            StudentID = studentId
                        },
                        commandType: CommandType.StoredProcedure);

                if (existingAttendance == null)
                {
                    return null;
                }

                // Update attendance
                await db.ExecuteAsync(
                    "UpdateAttendance",
                    new
                    {
                        StudentID = studentId,
                        attendance.SubjectID,
                        attendance.AttendanceDate,
                        attendance.Status,
                        ModifiedBy = attendance.ModifiedBy
                    },
                    commandType: CommandType.StoredProcedure);

                // Update returned object
                existingAttendance.SubjectID =
                    attendance.SubjectID;

                existingAttendance.AttendanceDate =
                    attendance.AttendanceDate;

                existingAttendance.Status =
                    attendance.Status;

                existingAttendance.ModifiedBy =
                    attendance.ModifiedBy;

                existingAttendance.ModifiedOn =
                    DateTime.Now;

                return existingAttendance;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // =========================================================
        // 5. DELETE ATTENDANCE - SOFT DELETE
        // =========================================================
        public async Task<bool> Delete(int studentId)
        {
            try
            {
                using IDbConnection db = Connection;

                int rowsAffected = await db.ExecuteAsync(
                    "DeleteAttendance",
                    new
                    {
                        StudentID = studentId
                    },
                    commandType: CommandType.StoredProcedure);

                return rowsAffected > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
