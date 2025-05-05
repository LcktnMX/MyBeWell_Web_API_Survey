using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    [Table("SurveyLogMail")]
    public class SurveyLogMailEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SurveyApplicationId { get; set; }
        public string SentUser { get; set; }
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Success { get; set; }
        public string SentDate { get; set; }
        public string Error { get; set; }
    }
}
