using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Buffalo.WebKernel.WebCommons.ContorlCommons;
using System.Drawing;
using Commons;
using Buffalo.WebKernel.WebCommons;

namespace Buffalo.WebControls.KimSoftWebCalendars
{
    [DefaultProperty("CurrentDate")]
    [ValidationProperty("Text")] 
    [ToolboxData("<{0}:KimSoftWebCalendar runat=server></{0}:KimSoftWebCalendar>")]
    public class KimSoftWebCalendar : WebControl, INamingContainer,ITextControl
    {
        private static bool isChecked = false;//是否已经检查过文件
        private const string jsName = "KimSoftWebCalendar.js";

        

        /// <summary>
        /// 文本
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                return txtValue.Text;
            }
            set
            {
                DateTime dt = DateTime.MinValue;
                if (DateTime.TryParse(value, out dt))
                {
                    txtValue.Text = dt.ToString("yyyy-MM-dd");
                }
            }
        }

        

        /// <summary>
        /// 当前日期
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public DateTime CurrentDate
        {
            get
            {
                if (Text == "")
                {
                    return DateTime.MinValue;
                }
                DateTime dt = Convert.ToDateTime(Text);
                return dt;
            }
            set
            {
                Text = value.ToShortDateString();
            }
        }

        [Description("当日日期文字颜色"),
       Category("外观")]
        public Color CurWord 
        {
            get 
            {
                if (ViewState["CurWord"]==null) 
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["CurWord"];
            }
            set 
            {
                ViewState["CurWord"] = value;
            }
        }
        /// <summary>
        /// 当前日期的最后一刻(如2007-1-1就返回2007-1-1 23:59:59 999)
        /// </summary>
        public DateTime CurrentDayLast
        {
            get
            {
                if (this.Text == "")
                {
                    return DateTime.MinValue;
                }
                DateTime dt = Convert.ToDateTime(this.Text);
                dt = dt.AddDays(1).Subtract(TimeSpan.FromMilliseconds(1));
                return dt;
            }
            
        }
        [Description("当日日期单元格背影色"),
       Category("外观")]
        public Color CurBg
        {
            get
            {
                if (ViewState["CurBg"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["CurBg"];
            }
            set
            {
                ViewState["CurBg"] = value;
            }
        }

        [Description("星期天文字颜色"),
       Category("外观")]
        public Color SunWord
        {
            get
            {
                if (ViewState["SunWord"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["SunWord"];
            }
            set
            {
                ViewState["SunWord"] = value;
            }
        }

        [Description("星期六文字颜色"),
       Category("外观")]
        public Color SatWord
        {
            get
            {
                if (ViewState["SatWord"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["SatWord"];
            }
            set
            {
                ViewState["SatWord"] = value;
            }
        }

        [Description("单元格文字颜色"),
       Category("外观")]
        public Color TdWordLight
        {
            get
            {
                if (ViewState["TdWordLight"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TdWordLight"];
            }
            set
            {
                ViewState["TdWordLight"] = value;
            }
        }
        [Description("单元格文字暗色"),
       Category("外观")]
        public Color TdWordDark
        {
            get
            {
                if (ViewState["TdWordDark"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TdWordDark"];
            }
            set
            {
                ViewState["TdWordDark"] = value;
            }
        }
        [Description("单元格背影色"),
       Category("外观")]
        public Color TdBgOut
        {
            get
            {
                if (ViewState["TdBgOut"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TdBgOut"];
            }
            set
            {
                ViewState["TdBgOut"] = value;
            }
        }
        [Description("单元格背影色"),
       Category("外观")]
        public Color TdBgOver
        {
            get
            {
                if (ViewState["TdBgOver"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TdBgOver"];
            }
            set
            {
                ViewState["TdBgOver"] = value;
            }
        }
        [Description("日历头文字颜色"),
       Category("外观")]
        public Color TrWord
        {
            get
            {
                if (ViewState["TrWord"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TrWord"];
            }
            set
            {
                ViewState["TrWord"] = value;
            }
        }
        [Description("日历头背影色"),
       Category("外观")]
        public Color TrBg
        {
            get
            {
                if (ViewState["TrBg"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TrBg"];
            }
            set
            {
                ViewState["TrBg"] = value;
            }
        }
        [Description("input控件的边框颜色"),
       Category("外观")]
        public Color InputBorder
        {
            get
            {
                if (ViewState["InputBorder"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["InputBorder"];
            }
            set
            {
                ViewState["InputBorder"] = value;
            }
        }
        [Description("input控件的背影色"),
       Category("外观")]
        public Color InputBg
        {
            get
            {
                if (ViewState["InputBg"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["InputBg"];
            }
            set
            {
                ViewState["InputBg"] = value;
            }
        }

        [Description("是否中文显示,(true为中文，false为英文)"),
       Category("外观")]
        public bool IsChinese
        {
            get
            {
                if (ViewState["IsChinese"] == null)
                {
                    return true;
                }
                return (bool)ViewState["IsChinese"];
            }
            set
            {
                ViewState["IsChinese"] = value;
            }
        }
        [Description("起始年份"),
       Category("外观")]
        public int BeginYear
        {
            get
            {
                if (ViewState["BeginYear"] == null)
                {
                    return 0;
                }
                return (int)ViewState["BeginYear"];
            }
            set
            {
                ViewState["BeginYear"] = value;
            }
        }

        [Description("结束年份"),
       Category("外观")]
        public int EndYear
        {
            get
            {
                if (ViewState["EndYear"] == null)
                {
                    return 0;
                }
                return (int)ViewState["EndYear"];
            }
            set
            {
                ViewState["EndYear"] = value;
            }
        }


        private TextBox txtValue;

       
        /// <summary>
        /// 容器的客户端ID
        /// </summary>
        public string TextClientID 
        {
            get 
            {
                return txtValue.ClientID;
            }
        }
        /// <summary>
        /// 初始化中转值的控件
        /// </summary>
        private void InitTextControl()
        {
            if (txtValue == null)
            {
                txtValue = new TextBox();

                txtValue.ID = "txtCalendar";
                
            }
        }
        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            //txtProvice.RenderControl(writer);
            //txtCity.RenderControl(writer);
            //txtArea.RenderControl(writer);

            this.Controls.Add(txtValue);
            
            base.CreateChildControls();
            txtValue.Width = this.Width;
            txtValue.Height = this.Height;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //string js=CreateJs();
            
            SaveJs();

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(jsName+"Include"))
            {
                Page.ClientScript.RegisterClientScriptInclude(jsName+"Include", JsSaver.GetDefualtJsUrl(jsName));
            }
            if (!Page.ClientScript.IsStartupScriptRegistered("Init"))
            {
                string commonPanl = "document.write('<div id=\"calendarPanel\" style=\"position: absolute;visibility: hidden;z-index: 9999;background-color: #FFFFFF;border: 1px solid #CCCCCC;width:175px;font-size:12px;\"></div>');\n";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Init", commonPanl, true);
            }
            string lan = "1";//语言
            if (IsChinese) 
            {
                lan = "0";
            }
            string showScript = "new Calendar(" + BeginYear.ToString() + ", " + EndYear.ToString() + ", " + lan + ",'yyyy-MM-dd','" +
                ContorlCommon.ToColorString(CurWord) + "','" + ContorlCommon.ToColorString(CurBg) + "','" + ContorlCommon.ToColorString(SunWord) + "','" +
                ContorlCommon.ToColorString(SatWord) + "','" + ContorlCommon.ToColorString(TdWordLight) + "','" + ContorlCommon.ToColorString(TdWordDark) + "','" +
                ContorlCommon.ToColorString(TdBgOut) + "','" + ContorlCommon.ToColorString(TdBgOver) + "','" + ContorlCommon.ToColorString(TrWord) + "'" +
                ",'" + ContorlCommon.ToColorString(TrBg) + "','" + ContorlCommon.ToColorString(InputBorder) + "','" + ContorlCommon.ToColorString(InputBg) + "').show(this,null);";
            txtValue.Attributes.Add("onclick", showScript);
            txtValue.Attributes.Add("readonly", "readonly");
            
        }



        /// <summary>
        /// 保存成JS文件
        /// </summary>
        private void SaveJs()
        {
            if (!isChecked)
            {
                JsSaver.SaveJS(jsName, CreateJs());
                isChecked = true;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        public KimSoftWebCalendar()
        {
            
            IsChinese = true;
            BeginYear = 1900;
            EndYear = 2100;

            CurWord = ContorlCommon.ColorStringToColor("#FFFFFF");
            CurBg = ContorlCommon.ColorStringToColor("#00FF00");
            SunWord = ContorlCommon.ColorStringToColor("#FF0000");
            SatWord = ContorlCommon.ColorStringToColor("#0000FF");
            TdWordLight = ContorlCommon.ColorStringToColor("#333333");
            TdWordDark = ContorlCommon.ColorStringToColor("#CCCCCC");
            TdBgOut = ContorlCommon.ColorStringToColor("#EFEFEF");
            TdBgOver = ContorlCommon.ColorStringToColor("#FFCC00");
            TrWord = ContorlCommon.ColorStringToColor("#FFFFFF");
            TrBg = ContorlCommon.ColorStringToColor("#666666");
            InputBorder = ContorlCommon.ColorStringToColor("#CCCCCC");
            InputBg = ContorlCommon.ColorStringToColor("#EFEFEF");
            this.Width = 100;
            this.Height = 20;
            InitTextControl();

           // ((WebControl)this.Page.FindControl(this.ID)).Height = 20;
        }


        
        /// <summary>
        /// 创建日期的JS脚本
        /// </summary>
        /// <returns></returns>
        private string CreateJs()
        {
            StringBuilder sb = new StringBuilder(28468);
            sb.Append("<!--\n");
            sb.Append("/**\n");
            sb.Append("* 返回日期\n");
            sb.Append("* @param d the delimiter\n");
            sb.Append("* @param p the pattern of your date\n");
            sb.Append("*/\n");
            sb.Append("String.prototype.toDate = function(x, p) {\n");
            sb.Append("if(x == null) x = \"-\";\n");
            sb.Append("if(p == null) p = \"ymd\";\n");
            sb.Append("var a = this.split(x);\n");
            sb.Append("var y = parseInt(a[p.indexOf(\"y\")]);\n");
            sb.Append("//remember to change this next century ;)\n");
            sb.Append("if(y.toString().length <= 2) y += 2000;\n");
            sb.Append("if(isNaN(y)) y = new Date().getFullYear();\n");
            sb.Append("var m = parseInt(a[p.indexOf(\"m\")]) - 1;\n");
            sb.Append("var d = parseInt(a[p.indexOf(\"d\")]);\n");
            sb.Append("if(isNaN(d)) d = 1;\n");
            sb.Append("return new Date(y, m, d);\n");
            sb.Append("}\n");
            sb.Append("/**\n");
            sb.Append("* 格式化日期\n");
            sb.Append("* @param   d the delimiter\n");
            sb.Append("* @param   p the pattern of your date\n");
            sb.Append("* @author meizz\n");
            sb.Append("*/\n");
            sb.Append("Date.prototype.format = function(style) {\n");
            sb.Append("var o = {\n");
            sb.Append("\"M+\" : this.getMonth() + 1, //month\n");
            sb.Append("\"d+\" : this.getDate(),      //day\n");
            sb.Append("\"h+\" : this.getHours(),     //hour\n");
            sb.Append("\"m+\" : this.getMinutes(),   //minute\n");
            sb.Append("\"s+\" : this.getSeconds(),   //second\n");
            sb.Append("\"w+\" : \"天一二三四五六\".charAt(this.getDay()),   //week\n");
            sb.Append("\"q+\" : Math.floor((this.getMonth() + 3) / 3), //quarter\n");
            sb.Append("\"S\" : this.getMilliseconds() //millisecond\n");
            sb.Append("}\n");
            sb.Append("if(/(y+)/.test(style)) {\n");
            sb.Append("style = style.replace(RegExp.$1,\n");
            sb.Append("(this.getFullYear() + \"\").substr(4 - RegExp.$1.length));\n");
            sb.Append("}\n");
            sb.Append("for(var k in o){\n");
            sb.Append("if(new RegExp(\"(\"+ k +\")\").test(style)){\n");
            sb.Append("style = style.replace(RegExp.$1,\n");
            sb.Append("RegExp.$1.length == 1 ? o[k] :\n");
            sb.Append("(\"00\" + o[k]).substr((\"\" + o[k]).length));\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("return style;\n");
            sb.Append("};\n");
            sb.Append("/**\n");
            sb.Append("* 日历类\n");
            sb.Append("* @param   beginYear 1990\n");
            sb.Append("* @param   endYear   2010\n");
            sb.Append("* @param   lang      0(中文)|1(英语) 可自由扩充\n");
            sb.Append("* @param   dateFormatStyle \"yyyy-MM-dd\";\n");
            sb.Append("* @version 2006-04-01\n");
            sb.Append("* @author KimSoft (jinqinghua [at] gmail.com)\n");
            sb.Append("* @update\n");
            sb.Append("*/\n");
            sb.Append("function Calendar(beginYear, endYear, lang, dateFormatStyle,curWord,curBg,sunWord,\n");
            sb.Append("satWord,tdWordLight,tdWordDark,tdBgOut,tdBgOver,trWord,trBg,inputBorder,inputBg) {\n");
            sb.Append("this.beginYear = beginYear;\n");
            sb.Append("this.endYear = endYear;\n");
            sb.Append("this.lang = 0;            //0(中文) | 1(英文)\n");
            sb.Append("this.dateFormatStyle = \"yyyy-MM-dd\";\n");
            sb.Append("if (beginYear != null && endYear != null){\n");
            sb.Append("this.beginYear = beginYear;\n");
            sb.Append("this.endYear = endYear;\n");
            sb.Append("}\n");
            sb.Append("if (lang != null){\n");
            sb.Append("this.lang = lang\n");
            sb.Append("}\n");
            sb.Append("if (dateFormatStyle != null){\n");
            sb.Append("this.dateFormatStyle = dateFormatStyle\n");
            sb.Append("}\n");
            sb.Append("this.dateControl = null;\n");
            sb.Append("this.panel = this.getElementById(\"calendarPanel\");\n");
            sb.Append("this.form = null;\n");
            sb.Append("this.date = new Date();\n");
            sb.Append("this.year = this.date.getFullYear();\n");
            sb.Append("this.month = this.date.getMonth();\n");
            sb.Append("this.colors = {\n");
            sb.Append("\"cur_word\"      : curWord, //当日日期文字颜色\n");
            sb.Append("\"cur_bg\"        : curBg, //当日日期单元格背影色\n");
            sb.Append("\"sun_word\"      : sunWord, //星期天文字颜色\n");
            sb.Append("\"sat_word\"      : satWord, //星期六文字颜色\n");
            sb.Append("\"td_word_light\" : tdWordLight, //单元格文字颜色\n");
            sb.Append("\"td_word_dark\" : tdWordDark, //单元格文字暗色\n");
            sb.Append("\"td_bg_out\"     : tdBgOut, //单元格背影色\n");
            sb.Append("\"td_bg_over\"    : tdBgOver, //单元格背影色\n");
            sb.Append("\"tr_word\"       : trWord, //日历头文字颜色\n");
            sb.Append("\"tr_bg\"         : trBg, //日历头背影色\n");
            sb.Append("\"input_border\" : inputBorder, //input控件的边框颜色\n");
            sb.Append("\"input_bg\"      : inputBg   //input控件的背影色\n");
            sb.Append("}\n");
            sb.Append("this.draw();\n");
            sb.Append("this.bindYear();\n");
            sb.Append("this.bindMonth();\n");
            sb.Append("this.changeSelect();\n");
            sb.Append("this.bindData();\n");
            sb.Append("}\n");
            sb.Append("/**\n");
            sb.Append("* 日历类属性（语言包，可自由扩展）\n");
            sb.Append("*/\n");
            sb.Append("Calendar.language = {\n");
            sb.Append("\"year\"   : [[\"\"], [\"\"]],\n");
            sb.Append("\"months\" : [[\"一月\",\"二月\",\"三月\",\"四月\",\"五月\",\"六月\",\"七月\",\"八月\",\"九月\",\"十月\",\"十一月\",\"十二月\"],\n");
            sb.Append("[\"JAN\",\"FEB\",\"MAR\",\"APR\",\"MAY\",\"JUN\",\"JUL\",\"AUG\",\"SEP\",\"OCT\",\"NOV\",\"DEC\"]\n");
            sb.Append("],\n");
            sb.Append("\"weeks\" : [[\"日\",\"一\",\"二\",\"三\",\"四\",\"五\",\"六\"],\n");
            sb.Append("[\"SUN\",\"MON\",\"TUR\",\"WED\",\"THU\",\"FRI\",\"SAT\"]\n");
            sb.Append("],\n");
            sb.Append("\"clear\" : [[\"清空\"], [\"CLS\"]],\n");
            sb.Append("\"today\" : [[\"今天\"], [\"TODAY\"]],\n");
            sb.Append("\"close\" : [[\"关闭\"], [\"CLOSE\"]]\n");
            sb.Append("}\n");
            sb.Append("Calendar.prototype.draw = function() {\n");
            sb.Append("calendar = this;\n");
            sb.Append("var mvAry = [];\n");
            sb.Append("if(document.forms.length<=0)\n");
            sb.Append("{\n");
            sb.Append("    mvAry[mvAry.length] = ' <form name=\"calendarForm\" style=\"margin: 0px;\">';\n");
            sb.Append("}\n");
            sb.Append("mvAry[mvAry.length] = '    <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"1\">';\n");
            sb.Append("mvAry[mvAry.length] = '      <tr>';\n");
            sb.Append("mvAry[mvAry.length] = '        <th align=\"left\" width=\"1%\"><input style=\"border: 1px solid ' + calendar.colors[\"input_border\"] + ';background-color:' + calendar.colors[\"input_bg\"] + ';width:16px;height:20px;\" name=\"prevMonth\" type=\"button\" id=\"prevMonth\" value=\"&lt;\" /></th>';\n");
            sb.Append("mvAry[mvAry.length] = '        <th align=\"center\" width=\"98%\" nowrap=\"nowrap\"><select name=\"calendarYear\" id=\"calendarYear\" style=\"font-size:12px;\"></select><select name=\"calendarMonth\" id=\"calendarMonth\" style=\"font-size:12px;\"></select></th>';\n");
            sb.Append("mvAry[mvAry.length] = '        <th align=\"right\" width=\"1%\"><input style=\"border: 1px solid ' + calendar.colors[\"input_border\"] + ';background-color:' + calendar.colors[\"input_bg\"] + ';width:16px;height:20px;\" name=\"nextMonth\" type=\"button\" id=\"nextMonth\" value=\"&gt;\" /></th>';\n");
            sb.Append("mvAry[mvAry.length] = '      </tr>';\n");
            sb.Append("mvAry[mvAry.length] = '    </table>';\n");
            sb.Append("mvAry[mvAry.length] = '    <table id=\"calendarTable\" width=\"100%\" style=\"border:0px solid #CCCCCC;background-color:#FFFFFF\" border=\"0\" cellpadding=\"3\" cellspacing=\"1\">';\n");
            sb.Append("mvAry[mvAry.length] = '      <tr>';\n");
            sb.Append("for(var i = 0; i < 7; i++) {\n");
            sb.Append("mvAry[mvAry.length] = '      <th style=\"font-weight:normal;background-color:' + calendar.colors[\"tr_bg\"] + ';color:' + calendar.colors[\"tr_word\"] + ';\">' + Calendar.language[\"weeks\"][this.lang][i] + '</th>';\n");
            sb.Append("}\n");
            sb.Append("mvAry[mvAry.length] = '      </tr>';\n");
            sb.Append("for(var i = 0; i < 6;i++){\n");
            sb.Append("mvAry[mvAry.length] = '    <tr align=\"center\">';\n");
            sb.Append("for(var j = 0; j < 7; j++) {\n");
            sb.Append("if (j == 0){\n");
            sb.Append("mvAry[mvAry.length] = ' <td style=\"cursor:default;color:' + calendar.colors[\"sun_word\"] + ';\"></td>';\n");
            sb.Append("} else if(j == 6) {\n");
            sb.Append("mvAry[mvAry.length] = ' <td style=\"cursor:default;color:' + calendar.colors[\"sat_word\"] + ';\"></td>';\n");
            sb.Append("} else {\n");
            sb.Append("mvAry[mvAry.length] = ' <td style=\"cursor:default;\"></td>';\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("mvAry[mvAry.length] = '    </tr>';\n");
            sb.Append("}\n");
            sb.Append("mvAry[mvAry.length] = '      <tr style=\"background-color:' + calendar.colors[\"input_bg\"] + ';\">';\n");
            sb.Append("mvAry[mvAry.length] = '        <th colspan=\"2\"><input name=\"calendarClear\" type=\"button\" id=\"calendarClear\" value=\"' + Calendar.language[\"clear\"][this.lang] + '\" style=\"border: 1px solid ' + calendar.colors[\"input_border\"] + ';background-color:' + calendar.colors[\"input_bg\"] + ';width:100%;height:20px;font-size:12px;\"/></th>';\n");
            sb.Append("mvAry[mvAry.length] = '        <th colspan=\"3\"><input name=\"calendarToday\" type=\"button\" id=\"calendarToday\" value=\"' + Calendar.language[\"today\"][this.lang] + '\" style=\"border: 1px solid ' + calendar.colors[\"input_border\"] + ';background-color:' + calendar.colors[\"input_bg\"] + ';width:100%;height:20px;font-size:12px;\"/></th>';\n");
            sb.Append("mvAry[mvAry.length] = '        <th colspan=\"2\"><input name=\"calendarClose\" type=\"button\" id=\"calendarClose\" value=\"' + Calendar.language[\"close\"][this.lang] + '\" style=\"border: 1px solid ' + calendar.colors[\"input_border\"] + ';background-color:' + calendar.colors[\"input_bg\"] + ';width:100%;height:20px;font-size:12px;\"/></th>';\n");
            sb.Append("mvAry[mvAry.length] = '      </tr>';\n");
            sb.Append("mvAry[mvAry.length] = '    </table>';\n");
            sb.Append("if(document.forms.length<=0)\n");
            sb.Append("{\n");
            sb.Append("    mvAry[mvAry.length] = ' </form>';\n");
            sb.Append("}\n");
            sb.Append("this.panel.innerHTML = mvAry.join(\"\");\n");
            sb.Append("this.form = document.forms[0];\n");
            sb.Append("this.form.prevMonth.onclick = function () {calendar.goPrevMonth(this);}\n");
            sb.Append("this.form.nextMonth.onclick = function () {calendar.goNextMonth(this);}\n");
            sb.Append("this.form.calendarClear.onclick = function () {calendar.dateControl.value = \"\";calendar.hide();}\n");
            sb.Append("this.form.calendarClose.onclick = function () {calendar.hide();}\n");
            sb.Append("this.form.calendarYear.onchange = function () {calendar.update(this);}\n");
            sb.Append("this.form.calendarMonth.onchange = function () {calendar.update(this);}\n");
            sb.Append("this.form.calendarToday.onclick = function () {\n");
            sb.Append("var today = new Date();\n");
            sb.Append("calendar.date = today;\n");
            sb.Append("calendar.year = today.getFullYear();\n");
            sb.Append("calendar.month = today.getMonth();\n");
            sb.Append("calendar.changeSelect();\n");
            sb.Append("calendar.bindData();\n");
            sb.Append("calendar.dateControl.value = today.format(calendar.dateFormatStyle);\n");
            sb.Append("calendar.hide();\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("//年份下拉框绑定数据\n");
            sb.Append("Calendar.prototype.bindYear = function() {\n");
            sb.Append("var cy = this.form.calendarYear;\n");
            sb.Append("cy.length = 0;\n");
            sb.Append("for (var i = this.beginYear; i <= this.endYear; i++){\n");
            sb.Append("cy.options[cy.length] = new Option(i + Calendar.language[\"year\"][this.lang], i);\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("//月份下拉框绑定数据\n");
            sb.Append("Calendar.prototype.bindMonth = function() {\n");
            sb.Append("var cm = this.form.calendarMonth;\n");
            sb.Append("cm.length = 0;\n");
            sb.Append("for (var i = 0; i < 12; i++){\n");
            sb.Append("cm.options[cm.length] = new Option(Calendar.language[\"months\"][this.lang][i], i);\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("//向前一月\n");
            sb.Append("Calendar.prototype.goPrevMonth = function(e){\n");
            sb.Append("if (this.year == this.beginYear && this.month == 0){return;}\n");
            sb.Append("this.month--;\n");
            sb.Append("if (this.month == -1) {\n");
            sb.Append("this.year--;\n");
            sb.Append("this.month = 11;\n");
            sb.Append("}\n");
            sb.Append("this.date = new Date(this.year, this.month, 1);\n");
            sb.Append("this.changeSelect();\n");
            sb.Append("this.bindData();\n");
            sb.Append("}\n");
            sb.Append("//向后一月\n");
            sb.Append("Calendar.prototype.goNextMonth = function(e){\n");
            sb.Append("if (this.year == this.endYear && this.month == 11){return;}\n");
            sb.Append("this.month++;\n");
            sb.Append("if (this.month == 12) {\n");
            sb.Append("this.year++;\n");
            sb.Append("this.month = 0;\n");
            sb.Append("}\n");
            sb.Append("this.date = new Date(this.year, this.month, 1);\n");
            sb.Append("this.changeSelect();\n");
            sb.Append("this.bindData();\n");
            sb.Append("}\n");
            sb.Append("//改变SELECT选中状态\n");
            sb.Append("Calendar.prototype.changeSelect = function() {\n");
            sb.Append("var cy = this.form.calendarYear;\n");
            sb.Append("var cm = this.form.calendarMonth;\n");
            sb.Append("for (var i= 0; i < cy.length; i++){\n");
            sb.Append("if (cy.options[i].value == this.date.getFullYear()){\n");
            sb.Append("cy[i].selected = true;\n");
            sb.Append("break;\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("for (var i= 0; i < cm.length; i++){\n");
            sb.Append("if (cm.options[i].value == this.date.getMonth()){\n");
            sb.Append("cm[i].selected = true;\n");
            sb.Append("break;\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("//更新年、月\n");
            sb.Append("Calendar.prototype.update = function (e){\n");
            sb.Append("this.year = e.form.calendarYear.options[e.form.calendarYear.selectedIndex].value;\n");
            sb.Append("this.month = e.form.calendarMonth.options[e.form.calendarMonth.selectedIndex].value;\n");
            sb.Append("this.date = new Date(this.year, this.month, 1);\n");
            sb.Append("this.changeSelect();\n");
            sb.Append("this.bindData();\n");
            sb.Append("}\n");
            sb.Append("//绑定数据到月视图\n");
            sb.Append("Calendar.prototype.bindData = function () {\n");
            sb.Append("var calendar = this;\n");
            sb.Append("var dateArray = this.getMonthViewArray(this.date.getYear(), this.date.getMonth());\n");
            sb.Append("var tds = this.getElementById(\"calendarTable\").getElementsByTagName(\"td\");\n");
            sb.Append("for(var i = 0; i < tds.length; i++) {\n");
            sb.Append("//tds[i].style.color = calendar.colors[\"td_word_light\"];\n");
            sb.Append("tds[i].style.backgroundColor = calendar.colors[\"td_bg_out\"];\n");
            sb.Append("tds[i].onclick = function () {return;}\n");
            sb.Append("tds[i].onmouseover = function () {return;}\n");
            sb.Append("tds[i].onmouseout = function () {return;}\n");
            sb.Append("if (i > dateArray.length - 1) break;\n");
            sb.Append("tds[i].innerHTML = dateArray[i];\n");
            sb.Append("if (dateArray[i] != \"&nbsp;\"){\n");
            sb.Append("tds[i].onclick = function () {\n");
            sb.Append("if (calendar.dateControl != null){\n");
            sb.Append("calendar.dateControl.value = new Date(calendar.date.getFullYear(),\n");
            sb.Append("calendar.date.getMonth(),\n");
            sb.Append("this.innerHTML).format(calendar.dateFormatStyle);\n");
            sb.Append("}\n");
            sb.Append("calendar.hide();\n");
            sb.Append("}\n");
            sb.Append("tds[i].onmouseover = function () {\n");
            sb.Append("this.style.backgroundColor = calendar.colors[\"td_bg_over\"];\n");
            sb.Append("}\n");
            sb.Append("tds[i].onmouseout = function () {\n");
            sb.Append("this.style.backgroundColor = calendar.colors[\"td_bg_out\"];\n");
            sb.Append("}\n");
            sb.Append("if (new Date().format(calendar.dateFormatStyle) ==\n");
            sb.Append("new Date(calendar.date.getFullYear(),\n");
            sb.Append("calendar.date.getMonth(),\n");
            sb.Append("dateArray[i]).format(calendar.dateFormatStyle)) {\n");
            sb.Append("//tds[i].style.color = calendar.colors[\"cur_word\"];\n");
            sb.Append("tds[i].style.backgroundColor = calendar.colors[\"cur_bg\"];\n");
            sb.Append("tds[i].onmouseover = function () {\n");
            sb.Append("this.style.backgroundColor = calendar.colors[\"td_bg_over\"];\n");
            sb.Append("}\n");
            sb.Append("tds[i].onmouseout = function () {\n");
            sb.Append("this.style.backgroundColor = calendar.colors[\"cur_bg\"];\n");
            sb.Append("}\n");
            sb.Append("}//end if\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("//根据年、月得到月视图数据(数组形式)\n");
            sb.Append("Calendar.prototype.getMonthViewArray = function (y, m) {\n");
            sb.Append("var mvArray = [];\n");
            sb.Append("var dayOfFirstDay = new Date(y, m, 1).getDay();\n");
            sb.Append("var daysOfMonth = new Date(y, m + 1, 0).getDate();\n");
            sb.Append("for (var i = 0; i < 42; i++) {\n");
            sb.Append("mvArray[i] = \"&nbsp;\";\n");
            sb.Append("}\n");
            sb.Append("for (var i = 0; i < daysOfMonth; i++){\n");
            sb.Append("mvArray[i + dayOfFirstDay] = i + 1;\n");
            sb.Append("}\n");
            sb.Append("return mvArray;\n");
            sb.Append("}\n");
            sb.Append("//扩展 document.getElementById(id) 多浏览器兼容性 from meizz tree source\n");
            sb.Append("Calendar.prototype.getElementById = function(id){\n");
            sb.Append("if (typeof(id) != \"string\" || id == \"\") return null;\n");
            sb.Append("if (document.getElementById) return document.getElementById(id);\n");
            sb.Append("if (document.all) return document.all(id);\n");
            sb.Append("try {return eval(id);} catch(e){ return null;}\n");
            sb.Append("}\n");
            sb.Append("//扩展 object.getElementsByTagName(tagName)\n");
            sb.Append("Calendar.prototype.getElementsByTagName = function(object, tagName){\n");
            sb.Append("if (document.getElementsByTagName) return document.getElementsByTagName(tagName);\n");
            sb.Append("if (document.all) return document.all.tags(tagName);\n");
            sb.Append("}\n");
            sb.Append("//取得HTML控件绝对位置\n");
            sb.Append("Calendar.prototype.getAbsPoint = function (e){\n");
            sb.Append("var x = e.offsetLeft;\n");
            sb.Append("var y = e.offsetTop;\n");
            sb.Append("while(e = e.offsetParent){\n");
            sb.Append("x += e.offsetLeft;\n");
            sb.Append("y += e.offsetTop;\n");
            sb.Append("}\n");
            sb.Append("return {\"x\": x, \"y\": y};\n");
            sb.Append("}\n");
            sb.Append("//显示日历\n");
            sb.Append("Calendar.prototype.show = function (dateControl, popControl) {\n");
            sb.Append("if (dateControl == null){\n");
            sb.Append("throw new Error(\"arguments[0] is necessary\")\n");
            sb.Append("}\n");
            sb.Append("this.dateControl = dateControl;\n");
            sb.Append("if (dateControl.value.length > 0){\n");
            sb.Append("this.date = new Date(dateControl.value.toDate());\n");
            sb.Append("this.year = this.date.getFullYear();\n");
            sb.Append("this.month = this.date.getMonth();\n");
            sb.Append("this.changeSelect();\n");
            sb.Append("this.bindData();\n");
            sb.Append("}\n");
            sb.Append("if (popControl == null){\n");
            sb.Append("popControl = dateControl;\n");
            sb.Append("}\n");
            sb.Append("var xy = this.getAbsPoint(popControl);\n");
            sb.Append("this.panel.style.left = xy.x + \"px\";\n");
            sb.Append("this.panel.style.top = (xy.y + dateControl.offsetHeight) + \"px\";\n");
            sb.Append("this.setDisplayStyle(\"select\", \"hidden\");\n");
            sb.Append("this.panel.style.visibility = \"visible\";\n");
            sb.Append("\n");
            sb.Append("}\n");
            sb.Append("//隐藏日历\n");
            sb.Append("Calendar.prototype.hide = function() {\n");
            sb.Append("this.setDisplayStyle(\"select\", \"visible\");\n");
            sb.Append("this.panel.style.visibility = \"hidden\";\n");
            sb.Append("}\n");
            sb.Append("//设置控件显示或隐藏\n");
            sb.Append("Calendar.prototype.setDisplayStyle = function(tagName, style) {\n");
            sb.Append("var tags = this.getElementsByTagName(null, tagName)\n");
            sb.Append("for(var i = 0; i < tags.length; i++) {\n");
            sb.Append("if (tagName.toLowerCase() == \"select\" &&\n");
            sb.Append("(tags[i].name == \"calendarYear\" ||\n");
            sb.Append("tags[i].name == \"calendarMonth\")){\n");
            sb.Append("continue;\n");
            sb.Append("}\n");
            sb.Append("tags[i].style.visibility = style;\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("\n");
            return sb.ToString();
        }

        

        
    }
}
