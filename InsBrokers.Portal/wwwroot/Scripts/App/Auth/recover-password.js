/// <reference path="../../Libs/jquery-3.1.1.min.js" />
$(document).ready(function () {


    $(document).on('click', '.btn-recover-password', function (e) {
        submitAjaxForm($(this), function (rep) { showNotif(notifyType.success, rep.Message); }, null, true);
    });

    $(document).on('keypress', '#Email', function (e) {
        if (e.keyCode === 13) $('.btn-recover-password').trigger('click');
    });


});