﻿using MovieAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppApplication.Interface.IServices
{
    public interface IEmailService
    {
        //void SendMail(string toEmail ,string subject, string body);
        Task<string> SendSMTPEmail(EmailServiceVM emailservice);

    }
}
