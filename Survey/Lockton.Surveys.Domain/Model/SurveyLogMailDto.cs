using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Domain.Model
{
    public class SurveyLogMailDto
    {
        public Guid Id { get; set; }
        public Guid SurveyApplicationId { get; set; }
        public string SentUser { get; set; }
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentDate { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}
