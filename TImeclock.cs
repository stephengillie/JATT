//Copyright 2025 Gilgamech Technologies
//Author: Stephen Gillie
//Created 06/22/2025
//Updated 06/22/2025
//Moar was the new finditem.





//////////////////////////////////////////====================////////////////////////////////////////
//////////////////////====================--------------------====================////////////////////
//====================--------------------      Init vars     --------------------====================//
//////////////////////====================--------------------====================////////////////////
//////////////////////////////////////////====================////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace ShopBotNamespace {
    public class Timeclock : Form {
//{ Ints
        public int build = 142;//Get-RebuildPipeApp Timeclock
		public string appName = "Timeclock";
		public string StoreName = "Not Loaded";
		public string StoreCoords = "Not Loaded";
		public string webHook = "Not Loaded";
		public string appTitle = "Just Another Time Tracker - 1.";

		List<FileData>  OldData = new List<FileData>();
		JavaScriptSerializer serializer = new JavaScriptSerializer();
		System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public Button runButton, stopButton;
		public TextBox hoursWorkedBox = new TextBox();
		public Label hoursWorkedLabel = new Label();
		public RichTextBox outBox = new RichTextBox();
		public System.Drawing.Bitmap myBitmap;
		public System.Drawing.Graphics pageGraphics;
		public ContextMenuStrip contextMenu1;
		
		public string[] parsedHtml = new string[1];
		public bool WebhookPresent = false;

		
		// public static string WindowsUsername = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
		// public static string MainFolder = "C:\\Users\\"+WindowsUsername+"\\AppData\\Roaming\\.minecraft\\";
		// public static string logFolder = MainFolder+"\\logs"; //Logs folder;
		public static string logFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\logs";
		//public string logFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Packages\\Microsoft.MinecraftUWP_8wekyb3d8bbwe\\LocalState\\logs"
		public string LatestLog = logFolder+"\\latest.log";

		public static string MainFolder = "C:\\ManVal";
		public static string logsFolder = MainFolder+"\\logs"; //VM Logs folder;
		public string TimeCardFileName = logsFolder+"\\timecard.txt";
		
		//ui
		public Panel pagePanel;
		public int displayLine = 0;
		public int sideBufferWidth = 0;
		public string Mode = "Stop";
		//outBox.Font = new Font("Calibri", 14);
		
		//Grid
		public static int gridItemWidth = 60;
		public static int gridItemHeight = 30;
		
		public static int row0 = gridItemHeight*0;
		public static int row1 = gridItemHeight*1;
		public static int row2 = gridItemHeight*2;
		public static int row3 = gridItemHeight*3;
		public static int row4 = gridItemHeight*4;
 		public static int row5 = gridItemHeight*5;
 		public static int row6 = gridItemHeight*6;
 		public static int row7 = gridItemHeight*7;
 		public static int row8 = gridItemHeight*8;
 		public static int row9 = gridItemHeight*9;
 		public static int row10 = gridItemHeight*10;
 			
 		public static int col0 = gridItemWidth*0;
 		public static int col1 = gridItemWidth*1;
 		public static int col2 = gridItemWidth*2;
 		public static int col3 = gridItemWidth*3;
 		public static int col4 = gridItemWidth*4;
 		public static int col5 = gridItemWidth*5;
 		public static int col6 = gridItemWidth*6;
 		public static int col7 = gridItemWidth*7;
 		public static int col8 = gridItemWidth*8;
 		public static int col9 = gridItemWidth*9;
 		public static int col10 = gridItemWidth*10;

public enum EventNames
{
    Empty,
    Full,
    Sell,
    Buy
}


		public int WindowWidth = col4+20;
		public int WindowHeight = row4+10;
		// public int WindowHeight = row7+10;
		
		public bool debuggingView = false;
		public string FullText = "is now full";//[05:33:18] [Render thread/INFO]: [System] [CHAT] SHOPS ▶ Your shop at 15274, 66, 20463 is now full.
		public string SellText = "to your shop";//[05:40:12] [Render thread/INFO]: [System] [CHAT] SHOPS ▶ kota490 sold 1728 Sea Lantern to your shop {3}.
		public string BuyText = "from your shop";//[05:47:12] [Render thread/INFO]: [System] [CHAT] SHOPS ▶ _Blackjack29313 purchased 2 Grindstone from your shop and you earned $9.50 ($0.50 in taxes).
		public string EmptyText = "has run out of";//[06:07:40] [Rend
		//public void OldDate = get-date -f dd

		//public string DataFile = "C:\\repos\\Timeclock\\Timeclock.csv";
		//public string OwnerList = "C:\\repos\\Timeclock\\ChillPWOwnerList.csv";






//////////////////////////////////////////====================////////////////////////////////////////
//////////////////////====================--------------------====================////////////////////
//====================--------------------    Boilerplate     --------------------====================//
//////////////////////====================--------------------====================////////////////////
//////////////////////////////////////////====================////////////////////////////////////////
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Timeclock());
        }// end Main

        public Timeclock() {
			// LoadSetting("StoreName", ref StoreName, "Moar"); 
			// LoadSetting("StoreCoords", ref StoreCoords, "StoreCoords"); 
			// LoadSetting("webHook", ref webHook, "webHook"); 
			this.Text =  appTitle + build;
			this.Size = new Size(WindowWidth,WindowHeight);
			this.Resize += new System.EventHandler(this.OnResize);
			this.AutoScroll = true;
			// Icon icon = Icon.ExtractAssociatedIcon("C:\\repos\\Timeclock\\Timeclock.ico");
			// this.Icon = icon;
			buildMenuBar();
			drawLabel(ref hoursWorkedLabel, col0, row0, col2, row1, "Hours worked today:");
			drawTextBox(ref hoursWorkedBox, col2, row0, col2, 0,"$0");
 			//drawButton(ref sendButton, col2, row0, col2, row1, "Daily Report", sendButton_Click);
 			drawButton(ref runButton, col0, row1, col2, row1, "Start", runButton_Click);
 			drawButton(ref stopButton, col2, row1, col2, row1, "Stop", stopButton_Click);
			// drawRichTextBox(ref outBox, col0, row1, col7, row4,"", "outBox");
			
			hoursWorkedBox.Font = new Font("Calibri", 14);
			outBox.Multiline = true;
			outBox.AcceptsTab = true;
			outBox.WordWrap = true;
			outBox.ReadOnly = true;
			outBox.DetectUrls = true;
			// outBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			RefreshStatus();

			timer.Interval = (5 * 1000);
			timer.Tick += new EventHandler(timer_everysecond);
			timer.Start();
        } // end Timeclock

		public void buildMenuBar (){
			this.Menu = new MainMenu();
			
			MenuItem item = new MenuItem("File");
			this.Menu.MenuItems.Add(item);
				item.MenuItems.Add("Open Timecard", new EventHandler(Open_TimeCardFileName)); 
			
			// item = new MenuItem("Reports");
			// this.Menu.MenuItems.Add(item);
				// item.MenuItems.Add("Daily Revenue", new EventHandler(Daily_Revenue));
				// item.MenuItems.Add("Daily Sales Report", new EventHandler(Daily_Report));
				// item.MenuItems.Add("Send Daily Sales Report to Webhook", new EventHandler(Send_Daily_Report));
				// item.MenuItems.Add("Biweekly Report", new EventHandler(Biweekly_Report));
			
			item = new MenuItem("Help");
			this.Menu.MenuItems.Add(item);
				item.MenuItems.Add("About", new EventHandler(About_Click));
				item.MenuItems.Add("Test", new EventHandler(About_Test));
	   }// end buildMenuBar
		
 		public void RefreshStatus() {
			try {
				hoursWorkedBox.Text = HoursWorkedToday(); 
			} catch {}
			Color color_RedBack = Color.FromArgb(240,0,0);
			Color color_GreenBack = Color.FromArgb(0,240,0);
			Color color_DefaultBack = Color.FromArgb(240,240,240);
			Color color_DefaultText = Color.FromArgb(0,0,0);
			Color color_InputBack = Color.FromArgb(255,255,255);
			Color color_ActiveBack = Color.FromArgb(200,240,240);
			
			Mode = GetCurrentMode();

			if (Mode == "Start") {
				this.BackColor = color_GreenBack;
				runButton.BackColor = color_DefaultBack;
				runButton.Size = new Size(col1, row1);				
				stopButton.BackColor = color_RedBack;
				stopButton.Location = new Point(col1, row1);
				stopButton.Size = new Size(col3, row1);				
				
			} else if (Mode == "Stop") {
				this.BackColor = color_RedBack;
				runButton.BackColor = color_GreenBack;
				runButton.Size = new Size(col3, row1);				
				stopButton.BackColor = color_DefaultBack;
				stopButton.Location = new Point(col3, row1);
				stopButton.Size = new Size(col1, row1);				
			} 
		} 




