using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;
using System.Configuration;


namespace AKSS_Management
{

    public static class CommonUtilities
    {
        /* Execute Non Query StoredProcedure all crud and read data operations dt,ds,scalar  */

        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;


        /// </summary>
        public static SqlParameter CreateParameter(string name, object value, SqlDbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            return new SqlParameter
            {
                ParameterName = name,
                Value = value ?? DBNull.Value,
                SqlDbType = dbType,
                Direction = direction
            };
        }

        // Database: Execute Non-Query using Stored Procedure
        //        string storedProcedureName = "sp_InsertEmployee";
        //        SqlParameter[] parameters = {
        //    new SqlParameter("@Name", "John Doe"),
        //    new SqlParameter("@Position", "Developer")
        //};

        //        int rowsAffected = CommonUtilities.ExecuteNonQueryStoredProcedure(storedProcedureName, parameters);
        //        Console.WriteLine($"Rows affected: {rowsAffected}");

        public static async Task<int> ExecuteNonQueryStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Database: Execute Scalar using Stored Procedure
        //string scalarStoredProcedure = "sp_GetEmployeeCount";
        //int employeeCount = (int)CommonUtilities.ExecuteScalarStoredProcedure(scalarStoredProcedure);
        //Console.WriteLine($"Number of employees: {employeeCount}");

        public static object ExecuteScalarStoredProcedure(string storedProcedureName, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                connection.Open();
                return command.ExecuteScalar();
            }
        }

        // Database: Fill DataTable using Stored Procedure
        //        string selectStoredProcedure = "sp_GetAllEmployees";
        //        DataTable employees = CommonUtilities.GetDataTableStoredProcedure(selectStoredProcedure);
        //foreach (DataRow row in employees.Rows)
        //{
        //    Console.WriteLine($"ID: {row["ID"]}, Name: {row["Name"]}, Position: {row["Position"]}");
        //}

        public static async Task<DataTable> GetDataTableStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        DataTable dataTable = new DataTable();

                        await connection.OpenAsync();
                        await Task.Run(() => adapter.Fill(dataTable));
                        
