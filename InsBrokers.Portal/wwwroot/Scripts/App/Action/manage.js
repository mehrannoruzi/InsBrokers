﻿/// <reference path="../../Libs/jquery-3.1.1.min.js" />
$(document).ready(function () {
    //submit view
    $(document).on('click', '.btn-submit-view', function () {
        let $btn = $(this);
        submitAjaxForm($(this),
            function (rep) {
                if ($('#ActionId').val() === '0') {
                    let $frm = $btn.closest('form').inlineNotify(notifyType.success, strings.success)[0].reset();
                }
                else {
                    $('#modal').modal('hide');
                }
            },
            null,
            false
        );
    });

});

