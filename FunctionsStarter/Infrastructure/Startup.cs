using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using FunctionsStarter.Infrastructure;
using FunctionsStarter.Infrastructure.IoC;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace FunctionsStarter.Infrastructure
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder) =>
            builder.AddDependencyInjection<AutofacServiceProviderBuilder>();

        public class AutofacServiceProviderBuilder : IServiceProviderBuilder
        {
            private readonly IConfiguration _configuration;

            public AutofacServiceProviderBuilder(IConfiguration configuration) => _configuration = configuration;

            public IServiceProvider Build()
            {
                // .NET Core
                var services = new ServiceCollection();
                services.AddTransient<Features.SetIndividualInfo.CommandValidator>();
                services.AddMediatR();
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Pipelines.ValidationPipeline<,>));
                services.AddSingleton<Junk.NameFormatter>();
                services.AddDbContext<Junk.DatabaseContext>();

                // Autofac
                var builder = new ContainerBuilder();
                builder.Populate(services); // Populate is needed to have support for scopes.

                // TODO: Fix this
                builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(t => t.Name.EndsWith("Validator"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

                builder.RegisterType<AutofacValidatorFactory>().As<IValidatorFactory>().SingleInstance();

                return new AutofacServiceProvider(builder.Build());
            }
        }
    }
}