using System.Net;
using System.Net.Mail;
using WebAPI.Entities;
using WebAPI.Models;

namespace WebAPI.Helpers;
public static class MailUtils
{
  public static async Task SendMail(string _from, string _to, string _subject, OrderProduct _body, SmtpClient client)
  {
    string templateContent = LoadTemplate("./Email.html");

    string orderDetails = GenerateOrderDetails(_body.product);
    string body = templateContent.Replace("{{OrderDetails}}", orderDetails)
    .Replace("{{address}}", _body.shipping.Address)
    .Replace("{{code}}", _body.order.Code)
    .Replace("{{date}}", _body.order.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss tt"))
    .Replace("{{price}}", _body.total)
    .Replace("{{discountValue}}", _body.discount)
    .Replace("{{finalPrice}}", _body.finalPrice);
    MailMessage message = new MailMessage(
        from: _from,
        to: _to,
        subject: _subject,
        body: body
    );
    message.BodyEncoding = System.Text.Encoding.UTF8;
    message.SubjectEncoding = System.Text.Encoding.UTF8;
    message.IsBodyHtml = true;
    message.ReplyToList.Add(new MailAddress(_from));
    message.Sender = new MailAddress(_from);

    try
    {
      await client.SendMailAsync(message);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }
  public static async Task SendMailGoogleSmtp(string _from, string _to, string _subject,
                                                              OrderProduct _body)
  {
    using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
    {
      client.Port = 587;
      client.Credentials = new NetworkCredential(AppSettings.Email, AppSettings.Password);
      client.EnableSsl = true;
      await SendMail(_from, _to, _subject, _body, client);
    }

  }

  static string LoadTemplate(string filePath)
  {
    try
    {
      // Read the contents of the template file
      return File.ReadAllText(filePath);
    }
    catch (Exception ex)
    {
      Console.WriteLine("Failed to load template: " + ex.Message);
      return string.Empty;
    }
  }

  static string GenerateOrderDetails(List<ProductDetail> items)
  {
    var html = "";

    foreach (var item in items)
    {
      html += $@"<div class='block-grid three-up no-stack' style='
  min-width: 320px;
  max-width: 680px;
  overflow-wrap: break-word;
  word-wrap: break-word;
  word-break: break-word;
  margin: 0 auto;
  background-color: transparent;
  '>
  <div style='
    border-collapse: collapse;
    display: table;
    width: 100%;
    background-color: transparent;
    display: flex;
    '>
    <div class='col num4' style='
      display: table-cell;
      vertical-align: top;
      max-width: 320px;
      min-width: 224px;
      width: 70%;
      '>
      <div class='col_cont' style='width: 100% !important'>
        <div style='
          border-top: 0px solid transparent;
          border-left: 0px solid transparent;
          border-bottom: 0px solid transparent;
          border-right: 0px solid transparent;
          padding-top: 5px;
          padding-bottom: 5px;
          padding-right: 5px;
          padding-left: 5px;
          '>
          <div style='
            color: #393d47;
            font-family: Nunito, Arial, Helvetica Neue,
            Helvetica, sans-serif;
            line-height: 1.2;
            padding-top: 10px;
            padding-right: 0px;
            padding-bottom: 10px;
            padding-left: 10px;
            '>
            <div class='txtTinyMce-wrapper' style='
              line-height: 1.2;
              font-size: 12px;
              color: #393d47;
              font-family: Nunito, Arial, Helvetica Neue,
              Helvetica, sans-serif;
              mso-line-height-alt: 14px;
              '>
              <p style='
                font-size: 14px;
                line-height: 1.2;
                word-break: break-word;
                mso-line-height-alt: 17px;
                margin: 0;
                '>
                {item.Name}
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class='col num4' style='
      display: table-cell;
      vertical-align: top;
      max-width: 320px;
      min-width: 224px;
      min-width: 134px;
      '>
      <div class='col_cont' style='width: 100% !important'>
        <div style='
          border-top: 0px solid transparent;
          border-left: 0px solid transparent;
          border-bottom: 0px solid transparent;
          border-right: 0px solid transparent;
          padding-top: 5px;
          padding-bottom: 5px;
          padding-right: 5px;
          padding-left: 5px;
          '>
          <div style='
            color: #393d47;
            font-family: Nunito, Arial, Helvetica Neue,
            Helvetica, sans-serif;
            line-height: 1.2;
            padding-top: 10px;
            padding-right: 5px;
            padding-bottom: 10px;
            padding-left: 5px;
            '>
            <div class='txtTinyMce-wrapper' style='
              line-height: 1.2;
              font-size: 12px;
              color: #393d47;
              font-family: Nunito, Arial, Helvetica Neue,
              Helvetica, sans-serif;
              mso-line-height-alt: 14px;
              '>
              <p style='
                font-size: 14px;
                line-height: 1.2;
                word-break: break-word;
                text-align: center;
                mso-line-height-alt: 17px;
                margin: 0;
                '>
                {item.Price} $/1
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class='col num4' style='
      display: table-cell;
      vertical-align: top;
      max-width: 320px;
      min-width: 224px;
      min-width: 134px;
      '>
      <div class='col_cont' style='width: 100% !important'>
        <div style='
          border-top: 0px solid transparent;
          border-left: 0px solid transparent;
          border-bottom: 0px solid transparent;
          border-right: 0px solid transparent;
          padding-top: 5px;
          padding-bottom: 5px;
          padding-right: 5px;
          padding-left: 5px;
          '>
          <div style='
            color: #393d47;
            font-family: Nunito, Arial, Helvetica Neue,
            Helvetica, sans-serif;
            line-height: 1.2;
            padding-top: 10px;
            padding-right: 5px;
            padding-bottom: 10px;
            padding-left: 5px;
            '>
            <div class='txtTinyMce-wrapper' style='
              line-height: 1.2;
              font-size: 12px;
              color: #393d47;
              font-family: Nunito, Arial, Helvetica Neue,
              Helvetica, sans-serif;
              mso-line-height-alt: 14px;
              '>
              <p style='
                font-size: 14px;
                line-height: 1.2;
                word-break: break-word;
                text-align: center;
                mso-line-height-alt: 17px;
                margin: 0;
                '>
                {item.Quantity}
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class='col num4' style='
      display: table-cell;
      vertical-align: top;
      max-width: 320px;
      min-width: 224px;
      min-width: 134px;
      '>
      <div class='col_cont' style='width: 100% !important'>
        <div style='
          border-top: 0px solid transparent;
          border-left: 0px solid transparent;
          border-bottom: 0px solid transparent;
          border-right: 0px solid transparent;
          padding-top: 5px;
          padding-bottom: 5px;
          padding-right: 5px;
          padding-left: 5px;
          '>
          <div style='
            color: #393d47;
            font-family: Nunito, Arial, Helvetica Neue,
            Helvetica, sans-serif;
            line-height: 1.2;
            padding-top: 10px;
            padding-right: 10px;
            padding-bottom: 10px;
            padding-left: 0px;
            '>
            <div class='txtTinyMce-wrapper' style='
              line-height: 1.2;
              font-size: 12px;
              color: #393d47;
              font-family: Nunito, Arial, Helvetica Neue,
              Helvetica, sans-serif;
              mso-line-height-alt: 14px;
              '>
              <p style='
                font-size: 14px;
                line-height: 1.2;
                word-break: break-word;
                text-align: right;
                mso-line-height-alt: 17px;
                margin: 0;
                '>
                {item.totalPrice} $
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>";
    }
    return html;
  }
}