//////////////////////////////////////////====================////////////////////////////////////////
//////////////////////====================--------------------====================////////////////////
//===================--------------------    Event Handlers   --------------------====================
//////////////////////====================--------------------====================////////////////////
//////////////////////////////////////////====================////////////////////////////////////////
		//timer
		private void timer_everysecond(object sender, EventArgs e) {
			RefreshStatus();
		}

		//ui
        public void runButton_Click(object sender, EventArgs e) {
			// Mode = "Start";
			// UpdateTimeCard(Mode);
			UpdateTimeCard("Start");
			// // outBox.Text = Mode + Environment.NewLine + outBox.Text;
			RefreshStatus();
			// RunBot(0);
			// timer.Start();
        }// end runButton_Click

        public void stopButton_Click(object sender, EventArgs e) {
			// Mode = "Stop";
			// UpdateTimeCard(Mode);
			UpdateTimeCard("Stop");
			// outBox.Text = Mode + Environment.NewLine + outBox.Text;
			RefreshStatus();
			// timer.Stop();
        }// end stopButton_Click

		public void OnResize(object sender, System.EventArgs e) {
		}

	//Menu
		//File
		public void Open_TimeCardFileName(object sender, EventArgs e) {
			Process.Start(TimeCardFileName);
		}// end Open_TimeCardFileName

		//Help
		public void About_Click (object sender, EventArgs e) {
			string AboutText = "Moar Shop Bot" + Environment.NewLine;
			AboutText += "Generates out-of-stock alerts and financial reports from QuickShop" + Environment.NewLine;
			AboutText += "Buy/Sell comments in Minecraft chat logs. Made for The Moar player" + Environment.NewLine;
			AboutText += "-run store on the ChillSMP Minecraft server. But this product isn't" + Environment.NewLine;
			AboutText += "affiliated with any of those." + Environment.NewLine;
			AboutText += "" + Environment.NewLine;
			AboutText += "Version 1." + build + Environment.NewLine;
			AboutText += "(C) 2025 Gilgamech Technologies" + Environment.NewLine;
			AboutText += "" + Environment.NewLine;
			AboutText += "Report bugs & request features:" + Environment.NewLine;
			AboutText += "https://github.com/Gilgamech/Timeclock/issues" + Environment.NewLine;
			MessageBox.Show(AboutText);
		} // end Link_Click

		public void About_Test (object sender, EventArgs e) {
			hoursWorkedBox.Text = HoursWorkedToday(); 
		} // end Link_Click






