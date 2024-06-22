namespace server.Email_Body
{
    public class ChangeOnProject
    {
        public static string GetHtmlBody(string projectName)
        {
            return $@"
                <html>
                <body>
                    <p>Dear Worker,</p>
                    <p>This is to inform you that there has been a change in the project '{projectName}'.</p>
                    <p>Regards,<br/>Your Project Team</p>
                </body>
                </html>";
        }
    }
}
