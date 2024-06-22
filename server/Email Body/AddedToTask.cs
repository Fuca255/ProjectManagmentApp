namespace server.Email_Body
{
    public class AddedToTask
    {
        public static string GetHtmlBody(string taskName)
        {
            return $@"
                <html>
                <body>
                    <p>Dear Worker,</p>
                    <p>You've been added to the task '{taskName}'.</p>
                    <p>Regards,<br/>Your Project Team</p>
                </body>
                </html>";
        }
    }
}
