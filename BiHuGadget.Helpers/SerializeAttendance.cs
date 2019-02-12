using System;
using System.Collections.Generic;
using System.Linq;
using BiHuGadget.Models;

namespace BiHuGadget.Helpers
{
    public static class SerializeAttendance
    {
        /// <summary>
        /// 获取考勤数据
        /// </summary>
        /// <param name="fullName">考勤xls文件</param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, UserAll>> GetUserAllList(string fullName)
        {
            //进行文件处理
            List<XlsModel> list = new OperateExcelWps().ExcelToDataTable(fullName, true);
            if (list == null)
            {
                return null;
            }
            try
            {
                Dictionary<string, Dictionary<string, UserAll>> userList = new Dictionary<string, Dictionary<string, UserAll>>();
                var users = list.Select(n => n.UserName).Distinct().ToList();
                for (int i = 0; i < users.Count; i++)
                {
                    List<XlsModel> listChild = list.Where(l => l.UserName == users[i]).ToList();
                    var thisUserList = DisPoseUser(listChild);
                    userList.Add(users[i], thisUserList);
                }
                return userList;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("考勤数据转换错误:" + ex.ToString());
            }
            return null;

        }

        private static Dictionary<string, UserAll> DisPoseUser(List<XlsModel> list1)
        {

            var last = Convert.ToDateTime(list1[0].PunchCardTime);
            int days = DateTime.DaysInMonth(last.Year, last.Month);

            List<User> userList = new List<User>();
            for (int i = 1; i <= days; i++)
            {
                string thisTime = null;
                bool isRight = false;
                int today = (int)new DateTime(last.Year, last.Month, i).DayOfWeek;
                for (int j = 0; j < list1.Count; j++)
                {
                    if (Convert.ToDateTime(list1[j].PunchCardTime).Day == i)
                    {
                        isRight = true;
                        thisTime = list1[j].PunchCardTime;
                        if (userList.SingleOrDefault(u => u.putCaredTime == Convert.ToDateTime(list1[j].PunchCardTime).ToString("yyyy-MM-dd HH:mm:ss")) != null)
                        {
                            continue;
                        }
                        bool isLastDay = false;
                        User userchild = ProofTime(thisTime, today, out isLastDay);
                        if (userchild.day == 0)
                        {
                            continue;
                        }
                        if (isLastDay)
                        {
                            if (list1.Count > j + 2 && Convert.ToDateTime(list1[j + 1].PunchCardTime).Day != i && Convert.ToDateTime(list1[j + 2].PunchCardTime).Day != i)
                            {
                                userList.Add(new User
                                {
                                    day = i,
                                    putCaredTime = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"),
                                    status = 0,
                                    isLeftTime = null,
                                    dayOfWeek = today
                                });
                            }
                        }
                        userList.Add(userchild);
                    }
                }
                if (thisTime == null && !isRight)
                {
                    if (today == 0)
                    {
                        userList.Add(new User
                        {
                            day = i,
                            putCaredTime = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"),
                            status = 100,
                            isLeftTime = null,
                            dayOfWeek = 7
                        });
                    }
                    else if (IsHolidayByDateSix(last.AddDays(1 - last.Day).AddDays(i - 1)))
                    {
                        userList.Add(new User
                        {
                            day = i,
                            putCaredTime = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"),
                            status = 100,
                            isLeftTime = null,
                            dayOfWeek = 6
                        });
                    }
                    else if (IsHolidayByDate(last.AddDays(1 - last.Day).AddDays(i - 1)))
                    {
                        userList.Add(new User
                        {
                            day = i,
                            putCaredTime = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"),
                            status = 100,
                            isLeftTime = null,
                            dayOfWeek = today
                        });
                    }
                    else
                    {
                        userList.Add(new User
                        {
                            day = i,
                            putCaredTime = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"),
                            status = 0,
                            isLeftTime = null,
                            dayOfWeek = today
                        });
                    }
                }
                if (i == 1 && thisTime != null && Convert.ToDateTime(thisTime).Hour < 8 && Convert.ToDateTime(list1[i + 1].PunchCardTime).Day != i && Convert.ToDateTime(list1[i + 2].PunchCardTime).Day != i)
                {
                    userList.Add(new User
                    {
                        day = i,
                        putCaredTime = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"),
                        status = 0,
                        isLeftTime = null,
                        dayOfWeek = today
                    });
                }
                //这是最后检测是否在list里边存在，若没有则为不存在
                if (!userList.Exists(u => u.day == i))
                {
                    userList.Add(new User
                    {
                        day = i,
                        putCaredTime = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"),
                        status = 0,
                        isLeftTime = null,
                        dayOfWeek = today
                    });
                }
            }
            Dictionary<string, UserAll> userAll = new Dictionary<string, UserAll>();
            for (int i = 0; i < userList.Count; i++)
            {
                string key = "day_" + userList[i].day;
                UserAll userChild = new UserAll();
                if (!userAll.ContainsKey(key))
                {
                    userAll.Add(key, userChild);
                }
                userAll[key].day = userList[i].day;
                userAll[key].dayOfWeek = userList[i].dayOfWeek;
                if (userList[i].status == 0)// && userList[i].isLeftTime == null
                {
                    userAll[key].status = 0;
                }
                else if (userList[i].status == 100)
                {
                    userAll[key].status = 2;
                }
                else
                {
                    userAll[key].status = 1;
                }

                if (userList[i].isLeftTime == true)
                {
                    userAll[key].UserLeft = new UserChild();
                    userAll[key].UserLeft.putCaredTime = userList[i].putCaredTime;
                    userAll[key].UserLeft.status = userList[i].status;
                }
                else if (userList[i].isLeftTime == false)
                {
                    userAll[key].UserRight = new UserChild();
                    userAll[key].UserRight.putCaredTime = userList[i].putCaredTime;
                    userAll[key].UserRight.status = userList[i].status;
                }
                else//isLeftTime=null的情况
                {
                    userAll[key].UserLeft = null;
                    userAll[key].UserRight = null;
                }
            }

            return userAll;
        }
        private static User ProofTime(string thisTime, int today, out bool isLastDay)
        {
            isLastDay = false;
            User userModel = new User();
            var todayTime = Convert.ToDateTime(thisTime);
            int day = todayTime.Day;
            int hour = todayTime.Hour;
            int minute = todayTime.Minute;
            int dayOfWeek = Convert.ToInt32(todayTime.DayOfWeek);

            userModel.putCaredTime = todayTime.ToString("yyyy-MM-dd HH:mm:ss");
            userModel.day = day;
            userModel.dayOfWeek = today;
            if (hour >= 6 && hour < 14)
            {
                userModel.isLeftTime = true;
                if ((hour == 9 && minute > 30) || hour >= 10)
                {
                    userModel.status = -3;
                }
                if ((hour >= 7 && hour < 9) || (hour == 9 && minute <= 30))
                {
                    userModel.status = 1;
                }
            }
            if (hour >= 14 || hour <= 6)
            {
                userModel.isLeftTime = false;
                if (hour >= 14 && hour < 21)
                {
                    if (hour < 6)
                    {
                        userModel.status = -2;
                    }
                    else
                    {
                        userModel.status = -1;
                    }
                }
                if (hour == 21)
                {
                    userModel.status = 1;
                }
                if (((hour == 22) || (hour == 23)) && hour < 24)
                {
                    userModel.status = 2;
                }
                if (hour >= 0 && hour <= 6)
                {
                    userModel.day -= 1;
                    if (userModel.dayOfWeek > 1)
                    {
                        userModel.dayOfWeek -= 1;
                    }
                    else
                    {
                        userModel.dayOfWeek = 7;
                    }

                    userModel.status = 2;
                    isLastDay = true;
                }
                if (dayOfWeek == 6 && hour >= 18)
                {
                    userModel.status = 1;
                }
            }
            return userModel;
        }

        /// <summary>
        /// 判断是不是节假日,节假日返回true 
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private static bool IsHolidayByDate(DateTime date)
        {
            Dictionary<DateTime, bool> times = new Dictionary<DateTime, bool>();
            var thisTime = date;
            var year = thisTime.Year;
            var month = thisTime.Month;
            var day = thisTime.Day;
            var thisYear = DateTime.Now.Year;
            if (year == thisYear)
            {
                if (month == 1 && day == 1)
                {
                    return true;
                }
                if (month == 2 && (day > 1 && day < 10))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool IsHolidayByDateSix(DateTime date)
        {
            Dictionary<DateTime, bool> times = new Dictionary<DateTime, bool>();
            var thisTime = date;
            var year = thisTime.Year;
            var month = thisTime.Month;
            var day = thisTime.Day;
            var thisYear = DateTime.Now.Year;
            //以下为单双休日期
            //if (month == 1 && (day == 1 || day == 31))
            //{
            //    return true;
            //}
            if (month == 2 && day == 23)
            {
                return true;
            }
            return false;
        }
    }
}