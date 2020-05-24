///<reference path="../Libs/jquery-3.1.1.min.js" />
var $threeDotLoader = '<span class="three-dot-loader"><span class="dot"></span><span class="dot"></span><span class="dot"></span></span>';
var $circularLoader = '<div class="spinner"><svg viewBox="25 25 50 50"><circle cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10"></circle></svg></div>';
var notifyType = {
    success: "success",
    danger: "danger",
    info: "info",
    warning: "warning"
};


$(document).ready(function () {

    var setActiveMenu = function () {
        let currentUrl = window.location.href.toString().toLowerCase();
        $('nav ul.nav > li:not(".nav-header")  a:not([href="#"])').each(function () {
            var linkUrl = $(this).attr('href');
            if (currentUrl.endsWith(linkUrl.toLowerCase().split('?')[0])) {
                $(this).closest('li').addClass('active').closest('.link-parent').addClass('active');
                let $parentUl = $(this).closest('ul.nav-second-level');
                if ($parentUl.length > 0) {
                    $parentUl.addClass('in');
                }
                return false;
            }
        });
    }();

    fireGlobalPlugins();

    //auto submit form with ajax
    $(document).on('click', '[data-ajax-submit="true"]', function () {
        let $btn = $(this);
        submitAjaxForm($btn, function (rep) { showNotif(notifyType.success, strings.success); });
    });

    //open close filter ibox
    $(document).on('click', '.filters .ibox-title', function (e) {
        var $ibox = $(this).closest('div.ibox');
        var button = $(this).find('.collapse-icon i');
        let nowClosed = button.hasClass('zmdi-chevron-down');
        var content = $ibox.children('.ibox-content');
        content.slideToggle(200);
        button.toggleClass('zmdi-chevron-up').toggleClass('zmdi-chevron-down');
        $ibox.toggleClass('').toggleClass('border-bottom');
        setTimeout(function () {
            $ibox.resize();
            $ibox.find('[id^=map-]').resize();
        }, 50);
        let $filters = $(this).closest('.filters');
        if ($ibox.hasClass('fixed-ibox'))
            return;
        if (nowClosed) {
            $ibox.animate({ 'margin-left': '-=125px' });
            $filters.animate({ 'padding-top': '+=40px' });
        }
        else {
            $ibox.animate({ 'margin-left': '+=125px' });
            $filters.animate({ 'padding-top': '-=40px' });
        }
    });

    //call appropriate action with json response via ajax
    $(document).on('click', '[data-ajax-action]', function () {
        let $elm = $(this);
        let type = 'post';
        if ($elm.data('type'))
            tyep = $elm.data('type');
        let $content = $elm.html();
        $elm.html($circularLoader);
        $.ajax({
            type: type,
            url: $elm.data('ajax-action'),
            contentType: 'json',
            success: function (rep) {
                $elm.html($content);
                if (rep.IsSuccessful) showNotif(notifyType.success, strings.success);
                else showNotif(notifyType.danger, rep.Message);
            },
            error: function () {
                $elm.html($content);
                showNotif(notifyType.danger, strings.error);
            }
        });
    });

    //load appropriate partial to load in target element
    $(document).on('click', '[data-ajax-load]', function () {
        let $target = $('.content-wrapper');
        if ($(this).data('target')) $target = $($(this).data('target'));
        $target.loadOverStart();
        $.get($(this).data('ajax-load'))
            .done(function (rep) {
                $target.html(rep).loadOverStop();
            })
            .fail(function (e) {
                $target.loadOverStop();
            });
    });

    //change default validation messages 
    jQuery.extend(jQuery.validator.messages, {
        required: validationMessages.required,
        remote: validationMessages.remote,
        email: validationMessages.email,
        url: validationMessages.url,
        date: validationMessages.date,
        dateISO: validationMessages.dateISO,
        number: validationMessages.number,
        digits: validationMessages.digits,
        creditcard: validationMessages.creditcard,
        equalTo: validationMessages.equalTo,
        accept: validationMessages.accept,
        maxlength: jQuery.validator.format(validationMessages.maxlength),
        minlength: jQuery.validator.format(validationMessages.minlength),
        rangelength: jQuery.validator.format(validationMessages.rangelength),
        range: jQuery.validator.format(validationMessages.range),
        max: jQuery.validator.format(validationMessages.max),
        min: jQuery.validator.format(validationMessages.min)
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
    let model = customSerialize($frm, true);
    console.log(model);
    $.post($frm.attr('action'), model)
        .done(function (rep) {
            if (rep.IsSuccessful) {
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
            ajaxBtn.normal();
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

                if (rep.IsSuccessful) {
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
                if (rep.IsSuccessful) {
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

var customSerialize = function ($wrapper, checkNumbers) {
    let model = {};
    $wrapper.find('input:not([type="checkbox"]):not([type="radio"]),select,textarea').each(function () {
        if (checkNumbers && !isNaN($(this).val()))
            model[$(this).attr('name')] = parseInt($(this).val());
        else
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

var postObjectList = function (url, model, success, error) {
    console.log('model5:');
    console.log(JSON.stringify(model));
    $.ajax({
        type: 'POST',
        url: url,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(model),
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
