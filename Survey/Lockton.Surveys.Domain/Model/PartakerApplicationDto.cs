using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Domain.Model
{
    public class PartakerApplicationDto
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
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? Finished { get; set; }
        public ParticipantDto Participant { get; set; }
        public List<ConfigContainer> Container { get; set; } = new List<ConfigContainer>();
    }

    public class ConfigContainer
    {
        public Guid Id { get; set; }
        public Guid IdSurvey { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public bool Active { get; set; }
        public ConfigSurvey Survey { get; set; }

    }

    public class ConfigSurvey
    {
        public Guid? Id { get; set; }
        public Guid? FingerPrint { get; set; }
        public string Name { get; set; }
        public string NameEs { get; set; }
        public string Description { get; set; }
        public string DescriptionEs { get; set; }
        public int Version { get; set; }
        public string User { get; set; }
        public Guid LineId { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.HasValue ? this.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm") : "";
            }
        }
        public string CreatedUser { get; set; }
        public List<ConfigSection> Sections { get; set; } = new List<ConfigSection>();
    }

    public class ConfigSection
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string TitleEs { get; set; }
        public string Description { get; set; }
        public string DescriptionEs { get; set; }
        public double Position { get; set; }
        public bool? Active { get; set; }
        public List<ConfigQuestion> Questions { get; set; } = new List<ConfigQuestion>();
    }

    public class ConfigQuestion
    {
        public Guid? Id { get; set; }
        public Guid? IdType { get; set; }
        public string Contents { get; set; }
        public string ContentsEs { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string HeaderEs { get; set; }
        public string FooterEs { get; set; }
        public double Position { get; set; }
        public bool? Required { get; set; }
        public int? Align { get; set; }
        public bool? ExtraTextQuestion { get; set; }
        public bool? Active { get; set; }
        public List<ConfigAnswer> Answers { get; set; } = new List<ConfigAnswer>();
        public List<Answered> Answered { get; set; } = new List<Answered>();
        public List<Message> Message { get; set; } = new List<Message>();
    }

    public class ConfigAnswer
    {
        public Guid? Id { get; set; }
        public double Position { get; set; }
        public string Contents { get; set; }
        public string ContentsEs { get; set; }
        public double Value { get; set; }
        public string Type { get; set; }
        public bool? Active { get; set; }
        public List<Answered> Answered { get; set; } = new List<Answered>();
    }

    public class Answered
    {
        public Guid Id { get; set; }
        public Guid IdSurveyApplicationContents { get; set; }
        public Guid IdQuestion { get; set; }
        public Guid? IdAnswer { get; set; }
        public bool? Value { get; set; }
        public string Text { get; set; }
        public bool? Active { get; set; }
        public int? Position { get; set; }
        public int? Row { get; set; }
    }

    public class Message
    {

        public Guid Id { get; set; }
        public Guid IdSurveyApplicationContents { get; set; }
        public Guid? IdQuestion { get; set; }
        public string Observation { get; set; }
        public DateTime? ReportedAt { get; set; }
        public bool Closed { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string Response { get; set; }
        public bool Sent { get; set; }
        public bool Resend { get; set; }
        public string ReportedAtStr
        {
            get
            {
                return ReportedAt.HasValue ? this.ReportedAt.Value.ToString("dd/MM/yyyy hh:mm") : "";
            }
        }
        public string ClosedAtStr
        {
            get
            {
                return ClosedAt.HasValue ? this.ReportedAt.Value.ToString("dd/MM/yyyy hh:mm") : "";
            }
        }
    }
}
