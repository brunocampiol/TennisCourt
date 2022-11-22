using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisCourt.Infra.Data.Context.Configurations
{
    public static class ContextConfiguration
    {

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                 .GetRequiredService<IServiceScopeFactory>()
                 .CreateScope();

            using var context = serviceScope.ServiceProvider.GetService<TennisCourtContext>();         
            //context.Database.Migrate();
        }

    }
}
