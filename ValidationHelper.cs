using System.Net.Mail;

namespace Ecoplaza // Пространство имен вашего проекта
{
    public class ValidationHelper
    {

        // Метод проверки корректности номера телефона
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            // 1. Проверка количества цифр
            int digitCount = phoneNumber.Count(char.IsDigit);
            if (digitCount < 6 || digitCount > 16)
                return false;

            // 2. Проверка начала номера
            if (!(phoneNumber.StartsWith("+") || char.IsDigit(phoneNumber[0])))
                return false;

            // 3. Проверка наличия скобок
            int openingBracketIndex = phoneNumber.IndexOf("(");
            int closingBracketIndex = phoneNumber.IndexOf(")");
            if ((openingBracketIndex >= 0 && closingBracketIndex < 0) || (openingBracketIndex < 0 && closingBracketIndex >= 0))
                return false;
            if (openingBracketIndex >= 0 && closingBracketIndex >= 0)
            {
                if (closingBracketIndex - openingBracketIndex < 4)
                    return false;
                if (!phoneNumber.Substring(openingBracketIndex + 1, closingBracketIndex - openingBracketIndex - 1).All(char.IsDigit))
                    return false;
                if (openingBracketIndex > 0 && !char.IsWhiteSpace(phoneNumber[openingBracketIndex - 1]) && !char.IsDigit(phoneNumber[openingBracketIndex - 1]))
                    return false;
                if (closingBracketIndex < phoneNumber.Length - 1 && !char.IsWhiteSpace(phoneNumber[closingBracketIndex + 1]) && !char.IsDigit(phoneNumber[closingBracketIndex + 1]))
                    return false;
            }

            // 4. Проверка повторяющихся символов, кроме цифр
            for (int i = 0; i < phoneNumber.Length - 1; i++)
            {
                if (!char.IsDigit(phoneNumber[i]) && phoneNumber[i] == phoneNumber[i + 1])
                    return false;
            }

            // 5. Проверка допустимых символов
            foreach (char c in phoneNumber)
            {
                if (!(char.IsDigit(c) || c == '+' || c == '-' || c == '(' || c == ')' || c == ' '))
                    return false;
            }

            // Если все проверки пройдены, возвращаем true
            return true;
        }



        // Метод для проверки корректности email адреса
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }





    }
}