//////////////////////////////////////////====================////////////////////////////////////////
//////////////////////====================--------------------====================////////////////////
//====================--------------------      Main     --------------------====================//
//////////////////////====================--------------------====================////////////////////
//////////////////////////////////////////====================////////////////////////////////////////


//Store start and end time in a csv
//If running, read start time and subract now, UI is green
//If not running, read start time and subtract end time, UI is red
//Go button always green and stop button always red
//Have reports

		public string GetHoursWorked () {
			string output = OldGetContent(TimeCardFileName);
			outBox.Text = output + Environment.NewLine + outBox.Text;
			
			return output.Split('\n')[0];
		}

		public void UpdateTimeCard (string mode) {
			string timeStamp = DateTime.Now.ToString("s");
			string content = timeStamp + " "+ mode +  Environment.NewLine;
			OutFile(TimeCardFileName, content, true);
		}
		
		public string GetCurrentMode() {
			string[] input = OldGetContent(TimeCardFileName).Replace("\r","").Split('\n');
			Array.Resize(ref input, input.Length - 1);//This removes the trailing line break at the end of the file. 
			return input[input.Length -1].Split(' ')[1];
		}
		
		public string HoursWorkedToday() {
			string Today = DateTime.Now.ToString("dd");
			string MatchDate = DateTime.Now.ToString("yyyy-MM-dd");
			string newvar = "";
			
			List<FileData> out_var = new List<FileData>();
			FileData output = new FileData();
			// List<string> Data = new List<string>();
			// int n = 0;
			// List<string> output2 = null;
			string[] input = OldGetContent(TimeCardFileName).Replace("\r","").Split('\n');
			Array.Resize(ref input, input.Length - 1);//This removes the trailing line break at the end of the file. 
			// outBox.Text = "input: " + serializer.Serialize(input) + Environment.NewLine + outBox.Text;
			// input = input.Where(d => !d.Contains(MatchDate)).ToList();
			foreach (string Item in input) {
				if (Item.Contains(MatchDate)) {
					if (Item.Length > 1) {
						newvar = newvar + Item.Replace("T"," ").Split(' ')[1] + ",";
					}
				}
				// n++;
				// string outTime = Item.Replace("T"," ").Split(' ')[1];
				// outBox.Text = "Item: " + serializer.Serialize(Item) + Environment.NewLine + outBox.Text;
				// output2.Add(outTime);
			}
			string[] newsplit = newvar.Split(',');
			// outBox.Text = "newsplit.Length: " + serializer.Serialize(newsplit.Length) + Environment.NewLine + outBox.Text;
			if ((newsplit.Length % 2) == 0) {
				newsplit[newsplit.Length -1] = DateTime.Now.ToString("T");
			} else {
				Array.Resize(ref newsplit, newsplit.Length - 1);//This removes the trailing line break at the end of the file. 
			}

			//output2 = output2.Where(d => d.Contains(MatchDate)).ToList();
			// outBox.Text = "newsplit: " + serializer.Serialize(newsplit) + Environment.NewLine + outBox.Text;
			double aggregator = 0;
			for (int incrementor = 0; incrementor < newsplit.Length; incrementor = incrementor + 2){
				string timeStamp = DateTime.Now.ToString("s");

				aggregator += DateTime.Parse(newsplit[incrementor+1]).Subtract(DateTime.Parse(newsplit[incrementor])).TotalHours;
			};
/*
			[math]::Round(aggregator.totalHours,2)
*/		
			
			return Math.Round(aggregator, 2).ToString();
		}



