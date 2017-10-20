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

    GetUserList: "/AnalysisStatistic/GetUserList",//获取用户考勤列表

    UploadXls: "/AnalysisExcel/UploadXls",//上传xls文件
    GetMonthData: "/AnalysisExcel/GetMonthData",//获取考勤数据
    EditMarkStatus: "/AnalysisExcel/EditMarkStatus",//操作打卡备注(增删改)
    GetUserMarkData:'/AnalysisExcel/GetUserMarkData'//获取用户打卡年记录
});
window.bhConfig = bhConfig;

