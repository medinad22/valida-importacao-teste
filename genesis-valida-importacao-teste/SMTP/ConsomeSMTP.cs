using genesis_valida_importacao_teste.valida_arquivo;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace genesis_valida_importacao_teste.consome_smtp
{
    public class ConsomeSMTP(IOptions<ValidadoresOptions> validadoresOptions)
    {
        private readonly ValidadoresOptions validadoresConfig = validadoresOptions.Value;

        public static void Connect()
        {
            using var client = new SmtpClient();
            client.Connect("localhost", 1025, SecureSocketOptions.None);
            client.Authenticate("username", "password");

            client.Disconnect(true);
        }
        public static void ConnectPOP3()
        {
            using var client = new Pop3Client();
            client.Connect("localhost", 1025, SecureSocketOptions.None);
            client.Authenticate("username", "password");

            client.Disconnect(true);
        }
        public void ConnectIMAP()
        {
            var client = new ImapClient();

            client.Connect("localhost", 1025, SecureSocketOptions.None);
            client.Authenticate("username", "password");



            MailKit.IMailFolder inbox = client.Inbox;
            var folder = client.GetFolder(inbox.FullName);

            IList<MailKit.IMailFolder> mailFolders = folder.GetSubfolders();

            foreach (var mailFolder in mailFolders)
            {
                Layouts? validador = validadoresConfig.Validadores.Find(id => id.Diretorio == mailFolder.Name) ?? throw new ArgumentException();
                MailKit.FolderAccess access = folder.Access;

                IEnumerable<MimeKit.MimeMessage> enumerable = folder.Distinct();

                foreach (var message in enumerable)
                {
                    foreach (var attachment in message.Attachments)
                    {
                        using var memory = new MemoryStream();
                        var fileName = "";
                        if (attachment is MimePart attachmentMime)
                        {
                            attachmentMime.Content.DecodeTo(memory);
                            fileName = attachmentMime.FileName;
                            long fileSize = MeasureAttachmentSize(attachmentMime);
                            ValidaArquivo.Valida(fileName, fileSize, validador);
                        }
                        else
                        {
                            // Se não for anexo?
                        }

                        var bytes = memory.ToArray();
                    }
                }
            }

            client.Disconnect(true);

        }

        private static long MeasureAttachmentSize(MimePart part)
        {
            using var measure = new MimeKit.IO.MeasuringStream();
            part.Content.DecodeTo(measure);
            return measure.Length;
        }
    }

}
