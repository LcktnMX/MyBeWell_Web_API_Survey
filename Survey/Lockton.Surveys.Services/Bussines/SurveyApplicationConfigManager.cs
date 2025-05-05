using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lockton.Surveys.DataAccess.DBModels.Entities;
using Lockton.Surveys.DataAccess.Repositories;
using Lockton.Surveys.Domain.Model;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lockton.Surveys.Services.Bussines
{
    public interface ISurveyApplicationConfigManager
    {
        Task<ApplicationDto> Upsert(ApplicationDto dto);
        Task<ApplicationDto> GetById(Guid id);
        Task<IEnumerable<ApplicationDto>> GetByParticipantId(Guid id);
        Task<IEnumerable<ApplicationDto>> GetByLineId(Guid id);
        Task<IEnumerable<ApplicationDto>> GetAll();
        Task Delete(Guid id);
        Task<IEnumerable<SurveyLogMailDto>> GetLogBySurverApplication(Guid id);
        Task SaveLog(SurveyLogMailDto dto);
    }
    public class SurveyApplicationConfigManager : ISurveyApplicationConfigManager
    {
        private readonly IRepository<Survey> _surveyRepository;
        private readonly IRepository<SurveyApplicationContentsAnswer> _answeredRepository;
        private readonly IRepository<SurveyApplicationContentsAnswerObservation> _observationRepository;
        private readonly IRepository<SurveyApplication> _surveyApplicationRepository;
        private readonly IRepository<SurveyApplicationContent> _surveyApplicationContentRepository;
        private readonly IRepository<SurveyLogMailEntity> _surveyLog;
        public SurveyApplicationConfigManager(IRepository<Survey> surveyRepository,
            IRepository<SurveyApplication> surveyApplicationRepository,
            IRepository<SurveyApplicationContent> repositorySurveyApplicationContent,
            IRepository<SurveyLogMailEntity> surveyLog,
            IRepository<SurveyApplicationContentsAnswer> answeredRepository,
            IRepository<SurveyApplicationContentsAnswerObservation> observationRepository)
        {
            _surveyRepository = surveyRepository;
            _surveyApplicationRepository = surveyApplicationRepository;
            _surveyApplicationContentRepository = repositorySurveyApplicationContent;
            _surveyLog = surveyLog;
            _observationRepository = observationRepository;
            _answeredRepository = answeredRepository;

        }

        public async Task<ApplicationDto> Upsert(ApplicationDto dto)
        {
            SurveyApplication surveyApplicationDb = null;
            if (dto.Id != null || dto.Id != Guid.NewGuid())
                surveyApplicationDb = (await _surveyApplicationRepository.GetByCondition(x => x.Id == dto.Id)
                                                .Include(x => x.Container)
                                                .FirstOrDefaultAsync());
            if (surveyApplicationDb == null)
            {
                surveyApplicationDb = (await _surveyApplicationRepository
                    .Create(new()
                    {
                        IdParticipant = dto.IdParticipant,
                        Title = dto.Title,
                        Responsible = dto.Responsible,
                        Instructions = dto.Instructions,
                        StartDate = dto.StartDate,
                        EndDate = dto.EndDate,
                        Sent = false,
                        Progress = 0,
                        SentMessage = dto.SentMessage,
                        FinishMessage = dto.FinishMessage,
                        CreatedUser = dto.CreatedUser,
                        CreatedDate = dto.CreatedDate,
                        ModifiedUser = dto.ModifiedUser,
                        ModifiedDate = dto.ModifiedDate,
                        Finished = false,
                        Active = true
                    })).Entity;

                await _surveyApplicationRepository.SaveChanges();

                foreach (var content in dto.Container.OrderBy(x => x.Position))
                {
                    var contentDb = (await _surveyApplicationContentRepository.Create(new()
                    {
                        IdSurveyApplication = surveyApplicationDb.Id,
                        IdSurvey = content.IdSurvey,
                        Description = content.Description,
                        Position = content.Position,
                        Active = true,
                    })).Entity;

                    await _surveyApplicationContentRepository.SaveChanges();
                }
            }
            else
            {
                surveyApplicationDb.IdParticipant = dto.IdParticipant;
                surveyApplicationDb.Title = dto.Title;
                surveyApplicationDb.Responsible = dto.Responsible;
                surveyApplicationDb.Instructions = dto.Instructions;
                surveyApplicationDb.StartDate = dto.StartDate;
                surveyApplicationDb.EndDate = dto.EndDate;
                surveyApplicationDb.Sent = dto.Sent;
                surveyApplicationDb.Progress = dto.Progress;
                surveyApplicationDb.SentMessage = dto.SentMessage;
                surveyApplicationDb.FinishMessage = dto.FinishMessage;
                surveyApplicationDb.CreatedUser = dto.CreatedUser;
                surveyApplicationDb.CreatedDate = dto.CreatedDate;
                surveyApplicationDb.ModifiedUser = dto.ModifiedUser;
                surveyApplicationDb.ModifiedDate = dto.ModifiedDate;
                surveyApplicationDb.Finished = dto.Finished;
                surveyApplicationDb.Active = dto.Active;

                _surveyApplicationRepository.Update(surveyApplicationDb);

                await _surveyApplicationRepository.SaveChanges();

                foreach (var content in dto.Container)
                {
                    var contentDb = surveyApplicationDb.Container.Where(x => x.Id == content.Id).FirstOrDefault();

                    if (contentDb == null)
                    {
                        await _surveyApplicationContentRepository.Create(new()
                        {
                            IdSurveyApplication = surveyApplicationDb.Id,
                            IdSurvey = content.IdSurvey,
                            Description = content.Description,
                            Active = content.Active,
                            Position = content.Position
                        });
                    }
                    else
                    {
                        contentDb.IdSurveyApplication = surveyApplicationDb.Id;
                        contentDb.IdSurvey = content.IdSurvey;
                        contentDb.Position = content.Position;
                        contentDb.Description = content.Description;
                        contentDb.Active = content.Active;

                        _surveyApplicationContentRepository.Update(contentDb);

                    }
                    await _surveyApplicationContentRepository.SaveChanges();
                }

            }
            //var elements = _surveyApplicationContentRepository.GetByCondition(x => !dto.Container.Select(y => y.Id).ToList().Contains(x.Id)).ToList();

            //foreach (var avoidElement in elements)
            //{
            //    avoidElement.Active = false;
            //    _surveyApplicationContentRepository.Update(avoidElement);
            //    await _surveyApplicationContentRepository.SaveChanges();
            //}

            return await GetById(surveyApplicationDb.Id);

        }

        public async Task<ApplicationDto> GetById(Guid id)
        {
            var surveyApplicationDB = (await _surveyApplicationRepository.GetByCondition(x => x.Id == id)
                                                   .Include(x => x.Container)
                                                   .FirstOrDefaultAsync());

            return surveyApplicationDB.Adapt<ApplicationDto>() ?? null;
        }

        public async Task<IEnumerable<ApplicationDto>> GetByLineId(Guid id)
        {
            var surveysInLine = (await _surveyRepository.GetAll().Where(x => x.LineId == id)?.ToListAsync())?.Select(x => x.Id) ?? null;
            if (surveysInLine == null || !surveysInLine.Any())
                return null;

            var applicationsInLine = (await _surveyApplicationContentRepository.GetByCondition(x => surveysInLine.ToList().Contains(x.IdSurvey.Value)).ToListAsync())?.Select(x => x.IdSurveyApplication) ?? null;
            if (applicationsInLine == null || !applicationsInLine.Any())
                return null;

            var surveysApplicationDB = (await _surveyApplicationRepository.GetAll()
                                                      .Include(x => x.Container)
                                                      .ToListAsync())?.Where(x => applicationsInLine.Contains(x.Id)) ?? null;


            return surveysApplicationDB == null || !surveysApplicationDB.Any() ? new List<ApplicationDto>() :
                    surveysApplicationDB?.Adapt<IEnumerable<ApplicationDto>>().OrderByDescending(x => x.StartDate) ?? null;

        }

        public async Task<IEnumerable<ApplicationDto>> GetByParticipantId(Guid id)
        {
            var surveysApplicationDB = (await _surveyApplicationRepository.GetByCondition(x => x.IdParticipant == id)
                                                     .Include(x => x.Container)
                                                     .ToListAsync());


            return surveysApplicationDB == null || !surveysApplicationDB.Any() ? new List<ApplicationDto>() :
                    surveysApplicationDB?.Adapt<IEnumerable<ApplicationDto>>().OrderByDescending(x => x.StartDate) ?? null;
        }

        public async Task<IEnumerable<ApplicationDto>> GetAll()
        {
            var surveysDB = await _surveyApplicationRepository.GetAll()
                                                  .Include(x => x.Container)
                                                  .ToListAsync();

            return surveysDB?.Adapt<IEnumerable<ApplicationDto>>().OrderByDescending(x => x.StartDate) ?? null;
        }

        public async Task Delete(Guid id)
        {
            var surveyApplicationDB = await _surveyApplicationRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
            if (surveyApplicationDB == null)
                throw new Exception("No existe el setup");

            surveyApplicationDB.Active = false;
            _surveyApplicationRepository.Update(surveyApplicationDB);
            await _surveyApplicationRepository.SaveChanges();

        }

        public async Task<IEnumerable<SurveyLogMailDto>> GetLogBySurverApplication(Guid id)
        {
            var result = (await _surveyLog.GetByCondition(x => x.SurveyApplicationId == id).ToListAsync());


            return result == null || !result.Any() ? new List<SurveyLogMailDto>() :
                    result?.Adapt<IEnumerable<SurveyLogMailDto>>().OrderByDescending(x => x.SentDate) ?? null;
        }

        public async Task SaveLog(SurveyLogMailDto dto)
        {
            try
            {
                var surveyLogMailEntity = (await _surveyLog
                       .Create(new()
                       {
                           SentUser = dto.SentUser,
                           Subject = dto.Subject,
                           Receiver = dto.Receiver,
                           Body = dto.Body,
                           SentDate = dto.SentDate.ToString(),
                           Success = dto.Success,
                           SurveyApplicationId = dto.SurveyApplicationId,
                           Error = dto.Error,
                       })).Entity;

                await _surveyLog.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }


    }
}
