using BankModel;
using BankService.BindingModels;
using BankService.Interfaces;
using BankService.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankService.ImpBD
{
    public class ReportServiceBD : IReportService
    {
        private AbstractDbContext context;

        private static SemaphoreSlim AccountSemaphore = new SemaphoreSlim(1, 1);

        private const decimal Salary = 1500;

        public ReportServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public async Task<List<AccountViewModel>> CalcSalaries()
        {
            context = AbstractDbContext.Create();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    List<SalaryBindingModel> workers = await context.Workers
                        .Select(rec => new SalaryBindingModel
                        {
                            Worker = rec,
                            Salary = Salary
                        }).ToListAsync();
                    List<AccountViewModel> result = await StartCalcTasks(workers);
                    await context.SaveChangesAsync();
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw new Exception("Не удалось рассчитать сотрудников");
                }
            }
        }
        private async Task<List<AccountViewModel>> StartCalcTasks(List<SalaryBindingModel> workers)
        {
            List<AccountViewModel> result = new List<AccountViewModel>();

            foreach (var worker in workers)
            {
                result.Add(await CalcSalary(worker));
            }

            return result;
        }

        private Task<AccountViewModel> CalcSalary(SalaryBindingModel model)
        {
            DateTime now;
            decimal salary;
            decimal bonus;
            try
            {
                DateTime lastDate = context.Accounts.Where(rec => rec.WorkerId == model.Worker.Id)
                    .Select(rec => rec.AccountDate).DefaultIfEmpty(model.Worker.DateCreate).Max();

                now = DateTime.Now;
                //получение оклада
                salary = Convert.ToDecimal(((now - lastDate).Days) * model.Salary);
                //получение премии
                bonus = Convert.ToDecimal(model.Worker.Bonus);
                //обнуление премии
                model.Worker.Bonus = 0;
                //добавление записи
                context.Accounts.Add(new Account
                {
                    WorkerId = model.Worker.Id,
                    Paid = false,
                    Salary = salary,
                    Bonus = bonus,
                    AccountDate = now
                });
            }
            catch (Exception)
            {
                return null;
            }

            return System.Threading.Tasks.Task.Run(() => new AccountViewModel
            {
                WorkerFIO = model.Worker.WorkerFIO,
                AccountDate = now.ToLongDateString(),
                Paid = "не расчитан",
                Salary = salary,
                Bonus = bonus,
                Total = salary + bonus
            });

        }

        public async Task<List<AccountViewModel>> GetAccounts(ReportBindingModel model)
        {
            var accounts = await context.Accounts
                 .Where(rec => rec.AccountDate >= model.DateFrom && rec.AccountDate <= model.DateTo).ToListAsync();
            accounts.ForEach(rec => rec.Paid = true);
            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Run(() => context.SaveChangesAsync()); ;
            return await context.Accounts
                .Include(rec => rec.Worker)
                .Where(rec => rec.AccountDate >= model.DateFrom && rec.AccountDate <= model.DateTo)
                .Select(rec => new AccountViewModel
                {
                    AccountDate = SqlFunctions.DateName("dd", rec.AccountDate) + " " +
                                            SqlFunctions.DateName("mm", rec.AccountDate) + " " +
                                            SqlFunctions.DateName("yyyy", rec.AccountDate),
                    WorkerFIO = rec.Worker.WorkerFIO,
                    Paid = (rec.Paid) ? "расчитан" : "не расчитан",
                    Salary = rec.Salary,
                    Bonus = rec.Bonus,
                    Total = rec.Salary + rec.Bonus

                })
                .OrderBy(rec => rec.AccountDate)
                .ThenBy(rec => rec.WorkerFIO)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task SaveWorkersSalaryAccount(ReportBindingModel model)
        {
            //открываем файл для работы
            FileStream fs = new FileStream(model.FileName, FileMode.OpenOrCreate, FileAccess.Write);
            //создаем документ, задаем границы, связываем документ и поток
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont(model.FontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //вставляем заголовок
            var phraseTitle = new Phrase("Расчеты с сотрудниками",
                new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(phraseTitle)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);

            var phrasePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString() +
                                    " по " + model.DateTo.Value.ToShortDateString(),
                                    new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD));
            paragraph = new iTextSharp.text.Paragraph(phrasePeriod)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);
            //вставляем таблицу, задаем количество столбцов, и ширину колонок
            PdfPTable table = new PdfPTable(6)
            {
                TotalWidth = 800F
            };
            table.SetTotalWidth(new float[] { 160, 140, 160, 100, 140, 100 });
            //вставляем шапку
            PdfPCell cell = new PdfPCell();
            var fontForCellBold = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("ФИО сотрудника", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Дата", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Расчет", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Оклад", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Бонус", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Итого", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            //заполняем таблицу
            var list = await GetAccounts(model);
            var fontForCells = new iTextSharp.text.Font(baseFont, 10);
            for (int i = 0; i < list.Count; i++)
            {
                cell = new PdfPCell(new Phrase(list[i].WorkerFIO, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].AccountDate, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Paid, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Salary.ToString(), fontForCells));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Bonus.ToString(), fontForCells));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Total.ToString(), fontForCells));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
            }
            //вставляем итого
            cell = new PdfPCell(new Phrase("Итого:", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Colspan = 3,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(list.Select(rec => rec.Salary).DefaultIfEmpty(0).Sum().ToString(), fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(list.Select(rec => rec.Bonus).DefaultIfEmpty(0).Sum().ToString(), fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(list.Select(rec => rec.Total).DefaultIfEmpty(0).Sum().ToString(), fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0
            };
            table.AddCell(cell);
            //вставляем таблицу
            doc.Add(table);
            doc.Close();
        }

        public async System.Threading.Tasks.Task SendMail(string mailto, string caption, string message, string path = null)
        {
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            SmtpClient stmpClient = null;
            try
            {
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                mailMessage.To.Add(new MailAddress(mailto));
                mailMessage.Subject = caption;
                mailMessage.Body = message;
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.BodyEncoding = Encoding.UTF8;
                if (path != null)
                {
                    mailMessage.Attachments.Add(new Attachment(path));
                }

                stmpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailLogin"].Split('@')[0],
                    ConfigurationManager.AppSettings["MailPassword"])
                };
                await stmpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mailMessage.Dispose();
                mailMessage = null;
                stmpClient = null;
            }
        }

        public async System.Threading.Tasks.Task SendWorkersSalaryDoc(ReportBindingModel model)
        {
            await AccountSemaphore.WaitAsync();
            List<AccountViewModel> list = await CalcSalaries();

            model.FileName += "WorkersReport.doc";
            var winword = new Microsoft.Office.Interop.Word.Application();
            try
            {
                object missing = System.Reflection.Missing.Value;
                //создаем документ
                Microsoft.Office.Interop.Word.Document document =
                    winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                //получаем ссылку на параграф
                var paragraph = document.Paragraphs.Add(missing);
                var range = paragraph.Range;
                //задаем текст
                range.Text = "Расчет с сотрудниками";
                //задаем настройки шрифта
                var font = range.Font;
                font.Size = 16;
                font.Name = "Times New Roman";
                font.Bold = 1;
                //задаем настройки абзаца
                var paragraphFormat = range.ParagraphFormat;
                paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphFormat.SpaceAfter = 10;
                paragraphFormat.SpaceBefore = 0;
                //добавляем абзац в документ
                range.InsertParagraphAfter();
                //создаем таблицу
                var paragraphTable = document.Paragraphs.Add(Type.Missing);
                var rangeTable = paragraphTable.Range;
                var table = document.Tables.Add(rangeTable, list.Count + 1, 4, ref missing, ref missing);
                font = table.Range.Font;
                font.Size = 14;
                font.Name = "Times New Roman";

                var paragraphTableFormat = table.Range.ParagraphFormat;
                paragraphTableFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphTableFormat.SpaceAfter = 0;
                paragraphTableFormat.SpaceBefore = 0;
                //шабка
                table.Cell(1, 1).Range.Text = "ФИО сотрудника";
                table.Cell(1, 2).Range.Text = "Оклад";
                table.Cell(1, 3).Range.Text = "Бонусы";
                table.Cell(1, 4).Range.Text = "Итого";

                for (int i = 0; i < list.Count; ++i)
                {
                    if (list[i] != null)
                    {
                        table.Cell(i + 2, 1).Range.Text = list[i].WorkerFIO;
                        table.Cell(i + 2, 2).Range.Text = list[i].Salary.ToString();
                        table.Cell(i + 2, 3).Range.Text = list[i].Bonus.ToString();
                        table.Cell(i + 2, 4).Range.Text = list[i].Total.ToString();
                    }
                }
                //задаем границы таблицы
                table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleInset;
                table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

                paragraph = document.Paragraphs.Add(missing);
                range = paragraph.Range;

                range.Text = "Дата: " + DateTime.Now.ToLongDateString();

                font = range.Font;
                font.Size = 12;
                font.Name = "Times New Roman";

                paragraphFormat = range.ParagraphFormat;
                paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphFormat.SpaceAfter = 10;
                paragraphFormat.SpaceBefore = 10;

                range.InsertParagraphAfter();
                //сохраняем
                object fileFormat = WdSaveFormat.wdFormatXMLDocument;
                document.SaveAs(model.FileName, ref fileFormat, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing);
                document.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                winword.Quit();
            }
            await SendMail(model.Mail, "Расчет сотрудника", "Отчет по расчету сотрудника от " + DateTime.Now.ToLongDateString(), model.FileName);
            File.Delete(model.FileName);
            AccountSemaphore.Release();
        }

        public async System.Threading.Tasks.Task SendWorkersSalaryXls(ReportBindingModel model)
        {
            await AccountSemaphore.WaitAsync();
            var dict = await CalcSalaries();


            model.FileName += "WorkersReport.xls";
            var excel = new Microsoft.Office.Interop.Excel.Application();
            try
            {

                if (File.Exists(model.FileName))
                {
                    excel.Workbooks.Open(model.FileName, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing);
                }
                else
                {
                    excel.SheetsInNewWorkbook = 1;
                    excel.Workbooks.Add(Type.Missing);
                    excel.Workbooks[1].SaveAs(model.FileName, XlFileFormat.xlExcel8, Type.Missing,
                        Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }

                Sheets excelsheets = excel.Workbooks[1].Worksheets;
                //Получаем ссылку на лист
                var excelworksheet = (Worksheet)excelsheets.get_Item(1);
                //очищаем ячейки
                excelworksheet.Cells.Clear();
                //настройки страницы
                excelworksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                excelworksheet.PageSetup.CenterHorizontally = true;
                excelworksheet.PageSetup.CenterVertically = true;
                //получаем ссылку на первые 3 ячейки
                Microsoft.Office.Interop.Excel.Range excelcells = excelworksheet.get_Range("A1", "E1");
                //объединяем их
                excelcells.Merge(Type.Missing);
                //задаем текст, настройки шрифта и ячейки
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Расчет с сотрудниками";
                excelcells.RowHeight = 25;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 14;

                excelcells = excelworksheet.get_Range("A2", "D2");
                excelcells.Merge(Type.Missing);
                excelcells.Value2 = "на " + DateTime.Now.ToShortDateString();
                excelcells.RowHeight = 20;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 12;
                if (dict != null)
                {
                    excelcells = excelworksheet.get_Range("A3", "A3");
                    //выделение таблицы
                    var excelTable =
                                excelworksheet.get_Range(excelcells,
                                            excelcells.get_Offset(dict.Count(), 3));
                    excelTable.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    excelTable.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    excelTable.HorizontalAlignment = Constants.xlCenter;
                    excelTable.VerticalAlignment = Constants.xlCenter;
                    excelTable.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                                            Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                                            Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic,
                                            1);
                    //шрифт шапки
                    var excelHead =
                                excelworksheet.get_Range(excelcells,
                                            excelcells.get_Offset(0, 3));
                    excelHead.Font.Bold = true;

                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = "ФИО сотрудника";
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = "Оклад";
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = "Бонусы";
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = "Итого";
                    excelcells = excelcells.get_Offset(1, -3);
                    foreach (var elem in dict)
                    {
                        //спускаемся на 2 ячейку вниз и 4 ячейкт влево
                        excelcells.ColumnWidth = 15;
                        excelcells.Value2 = elem.WorkerFIO;
                        excelcells = excelcells.get_Offset(0, 1);
                        excelcells.ColumnWidth = 15;
                        excelcells.Value2 = elem.Salary;
                        excelcells = excelcells.get_Offset(0, 1);
                        excelcells.ColumnWidth = 15;
                        excelcells.Value2 = elem.Bonus;
                        excelcells = excelcells.get_Offset(0, 1);
                        excelcells.ColumnWidth = 15;
                        excelcells.Value2 = elem.Total;
                        excelcells = excelcells.get_Offset(1, -3);
                    }
                    //Итого
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = "Итого:";
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = dict.Sum(rec => rec.Salary);
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = dict.Sum(rec => rec.Bonus);
                    excelcells = excelcells.get_Offset(0, 1);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = dict.Sum(rec => rec.Total);
                }
                //сохраняем
                excel.Workbooks[1].Save();
                excel.Workbooks[1].Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                excel.Quit();
            }
            await SendMail(model.Mail, "Расчет сотрудников", "Отчет по расчету сотрудников от " + DateTime.Now.ToLongDateString(), model.FileName);
            File.Delete(model.FileName);
            AccountSemaphore.Release();
        }
    }
}
