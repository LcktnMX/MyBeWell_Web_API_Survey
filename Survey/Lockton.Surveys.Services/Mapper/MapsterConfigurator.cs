using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lockton.Surveys.DataAccess.DBModels.Entities;
using Lockton.Surveys.Domain.Model;
using Mapster;

namespace Lockton.Surveys.Services.Mapper
{
    public  class MapsterConfigurator
    {
        public static TypeAdapterConfig CreateTypeAdapterConfig()
        { 
            var mapConfig= new TypeAdapterConfig();

            mapConfig.NewConfig<Survey, SurveyDto>();            
            mapConfig.NewConfig<SurveyDto, Survey>();


            return mapConfig;
        }
    }
}
