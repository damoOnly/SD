using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Entity;
using DevExpress.XtraCharts;
using Dal;
using System.Threading;
using CommandManager;
using System.Diagnostics;
using System.Media;
using System.Linq;
using SDApplication.Properties;
using System.Net;

namespace SDApplication
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        #region 变量

        /// <summary>
        /// 实时曲线X轴最小值
        /// </summary>
        private DateTime minTime = DateTime.Now;

        /// <summary>
        /// 实时曲线X轴最大值
        /// </summary>
        private DateTime maxTime = DateTime.Now.AddMinutes(30);

        /// <summary>
        /// 当前设备
        /// </summary>
        private Equipment equipmentOne;

        /// <summary>
        /// 曲线对象
        /// </summary>
        private Series seriesOne;

        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfig systemConfig = new SystemConfig();

        /// <summary>
        /// 所有设备列表
        /// </summary>
        private List<Equipment> mainList = new List<Equipment>();
        
        /// <summary>
        /// 读取数据线程开关
        /// </summary>
        private bool isRead = true;

        /// <summary>
        /// 暂停
        /// </summary>
        private bool suspend = false;

        /// <summary>
        /// 声音播放对象
        /// </summary>
        private SoundPlayer player;

        /// <summary>
        /// 是否已经播放声音
        /// </summary>
        private bool IsSoundPlayed = false;

        /// <summary>
        /// 关闭声音播放
        /// </summary>
        private bool IsClosePlay = false;

        private AsyncTcpServer tcpserver;

        private Thread threadForConnect;

        #endregion

        #region 方法
        // 切换曲线
        private void changeSeries(Equipment ep, bool isClear = true)
        {
            if (ep == null)
            {
                LogLib.Log.GetLogger(this).Warn("切换曲线时对象为空");
                return;
            }
            seriesOne.Name = ep.EName;
            seriesOne.Tag = ep.ID;
            if (isClear)
            {
                seriesOne.Points.Clear();
            }

            SwiftPlotDiagram diagram_Tem = chartControl_Main.Diagram as SwiftPlotDiagram;
            diagram_Tem.AxisY.Title.Text = string.Format("浓度({0})", ep.Unit);

            List<EquipmentData> datalist = EquipmentDataDal.GetListByTime(ep.ID, minTime, maxTime);
            if (datalist == null)
            {
                return;
            }
            datalist.ForEach(c =>
            {
                SeriesPoint sp = new SeriesPoint(c.AddTime, c.Chroma);
                seriesOne.Points.Add(sp);
            });
            //datalist.Aggregate
        }

        private void server_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<string> e)
        {
            if (e.Datagram != "Received")
            {
                if (Gloabl.IsAdmin)
                {
                    this.Invoke(new Action<string>(addText), "recv: " + e.Datagram);
                }
                List<EquipmentData> list = Parse.GetSocketDataList(e.Datagram);
                refreshSocketData(list);
            }
        }

        private void tcpserver_DataReceived(object sender, AsyncSocketEventArgs e) 
        {
            //string datastr = Encoding.Default.GetString(e._state.RecvDataBuffer);
            //if (Gloabl.IsAdmin)
            //{
            //    this.Invoke(new Action<string>(addText), "recv: " + datastr);
            //}
            //EquipmentData ed = Parse.GetSocketDataList(datastr);
            //refreshSocketData(ed);
        }

        private void GetAlertValue(ref EquipmentData ed, Equipment eq)
        {
            ed.Chroma = Convert.ToSingle(Math.Round(ed.Chroma * eq.Revise, eq.Point));

            if (ed.Chroma > eq.Range)
            {
                ed.ChromaAlertStr = EM_AlertType.超量程报警.ToString();
            }
            else if (ed.Chroma > eq.HighAlert)
            {
                ed.ChromaAlertStr = EM_AlertType.高浓度报警.ToString();
            }
            else if (ed.Chroma > eq.LowAlert)
            {
                ed.ChromaAlertStr = EM_AlertType.低浓度报警.ToString();
            }
        }

        private void refreshSocketData(List<EquipmentData> edList)
        {
            foreach (var item in edList)
            {
                EquipmentData ed = item;
                Equipment eq = mainList.Find(c => c.Address == ed.Address);
                if (eq == null || !ed.Flag)
                {
                    return;
                }
                ed.EquipmentID = eq.ID;

                GetAlertValue(ref ed, eq);

                if (!string.IsNullOrWhiteSpace(ed.CRC) && !ed.CRC.Equals(eq.CRC))
                {
                    eq.IsConnect = true;
                    eq.lostNum = 0;
                    eq.CRC = ed.CRC;
                }
                else if (!string.IsNullOrWhiteSpace(ed.CRC))
                {
                    eq.CRC = ed.CRC;
                }
                
                // 添加数据库
                EquipmentDataDal.AddOne(ed);

                eq.Chroma = ed.Chroma;


                // 绘制曲线
                if (eq.ID == Convert.ToInt32(seriesOne.Tag))
                {
                    this.Invoke(new Action<EquipmentData>(c => addPoint(c)), ed);
                }
                if (eq.AlertType == 0)
                {
                    eq.ChromaAlertStr = Gloabl.NormalStr;
                }
                else
                {
                    GetAlert(ref eq, ed);
                }
            }            

            Equipment eqqq = mainList.Find(c => !c.ChromaAlertStr.Equals(Gloabl.NormalStr, StringComparison.OrdinalIgnoreCase));
            if (eqqq != null)
            {
                PlaySound(true);
            }
            else
            {
                PlaySound(false);
            }
            gridControl_Main.RefreshDataSource();
            //this.Invoke(new Action(gridControl_Main.RefreshDataSource));
            this.Invoke(new Action(gridView_Main.BestFitColumns));
            


            //gridControl_Main.BeginUpdate();
           //  gridControl_Main.RefreshDataSource();
            //gridControl_Main.EndUpdate();

            // gridView_Main.BestFitColumns();
        }

        private void GetAlert(ref Equipment eq, EquipmentData ed) 
        {
            // 报警记录
            if (eq.ChromaAlertStr != ed.ChromaAlertStr)
            {

                if (eq.ChromaAlertStr.Equals(Gloabl.NormalStr, StringComparison.OrdinalIgnoreCase))
                {
                    Alert art = new Alert();
                    art.AlertName = ed.ChromaAlertStr;
                    art.EquipmentID = eq.ID;
                    eq.AlertObject = AlertDal.AddOneR(art);
                }
                else
                {
                    eq.AlertObject.EndTime = DateTime.Now;
                    AlertDal.UpdateOne(eq.AlertObject);
                    if (!eq.ChromaAlertStr.Equals(ed.ChromaAlertStr, StringComparison.OrdinalIgnoreCase))
                    {
                        Alert art = new Alert();
                        art.AlertName = ed.ChromaAlertStr;
                        art.EquipmentID = eq.ID;
                        eq.AlertObject = AlertDal.AddOneR(art);
                    }
                }
                eq.ChromaAlertStr = ed.ChromaAlertStr;
            }
            else
            {
                if (!eq.ChromaAlertStr.Equals(Gloabl.NormalStr, StringComparison.OrdinalIgnoreCase))
                {
                    eq.AlertObject.EndTime = DateTime.Now;
                    if (!AlertDal.UpdateOne(eq.AlertObject))
                    {
                        Alert art = new Alert();
                        art.AlertName = ed.ChromaAlertStr;
                        art.EquipmentID = eq.ID;
                        eq.AlertObject = AlertDal.AddOneR(art);
                    }
                }
            }
        }

        // 新增点
        private void addPoint(EquipmentData ed)
        {
            if (ed.AddTime > maxTime)
            {
                minTime = DateTime.Now;
                maxTime = minTime.AddMinutes(systemConfig.Xrange);
                changeSeries(mainList.Find(c => c.ID == ed.EquipmentID));
            }
            seriesOne.Points.Add(new SeriesPoint(ed.AddTime, ed.Chroma));
            // 计算并显示面积值
            CalcArea();
        }

        /// <summary>
        /// 计算面积和显示面积都放在同一个方法里面
        /// </summary>
        private void CalcArea()
        {
            decimal area1 = 0m;
            decimal area2 = 0m;
            decimal ggg = 0m;
            // 上一个节点,默认为第一个节点
            SeriesPoint lastPoint = seriesOne.Points.First() as SeriesPoint;
            if (lastPoint == null)
            {
                return;
            }

            // 循环计算每个梯形的面积，再求和
            foreach (SeriesPoint point in seriesOne.Points)
            {
                int high = (point.DateTimeArgument - lastPoint.DateTimeArgument).Seconds;
                //Console.WriteLine("lastPoint:" + lastPoint.DateTimeArgument);
                //Console.WriteLine("point:    " + point.DateTimeArgument);
                //Console.WriteLine("high :    " + high);
                //Console.WriteLine("\r\n");
                if (high<systemConfig.Preiod)
                {
                    high = systemConfig.Preiod;
                }
                // 梯形的面积，上底加下底乘以高除以2
                decimal temparea = Convert.ToDecimal((point.Values[0] + lastPoint.Values[0]) * high / 2);
                area1 += temparea;
                lastPoint = point;
            }
            area2 = (decimal)(systemConfig.Molecular / ((1 + systemConfig.Temperature / 272.15) * 22.4)) * area1;
            ggg = area2 * (decimal)systemConfig.Volume;
            textEdit_area1.Text = area1.ToString("f3");
            textEdit_area2.Text = area2.ToString("f3");
            textEdit_ggg.Text = ggg.ToString("f3");
        }

        /// <summary>
        /// 播放报警函数
        /// </summary>
        /// <param name="isp"></param>
        private void PlaySound(bool isp)
        {
            if (IsClosePlay)
            {
                LogLib.Log.GetLogger(this).Warn("IsClosePlay");
                if (IsSoundPlayed)
                {
                    LogLib.Log.GetLogger(this).Warn("IsSoundPlayed");
                    player.Stop();
                    IsSoundPlayed = false;
                }
                return;
            }

            if (isp)
            {
                if (!IsSoundPlayed)
                {
                    player.PlayLooping();
                    IsSoundPlayed = true;
                }
            }
            else
            {
                if (IsSoundPlayed)
                {
                    player.Stop();
                    IsSoundPlayed = false;
                }
            }
        }

        // 初始化Form
        private bool InitializeForm()
        {
            try
            {
                Gloabl.IsAdmin = false;
                richTextBox1.Dock = DockStyle.None;
                chartControl_Main.Dock = DockStyle.Fill;
                if (!ReadSystemConfig())
                {
                    return false;
                }
                
                // 登录
                //Form_Login login = new Form_Login(systemConfig);
                //login.ShowDialog();
                //if (login.DialogResult != System.Windows.Forms.DialogResult.OK)
                //{
                //    this.Close();
                //}
                //else if (Gloabl.IsAdmin)
                //{
                //    chartControl_Main.Dock = DockStyle.Top;
                //    richTextBox1.Dock = DockStyle.Fill;
                //    richTextBox1.Visible = true;
                //}
                // 设置皮肤                
                defaultLookAndFeel1.LookAndFeel.SkinName = systemConfig.SkinName;
                //DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(systemConfig.SkinName);
                // 设置数据库路径
                SqliteHelper.SetConnectionString(string.Format("Data Source={0};Version=3;", AppDomain.CurrentDomain.BaseDirectory + "\\SDData.db3"));
                //SqliteHelper.SetConnectionString(AppDomain.CurrentDomain.BaseDirectory + "SDData.db");

                mainList = EquipmentDal.GetAllList();
                gridControl_Main.DataSource = mainList;
                gridView_Main.BestFitColumns();
                gridControl_Add.DataSource = mainList;
                gridView_Add.BestFitColumns();

                DateTime time = DateTime.Now;
                dateEdit_Start.DateTime = time.AddDays(-7);
                dateEdit_End.DateTime = time;
                dateEdit_StartAlert.DateTime = time.AddDays(-7);
                dateEdit_EndAlert.DateTime = time;
                InitSeries();

                if (mainList.Count < 1)
                {
                    LogLib.Log.GetLogger(this).Warn("mainList为空");
                    return true;
                }    
                
                mainList.ForEach(c => { comboBoxEdit_ID.Properties.Items.Add(c.Address); });

                Equipment eee = mainList.First();
                if (eee != null)
                {
                    textEdit_AddressAdd.Text = eee.Address;
                    textEdit_GasNameAdd.Text = eee.EName;
                    textEdit_RangeAdd.Text = eee.Range.ToString();
                    comboBoxEdit_UnitAdd.SelectedItem = eee.Unit;
                    comboBoxEdit_PointAdd.SelectedIndex = eee.Point;
                    textEdit_ReviseAdd.Text = eee.Revise.ToString();
                    textEdit_HighAdd.Text = eee.HighAlert.ToString();
                    textEdit_LowAdd.Text = eee.LowAlert.ToString();
                    comboBoxEdit_OpenAdd.SelectedIndex = eee.AlertType;
                    textEdit_PlaceAdd.Text = eee.Place.ToString();
                }
                player = new SoundPlayer();
                player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + systemConfig.SoundName;
                player.Load();                

                Gloabl.IsOpen = true;
                
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
                return false;
            }
            return true;
        }

        // 读取系统配置文件
        private bool ReadSystemConfig()
        {
            try
            {
                XmlSerializerProvider xml = new XmlSerializerProvider();
                SystemConfig fc = xml.Deserialize<SystemConfig>(AppDomain.CurrentDomain.BaseDirectory + "\\SystemConfig.xml");
                if (string.IsNullOrWhiteSpace(fc.BackupPath)
                    || string.IsNullOrWhiteSpace(fc.CenterIP)
                    || string.IsNullOrWhiteSpace(fc.Port)
                    || string.IsNullOrWhiteSpace(fc.SoundName)
                    || fc.Xrange < 1
                    || fc.PreiodUnit < 0
                    || fc.Preiod < 1
                    || fc.CenterPort < 1
                    || fc.BaudRate < 1
                    || fc.BackupPreiod < 1)
                {
                    LogLib.Log.GetLogger(this).Warn("系统配置文件有空值");
                    return false;
                }
                systemConfig = fc;
                comboBoxEdit_Port.Text = systemConfig.Port;
                textEdit_BaudRate.Text = systemConfig.BaudRate.ToString();
                textEdit_Preod.Text = systemConfig.Preiod.ToString();
                comboBoxEdit_second.SelectedIndex = systemConfig.PreiodUnit;
                textEdit_RealTime.Text = systemConfig.Xrange.ToString();
                textEdit_Temperature.Text = systemConfig.Temperature.ToString();
                textEdit_Volume.Text = systemConfig.Volume.ToString();
                textEdit_Molecular.Text = systemConfig.Molecular.ToString();
                textEdit_Path.Text = systemConfig.BackupPath;
                checkEdit_Back.Checked = systemConfig.Isbackup;
                textEdit_BackupPreiod.Text = systemConfig.BackupPreiod.ToString();
                textEdit_CenterIP.Text = systemConfig.CenterIP;
                textEdit_centerPort.Text = systemConfig.CenterPort.ToString();
                checkEdit_autosample.Checked = systemConfig.Isauto;

            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
                return false;
            }
            return true;
        }

        // 初始化曲线
        private void InitSeries()
        {
            chartControl_Main.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            
            chartControl_Main.Series.Clear();
            Equipment ep = new Equipment();
            if (mainList.Count > 0)
            {
                ep = mainList.First();
            }
            seriesOne = new Series(string.Format(ep.EName), ViewType.SwiftPlot);
            seriesOne.Tag = ep.ID;
            seriesOne.ArgumentScaleType = ScaleType.DateTime;
            SwiftPlotSeriesView spsv1 = new SwiftPlotSeriesView();
            spsv1.LineStyle.Thickness = 2;
            seriesOne.View = spsv1;
            chartControl_Main.Series.Add(seriesOne);

            SwiftPlotDiagram diagram_Tem = chartControl_Main.Diagram as SwiftPlotDiagram;
            diagram_Tem.Margins.Right = 15;
            //diagram_Tem.AxisX.
            diagram_Tem.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
            diagram_Tem.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
            diagram_Tem.AxisX.Label.TextPattern = "{A:HH:mm:ss}";
            diagram_Tem.AxisX.VisualRange.AutoSideMargins = false;
            diagram_Tem.AxisX.WholeRange.AutoSideMargins = true;
            diagram_Tem.AxisX.Title.Text = "时间";
            diagram_Tem.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram_Tem.AxisX.Title.Alignment = StringAlignment.Far;
            diagram_Tem.AxisX.Title.Antialiasing = false;
            diagram_Tem.AxisX.Title.Font = new System.Drawing.Font("Tahoma", 8);

            diagram_Tem.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            //diagram_Tem.EnableAxisYZooming = true;
            //diagram_Tem.EnableAxisYScrolling = true;
            diagram_Tem.AxisY.Interlaced = true;
            diagram_Tem.AxisY.VisualRange.AutoSideMargins = true;
            diagram_Tem.AxisY.WholeRange.AutoSideMargins = true;
            diagram_Tem.AxisY.Title.Text = string.Format("浓度({0})", ep.Unit);
            diagram_Tem.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram_Tem.AxisY.Title.Alignment = StringAlignment.Far;
            diagram_Tem.AxisY.Title.Antialiasing = false;
            diagram_Tem.AxisY.Title.Font = new System.Drawing.Font("Tahoma", 8);
            //if (diagram_Tem != null && diagram_Tem.AxisX.DateTimeMeasureUnit == DateTimeMeasurementUnit.Millisecond)
            //    diagram_Tem.AxisX.Range.SetMinMaxValues(minDate, argument);
        }

        // 设置历史曲线
        private void setX(List<EquipmentData> list)
        {
            try
            {
                if (list == null || list.Count < 1)
                {
                    return;
                }

                list.Sort((customer1, customer2) => customer1.AddTime.CompareTo(customer2.AddTime));
                DateTime dt1 = list.First().AddTime;
                DateTime dt2 = list.Last().AddTime;
                TimeSpan ts = dt2 - dt1;

                SwiftPlotDiagram spd = chartControl_History.Diagram as SwiftPlotDiagram;

                if (ts.TotalMinutes < 1)
                {
                    spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
                    spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
                    spd.AxisX.Label.TextPattern = "{A:HH:mm:ss}";
                    spd.AxisX.WholeRange.SetMinMaxValues(dt1, dt2);
                    spd.AxisX.VisualRange.SetMinMaxValues(dt1, dt2);
                }
                // 时间范围在1小时以内
                else if (ts.TotalHours < 1)
                {
                    spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;
                    spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
                    spd.AxisX.Label.TextPattern = "{A:HH:mm}";

                }
                // 1天以内
                else if (ts.TotalDays <= 1)
                {
                    spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;
                    spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
                    spd.AxisX.Label.TextPattern = "{A:HH:mm}";
                }
                // 1个星期以内
                else if (ts.TotalDays < 7)
                {
                    spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour;
                    spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
                    spd.AxisX.Label.TextPattern = "{A:dd HH:mm}";
                }
                // 1个月以内
                else if (ts.TotalDays < 30)
                {
                    spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour;
                    spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
                    spd.AxisX.Label.TextPattern = "{A:yyyy-MM-dd HH:mm}";

                }
                // 1年以内
                else
                {
                    spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                    spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Month;
                    spd.AxisX.Label.TextPattern = "{A:yyyy-MM-dd}";
                }
            }
            catch (Exception e)
            {
                LogLib.Log.GetLogger(this).Warn(e);
            }
        }

        /// <summary>
        /// 获取界面系统配置参数
        /// </summary>
        /// <returns></returns>
        private void SaveSystemConfig()
        {
            try
            {
                systemConfig.Port = comboBoxEdit_Port.Text;
                systemConfig.BaudRate = int.Parse(textEdit_BaudRate.Text);
                systemConfig.Preiod = int.Parse(textEdit_Preod.Text);
                systemConfig.PreiodUnit = comboBoxEdit_second.SelectedIndex;
                systemConfig.Xrange = int.Parse(textEdit_RealTime.Text);
                systemConfig.Temperature = float.Parse(textEdit_Temperature.Text);
                systemConfig.Volume = float.Parse(textEdit_Volume.Text);
                systemConfig.Molecular = int.Parse(textEdit_Molecular.Text);
                systemConfig.BackupPath = textEdit_Path.Text;
                systemConfig.Isbackup = checkEdit_Back.Checked;
                systemConfig.BackupPreiod = int.Parse(textEdit_BackupPreiod.Text);
                systemConfig.CenterIP = textEdit_CenterIP.Text;
                systemConfig.CenterPort = int.Parse(textEdit_centerPort.Text);
                systemConfig.Isauto = checkEdit_autosample.Checked;
                if (PLAASerialPort.serialport.IsOpen)
                {
                    PLAASerialPort.GetInstance().Close();
                    PLAASerialPort.GetInstance().Open(systemConfig.Port, systemConfig.BaudRate);
                    Gloabl.IsOpen = true;
                }
                else
                {
                    PLAASerialPort.GetInstance().Open(systemConfig.Port, systemConfig.BaudRate);
                    Gloabl.IsOpen = true;
                }
                XmlSerializerProvider xml = new XmlSerializerProvider();
                xml.Serialize<SystemConfig>(AppDomain.CurrentDomain.BaseDirectory + "\\SystemConfig.xml", systemConfig);
                XtraMessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存失败");
                LogLib.Log.GetLogger(this).Warn(ex);
            }

        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        private Equipment GetAddEquip()
        {
            Equipment eee = new Equipment();
            eee.Address = textEdit_AddressAdd.Text;
            eee.EName = textEdit_GasNameAdd.Text;
            eee.Range = float.Parse(textEdit_RangeAdd.Text);
            eee.Unit = comboBoxEdit_UnitAdd.Text;
            eee.Point = (byte)comboBoxEdit_PointAdd.SelectedIndex;
            eee.Revise = float.Parse(textEdit_ReviseAdd.Text);
            eee.HighAlert = float.Parse(textEdit_HighAdd.Text);
            eee.LowAlert = float.Parse(textEdit_LowAdd.Text);
            eee.AlertType = (byte)comboBoxEdit_OpenAdd.SelectedIndex;
            eee.Place = textEdit_PlaceAdd.Text.Trim();
            eee.Addtime = DateTime.Now;
            return eee;
        }

        /// <summary>
        /// 刷新设备显示
        /// </summary>
        private void RefreshenAdd()
        {
            gridControl_Main.RefreshDataSource();
            gridView_Main.BestFitColumns();
            gridControl_Add.RefreshDataSource();
            gridView_Add.BestFitColumns();
        }

        private void addText(string txt)
        {
            int MaxLines = 1000;
            //cjComment这部分来的奇怪。应该会自己滚动的
            if (richTextBox1.Lines.Length > MaxLines)
            {
                richTextBox1.Clear();
            }
            richTextBox1.AppendText(txt);
            // 自动滚到底部
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        } 

        #endregion

        public MainForm()
        {
            InitializeComponent();
            
        }

        private void StartLishConnect()
        {
            while (true)
            {
                if (suspend)
                {
                    return;
                }

                foreach (var item in mainList)
                {
                    if (item.lostNum >= 3)
                    {
                        item.IsConnect = false;
                    }
                    else
                    {
                        item.lostNum++;
                    }
                }
                this.Invoke(new Action(gridControl_Main.RefreshDataSource));
                Thread.Sleep(60 * 1000);
            }
            
        }

        private void btn_Start_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (Gloabl.IsOpen == false)
            //{
            //    XtraMessageBox.Show("请先打开串口");
            //    return;
            //}
            if (mainList.Count < 1)
            {
                return;
            }
            minTime = DateTime.Now;
            maxTime = minTime.AddMinutes(systemConfig.Xrange);
            if (seriesOne == null || seriesOne.Tag == null)
            {
                changeSeries(mainList.First());
            }
            else
            {
                changeSeries(mainList.Find(c => c.ID == Convert.ToInt32(seriesOne.Tag)));
            }

            try
            {
                if (tcpserver == null)
                {
                    IPAddress ip = IPAddress.Parse(systemConfig.CenterIP);
                    tcpserver = new AsyncTcpServer(ip, systemConfig.CenterPort);
                    tcpserver.PlaintextReceived += server_PlaintextReceived;
                }

                tcpserver.Start();
            }
            catch
            {
                XtraMessageBox.Show("打开网络连接失败");
                return;
            }
            

            isRead = true;

            if (threadForConnect == null)
            {
                threadForConnect = new Thread(new ThreadStart(StartLishConnect));
                threadForConnect.Start();
            }

            suspend = false;
            
            btn_Start.Enabled = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
            if (!InitializeForm())
            {
                XtraMessageBox.Show("初始化失败");
            }
        }

        private void gridView_Main_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.Name == "gridColumn_Connect" && e.IsGetData)
            {
                if ((e.Row as Equipment).IsConnect)
                {
                    e.Value = Resources.link;
                }
                else
                {
                    e.Value = Resources.off_link;
                }
            }
        }

        private void btn_Stop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PlaySound(false);
            isRead = false;
            suspend = true;
            tcpserver.Stop();
            btn_Start.Enabled = true;
            // 连接状态改为 关闭
            foreach (Equipment eq in mainList)
            {
                eq.IsConnect = false;
            }
            RefreshenAdd();
        }

        private void btn_SearchHistory_Click(object sender, EventArgs e)
        {
            gridControl_History.DataSource = null;
            chartControl_History.Series[0].Points.Clear();


            if (comboBoxEdit_ID.Text.Trim() == string.Empty)
            {
                XtraMessageBox.Show("请选择设备名称");
                return;
            }
            TimeSpan ts = dateEdit_End.DateTime - dateEdit_Start.DateTime;
            TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
            if (ts < ts1)
            {
                XtraMessageBox.Show("截止时间必须大于起始时间");
                return;
            }
            Equipment eq = mainList.Find(c => c.Address == comboBoxEdit_ID.Text);

            List<EquipmentData> data = EquipmentDataDal.GetListByTime(eq.ID, dateEdit_Start.DateTime, dateEdit_End.DateTime);
            if (data == null || data.Count < 1)
            {
                LogLib.Log.GetLogger(this).Warn("数据库中没有记录");
                return;
            }
            gridControl_History.DataSource = data;
            gridView_History.BestFitColumns();
            data.ForEach(c =>
            {
                chartControl_History.Series[0].Points.Add(new SeriesPoint(c.AddTime, c.Chroma));
            });

            float max = data.Max(c => c.Chroma);
            float min = data.Min(c => c.Chroma);

            // 更改曲线纵坐标描述
            SwiftPlotDiagram diagram_Tem = chartControl_History.Diagram as SwiftPlotDiagram;
            diagram_Tem.AxisY.Title.Text = eq.EName + ":" + eq.Unit;

            setX(data);
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog mTempSaveDialog = new SaveFileDialog();
            mTempSaveDialog.Filter = "Excel files (*xls)|*.xls| Excel files (*xlsx)|*.xlsx";
            mTempSaveDialog.RestoreDirectory = true;
            if (DialogResult.OK == mTempSaveDialog.ShowDialog() && null != mTempSaveDialog.FileName.Trim())
            {
                string mTempSavePath = mTempSaveDialog.FileName;
                if (mTempSavePath.Contains("xlsx"))    // 导出07及以上版本的文件
                {
                    DevExpress.XtraPrinting.XlsxExportOptions options = new DevExpress.XtraPrinting.XlsxExportOptions(DevExpress.XtraPrinting.TextExportMode.Value);
                    options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
                    options.ExportMode = DevExpress.XtraPrinting.XlsxExportMode.SingleFile;
                    this.gridView_History.ExportToXlsx(mTempSaveDialog.FileName);
                }
                else if (mTempSavePath.Contains("xls"))  // 导出03版本的文件
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions(DevExpress.XtraPrinting.TextExportMode.Value);
                    options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
                    options.ExportMode = DevExpress.XtraPrinting.XlsExportMode.SingleFile;
                    this.gridView_History.ExportToXls(mTempSaveDialog.FileName, options);
                }
            }
        }

        private void btn_DeleteData_Click(object sender, EventArgs e)
        {
            //if (Gloabl.Userinfo.Level != EM_UserType.Admin)
            //{
            //    XtraMessageBox.Show("只有管理员才能删除数据");
            //    return;
            //}

            if (comboBoxEdit_ID.Text.Trim() == string.Empty)
            {
                XtraMessageBox.Show("请先查询数据");
                return;
            }
            TimeSpan ts = dateEdit_End.DateTime - dateEdit_Start.DateTime;
            TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
            if (ts < ts1)
            {
                XtraMessageBox.Show("请先查询数据");
                return;
            }
            if (XtraMessageBox.Show("数据将要被删除，是否继续", "注意", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            Equipment eq = mainList.Find(c => c.Address == comboBoxEdit_ID.Text);

            int total = EquipmentDataDal.DeleteByTime(eq.ID, dateEdit_Start.DateTime, dateEdit_End.DateTime);
            gridControl_History.DataSource = null;
            chartControl_History.Series[0].Points.Clear();
            XtraMessageBox.Show(string.Format("本次删除{0}条数据", total));
        }

        private void comboBoxEdit_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Equipment ee = mainList.Find(c => c.Address == comboBoxEdit_ID.Text);
            textEdit_GasName.Text = ee.EName;
            textEdit_Place.Text = ee.Place;
        }

        private void btn_Save1_Click(object sender, EventArgs e)
        {
            SaveSystemConfig();
        }

        private void btn_ChoosesPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();

            path.ShowDialog();
            textEdit_Path.Text = path.SelectedPath;
        }

        private void gridView_Add_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                Equipment eee = gridView_Add.GetFocusedRow() as Equipment;
                textEdit_AddressAdd.Text = eee.Address;
                textEdit_GasNameAdd.Text = eee.EName;
                textEdit_RangeAdd.Text = eee.Range.ToString();
                comboBoxEdit_UnitAdd.SelectedItem = eee.Unit;
                comboBoxEdit_PointAdd.SelectedIndex = eee.Point;
                textEdit_ReviseAdd.Text = eee.Revise.ToString();
                textEdit_HighAdd.Text = eee.HighAlert.ToString();
                textEdit_LowAdd.Text = eee.LowAlert.ToString();
                comboBoxEdit_OpenAdd.SelectedIndex = eee.AlertType;
                textEdit_PlaceAdd.Text = eee.Place.ToString();
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }

        }

        private void btn_AddAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Equipment eee = GetAddEquip();
                List<string> adds = EquipmentDal.GetAddress();
                if (adds.Contains(eee.Address))
                {
                    XtraMessageBox.Show("ID重复，请重新输入");
                    return;
                }
                Equipment eee1 = EquipmentDal.AddOneR(eee);
                mainList.Add(eee1);
                RefreshenAdd();
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
        }

        private void btn_UpdateAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Equipment eee = gridView_Add.GetFocusedRow() as Equipment;
                List<string> adds = EquipmentDal.GetAddress();
                // 不能和自己比较
                adds.Remove(eee.Address);
                
                Equipment upd = GetAddEquip();
                if (adds.Contains(upd.Address))
                {
                    XtraMessageBox.Show("ID重复，请重新输入");
                    return;
                }
                eee.Address = upd.Address;
                eee.EName = upd.EName;
                eee.Range = upd.Range;
                eee.Unit = upd.Unit;
                eee.Point = upd.Point;
                eee.Revise = upd.Revise;
                eee.HighAlert = upd.HighAlert;
                eee.LowAlert = upd.LowAlert;
                eee.AlertType = upd.AlertType;
                eee.Place = upd.Place;
                EquipmentDal.UpdateOne(eee);
                RefreshenAdd();

            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
        }

        private void btn_DeleteAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Equipment eee = gridView_Add.GetFocusedRow() as Equipment;
                EquipmentDal.DeleteOne(eee);
                mainList.Remove(eee);
                RefreshenAdd();
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
        }

        private void btn_searchAlert_Click(object sender, EventArgs e)
        {
            TimeSpan ts = dateEdit_EndAlert.DateTime - dateEdit_StartAlert.DateTime;
            TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
            if (ts < ts1)
            {
                XtraMessageBox.Show("截止时间必须大于起始时间");
                return;
            }
            //Equipment eq = mainList.Find(c => c.Name == comboBoxEdit_SensorName.Text.Trim() && c.GasName == comboBoxEdit_GasName.Text.Trim());

            List<Alert> data = AlertDal.GetListByTime(dateEdit_StartAlert.DateTime, dateEdit_EndAlert.DateTime);
            if (data == null || data.Count < 1)
            {
                gridControl_Alert.DataSource = null;
                LogLib.Log.GetLogger(this).Warn("数据库中没有记录");
                return;
            }
            gridControl_Alert.DataSource = data;
            gridView_Alert.BestFitColumns();
        }

        private void btn_ExportAlert_Click(object sender, EventArgs e)
        {
            SaveFileDialog mTempSaveDialog = new SaveFileDialog();
            mTempSaveDialog.Filter = "Excel files (*xls)|*.xls| Excel files (*xlsx)|*.xlsx";
            mTempSaveDialog.RestoreDirectory = true;
            if (DialogResult.OK == mTempSaveDialog.ShowDialog() && null != mTempSaveDialog.FileName.Trim())
            {
                string mTempSavePath = mTempSaveDialog.FileName;
                if (mTempSavePath.Contains("xlsx"))    // 导出07及以上版本的文件
                {
                    DevExpress.XtraPrinting.XlsxExportOptions options = new DevExpress.XtraPrinting.XlsxExportOptions(DevExpress.XtraPrinting.TextExportMode.Value);
                    options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
                    options.ExportMode = DevExpress.XtraPrinting.XlsxExportMode.SingleFile;
                    this.gridView_Alert.ExportToXlsx(mTempSaveDialog.FileName);
                }
                else if (mTempSavePath.Contains("xls"))  // 导出03版本的文件
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions(DevExpress.XtraPrinting.TextExportMode.Value);
                    options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
                    options.ExportMode = DevExpress.XtraPrinting.XlsExportMode.SingleFile;
                    this.gridView_Alert.ExportToXls(mTempSaveDialog.FileName, options);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tcpserver != null)
            {
                if (tcpserver.IsRunning)
                {
                    tcpserver.Stop();
                }
                tcpserver.Dispose();
            }

            if (threadForConnect != null)
            {
                threadForConnect.Abort();
            }
        }

        private void btn_Back_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage1;
        }

        private void btn_History_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage2;
            DateTime time = DateTime.Now;
            dateEdit_Start.DateTime = time.AddDays(-7);
            dateEdit_End.DateTime = time;
        }

        private void btn_ParamSet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!btn_Start.Enabled)
            {
                XtraMessageBox.Show("请先停止检测");
            }
            else
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage3;
            }            
        }

        private void btn_Alert_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage5;
            DateTime time = DateTime.Now;
            dateEdit_StartAlert.DateTime = time.AddDays(-7);
            dateEdit_EndAlert.DateTime = time;
        }

        private void btn_Add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage4;
        }

        private void btnm_Start_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btn_Start_ItemClick(null,null);
        }

        private void btnm_Stop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btn_Stop_ItemClick(null, null);
        }

        private void btnm_ParamSet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btn_ParamSet_ItemClick(null, null);
        }

        private void btnm_Add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btn_Add_ItemClick(null, null);
        }

        private void btnm_History_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btn_History_ItemClick(null, null);
        }

        private void btnm_Alert_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btn_Alert_ItemClick(null, null);
        }

        private void btnm_Exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            } 
        }

        private void btn_Help_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\CTS60有毒有害气体在线监测系统用户使用手册V1.0.1（简化版）.pdf";
                System.Diagnostics.Process.Start(path); //打开此文件。
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("请先安装PDF软件");
                LogLib.Log.GetLogger(this).Warn(ex);
            }
        }

        private void gridView_Main_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                Equipment eee = gridView_Main.GetFocusedRow() as Equipment;
                changeSeries(eee);
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
            
        }

        private void ntn_mute_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btn_mute.Caption == "关闭报警声音")
            {
                IsClosePlay = true;
                btn_mute.Glyph = Resources.mute_32x32;
                btn_mute.Caption = "打开报警声音";
            }
            else if (btn_mute.Caption == "打开报警声音")
            {
                IsClosePlay = false;
                btn_mute.Glyph = Resources.mute_off_32x32;
                btn_mute.Caption = "关闭报警声音";
            }
        }

        private void btnm_Help_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btn_Help_ItemClick(null,null);
        }

        private void btn_Save2_Click(object sender, EventArgs e)
        {
            btn_Save1_Click(null, null);
        }

        private void btn_ModifPass_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btn_ModifPass.Caption == "切换到普通用户")
            {
                richTextBox1.Visible = false;
                Gloabl.IsAdmin = false;
                btn_ModifPass.Caption = "管理员登入";
                richTextBox1.Dock = DockStyle.None;
                chartControl_Main.Dock = DockStyle.Fill;
            }
            else if (btn_ModifPass.Caption == "管理员登入")
            {
                Form_ChangeAdmin fc = new Form_ChangeAdmin();
                if (fc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {                    
                    if ("admin123"!= fc.ValueStr)
                    {
                        XtraMessageBox.Show("密码不正确");
                    }
                    else
                    {
                        chartControl_Main.Dock = DockStyle.Top;
                        richTextBox1.Dock = DockStyle.Fill;
                        Gloabl.IsAdmin = true;
                        richTextBox1.Visible = true;
                        btn_ModifPass.Caption = "切换到普通用户";
                        
                    }
                }
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {
                // 自动启动检测
                if (systemConfig.Isauto)
                {
                    btn_Start_ItemClick(null, null);
                }
                // 登录
                Form_Login login = new Form_Login(systemConfig);
                login.ShowDialog();
                if (login.DialogResult != System.Windows.Forms.DialogResult.OK)
                {
                    this.Close();
                }
                else if (Gloabl.IsAdmin)
                {
                    chartControl_Main.Dock = DockStyle.Top;
                    richTextBox1.Dock = DockStyle.Fill;
                    richTextBox1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
            
            
        }

        private void labelControl31_Click(object sender, EventArgs e)
        {

        }

        private void btn_Save3_Click(object sender, EventArgs e)
        {
            SaveSystemConfig();
        }

       
    }
}