using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Lockton.Surveys.DataAccess.DBModels.Entities;
using Lockton.Surveys.DataAccess.Repositories;
using Lockton.Surveys.Domain.Model;
using Mapster;
using Microsoft.EntityFrameworkCore;



namespace Lockton.Surveys.Services.Bussines
{
    public interface ISurveyManager
    {
        Task<SurveyDto> Upsert(SurveyDto dto);
        Task<SurveyDto> GetById(Guid id);
        Task<IEnumerable<SurveyDto>> GetAll();
        Task<IEnumerable<SurveyDto>> GetByLineId(Guid lineId);
        Task<IEnumerable<SurveyDto>> GetVersions(Guid id);
        Task DeleteSurvey(Guid id);
    }
    public class SurveyManager : ISurveyManager
    {
        private readonly IRepository<Survey> _surveyRepository;
        private readonly IRepository<Section> _sectionRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        public SurveyManager(IRepository<Survey> surveyRepository,
                             IRepository<Section> sectionRepository,
                             IRepository<Question> questionRepository,
                             IRepository<Answer> answerRepository)
        {
            _surveyRepository = surveyRepository;
            _sectionRepository = sectionRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }
        public async Task<SurveyDto> Upsert(SurveyDto dto)
        {
            Survey surveyDB = null;
            if (dto.Id != null && dto.Id != Guid.Empty)
                surveyDB = await _surveyRepository.GetByCondition(x => x.Id == dto.Id)
                                                   .Include(x => x.Sections)
                                                   .ThenInclude(x => x.Questions)
                                                   .ThenInclude(x => x.Answers)
                                                   .FirstOrDefaultAsync();

            if (surveyDB == null || (surveyDB != null && surveyDB.Version != dto.Version))
                dto = await Insert(surveyDB, dto);
            else
                dto = await Update(surveyDB, dto);

            return dto;
        }

        private async Task<SurveyDto> Insert(Survey previousVersion, SurveyDto dto)
        {
            var surveyDb = await _surveyRepository
                .Create(new()
                {
                    Name = dto.Name,
                    NameEs = dto.NameEs,
                    Description = dto.Description,
                    DescriptionEs = dto.DescriptionEs,
                    Version = dto.Version,
                    CreatedDate = DateTime.Now,
                    CreatedUser = dto.User,
                    LineId = dto.LineId,
                    Active = dto.Active
                });

            await _surveyRepository.SaveChanges();

            if (previousVersion != null)
            {
                previousVersion.Active = false;
                _surveyRepository.Update(previousVersion);
            }

            surveyDb.Entity.FingerPrint = previousVersion?.FingerPrint ?? surveyDb.Entity.Id;

            await _surveyRepository.SaveChanges();

            foreach (var section in dto.Sections)
            {
                var sectionDb = await _sectionRepository
                    .Create(new()
                    {
                        IdSurvey = surveyDb.Entity.Id,
                        Title = section.Title,
                        TitleEs = section.TitleEs,
                        Description = section.Description,
                        DescriptionEs = section.DescriptionEs,
                        Position = section.Position,
                        Active = section.Active,
                    });
                await _sectionRepository.SaveChanges();

                foreach (var question in section.Questions)
                {
                    var questionDb = await _questionRepository
                        .Create(new()
                        {
                            IdSurvey = surveyDb.Entity.Id,
                            IdSection = sectionDb.Entity.Id,
                            IdType = question.IdType,
                            Contents = question.Contents,
                            ContentsEs = question.ContentsEs,
                            Header = question.Header,
                            Footer = question.Footer,
                            HeaderEs = question.HeaderEs,
                            FooterEs = question.FooterEs,
                            Position = question.Position,
                            Required = question.Required,
                            Align = question.Align,
                            ExtraTextQuestion = question.ExtraTextQuestion,
                            Active = question.Active,
                        });
                    await _questionRepository.SaveChanges();

                    foreach (var answer in question.Answers)
                    {
                        var answerDb = await _answerRepository
                            .Create(new()
                            {
                                IdQuestion = questionDb.Entity.Id,
                                Position = answer.Position,
                                Contents = answer.Contents,
                                ContentsEs = answer.ContentsEs,
                                Value = answer.Value,
                                Type = answer.Type,
                                Active = answer.Active,
                            });
                        await _answerRepository.SaveChanges();
                    }
                }
            }

            return (await GetById(surveyDb.Entity.Id)).Adapt<SurveyDto>();
        }

