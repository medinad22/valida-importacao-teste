using MailKit.Net.Smtp;
using MailKit.Security;

namespace genesis_valida_importacao_teste.consome_smtp
{
    public class ConsomeSMTP
    {
        public static void Connect()
        {
            using (var client = new SmtpClient())
            {
                client.Connect("localhost", 1025, SecureSocketOptions.None);
                client.Authenticate("username", "password");
                client.Disconnect(true);
            }
        }

        public static void VerifyAddress()
        {
            MailKit.Net.Imap.IImapClient imapClient = new MailKit.Net.Imap.ImapClient();
            imapClient.Connect("localhost", 1025, false);
            imapClient.Authenticate("username", "password");
            MailKit.IMailFolder inbox = imapClient.Inbox;
            imapClient.Disconnect(true);
            
        }

    }

}
