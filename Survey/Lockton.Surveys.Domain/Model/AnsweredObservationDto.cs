using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Domain.Model
{
    public class AnsweredObservationDto
    {
        public Guid Id { get; set; }
        public Guid IdSurveyApplicationContents { get; set; }
        public Guid? IdQuestion { get; set; }
        public string Observation { get; set; }        
        public DateTime ReportedAt { get; set; }
        public bool? Closed { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string Response { get; set; }
    }
}
