///<reference path="../../Libs/jquery-3.1.1.min.js" />
$(document).ready(function () {

    $(document).on('click', '#btn-submit', function (e) {
        let $btn = $(this);
        submitAjaxForm($btn,
            function (rep) {
                $btn.closest('form').inlineNotify(notifyType.success, strings.success);
                $('#Subject').val('');
                $('#Content').val('');
            },
            null,
            false
        );
    });
});