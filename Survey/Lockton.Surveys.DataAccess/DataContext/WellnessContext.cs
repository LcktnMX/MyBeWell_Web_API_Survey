using System;
using Lockton.Surveys.DataAccess.DBModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Lockton.Surveys.DataAccess.DataContext
{
    public partial class WellnessContext : DbContext
    {
        public WellnessContext()
        {
        }

        public WellnessContext(DbContextOptions<WellnessContext> options)
            : base(options)
        {
        }
        public virtual DbSet<SurveyLogMailEntity> SurveyLogMailEntity { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Line> Lines { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<SurveyApplication> SurveyApplications { get; set; }
        public virtual DbSet<SurveyApplicationContent> SurveyApplicationContents { get; set; }
        public virtual DbSet<SurveyApplicationContentsAnswer> SurveyApplicationContentsAnswers { get; set; }
        public virtual DbSet<SurveyApplicationContentsAnswerObservation> SurveyApplicationContentsAnswerObservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.HasIndex(e => e.IdQuestion, "IX_Answer_IdQuestion");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Contents)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ContentsEs)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("Contents_Es");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdQuestionNavigation)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.IdQuestion)
                    .HasConstraintName("FK_Answer_Question");
            });

            modelBuilder.Entity<Line>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.ToTable("Participant");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Aviso).IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.RazonSocial)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Terminos).IsUnicode(false);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.HasIndex(e => e.IdSection, "IX_Question_IdSection");

                entity.HasIndex(e => e.IdType, "IX_Question_IdType");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Contents)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ContentsEs)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("Contents_Es");

                entity.Property(e => e.Footer)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.FooterEs)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Header)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.HeaderEs)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdSectionNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.IdSection)
                    .HasConstraintName("FK_Question_Section1");

                entity.HasOne(d => d.IdTypeNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.IdType)
                    .HasConstraintName("FK_Question_QuestionType");
            });

            modelBuilder.Entity<QuestionType>(entity =>
            {
                entity.ToTable("QuestionType");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Regex)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.ToTable("Section");

                entity.HasIndex(e => e.IdSurvey, "IX_Section_IdSurvey");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description)                    
                    .IsUnicode(false);

                entity.Property(e => e.DescriptionEs)                    
                    .IsUnicode(false)
                    .HasColumnName("Description_Es");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TitleEs)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Title_Es");

                entity.HasOne(d => d.IdSurveyNavigation)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.IdSurvey)
                    .HasConstraintName("FK_Section_Survey");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.ToTable("Survey");

                entity.HasIndex(e => e.LineId, "IX_Survey_LineId");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DescriptionEs)
                    .IsUnicode(false)
                    .HasColumnName("Description_Es");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NameEs)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Name_Es");

                entity.HasOne(d => d.Line)
                    .WithMany(p => p.Surveys)
                    .HasForeignKey(d => d.LineId)
                    .HasConstraintName("FK_Survey_Lines");
            });

            modelBuilder.Entity<SurveyApplication>(entity =>
            {
                entity.ToTable("SurveyApplication");

                entity.HasIndex(e => e.IdParticipant, "IX_SurveyApplication_IdParticipant");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FinishMessage).IsUnicode(false);

                entity.Property(e => e.Instructions)                    
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SentMessage).IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdParticipantNavigation)
                    .WithMany(p => p.SurveyApplications)
                    .HasForeignKey(d => d.IdParticipant)
                    .HasConstraintName("FK_SurveyApplication_Participant");
            });

            modelBuilder.Entity<SurveyApplicationContent>(entity =>
            {
                entity.HasIndex(e => e.IdSurvey, "IX_SurveyApplicationContents_IdSurvey");

                entity.HasIndex(e => e.IdSurveyApplication, "IX_SurveyApplicationContents_IdSurveyApplication");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.HasOne(d => d.IdSurveyNavigation)
                    .WithMany(p => p.SurveyApplicationContents)
                    .HasForeignKey(d => d.IdSurvey)
                    .HasConstraintName("FK_SurveyApplicationContents_Survey");

                entity.HasOne(d => d.IdSurveyApplicationNavigation)
                    .WithMany(p => p.Container)
                    .HasForeignKey(d => d.IdSurveyApplication)
                    .HasConstraintName("FK_SurveyApplicationContents_SurveyApplication");
            });

            modelBuilder.Entity<SurveyApplicationContentsAnswer>(entity =>
            {
                entity.ToTable("SurveyApplicationContentsAnswer");

                entity.HasIndex(e => e.IdAnswer, "IX_SurveyApplicationContentsAnswer_IdAnswer");

                entity.HasIndex(e => e.IdQuestion, "IX_SurveyApplicationContentsAnswer_IdQuestion");

                entity.HasIndex(e => e.IdSurveyApplicationContents, "IX_SurveyApplicationContentsAnswer_IdSurveyApplicationContents");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Text).IsUnicode(false);

                entity.HasOne(d => d.IdQuestionNavigation)
                    .WithMany(p => p.SurveyApplicationContentsAnswers)
                    .HasForeignKey(d => d.IdQuestion)
                    .HasConstraintName("FK_SurveyApplicationContentsAnswer_Question");

                entity.HasOne(d => d.IdSurveyApplicationContentsNavigation)
                    .WithMany(p => p.SurveyApplicationContentsAnswers)
                    .HasForeignKey(d => d.IdSurveyApplicationContents)
                    .HasConstraintName("FK_SurveyApplicationContentsAnswer_SurveyApplicationContents");
            });

            modelBuilder.Entity<SurveyApplicationContentsAnswerObservation>(entity =>
            {
                entity.ToTable("SurveyApplicationContentsAnswerObservation");

                entity.HasIndex(e => e.IdSurveyApplicationContents, "IX_SurveyApplicationContentsAnswerObservation_IdSurveyApplicationContentsAnswerId");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Observation)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ReportedAt).HasColumnType("datetime");

                entity.HasOne(d => d.IdQuestionNavigation)
                    .WithMany(p => p.SurveyApplicationContentsAnswerObservations)
                    .HasForeignKey(d => d.IdQuestion)
                    .HasConstraintName("FK_SurveyApplicationContentsAnswerObservation_Question");

                entity.HasOne(d => d.IdSurveyApplicationContentsNavigation)
                    .WithMany(p => p.SurveyApplicationContentsAnswerObservations)
                    .HasForeignKey(d => d.IdSurveyApplicationContents)
                    .HasConstraintName("FK_SurveyApplicationContentsAnswerObservation_SurveyApplicationContents");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("Data Source=172.17.0.28; initial catalog=Wellness; user id=UserBeflex;password=Pac#17.29eRy");
            }
        }
    }
}
