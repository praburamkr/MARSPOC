using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Common.Notification
{
    public class SendMailRequest
    {
        public SendMailRequest(string recipient, string cc, string replyTo, string subject, string body,
                               string filecontent, string filename)
        {
            this.recipient = recipient;
            this.cc = cc;
            this.replyto = replyTo;
            this.subject = subject;
            this.body = body;
            this.filecontent = filecontent;
            this.filename = filename;
        }
        public string recipient { get; set; }
        public string cc { get; set; }
        public string replyto { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string filecontent { get; set; }
        public string filename { get; set; }
    }
}
