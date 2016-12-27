using System;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Polaris.Utility.MailUtil
{
    /// <summary>
    /// 邮件发送配置对象
    /// </summary>
    public class SmtpConfig
    {
        /// <summary>
        /// 身份信息
        /// </summary>
        public ICredentialsByHost Credentials { get; set; }

        /// <summary>
        /// 邮件发送方式
        /// </summary>
        public SmtpDeliveryMethod? DeliveryMethod { get; set; }

        /// <summary>
        /// 是否使用ssl
        /// </summary>
        public bool? EnableSsl { get; set; }

        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 邮件存储的临时地址
        /// </summary>
        public string PickupDirectoryLocation { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// 是否使用默认的认证信息,只有在DeliveryMethod为<see cref="SmtpDeliveryMethod.SpecifiedPickupDirectory"/>，时才有效
        /// </summary>
        public bool? UseDefaultCredentials { get; set; }

        /// <summary>
        /// 通过url形式的格式构造配置信息
        /// </summary>
        /// <param name="smtpUri">smtp地址信息
        /// 格式形式如: smtps://username:password@host:port.
        /// 如果包含特殊符号，则需要外部先进行uri编码
        /// </param>
        public SmtpConfig(string smtpUri)
        {
            var tmpSmtpUri = new SmtpUri(new Uri(smtpUri));

            this.DeliveryMethod = SmtpDeliveryMethod.Network;
            this.Host = tmpSmtpUri.Host;
            this.Port = tmpSmtpUri.Port;
            this.EnableSsl = tmpSmtpUri.Secure;

            if (!string.IsNullOrWhiteSpace(tmpSmtpUri.UserName))
            {
                this.UseDefaultCredentials = false;
                this.Credentials = new NetworkCredential(tmpSmtpUri.UserName, tmpSmtpUri.Password);
            }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SmtpConfig()
        {
        }

        /// <summary>
        /// 设置认证信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        public void SetCredentials(string userName, string password)
        {
            this.Credentials = new NetworkCredential(userName, password);
        }

        /// <summary>
        /// smtpuri解析
        /// </summary>
        private class SmtpUri
        {
            /// <summary>
            /// 解析正则
            /// </summary>
            private static readonly Regex UserInfoParser = new Regex("^(?<username>[^:]*):(?<password>.*)$");

            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName { get; private set; }

            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; private set; }

            /// <summary>
            /// 地址
            /// </summary>
            public string Host { get; private set; }

            /// <summary>
            /// 端口
            /// </summary>
            public int Port { get; private set; }

            /// <summary>
            /// 是否使用ssl连接
            /// </summary>
            public bool Secure { get; private set; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="uri"></param>
            public SmtpUri(Uri uri)
            {
                Secure = uri.Scheme.Equals("smtps", StringComparison.OrdinalIgnoreCase);
                if (!Secure && !uri.Scheme.Equals("smtp", StringComparison.OrdinalIgnoreCase))
                {
                    throw new FormatException("Invalid SMTP URL: " + uri.ToString());
                }

                var m = UserInfoParser.Match(uri.UserInfo);
                if (m.Success)
                {
                    UserName = WebUtility.UrlDecode(m.Groups["username"].Value);
                    Password = WebUtility.UrlDecode(m.Groups["password"].Value);
                }
                else
                {
                    UserName = WebUtility.UrlDecode(uri.UserInfo);
                }
                Host = uri.Host;
                Port = uri.IsDefaultPort ? 25 : uri.Port;
            }
        }
    }
}
