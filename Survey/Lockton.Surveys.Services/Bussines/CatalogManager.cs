using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lockton.Surveys.DataAccess.DBModels.Entities;
using Lockton.Surveys.DataAccess.Repositories;
using Lockton.Surveys.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Lockton.Surveys.Services.Bussines
{
    public interface ICatalogManager
    {
        Task<IEnumerable<QuestionTypesDto>> GetQuestionTypes();

        Task<IEnumerable<LineDto>> GetLines();
    }
    public class CatalogManager:ICatalogManager
    {
        private readonly IRepository<Line> _lineRepository;
        private readonly IRepository<QuestionType> _questionTypeRepository;
        public CatalogManager(IRepository<Line> lineRepository, IRepository<QuestionType> questionTypeRepository) 
        { 
            _lineRepository = lineRepository;
            _questionTypeRepository = questionTypeRepository;
        }

        public async Task<IEnumerable<QuestionTypesDto>> GetQuestionTypes()
        {
            return (await _questionTypeRepository.GetAll().ToListAsync()).Adapt<IEnumerable<QuestionTypesDto>>().OrderBy(x => x.Description);
        }

        public async Task<IEnumerable<LineDto>> GetLines()
        {
            return (await _lineRepository.GetAll().ToListAsync()).Adapt<IEnumerable<LineDto>>().OrderBy(x => x.Description);
        }
    }
}
