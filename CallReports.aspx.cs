using BWELLRTC.Code;
using DataSetExportExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BWELLCallCenter
{
    public partial class CallReports : System.Web.UI.Page
    {
        ConnectSQL cn = new ConnectSQL();
        ToolsClass tc = new ToolsClass();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                setGVinfo();
                DataBind();
            }

        }
        private void setGVinfo()
        {

            string sql = "select * from TblCallHistory order by CallDateTime ";


            DataTable dt = cn.GetDataSet(sql).Tables[0];
            if (ViewState["SortOrder"] != null && ViewState["OrderDire"] != null)
            {
                dt.DefaultView.Sort = ViewState["SortOrder"].ToString() + "  " + ViewState["OrderDire"].ToString();
            }
            else
            {
                ViewState["SortOrder"] = "CallDateTime";
                ViewState["OrderDire"] = "ASC";
            }
            FillRows(dt);




        }
        private void FillRows(DataTable dt)
        {
            gvinfo.DataSource = dt;
            gvinfo.DataBind();
            return;
            int rowCnt = dt.Rows.Count;
            int pageSize = gvinfo.PageSize;
            if (pageSize > 20) { pageSize = 20; }

            if (rowCnt != pageSize)
            {

                if (dt != null)
                {
                    for (int i = 0; i < pageSize - rowCnt; i++)
                    {
                        int rowIndex = rowCnt + i + 1;
                        DataRow row = dt.NewRow();
                        dt.Rows.Add(row);
                    }
                }
            }
            gvinfo.DataSource = dt;
            gvinfo.DataBind();
        }

        protected void gvinfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvinfo.PageIndex = e.NewPageIndex;
            gvinfo.DataBind();
            setGVinfo();
        }

        protected void gvinfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                if (e.Row.Cells[1].Text != "&nbsp;" || e.Row.Cells[2].Text != "&nbsp;")
                {
                    int id = e.Row.RowIndex + 1 + gvinfo.PageIndex * gvinfo.PageSize;
                    e.Row.Cells[0].Text = id.ToString();
                }


            }
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {

            setGVinfo();
        }

        protected void gvinfo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public string SearchTrue()
        {
            string dd = "true";

            return dd;
        }

        protected void gvinfo_Sorting(object sender, GridViewSortEventArgs e)
        {

            string spage = e.SortExpression;
            string sortExpression = "";
            string direction = "";
            if (ViewState["SortOrder"] == null) { ViewState["SortOrder"] = ""; }
            if (ViewState["OrderDire"] == null) { ViewState["OrderDire"] = ""; }
            if (ViewState["SortOrder"].ToString() == spage)
            {
                if (ViewState["OrderDire"].ToString() == " Desc")
                {
                    ViewState["OrderDire"] = " ASC";
                }
                else
                {
                    ViewState["OrderDire"] = " Desc";
                }
            }
            else
            {
                ViewState["SortOrder"] = e.SortExpression;
            }
            sortExpression = ViewState["SortOrder"].ToString();
            direction = ViewState["OrderDire"].ToString();
            setGridView(sortExpression, direction);

        }

        public void setGridView(string sortExpression, string direction)
        {
            string sql = "select * from TblCallHistory order by CallDateTime ";

            gvinfo.DataBind();
            DataSet ds = cn.GetDataSet(sql);
            ds.Tables[0].DefaultView.Sort = sortExpression + "  " + direction;

            //gvinfo.DataSource = ds.Tables[0].DefaultView;

            //gvinfo.DataBind();
            FillRows(ds.Tables[0]);


        }

        


       

        protected void gvinfo_DataBound(object sender, EventArgs e)
        {

        }

        protected void gvinfo_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TableCellCollection tcc = e.Row.Cells;
                //tcc.Clear();
                e.Row.Cells.Add(new TableCell());
                //e.Row.Cells[0].Attributes.Add("colspan", gvinfo.Columns.Count.ToString());
                e.Row.Cells[0].Attributes.Add("id", "TotalLine");
                //string strSpace = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                getTotalInfo(e.Row);
            }
        }

        private void getTotalInfo(GridViewRow oRow)
        {

            //string sql = "select count(*) as CoachCount,sum(ActiveClients) as ActiveClients, sum(TotalClients) as TotalClients FROM [dbo].[View_CoachingStaff]";
            string sql = "select count(*) as CallCount FROM [dbo].[TblCallHistory]";
            DataTable dt = cn.GetDataSet(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                oRow.Cells[1].Text = "Total";
                oRow.Cells[2].Text = dr["CallCount"].ToString();
                //oRow.Cells[5].Text = dr["TotalClients"].ToString();
                //oRow.Cells[6].Text = dr["ActiveClients"].ToString();
                oRow.Cells[7].Visible = false;
                //oRow.BorderStyle = BorderStyle.None;
            }
        }

        protected void ExportBtn_ServerClick(object sender, EventArgs e)
        {

            clsRoot myExport = new clsRoot();
            string filename = "";
            string strTimeStamp = "";
            myExport.ExportFolder = Request.PhysicalApplicationPath + @"Reports";
            myExport.IsExcel = false;
            strTimeStamp = System.DateTime.Now.ToString("yyyyMMddhhmmss");
            myExport.ZipFileName = strTimeStamp + ".zip";
            string sql = "";
            string strFields = "";
            string strCallFN = "";



            DataSet dt_Call = null;
           

           
                sql = "";
                strFields = "*";
               
                sql = "SELECT * "  + " FROM View_CallHistory WHERE 1=1 order by CallDateTime, CallOperator";


                strCallFN = "CallHistory" + strTimeStamp + ".csv";
                dt_Call= cn.GetDataSet(sql);

           


           

            filename = myExport.CreateExcel(dt_Call, strCallFN);
            if (filename != "" && System.IO.File.Exists(filename))
            {
                //Response.TransmitFile(filename);
                //download the file
                FileStream fs = new FileStream(filename, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.ContentType = "application/octet-stream";

                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(System.IO.Path.GetFileName(filename), System.Text.Encoding.UTF8));
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();

            }
        }
    }
}