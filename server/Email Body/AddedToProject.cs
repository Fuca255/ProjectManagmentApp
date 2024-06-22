namespace server.Email_Body
{
    public class AddedToProject
    {
        public static string GetHtmlBody(string projectName)
        {
            return $@"
                <html>
                <body>
                    <p>Dear Worker,</p>
                    <p>You've been added to the project '{projectName}'.</p>
                    <p>Regards,<br/>Your Project Team</p>
                </body>
                </html>";
        }
    }
}
