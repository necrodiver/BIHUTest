using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBiHuGadget.Models
{
    /// <summary>
    /// 消息模型类
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string MsgTitle { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public object MsgContent { get; set; }
        /// <summary>
        /// 消息状态
        /// </summary>
        public bool MsgStatus { get; set; }
    }
    /// <summary>
    /// excel表单的用户打卡数据
    /// </summary>
    public class XlsModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 打卡时间
        /// </summary>
        public string PunchCardTime { get; set; }
    }
    /// <summary>
    /// 文件格式对应的编码
    /// </summary>
    public enum FileExtension
    {
        JPG = 255216,
        GIF = 7173,
        BMP = 6677,
        PNG = 13780,
        COM = 7790,
        EXE = 7790,
        DLL = 7790,
        RAR = 8297,
        ZIP = 8075,
        XML = 6063,
        HTML = 6033,
        ASPX = 239187,
        CS = 117115,
        JS = 119105,
        TXT = 210187,
        SQL = 255254,
        BAT = 64101,
        BTSEED = 10056,
        RDP = 255254,
        PSD = 5666,
        PDF = 3780,
        CHM = 7384,
        LOG = 70105,
        REG = 8269,
        HLP = 6395,
        DOC = 208207,
        XLS = 208207,
        DOCX = 208207,
        XLSX = 208207,
    }
    public class User
    {
        public int day { get; set; }
        public string putCaredTime { get; set; }
        /// <summary>
        /// -3：迟到     ,
        /// -2：早退
        /// -1：早回家     ,
        /// 0：未打卡     ,
        /// 1：正常     ,
        /// 2：加班    ,
        /// 100：周天、假期
        /// </summary>
        public int status { get; set; }
        public bool? isLeftTime { get; set; }
        public int dayOfWeek { get; set; }
    }
    public class UserChild
    {
        public string putCaredTime { get; set; }
        ///// <summary>
        ///// -2：迟到  ,   
        ///// -1：早退    , 
        ///// 0：未打卡   ,  
        ///// 1：正常     ,
        ///// 2：加班
        /////
        /// <summary>
        /// -3：迟到     ,
        /// -2：早退       ,
        /// -1：早回家     ,
        /// 0：未打卡     ,
        /// 1：正常     ,
        /// 2：加班    ,
        /// 100：周天、假期
        /// </summary>
        public int status { get; set; }

    }
    public class UserAll
    {
        public int day { get; set; }
        public int dayOfWeek { get; set; }
        /// <summary>
        /// 0：整天未在，1：有打卡记录，2：假期
        /// </summary>
        public int status { get; set; }
        public UserChild UserLeft { get; set; }
        public UserChild UserRight { get; set; }
    }
    public class UpdateData
    {
        public HttpPostedFileBase file { get; set; }
        public string userName { get; set; }
    }
}