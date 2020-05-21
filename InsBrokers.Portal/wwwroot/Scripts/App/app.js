///<reference path="../Libs/jquery-3.1.1.min.js" />

$(document).ready(function () {

});
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

///<reference path="../Libs/jquery-3.1.1.min.js" />
var $threeDotLoader = '<span class="three-dot-loader"><span class="dot"></span><span class="dot"></span><span class="dot"></span></span>';
var $circularLoader = '<div class="spinner"><svg viewBox="25 25 50 50"><circle cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10"></circle></svg></div>';



$(document).ready(function () {

    $(document).on('focus', '.material-input', function () {
        let $lbl = $(this).next();
        if (!$lbl.hasClass('lbl-top')) $lbl.addClass('lbl-top');
    }).on('focusout', '.material-input', function () {
        let $lbl = $(this).next();
        if ($(this).val().length === 0) $lbl.removeClass('lbl-top');
    });;

    fireGlobalPlugins();


    //change default validation messages 
    jQuery.extend(jQuery.validator.messages, {
        required: strings.required,
        remote: strings.remote,
        email: strings.email,
        url: strings.url,
        date: strings.date,
        dateISO: strings.dateISO,
        number: strings.number,
        digits: strings.digits,
        creditcard: strings.creditcard,
        equalTo: strings.equalTo,
        accept: strings.accept,
        maxlength: jQuery.validator.format(strings.maxlength),
        minlength: jQuery.validator.format(strings.minlength),
        rangelength: jQuery.validator.format(strings.rangelength),
        range: jQuery.validator.format(strings.range),
        max: jQuery.validator.format(strings.max),
        min: jQuery.validator.format(strings.min)
    });

    //
    $(document).on('click', '.inner-notification', function () {
        $(this).slideUp(300).find('.text').text('');
    });

    //Copy 'data-copy-value' attribute value to clipboard
    $(document).on('click', '.copy-value', function (e) {
        e.preventDefault();

        let $this = $(this);
        let value = $this.data('copy-value');
        let elm = document.createElement('textarea');

        elm.value = value;
        document.body.appendChild(elm);
        elm.select();
        document.execCommand('copy');
        document.body.removeChild(elm);
        $this.addClass('animated fadeIn').bind("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd", function () {
            $this.removeClass('animated fadeIn');
        });

        //$this.hide('fast')
        //    .removeClass('zmdi-copy copy-value')
        //    .addClass('zmdi-check-all copied-value')
        //    .show('fast')
        //    .delay(3000)
        //    .hide('fast', function () {
        //        $this.removeClass('zmdi-check-all copied-value')
        //            .addClass('zmdi-copy copy-value')
        //            .show('fast');
        //    });

    });

    //auto submit filter on pressing enter
    $(document).on('keypress', '.filters input[type="text"]', function (e) {
        if (e.keyCode === 13) $(this).closest('form').find('button.search').trigger('click');
    });
});

//--- loading function
var ajaxBtn = new (function () {
    var ins = function () { };
    var _$btn, _$icon, _$btnHtml, btnH;
    ins.prototype.inProgress = function ($btn, $loader) {
        _$btn = $btn;
        $loader = typeof $loader !== 'undefined' ? $loader : $circularLoader;
        _$btn.prop('disabled', true);
        if (_$btn.find('.icon').length > 0) {
            _$icon = $btn.find('.icon');
            _$btnHtml = _$icon.html();
            _$icon.html($loader);
        }
        else {
            btnH = _$btn.outerHeight();
            _$btnHtml = _$btn.html();
            _$btn.css({ "height": btnH + "px" }).html($loader);
        }

    };
    ins.prototype.normal = function () {
        _$btn.prop('disabled', false);
        if (_$btn.find('.icon').length > 0) {
            _$icon.html(_$btnHtml);
        }
        else {
            _$btn.html(_$btnHtml);
        }
    };
    return ins;
}());
/*--------------------------------------
            submit ajax form
---------------------------------------*/
var submitAjaxForm = function ($btn, successFunc, errorFunc, useToastr) {
    useToastr = typeof useToastr !== 'undefined' ? useToastr : true;
    if (!useToastr)
        $('.inner-notification').hide();
    let $frm = $btn.closest('form');
    if (!$frm.valid()) return;
    ajaxBtn.inProgress($btn);

    $.post($frm.attr('action'), $frm.serialize())
        .done(function (rep) {
            console.log(rep);
            if (rep.Success) {
                if (successFunc && typeof successFunc === 'function') successFunc(rep);
            }
            else {
                if (typeof errorFunc === 'function') errorFunc(rep);
                else {
                    if (useToastr) showNotif(notifyType.danger, rep.Message);
                    else $frm.inlineNotify(notifyType.danger, rep.Message);
                }
            }
            ajaxBtn.normal();
        })
        .fail(function (e) {
            if (useToastr) showNotif(notifyType.danger, strings.error);
            else $frm.inlineNotify(notifyType.danger, strings.error);

        });
};
/*--------------------------------------
            for simple ajax call
---------------------------------------*/
var ajaxCall = function ($elm, data, successFunc, errorFunc, loader, method) {
    loader = typeof loader !== 'undefined' ? loader : $threeDotLoader;
    method = typeof method !== 'undefined' ? method.toLowerCase() : 'post';
    data = typeof data !== 'undefined' ? data : {};
    let isBtnAction = $elm.hasClass('btn-action');
    if (isBtnAction)
        ajaxBtn.inProgress($elm);
    else {
        var elmHtml = $elm.html();
        $elm.html(loader);
    }
    $elm.prop('disabled', true);
    if (method === 'post') {
        $.post($elm.data('url'), data)
            .done(function (rep) {
                if (isBtnAction) ajaxBtn.normal();
                else $elm.html(elmHtml);

                $elm.prop('disabled', false);

                if (rep.Success) {
                    if (typeof successFunc === 'function') successFunc(rep);
                }
                else {
                    if (typeof errorFunc === 'function') errorFunc(rep.Message);
                    else showNotif(notifyType.danger, strings.error + ' (' + e.status + ')');
                }
            })
            .fail(function (e) {
                console.log(e);
                if (isBtnAction) ajaxBtn.normal();
                else $elm.html(elmHtml);

                $elm.prop('disabled', false);

                if (typeof errorFunc === 'function') errorFunc(strings.error);
                else showNotif(notifyType.danger, strings.error + '(' + e.status + ')');
            });
    }
    else {
        $.get($elm.data('url'), data)
            .done(function (rep) {
                if (isBtnAction) ajaxBtn.normal();
                else $elm.html(elmHtml);

                $elm.prop('disabled', false);
                if (rep.Success) {
                    if (typeof successFunc === 'function') successFunc(rep);
                }
                else {
                    if (typeof errorFunc === 'function') errorFunc(rep.Message);
                }
            })
            .fail(function (e) {
                if (isBtnAction) ajaxBtn.normal();
                else $elm.html(elmHtml);
                $elm.prop('disabled', false);
                if (typeof errorFunc === 'function') errorFunc(strings.error);
                else showNotif(notifyType.danger, strings.error + '(' + e.status + ')');
            });
    }
};
/*-----------------------------------------------------
            customized swal confirmed modal
-------------------------------------------------------*/
var swalConfirm = function (confirmedFunc, denyFunc) {
    swal({
        title: '',
        text: strings.AreYouSure,
        confirmButtonColor: "#4285F4",
        showCancelButton: true,
        confirmButtonText: strings.yes,
        cancelButtonText: strings.no
    },
        function (isConfirm) {
            if (isConfirm) {
                confirmedFunc();
            }
            else if (typeof denyFunc !== 'undefined') {
                denyFunc();
            }
        });
};
/*--------------------------------------
           ajax global error log
---------------------------------------*/
$(document).ajaxError(function (event, jqxhr, settings, thrownError) {
    console.log(jqxhr.responseText);
    try {
        if (jqxhr.status === 403 || jqxhr.status === 408)
            window.location.href = strings.signInUrl;
        else if (jqxhr.status === 401)
            showNotif(notifyType.danger, strings.unAuthorizedErrorMessage);

    }
    catch (e) { console.log(e); }
});

