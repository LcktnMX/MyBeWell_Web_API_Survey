using System;
using System.Collections.Generic;


namespace Lockton.Surveys.Domain.Model
{
    public class SurveyDto
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
        public List<SectionDto> Sections { get; set; } = new List<SectionDto>();

    }
    public class SectionDto
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string TitleEs { get; set; }
        public string Description { get; set; }
        public string DescriptionEs { get; set; }
        public double Position { get; set; }
        public bool? Active { get; set; }
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }
    public class QuestionDto
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
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();

    }

    public class AnswerDto
    {
        public Guid? Id { get; set; }
        public double Position { get; set; }
        public string Contents { get; set; }
        public string ContentsEs { get; set; }
        public double Value { get; set; }
        public string Type { get; set; }
        public bool? Active { get; set; }
    }
}
