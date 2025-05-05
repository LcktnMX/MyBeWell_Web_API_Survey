using Lockton.Surveys.DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lockton.Surveys.Web.Configuration
{
    public static class DataBaseConfigurationExtension
    {
        public static void addDataBaseService(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<AppDbContext>(options => { options.UseSqlServer(configuration.GetConnectionString("SecurityConnection"));}, ServiceLifetime.Transient);
            service.AddDbContext<WellnessContext>(options => { options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); }, ServiceLifetime.Transient);
        }
    }
}
