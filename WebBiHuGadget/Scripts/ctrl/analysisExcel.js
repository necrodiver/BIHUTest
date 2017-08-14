var vm = new Vue({
    el: '#vue1',
    data: {
        userPunchCards: [],
        userName: "",
        userMounthNullCount: 0,
        userMounths: [],
        selectMonth: "-1"
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
        $("#selectMonth").bind('change', function () {
            vm.selectMonth = $(this).val();
        });
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
        }
    }
});