//////////////////////////////////////////====================////////////////////////////////////////
//////////////////////====================--------------------====================////////////////////
//===================--------------------   Utility Functions  --------------------====================
//////////////////////====================--------------------====================////////////////////
//////////////////////////////////////////====================////////////////////////////////////////
/*Powershell functional equivalency imperatives
		Get-Clipboard = Clipboard.GetText();
		Get-Date = Timestamp.Now.ToString("M/d/yyyy");
		Get-Process = public Process[] processes = Process.GetProcesses(); or var processes = Process.GetProcessesByName("Test");
		New-Item = Directory.CreateDirectory(Path) or File.Create(Path);
		Remove-Item = Directory.Delete(Path) or File.Delete(Path);
		Get-ChildItem = string[] entries = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
		Start-Process = System.Diagnostics.Process.Start("PathOrUrl");
		Stop-Process = StopProcess("ProcessName");
		Start-Sleep = Thread.Sleep(GitHubRateLimitDelay);
		Get-Random - Random rnd = new Random(); or int month  = rnd.Next(1, 13);  or int card   = rnd.Next(52);
		Create-Archive = ZipFile.CreateFromDirectory(dataPath, zipPath);
		Expand-Archive = ZipFile.ExtractToDirectory(zipPath, extractPath);
		Sort-Object = .OrderBy(n=>n).ToArray(); and -Unique = .Distinct(); Or Array.Sort(strArray); or List
		
		Get-VM = GetVM("VMName");
		Start-VM = SetVMState("VMName", 2);
		Stop-VM = SetVMState("VMName", 4);
		Stop-VM -TurnOff = SetVMState("VMName", 3);
		Reboot-VM = SetVMState("VMName", 10);
		Reset-VM = SetVMState("VMName", 11);
		
		Diff
		var inShopDataButNotInOldData = ShopData.Except(OldData);
		var inOldDataButNotInShopData = OldData.Except(ShopData);
		
*/
		public string findIndexOf(string pageString,string startString,string endString,int startPlus,int endPlus){
			return pageString.Substring(pageString.IndexOf(startString)+startPlus, pageString.IndexOf(endString) - pageString.IndexOf(startString)+endPlus);
        }// end findIndexOf

		public void DeGZip (string infile) {
			string outfile = infile.Replace(".gz","");
			FileStream compressedFileStream = File.Open(infile, FileMode.Open);
			FileStream outputFileStream = File.Create(outfile);
			var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
			decompressor.CopyTo(outputFileStream);
		}
		//JSON
		public dynamic FromJson(string string_input) {
			dynamic dynamic_output = new System.Dynamic.ExpandoObject();
			dynamic_output = serializer.Deserialize<dynamic>(string_input);
			return dynamic_output;
		}

		public string ToJson(dynamic dynamic_input) {
			string string_out;
			string_out = serializer.Serialize(dynamic_input);
			return string_out;
		}
		//CSV
		public Dictionary<string, dynamic>[] FromCsv(string csv_in) {
			//CSV isn't just a 2d object array - it's an array of Dictionary<string,object>, whose string keys are the column headers. 
			string[] Rows = csv_in.Replace("\r\n","\n").Replace("\"","").Split('\n');
			string[] columnHeaders = Rows[0].Split(',');
			Dictionary<string, dynamic>[] matrix = new Dictionary<string, dynamic> [Rows.Length];
			try {
				for (int row = 1; row < Rows.Length; row++){
					matrix[row] = new Dictionary<string, dynamic>();
					//Need to enumerate values to create first row.
					string[] rowData = Rows[row].Split(',');
					try {
						for (int col = 0; col < rowData.Length; col++){
							//Need to record or access first row to match with values. 
							matrix[row].Add(columnHeaders[col].ToString(), rowData[col]);
						}
					} catch {
					}
				}
			} catch {
			}
			return matrix;
		}

		public string ToCsv(Dictionary<string, dynamic>[] matrix) {
			string csv_out = "";
			//Arrays seem to have a buffer row above and below the data.
			int topRow = 1;
			Dictionary<string, dynamic> headerRow = matrix[topRow];
			//Write header row (th). Support for multi-line headers maybe someday but not today. 
			if (headerRow != null) {
				string[] columnHeaders = new string[headerRow.Keys.Count];
				headerRow.Keys.CopyTo(columnHeaders, 0);
				//var a = matrix[0].Keys;
				foreach (string columnHeader in columnHeaders){
						csv_out += columnHeader.ToString()+",";
				}
				csv_out = csv_out.TrimEnd(',');
				// Write data rows (td).
				for (int row = topRow; row < matrix.Length -1; row++){
					csv_out += "\n";
					foreach (string columnHeader in columnHeaders){
						csv_out += matrix[row][columnHeader]+",";
					}
					csv_out = csv_out.TrimEnd(',');
				}
			}
			csv_out += "\n";
			return csv_out;
		}
		//File
		//Non-locking alternative: System.IO.File.ReadAllBytes(Filename);
		public string GetContent(string Filename, bool NoErrorMessage = false, bool Debug = false) {
			string fiileString = null;
			try {
			//outBox.Text = "fiileString Start" +  Environment.NewLine + outBox.Text;
			
			FileStream logFileStream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			StreamReader logFileReader = new StreamReader(logFileStream);
			while (!logFileReader.EndOfStream) {
				// outBox.Text = "(GetContent) fiileString While."+ fiileString.Length + Environment.NewLine + outBox.Text;
				fiileString = logFileReader.ReadLine();
			if (Debug == true) {
				outBox.Text = "(GetContent) fiileString.Length."+ fiileString.Length + Environment.NewLine + outBox.Text;
			}
			}
			if (Debug == true) {
				outBox.Text = "(GetContent) FileStream Success."+ fiileString.Length + Environment.NewLine + outBox.Text;
			}
			logFileReader.Close();
			logFileStream.Close();
			} catch (Exception e){
				if (Debug == true) {
					outBox.Text = "(GetContent) Error."+ e.Message + Environment.NewLine + outBox.Text;
				}
			}
			return fiileString;
		}
		public string OldGetContent(string Filename, bool NoErrorMessage = false) {
			string string_out = "";
			try {
				// Open the text file using a stream reader.
				using (var sr = new StreamReader(Filename)) {
					// Read the stream as a string, and write the string to the console.
					string_out = sr.ReadToEnd();
				}
			} catch (Exception e){
				outBox.Text = "(OldGetContent) Error."+ e.Message + Environment.NewLine + outBox.Text;
			}
			return string_out;
		}

		public void OutFile(string path, object content, bool Append = false) {
			//From SO: Use "typeof" when you want to get the type at compilation time. Use "GetType" when you want to get the type at execution time. "is" returns true if an instance is in the inheritance tree.
			if (TestPath(path) == "None") {
				File.Create(path).Close();
			}
			if (content.GetType() == typeof(string)) {
				string out_content = (string)content;
			//From SO: File.WriteAllLines takes a sequence of strings - you've only got a single string. If you only want your file to contain that single string, just use File.WriteAllText.
				if (Append == true) {
					File.AppendAllText(path, out_content, Encoding.ASCII);//string
				} else {
					File.WriteAllText(path, out_content, Encoding.ASCII);//string
				}
			} else {
				IEnumerable<string> out_content = (IEnumerable<string>)content;
				if (Append == true) {
					File.AppendAllLines(path, out_content, Encoding.ASCII);//IEnumerable<string>'
				} else {
					File.WriteAllLines(path, out_content, Encoding.ASCII);//string[]
				}				
			}
		}

		public void RemoveItem(string Path,bool remake = false){
			if (TestPath(Path) == "File") {
				File.Delete(Path);
				if (remake) {
					File.Create(Path);
				}
			} else if (TestPath(Path) == "Directory") {
				Directory.Delete(Path, true);
				if (remake) {
					Directory.CreateDirectory(Path);
				}
			}
		}

		public string TestPath(string path) {
				string string_out = "";
				if (path != null) {
						path = path.Trim();
					if (Directory.Exists(path)) {
						string_out = "Directory";
					} else if (File.Exists(path)) {
						string_out = "File";
					} else {// neither file nor directory exists. guess intention
						string_out = "None";
					}
				} else {// neither file nor directory exists. guess intention
					string_out = "Empty";
				}
				return string_out;
			}
		//Web
		public string InvokeWebRequest(string Url, string Method = WebRequestMethods.Http.Get, string Body = "",bool Authorization = false,bool JSON = false){ 
			string response_out = "";

				// SSL stuff
				//ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
				
				if (JSON == true) {
					request.ContentType = "application/json";
				}
				if (Authorization == true) {
					//request.Headers.Add("Authorization", "Bearer "+webHook);
					//request.Headers.Add("X-GitHub-Api-build", "2022-11-28");
					request.PreAuthenticate = true;
				}

				request.Method = Method;
				request.ContentType = "application/json;charset=utf-8";
				request.Accept = "application/vnd.github+json";
				request.UserAgent = "WinGetApprovalPipeline";

			 //Check Headers
			 // for (int i=0; i < response.Headers.Count; ++i)  {
				// outBox_msg.AppendText(Environment.NewLine + "Header Name : " + response.Headers.Keys[i] + "Header value : " + response.Headers[i]);
			 // }

			try {
				if ((Body == "") || (Method ==WebRequestMethods.Http.Get)) {
				} else {
					var data = Encoding.Default.GetBytes(Body); // note: choose appropriate encoding
					request.ContentLength = data.Length;
					var newStream = request.GetRequestStream(); // get a ref to the request body so it can be modified
					newStream.Write(data, 0, data.Length);
					newStream.Close();
				} 

				} catch (Exception e) {
					outBox.Text = "(InvokeWebRequest) Request Error: " + e.Message + Environment.NewLine + outBox.Text;
				}
				
				try {
					HttpWebResponse response = (HttpWebResponse)request.GetResponse();
					StreamReader sr = new StreamReader(response.GetResponseStream());
					if (Method == "Head") {
						string response_text = response.StatusCode.ToString();
						response_out = response_text;

					} else {
						string response_text = sr.ReadToEnd();
						response_out = response_text;
					}
					sr.Close();
				} catch (Exception e) {
					outBox.Text = "(InvokeWebRequest) Response Error: " + e.Message + Environment.NewLine + outBox.Text;
				}
		return response_out;
		}// end InvokeWebRequest	
		//Discord
		public void SendMessageToWebhook (string content) {
			string payload = "{\"content\": \"" + content + "\"}";
			if (webHook.Contains("http")) {
				InvokeWebRequest(webHook, WebRequestMethods.Http.Post, payload,false,true);
			}
		}

        public string ReadSetting(string key) {
			string result = "Not Found";
            try {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key] ?? "Not Found";
            } catch (ConfigurationErrorsException) {
				outBox.Text = "Error reading app settings" + Environment.NewLine + outBox.Text;
            }
			return result;
        }

        public void DeleteSetting(string key) {
            try {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null) {
                } else {
                    settings.Remove(key);
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            } catch (ConfigurationErrorsException) {
				outBox.Text = "Error reading app settings" + Environment.NewLine + outBox.Text;
            }
        }

        public void AddOrUpdateSetting(string key, string value) {
            try {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null) {
                    settings.Add(key, value);
                } else {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            } catch (ConfigurationErrorsException) {
				outBox.Text = "Error reading app settings" + Environment.NewLine + outBox.Text;
            }
        }
		
        public void LoadSetting(string key, ref string value, string defaultValue) {
			if (value == "Not Loaded") {
				value = ReadSetting(key);
				if (value == "Not Found") {
					value = defaultValue;
					AddOrUpdateSetting(key, value);
				}
			}
        }








