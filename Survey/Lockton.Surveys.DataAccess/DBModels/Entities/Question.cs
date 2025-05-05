using System;
using System.Collections.Generic;

#nullable disable

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            SurveyApplicationContentsAnswerObservations = new HashSet<SurveyApplicationContentsAnswerObservation>();
            SurveyApplicationContentsAnswers = new HashSet<SurveyApplicationContentsAnswer>();
        }

        public Guid Id { get; set; }
        public Guid? IdSurvey { get; set; }
        public Guid? IdSection { get; set; }
        public Guid? IdType { get; set; }
        public string Contents { get; set; }
        public string ContentsEs { get; set; }
        public string Header { get; set; }
        public string HeaderEs { get; set; }
        public string Footer { get; set; }
        public string FooterEs { get; set; }
        public double? Position { get; set; }
        public int? Align { get; set; }
        public bool? ExtraTextQuestion { get; set; }
        public bool? Required { get; set; }
        public bool? Active { get; set; }

        public virtual Section IdSectionNavigation { get; set; }
        public virtual QuestionType IdTypeNavigation { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<SurveyApplicationContentsAnswerObservation> SurveyApplicationContentsAnswerObservations { get; set; }
        public virtual ICollection<SurveyApplicationContentsAnswer> SurveyApplicationContentsAnswers { get; set; }
    }
}
