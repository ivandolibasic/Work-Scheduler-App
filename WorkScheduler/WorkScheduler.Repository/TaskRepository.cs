using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;

namespace WorkScheduler.Repository
{
    public class TaskRepository : ITaskRepository
    {
        public static string connectionString = Environment.GetEnvironmentVariable("SQLConn", EnvironmentVariableTarget.User);

        public async Task<List<ITask>> GetAsync()
        {
            List<ITask> tasks = new List<ITask>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Task.*, Account.Username FROM Task LEFT JOIN Account ON Task.CreatedByUser = Account.Id", connection);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tasks.Add(new WorkScheduler.Model.Task()
                        {
                            Id = reader.GetGuid(0),
                            Description = reader.GetString(1),
                            TotalHoursNeeded = reader.GetInt32(2),
                            TaskStatusId = reader.GetGuid(3),
                            DateCreated = reader.GetDateTime(4),
                            DateUpdated = reader.GetDateTime(5),
                            CreatedByUser = reader.GetGuid(6),
                            UpdatedByUser = reader.GetGuid(7),
                            Username = reader.GetString(8)
                        });
                    }
                }
            }
            return tasks;
        }

        public async Task<ITask> GetAsync(Guid id)
        {
            ITask task = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Task.*, Account.Username FROM Task LEFT JOIN Account ON Task.CreatedByUser = Account.Id WHERE Task.Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        task = new WorkScheduler.Model.Task()
                        {
                            Id = reader.GetGuid(0),
                            Description = reader.GetString(1),
                            TotalHoursNeeded = reader.GetInt32(2),
                            TaskStatusId = reader.GetGuid(3),
                            DateCreated = reader.GetDateTime(4),
                            DateUpdated = reader.GetDateTime(5),
                            CreatedByUser = reader.GetGuid(6),
                            UpdatedByUser = reader.GetGuid(7),
                            Username = reader.GetString(8)
                        };
                    }
                }
            }
            return task;
        }

        public async Task<bool> PostAsync(ITask task)
        {
            if (task == null)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Task VALUES (@Id, @Description, @TotalHoursNeeded, @TaskStatusId, @DateCreated, @DateUpdated, @CreatedByUser, @UpdatedByUser)", connection);
                await connection.OpenAsync();
                SqlDataAdapter adapter = new SqlDataAdapter();
                command.Parameters.AddWithValue("@Id", Guid.NewGuid());
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@TotalHoursNeeded", task.TotalHoursNeeded);
                command.Parameters.AddWithValue("@TaskStatusId", task.TaskStatusId);
                command.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                command.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                command.Parameters.AddWithValue("@CreatedByUser", task.CreatedByUser);
                command.Parameters.AddWithValue("@UpdatedByUser", task.UpdatedByUser);
                adapter.InsertCommand = command;
                await adapter.InsertCommand.ExecuteNonQueryAsync();
                return true;
                //if (await adapter.InsertCommand.ExecuteNonQueryAsync() != 0)
                //{
                //    return true;
                //}
                //return false;
            }
        }

        public async Task<bool> PutAsync(Guid id, ITask task)
        {
            if (id == Guid.Empty || task == null)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Task WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        connection.Close();
                        return false;
                    }
                }
                connection.Close();
                SqlCommand updateCommand = new SqlCommand("UPDATE Task SET Description = @Description, TotalHoursNeeded = @TotalHoursNeeded, TaskStatusId = @TaskStatusId, DateUpdated = @DateUpdated, UpdatedByUser = @UpdatedByUser WHERE Task.Id = @Id", connection);
                await connection.OpenAsync();
                SqlDataAdapter adapter = new SqlDataAdapter();
                updateCommand.Parameters.AddWithValue("@Description", task.Description);
                updateCommand.Parameters.AddWithValue("@TaskStatusId", task.TaskStatusId);
                updateCommand.Parameters.AddWithValue("@TotalHoursNeeded", task.TotalHoursNeeded);
                updateCommand.Parameters.AddWithValue("@DateUpdated", task.DateUpdated);
                updateCommand.Parameters.AddWithValue("@UpdatedByUser", task.UpdatedByUser);
                updateCommand.Parameters.AddWithValue("@Id", id);
                adapter.UpdateCommand = updateCommand;
                await adapter.UpdateCommand.ExecuteNonQueryAsync();
            }
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Task WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        reader.Close();
                        SqlCommand deleteCommand = new SqlCommand("DELETE FROM Task WHERE Id = @Id", connection);
                        deleteCommand.Parameters.AddWithValue("@Id", id);
                        adapter.DeleteCommand = deleteCommand;
                        if (await adapter.DeleteCommand.ExecuteNonQueryAsync() != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}