//////////////////////////////////////////====================////////////////////////////////////////
//////////////////////====================--------------------====================////////////////////
//====================--------------------   UI templates    --------------------====================//
//////////////////////====================--------------------====================////////////////////
//////////////////////////////////////////====================////////////////////////////////////////
		public void drawButton(ref Button button, int pointX, int pointY, int sizeX, int sizeY,string buttonText, EventHandler buttonOnclick){
			button = new Button();
			button.Text = buttonText;
			button.Location = new Point(pointX, pointY);
			button.Size = new Size(sizeX, sizeY);
			//button.BackColor = color_DefaultBack;
			//button.ForeColor = color_DefaultText;
			button.Click += new EventHandler(buttonOnclick);
			// button.Font = new Font(buttonFont, buttonFontSIze);
			Controls.Add(button);
		}// end drawButton

		public void drawRichTextBox(ref RichTextBox richTextBox, int pointX,int pointY,int sizeX,int sizeY,string text, string name){
			richTextBox = new RichTextBox();
			richTextBox.Text = text;
			richTextBox.Name = name;
			richTextBox.Multiline = true;
			richTextBox.AcceptsTab = true;
			richTextBox.WordWrap = true;
			richTextBox.ReadOnly = true;
			richTextBox.DetectUrls = true;
			// richTextBox.BackColor = color_DefaultBack;
			// richTextBox.ForeColor = color_DefaultText;
			// richTextBox.Font = new Font(AppFont, AppFontSIze);
			richTextBox.Location = new Point(pointX, pointY);
			//richTextBox.LinkClicked  += new LinkClickedEventHandler(Link_Click);
			richTextBox.Width = sizeX;
			richTextBox.Height = sizeY;
			//richTextBox.Dock = DockStyle.Fill;
			richTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;


			//richTextBox.BackColor = Color.Red;
			//richTextBox.ForeColor = Color.Blue;
			//richTextBox.RichTextBoxScrollBars = ScrollBars.Both;
			//richTextBox.AcceptsReturn = true;

			Controls.Add(richTextBox);
		}// end drawRichTextBox
		
		public void drawTextBox(ref TextBox urlBox, int pointX, int pointY, int sizeX, int sizeY,string text){
			urlBox = new TextBox();
			urlBox.Text = text;
			urlBox.Name = "urlBox";
			// urlBox.Font = new Font(AppFont, urlBoxFontSIze);
			urlBox.Location = new Point(pointX, pointY);
			// urlBox.BackColor = color_InputBack;
			// urlBox.ForeColor = color_DefaultText;
			urlBox.Width = sizeX;
			urlBox.Height = sizeY;
			Controls.Add(urlBox);
		}
		
		public void drawLabel(ref Label newLabel, int pointX, int pointY, int sizeX, int sizeY,string text){
			newLabel = new Label();
			newLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			//newLabel.ImageList = imageList1;
			newLabel.ImageIndex = 1;
			newLabel.ImageAlign = ContentAlignment.TopLeft;
			// newLabel.BackColor = color_DefaultBack;
			// newLabel.ForeColor = color_DefaultText;
			newLabel.Name = "newLabel";
			// newLabel.Font = new Font(AppFont, AppFontSIze);
			newLabel.Location = new Point(pointX, pointY);
			newLabel.Width = sizeX;
			newLabel.Height = sizeY;
			//newLabel.KeyUp += newLabel_KeyUp;

			newLabel.Text = text;

			//newLabel.Size = new Size (label1.PreferredWidth, label1.PreferredHeight);
			Controls.Add(newLabel);
		}

		public void drawDataGrid(ref DataGridView dataGridView, int startX, int startY, int sizeX, int sizeY){
			dataGridView = new DataGridView();
			dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
			dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
			dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			
			// dataGridView.ForeColor = color_DefaultText;//Selected cell text color
			// dataGridView.BackColor = color_DefaultBack;//Selected cell BG color
			// dataGridView.DefaultCellStyle.SelectionForeColor  = color_DefaultText;//Unselected cell text color
			// dataGridView.DefaultCellStyle.SelectionBackColor = color_DefaultBack;//Unselected cell BG color
			// dataGridView.BackgroundColor = color_DefaultBack;//Space underneath/between cells
			dataGridView.GridColor = SystemColors.ActiveBorder;//Gridline color
			
			dataGridView.Name = "dataGridView";
			// dataGridView.Font = new Font(AppFont, AppFontSize);
			dataGridView.Location = new Point(startX, startY);
			dataGridView.Size = new Size(sizeX, sizeY);
			// dataGridView.KeyUp += dataGridView_KeyUp;
			// dataGridView.Text = text;
			Controls.Add(dataGridView);


		
			dataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
			dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
			dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridView.AllowUserToDeleteRows = false;
			dataGridView.RowHeadersVisible = false;
			dataGridView.MultiSelect = false;
			//dataGridView.Dock = DockStyle.Fill;

/*
			dataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView_CellFormatting);
			dataGridView.CellParsing += new DataGridViewCellParsingEventHandler(dataGridView_CellParsing);
			addNewRowButton.Click += new EventHandler(addNewRowButton_Click);
			deleteRowButton.Click += new EventHandler(deleteRowButton_Click);
			ledgerStyleButton.Click += new EventHandler(ledgerStyleButton_Click);
			dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
*/
		}// end drawDataGrid

		public void drawToolTip(ref ToolTip toolTip, ref Button button, string DisplayText, int AutoPopDelay = 5000, int InitialDelay = 1000, int ReshowDelay = 500){
			toolTip = new ToolTip();

			// Set up the delays for the ToolTip.
			toolTip.AutoPopDelay = AutoPopDelay;
			toolTip.InitialDelay = InitialDelay;
			toolTip.ReshowDelay = ReshowDelay;
			// Force the ToolTip text to be displayed whether or not the form is active.
			toolTip.ShowAlways = true;
			 
			// Set up the ToolTip text for the Button and Checkbox.
			toolTip.SetToolTip(button, DisplayText);
			//toolTip.SetToolTip(this.checkBox1, "My checkBox1");
		}

		public void drawStatusStrip (StatusStrip statusStrip,ToolStripStatusLabel toolStripStatusLabel) {
			statusStrip = new System.Windows.Forms.StatusStrip();
			statusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            statusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            
			toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new System.Drawing.Size(109, 17);
            toolStripStatusLabel.Text = "toolStripStatusLabel";
			statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel });
            
            statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            statusStrip.Location = new System.Drawing.Point(0, 0);
            statusStrip.Name = "statusStrip";
            statusStrip.ShowItemToolTips = true;
            statusStrip.Size = new System.Drawing.Size(292, 22);
            statusStrip.SizingGrip = false;
            statusStrip.Stretch = false;
            statusStrip.TabIndex = 0;
            statusStrip.Text = "statusStrip";
			
			Controls.Add(statusStrip);
		}
		
		public static DialogResult drawInputDialog(ref string input, string boxTitle) {
			System.Drawing.Size size = new System.Drawing.Size(200, 70);
			Form inputBox = new Form();

			inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			inputBox.ClientSize = size;
			inputBox.Text = boxTitle;

			System.Windows.Forms.TextBox textBox = new TextBox();
			textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
			textBox.Location = new System.Drawing.Point(5, 5);
			textBox.Text = input;
			inputBox.Controls.Add(textBox);

			Button okButton = new Button();
			okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			okButton.Name = "okButton";
			okButton.Size = new System.Drawing.Size(75, 23);
			okButton.Text = "&OK";
			okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
			inputBox.Controls.Add(okButton);

			Button cancelButton = new Button();
			cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new System.Drawing.Size(75, 23);
			cancelButton.Text = "&Cancel";
			cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
			inputBox.Controls.Add(cancelButton);

			inputBox.AcceptButton = okButton;
			inputBox.CancelButton = cancelButton; 

			DialogResult result = inputBox.ShowDialog();
			input = textBox.Text;
			return result;
		}
    }// end Timeclock
	
	public class FileData  {
	  public string Timestamp;
	  public string State;
	}
}// end ShopBotNamespace

