using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduler.Common;
using WorkScheduler.Model.Common;
using WorkScheduler.Repository.Common;
using WorkScheduler.Model;
using System.Security.Principal;
using System.Runtime.InteropServices.ComTypes;

namespace WorkScheduler.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private static string connection = Environment.GetEnvironmentVariable("SQLConn", EnvironmentVariableTarget.User);
        IWorkerStatusRepository workerStatusRepository;
        public RequestRepository(IWorkerStatusRepository workerStatusRepository)
        {
            this.workerStatusRepository = workerStatusRepository;
        }
        public async System.Threading.Tasks.Task CreateRequestAsync(IRequest request)
        {
            SqlConnection conn = new SqlConnection(connection);
            await conn.OpenAsync();
            SqlCommand command = new SqlCommand("INSERT INTO Request VALUES (@Id, @RequestStatus, @StartDate, @EndDate, @Description, @WorkerStatusId, @DateCreated, @DateUpdated, @CreatedByUser, @UpdatedByUser)", conn);

            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = request.Id;
            command.Parameters.Add("@RequestStatus", SqlDbType.VarChar).Value = request.RequestStatus;
            command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = request.StartDate;
            command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = request.EndDate;
            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = request.Description;
            command.Parameters.Add("@WorkerStatusId", SqlDbType.UniqueIdentifier).Value = request.WorkerStatusId;
            command.Parameters.Add("@DateCreated", SqlDbType.DateTime).Value = request.DateCreated;
            command.Parameters.Add("@DateUpdated", SqlDbType.DateTime).Value = request.DateUpdated;
            command.Parameters.Add("@CreatedByUser", SqlDbType.UniqueIdentifier).Value = request.CreatedByUser;
            command.Parameters.Add("@UpdatedByUser", SqlDbType.UniqueIdentifier).Value = request.UpdatedByUser;


            SqlDataAdapter adapter = new SqlDataAdapter
            {
                InsertCommand = command
            };

            await adapter.InsertCommand.ExecuteNonQueryAsync();
            conn.Close();
        }

        public async Task<bool> CreateOnTaskWorkerAsync(Guid workerAvailabilityId, IRequest request,Guid adminId)
        {
            SqlConnection conn = new SqlConnection(connection);
            await conn.OpenAsync();
            Guid workerStatus = await workerStatusRepository.GetOnTaskIdAsync();
            SqlCommand command = new SqlCommand("INSERT INTO Request VALUES (@Id, @RequestStatus, @StartDate, @EndDate, @Description, @WorkerStatusId, @DateCreated, @DateUpdated, @CreatedByUser, @UpdatedByUser)", conn);

            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = request.Id;
            command.Parameters.Add("@RequestStatus", SqlDbType.VarChar).Value = request.RequestStatus;
            command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = request.StartDate;
            command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = request.EndDate;
            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = request.Description;
            command.Parameters.Add("@WorkerStatusId", SqlDbType.UniqueIdentifier).Value = workerStatus;
            command.Parameters.Add("@DateCreated", SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@DateUpdated", SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@CreatedByUser", SqlDbType.UniqueIdentifier).Value = adminId;
            command.Parameters.Add("@UpdatedByUser", SqlDbType.UniqueIdentifier).Value = adminId;

            SqlDataAdapter adapter = new SqlDataAdapter
            {
                InsertCommand = command
            };

            await adapter.InsertCommand.ExecuteNonQueryAsync();
            conn.Close();
            return false;
        }

        public async System.Threading.Tasks.Task DeleteRequestAsync(Guid id)
        {
            SqlConnection conn = new SqlConnection(connection);
            await conn.OpenAsync();
            SqlCommand command = new SqlCommand("DELETE FROM Request WHERE Id = @Id", conn);

            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;

            SqlDataAdapter adapter = new SqlDataAdapter
            {
                DeleteCommand = command
            };

            await adapter.DeleteCommand.ExecuteNonQueryAsync();
            conn.Close();
        }

        public async Task<List<IRequest>> GetAllRequestsAsync(Sorting sorting, Paging paging)
        {
            SqlConnection conn = new SqlConnection(connection);
            await conn.OpenAsync();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT * From Request");
            stringBuilder.AppendFormat("ORDER BY {0} {1} " +
                "OFFSET ({2}-1) * {3} ROWS " +
                "FETCH NEXT {3} ROWS ONLY", sorting.OrderName, sorting.OrderDirection, paging.PageNumber, paging.PageSize);

            SqlCommand command = new SqlCommand(stringBuilder.ToString(), conn);

            SqlDataReader reader = await command.ExecuteReaderAsync();

            List<IRequest> requests = new List<IRequest>();

            while (await reader.ReadAsync())
            {
                Request request = new Request 
                {
                    Id = (Guid)reader[0],
                    RequestStatus = (string)reader[1],
                    StartDate = (DateTime)reader[2],
                    EndDate = (DateTime)reader[3],
                    Description = (string)reader[4],
                    WorkerStatusId = (Guid)reader[5],
                    DateCreated = (DateTime)reader[6],
                    DateUpdated = (DateTime)reader[7],
                    CreatedByUser = (Guid)reader[8],
                    UpdatedByUser = (Guid)reader[9],
                };
                requests.Add(request);
            }

            conn.Close();
            return requests;
        }

        public async Task<IRequest> GetOneRequestAsync(Guid id)
        {
            SqlConnection conn = new SqlConnection(connection);
            await conn.OpenAsync();
            SqlCommand command = new SqlCommand("SELECT * FROM Request WHERE Id = @Id ", conn);
            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;

            SqlDataReader reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();

            Request request = new Request
            {
                Id = (Guid)reader[0],
                RequestStatus = (string)reader[1],
                StartDate = (DateTime)reader[2],
                EndDate = (DateTime)reader[3],
                Description = (string)reader[4],
                WorkerStatusId = (Guid)reader[5],
                DateCreated = (DateTime)reader[6],
                DateUpdated = (DateTime)reader[7],
                CreatedByUser = (Guid)reader[8],
                UpdatedByUser = (Guid)reader[9],
            };

            conn.Close();
            return request;
        }

        public async System.Threading.Tasks.Task UpdateRequestAsync(Guid id, IRequest request)
        {
            SqlConnection conn = new SqlConnection(connection);
            await conn.OpenAsync();

            SqlCommand command = new SqlCommand("UPDATE Request SET RequestStatus = @RequestStatus, StartDate = @StartDate, EndDate = @EndDate, Description = @Description, WorkerStatusId = @WorkerStatusId, DateCreated = @DateCreated, DateUpdated = @DateUpdated, CreatedByUser = @CreatedByUser, UpdatedByUser = @UpdatedByUser WHERE id = @Id", conn);
            command.Parameters.Add("@RequestStatus", SqlDbType.VarChar).Value = request.RequestStatus;
            command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = request.StartDate;
            command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = request.EndDate;
            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = request.Description;
            command.Parameters.Add("@WorkerStatusId", SqlDbType.UniqueIdentifier).Value = request.WorkerStatusId;
            command.Parameters.Add("@DateCreated", SqlDbType.DateTime).Value = request.DateCreated;
            command.Parameters.Add("@DateUpdated", SqlDbType.DateTime).Value = request.DateUpdated;
            command.Parameters.Add("@CreatedByUser", SqlDbType.UniqueIdentifier).Value = request.CreatedByUser;
            command.Parameters.Add("@UpdatedByUser", SqlDbType.UniqueIdentifier).Value = request.UpdatedByUser;

            SqlDataAdapter adapter = new SqlDataAdapter
            {
                UpdateCommand = command
            };
            await adapter.UpdateCommand.ExecuteNonQueryAsync();
            conn.Close();
        }
    }
}
