var helper = {};
helper.pwd = function (val) {
    if (!/^[A-Za-z\.0-9]{6,20}/.test(val))
        return false;
    return true;
};
helper.bihuEmail = function (val) {
    if (!/^[A-Za-z]{2,20}\@91bihu\.com/.test(val))
        return false;
    return true;
};
window.helper = helper;