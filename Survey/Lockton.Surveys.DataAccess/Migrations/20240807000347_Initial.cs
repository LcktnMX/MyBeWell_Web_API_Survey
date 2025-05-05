using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lockton.Surveys.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Description = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Aviso = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Terminos = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Description = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Regex = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Survey",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FingerPrint = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Name_Es = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Description_Es = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedUser = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    LineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Survey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Survey_Lines",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurveyApplication",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IdParticipant = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Sent = table.Column<bool>(type: "bit", nullable: true),
                    Progress = table.Column<double>(type: "float", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyApplication_Participant",
                        column: x => x.IdParticipant,
                        principalTable: "Participant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IdSurvey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Title_Es = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Description_Es = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Position = table.Column<double>(type: "float", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Section_Survey",
                        column: x => x.IdSurvey,
                        principalTable: "Survey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurveyApplicationContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IdSurveyApplication = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdSurvey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyApplicationContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyApplicationContents_Survey",
                        column: x => x.IdSurvey,
                        principalTable: "Survey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurveyApplicationContents_SurveyApplication",
                        column: x => x.IdSurveyApplication,
                        principalTable: "SurveyApplication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IdSurvey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdSection = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdType = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Contents = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Contents_Es = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Header = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true),
                    Footer = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true),
                    Position = table.Column<double>(type: "float", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_QuestionType",
                        column: x => x.IdType,
                        principalTable: "QuestionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Question_Section1",
                        column: x => x.IdSection,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IdQuestion = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Position = table.Column<double>(type: "float", nullable: true),
                    Contents = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Contents_Es = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Value = table.Column<double>(type: "float", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Question",
                        column: x => x.IdQuestion,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurveyApplicationContentsAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IdSurveyApplicationContents = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdQuestion = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdAnswer = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuestionType = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Value = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Position = table.Column<double>(type: "float", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyApplicationContentsAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyApplicationContentsAnswer_Answer",
                        column: x => x.IdAnswer,
                        principalTable: "Answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurveyApplicationContentsAnswer_Question",
                        column: x => x.IdQuestion,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurveyApplicationContentsAnswer_SurveyApplicationContents",
                        column: x => x.IdSurveyApplicationContents,
                        principalTable: "SurveyApplicationContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurveyApplicationContentsAnswerObservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IdSurveyApplicationContentsAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Observation = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    ReportedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Closed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyApplicationContentsAnswerObservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyApplicationContentsAnswerObservation_SurveyApplicationContentsAnswer",
                        column: x => x.IdSurveyApplicationContentsAnswerId,
                        principalTable: "SurveyApplicationContentsAnswer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_IdQuestion",
                table: "Answer",
                column: "IdQuestion");

            migrationBuilder.CreateIndex(
                name: "IX_Question_IdSection",
                table: "Question",
                column: "IdSection");

            migrationBuilder.CreateIndex(
                name: "IX_Question_IdType",
                table: "Question",
                column: "IdType");

            migrationBuilder.CreateIndex(
                name: "IX_Section_IdSurvey",
                table: "Section",
                column: "IdSurvey");

            migrationBuilder.CreateIndex(
                name: "IX_Survey_LineId",
                table: "Survey",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyApplication_IdParticipant",
                table: "SurveyApplication",
                column: "IdParticipant");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyApplicationContents_IdSurvey",
                table: "SurveyApplicationContents",
                column: "IdSurvey");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyApplicationContents_IdSurveyApplication",
                table: "SurveyApplicationContents",
                column: "IdSurveyApplication");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyApplicationContentsAnswer_IdAnswer",
                table: "SurveyApplicationContentsAnswer",
                column: "IdAnswer");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyApplicationContentsAnswer_IdQuestion",
                table: "SurveyApplicationContentsAnswer",
                column: "IdQuestion");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyApplicationContentsAnswer_IdSurveyApplicationContents",
                table: "SurveyApplicationContentsAnswer",
                column: "IdSurveyApplicationContents");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyApplicationContentsAnswerObservation_IdSurveyApplicationContentsAnswerId",
                table: "SurveyApplicationContentsAnswerObservation",
                column: "IdSurveyApplicationContentsAnswerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyApplicationContentsAnswerObservation");

            migrationBuilder.DropTable(
                name: "SurveyApplicationContentsAnswer");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "SurveyApplicationContents");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "SurveyApplication");

            migrationBuilder.DropTable(
                name: "QuestionType");

            migrationBuilder.DropTable(
                name: "Section");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "Survey");

            migrationBuilder.DropTable(
                name: "Lines");
        }
    }
}
