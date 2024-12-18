using System;
using System.Buffers.Text;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace AKSS_Management
{

    public static class StringUtilities
    {
        // Check if a string is null, empty, or whitespace
        //bool isEmpty = StringUtilities.IsNullOrEmptyOrWhitespace("   ");
        //Console.WriteLine($"Is Empty: {isEmpty}");

        public static bool IsNullOrEmptyOrWhitespace(string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        // Convert string to title case
        //string titleCase = StringUtilities.ToTitleCase("hello world");
        //Console.WriteLine($"Title Case: {titleCase}");

        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        // Reverse a string
        //string reversed = StringUtilities.Reverse("hello");
        //Console.WriteLine($"Reversed: {reversed}");

        public static string Reverse(string input)
        {
            if (input == null) return null;
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        // Generate a random string
        //string randomString = StringUtilities.GenerateRandomString(10);
        //Console.WriteLine($"Random String: {randomString}");

        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Remove all whitespace from a string
        //string withoutWhitespace = StringUtilities.RemoveWhitespace("h e l l o");
        //Console.WriteLine($"Without Whitespace: {withoutWhitespace}");

        public static string RemoveWhitespace(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? input : new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        // Check if a string contains only digits
        //bool isDigits = StringUtilities.IsDigitsOnly("12345");
        //Console.WriteLine($"Is Digits Only: {isDigits}");

        public static bool IsDigitsOnly(string input)
        {
            return input.All(char.IsDigit);
        }

        // Convert string to camel case
        //string camelCase = StringUtilities.ToCamelCase("Hello World");
        //Console.WriteLine($"Camel Case: {camelCase}");

        public static string ToCamelCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            input = ToTitleCase(input);
            return char.ToLowerInvariant(input[0]) + input.Substring(1);
        }

        // Convert string to snake case
        //string snakeCase = StringUtilities.ToSnakeCase("HelloWorld");
        //Console.WriteLine($"Snake Case: {snakeCase}");

        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            var regex = new Regex("([a-z])([A-Z])");
            return regex.Replace(input, "$1_$2").ToLower();
        }

        // Truncate a string to a specified length
        //string truncated = StringUtilities.Truncate("This is a long string", 10);
        //Console.WriteLine($"Truncated: {truncated}");

        public static string Truncate(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input) || maxLength <= 0) return string.Empty;
            return input.Length <= maxLength ? input : input.Substring(0, maxLength);
        }

        // Count occurrences of a character in a string
        //int occurrences = StringUtilities.CountOccurrences("hello", 'l');
        //Console.WriteLine($"Occurrences of 'l': {occurrences}");

        public static int CountOccurrences(string input, char character)
        {
            if (input == null) return 0;
            return input.Count(c => c == character);
        }

        // Replace multiple spaces with a single space
        //string normalized = StringUtilities.NormalizeSpaces("This   is   a   test");
        //Console.WriteLine($"Normalized Spaces: {normalized}");

        public static string NormalizeSpaces(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            return Regex.Replace(input, @"\s+", " ");
        }

        // Check if a string is a valid email
        //bool isValidEmail = StringUtilities.IsValidEmail("test@example.com");
        //Console.WriteLine($"Is Valid Email: {isValidEmail}");

        public static bool IsValidEmail(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(input);
                return addr.Address == input;
            }
            catch
            {
                return false;
            }
        }

        // Convert string to base64
        //string base64 = StringUtilities.ToBase64("hello");
        //Console.WriteLine($"Base64: {base64}");

        public static string ToBase64(string input)
        {
            if (input == null) return null;
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        // Convert base64 to string
        //string fromBase64 = StringUtilities.FromBase64(base64);
        //Console.WriteLine($"From Base64: {fromBase64}");

        public static string FromBase64(string base64Input)
        {
            if (base64Input == null) return null;
            var bytes = Convert.FromBase64String(base64Input);
            return Encoding.UTF8.GetString(bytes);
        }

        // Generate MD5 hash for a string
        //string md5Hash = StringUtilities.GenerateMD5Hash("hello");
        //Console.WriteLine($"MD5 Hash: {md5Hash}");

        public static string GenerateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = md5.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        // Generate SHA256 hash for a string
        //string sha256Hash = StringUtilities.GenerateSHA256Hash("hello");
        //Console.WriteLine($"SHA256 Hash: {sha256Hash}");

        public static string GenerateSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        // Convert string to hexadecimal
        //string hex = StringUtilities.ToHex("hello");
        //Console.WriteLine($"Hexadecimal: {hex}");

        public static string ToHex(string input)
        {
            if (input == null) return null;
            var bytes = Encoding.UTF8.GetBytes(input);
            var hex = BitConverter.ToString(bytes).Replace("-", "");
            return hex;
        }

        // Convert hexadecimal to string
        //string fromHex = StringUtilities.FromHex(hex);
        //Console.WriteLine($"From Hexadecimal: {fromHex}");

        public static string FromHex(string hexInput)
        {
            if (hexInput == null) return null;
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return Encoding.UTF8.GetString(bytes);
        }

        // Validate if a string is a valid URL
        //bool isValidURL = StringUtilities.IsValidURL("https://example.com");
        //Console.WriteLine($"Is Valid URL: {isValidURL}");

        public static bool IsValidURL(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            return Uri.TryCreate(input, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        // Escape HTML entities in a string
        //string escapedHtml = StringUtilities.EscapeHtml("<div>Hello & Welcome</div>");
        //Console.WriteLine($"Escaped HTML: {escapedHtml}");

        public static string EscapeHtml(string input)
        {
            if (input == null) return null;
            return System.Net.WebUtility.HtmlEncode(input);
        }

        // Unescape HTML entities in a string
        //string unescapedHtml = StringUtilities.UnescapeHtml(escapedHtml);
        //Console.WriteLine($"Unescaped HTML: {unescapedHtml}");

        public static string UnescapeHtml(string input)
        {
            if (input == null) return null;
            return System.Net.WebUtility.HtmlDecode(input);
        }
    }

}