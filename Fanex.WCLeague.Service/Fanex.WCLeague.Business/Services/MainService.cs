using Fanex.MailDelivery.Core.Models;
using Fanex.WCLeague.Business.Models;
using System;

namespace Fanex.WCLeague.Business.Services
{
    public interface MainService
    {
        JobMessage Execute(string league);
    }

    public class MainServiceImp(
        Log log,
        MailService mailService,
        DataService dataService,
        ExcelService excelService)
        : MainService
    {
        public JobMessage Execute(string league)
        {
            var result = new ExecuteResultModel();
            JobMessage jobMessage = new JobMessage();
            try
            {
                result = SendNotification(league);
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
                log.Log(e);
            }
            finally
            {
                jobMessage.Mail = result.Mail;
                jobMessage.JobStatus = result.Success ? JobStatus.Success : JobStatus.DataIsNotReady;
                jobMessage.TaskLog = new TaskLog
                {
                    LogTime = DateTime.Now,
                    Message = result.Message
                };
            }

            return jobMessage;
        }

        private ExecuteResultModel SendNotification(string league)
        {
            var excelFile = excelService.Export(league);
            var mail = mailService.CreateMail(excelFile, league);
            return new ExecuteResultModel
            {
                Mail = mail,
                Success = true,
                Message = string.Empty
            };
        }
    }
}