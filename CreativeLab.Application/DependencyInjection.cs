using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CreativeLab.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(provider =>
                provider.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
