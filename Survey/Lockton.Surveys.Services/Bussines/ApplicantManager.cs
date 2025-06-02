using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Lockton.Surveys.DataAccess.DBModels.Entities;
using Lockton.Surveys.DataAccess.Repositories;
using Lockton.Surveys.Domain.Model;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;
using HtmlToOpenXml;
using System.IO.Compression;
using Microsoft.IdentityModel.Tokens;
using DocumentFormat.OpenXml.Office.SpreadSheetML.Y2023.MsForms;
using System.Reflection;
using Org.BouncyCastle.Asn1.X509;




namespace Lockton.Surveys.Services.Bussines
{
    public interface IApplicantManager
    {
        Task<PartakerApplicationDto> GetPartakerApplication(Guid surveyApplicationId);
        Task SaveAnswer(AnsweredDto dto);
        Task FinishApplication(Guid surveyApplicationId);
        Task UpdateProgress(Guid surveyApplicationId, float progress);
        Task<AnsweredObservationDto> UpsertMessage(AnsweredObservationDto dto);
        Task<LogAnswerDto> LogAnswer(LogAnswerDto dto);
        //Task<byte[]> DownloadPartakerApplication(Guid AppId);
        Task<IEnumerable<AnsweredObservationDto>> GetObservations(Guid idSurveyApplicationContents, bool Open);

    }
    public class ApplicantManager : IApplicantManager
    {
        private readonly ILogRepository _logRepository;
        private readonly IRepository<Survey> _surveyRepository;
        private readonly IRepository<Participant> _participantRepository;
        private readonly IRepository<SurveyApplication> _surveyApplicationRepository;
        private readonly IRepository<SurveyApplicationContent> _surveyApplicationContentRepository;
        private readonly IRepository<SurveyApplicationContentsAnswer> _answeredRepository;
        private readonly IRepository<SurveyApplicationContentsAnswerObservation> _observationRepository;
        public ApplicantManager(
            IRepository<Survey> surveyRepository,
            IRepository<SurveyApplication> surveyApplicationRepository,
            IRepository<SurveyApplicationContent> surveyApplicationContentRepository,
            IRepository<SurveyApplicationContentsAnswer> surveyApplicationContentsAnswerRepository,
            IRepository<SurveyApplicationContentsAnswerObservation> surveyApplicationContentsAnswerObservationRepository,
            IRepository<Participant> participantRepository,
            ILogRepository logRepository)
        {
            _surveyRepository = surveyRepository;
            _surveyApplicationRepository = surveyApplicationRepository;
            _surveyApplicationContentRepository = surveyApplicationContentRepository;
            _answeredRepository = surveyApplicationContentsAnswerRepository;
            _observationRepository = surveyApplicationContentsAnswerObservationRepository;
            _participantRepository = participantRepository;
            _logRepository = logRepository;
        }
        public async Task FinishApplication(Guid surveyApplicationId)
        {
            var surveyApplicationDB = (await _surveyApplicationRepository.GetByCondition(x => x.Id == surveyApplicationId).FirstOrDefaultAsync());
            if (surveyApplicationDB != null)
            {
                surveyApplicationDB.Finished = true;
                surveyApplicationDB.Progress = 100;
                surveyApplicationDB.ModifiedDate = DateTime.Now;
                _surveyApplicationRepository.Update(surveyApplicationDB);
                await _surveyApplicationRepository.SaveChanges();
            }
        }

        public async Task UpdateProgress(Guid surveyApplicationId, float progress)
        {
            var surveyApplicationDB = (await _surveyApplicationRepository.GetByCondition(x => x.Id == surveyApplicationId).FirstOrDefaultAsync());
            if (surveyApplicationDB != null)
            {
                surveyApplicationDB.Progress = progress;
                _surveyApplicationRepository.Update(surveyApplicationDB);
                await _surveyApplicationRepository.SaveChanges();
            }
        }

