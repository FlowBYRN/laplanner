using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Trainingsplanner.Postgres.AuthorizationHandler;
using Trainingsplanner.Postgres.DataAccess;
using Trainingsplanner.Postgres.DataAccess.Implementation;

namespace Trainingsplanner.Postgres
{
    internal static class Bootstrapper
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
            => services.AddScoped<ITrainingsModuleRepository, TrainingsModuleRepository>()
                .AddScoped<ITrainigsAppointmentRepository, TrainingsAppointmentRepository>()
                .AddScoped<ITrainingsExerciseRepository, TrainingsExerciseRepository>()
                .AddScoped<ITrainingsGroupRepository, TrainingsGroupRepository>()
                .AddScoped<ITrainingsGroupUserRepository, TrainingsGroupUserRepository>()
                .AddScoped<ITrainingsModuleTagRepository, TrainingsModuleTagRepository>()
                .AddSingleton<IAuthorizationHandler, TrainingsGroupHandler>()
                .AddSingleton<IAuthorizationHandler, TrainingsAppointmentHandler>()
                .AddSingleton<IAuthorizationHandler, TrainingsModuleHandler>()
                .AddSingleton<IAuthorizationHandler, HasRoleHandler>()
            ;

    }
}