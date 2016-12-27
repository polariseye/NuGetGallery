using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Polaris.Utility.MailUtil
{
    /// <summary>
    /// 邮件发送接口
    /// </summary>
    public interface IMailSender : IDisposable
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="message">消息发送体</param>
        void Send(MailMessage message);

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toAddress">收件地址</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        void Send(string toAddress, string subject, string content);

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toAddress">收件地址</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        void Send(List<MailAddress> toAddress, string subject, string content);

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromAddress">发件地址</param>
        /// <param name="toAddress">收件地址,多个收件人，则以;间隔</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        void Send(string fromAddress, string toAddress, string subject, string content);

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromAddress">发件地址</param>
        /// <param name="toAddress">收件地址</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        void Send(MailAddress fromAddress, List<MailAddress> toAddress, string subject, string content);

        /// <summary>
        /// 设置默认的发件地址
        /// </summary>
        /// <param name="fromAddress">发件地址</param>
        void SetDefaultFromAddress(MailAddress fromAddress);
    }
}