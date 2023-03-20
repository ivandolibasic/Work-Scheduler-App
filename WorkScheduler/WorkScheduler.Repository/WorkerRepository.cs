using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Common;
using WorkScheduler.Model;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;

namespace WorkScheduler.Repository
{
    public class WorkerRepository : IWorkerRepository
    {
        public static string connectionString = Environment.GetEnvironmentVariable("SQLConn", EnvironmentVariableTarget.User);
        public async Task<bool> Delete(Guid id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("Select * from Worker where id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                using (var command2 = new SqlCommand("Delete from Worker where id=@id", connection))
                                {
                                    command2.Parameters.AddWithValue("@id", id);
                                    reader.Close();
                                    if (await command2.ExecuteNonQueryAsync() != 0)
                                    {
                                        return true;
                                    }
                                }
                            }
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<IWorker>> Get(Sorting sorting, Paging paging, Filter filter)
        {
            List<IWorker> workers = new List<IWorker>();
            try
            {
                if (sorting.OrderDirectionIsValid() && sorting.CheckIfPropertyNameExists(new Worker()))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        StringBuilder stringBuilder = new StringBuilder("select Worker.Id, Worker.FirstName, Worker.LastName, Worker.PositionId," +
                            " WorkPosition.PositionName, WorkPosition.Description, Worker.CreatedByUser, Worker.UpdatedByUser, Worker.DateCreated," +
                            " Worker.DateUpdated from Worker left join WorkPosition on Worker.PositionId = WorkPosition.Id");
                        if (filter != null)
                        {
                            stringBuilder.AppendFormat(" where WorkPosition.PositionName like '%{0}%'", filter.SearchQuery);
                        }
                        stringBuilder.AppendFormat(" order by {0} {1} offset {2} rows fetch next {3} rows only",
                        sorting.OrderName, sorting.OrderDirection, paging.PageNumber * paging.PageSize, paging.PageSize);
                        using (SqlCommand command = new SqlCommand(stringBuilder.ToString(), connection))
                        {
                            await connection.OpenAsync();
                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    workers.Add(new Worker()
                                    {
                                        FirstName = reader["FirstName"].ToString(),
                                        LastName = reader["LastName"].ToString(),
                                        Id = Guid.Parse(reader["id"].ToString()),
                                        PositionId = Guid.Parse(reader["PositionId"].ToString()),
                                        DateCreated = DateTime.Parse(reader["DateCreated"].ToString()),
                                        workPosition = new WorkPosition()
                                        {
                                            Id = Guid.Parse(reader["PositionId"].ToString()),
                                            PositionName = reader["PositionName"].ToString(),
                                            Description = reader["description"].ToString()
                                        }
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return workers;
        }

        public async Task<IWorker> Get(Guid id)
        {
            IWorker worker = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    StringBuilder stringBuilder = new StringBuilder("select Worker.Id, Worker.FirstName, Worker.LastName, Worker.PositionId," +
                            " WorkPosition.PositionName, WorkPosition.Description, Worker.CreatedByUser, Worker.UpdatedByUser, Worker.DateCreated," +
                            " Worker.DateUpdated from Worker left join WorkPosition on Worker.PositionId = WorkPosition.Id");
                    using (SqlCommand command = new SqlCommand(stringBuilder.ToString(), connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var x = reader["DateUpdated"];
                                var y = reader["DateUpdated"].ToString() ?? null;
                                worker = new Worker()
                                {
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Id = Guid.Parse(reader["id"].ToString()),
                                    PositionId = Guid.Parse(reader["PositionId"].ToString()),
                                    DateCreated = DateTime.Parse(reader["DateCreated"].ToString()),
                                    workPosition = new WorkPosition()
                                    {
                                        Id = Guid.Parse(reader["PositionId"].ToString()),
                                        PositionName = reader["PositionName"].ToString(),
                                        Description = reader["description"].ToString()
                                    }
                                };
                            }
                            return worker;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IWorker> GetWorkerByAvailability(Guid workerAvailabilityId)
        {
            using (SqlConnection connectionAsync = new SqlConnection(connectionString))
            {
                IWorker worker = new Worker();
                SqlCommand findAvailability = new SqlCommand("Select * From Worker Left join WorkerAvailability on WorkerAvailability.workerId = Worker.Id where WorkerAvailability.Id = @workerAvailabilityId ;", connectionAsync);
                await connectionAsync.OpenAsync();
                findAvailability.Parameters.AddWithValue("@workerAvailabilityId", workerAvailabilityId);
                SqlDataReader readerAsync = await findAvailability.ExecuteReaderAsync();
                if (readerAsync.HasRows)
                {
                    await readerAsync.ReadAsync();
                    worker.Id = Guid.Parse(readerAsync[0].ToString());
                    worker.FirstName = readerAsync[1].ToString();
                    worker.LastName = readerAsync[2].ToString();
                    worker.PositionId = Guid.Parse(readerAsync[3].ToString());
                    worker.DateCreated = DateTime.Parse(readerAsync[4].ToString());
                    worker.DateUpdated = DateTime.Parse(readerAsync[5].ToString());
                    worker.CreatedByUser = Guid.Parse(readerAsync[4].ToString());
                    worker.UpdatedByUser = Guid.Parse(readerAsync[4].ToString());
                }
                readerAsync.Close();
                connectionAsync.Close();
                return worker;
            }
        }
        public async Task<bool> Post(IWorker worker)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Insert into Worker values(@id, @firstName, @lastName, @positionId," +
                    " @dateCreated,@dateUpdated, @createdByUser, @updatedByUser)", connection))
                {
                    command.Parameters.AddWithValue("@id", worker.Id);
                    command.Parameters.AddWithValue("@firstName", worker.FirstName);
                    command.Parameters.AddWithValue("@lastName", worker.LastName);
                    command.Parameters.AddWithValue("@positionId", worker.PositionId);
                    command.Parameters.AddWithValue("@dateCreated", worker.DateCreated);
                    command.Parameters.AddWithValue("@dateUpdated", worker.DateUpdated);
                    command.Parameters.AddWithValue("@createdByUser", worker.CreatedByUser);
                    command.Parameters.AddWithValue("@updatedByUser", worker.UpdatedByUser);
                    await connection.OpenAsync();
                    try
                    {
                        if (await command.ExecuteNonQueryAsync() != 0)
                        {
                            return true;
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw ex;
                    }
                }
            }
        }

        public async Task<bool> Update(Guid id, IWorker worker)
        {
            Worker updatedWorker = null;
            bool result;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                await connection.OpenAsync();
                using (SqlCommand commandRead = new SqlCommand("Select * from Worker where id=@id", connection))
                {
                    commandRead.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = await commandRead.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            updatedWorker = new Worker()
                            {
                                FirstName = worker.FirstName ?? reader["FirstName"].ToString(),
                                LastName = worker.LastName ?? reader["LastName"].ToString(),
                                PositionId = worker.PositionId == Guid.Parse("00000000-0000-0000-0000-000000000000") ?  Guid.Parse(reader["PositionId"].ToString()) : worker.PositionId,
                                DateUpdated = worker.DateUpdated,
                                UpdatedByUser = worker.UpdatedByUser
                            };
                        }
                    }
                }
                using (SqlCommand commandUpdate = new SqlCommand("Update Worker set FirstName=@firstName, LastName=@lastName, PositionId=@positionId, " +
                    "DateUpdated=@dateUpdated, UpdatedByUser=@updatedByUser where Id=@id", connection))
                {
                    commandUpdate.Parameters.AddWithValue("@firstName", updatedWorker.FirstName);
                    commandUpdate.Parameters.AddWithValue("@lastName", updatedWorker.LastName);
                    commandUpdate.Parameters.AddWithValue("@positionId", updatedWorker.PositionId);
                    commandUpdate.Parameters.AddWithValue("@dateUpdated", updatedWorker.DateUpdated);
                    commandUpdate.Parameters.AddWithValue("@updatedByUser", updatedWorker.UpdatedByUser);
                    commandUpdate.Parameters.AddWithValue("@id", id);
                    if (await commandUpdate.ExecuteNonQueryAsync() != 0)
                    {
                        result = true;
                    }
                    else
                    {
                    result = false;
                    }
                }
            }
            return result;
        }


    }
}

