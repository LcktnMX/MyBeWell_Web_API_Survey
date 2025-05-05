using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Domain.Model
{
    public class ApplicantContentDto
    {
        public ParticipantDto Participant { get; set; }
        public List<SurveyApplicantDto> Surveys { get; set; }
    }
    public class SurveyApplicantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameEs { get; set; }
        public string Description { get; set; }
        public string DescriptionEs { get; set; }
        public string Version { get; set; }
        public List<SectionAplicantDto> Sections { get; set; } = new List<SectionAplicantDto>();

    }
    public class SectionAplicantDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string TitleEs { get; set; }
        public string Description { get; set; }
        public string DescriptionEs { get; set; }
        public string Position { get; set; }
        public List<QuestionApplicantDto> Questions { get; set; } = new List<QuestionApplicantDto>();
    }
    public class QuestionApplicantDto
    {
        public Guid Id { get; set; }
        public Guid IdType { get; set; }
        public string Contents { get; set; }
        public string ContentsEs { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string Position { get; set; }
        public List<AnswerApplicantDto> Answers { get; set; } = new List<AnswerApplicantDto>();
        public List<AnsweredObservationDto> Observation { get;set; } = new List<AnsweredObservationDto>();
    }

    public class AnswerApplicantDto
    {
        public Guid Id { get; set; }
        public string Position { get; set; }
        public string Contents { get; set; }
        public string ContentsEs { get; set; }
        public string Value { get; set; }
        public List<AnsweredDto> Answered { get; set; } = new List<AnsweredDto>();
    }
}
