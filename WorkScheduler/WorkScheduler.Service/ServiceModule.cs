

using Autofac;
using WorkScheduler.Service.Common;

namespace WorkScheduler.Service
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<NameService>().As<INAmeService>();
            builder.RegisterType<AccessLevelService>().As<IAccessLevelService>();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<RequestService>().As<IRequestService>();
            builder.RegisterType<ScheduleTaskWorkerService>().As<IScheduleTaskWorkerService>();
            builder.RegisterType<TaskService>().As<ITaskService>();
            builder.RegisterType<TaskStatusService>().As<ITaskStatusService>();
            builder.RegisterType<WorkerAvailabilityService>().As<IWorkerAvailabilityService>();
            builder.RegisterType<WorkerService>().As<IWorkerService>();
            builder.RegisterType<WorkerStatusService>().As<IWorkerStatusService>();
            builder.RegisterType<WorkPositionService>().As<IWorkPositionService>();
        }
    }
}
