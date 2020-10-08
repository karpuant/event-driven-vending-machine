using Autofac;
using MediatR;
using System.Reflection;
using VendingMachine.API.Application.Commands;
using VendingMachine.API.Application.DomainEventHandlers;
using VendingMachine.API.Application.Queries;
using VendingMachine.Domain.Abstractions;
using VendingMachine.Domain.Aggregates.Product;
using VendingMachine.Domain.Aggregates.Wallet;
using VendingMachine.Infrastructure;
using VendingMachine.Infrastructure.Repositories;

namespace VendingMachine.API.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterType<DepositQueries>().As<IDepositQueries>();
            builder.RegisterType<ProductQueries>().As<IProductQueries>();
            builder.RegisterType<WalletRepository>().As<IRepository<Wallet>>();
            builder.RegisterType<ProductRepository>().As<IRepository<Product>>();
            builder.RegisterType<VendingMachineUnitOfWork>().As<IUnitOfWork>();

            builder.RegisterAssemblyTypes(typeof(AcceptCoinsCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(DepositChangedEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });
        }
    }
}
