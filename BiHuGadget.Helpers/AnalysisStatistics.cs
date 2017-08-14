using BiHuGadget.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BiHuGadget.Helpers
{
    public static class AnalysisStatistics
    {
        private static string BasePath = AppDomain.CurrentDomain.BaseDirectory + "XLS\\";
        public static List<UserCountModel> AnalysisUserData()
        {
            Dictionary<string, List<UserAnalysisData>> userAnalysisDy = new Dictionary<string, List<UserAnalysisData>>();
            for (int i = 1; i < 13; i++)
            {
                Dictionary<string, Dictionary<string, UserAll>> userListNew = null;
                var userList = CacheHelper.GetCache(i.ToString());
                if (userList == null)
                {
                    var monthFileName = $"{i}月考勤.xls";
                    var fullName = Path.Combine(BasePath, Path.GetFileName(monthFileName));

                    if (!File.Exists(fullName))
                        userListNew = null;
                    else
                        userListNew = SerializeAttendance.GetUserAllList(fullName);
                }
                else
                    userListNew = (Dictionary<string, Dictionary<string, UserAll>>)userList;//day,userall
                if (userListNew == null)
                {
                    continue;
                }
                foreach (var userChild in userListNew)
                {
                    int normalCheckNum = userChild.Value.Where(u => u.Value.status == 1 && (u.Value.UserLeft != null && u.Value.UserLeft.status == 1)).ToList().Count;
                    normalCheckNum += userChild.Value.Where(u => u.Value.status == 1 && (u.Value.UserRight != null && u.Value.UserRight.status == 1)).ToList().Count;

                    int beLateNum = userChild.Value.Where(u => u.Value.status == 1 && (u.Value.UserLeft != null && u.Value.UserLeft.status == -3)).ToList().Count;

                    int noCheckNum = userChild.Value.Where(u => u.Value.status == 1 && u.Value.UserLeft == null).ToList().Count;
                    noCheckNum += userChild.Value.Where(u => u.Value.status == 1 && u.Value.UserRight == null).ToList().Count;

                    int leaveNum = userChild.Value.Where(u => u.Value.status == 0).ToList().Count;

                    int leaveEarlyNum = userChild.Value.Where(u => u.Value.status == 1 && (u.Value.UserRight != null && u.Value.UserRight.status == -2)).ToList().Count;

                    int earlyHomeNum = userChild.Value.Where(u => u.Value.status == 1 && (u.Value.UserRight != null && u.Value.UserRight.status == -1)).ToList().Count;

                    int workOverTimeNum = userChild.Value.Where(u => u.Value.status == 1 && (u.Value.UserRight != null && u.Value.UserRight.status == 2)).ToList().Count;

                    List<UserAnalysisData> userListChild = new List<UserAnalysisData>();

                    if (!userAnalysisDy.ContainsKey(userChild.Key))
                    {
                        userListChild.Add(new UserAnalysisData
                        {
                            UserName = userChild.Key,
                            MonthNum = i,
                            NormalCheckNum = normalCheckNum,
                            BeLateNum = beLateNum,
                            NoCheckNum = noCheckNum,
                            LeaveNum = leaveNum,
                            LeaveEarlyNum = leaveEarlyNum,
                            EarlyHomeNum = earlyHomeNum,
                            WorkOverTimeNum = workOverTimeNum
                        });
                        userListChild.Add(new UserAnalysisData
                        {
                            UserName = userChild.Key,
                            MonthNum = 0,//0代表综合
                            NormalCheckNum = normalCheckNum,
                            BeLateNum = beLateNum,
                            NoCheckNum = noCheckNum,
                            LeaveNum = leaveNum,
                            LeaveEarlyNum = leaveEarlyNum,
                            EarlyHomeNum = earlyHomeNum,
                            WorkOverTimeNum = workOverTimeNum
                        });
                        userAnalysisDy.Add(userChild.Key, userListChild);
                        continue;
                    }
                    if (userAnalysisDy[userChild.Key].Exists(u => u.MonthNum == i))
                    {
                        continue;
                    }
                    else
                    {
                        userAnalysisDy[userChild.Key].Add(new UserAnalysisData
                        {
                            UserName = userChild.Key,
                            MonthNum = i,
                            NormalCheckNum = normalCheckNum,
                            BeLateNum = beLateNum,
                            NoCheckNum = noCheckNum,
                            LeaveNum = leaveNum,
                            LeaveEarlyNum = leaveEarlyNum,
                            EarlyHomeNum = earlyHomeNum,
                            WorkOverTimeNum = workOverTimeNum
                        });
                    }
                    if (userAnalysisDy[userChild.Key].Exists(u => u.MonthNum == 0))
                    {
                        UserAnalysisData userthis = userAnalysisDy[userChild.Key].SingleOrDefault(u => u.MonthNum == 0);
                        userAnalysisDy[userChild.Key].Remove(userthis);
                        userthis.NormalCheckNum += normalCheckNum;
                        userthis.BeLateNum += beLateNum;
                        userthis.NoCheckNum += noCheckNum;
                        userthis.LeaveNum += leaveNum;
                        userthis.LeaveEarlyNum += leaveEarlyNum;
                        userthis.EarlyHomeNum = earlyHomeNum;
                        userthis.WorkOverTimeNum += workOverTimeNum;
                        userAnalysisDy[userChild.Key].Add(userthis);
                    }
                }
            }
            //Dictionary<string, List<UserAnalysisData>>()
            List<UserCountModel> userCountList = new List<UserCountModel>();
            foreach (var item in userAnalysisDy)
            {
                userCountList.Add(new UserCountModel
                {
                    UserName = item.Key,
                    UserChecks = item.Value
                });
            }
            return userCountList;
        }
    }
    public class UserCountModel
    {
        public string UserName { get; set; }
        public List<UserAnalysisData> UserChecks { get; set; }
    }
    public class UserAnalysisData
    {
        //迟到/早退/早回家/正常/加班

        public string UserName { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int MonthNum { get; set; }
        /// <summary>
        /// 正常打卡数
        /// </summary>
        public int NormalCheckNum { get; set; }
        /// <summary>
        /// 未打卡
        /// </summary>
        public int NoCheckNum { get; set; }
        /// <summary>
        /// 请假
        /// </summary>
        public int LeaveNum { get; set; }
        /// <summary>
        /// 迟到
        /// </summary>
        public int BeLateNum { get; set; }
        /// <summary>
        /// 早退
        /// </summary>
        public int LeaveEarlyNum { get; set; }
        /// <summary>
        /// 早回家
        /// </summary>
        public int EarlyHomeNum { get; set; }
        /// <summary>
        /// 加班
        /// </summary>
        public int WorkOverTimeNum { get; set; }

    }
}