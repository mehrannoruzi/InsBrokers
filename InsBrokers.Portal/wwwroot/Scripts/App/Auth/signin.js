/// <reference path="../../Libs/jquery-3.1.1.min.js" />
$(document).ready(function () {
    $(document).on('click', '.btn-sign-in', function () {
        let $btn = $(this);
        submitAjaxForm(
            $btn,
            function (rep) {
                window.location.href = rep.Result;
            }
        );
    });

    $(document).on('keypress', '#Password', function (e) {
        if (e.keyCode === 13) $('.btn-sign-in').trigger('click');
    });
});