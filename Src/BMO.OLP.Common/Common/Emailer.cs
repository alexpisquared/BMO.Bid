using AsLink;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace BMO.OLP.Common
{
  public static class Emailer
  {
    public static async Task<bool> TrySend(string from, string to__, string subject, string body, string host, string epName, string epAdrs)
    {
      var isSuccess = false;
      try
      {
        isSuccess = await Emailer.Send(
          string.IsNullOrEmpty(from) ? "creditobligor.support@BMO.com" : from,
          string.IsNullOrEmpty(to__) ? "creditobligor.support@BMO.com" : to__,
          subject, body, host);
      }
      catch (Exception ex) { DevOp.ExHrT(ex, System.Reflection.MethodInfo.GetCurrentMethod()); }
      DevOp.SysLogger.LogMessage(string.Format("Send email by App: {0}.", isSuccess ? "SUCCESS" : "FAILURE"));

      if (isSuccess) return isSuccess;

      try
      {
        //using (var svc = new OLServiceReference.OLServiceClient(epName/*, epAdrs*/)) //todo: test epAdrs
        //{
        //  try { isSuccess = await svc.SendEmailAsync(from, to__, subject, body); }
        //  catch (Exception ex) { DevOp.ExHrT(ex, System.Reflection.MethodInfo.GetCurrentMethod()); }
        //  finally { svc.Close(); }
        //}
      }
      catch (Exception ex) { DevOp.ExHrT(ex, System.Reflection.MethodInfo.GetCurrentMethod()); }
      DevOp.SysLogger.LogMessage(string.Format("Send email by WS: {0}.", isSuccess ? "SUCCESS" : "FAILURE"));

      return isSuccess;
    }

    async static Task<bool> Send(string from, string to, string subject, string body, string host, string[] attachmentFiles = null, string[] signatureImages = null)
    {
      try
      {
        using (var mailMessage = new MailMessage(from, to, subject, body))
        {
          mailMessage.IsBodyHtml = body.Length > 0 && body.Substring(0, 1) == "<";

          if (signatureImages != null) addSignatureImages(body, signatureImages, mailMessage);

          if (attachmentFiles != null) attachmentFiles.Where(r => !string.IsNullOrEmpty(r) && File.Exists(r)).ToList().ForEach(fnm => mailMessage.Attachments.Add(new Attachment(fnm)));

          await new SmtpClient { Host = host }.SendMailAsync(mailMessage);
        }

        return true;
      }
      catch (FormatException ex) { DevOp.ExHrT(ex, System.Reflection.MethodInfo.GetCurrentMethod()); }
      catch (SmtpException ex) { DevOp.ExHrT(ex, System.Reflection.MethodInfo.GetCurrentMethod()); }
      catch (Exception ex) { DevOp.ExHrT(ex, System.Reflection.MethodInfo.GetCurrentMethod()); }

      return false;
    }

    static void addSignatureImages(string body, string[] signatureImages, MailMessage mailMessage)
    {
      signatureImages.Where(img => !string.IsNullOrEmpty(img) && File.Exists(img)).ToList().ForEach(img =>
      {
        var contentId = Guid.NewGuid().ToString();
        var html_View = AlternateView.CreateAlternateViewFromString(body.Replace("Logo.png", "cid:" + contentId), null, "text/html"); // (body + "<img src=\"cid:" + contentId + "\" alt=\"MCSD\"><hr />", null, "text/html");
        var imageResource = new LinkedResource(img, new ContentType(MediaTypeNames.Image.Jpeg));
        imageResource.ContentId = contentId;
        html_View.LinkedResources.Add(imageResource);
        mailMessage.AlternateViews.Add(html_View);
        //var plainView = AlternateView.CreateAlternateViewFromString(body, null, "text/plain");
        //mailMessage.AlternateViews.Add(plainView);
      });
    }
  }
}
