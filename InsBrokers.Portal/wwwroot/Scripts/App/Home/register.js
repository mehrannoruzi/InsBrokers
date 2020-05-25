///<reference path="../../Libs/jquery-3.1.1.min.js" />
$(document).ready(function () {
    $('#home-auth-page').on('click', '#btn-sign-up', function () {
        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) return;
        ajaxBtn.inProgress($btn);
        console.log(JSON.stringify(customSerialize($frm)));
        console.log($frm.attr('action'));
        $.ajax({
            type: 'POST',
            url: $frm.attr('action'),
            contentType: 'application/json; charset=utf-8;',
            data: JSON.stringify(customSerialize($frm)),
            success: function (data) {
                ajaxBtn.normal();
                if (data.IsSuccessful) {
                    window.location.href = data.Result;
                }
                else {
                    showToast(notifyType.danger, data.Message);
                }
            },
            error: function (e) {
                console.log(e);
            }
        });
    });
});