﻿function init() {
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
        vm.selectMonth = $(this).val();
    });
    $("#selectMonth").bind('change', function () {
        vm.selectMonth = $(this).val();
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
        addMark: {
            selectTime: "",//选择已有时间
            newTime: "",//自定义时间
            markExplain: "",//说明
            markState: "",//打卡状态
            dayTime: ""
        }
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
        init();
    },
    methods: {
        getData: function () {
            this.userPunchCards = [];
            this.userMounths = [];
            this.userMounthNullCount = 0;
            var fd = new FormData();
            fd.append("requestFile", $("#xlsSelect")[0].files[0]);
            $.ajax({
                url: "/AnalysisExcel/UploadXls",
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
                url: '/AnalysisExcel/GetMonthData',
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
                if (loadingName) {
                    if (textStatus == "timeout") {
                        return alert("请求超时！");
                    }
                    else if (textStatus == "error") {
                        return alert("网络连接超时！");
                    }
                }
            });
            req.always(function () {
                //所有的走完之后调用的数据
            });
        },
        editMark: function (val) {
            var _self = this;
            var asas = val;
            if (!this.addMark.UserLeft || !this.addMark.UserLeft.putCaredTime || this.addMark.UserLeft.putCaredTime.length < 8) {
                return;
            }
            this.addMark.dayTime = moment(this.addMark.UserLeft.putCaredTime).format('YYYY-MM-DD');
            $('#addMark-dayTime').text(this.addMark.dayTime);
            $('#editMark').modal('show');
        },
        saveMark: function () {
            //addMark: {
            //    year: '',
            //        month: '',
            //            day: '',
            //                selectSort: '',
            //                    selectTime: '',
            //                        markReason: ''
            //}
        },
        initClockPopup: function () {
            $('.clockBtn').popup({
                popup: '#clockPopup'
            });
        },
        selectMask: function (val) {
            this.addMark = val;
        },
        editMarkContent: function (val, typeId) {
            switch (typeId) {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:

                    return;
            }
            $("#modelBox").modal("show");
        }

    }
});