//<reference path="../Libs/jquery-3.1.1.min.js" />
$(document).ready(function () {

    //show modal event
    $(document).on('click', '.btn-modal', function () {
        let $elm = $(this);
        showModal({ $elm: $(this) });
    });

    //modal open event
    $('#modal').on('shown.bs.modal', function (e) {
        fireGlobalPlugins();
    });

    //auto submit modal
    $(document).on('click', 'button[data-auto-submit="true"]', function () {
        let $btn = $(this);
        submitAjaxForm($btn, function (rep) {
            let $frm = $btn.closest('.modal-body').find('form');
            $frm.inlineNotify('success', strings.success);
            if ($btn.data('reset')) $frm[0].reset();
        },
            null,
            false
        );
    });

    //modal close event
    $("#modal").on("hidden.bs.modal", function () {
        console.log($(this).data('refresh-list'));
        if ($(this).data('refresh-list')) refreshList();
    });
});
var modalTemplate = {
    notif: `<div class="row inner-notification">
                <div class="col-12">
                    <div class="alert">
                        <p class="text"></p>
                    </div>
                </div>
            </div>`,
    footer: `<div class="custom-modal-footer">
                <button type="button" class="btn btn-secondary float-right" data-dismiss="modal">${strings.close}</button>
                <button type="button" class="btn btn-primary btn-action float-left" data-auto-submit="true" data-reset="{0}">
                    <span class="text">{1}</span>
                    <div class="icon">
                        <i class="zmdi {2}"></i>
                    </div>
                </button>
            </div>`
};

var showModal = function ({ $elm, beforeFunc, afterFunc, errorFunc }) {
    if (typeof beforeFunc === 'function') beforeFunc();
    let elmIsActionBtn = $elm.find('.icon').length > 0;
    if (elmIsActionBtn) ajaxBtn.inProgress($elm);
    $.get($elm.data('url'))
        .done(function (rep) {
            if (elmIsActionBtn) ajaxBtn.normal();
            if (!rep.IsSuccessful) {
                showNotif(notifyType.danger, rep.Message);
                return;
            }
            let $modal = $('#modal').data('refresh-list', rep.RefreshList);
            $modal.find('.modal-title').text(rep.Title);
            let $body = $modal.find('.modal-body-content').empty();
            if (rep.AutoSubmit) {
                $frm = $('<form></form>', {
                    method: "post",
                    class: "modal-frm",
                    action: rep.AutoSubmitUrl
                }).html(modalTemplate.notif)
                    .append(rep.Body)
                    .append(modalTemplate.footer
                        .replace('{0}', rep.ResetForm)
                        .replace('{1}', rep.AutoSubmitBtnText)
                        .replace('{2}', rep.AutoSubmitBtnIcon));
                $body.html($frm);
            }
            else $body.html(rep.Body);

            $.validator.unobtrusive.parse($modal);
            $modal.modal('show');


            if (typeof afterFunc === 'function') afterFunc();
        })
        .fail(function (e) {
            if (elmIsActionBtn) ajaxBtn.normal();
            if (typeof errorFunc === 'function') errorFunc(e);
        });
};