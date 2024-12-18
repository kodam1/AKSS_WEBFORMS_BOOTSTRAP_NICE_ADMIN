using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AKSS_Management
{
    public partial class EmpList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindOnFirstPageLoad();
            }
        }

        public void BindOnFirstPageLoad()
        {            
            BindEmpDemo();
        }

        public async void BindEmpDemo()
        {
            string spname = "CRUD_EmpDemo";   
            
            var parameters = new[]
            {
                 CommonUtilities.CreateParameter("@CRUD_Action", "GET_ALL", SqlDbType.NVarChar)
            };           
            
            DataTable dt = await CommonUtilities.GetDataTableStoredProcedureAsync(spname, parameters);            

            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }

        protected void btnAddEmp_Click(object sender, EventArgs e)
        {
            Response.Redirect("/EmpDemo.aspx");
        }

        //protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        //{
        //    int index = Convert.ToInt32(e.NewSelectedIndex);
        //    txtEmpId.Text = GridView1.Rows[index].Cells[0].Text;
        //    txtName.Text = GridView1.Rows[index].Cells[1].Text;
        //    txtRole.Text = GridView1.Rows[index].Cells[2].Text;
        //    txtSal.Text = GridView1.Rows[index].Cells[3].Text;            
        //}

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridViewRow row = GridView1.SelectedRow;
            //lblMessage.Text = "Selected" + row.Cells[2].Text + ".";

            //Accessing BoundField Column.

            //txtEmpId.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
            //txtName.Text = GridView1.SelectedRow.Cells[1].Text.Trim();
            //txtRole.Text = GridView1.SelectedRow.Cells[2].Text.Trim();
            //txtSal.Text = GridView1.SelectedRow.Cells[3].Text.Trim();

            int id = Convert.ToInt32(GridView1.SelectedRow.Cells[0].Text.Trim());

            Response.Redirect("/EmpDemo.aspx?id="+id);

            GridViewRow selectedRow = GridView1.SelectedRow;
            if (selectedRow != null)
            {
                lblMessage.Text = "Selected Employee: " + selectedRow.Cells[0].Text + ", " + selectedRow.Cells[1].Text;
            }

            //Accessing TemplateField Column controls.
            //string country = (GridView1.SelectedRow.FindControl("lblCountry") as Label).Text;

            //lblValues.Text = "<b>Name:</b> " + name + " <b>Country:</b> " + country;

            //CommonUtilities.GenerateRandomPassword(;
        }
        //protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
        //        e.Row.Attributes["style"] = "cursor:pointer";
                
        //    }
        //}
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindEmpDemo();
        }

        protected async void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {            
            if (e.CommandName == "Edit")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int id = Convert.ToInt32(GridView1.DataKeys[rowIndex].Value);

                Response.Redirect("/EmpDemo.aspx?id=" + id);

                ////Determine the RowIndex of the Row whose Button was clicked.
                //int rowIndex = Convert.ToInt32(e.CommandArgument);

                ////Reference the GridView Row.
                //GridViewRow row = GridView1.Rows[rowIndex];

                ////Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                ////Fetch value of Country
                //string country = row.Cells[1].Text;

            }
            else if (e.CommandName == "Delete")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int id = Convert.ToInt32(GridView1.DataKeys[rowIndex].Value);

                // Delete the row from the database
                // Your deletion logic here

                string spname = "CRUD_EmpDemo";
                // int timeout = 60; // 60 seconds
                var parameters = new[]
                {
                    CommonUtilities.CreateParameter("@CRUD_Action", "DELETE_BY_ID", SqlDbType.NVarChar),
                    CommonUtilities.CreateParameter("@id", Convert.ToInt64(id), SqlDbType.Int)
                };
                int i = await CommonUtilities.ExecuteNonQueryStoredProcedureAsync(spname, parameters);
                //Console.WriteLine($"Orders retrieved: {orders.Rows.Count}");
                if (i > 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "delete", "alert('Record Deleted Successfully !');", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "save", "alert('Record Not Deleted!');", true);
                }                
                BindOnFirstPageLoad();                
            }              
        }

        //for colour grid view
        //protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    // Check if the row is a data row
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        // Example: Change the background color of the row
        //        e.Row.BackColor = System.Drawing.Color.LightGray;

        //        // Example: Add a tooltip to the row
        //        e.Row.ToolTip = "This is row number " + e.Row.RowIndex.ToString();
        //    }
        //}



        //protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        //{
        //    int index = Convert.ToInt32(e.NewSelectedIndex);
        //    txtcustname.Text = GridView1.Rows[index].Cells[3].Text;
        //    txtcustaddr.Text = GridView1.Rows[index].Cells[4].Text;
        //    txtcustcountry.Text = GridView1.Rows[index].Cells[5].Text;
        //    txtcustcity.Text = GridView1.Rows[index].Cells[6].Text;
        //    txtcustincode.Text = GridView1.Rows[index].Cells[7].Text;
        //    HiddenField1.Value = GridView1.Rows[index].Cells[2].Text;

        //    btnaddcustomer.Text = "Update Customer";
        //}
        //protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    int index = Convert.ToInt32(e.RowIndex);

        //    int custid = Convert.ToInt16(GridView1.Rows[index].Cells[2].Text);

        //    objLogic.DeleteCustomer(custid);
        //}
        //protected void SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //Reference the DropDownList.
        //    DropDownList dropDownList = sender as DropDownList;

        //    //Get the ID of the DropDownList.
        //    string id = dropDownList.ID;

        //    //Display the Selected Text of DropDownList.
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + dropDownList.SelectedItem.Text + "');", true);
        //}

    }
}