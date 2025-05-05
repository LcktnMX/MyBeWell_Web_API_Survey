using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Domain.Model
{
    public class LogAnswerDto
    {
        public Guid Id { get; set; }
        public Guid SurveyApplicationContentsId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public string Contents { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDatetime { get; set; }
    }
}
