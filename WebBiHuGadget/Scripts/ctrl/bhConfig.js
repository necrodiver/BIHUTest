function defineConst(props) {
    var obj = {};
    for (var prop in props) {
        // Modern browsers, IE9+
        Object.defineProperty(obj, prop, {
            writable: false,
            value: props[prop]
        });
    }
    return obj;
};
var bhConfig = defineConst({
    LoginIn: "/Home/LoginIn",//登录
    GetSingleUser: "/Home/GetSingleUser",//获取单个用户信息
    SingleOut: "/Home/SingleOut",//退出

    GetAnalysisUserList: "/AnalysisStatistic/GetAnalysisUserList",//获取打卡备注列表
    GetAttendanceList: '/AnalysisExcel/GetAttendanceList',//获取打卡备注分页集合
    UploadXls: "/AnalysisExcel/UploadXls",//上传xls文件
    GetMonthData: "/AnalysisExcel/GetMonthData",//获取考勤数据
    EditMarkStatus: "/AnalysisExcel/EditMarkStatus",//操作打卡备注(增删改)
    GetUserMarkData: '/AnalysisExcel/GetUserMarkData',//获取用户打卡年记录
    GetUserList: '/MyCenter/GetUserList',//获取用户列表
    AddUserList: '/MyCenter/AddUserList',//添加用户集合
    DeleteUserList: '/MyCenter/DeleteUserList',//删除用户集合
    EditUser: '/MyCenter/EditUser', //修改用户信息
    EditAttendanceIsPass:'/AnalysisExcel/EditAttendanceIsPass'//修改用户打卡备注状态
});
window.bhConfig = bhConfig;

