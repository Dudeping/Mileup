/*全局*/

function redelete() {
    if (confirm("确定要删除吗？")) {
        alert("删除成功！");
        return true;
    }
    else {
        return false;
    }
}

function loginCheck() {
    var userName = document.getElementById("userName");
    var password = document.getElementById("password");
    if (userName.value == "") {
        alert('请输入用户名！');
        userName.focus();
        return false;
    }
    if (password.value == "") {
        alert('请输入密码！');
        password.focus();
        return false;
    }
}