using System;
using System.Collections.Generic;

#nullable disable

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    public partial class SurveyApplicationContentsAnswer
    {
        public Guid Id { get; set; }
        public Guid? IdSurveyApplicationContents { get; set; }
        public Guid? IdQuestion { get; set; }
        public Guid? IdAnswer { get; set; }
        public Guid? QuestionType { get; set; }
        public bool? Value { get; set; }
        public string Text { get; set; }
        public double? Position { get; set; }
        public int? Row { get; set; }
        public bool? Active { get; set; }

        public virtual Question IdQuestionNavigation { get; set; }
        public virtual SurveyApplicationContent IdSurveyApplicationContentsNavigation { get; set; }
    }
}
