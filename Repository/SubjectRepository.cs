using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using SubjectAPI.Models;

namespace SubjectAPI.Repository
{
    public class SubjectRepository
    {
        // Connection string
        private readonly string _connectionString;

        // Stored procedure names
        private readonly string _getAllSubjects;
        private readonly string _getSubjectByCode;
        private readonly string _addSubject;
        private readonly string _updateSubject;
        private readonly string _deleteSubject;


        // Constructor
        public SubjectRepository(IConfiguration config)
        {
            // Get connection string from appsettings.json
            _connectionString =
                config.GetConnectionString("CollegeDB")
                ?? throw new InvalidOperationException(
                    "Connection string 'CollegeDB' not found.");


            // Get stored procedure names from appsettings.json
            _getAllSubjects =
                config["StoredProcedures:Subject:GetAllSubjects"]
                ?? throw new InvalidOperationException(
                    "GetAllSubjects procedure name is missing.");

            _getSubjectByCode =
                config["StoredProcedures:Subject:GetSubjectByCode"]
                ?? throw new InvalidOperationException(
                    "GetSubjectByCode procedure name is missing.");

            _addSubject =
                config["StoredProcedures:Subject:AddSubject"]
                ?? throw new InvalidOperationException(
                    "AddSubject procedure name is missing.");

            _updateSubject =
                config["StoredProcedures:Subject:UpdateSubject"]
                ?? throw new InvalidOperationException(
                    "UpdateSubject procedure name is missing.");

            _deleteSubject =
                config["StoredProcedures:Subject:DeleteSubject"]
                ?? throw new InvalidOperationException(
                    "DeleteSubject procedure name is missing.");
        }


        // Create SQL connection
        private IDbConnection Connection =>
            new SqlConnection(_connectionString);


        // GET ALL SUBJECTS
        public async Task<List<Subject>> GetAll()
        {
            try
            {
                using IDbConnection db = Connection;

                var result = await db.QueryAsync<Subject>(
                    _getAllSubjects,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        // GET SUBJECT BY CODE
        public async Task<Subject?> GetSubjectByCode(
            int subjectCode)
        {
            try
            {
                using IDbConnection db = Connection;

                var result =
                    await db.QueryFirstOrDefaultAsync<Subject>(
                        _getSubjectByCode,
                        new
                        {
                            SubjectCode = subjectCode
                        },
                        commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // ADD SUBJECT
        public async Task<Subject> AddSubject(
            Subject subject)
        {
            try
            {
                using IDbConnection db = Connection;

                var result =
                    await db.QuerySingleAsync<Subject>(
                        _addSubject,
                        new
                        {
                            subject.SubjectName,
                            subject.Credits,
                            subject.FacultyID,
                            subject.DepartmentID,
                            subject.CreatedBy
                        },
                        commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // UPDATE SUBJECT
        public async Task<Subject?> UpdateSubject(
            int subjectCode,
            Subject subject)
        {
            try
            {
                using IDbConnection db = Connection;

                // Check whether subject exists
                var existingSubject =
                    await db.QueryFirstOrDefaultAsync<Subject>(
                        _getSubjectByCode,
                        new
                        {
                            SubjectCode = subjectCode
                        },
                        commandType: CommandType.StoredProcedure);

                if (existingSubject == null)
                {
                    return null;
                }


                // Update subject
                var updatedSubject =
                    await db.QuerySingleAsync<Subject>(
                        _updateSubject,
                        new
                        {
                            SubjectCode = subjectCode,
                            subject.SubjectName,
                            subject.Credits,
                            subject.FacultyID,
                            subject.DepartmentID,
                            subject.ModifiedBy
                        },
                        commandType: CommandType.StoredProcedure);

                return updatedSubject;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // SOFT DELETE SUBJECT
        public async Task<bool> DeleteSubject(
            int subjectCode)
        {
            try
            {
                using IDbConnection db = Connection;

                int rowsAffected =
                    await db.ExecuteAsync(
                        _deleteSubject,
                        new
                        {
                            SubjectCode = subjectCode
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