        public async Task<AnsweredObservationDto> UpsertMessage(AnsweredObservationDto dto)
        {
            SurveyApplicationContentsAnswerObservation observationDb = null;
            if (dto.Id != Guid.Empty)
                observationDb = await _observationRepository.GetByCondition(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (observationDb == null)
            {
                observationDb = (await _observationRepository.Create(new()
                {
                    IdQuestion = dto.IdQuestion,
                    Closed = dto.Closed ?? false,
                    Observation = dto.Observation,
                    IdSurveyApplicationContents = dto.IdSurveyApplicationContents,
                    ReportedAt = dto.ReportedAt,
                    ClosedAt = dto.ClosedAt,
                    Response = dto.Response,
                    Sent = dto.Sent ?? false,
                    Resend = dto.Resend ?? false

                })).Entity;
            }
            else
            {
                observationDb.IdQuestion = dto.IdQuestion;
                observationDb.Closed = dto.Closed;
                observationDb.Observation = dto.Observation;
                observationDb.IdSurveyApplicationContents = dto.IdSurveyApplicationContents;
                observationDb.ReportedAt = dto.ReportedAt;
                observationDb.ClosedAt = dto.ClosedAt;
                observationDb.Response = dto.Response;
                observationDb.Sent = dto.Sent ?? false;
                observationDb.Resend = dto.Resend ?? false;
                _observationRepository.Update(observationDb);
            }
            await _observationRepository.SaveChanges();

            return observationDb.Adapt<AnsweredObservationDto>();
        }

        public async Task<IEnumerable<AnsweredObservationDto>> GetObservations(Guid idSurveyApplicationContents, bool Open)
        {
            var observationsDb =
                    await _observationRepository
                            .GetByCondition(x => x.IdSurveyApplicationContents == idSurveyApplicationContents
                                && x.Closed == !Open)
                            .ToListAsync();

            return observationsDb.Adapt<List<AnsweredObservationDto>>();
        }

        public async Task SaveAnswer(AnsweredDto dto)
        {
            SurveyApplicationContentsAnswer answered = null;

            if (dto.Id != Guid.Empty)
                answered = await _answeredRepository.GetByCondition(x => x.Id == dto.Id).FirstOrDefaultAsync();
            else
                answered = await _answeredRepository
                    .GetByCondition(x => x.IdSurveyApplicationContents == dto.IdSurveyApplicationContents && x.IdQuestion == dto.IdQuestion && x.IdAnswer == dto.IdAnswer && x.Position == dto.Position && x.Row == dto.Row)
                    .FirstOrDefaultAsync();

            if (answered == null)
            {
                answered = (await _answeredRepository.Create(new()
                {
                    IdQuestion = dto.IdQuestion,
                    IdAnswer = dto.IdAnswer,
                    IdSurveyApplicationContents = dto.IdSurveyApplicationContents,
                    Value = dto.Value,
                    Text = dto.Text,
                    QuestionType = dto.QuestionType,
                    Position = dto.Position,
                    Row = dto.Row,
                    Active = dto.Active
                })).Entity;
            }
            else
            {
                answered.IdQuestion = dto.IdQuestion;
                answered.IdAnswer = dto.IdAnswer;
                answered.IdSurveyApplicationContents = dto.IdSurveyApplicationContents;
                answered.Value = dto.Value;
                answered.Text = dto.Text;
                answered.QuestionType = dto.QuestionType;
                answered.Position = dto.Position;
                answered.Active = true;
                _answeredRepository.Update(answered);
            }
            await _answeredRepository.SaveChanges();
        }

        public async Task<PartakerApplicationDto> GetPartakerApplication(Guid surveyApplicationId)
        {
            var model = new PartakerApplicationDto();
            var surveyApplicationDB = (await _surveyApplicationRepository.GetByCondition(x => x.Id == surveyApplicationId)
                                                    .Include(x => x.Container)
                                                    .FirstOrDefaultAsync());

            if (surveyApplicationDB == null)
                throw new Exception("Application does not exist");

            model = surveyApplicationDB?.Adapt<PartakerApplicationDto>();

            model.Participant = (await _participantRepository
                                            .GetByCondition(x => x.Id == model.IdParticipant)
                                            .FirstOrDefaultAsync())
                                            .Adapt<ParticipantDto>();

            model.Container = model.Container.Where(x => x.Active == true).ToList();



            foreach (var surveyConfig in model.Container)
            {
                var surveyDB = (await _surveyRepository.GetByCondition(x => x.Id == surveyConfig.IdSurvey)
                                                   .Include(x => x.Sections)
                                                   .ThenInclude(x => x.Questions)
                                                   .ThenInclude(x => x.Answers)
                                                   .FirstOrDefaultAsync());

                surveyConfig.Survey = surveyDB.Adapt<ConfigSurvey>();

                surveyConfig.Survey.Sections = surveyConfig.Survey.Sections.Where(x => x.Active == true).ToList();
                foreach (var section in surveyConfig.Survey.Sections)
                {
                    section.Questions = section.Questions.Where(x => x.Active == true).ToList();
                    foreach (var questionConfig in section.Questions)
                    {
                        questionConfig.Answers = questionConfig.Answers.Where(x => x.Active == true).ToList();

                        questionConfig.Answered = (await _answeredRepository
                                .GetByCondition(x => x.IdSurveyApplicationContents == surveyConfig.Id && x.IdQuestion == questionConfig.Id && x.Active == true && (x.Value == true || !string.IsNullOrEmpty(x.Text)))
                                .ToListAsync())
                                .Adapt<List<Answered>>();

                        questionConfig.Message = (await _observationRepository
                                .GetByCondition(x => x.IdSurveyApplicationContents == surveyConfig.Id && x.IdQuestion == questionConfig.Id)
                                .ToListAsync())
                                .Adapt<List<Message>>();


                        if (questionConfig.IdType.ToString().ToLower() == "0bd9a7f7-4c98-4b76-ba6e-d6f950159e3b")
                        {
                            questionConfig.Answered = (await _answeredRepository
                                    .GetByCondition(x => x.IdSurveyApplicationContents == surveyConfig.Id && x.IdQuestion == questionConfig.Id && x.Active == true)
                                    .ToListAsync())
                                    .Adapt<List<Answered>>();
                        }

                        foreach (var answer in questionConfig.Answers)
                        {
                            var answered = (await _answeredRepository
                                .GetByCondition(x => x.IdSurveyApplicationContents == surveyConfig.Id && x.IdAnswer == answer.Id && x.IdQuestion == questionConfig.Id && x.Active != false)
                                .ToListAsync());

                            answer.Answered = answered.Adapt<List<Answered>>();
                        }

                    }
                }
            }

            return model;
        }

        public async Task<LogAnswerDto> LogAnswer(LogAnswerDto dto)
        {
            return (await _logRepository.LogAnswer(dto.Adapt<LogAnswerEntity>())).Adapt<LogAnswerDto>();
        }

        //public async Task<byte[]> DownloadPartakerApplication(Guid AppId)
        //{
        //    var files = new List<InMemoryFile>();

        //    var application = await GetPartakerApplication(AppId);

        //    foreach (var container in application.Container.OrderBy(x => x.Position))
        //    {
        //        files.Add(await ProcessContainer(container, application.ModifiedDate?.ToString("dd/MM/yyyy") ?? string.Empty));
        //    }

        //    return GetZipArchive(files);
        //}

        //private async Task<InMemoryFile> ProcessContainer(ConfigContainer container, string date)
        //{
        //    var file = new InMemoryFile();
        //    var html = new StringBuilder("<html><head><style>html {font-family:Segoe UI;}</style><meta charset=\"UTF-8\">\r\n<meta http-equiv=\"Content-type\" content=\"text/html; charset=UTF-8\"></head><body>");

        //    foreach (var section in container.Survey.Sections.OrderBy(x => x.Position))
        //    {
        //        if (!string.IsNullOrEmpty(section.TitleEs))
        //            html.AppendLine($"<p style='font-size:26pt;color:#000000;margin:0px; font-weight: bold;'>{section.TitleEs}<p>");

        //        if (!string.IsNullOrEmpty(section.Title))
        //            html.AppendLine($"<p style='font-size:20pt;color:#00AEEF;margin:0px; font-weight: bold;'>{section.Title}<p>");

        //        html.AppendLine($"<br/>");

        //        if (!string.IsNullOrEmpty(section.DescriptionEs))
        //        {
        //            html.AppendLine($"<br/>");
        //            html.AppendLine($"{section.DescriptionEs.Replace("<!--block-->", string.Empty)}");

        //            if (!string.IsNullOrEmpty(section.Description))
        //                html.AppendLine($"{section.Description}");

        //            //html.AppendLine("<div style=\"border-top:1px Solid Black;\"/>");
        //            html.AppendLine($"<br/>");
        //            html.AppendLine($"<br/>");
        //        }



        //        foreach (var question in section.Questions.OrderBy(x => x.Position))
        //        {
        //            if (!string.IsNullOrEmpty(question.HeaderEs))
        //                html.AppendLine($"<p style='font-size:11px; color:#000000;'>{question.HeaderEs}</p>");

        //            if (!string.IsNullOrEmpty(question.Header))
        //                html.AppendLine($"<p style='font-size:10px;color:#00AEEF;'>{question.Header}</p>");

        //            var align = question.Align == 1 ? "left" : (question.Align == 2 ? "center" : "right");

        //            html.AppendLine("<div style='line-height: 1.0;'>");
        //            if (!string.IsNullOrEmpty(question.ContentsEs))
        //                html.AppendLine($"<p style='font-size:11pt; font-weight: bold; text-align:{align};color:#000000;margin:0px;'>{question.ContentsEs}</p>");

        //            if (!string.IsNullOrEmpty(question.Contents))
        //                html.AppendLine($"<p style='font-size:10pt; text-align:{align};color:#00AEEF;margin:0px'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{question.Contents}</p>");
        //            html.AppendLine("</div>");
        //            //html.AppendLine("<div style=\"border-top:1px Solid Black;\"/>");

        //            switch (question.IdType.ToString())
        //            {
        //                case "20485757-3aaf-4bd6-8b2d-370f87860667"://Documento
        //                    {
        //                        var filename = System.IO.Path.GetFileName(question.Answered.FirstOrDefault().Text);
        //                        html.AppendLine($"<p style='margin-left:20px;font-size:11pt; text-align:{align};color:#000000;background-color:#D3D3D3;'>{filename}</p>");
        //                        var f = question?.Answered?.FirstOrDefault()?.Text ?? string.Empty;
        //                        if (!string.IsNullOrEmpty(f))
        //                            file.FolderFiles.Add(new FolderFiles
        //                            {
        //                                FileName = filename,
        //                                Content = System.IO.File.ReadAllBytes(f)
        //                            });
        //                    }
        //                    break;
        //                case "2690e86f-0a7e-483c-a740-528cea55f10f"://Imagen
        //                    {
        //                        var filename = System.IO.Path.GetFileName(question.Answered.FirstOrDefault().Text);
        //                        html.AppendLine($"<p style='margin-left:20px;font-size:11pt; text-align:{align};color:#000000;background-color:#D3D3D3;'>{filename}</p>");
        //                        var f = question?.Answered?.FirstOrDefault()?.Text ?? string.Empty;
        //                        if (!string.IsNullOrEmpty(f))
        //                            file.FolderFiles.Add(new FolderFiles
        //                            {
        //                                FileName = filename,
        //                                Content = System.IO.File.ReadAllBytes(f)
        //                            });
        //                    }
        //                    break;
        //                case "367525f5-2acd-45f3-addd-107944a16e2d"://Incisos Opción
        //                    {
        //                        char x = 'a';
        //                        var i = 0;
        //                        foreach (var answer in question.Answers.OrderBy(x => x.Position))
        //                        {
        //                            var margin = i == 0 ? "margin:10px 0px 0px 20px;" : "margin:4px 0px 0px 20px;";
        //                            if (string.IsNullOrEmpty(answer.Type))
        //                            {
        //                                var style = answer.Answered?.FirstOrDefault()?.Value == true ? "background-color:#D3D3D3;" : string.Empty;
        //                                html.Append($"<p style='{margin}font-size:13px; text-align:{align};color:#000000;{style}'>");
        //                                html.AppendLine($"{x}.-{answer.ContentsEs}");
        //                                if (!string.IsNullOrEmpty(answer.Contents))
        //                                    html.AppendLine($" / {answer.Contents}");
        //                                html.Append($"</p>");
        //                            }
        //                            else
        //                            {
        //                                html.Append($"<p style='{margin};font-size:13px; text-align:{align};color:#000000;background-color:#D3D3D3;'>");
        //                                html.AppendLine($"-{answer.ContentsEs}");
        //                                if (!string.IsNullOrEmpty(answer.Contents))
        //                                    html.AppendLine($" / {answer.Contents}");
        //                                html.Append($": {answer?.Answered?.FirstOrDefault()?.Text ?? string.Empty}</p>");
        //                            }
        //                            x++;
        //                            i++;
        //                        }
        //                    }
        //                    break;
        //                case "56542709-9368-4a57-b7b5-30582d6305d4"://Incisos Opción Multiple
        //                    {
        //                        char x = 'a';
        //                        var i = 0;
        //                        foreach (var answer in question.Answers.OrderBy(x => x.Position))
        //                        {
        //                            var margin = i == 0 ? "margin:10px 0px 0px 20px;" : "margin:4px 0px 0px 20px;";
        //                            if (string.IsNullOrEmpty(answer.Type))
        //                            {
        //                                var style = answer.Answered?.FirstOrDefault()?.Value == true ? "background-color:#D3D3D3;" : string.Empty;
        //                                html.Append($"<p style='{margin}font-size:13px; text-align:{align};color:#000000;{style}'>");
        //                                html.AppendLine($"{x}.-{answer.ContentsEs}");
        //                                if (!string.IsNullOrEmpty(answer.Contents))
        //                                    html.AppendLine($" / {answer.Contents}");
        //                                html.Append($"</p>");

        //                            }
        //                            else
        //                            {
        //                                html.Append($"<p style='{margin}font-size:13px; text-align:{align};color:#000000;background-color:#D3D3D3;'>");
        //                                html.AppendLine($"-{answer.ContentsEs}");
        //                                if (!string.IsNullOrEmpty(answer.Contents))
        //                                    html.AppendLine($" / {answer.Contents}");
        //                                html.Append($": {answer?.Answered?.FirstOrDefault()?.Text ?? string.Empty}</p>");
        //                            }
        //                            x++;
        //                            i++;
        //                        }
        //                    }
        //                    break;
        //                case "04cc5ece-aa62-4453-ac27-c07b618bbf56"://Incisos Texto Libre
        //                    {
        //                        char x = 'a';
        //                        var i = 0;
        //                        foreach (var answer in question.Answers.OrderBy(x => x.Position))
        //                        {
        //                            var margin = i == 0 ? "margin:10px 0px 0px 20px;" : "margin:4px 0px 0px 20px;";
        //                            html.Append($"<p style='{margin}font-size:13px; text-align:{align};color:#000000;'>");
        //                            html.AppendLine($"{x}.-{answer.ContentsEs}");
        //                            if (!string.IsNullOrEmpty(answer.Contents))
        //                                html.AppendLine($" / {answer.Contents}");
        //                            html.Append($"<span style='background-color:#D3D3D3;'>{answer?.Answered?.FirstOrDefault()?.Text ?? string.Empty}</span>");
        //                            html.Append($"</p>");
        //                            x++;
        //                            i++;
        //                        }
        //                    }
        //                    break;
        //                case "510922fc-8130-4a78-9197-f6f241c70fcd"://Opción
        //                    {
        //                        var x = 1;
        //                        var i = 0;
        //                        foreach (var answer in question.Answers.OrderBy(x => x.Position))
        //                        {
        //                            var margin = i == 0 ? "margin:10px 0px 0px 20px;" : "margin:4px 0px 0px 20px;";
        //                            if (string.IsNullOrEmpty(answer.Type))
        //                            {
        //                                var style = answer.Answered?.FirstOrDefault()?.Value == true ? "background-color:#D3D3D3;" : string.Empty;
        //                                html.Append($"<p style='{margin}font-size:13px; text-align:{align};color:#000000;{style}'>");
        //                                html.AppendLine($"{x}.-{answer.ContentsEs}");
        //                                if (!string.IsNullOrEmpty(answer.Contents))
        //                                    html.AppendLine($" / {answer.Contents}");
        //                                html.Append($"</p>");
        //                            }
        //                            else
        //                            {
        //                                html.Append($"<p style='{margin}font-size:13px; text-align:{align};color:#000000;'>");
        //                                html.AppendLine($"-{answer.ContentsEs}");
        //                                if (!string.IsNullOrEmpty(answer.Contents))
        //                                    html.AppendLine($" / {answer.Contents}");
        //                                html.Append($"<span style='background-color:#D3D3D3;'>{answer.Answered?.FirstOrDefault()?.Text ?? string.Empty}</span></p>");
        //                            }
        //                            x++;
        //                            i++;
        //                        }
        //                    }
        //                    break;
        //                case "743a34fd-00df-4bf2-a5dc-034d36ad54cc"://Opción Multiple
        //                    {
        //                        var x = 1;
        //                        var i = 0;
        //                        foreach (var answer in question.Answers.OrderBy(x => x.Position))
        //                        {
        //                            var margin = i == 0 ? "margin:10px 0px 0px 20px;" : "margin:4px 0px 0px 20px;";
        //                            var style = answer.Answered?.FirstOrDefault()?.Value == true ? "background-color:#D3D3D3;" : string.Empty;
        //                            html.Append($"<p style='{margin}font-size:13px; text-align:{align};color:#000000;{style}'>");
        //                            html.AppendLine($"{x}.-{answer.ContentsEs}");
        //                            if (!string.IsNullOrEmpty(answer.Contents))
        //                                html.AppendLine($" / {answer.Contents}");
        //                            html.Append($"</p>");
        //                            x++;
        //                            i++;
        //                        }
        //                    }
        //                    break;
        //                case "0bd9a7f7-4c98-4b76-ba6e-d6f950159e3b"://Tabla
        //                    {
        //                        html.AppendLine("<table width='100%' style='font-family:Segoe UI;'>");

        //                        var width = Math.Round((decimal)100 / (decimal)question.Answers.Count());

        //                        html.AppendLine("<tr>");
        //                        foreach (var column in question.Answers.OrderBy(x => x.Position))
        //                        {
        //                            html.AppendLine($"<th style='background:#010000;color:white; font-family:Segoe UI; font-size:11pt; width:{width}%;'>{column.ContentsEs}{(string.IsNullOrEmpty(column.Contents) ? string.Empty : (" / " + column.Contents))}</th>");
        //                        }
        //                        html.AppendLine("</tr>");

        //                        if (question.Answered.Any())
        //                            foreach (var row in question.Answered.Select(x => x.Row).Distinct().OrderBy(x => x))
        //                            {
        //                                html.AppendLine("<tr style='background:white;color:#010000;font-family:Segoe UI; font-size:11pt;'>");
        //                                var position = 1;
        //                                foreach (var item in question.Answered.Where(x => x.Row == row))
        //                                {
        //                                    var value = question.Answered.Where(x => x.Row == row && x.Position == position).FirstOrDefault()?.Text ?? string.Empty;
        //                                    html.Append($"<td>{value}</td>");
        //                                    position++;
        //                                }
        //                                html.AppendLine("</tr>");
        //                            }

        //                        html.AppendLine("</table>");
        //                    }
        //                    break;
        //                case "e0cd255a-a615-4de7-936c-bacde0ee60ad": //Texto Libre
        //                    {
        //                        html.AppendLine($"<p style='margin-left:20px;font-size:11pt; text-align:{align};color:#000000;background-color:#D3D3D3;'>{question?.Answered?.FirstOrDefault()?.Text ?? string.Empty}</p>");
        //                    }
        //                    break;
        //                case "c13fdf6a-47b1-4314-b1fb-0524511c9962"://Texto Libre Númerico
        //                    {
        //                        html.AppendLine($"<p style='margin-left:20px;font-size:13px; text-align:{align};color:#000000;background-color:#D3D3D3;'>{question?.Answered?.FirstOrDefault()?.Text ?? string.Empty}</p>");
        //                    }
        //                    break;
        //            }
        //            if (!string.IsNullOrEmpty(question.FooterEs))
        //                html.AppendLine($"<p style='font-size:11px'color:#000000;>{question.FooterEs}</p>");

        //            if (!string.IsNullOrEmpty(question.Footer))
        //                html.AppendLine($"<p style='font-size:11px;color:#00AEEF;'>{question.Footer}</p>");

        //            html.AppendLine("<br/>");
        //        }
        //    }
        //    html.AppendLine("</body></html>");
        //    file.Content = await ConvertHtmlToWordInterop(html.ToString(), container.Survey.NameEs, date);
        //    file.FileName = $"{container.Survey.NameEs}.docx";

        //    return file;
        //}

        //private async Task<byte[]> ConvertHtmlToWordOpenXML(string html)
        //{
        //    using (MemoryStream generatedDocument = new MemoryStream())
        //    {
        //        using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
        //        {
        //            MainDocumentPart mainPart = package.MainDocumentPart;
        //            if (mainPart == null)
        //            {
        //                mainPart = package.AddMainDocumentPart();
        //                new Document(new Body()).Save(mainPart);

        //            }

        //            HtmlConverter converter = new HtmlConverter(mainPart);
        //            await converter.ParseBody(html);
        //            mainPart.Document.Save();
        //        }

        //        return generatedDocument.ToArray();
        //    }
        //}
        //public static byte[] GetZipArchive(List<InMemoryFile> files)
        //{
        //    byte[] archiveFile;
        //    using (var archiveStream = new MemoryStream())
        //    {
        //        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        //        {
        //            foreach (var file in files)
        //            {
        //                var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
        //                using (var zipStream = zipArchiveEntry.Open())
        //                    zipStream.Write(file.Content, 0, file.Content.Length);

        //                if (file.FolderFiles.Any())
        //                {
        //                    foreach (var fileInFolder in file.FolderFiles)
        //                    {
        //                        var zipArchiveEntry2 = archive.CreateEntry($"{file.FileName.Replace(".docx", "")}\\{fileInFolder.FileName}", CompressionLevel.Fastest);
        //                        using (var zipStream = zipArchiveEntry2.Open())
        //                            zipStream.Write(fileInFolder.Content, 0, fileInFolder.Content.Length);
        //                    }
        //                }
        //            }
        //        }

        //        archiveFile = archiveStream.ToArray();
        //    }

        //    return archiveFile;
        //}

        //public class InMemoryFile
        //{
        //    public string FileName { get; set; }
        //    public byte[] Content { get; set; }
        //    public List<FolderFiles> FolderFiles { get; set; } = new List<FolderFiles>();
        //}

        //public class FolderFiles
        //{
        //    public string FileName { get; set; }
        //    public byte[] Content { get; set; }
        //}

        //private async Task<Byte[]> ConvertHtmlToWordInterop(string html, string title, string date)
        //{
        //    var path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\wordTmp";
        //    Guid fileTmp = Guid.NewGuid();

        //    if (!Directory.Exists(path))
        //        Directory.CreateDirectory(path);

        //    await File.WriteAllTextAsync($"{path}\\{fileTmp}.html", html);

        //    Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application { Visible = false };
        //    word.Documents.Open($"{path}\\{fileTmp}.html", Format: Microsoft.Office.Interop.Word.WdOpenFormat.wdOpenFormatWebPages);
        //    Microsoft.Office.Interop.Word.Document doc = word.Documents[$"{path}\\{fileTmp}.html"];
        //    doc.PageSetup.LeftMargin = 56.7f;// doc.PageSetup.Application.CentimetersToPoints(float.Parse("2.00"));
        //    doc.PageSetup.RightMargin = 56.7f;// doc.PageSetup.Application.CentimetersToPoints(float.Parse("2.00"));
        //    doc.PageSetup.PaperSize = Microsoft.Office.Interop.Word.WdPaperSize.wdPaperLetter;
        //    doc.SaveAs2($"{path}\\{fileTmp}", Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatStrictOpenXMLDocument);
        //    doc.Close();
        //    doc = null;
        //    word.Quit();
        //    word = null;

        //    var files = new List<string> { $"{path}\\FrontPage.docx", $"{path}\\{fileTmp}.docx", $"{path}\\EndPage.docx" };

        //    var mergedFile = mergeDocumentsTest(files, title, date);

        //    return await File.ReadAllBytesAsync($"{mergedFile}.docx");

        //    //return await File.ReadAllBytesAsync($"{path}\\{fileTmp}.docx");
        //}

        //string mergeDocumentsTest(List<string> documentFiles, string title, string date)
        //{
        //    var mergedFile = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\wordTmp\\merged_{Guid.NewGuid()}";

        //    var oWord = new Microsoft.Office.Interop.Word.Application();
        //    var oDoc = oWord.Documents.Add();
        //    var oSelection = oWord.Selection;

        //    foreach (string documentFile in documentFiles)
        //    {
        //        var oCurrentDocument = oWord.Documents.Add(documentFile);
        //        copyPageSetup(oCurrentDocument.PageSetup, oDoc.Sections.Last.PageSetup);
        //        oCurrentDocument.Range().Copy();
        //        oSelection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);
        //        if (!Object.ReferenceEquals(documentFile, documentFiles.Last()))
        //            oSelection.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdSectionBreakNextPage);

        //        oCurrentDocument.Close();
        //        oCurrentDocument = null;
        //    }

        //    oDoc.SaveAs(mergedFile);
        //    oDoc.Close();

        //    object missing = System.Reflection.Missing.Value;

        //    oDoc = null;
        //    oWord.Quit(ref missing, ref missing, ref missing);
        //    oWord = null;

        //    mergedFile = SearchReplace(mergedFile, title, date);

        //    return mergedFile;
        //}

        //void copyPageSetup(Microsoft.Office.Interop.Word.PageSetup source, Microsoft.Office.Interop.Word.PageSetup target)
        //{
        //    target.PaperSize = source.PaperSize;

        //    //target.Orientation = source.Orientation; //not working in word 2003, so here is another way
        //    if (!source.Orientation.Equals(target.Orientation))
        //        target.TogglePortrait();

        //    target.TopMargin = source.TopMargin;
        //    target.BottomMargin = source.BottomMargin;
        //    target.RightMargin = source.RightMargin;
        //    target.LeftMargin = source.LeftMargin;
        //    target.FooterDistance = source.FooterDistance;
        //    target.HeaderDistance = source.HeaderDistance;
        //    target.LayoutMode = source.LayoutMode;
        //}

        //string SearchReplace(string path, string title, string date)
        //{
        //    var mergedFile = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\wordTmp\\merged_title_{Guid.NewGuid()}";
        //    object missing = System.Reflection.Missing.Value;

        //    var application = new Microsoft.Office.Interop.Word.Application();
        //    Microsoft.Office.Interop.Word.Document document = application.Documents.Add($"{path}.docx");


        //    var shapes = document.Shapes;
        //    foreach (Microsoft.Office.Interop.Word.Shape shape in shapes)
        //    {
        //        if (shape.TextFrame.HasText != 0)
        //        {
        //            var initialText = shape.TextFrame.TextRange.Text;
        //            var resultingText = initialText.Replace("<Title>", title);
        //            if (initialText != resultingText)
        //            {
        //                shape.TextFrame.TextRange.Text = resultingText;
        //            }
        //        }
        //    }

        //    foreach (Microsoft.Office.Interop.Word.Shape shape in shapes)
        //    {
        //        if (shape.TextFrame.HasText != 0)
        //        {
        //            var initialText = shape.TextFrame.TextRange.Text;
        //            var resultingText = initialText.Replace("<Date>", date);
        //            if (initialText != resultingText)
        //            {
        //                shape.TextFrame.TextRange.Text = resultingText;
        //            }
        //        }
        //    }

        //    var pag = 1;
        //    var numPages = document.Sections.Count;

        //    foreach (Microsoft.Office.Interop.Word.Section wordSection in document.Sections)
        //    {
        //        if (pag != 1)
        //        {
        //            if (pag == numPages)
        //            {
        //                var header = wordSection.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
        //                header.LinkToPrevious = false;
        //                var headerRange = header.Range;
        //                headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
        //                headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
        //                System.Drawing.Color c = System.Drawing.Color.FromArgb(0, 174, 239);
        //                headerRange.Font.TextColor.RGB = (c.R + 0x100 * c.G + 0x10000 * c.B);
        //                headerRange.Font.Size = 12;
        //                headerRange.Text = "";

        //                var footer = wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
        //                footer.LinkToPrevious = false;
        //                footer.PageNumbers.RestartNumberingAtSection = true;
        //                footer.PageNumbers.StartingNumber = 1;
        //                var footerRange = footer.Range;
        //                footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlack;
        //                footerRange.Font.Size = 10;
        //                footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                footerRange.Text = "lockton.com.mx | ©2025 Lockton México. Todos los derechos reservados.";
        //            }
        //            else
        //            {
        //                var header = wordSection.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
        //                header.LinkToPrevious = false;
        //                var headerRange = header.Range;
        //                headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
        //                headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
        //                System.Drawing.Color c = System.Drawing.Color.FromArgb(0, 174, 239);
        //                headerRange.Font.TextColor.RGB = (c.R + 0x100 * c.G + 0x10000 * c.B);
        //                headerRange.Font.Size = 12;
        //                headerRange.Text = title;

        //                wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].LinkToPrevious = false;
        //                wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.RestartNumberingAtSection = true;
        //                wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.StartingNumber = 1;
        //                Microsoft.Office.Interop.Word.Range footerRange = wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
        //                footerRange.Collapse(Microsoft.Office.Interop.Word.WdCollapseDirection.wdCollapseEnd);
        //                footerRange.Fields.Add(footerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);

        //                Microsoft.Office.Interop.Word.Paragraph p1 = footerRange.Paragraphs.Add();
        //                p1.Range.Text = "Lockton México | ";
        //                p1.Range.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlack;
        //                p1.Range.Font.Size = 10;

        //                footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
        //            }
        //        }
        //        pag++;
        //    }



        //    document.SaveAs(mergedFile);

        //    document.Close(ref missing, ref missing, ref missing);
        //    document = null;
        //    application.Quit(ref missing, ref missing, ref missing);
        //    application = null;





        //    return mergedFile;
        //}
    }

}
