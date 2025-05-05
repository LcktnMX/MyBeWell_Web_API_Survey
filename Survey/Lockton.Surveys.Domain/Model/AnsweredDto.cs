using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Domain.Model
{
    public class AnsweredDto
    {
        public Guid Id { get; set; }
        public Guid IdSurveyApplicationContents { get; set; }
        public Guid IdQuestion { get; set; }
        public Guid? IdAnswer { get; set; }
        public Guid QuestionType { get; set; }
        public bool? Value { get; set; }
        public string Text { get; set; }
        public int? Row { get; set; }
        public bool? Active { get; set; }
        public float Position { get; set; }
    }
}
