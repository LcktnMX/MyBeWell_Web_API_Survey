using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Domain.Model
{
    public class ApplicationDto
    {
        public Guid? Id { get; set; }
        public Guid IdParticipant { get; set; }
        public string Instructions { get; set; }
        public string Title { get; set; }
        public string Responsible { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Sent { get; set; }
        public float Progress { get; set; }
        public string SentMessage { get; set; }
        public string FinishMessage { get; set; }
        public bool Active { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public bool? Finished { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<ApplicationContentsDto> Container { get; set; } = new List<ApplicationContentsDto>();
    }
    public class ApplicationContentsDto
    {
        public Guid Id { get; set; }
        public Guid IdSurvey { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public bool Active { get; set; }
    }
}
