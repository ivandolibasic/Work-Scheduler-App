using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;

namespace WorkScheduler.Repository
{
    public class TaskStatusRepository : ITaskStatusRepository
    {
        public static string connectionString = Environment.GetEnvironmentVariable("SQLConn", EnvironmentVariableTarget.User);

        public async Task<List<ITaskStatus>> GetAsync()
        {
            List<ITaskStatus> taskStatuses = new List<ITaskStatus>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM TaskStatus;", connection);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    taskStatuses.Add(new WorkScheduler.Model.TaskStatus(reader.GetGuid(0), reader.GetString(1)));
                }
                reader.Close();
                connection.Close();
            }
            return taskStatuses;
        }

        public async Task<ITaskStatus> GetAsync(Guid id)
        {
            ITaskStatus taskStatus = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM TaskStatus WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        taskStatus = new WorkScheduler.Model.TaskStatus(reader.GetGuid(0), reader.GetString(1));
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return taskStatus;
        }

        public async Task<ITaskStatus> GetAsync(string status)
        {
            ITaskStatus taskStatus = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM TaskStatus WHERE Status = @Status", connection);
                    command.Parameters.AddWithValue("@Status", status);
                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        taskStatus = new WorkScheduler.Model.TaskStatus(reader.GetGuid(0), reader.GetString(1));
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return taskStatus;
        }
    }
}