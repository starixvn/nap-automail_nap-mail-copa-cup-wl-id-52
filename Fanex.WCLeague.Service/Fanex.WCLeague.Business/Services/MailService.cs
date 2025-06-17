using Fanex.MailDelivery.Core.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fanex.WCLeague.Business.Services
{
    public interface MailService
    {
        Mail CreateMail(string fileName);
    }

    public class MailServiceImp : MailService
    {
        public Mail CreateMail(string fileName)
        {
            var atts = new List<MailAttachment>
            {
                new MailAttachment(fileName, FileExtension.MS_EXCEL)
            };
            var shortName = new FileInfo(fileName).Name;

            var mail = new Mail
            {
                Title = "Auto Generated Report Administrator",
                Subject = $"Automail Report - Daily WL Report - Club WC 2025 USA - {DateTime.Today:yyyy/MM/dd}",
                Body = LoadBody(shortName),
                Attachments = atts
            };

            return mail;
        }

        private static string LoadBody(string fileName)
        {
            var template = File.ReadAllText(@"Templates/EmailTemplate.cshtml");
            var tuple = new Tuple<string>(fileName);
            const string tplKey = "CCLeague-ID52-Template";
            var result = Engine.Razor.IsTemplateCached(tplKey, null)
                ? Engine.Razor.Run(tplKey, null, tuple)
                : Engine.Razor.RunCompile(template, tplKey, model: tuple);

            return result;
        }
    }
}