                        return dataTable;
                    }
                }
            }
        }

        // Database: Execute Reader using Stored Procedure
        //        string readerStoredProcedure = "sp_GetEmployeeDetails";
        //        SqlParameter[] readerParameters = {
        //    new SqlParameter("@EmployeeID", 1)
        //};

        //using (SqlDataReader reader = CommonUtilities.ExecuteReaderStoredProcedure(readerStoredProcedure, readerParameters))
        //{
        //    while (reader.Read())
        //    {
        //        Console.WriteLine($"ID: {reader["ID"]}, Name: {reader["Name"]}, Position: {reader["Position"]}");
        //    }
        //}

        public static SqlDataReader ExecuteReaderStoredProcedure(string storedProcedureName, SqlParameter[] parameters = null)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(storedProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /* Execute Non Query StoredProcedure all crud and read data operations dt,ds,scalar  */

        // Session Management: Set Session Value
        public static void SetSessionValue(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        // Session Management: Get Session Value
        public static object GetSessionValue(string key)
        {
            return HttpContext.Current.Session[key];
        }

        // Form Handling: Clear TextBoxes
        // Assuming 'this' is a Page object
        //CommonUtilities.ClearTextBoxes(this);

        public static void ClearTextBoxes(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                else if (ctrl.HasControls())
                    ClearTextBoxes(ctrl);
            }
        }

        // Form Handling: Populate DropDownList
        //string ddlQuery = "SELECT ID, Name FROM Departments";
        //DataTable departments = CommonUtilities.GetDataTable(ddlQuery);
        //CommonUtilities.PopulateDropDownList(ddlDepartments, departments, "Name", "ID");

        public static void PopulateDropDownList(DropDownList ddl, DataTable dataTable, string textField, string valueField)
        {
            ddl.DataSource = dataTable;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
        }

        // Logging: Log Errors to a File
        //        try
        //{
        //    // Some code that may throw an exception
        //}
        //catch (Exception ex)
        //{
        //    CommonUtilities.LogError(ex, "error_log.txt");
        //}

        public static void LogError(Exception ex, string filePath)
        {
            string errorMessage = $"[{DateTime.Now}] Error: {ex.Message}\nStack Trace: {ex.StackTrace}\n";
            File.AppendAllText(filePath, errorMessage);
        }

        // Logging: Log Messages to a File

        //CommonUtilities.LogMessage("Application started", "app_log.txt");

        public static void LogMessage(string message, string filePath)
        {
            string logMessage = $"[{DateTime.Now}] Message: {message}\n";
            File.AppendAllText(filePath, logMessage);
        }

        // Form Handling: Populate GridView
        //string selectQuery = "SELECT * FROM Employees";
        //DataTable employees = CommonUtilities.GetDataTable(selectQuery);
        //CommonUtilities.PopulateGridView(gridViewEmployees, employees);

        public static void PopulateGridView(GridView gridView, DataTable dataTable)
        {
            gridView.DataSource = dataTable;
            gridView.DataBind();
        }

        // Utility: Format Date
        //DateTime now = DateTime.Now;
        //string formattedDate = CommonUtilities.FormatDate(now, "dd/MM/yyyy");
        //Console.WriteLine($"Formatted Date: {formattedDate}");

        public static string FormatDate(DateTime date, string format = "yyyy-MM-dd")
        {
            return date.ToString(format);
        }

        // Utility: Generate Random Password
        //string randomPassword = CommonUtilities.GenerateRandomPassword(16);
        //Console.WriteLine($"Random Password: {randomPassword}");

        public static string GenerateRandomPassword(int length = 12)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }
            return new string(password);
        }

        // Generate a random string
        //var helper = new CommonLogicHelper();
        //string randomString = helper.GenerateRandomString(10);
        //Console.WriteLine($"Random String: {randomString}");

        public static string GenerateRandomString1(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }


        // Security: Hash Password (Simple SHA256)
        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var builder = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Security: Validate Password (Simple SHA256)
        //string password = "MySecurePassword";
        //string hashedPassword = CommonUtilities.HashPassword(password);
        //bool isValid = CommonUtilities.ValidatePassword("MySecurePassword", hashedPassword);
        //Console.WriteLine($"Is Valid Password: {isValid}");

        public static bool ValidatePassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }


        /* Send Email & Send Sms  */


        // Email: Send Email
        //        CommonUtilities.SendEmail("recipient@example.com", "Test Subject", "<p>This is a test email.</p>");
        //Console.WriteLine("Email sent successfully.");

        public static void SendEmail(string to, string subject, string body, string from = "noreply@yourdomain.com")
        {
            MailMessage mail = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            SmtpClient client = new SmtpClient("smtp.yourdomain.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("your_username", "your_password"),
                EnableSsl = true
            };
            client.Send(mail);
        }

        /// <summary>
        /// Sends an email using SMTP.
        /// </summary>
        /// helper.SendEmail(
        //        smtpServer: "smtp.gmail.com",
        //    port: 587,
        //    fromEmail: "yourEmail@gmail.com",
        //    toEmail: "recipientEmail@gmail.com",
        //    subject: "Test Email",
        //    body: "This is a test email.",
        //    username: "yourEmail@gmail.com",
        //    password: "yourEmailPassword"
        //);

        public static void SendEmail1(string smtpServer, int port, string fromEmail, string toEmail, string subject, string body, string username, string password)
        {
            using (var client = new SmtpClient(smtpServer, port))
            {
                client.Credentials = new System.Net.NetworkCredential(username, password);
                client.EnableSsl = true;

                var mailMessage = new MailMessage(fromEmail, toEmail, subject, body);
                client.Send(mailMessage);
            }
        }

        /// <summary>
        /// Sends an SMS using an external API.
        /// </summary>
        /// string apiUrl = "https://sms-provider.com/api/send";
        //string apiKey = "yourApiKey";
        //string toPhoneNumber = "1234567890";
        //string message = "This is a test SMS.";

        //bool isSmsSent = await helper.SendSmsAsync(apiUrl, apiKey, toPhoneNumber, message);
        //Console.WriteLine($"SMS Sent: {isSmsSent}");

        public static async Task<bool> SendSmsAsync(string apiUrl, string apiKey, string toPhoneNumber, string message)
        {
            using (var client = new HttpClient())
            {
                var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "apikey", apiKey },
                { "to", toPhoneNumber },
                { "message", message }
            });

                HttpResponseMessage response = await client.PostAsync(apiUrl, requestContent);
                return response.IsSuccessStatusCode;
            }
        }

        /* File opeations  */

        // File Handling: Upload File
        //string uploadedFilePath = CommonUtilities.UploadFile(Request.Files[0], Server.MapPath("~/Uploads"));
        //Console.WriteLine($"File uploaded to: {uploadedFilePath}");

        public static string UploadFile(HttpPostedFile file, string uploadPath)
        {
            if (file == null || file.ContentLength == 0)
                throw new ArgumentException("Invalid file");

            string filePath = Path.Combine(uploadPath, Path.GetFileName(file.FileName));
            file.SaveAs(filePath);
            return filePath;
        }

        // Caching: Add Item to Cache
        //        CommonUtilities.AddToCache("CacheKey", "CacheValue", DateTime.Now.AddHours(1));
        //Console.WriteLine("Item added to cache.");

        public static void AddToCache(string key, object value, DateTime absoluteExpiration)
        {
            HttpContext.Current.Cache.Insert(key, value, null, absoluteExpiration, Cache.NoSlidingExpiration);
        }

        // Caching: Get Item from Cache
        //object cachedValue = CommonUtilities.GetFromCache("CacheKey");
        //Console.WriteLine($"Cached Value: {cachedValue}");

        public static object GetFromCache(string key)
        {
            return HttpContext.Current.Cache[key];
        }

        // Caching: Remove Item from Cache
        //        CommonUtilities.RemoveFromCache("CacheKey");
        //Console.WriteLine("Item removed from cache.");

        public static void RemoveFromCache(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        // Image Processing: Resize Image
        public static void ResizeImage(string inputPath, string outputPath, int width, int height)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(inputPath))
            using (Bitmap bitmap = new Bitmap(image, new Size(width, height)))
            {
                bitmap.Save(outputPath, ImageFormat.Jpeg);
            }
        }

        // Image Processing: Convert Image Format
        public static void ConvertImageFormat(string inputPath, string outputPath, ImageFormat format)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(inputPath))
            {
                image.Save(outputPath, format);
            }
        }

        // Encryption: Encrypt Text
        public static string EncryptText(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] keyBytes = Convert.FromBase64String(key);
                aes.Key = keyBytes;
                aes.IV = new byte[16]; // Zero IV for simplicity

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                    sw.Close();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        // Encryption: Decrypt Text
        public static string DecryptText(string cipherText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] keyBytes = Convert.FromBase64String(key);
                aes.Key = keyBytes;
                aes.IV = new byte[16]; // Zero IV for simplicity

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Hashes a string using SHA256.
        /// </summary>
        /// string hashedValue = helper.HashString("myPassword123");
        //Console.WriteLine($"Hashed Value: {hashedValue}");

        public static string HashString(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Validates an email address format.
        /// </summary>
        /// bool isValidEmail = helper.ValidateEmail("example@domain.com");
        //Console.WriteLine($"Is Valid Email: {isValidEmail}");

        public static bool ValidateEmail(string email)
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

        /// <summary>
        /// Writes a log message to a specified log file.
        /// </summary>
        /// helper.WriteLog("This is a log message.", "log.txt");

        public static void WriteLog(string message, string logFilePath)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"[{DateTime.Now}] {message}");
            }
        }

        /// <summary>
        /// Reads all lines from a specified file.
        /// </summary>
        /// string[] fileContents = helper.ReadFile("log.txt");
        //foreach (var line in fileContents)
        //{
        //    Console.WriteLine(line);
        //}

        public static string[] ReadFile(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        /// <summary>
        /// Encodes a string for use in a URL.
        /// </summary>
        /// string encodedUrl = helper.UrlEncode("https://example.com?query=value");
        //Console.WriteLine($"Encoded URL: {encodedUrl}");


        public static string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        /// <summary>
        /// Decodes a URL-encoded string.
        /// </summary>
        ///  //string decodedUrl = helper.UrlDecode(encodedUrl);
        //        Console.WriteLine($"Decoded URL: {decodedUrl}");
        public static string UrlDecode(string value)
        {
            return HttpUtility.UrlDecode(value);
        }

        /// <summary>
        /// Decodes HTML-encoded characters in a string.
        /// </summary>
        /// 
        //        string htmlEncoded = helper.HtmlEncode("<div>Hello</div>");
        //        Console.WriteLine($"HTML Encoded: {htmlEncoded}");

        public static string HtmlEncode(string value)
        {
            return HttpUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Decodes HTML-encoded characters in a string.
        /// </summary>
        //string htmlDecoded = helper.HtmlDecode(htmlEncoded);
        //        Console.WriteLine($"HTML Decoded: {htmlDecoded}");

        public static string HtmlDecode(string value)
        {
            return HttpUtility.HtmlDecode(value);
        }

        /// <summary>
        /// Removes all non-alphanumeric characters from a string.
        /// </summary>
        /// string cleanString = helper.RemoveSpecialCharacters("Hello@World!123");
        //Console.WriteLine($"Clean String: {cleanString}");

        public static string RemoveSpecialCharacters(string input)
        {
            return Regex.Replace(input, "[^a-zA-Z0-9]", "");
        }

        /// <summary>
        /// Calculates the difference in days between two dates.
        /// </summary>
        /// int daysDifference = helper.CalculateDaysDifference(DateTime.Now, DateTime.Now.AddDays(10));
        //Console.WriteLine($"Days Difference: {daysDifference}");

        public static int CalculateDaysDifference(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days;
        }

        /// <summary>
        /// Checks if a given string is a valid date.
        /// </summary>
        /// 
        //bool isValidDate = helper.IsValidDate("2024-12-31");
        //Console.WriteLine($"Is Valid Date: {isValidDate}");

        public static bool IsValidDate(string input)
        {
            return DateTime.TryParse(input, out _);
        }

        /// <summary>
        /// Generates a GUID string.
        /// </summary>
        /// 13. Generate GUID
        /// string guid = helper.GenerateGuid();
        //Console.WriteLine($"Generated GUID: {guid}");

        public static string GenerateGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Converts a DataTable to a CSV string.
        /// </summary>
        public static string DataTableToCsv(DataTable dataTable)
        {
            var csvBuilder = new StringBuilder();
            foreach (DataColumn column in dataTable.Columns)
            {
                csvBuilder.Append(column.ColumnName + ",");
            }
            csvBuilder.AppendLine();

            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    csvBuilder.Append(item.ToString() + ",");
                }
                csvBuilder.AppendLine();
            }
            return csvBuilder.ToString();
        }

        /// <summary>
        /// Parses a CSV string into a DataTable.
        /// </summary>
        public static DataTable CsvToDataTable(string csvContent)
        {
            var dataTable = new DataTable();
            var rows = csvContent.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var headers = rows[0].Split(',');

            foreach (var header in headers)
            {
                dataTable.Columns.Add(header);
            }

            for (int i = 1; i < rows.Length; i++)
            {
                var values = rows[i].Split(',');
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        /// <summary>
        /// Converts a string to Title Case.
        /// </summary>
        public static string ToTitleCase(string input)
        {
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        /// <summary>
        /// Removes duplicate characters from a string.
        /// </summary>
        public static string RemoveDuplicateCharacters(string input)
        {
            return new string(input.ToCharArray().Distinct().ToArray());
        }

        /// <summary>
        /// Retrieves the file extension from a file path.
        /// </summary>
        /// string extension = helper.GetFileExtension("document.pdf");
        //Console.WriteLine($"File Extension: {extension}");

        public static string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        /// Reads a JSON file and deserializes it into an object of type T.
        /// </summary>
        /// 
        //12. Read and Write JSON Files
        //        var data = new { Name = "John", Age = 30 };
        //        helper.WriteJsonFile("data.json", data);



        public static T ReadJsonFile<T>(string filePath)
        {
            var jsonContent = File.ReadAllText(filePath);
            return System.Text.Json.JsonSerializer.Deserialize<T>(jsonContent);
        }

        /// <summary>
        /// Writes an object to a file as JSON.
        /// </summary>
        ///   var readData = helper.ReadJsonFile<dynamic>("data.json");
        //    Console.WriteLine($"Name: {readData.Name}, Age: {readData.Age}");

        public static void WriteJsonFile<T>(string filePath, T obj)
        {
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(obj);
            File.WriteAllText(filePath, jsonContent);
        }

        /* Calculations of Logical Math and */

        // Calculate age from date of birth
        //DateTime dob = new DateTime(1990, 1, 1);
        //int age = CalculationLibrary.CalculateAge(dob);
        //Console.WriteLine($"Age: {age}");

        public static int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }

        // Get date of birth from age
        //int age = 30;
        //DateTime dob = CalculationLibrary.GetDateOfBirth(age);
        //Console.WriteLine($"Date of Birth: {dob.ToString("yyyy-MM-dd")}");

        public static DateTime GetDateOfBirth(int age)
        {
            var today = DateTime.Today;
            return today.AddYears(-age);
        }

        // Calculate the number of days between two dates
        //DateTime startDate = new DateTime(2023, 1, 1);
        //DateTime endDate = new DateTime(2023, 12, 31);
        //int daysBetween = CalculationLibrary.CalculateDaysBetweenDates(startDate, endDate);
        //Console.WriteLine($"Days Between: {daysBetween}");

        public static int CalculateDaysBetweenDates(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days;
        }

        // Check if a given year is a leap year
        //int year = 2024;
        //bool isLeapYear = CalculationLibrary.IsLeapYear(year);
        //Console.WriteLine($"Is Leap Year: {isLeapYear}");

        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        // Get the number of days in a month for a given year
        //int year = 2023;
        //int month = 2;
        //int daysInMonth = CalculationLibrary.GetDaysInMonth(year, month);
        //Console.WriteLine($"Days in Month: {daysInMonth}");

        public static int GetDaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        // Calculate the number of workdays between two dates
        //DateTime startDate = new DateTime(2023, 1, 1);
        //DateTime endDate = new DateTime(2023, 1, 31);
        //int workdays = CalculationLibrary.CalculateWorkdays(startDate, endDate);
        //Console.WriteLine($"Workdays: {workdays}");

        public static int CalculateWorkdays(DateTime startDate, DateTime endDate)
        {
            int totalDays = CalculateDaysBetweenDates(startDate, endDate);
            int workDays = 0;

            for (int i = 0; i <= totalDays; i++)
            {
                DateTime currentDate = startDate.AddDays(i);
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    workDays++;
                }
            }

            return workDays;
        }

        // Get the next occurrence of a specific weekday from a given date
        //DateTime startDate = DateTime.Today;
        //DayOfWeek day = DayOfWeek.Friday;
        //DateTime nextFriday = CalculationLibrary.GetNextWeekday(startDate, day);
        //Console.WriteLine($"Next Friday: {nextFriday.ToString("yyyy-MM-dd")}");

        public static DateTime GetNextWeekday(DateTime startDate, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)startDate.DayOfWeek + 7) % 7;
            return startDate.AddDays(daysToAdd == 0 ? 7 : daysToAdd);
        }

        // Add a specified number of business days to a given date
        //DateTime startDate = DateTime.Today;
        //int businessDaysToAdd = 10;
        //DateTime resultDate = CalculationLibrary.AddBusinessDays(startDate, businessDaysToAdd);
        //Console.WriteLine($"Date after adding business days: {resultDate.ToString("yyyy-MM-dd")}");

        public static DateTime AddBusinessDays(DateTime startDate, int numberOfDays)
        {
            int addedDays = 0;
            DateTime currentDate = startDate;

            while (addedDays < numberOfDays)
            {
                currentDate = currentDate.AddDays(1);
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    addedDays++;
                }
            }

            return currentDate;
        }

        // Calculate compound interest
        //double principal = 1000;
        //double rate = 0.05; // 5% interest rate
        //int timesCompounded = 4; // Quarterly
        //int years = 10;
        //double amount = CalculationLibrary.CalculateCompoundInterest(principal, rate, timesCompounded, years);
        //Console.WriteLine($"Compound Interest: {amount}");

        public static double CalculateCompoundInterest(double principal, double rate, int timesCompounded, int years)
        {
            return principal * Math.Pow((1 + rate / timesCompounded), timesCompounded * years);
        }

        // Calculate the number of months between two dates
        //DateTime startDate = new DateTime(2022, 1, 1);
        //DateTime endDate = new DateTime(2023, 6, 1);
        //int monthsBetween = CalculationLibrary.CalculateMonthsBetweenDates(startDate, endDate);
        //Console.WriteLine($"Months Between: {monthsBetween}");

        public static int CalculateMonthsBetweenDates(DateTime startDate, DateTime endDate)
        {
            return (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;
        }

        // Calculate the difference in hours between two dates
        public static double CalculateHoursBetweenDates(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).TotalHours;
        }

        // Calculate the difference in minutes between two dates
        //DateTime startDate = DateTime.Now;
        //DateTime endDate = startDate.AddHours(5);
        //double hoursBetween = CalculationLibrary.CalculateHoursBetweenDates(startDate, endDate);
        //Console.WriteLine($"Hours Between: {hoursBetween}");

        public static double CalculateMinutesBetweenDates(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).TotalMinutes;
        }

        // Calculate the difference in seconds between two dates
        public static double CalculateSecondsBetweenDates(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).TotalSeconds;
        }

        // Calculate the future value of an investment
        //double presentValue = 1000;
        //double rate = 0.05; // 5% interest rate
        //int periods = 10; // 10 periods
        //double futureValue = CalculationLibrary.CalculateFutureValue(presentValue, rate, periods);
        //Console.WriteLine($"Future Value: {futureValue}");

        public static double CalculateFutureValue(double presentValue, double rate, int periods)
        {
            return presentValue * Math.Pow(1 + rate, periods);
        }

        // Calculate the present value of a future amount
        public static double CalculatePresentValue(double futureValue, double rate, int periods)
        {
            return futureValue / Math.Pow(1 + rate, periods);
        }

        // Calculate the monthly payment for a loan
        //double loanPrincipal = 20000;
        //double annualRate = 0.05; // 5% annual interest rate
        //int totalPayments = 60; // 5 years
        //double monthlyPayment = CalculationLibrary.CalculateMonthlyLoanPayment(loanPrincipal, annualRate, totalPayments);
        //Console.WriteLine($"Monthly Payment: {monthlyPayment}");

        public static double CalculateMonthlyLoanPayment(double principal, double annualRate, int totalPayments)
        {
            double monthlyRate = annualRate / 12;
            return (principal * monthlyRate) / (1 - Math.Pow(1 + monthlyRate, -totalPayments));
        }

        // Calculate the total interest paid on a loan
        //double loanPrincipal = 20000;
        //double annualRate = 0.05; // 5% annual interest rate
        //int totalPayments = 60; // 5 years
        //double totalInterestPaid = CalculationLibrary.CalculateTotalInterestPaid(loanPrincipal, annualRate, totalPayments);
        //Console.WriteLine($"Total Interest Paid: {totalInterestPaid}");

        public static double CalculateTotalInterestPaid(double principal, double annualRate, int totalPayments)
        {
            double monthlyPayment = CalculateMonthlyLoanPayment(principal, annualRate, totalPayments);
            return (monthlyPayment * totalPayments) - principal;
        }

        // Calculate the time until a specific date
        //DateTime futureDate = new DateTime(2024, 12, 25);
        //TimeSpan timeUntil = CalculationLibrary.CalculateTimeUntil(futureDate);
        //Console.WriteLine($"Time Until: {timeUntil.Days} days, {timeUntil.Hours} hours, {timeUntil.Minutes} minutes");

        public static TimeSpan CalculateTimeUntil(DateTime futureDate)
        {
            return futureDate - DateTime.Now;
        }


        // Calculate the Body Mass Index (BMI)
        //double weightKg = 70;
        //double heightM = 1.75;
        //double bmi = CalculationLibrary.CalculateBMI(weightKg, heightM);
        //Console.WriteLine($"BMI: {bmi}");

        public static double CalculateBMI(double weightKg, double heightM)
        {
            return weightKg / (heightM * heightM);
        }

        // Calculate the distance between two points (Haversine formula)
        //double lat1 = 40.7128;
        //double lon1 = -74.0060;
        //double lat2 = 34.0522;
        //double lon2 = -118.2437;
        //double distance = CalculationLibrary.CalculateDistance(lat1, lon1, lat2, lon2);
        //Console.WriteLine($"Distance: {distance} km");

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of the Earth in kilometers
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        // Convert degrees to radians
        public static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        // Calculate the area of a triangle using Heron's formula
        //double a = 3;
        //double b = 4;
        //double c = 5;
        //double area = CalculationLibrary.CalculateTriangleArea(a, b, c);
        //Console.WriteLine($"Triangle Area: {area}");

        public static double CalculateTriangleArea(double a, double b, double c)
        {
            double s = (a + b + c) / 2;
            return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }

        // Calculate the Fibonacci sequence up to the Nth term
        //int n = 10;
        //int[] fibonacci = CalculationLibrary.CalculateFibonacci(n);
        //Console.WriteLine($"Fibonacci sequence: {string.Join(", ", fibonacci)}");

        public static int[] CalculateFibonacci(int n)
        {
            int[] fibonacci = new int[n];
            if (n > 0) fibonacci[0] = 0;
            if (n > 1) fibonacci[1] = 1;

            for (int i = 2; i < n; i++)
            {
                fibonacci[i] = fibonacci[i - 1] + fibonacci[i - 2];
            }

            return fibonacci;
        }

        // Check if a number is prime
        //int number = 17;
        //bool isPrime = CalculationLibrary.IsPrime(number);
        //Console.WriteLine($"Is Prime: {isPrime}"); // Output: Is Prime: True

        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            for (int i = 3; i <= Math.Sqrt(number); i += 2)
            {
                if (number % i == 0) return false;
            }

            return true;
        }

        // Calculate the greatest common divisor (GCD) of two numbers using Euclid's algorithm
        //int a = 48;
        //int b = 18;
        //int gcd = CalculationLibrary.CalculateGCD(a, b);
        //Console.WriteLine($"GCD: {gcd}"); // Output: GCD: 6

        public static int CalculateGCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        // Calculate the least common multiple (LCM) of two numbers
        //int a = 12;
        //int b = 15;
        //int lcm = CalculationLibrary.CalculateLCM(a, b);
        //Console.WriteLine($"LCM: {lcm}"); // Output: LCM: 60

        public static int CalculateLCM(int a, int b)
        {
            return Math.Abs(a * b) / CalculateGCD(a, b);
        }

        // Calculate the square root of a number using Newton's method
        //double number = 25;
        //double sqrt = CalculationLibrary.CalculateSquareRoot(number);
        //Console.WriteLine($"Square Root: {sqrt}"); // Output: Square Root: 5

        public static double CalculateSquareRoot(double number)
        {
            if (number < 0) throw new ArgumentOutOfRangeException(nameof(number), "Cannot calculate square root of a negative number.");
            double epsilon = 1e-10; // Precision level
            double guess = number / 2.0;

            while (Math.Abs(guess * guess - number) >= epsilon)
            {
                guess = (guess + number / guess) / 2.0;
            }

            return guess;
        }

        // Calculate the power of a number (x^y)
        //double x = 2;
        //double y = 3;
        //double power = CalculationLibrary.CalculatePower(x, y);
        //Console.WriteLine($"Power: {power}"); // Output: Power: 8

        public static double CalculatePower(double x, double y)
        {
            return Math.Pow(x, y);
        }

        // Calculate the natural logarithm of a number
        public static double CalculateLog(double number)
        {
            return Math.Log(number);
        }

        // Calculate the logarithm of a number to a specified base
        //double number = 100;
        //double baseValue = 10;
        //double logBase = CalculationLibrary.CalculateLogBase(number, baseValue);
        //Console.WriteLine($"Logarithm Base 10: {logBase}"); // Output: Logarithm Base 10: 2

        public static double CalculateLogBase(double number, double baseValue)
        {
            return Math.Log(number, baseValue);
        }

        // Convert radians to degrees
        //double radians = Math.PI;
        //double degrees = CalculationLibrary.ToDegrees(radians);
        //Console.WriteLine($"Degrees: {degrees}"); // Output: Degrees: 180

        public static double ToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        // Calculate the sum of an array of integers
        //int[] numbers = { 1, 2, 3, 4, 5 };
        //int sum = CalculationLibrary.CalculateSum(numbers);
        //Console.WriteLine($"Sum: {sum}"); // Output: Sum: 15
        public static int CalculateSum(int[] numbers)
        {
            int sum = 0;
            foreach (int number in numbers)
            {
                sum += number;
            }
            return sum;
        }

    }
}