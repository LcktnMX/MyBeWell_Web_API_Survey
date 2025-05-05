using Lockton.Surveys.DataAccess.DBModels;
using Lockton.Surveys.DataAccess.Repositories;
using Lockton.Surveys.Services.Utils;
using Lockton.Surveys.DataAccess.DataContext;
using Lockton.Surveys.Services;
using Microsoft.Extensions.DependencyInjection;
using Lockton.Surveys.Services.Bussines;
using Lockton.Surveys.DataAccess.DBModels.Entities;
using Lockton.Surveys.Services.Mapper;
using MapsterMapper;

namespace Lockton.Surveys.Web.Configuration
{
    public static class ServicesConfigurationExtension
    {

        public static void addServices(this IServiceCollection service)
        {
            //Database
            service.AddSingleton<DapperContext>();
            service.AddSingleton<IUserUtil, UserUtil>();

            //Repositories
            //Security
            service.AddTransient<IRepository<User>, GenericRepository<User>>();
            //Wellness
            service.AddTransient<IRepository<Answer>, WellnessRepository<Answer>>();
            service.AddTransient<IRepository<Line>, WellnessRepository<Line>>();
            service.AddTransient<IRepository<Participant>, WellnessRepository<Participant>>();
            service.AddTransient<IRepository<Question>, WellnessRepository<Question>>();
            service.AddTransient<IRepository<QuestionType>, WellnessRepository<QuestionType>>();
            service.AddTransient<IRepository<Section>, WellnessRepository<Section>>();
            service.AddTransient<IRepository<Survey>, WellnessRepository<Survey>>();
            service.AddTransient<IRepository<SurveyApplication>, WellnessRepository<SurveyApplication>>();
            service.AddTransient<IRepository<SurveyApplicationContent>, WellnessRepository<SurveyApplicationContent>>();
            service.AddTransient<IRepository<SurveyApplicationContentsAnswer>, WellnessRepository<SurveyApplicationContentsAnswer>>();
            service.AddTransient<IRepository<SurveyApplicationContentsAnswerObservation>, WellnessRepository<SurveyApplicationContentsAnswerObservation>>();
            service.AddTransient<IRepository<SurveyLogMailEntity>, WellnessRepository<SurveyLogMailEntity>>();
            service.AddTransient<ILogRepository, LogRepository>();

            //Services            
            service.AddTransient<ISurveyManager, SurveyManager>();
            service.AddTransient<ICatalogManager, CatalogManager>();
            service.AddTransient<IParticipantManager, ParticipantManager>();
            service.AddTransient<ISurveyApplicationConfigManager, SurveyApplicationConfigManager>();
            service.AddTransient<IApplicantManager, ApplicantManager>();

            var mapConfig = MapsterConfigurator.CreateTypeAdapterConfig();
            service.AddSingleton(mapConfig);
            service.AddScoped<IMapper, MapsterMapper.ServiceMapper>();

        }
    }
}
