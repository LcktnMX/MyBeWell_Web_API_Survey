using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lockton.Surveys.Web.Configuration
{
    public static class AutoMapperConfigurationExtension
    {
        public static void addAutoMapperService(this IServiceCollection service)
        {
            //service.AddAutoMapper(typeof(UserMapper));
        }
    }
}
