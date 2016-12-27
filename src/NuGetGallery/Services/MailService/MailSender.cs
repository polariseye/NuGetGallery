using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Polaris.Utility.MailUtil
{
    /// <summary>
    /// 邮件发送对象
    /// </summary>
    public partial class MailSender : IDisposable, IMailSender
    {
        /// <summary>
        /// smtp客户端
        /// </summary>
        private readonly SmtpClient smtpClient = null;

        /// <summary>
        /// 默认的发件地址
        /// </summary>
        private MailAddress defaultFromAddress = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client">客户端函数</param>
        public MailSender(SmtpClient client)
        {
            this.smtpClient = client;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">邮件发送配置对象</param>
        public MailSender(SmtpConfig config)
        {
            var tmpClient = new SmtpClient(config.Host);
            if (config.Port.HasValue)
                tmpClient.Port = config.Port.Value;
            if (config.EnableSsl.HasValue)
                tmpClient.EnableSsl = config.EnableSsl.Value;
            if (config.DeliveryMethod.HasValue)
                tmpClient.DeliveryMethod = config.DeliveryMethod.Value;
            if (config.UseDefaultCredentials.HasValue)
                tmpClient.UseDefaultCredentials = config.UseDefaultCredentials.Value;
            if (config.Credentials != null)
                tmpClient.Credentials = config.Credentials;
            if (config.PickupDirectoryLocation != null)
                tmpClient.PickupDirectoryLocation = config.PickupDirectoryLocation;


            this.smtpClient = tmpClient;
        }

        /// <summary>
        /// 设置默认的发件地址
        /// </summary>
        /// <param name="fromAddress">发件地址</param>
        public void SetDefaultFromAddress(MailAddress fromAddress)
        {
            this.defaultFromAddress = fromAddress;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="message">消息发送体</param>
        public void Send(MailMessage message)
        {
            this.smtpClient.Send(message);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toAddress">收件地址</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        public void Send(string toAddress, string subject, string content)
        {
            if (this.defaultFromAddress == null)
            {
                throw new ArgumentNullException(String.Empty, "未设置默认的发件地址");
            }

            Send(this.defaultFromAddress, GetMailAddressList(toAddress), subject, content);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromAddress">发件地址</param>
        /// <param name="toAddress">收件地址,多个收件人，则以;间隔</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        public void Send(string fromAddress, string toAddress, string subject, string content)
        {
            if (this.defaultFromAddress == null)
            {
                throw new ArgumentNullException(String.Empty, "未设置默认的发件地址");
            }


            Send(new MailAddress(fromAddress), GetMailAddressList(toAddress), subject, content);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toAddress">收件地址</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        public void Send(List<MailAddress> toAddress, string subject, string content)
        {
            Send(this.defaultFromAddress, toAddress, subject, content);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromAddress">发件地址</param>
        /// <param name="toAddress">收件地址</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        public void Send(MailAddress fromAddress, List<MailAddress> toAddress, string subject, string content)
        {
            string htmlBody = new Markdown().Transform(content);
            var mailMessage = new MailMessage(fromAddress, toAddress[0])
            {
                Subject = subject,
                Body = htmlBody
            };

            // 设置内容是html代码
            mailMessage.IsBodyHtml = true;

            if (toAddress.Count > 1)
            {
                for (int i = 1; i < toAddress.Count; i++)
                {
                    mailMessage.To.Add(toAddress[i]);
                }
            }

            Send(mailMessage);
        }

        /// <summary>
        /// 获取邮件地址列表
        /// </summary>
        /// <param name="toAddr">要接收的邮件地址列表</param>
        /// <returns></returns>
        private List<MailAddress> GetMailAddressList(string toAddr)
        {
            var toList = toAddr.Split(';');
            List<MailAddress> addrList = new List<MailAddress>();
            foreach (var addr in toList)
            {
                addrList.Add(new MailAddress(addr));
            }

            return addrList;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (this.smtpClient != null)
            {
                this.smtpClient.Dispose();
            }
        }
    }
}
