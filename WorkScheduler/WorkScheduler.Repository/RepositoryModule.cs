using Autofac;
using WorkScheduler.Repository.Common;

namespace WorkScheduler.Repository
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<NameRepository>().As<INAmeRepository>();
            builder.RegisterType<AccessLevelRepository>().As<IAccessLevelRepository>();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>();
            builder.RegisterType<RequestRepository>().As<IRequestRepository>();
            builder.RegisterType<ScheduleTaskWorkerRepository>().As<IScheduleTaskWorkerRepository>();
            builder.RegisterType<TaskRepository>().As<ITaskRepository>();
            builder.RegisterType<TaskStatusRepository>().As<ITaskStatusRepository>();
            builder.RegisterType<WorkerAvailabilityRepository>().As<IWorkerAvailabilityRepository>();
            builder.RegisterType<WorkerRepository>().As<IWorkerRepository>();
            builder.RegisterType<WorkerStatusRepository>().As<IWorkerStatusRepository>();
            builder.RegisterType<WorkPositionRepository>().As<IWorkPositionRepository>();

        }
    }
}