        private async Task<SurveyDto> Update(Survey surveyDb, SurveyDto dto)
        {


            surveyDb.Name = dto.Name;
            surveyDb.NameEs = dto.NameEs;
            surveyDb.Description = dto.Description;
            surveyDb.DescriptionEs = dto.DescriptionEs;
            surveyDb.Version = dto.Version;
            surveyDb.CreatedDate = DateTime.Now;
            surveyDb.CreatedUser = dto.User;
            surveyDb.LineId = dto.LineId;
            surveyDb.Active = dto.Active;

            _surveyRepository.Update(surveyDb);
            await _surveyRepository.SaveChanges();


            foreach (var section in dto.Sections)
            {
                var sectionDb = surveyDb.Sections.Where(x => x.Id == section.Id).FirstOrDefault();

                if (sectionDb == null)
                    sectionDb = (await _sectionRepository.Create(new()
                    {
                        IdSurvey = surveyDb.Id,
                        Title = section.Title,
                        TitleEs = section.TitleEs,
                        Description = section.Description,
                        DescriptionEs = section.DescriptionEs,
                        Position = section.Position,
                        Active = section.Active,
                    })).Entity;
                else
                {
                    sectionDb.IdSurvey = surveyDb.Id;
                    sectionDb.Title = section.Title;
                    sectionDb.TitleEs = section.TitleEs;
                    sectionDb.Description = section.Description;
                    sectionDb.DescriptionEs = section.DescriptionEs;
                    sectionDb.Position = section.Position;
                    sectionDb.Active = section.Active;

                    _sectionRepository.Update(sectionDb);
                }

                await _sectionRepository.SaveChanges();

                foreach (var question in section.Questions)
                {
                    var questionDb = sectionDb.Questions.Where(x => x.Id == question.Id).FirstOrDefault();
                    if (questionDb == null)
                        questionDb = (await _questionRepository
                            .Create(new()
                            {
                                IdSurvey = surveyDb.Id,
                                IdSection = sectionDb.Id,
                                IdType = question.IdType,
                                Contents = question.Contents,
                                ContentsEs = question.ContentsEs,
                                Header = question.Header,
                                Footer = question.Footer,
                                Position = question.Position,
                                Required = question.Required,
                                Align = question.Align,
                                ExtraTextQuestion = question.ExtraTextQuestion, 
                                Active = question.Active,
                            })).Entity;
                    else
                    {
                        questionDb.IdSurvey = surveyDb.Id;
                        questionDb.IdSection = sectionDb.Id;
                        questionDb.IdType = question.IdType;
                        questionDb.Contents = question.Contents;
                        questionDb.ContentsEs = question.ContentsEs;
                        questionDb.Header = question.Header;
                        questionDb.Footer = question.Footer;
                        questionDb.HeaderEs = question.HeaderEs;
                        questionDb.FooterEs = question.FooterEs;
                        questionDb.Position = question.Position;
                        questionDb.Required = question.Required;
                        questionDb.Align = question.Align;
                        questionDb.ExtraTextQuestion= question.ExtraTextQuestion;
                        questionDb.Active = question.Active;

                        _questionRepository.Update(questionDb);
                    }

                    await _questionRepository.SaveChanges();

                    foreach (var answer in question.Answers)
                    {
                        var answerDb = questionDb.Answers.Where(x => x.Id == answer.Id).FirstOrDefault();

                        if (answerDb == null)
                            answerDb = (await _answerRepository
                                .Create(new()
                                {
                                    IdQuestion = questionDb.Id,
                                    Position = answer.Position,
                                    Contents = answer.Contents,
                                    ContentsEs = answer.ContentsEs,
                                    Value = answer.Value,
                                    Type = answer.Type,
                                    Active = answer.Active,
                                })).Entity;
                        else
                        {
                            answerDb.IdQuestion = questionDb.Id;
                            answerDb.Position = answer.Position;
                            answerDb.Contents = answer.Contents;
                            answerDb.ContentsEs = answer.ContentsEs;
                            answerDb.Value = answer.Value;
                            answerDb.Type = answer.Type;
                            answerDb.Active = answer.Active;

                            _answerRepository.Update(answerDb);
                        }

                        await _answerRepository.SaveChanges();
                    }

                    foreach (var avoidElement in _answerRepository.GetByCondition(x => !question.Answers.Select(y => y.Id).ToList().Contains(x.Id)).ToList())
                    {
                        avoidElement.Active = false;
                        _answerRepository.Update(avoidElement);
                        await _answerRepository.SaveChanges();
                    }
                }

                foreach (var avoidElement in _questionRepository.GetByCondition(x => !section.Questions.Select(y => y.Id).ToList().Contains(x.Id)).ToList())
                {
                    avoidElement.Active = false;
                    _questionRepository.Update(avoidElement);
                    await _questionRepository.SaveChanges();
                }
            }
            foreach (var avoidElement in _sectionRepository.GetByCondition(x => !dto.Sections.Select(y => y.Id).ToList().Contains(x.Id)).ToList())
            {
                avoidElement.Active = false;
                _sectionRepository.Update(avoidElement);
                await _sectionRepository.SaveChanges();
            }

            return (await GetById(surveyDb.Id)).Adapt<SurveyDto>();
        }

