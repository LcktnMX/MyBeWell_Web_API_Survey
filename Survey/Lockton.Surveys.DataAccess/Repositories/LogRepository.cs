using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Lockton.Surveys.DataAccess.DataContext;
using Lockton.Surveys.DataAccess.DBModels.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace Lockton.Surveys.DataAccess.Repositories
{
    public interface ILogRepository
    {
        Task<LogAnswerEntity> LogAnswer(LogAnswerEntity entity);
    }
    public class LogRepository : ILogRepository
    {
        private readonly DapperContext _db;
        public LogRepository(DapperContext db)
        {
            _db = db;
        }

        public async Task<LogAnswerEntity> LogAnswer(LogAnswerEntity entity)
        {
            var sql = @"
                 INSERT INTO [dbo].[SurveyApplicationContentsAnswerLog]
                            (SurveyApplicationContentsId
                            ,QuestionId
                            ,AnswerId
                            ,Contents
                            ,CreatedUser
                            ,CreatedDatetime)
                        OUTPUT
                            INSERTED.*
                        VALUES
                            (@SurveyApplicationContentsId
                            ,@QuestionId
                            ,@AnswerId
                            ,@Contents
                            ,@CreatedUser
                            ,@CreatedDatetime)";


            var values = new
            {
                SurveyApplicationContentsId = entity.SurveyApplicationContentsId,
                QuestionId = entity.QuestionId,
                AnswerId = entity.AnswerId,
                Contents = entity.Contents,
                CreatedUser = entity.CreatedUser,
                CreatedDatetime = entity.CreatedDatetime,
            };


            using var connection = _db.CreateConnection();
            var response = await connection.QuerySingleAsync<LogAnswerEntity>(sql, values);

            return response;



        }
    }
}
