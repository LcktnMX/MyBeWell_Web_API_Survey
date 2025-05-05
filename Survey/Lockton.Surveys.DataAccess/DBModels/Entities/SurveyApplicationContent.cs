using System;
using System.Collections.Generic;

#nullable disable

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    public partial class SurveyApplicationContent
    {
        public SurveyApplicationContent()
        {
            SurveyApplicationContentsAnswerObservations = new HashSet<SurveyApplicationContentsAnswerObservation>();
            SurveyApplicationContentsAnswers = new HashSet<SurveyApplicationContentsAnswer>();
        }

        public Guid Id { get; set; }
        public Guid? IdSurveyApplication { get; set; }
        public Guid? IdSurvey { get; set; }
        public string Description { get; set; }
        public int? Position { get; set; }
        public bool? Active { get; set; }

        public virtual SurveyApplication IdSurveyApplicationNavigation { get; set; }
        public virtual Survey IdSurveyNavigation { get; set; }
        public virtual ICollection<SurveyApplicationContentsAnswerObservation> SurveyApplicationContentsAnswerObservations { get; set; }
        public virtual ICollection<SurveyApplicationContentsAnswer> SurveyApplicationContentsAnswers { get; set; }
    }
}
