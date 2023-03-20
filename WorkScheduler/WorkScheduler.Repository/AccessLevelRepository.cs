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
    public class AccessLevelRepository:IAccessLevelRepository
    {
        public static string connectionString = Environment.GetEnvironmentVariable("SQLConn", EnvironmentVariableTarget.User);
        public async Task<List<IAccessLevel>> GetAll()
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                List<IAccessLevel> accessLevels = new List<IAccessLevel>();
                SqlCommand findAccount = new SqlCommand("Select * From AccessLevel;", connectionAsync);
                await connectionAsync.OpenAsync();
                SqlDataReader readerAsync = await findAccount.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    while (await readerAsync.ReadAsync())
                    {
                        IAccessLevel accessLevel = new AccessLevel();
                        accessLevel.Id = readerAsync.GetGuid(0);
                        accessLevel.AccessLevelName = readerAsync.GetString(1);
                        accessLevels.Add(accessLevel);
                    }
                }
                readerAsync.Close();
                connectionAsync.Close();
                return accessLevels;
            }
        }

        public async Task<IAccessLevel> Get(Guid id)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                IAccessLevel accessLevel = new AccessLevel();
                SqlCommand findAccessLevel = new SqlCommand("Select * From AccessLevel where id = @id;", connectionAsync);
                findAccessLevel.Parameters.AddWithValue("@id", id);
                await connectionAsync.OpenAsync();
                SqlDataReader readerAsync = await findAccessLevel.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    await readerAsync.ReadAsync();
                    accessLevel.Id = (Guid)readerAsync[0];
                    accessLevel.AccessLevelName = (string)readerAsync[1];

                }
                readerAsync.Close();
                connectionAsync.Close();
                return accessLevel;
            }
        }
    }
}
