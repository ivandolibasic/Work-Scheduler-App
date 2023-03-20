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
    public class WorkPositionRepository : IWorkPositionRepository
    {
        public static string connectionString = Environment.GetEnvironmentVariable("SQLConn", EnvironmentVariableTarget.User);

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("Select * from WorkPosition where Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                using (var command2 = new SqlCommand("Delete from WorkPosition where Id=@id", connection))
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





        public async Task<List<IWorkPosition>> Get(Sorting sorting, Paging paging)
        {
            List<IWorkPosition> workPositions = new List<IWorkPosition>();
            if (sorting.CheckIfPropertyNameExists(new WorkPosition()) && sorting.OrderDirectionIsValid())
            {

                SqlConnection connection = new SqlConnection(connectionString);
                StringBuilder stringBuilder = new StringBuilder("Select * from WorkPosition");
                stringBuilder.AppendFormat(" order by {0} {1} offset {2} rows fetch next {3} rows only",
                    sorting.OrderName, sorting.OrderDirection, paging.PageNumber * paging.PageSize, paging.PageSize);
                SqlCommand command = new SqlCommand(stringBuilder.ToString(), connection);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    workPositions.Add(new WorkPosition
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        PositionName = reader["PositionName"].ToString(),
                        Description = reader["Description"].ToString(),
                        CreatedByUser = Guid.Parse(reader["CreatedByUser"].ToString()),
                        UpdatedByUser = Guid.Parse(reader["UpdatedByUser"].ToString()),
                        DateCreated = DateTime.Parse(reader["DateCreated"].ToString()),
                        DateUpdated = DateTime.Parse(reader["DateUpdated"].ToString())
                    });
                }
                reader.Close();
                connection.Close();

            }
            return workPositions;
        }

        public async Task<IWorkPosition> Get(Guid id)
        {
            IWorkPosition workPosition = null;
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("Select * from WorkPosition where Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    workPosition = new WorkPosition
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        PositionName = reader["PositionName"].ToString(),
                        Description = reader["Description"].ToString(),
                        CreatedByUser = Guid.Parse(reader["CreatedByUser"].ToString()),
                        UpdatedByUser = Guid.Parse(reader["UpdatedByUser"].ToString()),
                        DateCreated = DateTime.Parse(reader["DateCreated"].ToString()),
                        DateUpdated = DateTime.Parse(reader["DateUpdated"].ToString())
                    };
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return workPosition;
        }

        public async Task<bool> Post(IWorkPosition position)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("insert into WorkPosition values " +
                    "(@id, @positionName, @description, @dateCreated, @dateUpdated, @createdByUser, @updatedByUser)", connection))
                {
                    command.Parameters.AddWithValue("@id", position.Id);
                    command.Parameters.AddWithValue("@positionName", position.PositionName);
                    command.Parameters.AddWithValue("@description", position.Description);
                    command.Parameters.AddWithValue("@dateCreated", position.DateCreated);
                    command.Parameters.AddWithValue("@dateUpdated", position.DateUpdated);
                    command.Parameters.AddWithValue("@createdByUser", position.CreatedByUser);
                    command.Parameters.AddWithValue("@updatedByUser", position.UpdatedByUser);
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

        public async Task<bool> Update(Guid id, IWorkPosition workPosition)
        {
            WorkPosition updatedPosition = null;
            bool result;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand commandRead = new SqlCommand("select * from WorkPosition where Id=@id", connection))
                {
                    commandRead.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = await commandRead.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            updatedPosition = new WorkPosition
                            {
                                PositionName = workPosition.PositionName ?? reader["PositionName"].ToString(),
                                Description = workPosition.Description ?? reader["Description"].ToString(),
                                DateUpdated = workPosition.DateUpdated,
                                UpdatedByUser = workPosition.UpdatedByUser
                            };
                        }
                    }
                }
                using (SqlCommand commandUpdate = new SqlCommand("update WorkPosition set PositionName=@positionName , " +
                    "Description=@description , UpdatedByUser=@updatedByUser , DateUpdated=@dateUpdated where Id=@id", connection))
                {
                    commandUpdate.Parameters.AddWithValue("@description", updatedPosition.Description);
                    commandUpdate.Parameters.AddWithValue("@positionName", updatedPosition.PositionName);
                    commandUpdate.Parameters.AddWithValue("@updatedByUser", updatedPosition.UpdatedByUser);
                    commandUpdate.Parameters.AddWithValue("@dateUpdated", updatedPosition.DateUpdated);
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
