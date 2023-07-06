using APITracker.Repositories;
using APITracker_Service;
using APITracker_Service.Application.Interfaces;
using APITracker_Service.Application.Services;
using APITracker_Service.Data;
using APITracker_Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

try
{
    var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((ctx, services) =>
        {
            services.AddEntityFrameworkSqlServer().AddDbContext<BaseContext>((serviceProvider, dbContextBuilder) =>
            {
                dbContextBuilder.UseSqlServer(ctx.Configuration.GetConnectionString("BaseDatabase"));
            }, contextLifetime: ServiceLifetime.Transient,
               optionsLifetime: ServiceLifetime.Transient);

            services.AddTransient<IBaseRepositorio, BaseRepositorio>();

            services.AddTransient<IEnderecoApiRepository, EnderecoApiRepository>();

            services.AddHostedService<Worker>().AddTransient<IEmail, EmailService>().AddTransient<IHttpService, HttpService>();
        })
        .Build();

    await host.RunAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    throw;
}