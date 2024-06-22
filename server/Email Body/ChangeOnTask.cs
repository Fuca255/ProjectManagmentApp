namespace server.Email_Body
{
    public class ChangeOnTask
    {
        public static string GetHtmlBody(string taskName)
        {
            return $@"
                <html>
                <body>
                    <p>Dear Worker,</p>
                    <p>This is to inform you that there has been a change in the task '{taskName}'.</p>
                    <p>Regards,<br/>Your Project Team</p>
                </body>
                </html>";
        }
    }
}
