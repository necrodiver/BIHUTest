var __RequestVerificationToken = $('input[name="__RequestVerificationToken"]').val();
function init(_self) {
    $('#editMark')
        .modal({
            blurring: true,
            onApprove: function () {
                _self.saveMark();
            }
        })
        .modal('hide');
    $("#modelBox")
        .modal({
            blurring: false,
            onApprove: function () {
                _self.saveMark();
            }
        }).modal("hide");
    $("#selectMonth").bind('change', function () {
        _self.selectMonth = $(this).val();
    });
    $("#selectMonth").bind('change', function () {
        _self.selectMonth = $(this).val();
    });
    $("#selectTimeSlot").bind('change', function () {
        _self.addMarkData.timeSlot = $(this).val();
    });
    $("#selectMarkState").bind('change', function () {
        _self.addMarkData.markState = $(this).val();
    });
    _self.markStateList.forEach(function (item, i) {
        $("#markStatusMenu").append("<div class='item' data-value=" + i + ">" + item + "</div>");
    });
}

var vm = new Vue({
    el: '#vue1',
    data: {
        userPunchCards: [],
        userName: "",
        userMounthNullCount: 0,
        userMounths: [],
        selectMonth: "-1",
        addMarkData: {
            markId: -1,//打卡备注Id
            markIUD: -1,//操作类型:增,改,删
            userId: -1,//用户Id
            dayTime: '', //记录日期
            timeSlot: -1,//时间段:全天,上午,下午
            markState: -1,//打卡状态
            markReason: ''//备注说明
        },
        addMarkInit: {
            markId: -1,//打卡备注Id
            markIUD: -1,//操作类型:增,改,删
            userId: -1,//用户Id
            dayTime: '', //记录日期
            timeSlot: -1,//时间段:全天,上午,下午
            markState: -1,//打卡状态
            markReason: ''//备注说明
        },
        selectMarkData: {
            year: 2017,
            //userId: -1,
            userName: ''
        },
        showSingleDayRemark: [],//展示单天的打卡记录
        markStateList: ["母鸡呀", "迟到", "早退", "早回家", "正常", "加班", "请假", "忘打卡"],
        markDatas: []
    },
    components: {//组件

    },
    filters: {//过滤器

    },
    watch: {
        userName: function () {
            this.selectUserName();
        },
        selectMonth: function () {
            this.selectMonthData();
        }
    },
    mounted: function () {
        init(this);
    },
    methods: {
        getData: function () {
            this.userPunchCards = [];
            this.userMounths = [];
            this.userMounthNullCount = 0;
            var fd = new FormData();
            fd.append("requestFile", $("#xlsSelect")[0].files[0]);
            $.ajax({
                url: bhConfig.UploadXls,
                type: "POST",
                processData: false,
                contentType: false,
                data: fd,
                success: function (d) {
                    if (d.MsgStatus) {
                        alert(d.MsgContent);
                    } else {
                        alert(d.MsgContent);
                    }
                }
            });
        },
        calendarAssembly: function () {
            var jsonData = $.extend({}, this.userPunchCards);
        },
        selectUserName: function () {
            if (this.userName == null || this.userName == '' || this.userName.length < 2) {
                return;
            }
            this.userMounths = [];
            this.userMounthNullCount = 0;
            var temporary = this.userPunchCards[this.userName];
            if (temporary != undefined && temporary != null && temporary['day_1'].day == 1) {
                this.userMounthNullCount = temporary['day_1'].dayOfWeek - 1;
                this.userMounths = temporary;
                this.selectMarkData.userName = this.userName;
                this.getUserMarkData();
            }
        },
        selectMonthData: function () {
            var thisMonthNum = parseInt(this.selectMonth);
            if (thisMonthNum <= 0 || thisMonthNum > 12) {
                return alert("请正确选择月份");
            }
            //这里进行数据获取
            this.userMounths = [];
            this.userMounthNullCount = 0;
            var thisMonth = localStorage.getItem(this.selectMonth);
            var _self = this;
            if (thisMonth != null) {
                thisMonth = JSON.parse(thisMonth);
                this.userPunchCards = thisMonth;
                this.selectUserName();
                return;
            }
            var req = $.ajax({
                type: 'GET',
                url: bhConfig.GetMonthData,
                data: { monthNum: thisMonthNum },
                dataType: "JSON",
                cache: false,
                beforeSend: function () {

                }
            });
            req.done(function (res) {
                if (res == null || res == "") {
                    return alert("操作失败");
                }
                if (!res.MsgStatus) {
                    return alert(res.MsgContent);
                }
                localStorage.setItem(_self.selectMonth, JSON.stringify(res.MsgContent));
                _self.userPunchCards = res.MsgContent;
                _self.selectUserName();
            });
            req.fail(function (jqXHR, textStatus, error) {
                //if (loadingName) {
                //    if (textStatus == "timeout") {
                //        return alert("请求超时！");
                //    }
                //    else if (textStatus == "error") {
                //        return alert("网络连接超时！");
                //    }
                //}
            });
            req.always(function () {
                //所有的走完之后调用的数据
            });
        },
        editMark: function (val) {
            var _self = this;
            if (val)
                _self.addMarkData.markIUD = val;
            if (!this.addMarkData.dayTime) {
                return;
            }
            $('#addMark-dayTime').text(this.addMarkData.dayTime);
            $('#editMark').modal('show');
        },
        saveMark: function () {
            var dt = $.extend({}, this.addMarkData);
            dt.__RequestVerificationToken = __RequestVerificationToken;
            if (dt.markId == -1 || dt.markIUD == -1 || dt.timeSlot == -1 || dt.markState == -1) {
                dt.markId = null;
            }
            if (dt.userId == -1) {
                dt.userId = null;
            }
            var req = $.ajax({
                type: 'POST',
                url: bhConfig.EditMarkStatus,
                data: dt,
                dataType: "JSON",
                cache: false
            });
            req.done(function (res) {
                if (res == null || res == "") {
                    return alert("操作失败");
                }
                return alert(res.MsgContent);
            });
            req.always(function () {
                getUserMarkData();
            });
        },
        selectMask: function (val) {
            this.addMarkData = $.extend({}, this.addMarkInit);
            var dayTimeTP = "";
            if (val.day && val.day > 0 && val.day < 32) {
                if (this.markDatas && this.markDatas.length > 0) {
                    var nowMarkData = this.markDatas.filter(function (item) {
                        return item.day == val.day;
                    });
                    if (nowMarkData.length == 1 && nowMarkData[0].day)
                        this.addMarkData = nowMarkData[0];
                    else
                        dayTimeTP = moment(this.selectMarkData.year + '-' + this.selectMonth + '-' + val.day).format('YYYY-MM-DD');
                } else {
                    dayTimeTP = moment(this.selectMarkData.year + '-' + this.selectMonth + '-' + val.day).format('YYYY-MM-DD');
                }
                this.addMarkData.dayTime = dayTimeTP;
                if (this.markDatas != [] && this.markDatas.length > 0) {
                    this.showSingleDayRemark = this.markDatas.filter(function (item) {
                        return item.ClockTime == dayTimeTP;
                    });
                }
            }
        },
        getUserMarkData: function () {
            var _self = this;
            var req = $.ajax({
                type: 'POST',
                url: bhConfig.GetUserMarkData,
                data: this.selectMarkData,
                dataType: 'JSON',
                cache: false
            });
            req.done(function (res) {
                if (res == null || res == "") {
                    return alert("获取用户列表失败");
                }
                if (res.MsgStatus)
                    _self.markDatas = res.MsgContent;
                else
                    alert(res.MsgContent);
            });
        }
    }
});