using System;
using System.Collections.Generic;

#nullable disable

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    public partial class SurveyApplication
    {
        public SurveyApplication()
        {
            Container = new HashSet<SurveyApplicationContent>();
        }

        public Guid Id { get; set; }
        public Guid? IdParticipant { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Sent { get; set; }
        public double? Progress { get; set; }
        public string SentMessage { get; set; }
        public string FinishMessage { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? Active { get; set; }
        public string Title { get; set; }
        public string Responsible { get; set; }
        public string Instructions { get; set; }
        public bool? Finished { get; set; }
        public virtual Participant IdParticipantNavigation { get; set; }
        public virtual ICollection<SurveyApplicationContent> Container { get; set; }
    }
}
