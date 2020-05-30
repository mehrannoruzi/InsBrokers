var notifyType = {
    success: "success",
    danger: "danger",
    info: "info",
    warning: "warning"
};
var fireGlobalPlugins = function () {

    $('.pdate').Zebra_DatePicker();

};
/*--------------------------------------
            notifications
---------------------------------------*/
$.fn.showMessage = function (type, message) {
    let $frm = $(this);
    let template = `<div class="row inner-notification">
                        <div class="col-12">
                            <div class="alert">
                                <p class="text"></p>
                            </div>
                        </div>
                    </div>`;
    let $elm = $frm.find('.inner-notification');
    if ($elm.length === 0)
        $elm = $(template).prependTo($frm);
    $elm.find('.alert').removeClass().addClass('alert alert-' + type);
    $elm.find('.text').text(message);
    $elm.slideDown(300);
    return this;
};

var showToast = function (type, message) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "progressBar": true,
        "preventDuplicates": false,
        "positionClass": "toast-top-left",
        "onclick": null,
        "showDuration": "400",
        "hideDuration": "1000",
        "timeOut": "7000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    type = type === 'danger' ? 'error' : type;
    toastr[type](message);
};


