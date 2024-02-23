using System.Text;

namespace EmailingService.Utils;

public class ConfirmCodeGenerator
{
    public static int GenerateCode()
    {
        StringBuilder code = new();
        Random rnd = new Random();
        for (int i = 0; i < 4; ++i)
        {
            code.Append(rnd.Next(0, 10));
        }

        return int.Parse(code.ToString());
    }
}