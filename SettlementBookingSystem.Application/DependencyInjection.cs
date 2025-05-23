﻿using System.Linq;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SettlementBookingSystem.Application.Behaviours;

namespace SettlementBookingSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddAutoMapper(assembly);

            services.AddFluentValidation(assembly);
            services.AddMediatR(cfg => cfg.AsScoped(), assembly);

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(UnhandledExceptionBehaviour<,>)
            );
            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(RequestValidationBehaviour<,>)
            );

            return services;
        }

        private static void AddFluentValidation(this IServiceCollection services, Assembly assembly)
        {
            var validatorType = typeof(IValidator<>);

            var validatorTypes = assembly
                .GetExportedTypes()
                .Where(t =>
                    t.GetInterfaces()
                        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType)
                )
                .ToList();

            foreach (var validator in validatorTypes)
            {
                var requestType = validator
                    .GetInterfaces()
                    .Where(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)
                    )
                    .Select(i => i.GetGenericArguments()[0])
                    .First();

                var validatorInterface = validatorType.MakeGenericType(requestType);

                services.AddTransient(validatorInterface, validator);
            }
        }
    }
}
