using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace DCSA.Helpers
{
    public class EmailManager
    {
        public static bool SendEmail(string toEmail,double amount,string CauseName,string RName)
        {
            var db = new DCSA.Database.DefaultConnection();

            try
            {
                string senderEmail = db.SystemEmails.First().Email;
                string senderPassword = db.SystemEmails.First().Password;

                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword);
                client.Port = int.Parse(db.SystemEmails.First().Port); // 25 587
                client.Host = db.SystemEmails.First().Host;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;

                string emailBody = @"<div style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;background-color:#f5f8fa;color:#74787e;height:100%;line-height:1.4;margin:0;width:100%!important;word-break:break-word'>
<div style='box-sizing:border-box;max-width:800px;margin:auto;padding:30px;border:1px solid #eee;font-size:16px;line-height:24px;color:#555;direction:rtl;font-family:Tahoma,'Helvetica Neue','Helvetica',Helvetica,Arial,sans-serif'>
    <table cellpadding='0' cellspacing='0' style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;width:100%;line-height:inherit;text-align:right'>
        <tbody><tr>
            <td colspan='2' style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top'>
                <table style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;width:100%;line-height:inherit;text-align:right'>
                    <tbody><tr>
                        <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top;padding-bottom:20px;text-align:right'>
                            <h2 style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;color:#2f3133;font-size:16px;font-weight:bold;margin-top:0;text-align:right'>الجمعية الخيرية لمتلازمة داون</h2>
                             السعودية الرياض الرياض الرياض<br>
                        </td>
                        <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top;padding-bottom:20px;font-size:45px;line-height:45px;color:#333;text-align:left'>
                            <a href='https://dsca-store.com/' style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;color:#3869d4' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://dsca-store.com/&amp;source=gmail&amp;ust=1650648914291000&amp;usg=AOvVaw0GYkx7fpoVjzNNvWyN3Bgo'><img src='https://ci3.googleusercontent.com/proxy/z-BptV-_INw-MA5oDKbutIZPKol3r6js_2-yiCXyMLgioAwmmDSHORujfakie1wXhxlo-9wAeCm0E-Cvcfn1hbt2uQ5tesV9q-aaVqVUQylvrWeFKv7TUkq36NVoGH1vUJsE6i1EJgbuGEyccLiYQ00y_vsrPyuZ7KTRdPw=s0-d-e1-ft#https://media.zid.store/3856a3be-28b8-4400-8631-265f3f64366f/84ebbdce-6606-4c03-9625-01c35dc54b33-200x.png' style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;border:none;width:100%;max-width:300px' class='CToWUd'></a>
                        </td>

                    </tr>
                </tbody></table>
            </td>
        </tr>

        <tr>
            <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top;text-align:right'>
                <h3 style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;color:#2f3133;font-size:14px;font-weight:bold;margin-top:0;text-align:right'>المكرمـ/ـة "+ RName + @"،</h3>
                <p style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;color:#74787e;font-size:16px;line-height:1.5em;margin-top:0;text-align:right'>شكرًا لجزيل مساهمتك بالتبرع   لمشروع  مبرة الاستدامة مشروع وقف تعليمي خيري تحت التأسيس لذوي متلازمة داون وأقرانهم من ذوي الإعاقة العقلية </p>
                <p style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;color:#74787e;font-size:16px;line-height:1.5em;margin-top:0;text-align:right'>نسأل الله أن يتقبل منك ويديم عليك النعم ويدفع عنك النقم.</p>
                <small style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box'>*سيتم إعلامكم في حال اكتمال المشروع قريبًا*</small>
                <br>
                <br>
            </td>
        </tr>

        <tr>
            <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top;text-align:right'>
                
                <p style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;color:#74787e;font-size:16px;line-height:1.5em;margin-top:0;text-align:right'><b style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box'>التاريخ: </b>"+DateTime.Now+@"</p>
            </td>
        </tr>

        <tr>
            <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top;background:#eee;border-bottom:1px solid #ddd;font-weight:bold'>المشروع</td>
            <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top;background:#eee;border-bottom:1px solid #ddd;font-weight:bold;text-align:right'>الكمية</td>
            <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top;background:#eee;border-bottom:1px solid #ddd;font-weight:bold'>القيمة</td>
        </tr>

                    <tr>
                <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top;border-bottom:none'>"+CauseName+@"</td>
                                    <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top;border-bottom:none;text-align:right'>1</td>
                    <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top;border-bottom:none'>"+amount+ @" SAR</td>
                            </tr>
                <tr>
            <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top'></td>

            <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top;border-top:2px solid #eee;font-weight:bold;text-align:right'>الإجمالي: " + amount + @" SAR</td>
        </tr>
        <tr>
            <td style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;padding:5px;vertical-align:top'>
                <p style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;color:#74787e;font-size:16px;line-height:1.5em;margin-top:0;text-align:center!important'>هذا الإيصال الكتروني لا يحتاج إلى ختم أو توقيع</p>
                <p style='font-family:Avenir,Helvetica,sans-serif;box-sizing:border-box;color:#74787e;font-size:16px;line-height:1.5em;margin-top:0;text-align:center!important'>© 2022 الجمعية الخيرية لمتلازمة داون</p>
            </td>
        </tr>
    </tbody></table>
</div>
</div>";

                  MailMessage mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(new MailAddress(toEmail));
                mail.Subject = "شكرًا لمساهمتك الخيرية لمنصة الجمعية الخيرية لمتلازمة داون";
                mail.Body = emailBody;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mail);


                return true;

            }
            catch (Exception ex)
            {
                return false;

            }

        }
    }
}