using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Com.Xuqingkai
{
	[System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class FrmMain :Form
    {
        
		private System.Windows.Forms.WebBrowser webBrowser = new System.Windows.Forms.WebBrowser();
        public FrmMain()
        {
            InitializeComponent();
        }
		
		/// <summary>
        /// 登录窗口
        /// </summary>
        private void InitializeComponent()
        {
			this.DoubleBuffered = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Name = "frmMain";
			this.Text = "桌面程序";
			this.ClientSize = new System.Drawing.Size(800, 600);
			this.Icon = new System.Drawing.Icon(System.Environment.CurrentDirectory + "\\resources\\logo.ico");
			//this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;//是否显示窗体原始框
			//this.BackColor = System.Drawing.Color.FromArgb(250,191,1);
			
			webBrowser.ObjectForScripting = this;
			webBrowser.Dock = System.Windows.Forms.DockStyle.Bottom;
            webBrowser.Location = new System.Drawing.Point(0, 30);
            webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowser.Name = "webBrowser1";
            webBrowser.Size = new System.Drawing.Size(800, 570);
            webBrowser.TabIndex = 0;
			webBrowser.Url = new System.Uri("file:///" + System.Environment.CurrentDirectory + "/html/index.html", System.UriKind.Absolute);
			this.Controls.Add(webBrowser);
            
            
            
            
			
			//webBrowser.Document.InvokeScript(functionName, objects);

			//关闭按钮
            System.Windows.Forms.Button btnSwipeIDCard = new System.Windows.Forms.Button();
			btnSwipeIDCard.Text = "刷身份证";
			btnSwipeIDCard.Location = new System.Drawing.Point(0,0);
			btnSwipeIDCard.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			btnSwipeIDCard.Size = new System.Drawing.Size(100, 30);
			btnSwipeIDCard.UseVisualStyleBackColor = true;
			btnSwipeIDCard.Click += new System.EventHandler(delegate(object sender, EventArgs e){
				SwipeIDCard("{\"exam_name\":\"测试员\",\"exam_sex\":\"男\",\"exam_age\":66,\"exam_birthday\":\"1955-05-05\",\"idcard_no\":\"3708311905051239\",\"create_time\":\"" + System.DateTime.Now + "\"}");
   			});
			this.Controls.Add(btnSwipeIDCard);
            
            
            System.Windows.Forms.Button btnInsertScript = new System.Windows.Forms.Button();
			btnInsertScript.Text = "插入脚本";
			btnInsertScript.Location = new System.Drawing.Point(120,0);
			btnInsertScript.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			btnInsertScript.Size = new System.Drawing.Size(100, 30);
			btnInsertScript.UseVisualStyleBackColor = true;
			btnInsertScript.Click += new System.EventHandler(delegate(object sender, EventArgs e){
				InsertScript("{\"start_date\":\"2022-10-22\",\"end_date\":\"2022-11-22\",\"gaoxueya\":1,\"exam_no\":\"2206280005\",\"idcard_no\":\"3708311905051239\",\"create_time\":\"" + System.DateTime.Now + "\"}");
   			});
			this.Controls.Add(btnInsertScript);
        }
		public void SwipeIDCard(string info)
		{
			InvokeScript("swipeIDCard", info);
		}
        
        
        public void InvokeScript(string jsFunction, string data){
            webBrowser.Document.InvokeScript(jsFunction, new object[] { data });
		}
        

            
        public void InsertScript(string json)
		{
			System.Windows.Forms.HtmlElement script = webBrowser.Document.CreateElement("script");			
			script = webBrowser.Document.CreateElement("script");
            script.SetAttribute("type", "text/javascript");
			string function = "function(YLYQ){" + System.IO.File.ReadAllText("./InsertScript.js") + "}";
            script.SetAttribute("text", "(" + function + ")({\"config\":{},\"request\":" + json + "});");
            webBrowser.Document.Body.AppendChild(script);
		}
        public string HttpGet(string url, string charset = "UTF-8")
		{
			string result = null;
			System.Net.WebClient webClient = new System.Net.WebClient();
			System.Net.ServicePointManager.Expect100Continue = false;
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
			//webClient.Encoding = System.Text.Encoding.UTF8;
			try
			{
				byte[] bytes = webClient.DownloadData(url);
				result = System.Text.Encoding.GetEncoding(charset).GetString(bytes);
			}
			catch (System.Net.WebException webException)
			{
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webException.Response;
				if(httpWebResponse != null)
				{
					result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")).ReadToEnd();
				}
				else
				{
					result = "HttpGetError:" + webException.Message;
				}
			}
			catch (System.Exception ex)
			{
				result = "HttpGetError:" + ex.Message;
				//result = null;
			}
            return result;
		}
        public string HttpPost(string url, string data, string charset = "UTF-8", string contentType = "UTF-8")
		{
			string result = null;
			System.Net.WebClient webClient = new System.Net.WebClient();
			System.Net.ServicePointManager.Expect100Continue = false;
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
			//webClient.Encoding = System.Text.Encoding.UTF8;
			try
			{
				webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
				byte[] bytes = webClient.UploadData(url, System.Text.Encoding.GetEncoding(charset).GetBytes(data));
				result = System.Text.Encoding.GetEncoding(contentType).GetString(bytes);
			}
			catch (System.Net.WebException webException)
			{
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webException.Response;
				if(httpWebResponse != null)
				{
					result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")).ReadToEnd();
				}
			}
			catch (System.Exception ex)
			{
				result = "HttpPost:" + ex.Message;
			}
            return result;
		}
        
        public async void AjaxGet(object url){
			System.Threading.Tasks.Task<string> httpGet = System.Threading.Tasks.Task.Run(() => { return HttpGet(url.ToString()); });
            string response = await httpGet;
			InvokeScript("ajaxGet", response);
        }
        
        public string AjaxPost(object url, object data){
            return null;
        }
        public string AjaxJSON(object url, object data){
            return null;
        }
        
        
        public string SQLitePath(string path = null) 
        {
            string currentPath = System.IO.Directory.GetCurrentDirectory();
            return "Data Source=" + currentPath + "/database.db;version=3;";
        }

        public int ExecuteNonQuery(string sql, System.Collections.Specialized.NameValueCollection parameters = null) 
        {
            int rows = 0;
            System.Data.SQLite.SQLiteConnection sqliteConnection = new System.Data.SQLite.SQLiteConnection(SQLitePath());
            if (sqliteConnection.State != System.Data.ConnectionState.Open) { sqliteConnection.Open(); }
            System.Data.SQLite.SQLiteCommand sQLiteCommand = new System.Data.SQLite.SQLiteCommand();
            sQLiteCommand.Connection = sqliteConnection;
            sQLiteCommand.CommandText = sql;
            rows = sQLiteCommand.ExecuteNonQuery();
            sqliteConnection.Close();
            sqliteConnection.Dispose();
            return rows;
        }

        public object ExecuteScalar(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
            System.Data.SQLite.SQLiteConnection sqliteConnection = new System.Data.SQLite.SQLiteConnection(SQLitePath());
            if (sqliteConnection.State != System.Data.ConnectionState.Open) { sqliteConnection.Open(); }
            System.Data.SQLite.SQLiteCommand sQLiteCommand = new System.Data.SQLite.SQLiteCommand();
            sQLiteCommand.Connection = sqliteConnection;
            sQLiteCommand.CommandText = sql;
            object result = sQLiteCommand.ExecuteScalar();
            sqliteConnection.Close();
            sqliteConnection.Dispose();
            return result;
        }

        public System.Data.DataTable SQLiteDataAdapter(string sql, System.Collections.Specialized.NameValueCollection parameters = null) 
        {
            System.Data.SQLite.SQLiteConnection sqliteConnection = new System.Data.SQLite.SQLiteConnection(SQLitePath());
            if (sqliteConnection.State != System.Data.ConnectionState.Open) { sqliteConnection.Open(); }
            System.Data.SQLite.SQLiteCommand sQLiteCommand = new System.Data.SQLite.SQLiteCommand();
            sQLiteCommand.Connection = sqliteConnection;
            sQLiteCommand.CommandText = sql;
            sQLiteCommand.ExecuteNonQuery();
            System.Data.DataTable dataTable = new System.Data.DataTable();
            new System.Data.SQLite.SQLiteDataAdapter(sQLiteCommand).Fill(dataTable);
            sqliteConnection.Close();
            sqliteConnection.Dispose();
            return dataTable;
        }

        public System.Data.DataTable DataTable(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
              return SQLiteDataAdapter(sql, parameters);
        }
		
		protected System.Data.DataRow DataRow(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
            System.Data.DataTable dataTable = DataTable(sql, parameters);
            if (dataTable.Rows.Count == 0) { return null; }
            return dataTable.Rows[0];
        }
		
		protected System.Data.DataRowCollection DataRows(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
            return DataTable(sql, parameters).Rows;
        }
        public void GetExamData(object sql)
		{
			string rows = null;
			System.Data.DataRowCollection drc = DataRows("" + sql);
			foreach(System.Data.DataRow dr in drc)
			{
				string col = null;
				foreach (System.Data.DataColumn dataColumn in dr.Table.Columns)
				{
					col += ",\"" + dataColumn.ColumnName + "\":\"" + dr[dataColumn] + "\"";
				}
				if(col != null){col = col.Substring(1);}
				rows += ",{" + col + "}";
			}
			if(rows != null){rows = rows.Substring(1);}
			rows = "[" + rows + "]";
			object ret = webBrowser.Document.InvokeScript("getExamData", new object[] { rows });
		}
    }
}
