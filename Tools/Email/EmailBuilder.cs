using System.Net;
using System.Net.Mail; 

namespace coer91.Tools
{
    public abstract class EmailBuilder
    {
        //From
        protected string _from = "";
        protected string _sender = "";
        protected IEnumerable<string> _to = [];
        protected IEnumerable<string> _cc = [];
        protected string _userName = "";
        protected string _password = "";
        protected string _subject = "";
        protected string _body = "";

        //Settings
        protected string _host;
        protected int _port;
        protected bool _enableSsl = true;
        protected bool _useDefaultCredentials = false;
        protected SmtpDeliveryMethod _deliveryMethod = SmtpDeliveryMethod.Network;
        protected bool _isBodyHtml = true; 

        public async Task<ResponseDTO> Send()
        {
            ResponseDTO response = new();

            try
            {
                SmtpClient smtp = GetSMTPClient();

                MailMessage mail = new()
                {
                    From = new MailAddress(_from, _sender),
                    Subject = _subject,
                    Body = _body,
                    IsBodyHtml = _isBodyHtml
                };
                                 
                foreach (string to in _to) mail.To.Add(to);
                foreach (string cc in _cc) mail.CC.Add(cc);
                await smtp.SendMailAsync(mail);
            }

            catch (Exception ex)
            {
                return response.Exception(ex);
            }

            return response;
        }


        private SmtpClient GetSMTPClient() => new()
        {
            Host = _host,
            Port = _port,
            EnableSsl = _enableSsl,
            UseDefaultCredentials = _useDefaultCredentials,
            DeliveryMethod = _deliveryMethod,
            Credentials = new NetworkCredential(_userName, _password)
        };
    }
}