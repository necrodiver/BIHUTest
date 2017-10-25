var __RequestVerificationToken = $('input[name="__RequestVerificationToken"]').val();
function init(_self) {
    $("#selectMonth").bind('change', function () {
        _self.selectMonth = $(this).val();
    });

    _self.markStateList.forEach(function (item, i) {
        $("#markStatusMenu").append("<div class='item' data-value=" + i + ">" + item + "</div>");
    });
    $('#editMark')
        .modal({
            blurring: true,
            onApprove: function () {
                _self.saveMark();
            }
        })
        .modal('hide');
    initBind(_self);
    $('#btnCancle').click(function () {
        alert('cancle');
        $('#editMark').modal('hide');
    });
    $('#btnDelete').click(function () {
        alert('delete');
        $('#editMark').modal('hide');
    });
    $('#btnEdit').click(function () {
        alert('edit');
        $('#editMark').modal('hide');
    });
}
function initBind(_self) {
    $('#selectTimeSlot').dropdown('set value', _self.addMarkData.TimeSlot);
    $('#selectMarkState').dropdown('set value', _self.addMarkData.UDayStateId);
    $("#selectTimeSlot").dropdown({
        onChange: function (val) {
            _self.addMarkData.TimeSlot = parseInt(val);
        }
    });
    $("#selectMarkState").dropdown({
        onChange: function (val) {
            _self.addMarkData.UDayStateId = parseInt(val);
        }
    });
   
};

var vm = new Vue({
    el: '#vue1',
    data: {
        userPunchCards: [],
        userName: "",
        userMounthNullCount: 0,
        userMounths: [],
        selectMonth: "-1",
        addMarkData: {
            AttendanceId: -1,//打卡备注Id
            markIUD: -1,//操作类型:增,改,删
            UserId: -1,//用户Id
            ClockTime: '', //记录日期
            TimeSlot: -1,//时间段:全天,上午,下午
            UDayStateId: 0,//打卡状态
            ClockContent: ''//备注说明
        },
        addMarkInit: {
            AttendanceId: -1,//打卡备注Id
            markIUD: -1,//操作类型:增,改,删
            UserId: -1,//用户Id
            ClockTime: '', //记录日期
            TimeSlot: -1,//时间段:全天,上午,下午
            UDayStateId: 0,//打卡状态
            ClockContent: ''//备注说明
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
        momentTime: function (val) {
            if (val != null && val != undefined && val != '') {
                return moment(val).format('HH:mm:ss');
            }
            return val;
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
        },
        saveMark: function () {
            var _self = this;
            var dt = $.extend({}, this.addMarkData);
            dt.__RequestVerificationToken = __RequestVerificationToken;
            if (dt.AttendanceId == -1 || dt.markIUD == -1 || dt.TimeSlot == -1 || dt.UDayStateId == -1) {
                dt.AttendanceId = null;
            }
            if (dt.UserId == -1) {
                dt.UserId = null;
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
                _self.getUserMarkData();
            });
        },
        selectMark: function (val, index) {
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
                this.addMarkData.ClockTime = dayTimeTP;
                this.showSingleDayRemark = [];
                if (this.markDatas != [] && this.markDatas.length > 0) {
                    this.showSingleDayRemark = this.markDatas.filter(function (item) {
                        return item.ClockTime == dayTimeTP;
                    });
                }

                if (!this.addMarkData.ClockTime) {
                    return;
                }

                if (index == 0) {
                    if (val.status && val.status == 2)
                        return;
                    return this.editMark(true);
                }

                if (index == 1) {
                    var mList = this.showSingleDayRemark.filter(function (item) {
                        return item.TimeSlot == 1;
                    });
                    //if (val.UserLeft == undefined || val.UserLeft == null)
                    //    return this.editMark(true);
                    //if (val.UserLeft.status == 1 || val.UserLeft.status == 2)
                    //    return;
                    this.addMarkData.UDayStateId = 1;
                    if (mList != null && mList != undefined && mList.length == 1)
                        return this.editMark(false, mList[0])
                    return this.editMark(true);
                }
                if (index == 2) {
                    var aList = this.showSingleDayRemark.filter(function (item) {
                        return item.TimeSlot == 2;
                    });
                    //if (val.UserRight == undefined || val.UserRight == null)
                    //    return this.editMark(true);
                    //if (val.UserRight.status == 1 || val.UserRight.status == 2)
                    //    return;
                    if (aList != null && aList != undefined && aList.length == 1)
                        return this.editMark(false, aList[0]);
                    return this.editMark(true);
                }
            }
        },
        editMark: function (isAdd, data) {
            if (isAdd) {
                $('#editMark-header').text('新增--备注说明');
                this.addMarkData.markIUD = 1;
            } else {
                $('#editMark-header').text('修改/删--备注说明');
                $.extend(this.addMarkData, data);
                this.addMarkData.markIUD = 2;
            }
            $('#addMark-dayTime').text(this.addMarkData.ClockTime);
            //this.btnEditIDUS();
            $('#editMark').modal('show');

            initBind(this);
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
        },
        btnEditIDUS: function () {
            var _self = this;
            var headerTitle = "备注说明";
            var editMarkBox = dialog({
                width: 380,
                height: 100,
                title: headerTitle,
                content: $("#editMarkBox").html(),
                reset: false,
                init: function () {
                },
                button: [{
                    value: "取消",
                    callback: function ($btn) {
                        $btn.button("reset");
                        editMarkBox.close();
                    }
                }, {
                    value: "确认",
                    callback: function ($btn) {

                    }
                }]
            }).show();
        }
    }
});