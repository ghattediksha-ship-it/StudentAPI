using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using UserAPI.Models;

namespace UserAPI.Repository
{
    public class UserRepository
    {
        private readonly string _connectionString;

        // Stored procedure names
        private readonly string _getAllUsers;
        private readonly string _getUserById;
        private readonly string _addUser;
        private readonly string _updateUser;
        private readonly string _deleteUser;


        // Constructor
        public UserRepository(IConfiguration config)
        {
            // Get connection string from appsettings.json
            _connectionString =
                config.GetConnectionString("CollegeDB")
                ?? throw new InvalidOperationException(
                    "Connection string 'CollegeDB' not found.");


            // Get stored procedure names from appsettings.json
            _getAllUsers =
                config["StoredProcedures:User:GetAllUsers"]
                ?? throw new InvalidOperationException(
                    "GetAllUsers procedure name is missing.");

            _getUserById =
                config["StoredProcedures:User:GetUserById"]
                ?? throw new InvalidOperationException(
                    "GetUserById procedure name is missing.");

            _addUser =
                config["StoredProcedures:User:AddUser"]
                ?? throw new InvalidOperationException(
                    "AddUser procedure name is missing.");

            _updateUser =
                config["StoredProcedures:User:UpdateUser"]
                ?? throw new InvalidOperationException(
                    "UpdateUser procedure name is missing.");

            _deleteUser =
                config["StoredProcedures:User:DeleteUser"]
                ?? throw new InvalidOperationException(
                    "DeleteUser procedure name is missing.");
        }


        // Create SQL connection
        private IDbConnection Connection =>
            new SqlConnection(_connectionString);


        // =====================================================
        // GET ALL USERS
        // =====================================================

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                using IDbConnection db = Connection;

                var result = await db.QueryAsync<User>(
                    _getAllUsers,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        // =====================================================
        // GET USER BY ID
        // =====================================================

        public async Task<User?> GetUserById(int id)
        {
            try
            {
                using IDbConnection db = Connection;

                var result =
                    await db.QueryFirstOrDefaultAsync<User>(
                        _getUserById,
                        new
                        {
                            UserID = id
                        },
                        commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // =====================================================
        // ADD USER
        // =====================================================

        public async Task<User> AddUser(User user)
        {
            try
            {
                using IDbConnection db = Connection;

                var result =
                    await db.QuerySingleAsync<User>(
                        _addUser,
                        new
                        {
                            Username = user.Username,
                            Email = user.Email,
                            Password = user.Password,
                            CreatedBy = user.CreatedBy
                        },
                        commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while adding user.",
                    ex);
            }
        }


        // =====================================================
        // UPDATE USER
        // =====================================================

        public async Task<User?> UpdateUser(
            int id,
            User user)
        {
            try
            {
                using IDbConnection db = Connection;

                // Check whether active user exists
                var existingUser =
                    await db.QueryFirstOrDefaultAsync<User>(
                        _getUserById,
                        new
                        {
                            UserID = id
                        },
                        commandType: CommandType.StoredProcedure);

                if (existingUser == null)
                {
                    return null;
                }


                // Update user
                await db.ExecuteAsync(
                    _updateUser,
                    new
                    {
                        UserID = id,
                        Username = user.Username,
                        Email = user.Email,
                        Password = user.Password,
                        ModifiedBy = user.ModifiedBy
                    },
                    commandType: CommandType.StoredProcedure);


                // Update returned object
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.ModifiedOn = DateTime.Now;

                return existingUser;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // =====================================================
        // SOFT DELETE USER
        // =====================================================

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                using IDbConnection db = Connection;

                int rowsAffected =
                    await db.QuerySingleAsync<int>(
                        _deleteUser,
                        new
                        {
                            UserID = id
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