using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuGadget.Models;
using BiHuGadget.Dal;

namespace BiHuGadget.Bll
{
    public class Attendance_Bll
    {
        private Attendance_Dal attendanceDal = new Attendance_Dal();
        public AttendanceModel GetSingleAttendanceModel(AttendanceModel adModel)
        {
            return attendanceDal.GetSingleAttendance(adModel);
        }
        public bool AddAttendance(AttendanceModel adModel)
        {
            return attendanceDal.AddAttendance(adModel);
        }

        public bool EditAttendance(AttendanceModel adModel)
        {
            return attendanceDal.UpdateAttendance(adModel);
        }

        public bool DeleteAttendance(int attendanceId)
        {
            return attendanceDal.DeleteAttendance(attendanceId);
        }

        public List<AttendanceModel> GetUserMarks(AttendanceSearchModel asModel)
        {
            return attendanceDal.GetUserMarks(asModel);
        }
    }
}
