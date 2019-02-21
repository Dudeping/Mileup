
function ajax(url, onsuccess) {
    var xmlhttp = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject('Microsoft.XMLHTTP'); //创建XMLHTTP对象，考虑兼容性
    xmlhttp.open("POST", url, true);
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            if (xmlhttp.status == 200) //如果状态码为200则是成功
            {
                onsuccess(xmlhttp.responseText);
            }
            else {
                alert("AJAX服务器返回错误！");
            }
        }
    }
    xmlhttp.send();
}