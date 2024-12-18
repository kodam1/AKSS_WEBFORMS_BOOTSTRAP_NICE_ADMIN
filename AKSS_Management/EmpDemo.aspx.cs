using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;

namespace AKSS_Management
{
    public partial class EmpDemo : System.Web.UI.Page
    {
        Helper_SP dbHelper = new Helper_SP();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                if (Request.QueryString.HasKeys() && Request.QueryString["id"].ToString() != null)
                {
                    int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                  
                    BindEmpDetailsById(id);
                                    
                }
                else
                {
                    BindOnFirstPageLoad();
                }
            }
        }

        public void BindOnFirstPageLoad()
        {
            GetEmpMaxId();
            //BindEmpDemo();
        }
        public async void BindEmpDetailsById(int id)
        {
            string spname = "CRUD_EmpDemo";
            // int timeout = 60; // 60 seconds
            var parameters = new[]
            {
                CommonUtilities.CreateParameter("@CRUD_Action", "GET_BY_ID", SqlDbType.NVarChar),
                CommonUtilities.CreateParameter("@id", id, SqlDbType.Int)
            };

            DataTable dt = await CommonUtilities.GetDataTableStoredProcedureAsync(spname, parameters); //dbHelper.ExecuteStoredProcedure(spname, parameters);
            //Console.WriteLine($"Orders retrieved: {orders.Rows.Count}");
            if (dt.Rows.Count > 0)
            {
                txtEmpId.Text = dt.Rows[0]["id"].ToString();
                txtName.Text = dt.Rows[0]["name"].ToString();
                txtRole.Text = dt.Rows[0]["role"].ToString();
                txtSal.Text = dt.Rows[0]["salary"].ToString();
            }



        }


        public async void GetEmpMaxId()
        {
            string spname = "CRUD_EmpDemo";
           // int timeout = 60; // 60 seconds
            var parameters = new[]
            {
                CommonUtilities.CreateParameter("@CRUD_Action", "GetMaxId", SqlDbType.NVarChar)               
            };
            
             DataTable dt = await CommonUtilities.GetDataTableStoredProcedureAsync(spname, parameters); //dbHelper.ExecuteStoredProcedure(spname, parameters);
            //Console.WriteLine($"Orders retrieved: {orders.Rows.Count}");
            if (dt.Rows.Count > 0)
            {
                txtEmpId.Text = dt.Rows[0]["id"].ToString();            
            }
        }
     
        protected async void btnSubmit_OnClick(object sender, EventArgs e)
        {
            string spname = "CRUD_EmpDemo";
            // int timeout = 60; // 60 seconds
            var parameters = new[]
            {
                dbHelper.CreateParameter("@CRUD_Action", "CREATE", SqlDbType.NVarChar),
                dbHelper.CreateParameter("@name", txtName.Text.Trim(),SqlDbType.NVarChar),
                dbHelper.CreateParameter("@role", txtRole.Text.Trim(), SqlDbType.NVarChar),
                dbHelper.CreateParameter("@salary", Convert.ToInt64(txtSal.Text.Trim()), SqlDbType.Int)
            };
            int i  = await CommonUtilities.ExecuteNonQueryStoredProcedureAsync(spname, parameters);
            //Console.WriteLine($"Orders retrieved: {orders.Rows.Count}");
            if (i > 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(),"save","alert('Record Saved Successfully !');",true);
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "save", "alert('Record Not Save!');", true);
            }

            ClearAllTextBox();
            BindOnFirstPageLoad();

        }

        protected async void btnUpdate_OnClick(object sender, EventArgs e)
        {
            string spname = "CRUD_EmpDemo";
            // int timeout = 60; // 60 seconds
            var parameters = new[]
            {
                dbHelper.CreateParameter("@CRUD_Action", "UPDATE", SqlDbType.NVarChar),
                dbHelper.CreateParameter("@name", txtName.Text.Trim(),SqlDbType.NVarChar),
                dbHelper.CreateParameter("@role", txtRole.Text.Trim(), SqlDbType.NVarChar),
                dbHelper.CreateParameter("@salary", Convert.ToInt64(txtSal.Text.Trim()), SqlDbType.Int),
                dbHelper.CreateParameter("@id", Convert.ToInt64(txtEmpId.Text.Trim()), SqlDbType.Int)
            };
            int i = await CommonUtilities.ExecuteNonQueryStoredProcedureAsync(spname, parameters);
            //Console.WriteLine($"Orders retrieved: {orders.Rows.Count}");
            if (i > 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "save", "alert('Record Update Successfully !');", true);
                ClearAllTextBox();
                BindOnFirstPageLoad();
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "save", "alert('Record Not Update!');", true);
            }            
        }

        protected async void btnDelete_OnClick(object sender, EventArgs e)
        {

            string spname = "CRUD_EmpDemo";
            // int timeout = 60; // 60 seconds
            var parameters = new[]
            {
                dbHelper.CreateParameter("@CRUD_Action", "DELETE_BY_ID", SqlDbType.NVarChar),              
                dbHelper.CreateParameter("@id", Convert.ToInt64(txtEmpId.Text.Trim()), SqlDbType.Int)
            };
            int i = await CommonUtilities.ExecuteNonQueryStoredProcedureAsync(spname, parameters);
            //Console.WriteLine($"Orders retrieved: {orders.Rows.Count}");
            if (i > 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "delete", "alert('Record Deleted Successfully !');", true);
                ClearAllTextBox();
                BindOnFirstPageLoad();
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "delete", "alert('Record Not Deleted !');", true);
            }
           
        }

        protected void btnGetEmpData_OnClick(object sender, EventArgs e)
        {
           // BindEmpDemo();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ClearAllTextBox();
            BindOnFirstPageLoad();
        }        

        public void ClearAllTextBox()
        {
            txtName.Text = string.Empty;
            txtRole.Text = string.Empty;
            txtSal.Text = string.Empty;

            GetEmpMaxId();
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            //var helper = new  CommonLogicHelper();

            string email = ""; //EmailTextBox.Text;
            string message = ""; //MessageTextBox.Text;

            if (CommonUtilities.ValidateEmail(email))
            {
                CommonUtilities.SendEmail1(
                    smtpServer: "smtp.gmail.com",
                    port: 587,
                    fromEmail: "yourEmail@gmail.com",
                    toEmail: email,
                    subject: "Thank you for contacting us",
                    body: $"Your message: {message}",
                    username: "yourEmail@gmail.com",
                    password: "yourEmailPassword"
                );

                //StatusLabel.Text = "Message sent successfully!";
                //string StatusLabel = "Message sent successfully!";
            }
            else
            {
                //StatusLabel.Text = "Invalid email address.";
               // string StatusLabel = "Invalid email address.";
            }
             
            

        }

        protected async void btnExportToExcel_Click(object sender, EventArgs e)
        {
            string spname = "CRUD_EmpDemo";
            // int timeout = 60; // 60 seconds
            var parameters = new[]
            {
                dbHelper.CreateParameter("@CRUD_Action", "READ_ALL", SqlDbType.NVarChar)
            };
            
            DataTable dt = await CommonUtilities.GetDataTableStoredProcedureAsync(spname, parameters);
         
            if (dt.Rows.Count > 0)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Emp");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=EmpDemo.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            else
            {
                // data not present
            }
        }

        /*
                 
        Here are more advanced features you can implement or utilize in the CommonLogicHelper class:

        1. Enhanced SMS Example
        Use the SendSmsAsync method with a real-world SMS API (e.g., Twilio or another provider). 
        Customize the parameters based on the specific API requirements. Here's an example with Twilio:

        string twilioApiUrl = "https://api.twilio.com/2010-04-01/Accounts/YourAccountSID/Messages.json";
        string apiKey = "YourTwilioApiKey";
        string toPhoneNumber = "+1234567890";
        string message = "This is a test SMS from Twilio.";

        var smsSent = await helper.SendSmsAsync(twilioApiUrl, apiKey, toPhoneNumber, message);
        Console.WriteLine($"SMS Sent Successfully: {smsSent}");

        2. Log Exception Details
        Add detailed exception logging for better debugging:

                try
        {
            helper.WriteLog("Attempting to send email...", "appLog.txt");
            helper.SendEmail(
                "smtp.gmail.com", 
                587, 
                "youremail@gmail.com", 
                "recipient@gmail.com", 
                "Subject", 
                "Body", 
                "username", 
                "password"
            );
        }
        catch (Exception ex)
        {
            helper.WriteLog($"Error: {ex.Message} - {ex.StackTrace}", "appLog.txt");
        }

        3. Utility Functions for File Handling
        Use ReadFile to load configurations:

        string[] configLines = helper.ReadFile("config.txt");
        foreach (var line in configLines)
        {
            Console.WriteLine(line);
        }

        4. URL Encoding in Forms
        Ensure user inputs are safe by encoding them for URLs:

        string userInput = "This is a test!";
        string safeUrlInput = helper.UrlEncode(userInput);
        Console.WriteLine($"Encoded Input: {safeUrlInput}");

        5. Combine Multiple Methods
        You can chain multiple helper methods for tasks. For example:

        Validate an email
        Hash its value for storage
        Log the result


        string email = "test@example.com";
        if (helper.ValidateEmail(email))
        {
            string hashedEmail = helper.HashString(email);
            helper.WriteLog($"Valid email hashed: {hashedEmail}", "appLog.txt");
        }
        else
        {
            helper.WriteLog("Invalid email provided.", "appLog.txt");
        }


        6. Add Database Helper Integration
        Combine the helper with a database utility to store logs or SMS/email data.

        string logMessage = "A test log to store in DB.";
        // Assuming you have a DatabaseHelper from another class
        int rowsAffected = dbHelper.ExecuteNonQuery(
            "INSERT INTO Logs (Message, Date) VALUES (@Message, @Date)",
            new SqlParameter("@Message", logMessage),
            new SqlParameter("@Date", DateTime.Now)
        );
        Console.WriteLine($"Rows Affected: {rowsAffected}");




         
         * */




    }
}