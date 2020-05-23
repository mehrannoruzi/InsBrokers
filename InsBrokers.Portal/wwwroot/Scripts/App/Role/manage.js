/// <reference path="../../Libs/jquery-3.1.1.min.js" />
$(document).ready(function () {
    //search with enter key
    $(document).on('keydown', '#User_Email', function (e) {
        if (e.keyCode === 13) {
            $('.btn-search-user').trigger('click');
        }
    });
    //
    //load user in roles for selected user
    $(document).on('select2:select', '#UserId', function () {
        let $elm = $(this);
        let $loader = $('.loader');
        $loader.html($threeDotLoader);
        $.get($('.user-in-roles').data('url'), { userId: $elm.val() })
            .done(function (res) {
                $loader.empty();
                $('.user-in-roles').replaceWith(res);
            })
            .fail(function (e) {
                $loader.empty();
                showNotif(notifyType.danger, strings.error);
            });
    });
    //load action in roles for selected action
    $(document).on('select2:select', '#ActionId', function () {
        let $elm = $(this);
        let $loader = $('.loader');
        $loader.html($threeDotLoader);
        $.get($('.action-in-roles').data('url'), { actionId: $elm.val() })
            .done(function (res) {
                $loader.empty();
                $('.action-in-roles').replaceWith(res);
            })
            .fail(function (e) {
                $loader.empty();
                showNotif(notifyType.danger, strings.error);
            });
    });

    //add user in role
    $(document).on('click', '.btn-add-user-in-role', function (e) {
        if ($('UserId').val() === '00000000-0000-0000-0000-000000000000') {
            $(this).closest('form').inlineNotify(notifyType.danger, strings.error);
            return;
        }
        submitAjaxForm($(this),
            function (rep) {
                $('.user-in-roles').replaceWith(rep.Result);
            },
            null,
            false
        );
    });

    //load all ViewInRole by ViewId
    $(document).on('change', '#ActionId', function (e) {
        let $list = $('.action-in-roles');
        $.get($list.data('url'), { actionId: $('#ActionId').val() })
            .done(function (rep) {
                $list.loadOverStop().replaceWith(rep);
                $('.footable').footable({
                    "breakpoints": {
                        phone: 480,
                        tablet: 1024
                    }
                });
            })
            .fail(function (e) {
                $list.loadOverStop();
            });
    });

    //add view in role 
    $(document).on('click', '.btn-add-action-in-role', function (e) {
        if ($('#ViewId').val() === '') {
            $(this).closest('form').inlineNotify(notifyType.danger, strings.error);
            return;
        }
        submitAjaxForm($(this),
            function (rep) {
                $('.action-in-roles').replaceWith(rep.Result);
                $('.footable').footable({
                    "breakpoints": {
                        phone: 480,
                        tablet: 1024
                    }
                });
            },
            null,
            false
        );
    });

    //
    $(document).on('click', '.delete-user-in-role', function (e) {
        let $elm = $(this);
        console.log('fired');
        swalConfirm(function () {
            ajaxCall($elm, function (rep) {
                $elm.closest('tr').remove();
            }
            );
        });
    });


    $(document).on('change', '.uploader', function (e) {

        upload($(this), e.target.files, function (rep) { console.log(rep); });
    });
});