var customSerialize = function ($wrapper) {
    let model = {};
    $wrapper.find('input:not([type="checkbox"]):not([type="radio"]),select,textarea').each(function () {
        model[$(this).attr('name')] = $(this).val();
    });

    $wrapper.find('input[type="checkbox"],input[type="radio"]').each(function () {
        let name = $(this).attr('name');
        let val = $(this).attr('value').toLowerCase();
        if (!val || val === 'true' || val === 'false') val = $(this).prop('checked');
        if (!model[name]) {
            model[name] = val;
        }
        else {
            if (Array.isArray(model[name])) model[name].push(val);
            else model[name] = [model[name], val];
        }
    });
    return model;
};

var postObjectList = function ({ url, model, success, error }) {
    $.ajax({
        url: url,
        data: model,
        method: 'post',
        contentType: 'application/json; charset=utf-8;',
        success: function (rep) { if (success) success(rep); },
        error: function (e) { if (error) error(e); }
    });
};

const fileTypes = {
    Unknown: { id: 0, type: 'application/octet-stream' },
    Image: { id: 1, type: 'image/png' },
    Document: { id: 2, type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' },
    Archive: { id: 3, type: 'application/zip' },
    Audio: { id: 4, type: 'audio/mpeg' },
    Video: { id: 5, type: 'video/mp4' }
};

var getFileType = function (fileName) {
    let ext = fileName.toLowerCase().split('.').reverse()[0];
    switch (ext) {
        case "png":
        case "jpg":
        case "jpeg":
        case "gif":
        case "tiff":
            return fileTypes.Image;
        case "mp3":
        case "wav":
        case "flm":
        case "fsm":
        case "ogg":
        case "m4a":
        case "m4b":
        case "m4p":
        case "m4r":
            return fileTypes.Audio;
        case "mp4":
        case "mkv":
        case "avi":
        case "ts":
        case "m4v":
        case "flv":
            return fileTypes.Video;
        case "zip":
        case "rar":
        case "iso":
        case "tar":
        case "jar":
            return fileTypes.Archive;
        case "pdf":
        case "doc":
        case "docx":
        case "txt":
        case "xls":
        case "xlsx":
        case "josn":
        case "pptx":
            return fileTypes.Document;
        default:
            return fileTypes.Unknown;
    }
};

var getDefaultImageUrl = function (fileName) {
    let ext = fileName.toLowerCase().split('.').reverse()[0];
    switch (getFileType(ext).id) {
        case fileTypes.Image:
            return null;
        case fileTypes.Audio.id:
            return urlPrefix + '/Images/FileTypes/audio.png';
        case fileTypes.Video.id:
            return urlPrefix + '/Images/FileTypes/video.png';
        case fileTypes.Archive.id:
            return urlPrefix + '/Images/FileTypes/archive.png';
        case fileTypes.Document.id:
            return urlPrefix + '/Images/FileTypes/document.png';
        default:
            return urlPrefix + '/Images/FileTypes/unknown.png';
    }
};

var convertToOptionTags = function (items, isNullable) {
    let $optTags = '';
    if (isNullable) $optTags = '<option value="">' + strings.pleaseSelect + '</option>';
    $optTags = items.reduce(function (total, x) {
        return total + ('<option value="' + x.Value + '">' + x.Text + '</option>');
    }, $optTags);
    return $optTags;
};
