var helper = {};
helper.pwd = function (val) {
    if (!/^[A-Za-z\.0-9]{4,20}/.test(val))
        return false;
    return true;
};
helper.bihuEmail = function (val) {
    if (!/^[0-9a-zA-Z]{2,20}\@91bihu\.com/.test(val))
        return false;
    return true;
};

//弹出提示框,2秒后消失
helper.toolTipBox = function (msg) {
    var d = dialog({
        content: msg
    });
    d.show();
    setTimeout(function () {
        d.close().remove();
    }, 2000);
}

window.helper = helper;