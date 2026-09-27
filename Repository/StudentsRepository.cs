using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using StudentAPI.Models;

namespace StudentAPI.Repository
{
    public class StudentRepository
    {
        private readonly string _connectionString;
        private readonly string _getAllStudents;
        private readonly string _getStudentById;
        private readonly string _addStudent;
        private readonly string _updateStudent;
        private readonly string _deleteStudent;

        public StudentRepository(IConfiguration config)
        {
            // Get connection string from appsettings.json
            _connectionString =
                config.GetConnectionString("CollegeDB")
                ?? throw new Exception(
                    "CollegeDB connection string is missing.");

            // Get stored procedure names from appsettings.json
            _getAllStudents =
                config["StoredProcedures:GetAllStudents"]
                ?? throw new Exception(
                    "GetAllStudents procedure name is missing.");

            _getStudentById =
                config["StoredProcedures:GetStudentById"]
                ?? throw new Exception(
                    "GetStudentById procedure name is missing.");

            _addStudent =
                config["StoredProcedures:AddStudent"]
                ?? throw new Exception(
                    "AddStudent procedure name is missing.");

            _updateStudent =
                config["StoredProcedures:UpdateStudent"]
                ?? throw new Exception(
                    "UpdateStudent procedure name is missing.");

            _deleteStudent =
                config["StoredProcedures:DeleteStudent"]
                ?? throw new Exception(
                    "DeleteStudent procedure name is missing.");
        }

        private IDbConnection Connection =>
            new SqlConnection(_connectionString);


        // GET ALL STUDENTS
        public async Task<List<Student>> GetAll()
        {
            try
            {
                using IDbConnection db = Connection;

                var result = await db.QueryAsync<Student>(
                    _getAllStudents,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while getting all students.", ex);
            }
        }


        // GET STUDENT BY ID
        public async Task<Student?> GetById(int id)
        {
            try
            {
                using IDbConnection db = Connection;

                var result =
                    await db.QueryFirstOrDefaultAsync<Student>(
                        _getStudentById,
                        new
                        {
                            Id = id
                        },
                        commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error while getting student with ID {id}.",
                    ex);
            }
        }


        // ADD STUDENT
        public async Task<Student?> Add(Student student)
        {
            try
            {
                using IDbConnection db = Connection;

                var id = await db.QuerySingleAsync<int>(
                    _addStudent,
                    new
                    {
                        student.Name,
                        student.CGPA,
                        student.Age,
                        student.CreatedBy
                    },
                    commandType: CommandType.StoredProcedure);

                return await db.QueryFirstOrDefaultAsync<Student>(
                    _getStudentById,
                    new
                    {
                        Id = id
                    },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while adding student.", ex);
            }
        }


        // UPDATE STUDENT
        public async Task<Student?> Update(
            int id,
            Student student)
        {
            try
            {
                using IDbConnection db = Connection;

                var existingStudent =
                    await db.QueryFirstOrDefaultAsync<Student>(
                        _getStudentById,
                        new
                        {
                            Id = id
                        },
                        commandType: CommandType.StoredProcedure);

                if (existingStudent == null)
                {
                    return null;
                }

                await db.ExecuteAsync(
                    _updateStudent,
                    new
                    {
                        Id = id,
                        student.Name,
                        student.CGPA,
                        student.Age,
                        student.ModifiedBy
                    },
                    commandType: CommandType.StoredProcedure);

                return await db.QueryFirstOrDefaultAsync<Student>(
                    _getStudentById,
                    new
                    {
                        Id = id
                    },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error while updating student with ID {id}.",
                    ex);
            }
        }


        // DELETE STUDENT
        public async Task<bool> Delete(int id)
        {
            try
            {
                using IDbConnection db = Connection;

                var rowsAffected =
                    await db.QuerySingleAsync<int>(
                        _deleteStudent,
                        new
                        {
                            Id = id
                        },
                        commandType: CommandType.StoredProcedure);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while deleting the student.",
                    ex);
            }
        }
    }
}