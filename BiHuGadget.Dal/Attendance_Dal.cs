using BiHuGadget.Helpers;
using BiHuGadget.Models;
using Dapper;
using System.Text;
using System;
using System.Collections.Generic;

namespace BiHuGadget.Dal
{
    public class Attendance_Dal : Base_Dal
    {
        public AttendanceModel GetSingleAttendance(AttendanceModel adModel)
        {
            string _where = string.Empty;
            var _args = DisposeAttendanceModel(adModel, true, out _where);
            if (_where == null)
                return null;
            string _sql = string.Format(AD_QueryString.Select_Attendance, _where);
            return GetSingle<AttendanceModel>(_sql, _args);
        }

        public bool AddAttendance(AttendanceModel adModel)
        {
            return Operate(AD_QueryString.insert_Attendance, adModel);
        }
        public bool DeleteAttendance(int attendanceId)
        {
            string _where = " AttendanceId=@AttendanceId ";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@AttendanceId", attendanceId);
            string _sql = string.Format(AD_QueryString.delete_Attendance, _where);
            return Operate(_sql, dp);
        }

        public List<AttendanceModel> GetUserMarks(AttendanceSearchModel asModel)
        {
            try
            {
                var args = new DynamicParameters();
                StringBuilder sb = new StringBuilder();
                if (asModel.UserId != null)
                {
                    sb.Append(" u.UserId=@UserId AND");
                    args.Add("@UserId", asModel.UserId);
                }
                if (!string.IsNullOrWhiteSpace(asModel.UserName))
                {
                    sb.Append(" u.UserName=@UserName AND");
                    args.Add("@UserName", asModel.UserName);
                }
                sb.Append(" a.ClockYear=@ClockYear AND");
                args.Add("@ClockYear", asModel.ClockYear);

                sb.Append(" 1=1");
                string _where = sb.ToString();
                string _sql = $@"SELECT a.* FROM attendance AS a
                            JOIN users AS u ON a.UserId=u.UserId
                            WHERE {_where}";

                return GetList<AttendanceModel>(_sql, args);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("读取用户打卡备注集合：" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 获取分页打卡备注数据
        /// </summary>
        /// <param name="asModel"></param>
        /// <returns></returns>
        public List<AttendanceMoreModel> GetAttendanceList(AttendanceSearchAllModel asModel)
        {
            try
            {
                var args = new DynamicParameters();
                StringBuilder sb = new StringBuilder();
                if (asModel.UserId.HasValue)
                {
                    sb.Append(" u.UserId=@UserId AND");
                    args.Add("@UserId", asModel.UserId);
                }
                if (!string.IsNullOrWhiteSpace(asModel.UserName))
                {
                    sb.Append(" u.UserName=@UserName AND");
                    args.Add("@UserName", asModel.UserName);
                }
                if (asModel.ClockMonth.HasValue)
                {
                    sb.Append(" a.ClockMonth=@ClockMonth AND");
                    args.Add("@ClockMonth", asModel.ClockMonth);
                }

                sb.Append(" a.ClockYear=@ClockYear AND");
                args.Add("@ClockYear", asModel.ClockYear);

                sb.Append(" 1=1");
                string _where = sb.ToString();
                string _sql = $@"SELECT u.UserName,a.* FROM attendance AS a
                            JOIN users AS u ON a.UserId=u.UserId
                            WHERE {_where} Limit {asModel.LeftNum},{asModel.PageCount}";

                return GetList<AttendanceMoreModel>(_sql, args);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("读取用户打卡备注集合：" + ex.ToString());
            }
            return null;
        }

        public bool UpdateAttendance(AttendanceModel adModel)
        {
            string _content = string.Empty;
            var _args = DisposeAttendanceModel(adModel, false, out _content);
            if (_content == null)
            {
                Log4NetHelper.Info("查询UpdateAttendance-where条件为空");
                return false;
            }
            string _where = " AttendanceId=@AttendanceId";
            string _sql = string.Format(AD_QueryString.update_Attendance, _content, _where);
            return Operate(_sql, _args);
        }
        public bool EditAttendanceIsPass(int attendanceId, int isPass)
        {
            if (attendanceId < 0 || isPass <= 0)
                return false;
            string _value = " IsPass=" + isPass;
            string _where = " AttendanceId=" + attendanceId;
            string _sql = string.Format(AD_QueryString.update_Attendance, _value, _where);
            return Operate(_sql);
        }

        /// <summary>
        /// 处理Attendance，拼接where条件
        /// </summary>
        /// <param name="adModel"></param>
        /// <param name="where">返回条件sql语句</param>
        /// <returns></returns>
        private DynamicParameters DisposeAttendanceModel(AttendanceModel adModel, bool isSelect, out string where)
        {
            var args = new DynamicParameters();
            if (adModel == null)
            {
                where = null;
                return null;
            }
            StringBuilder sbSelect = new StringBuilder();
            StringBuilder sbUpdate = new StringBuilder();
            if (adModel.AttendanceId != null)
            {
                sbSelect.Append(" AttendanceId=@AttendanceId AND");
                if (isSelect)
                    sbUpdate.Append(" AttendanceId=@AttendanceId,");
                args.Add("@AttendanceId", adModel.AttendanceId);
            }
            if (!string.IsNullOrWhiteSpace(adModel.ClockContent))
            {
                sbSelect.Append(" ClockContent=@ClockContent AND");
                sbUpdate.Append(" ClockContent=@ClockContent,");
                args.Add("@ClockContent", adModel.ClockContent);
            }
            if (adModel.ClockTime != null)
            {
                sbSelect.Append(" ClockTime=@ClockTime AND");
                sbUpdate.Append(" ClockTime=@ClockTime,");
                args.Add("@ClockTime", adModel.ClockTime);
            }
            if (adModel.IsPass.HasValue)
            {
                sbSelect.Append(" IsPass=@IsPass AND");
                sbUpdate.Append(" IsPass=@IsPass,");
                args.Add("@IsPass", adModel.IsPass);
            }

            sbSelect.Append(" ClockYear=@ClockYear AND");
            sbUpdate.Append(" ClockYear=@ClockYear,");
            args.Add("@ClockYear", adModel.ClockYear);

            sbSelect.Append(" TimeSlot=@TimeSlot AND");
            sbUpdate.Append(" TimeSlot=@TimeSlot,");
            args.Add("@TimeSlot", adModel.TimeSlot);

            sbSelect.Append(" UDayStateId=@UDayStateId AND");
            sbUpdate.Append(" UDayStateId=@UDayStateId,");
            args.Add("@UDayStateId", adModel.UDayStateId);

            if (adModel.UserId != null)
            {
                sbSelect.Append(" UserId=@UserId AND");
                sbUpdate.Append(" UserId=@UserId,");
                args.Add("@UserId", adModel.UserId);
            }

            if (isSelect)
            {
                sbSelect.Append(" 1=1");
                where = sbSelect.ToString();
            }
            else
                where = sbUpdate.ToString();

            if (!isSelect && sbUpdate.Length > 0)
            {
                where = where.Substring(0, where.Length - 1);
            }
            return args;
        }
        public static class AD_QueryString
        {
            /// <summary>
            /// 查询Attendance数据库
            /// </summary>
            public static string Select_Attendance = "SELECT * FROM Attendance WHERE {0}";
            /// <summary>
            /// 添加Attendance单条数据
            /// </summary>
            public static string insert_Attendance = "INSERT INTO Attendance (ClockContent,ClockTime,ClockYear,ClockMonth,TimeSlot,UDayStateId,UserId,CreateTime,IsPass) VALUES (@ClockContent,@ClockTime,@ClockYear,@ClockMonth,@TimeSlot,@UDayStateId,@UserId,@CreateTime,@IsPass)";
            /// <summary>
            /// 更新Attendance单条数据
            /// </summary>
            public static string update_Attendance = "UPDATE Attendance SET {0} WHERE {1}";
            /// <summary>
            /// 删除Attendance相关条件的数据
            /// </summary>
            public static string delete_Attendance = "DELETE FROM Attendance WHERE {0}";
        }
    }
}
