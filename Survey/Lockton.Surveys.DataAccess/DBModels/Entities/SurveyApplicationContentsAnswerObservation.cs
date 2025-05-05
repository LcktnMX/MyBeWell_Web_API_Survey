using System;
using System.Collections.Generic;

#nullable disable

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    public partial class SurveyApplicationContentsAnswerObservation
    {
        public Guid Id { get; set; }
        public Guid? IdSurveyApplicationContents { get; set; }
        public Guid? IdQuestion { get; set; }
        public string Observation { get; set; }
        public DateTime? ReportedAt { get; set; }
        public bool? Closed { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string Response { get; set; }
        public virtual Question IdQuestionNavigation { get; set; }
        public virtual SurveyApplicationContent IdSurveyApplicationContentsNavigation { get; set; }
        public bool? Sent { get; set; }
    }
}
