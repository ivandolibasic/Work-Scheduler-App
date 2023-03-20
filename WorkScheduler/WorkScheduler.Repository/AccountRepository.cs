using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Model;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;

namespace WorkScheduler.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public static string connectionString = Environment.GetEnvironmentVariable("SQLConn", EnvironmentVariableTarget.User);
        IAccessLevelRepository accessLevelRepository;
        public AccountRepository(IAccessLevelRepository accessLevelRepository)
        {
            this.accessLevelRepository = accessLevelRepository;
        }
        public async Task<List<IAccount>> GetAllAsync()
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                List<IAccount> accounts = new List<IAccount>();
                
                SqlCommand findAccount = new SqlCommand("Select * From Account;", connectionAsync);
                await connectionAsync.OpenAsync();
                SqlDataReader readerAsync = await findAccount.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    while (await readerAsync.ReadAsync())
                    {
                        IAccount account = new Account();
                        account.Id = Guid.Parse(readerAsync[0].ToString());
                        account.Username = readerAsync[1].ToString();
                        account.Password = readerAsync[2].ToString();
                        account.AccessLevelId = Guid.Parse(readerAsync[3].ToString());
                        account.DateCreated = DateTime.Parse(readerAsync[4].ToString());
                        account.DateUpdated = DateTime.Parse(readerAsync[5].ToString());
                        account.CreatedByUser = Guid.Parse(readerAsync[6].ToString());
                        account.UpdatedByUser = Guid.Parse(readerAsync[7].ToString());
                        accounts.Add(account);
                    }
                }
                readerAsync.Close();
                connectionAsync.Close();
                return accounts;
            }
        }
        public async Task<IAccount> FindAccountAsync(string username, string password)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                IAccount account = new Account();
                SqlCommand findAccount = new SqlCommand("Select * From Account where Username = @username AND Password = @password;", connectionAsync);
                await connectionAsync.OpenAsync();
                findAccount.Parameters.AddWithValue("@username", username);
                findAccount.Parameters.AddWithValue("@password", password);
                SqlDataReader readerAsync = await findAccount.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    await readerAsync.ReadAsync();
                    account.Id = Guid.Parse(readerAsync[0].ToString());
                    account.Username = readerAsync[1].ToString();
                    account.Password = readerAsync[2].ToString();
                    account.AccessLevelId = Guid.Parse(readerAsync[3].ToString());
                    account.DateCreated = DateTime.Parse(readerAsync[4].ToString());
                    account.DateUpdated = DateTime.Parse(readerAsync[5].ToString());
                    account.CreatedByUser = Guid.Parse(readerAsync[6].ToString());
                    account.UpdatedByUser = Guid.Parse(readerAsync[7].ToString());
                }
                readerAsync.Close();
                connectionAsync.Close();
                return account;
            }
        }

        public async Task<IAccount> FindAccountByNameAsync(string username)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                IAccount account = new Account();
                SqlCommand findAccount = new SqlCommand("Select * From Account where Username = @username;", connectionAsync);
                await connectionAsync.OpenAsync();
                findAccount.Parameters.AddWithValue("@username", username);
                SqlDataReader readerAsync = await findAccount.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    await readerAsync.ReadAsync();
                    account.Id = Guid.Parse(readerAsync[0].ToString());
                    account.Username = readerAsync[1].ToString();
                    account.Password = readerAsync[2].ToString();
                    account.AccessLevelId = Guid.Parse(readerAsync[3].ToString());
                    account.DateCreated = DateTime.Parse(readerAsync[4].ToString());
                    account.DateUpdated = DateTime.Parse(readerAsync[5].ToString());
                    account.CreatedByUser = Guid.Parse(readerAsync[6].ToString());
                    account.UpdatedByUser = Guid.Parse(readerAsync[7].ToString());
                }
                readerAsync.Close();
                connectionAsync.Close();
                return account;
            }
        }

        public async Task<string> AddAccountAsync(IAccount account)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                SqlCommand getAccessLevel = new SqlCommand("Select Id From AccessLevel where Id = @AccessLevelId;", connectionAsync);
                await connectionAsync.OpenAsync();
                getAccessLevel.Parameters.AddWithValue("@AccessLevelId", account.AccessLevelId);
                SqlDataReader readerAsync = await getAccessLevel.ExecuteReaderAsync();
                if (!readerAsync.HasRows)
                {
                    readerAsync.Close();
                    connectionAsync.Close();
                    return "Error while selecting Access level";
                }
                readerAsync.Close();
                if (string.IsNullOrEmpty(account.Username) || string.IsNullOrEmpty(account.Password))
                {
                    connectionAsync.Close();
                    return "Username or password cannot be null";
                }
                connectionAsync.Close();
                SqlCommand addAccount = new SqlCommand("Insert Into Account Values(@Id, @Username, @Password, @AccessLevelId, @DateCreated, @DateUpdated, @CreatedByUser, @UpdatedByUser);", connectionAsync);
                await connectionAsync.OpenAsync();
                addAccount.Parameters.AddWithValue("@Id", account.Id);
                addAccount.Parameters.AddWithValue("@Username", account.Username);
                addAccount.Parameters.AddWithValue("@Password", account.Password);
                addAccount.Parameters.AddWithValue("@AccessLevelId", account.AccessLevelId);
                addAccount.Parameters.AddWithValue("@DateCreated", account.DateCreated);
                addAccount.Parameters.AddWithValue("@DateUpdated", account.DateUpdated);
                addAccount.Parameters.AddWithValue("@CreatedByUser", account.CreatedByUser);
                addAccount.Parameters.AddWithValue("@UpdatedByUser", account.UpdatedByUser);
                await addAccount.ExecuteNonQueryAsync();
                connectionAsync.Close();
                return "Account added";
            }
        }
        public async Task<string> UpdateAccountAsync(Guid id, IAccount account)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                SqlCommand getAccount = new SqlCommand("Select * From Account where Id = @Id;", connectionAsync);
                await connectionAsync.OpenAsync();
                getAccount.Parameters.AddWithValue("@Id", id);
                SqlDataReader readerAsync = await getAccount.ExecuteReaderAsync();
                if (!readerAsync.HasRows)
                {
                    readerAsync.Close();
                    connectionAsync.Close();
                    return "Account not found";
                }
                readerAsync.Close();

                SqlCommand getAccessLevel = new SqlCommand("Select Id From AccessLevel where Id = @AccessLevelId;", connectionAsync);
                getAccessLevel.Parameters.AddWithValue("@AccessLevelId", account.AccessLevelId);
                readerAsync = await getAccessLevel.ExecuteReaderAsync();
                if (!readerAsync.HasRows)
                {
                    readerAsync.Close();
                    connectionAsync.Close();
                    return "Error while selecting Access level";
                }
                readerAsync.Close();
                SqlCommand updateAccount = new SqlCommand("Update Account set Username = @Username, Password = @Password, AccessLevelId = @AccessLevelId, DateUpdated = @DateUpdated, UpdatedByUser = @UpdatedByUser Where id = @Id;", connectionAsync);
                updateAccount.Parameters.AddWithValue("@Username", account.Username);
                updateAccount.Parameters.AddWithValue("@Password", account.Password);
                updateAccount.Parameters.AddWithValue("@AccessLevelId", account.AccessLevelId);
                updateAccount.Parameters.AddWithValue("@DateUpdated", account.DateUpdated);
                updateAccount.Parameters.AddWithValue("@UpdatedByUser", account.UpdatedByUser);
                updateAccount.Parameters.AddWithValue("@Id", id);
                await updateAccount.ExecuteNonQueryAsync();
                connectionAsync.Close();
                return "Account updated";
            }
        }
        public async Task<bool> DeleteAccountAsync(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand findAccount = new SqlCommand("Select * From Account where Id = @id;", connection);
                findAccount.Parameters.AddWithValue("@id", id);
                await connection.OpenAsync();
                SqlDataReader reader = await findAccount.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    reader.Read();
                    Guid accessLevelId = reader.GetGuid(3);
                    List<IAccessLevel> accessLevels = await accessLevelRepository.GetAll(); 
                    foreach(IAccessLevel accessLevel in accessLevels)
                    {
                        if(accessLevel.Id == accessLevelId)
                        {
                            if(accessLevel.AccessLevelName=="SuperAdmin")
                            {
                                return false;
                            }
                        }
                    }
                    reader.Close();
                    SqlCommand deleteAccount = new SqlCommand("Delete From Account where Id = @id;", connection);
                    deleteAccount.Parameters.AddWithValue("@id", id);
                    await deleteAccount.ExecuteNonQueryAsync();
                    connection.Close();
                    return true;
                }
                reader.Close();
                connection.Close();
                return false;
            }
        }
    }
}