        public async Task<SurveyDto> GetById(Guid id)
        {
            var surveyDB = await _surveyRepository.GetByCondition(x => x.Id == id)
                                                   .Include(x => x.Sections)
                                                   .ThenInclude(x => x.Questions)
                                                   .ThenInclude(x => x.Answers)
                                                   .FirstOrDefaultAsync();

            return surveyDB?.Adapt<SurveyDto>() ?? null;
        }
        public async Task<IEnumerable<SurveyDto>> GetAll()
        {
            var surveysDB = await _surveyRepository.GetAll()
                                                   .Include(x => x.Sections)
                                                   .ThenInclude(x => x.Questions)
                                                   .ThenInclude(x => x.Answers)
                                                   .ToListAsync();

            return surveysDB?.Adapt<List<SurveyDto>>().OrderBy(x => x.Name).OrderBy(x => x.Version) ?? null;
        }

        public async Task<IEnumerable<SurveyDto>> GetByLineId(Guid lineId)
        {
            var surveysDB = await _surveyRepository.GetByCondition(x => x.LineId == lineId)
                                                  .Include(x => x.Sections)
                                                  .ThenInclude(x => x.Questions)
                                                  .ThenInclude(x => x.Answers)
                                                  .ToListAsync();

            return surveysDB?.Adapt<IEnumerable<SurveyDto>>().OrderByDescending(x => x.Name) ?? null;
        }

        public async Task<IEnumerable<SurveyDto>> GetVersions(Guid id)
        {
            var surveyDB = await _surveyRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
            if (surveyDB == null)
                return null;

            var surveysDB = await _surveyRepository.GetByCondition(x => x.FingerPrint == surveyDB.FingerPrint)
                                                 .Include(x => x.Sections)
                                                 .ThenInclude(x => x.Questions)
                                                 .ThenInclude(x => x.Answers)
                                                 .ToListAsync();

            return surveysDB?.Adapt<IEnumerable<SurveyDto>>().OrderByDescending(x => x.Name) ?? null;
        }

        public async Task DeleteSurvey(Guid id)
        {
            var surveyDB = await _surveyRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
            if (surveyDB == null)
                throw new Exception("No existe el survey");

            surveyDB.Active = false;
            _surveyRepository.Update(surveyDB);
            await _surveyRepository.SaveChanges();
        }
    }
}
