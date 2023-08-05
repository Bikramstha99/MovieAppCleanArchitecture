using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppDomain.Entities
{
    public class EmailServiceVM
    {
        public string Subject { get; set; }
        public string ReceiverEmail { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string HtmlContent { get; set; }
    }
}
