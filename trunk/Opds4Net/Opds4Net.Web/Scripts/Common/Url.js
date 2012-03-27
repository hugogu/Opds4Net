function changeUrlParameter(url, name, value) {
    var reg = new RegExp("(^|)" + name + "=([^&]*)(|$)");
    var tmp = name + "=" + value;
    url = url + "";
    if (url.match(reg) != null) {
        return url.replace(eval(reg), tmp);
    }
    else {
        if (url.match("[\?]")) {
            return url + "&" + tmp;
        }
        else {
            return url + "?" + tmp;
        }
    }
}