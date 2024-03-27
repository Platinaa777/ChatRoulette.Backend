namespace EmailingService.Api.EmailUtils;

public static class EmailTemplate
{
  public const string START = @"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
              <meta charset=""UTF-8"">
              <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
              <title>Confirm Your Email</title>
              <style>
                body {
                  font-family: Arial, sans-serif;
                  background-color: #f4f4f4;
                  margin: 0;
                  padding: 0;
                }
                .container {
                  max-width: 600px;
                  margin: 0 auto;
                  padding: 20px;
                  background-color: #fff;
                  border-radius: 8px;
                  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }
                h1 {
                  color: #333;
                }
                p {
                  color: #666;
                }
                .btn {
                  display: inline-block;
                  padding: 10px 20px;
                  background-color: #007bff;
                  color: #fff; /* Set text color to white */
                  text-decoration: none;
                  border-radius: 5px;
                }
                .btn:hover {
                  background-color: #0056b3;
                }
              </style>
            </head>
            <body>
              <div class=""container"">
                <h1>Confirm Your Email</h1>
                <p>Thank you for signing up! Please confirm your email address by clicking the link below:</p>
                <a href=""";
    
  public const string END = @""" class=""btn"">Confirm Email</a>
                <p>If you didn't request this, you can safely ignore this email.</p>
              </div>
            </body>
            </html>";
}
