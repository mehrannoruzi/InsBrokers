var notifyType = {
    success: "success",
    danger: "danger",
    info: "info",
    warning: "warning"
};

var fireGlobalPlugins = function () {

    $('.footable').footable({
        pageSize: 200,
        "breakpoints": {
            phone: 576,
            tablet: 1024
        }
    });

    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });

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

var fireDropzone = function () {

    let getOptions = function ($elm) {
        let removable = true;

        if ($elm.data('removable') === 'false') removable = false;
        let rep = {
            renameFilename: function (fileName) {
                var ascii = /^[ -~\t\n\r]+$/;
                if (!ascii.test(fileName)) return "noneAscciFileName." + fileName.split('.').reverse()[0];
                else return fileName;
            },
            init: function () {
                this.on("addedfile", function (file) {
                    if (!file.type.match(/image.*/)) {
                        this.emit("thumbnail", file, getDefaultImageUrl(file.name));
                    }
                });
                let urls = $elm.data('file-urls');
                let temp = this;
                if (typeof urls !== 'undefined') {
                    urls.forEach(function (u) {
                        var mockFile = {
                            name: u.split('/').reverse()[0],
                            size: 10244,
                            accepted: true,
                            status: Dropzone.ADDED,
                            type: getFileType(u).type,
                            url: u
                        };
                        temp.files.push(mockFile);
                        temp.emit("addedfile", mockFile);
                        temp.emit("thumbnail", mockFile, u);
                        temp.emit('complete', mockFile);
                    });
                }
                let url = $elm.data('file-url');
                if (typeof url !== 'undefined') {
                    var mockFile = {
                        name: url.split('/').reverse()[0],
                        size: 10244,
                        accepted: true,
                        status: Dropzone.ADDED,
                        type: getFileType(url).type,
                        url: url
                    };
                    temp.files.push(mockFile);
                    temp.emit("addedfile", mockFile);
                    temp.emit("thumbnail", mockFile, url);
                    temp.emit('complete', mockFile);
                }
            },
            dictRemoveFile: typeof $elm.data('remove-message') !== 'undefined' ? $elm.data('remove-message') : strings.remove,
            addRemoveLinks: removable,
            url: $elm.data('url'),
            params: function () {
                let p = {};
                let params = $elm.data('params');
                if (typeof params !== 'undefined') {

                    for (var i = 0; i < params.length; i++) {
                        p[params[i].Key] = $('#' + params[i].Value).val();
                    }
                }
                return p;
            },
            maxFilesize: typeof $elm.data('max-file-size') !== 'undefined' ? $elm.data('max-file-size') : 500,
            acceptedFiles: $elm.data('accepted-files'),
            success: function (file, rep) {
                if (rep.Success === true) {
                    file.url = rep.Result;
                    if (typeof $elm.data('success') !== 'undefined') {
                        rep['success'] = eval($elm.data('success'))(rep.Result);
                    }
                    this.emit("complete", file);

                }
                else {
                    if (this.files.length === 1) {
                        this.removeAllFiles(true);
                    }
                    else {
                        var ref;
                        if (file.previewElement) {
                            if ((ref = file.previewElement) !== null) {
                                ref.parentNode.removeChild(file.previewElement);
                            }
                        }
                    }
                    showNotif(notifyType.danger, rep.Message);
                }

            },
            error: function (file, errorMessage, xhr) {
                console.log(errorMessage);
                showNotif(notifyType.danger, errorMessage);
                if (this.files.length === 1) {
                    this.removeAllFiles(true);
                }
                else {
                    var ref;
                    if (file.previewElement) {
                        if ((ref = file.previewElement) !== null) {
                            ref.parentNode.removeChild(file.previewElement);
                        }
                    }
                }
            }
        };
        if (typeof $elm.data('accept') !== 'undefined')
            rep['accept'] = eval($elm.data('accept'));
        if (removable)
            rep.removedfile = function (file) {
                let p = { url: file.url };
                let params = $elm.data('remove-params');
                if (typeof params !== 'undefined') {
                    for (var i = 0; i < params.length; i++) p[params[i].Key] = $('#' + params[i].Value).val();
                }
                let drop = this;
                $.post($elm.data('remove-url'), p)
                    .done(function (rep) {
                        var _ref;
                        let res = (_ref = file.previewElement) !== null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
                        if (!rep.Success) {
                            drop.files.push(file);
                            drop.emit("addedfile", file);
                            drop.emit("thumbnail", file, file.url);
                            drop.emit('complete', file);
                            showNotif(notifyType.danger, rep.Message);
                        }
                        //return res;
                    })
                    .fail(function (e) {
                        return void 0;
                    });
                console.log(this.files.length);
                //reset dropzone.js
                if (this.files.length === 0)
                    this.removeAllFiles(true);

                return this._updateMaxFilesReachedClass();
            };
        return rep;
    };

    let drop = $('.multi-uploader').each(function () {
        let $elm = $(this);
        let opts = getOptions($elm);
        opts.maxFiles = typeof $elm.data('max-files') !== 'undefined' ? $elm.data('max-files') : 10;
        opts.dictDefaultMessage = typeof $elm.data('default-message') !== 'undefined' ? $elm.data('default-message') : strings.dropYourFilesHere;

        $elm.dropzone(opts);
    });

    $('.single-uploader').each(function () {
        let $elm = $(this);
        let opts = getOptions($elm);
        opts.maxFiles = 1;
        opts.dictDefaultMessage = typeof $elm.data('default-message') !== 'undefined' ? $elm.data('default-message') : strings.dropYourFileHere;
        $elm.dropzone(opts);

    });
};
