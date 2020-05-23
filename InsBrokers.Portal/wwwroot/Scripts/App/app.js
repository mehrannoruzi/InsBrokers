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
///<reference path="../Libs/jquery-3.1.1.min.js" />
$(document).ready(function () {
    //Search
    $(document).on('click', 'button.search', function () {
        refreshList();
    });

    //refresh
    $(document).on('click', '.list .refresh-link', function () {
        refreshList();
    });

    //Update
    $(document).on('click', '.list .actions .update', function () {
        let $elm = $(this);
        showModal({
            $elm: $elm,
            beforeFunc: function () {
                ajaxActionLink.inProgress($elm);
            },
            afterFunc: function () {
                ajaxActionLink.normal();
            },
            errorFunc: function () {
                ajaxActionLink.normal();
            }
        });
    });

    //Delete
    $(document).on('click', '.list .actions .delete,.list .actions .action', function () {
        let $elm = $(this);
        swalConfirm(function () {
            ajaxActionLink.inProgress($elm);
            $.post($elm.data('url'))
                .done(function (rep) {
                    ajaxActionLink.normal();
                    if (rep.IsSuccessful) refreshList();
                    else swal("", rep.Message, "error");
                })
                .fail(function (e) {
                    ajaxActionLink.normal();
                    swal("", strings.error, "error");
                });
        });
    });


    //page size change
    $(document).on('click', '.list .page-size a', function () {
        $('#pagesize').val($(this).text());
        refreshList();
    });

    //paging
    $(document).on('click', '.pagination > li > a:not(.disabled)', function () {
        refreshList($(this).data('number'));
    });

});

var refreshList = function (pageNumber) {
    pageNumber = typeof pageNumber !== 'undefined' ? pageNumber : 1;
    let $wrapper = $('.list');
    let $frm = $('.list').closest('form');
    let params = $frm.serialize();
    params += "&PageNumber=" + pageNumber;
    $wrapper.loadOverStart();
    $.get($frm.attr('action'), params)
        .done(function (rep) {
            $wrapper.loadOverStop();
            $wrapper.find('.ibox-content').html(rep);
            //enable footable plugin on items tables
            $('.footable').footable({
                pageSize: 200,
                "breakpoints": {
                    phone: 480,
                    tablet: 1024
                }
            });
        })
        .fail(function (e) {
            $wrapper.loadOverStop();
        });
};

var ajaxActionLink = new (function () {
    var ins = function () { };
    var $elm = null, elmHtml = null;
    ins.prototype.inProgress = function ($a) {
        $elm = $a.closest('.dropdown').find('[data-toggle="dropdown"]');
        elmHtml = $elm.html();
        $elm.html($circularLoader);
    };
    ins.prototype.normal = function () {
        $elm.html(elmHtml);
    };
    return ins;
}());


/*
 *
 *   INSPINIA - Responsive Admin Theme
 *   version 2.8
 *
 */

$(document).ready(function () {

    // Fast fix bor position issue with Propper.js
    // Will be fixed in Bootstrap 4.1 - https://github.com/twbs/bootstrap/pull/24092
    Popper.Defaults.modifiers.computeStyle.gpuAcceleration = false;


    // Add body-small class if window less than 768px
    if ($(window).width() < 769) {
        $('body').addClass('body-small');
    } else {
        $('body').removeClass('body-small');
    }

    // MetisMenu
    var sideMenu = $('#side-menu').metisMenu();

    sideMenu.on('shown.metisMenu', function (e) {
        fix_height();
    });
    // Collapse ibox function
    $(document).on('click', '.collapse-link', function (e) {
        e.preventDefault();
        var ibox = $(this).closest('div.ibox');
        var button = $(this).find('i');
        var content = ibox.children('.ibox-content');
        content.slideToggle(200);
        button.toggleClass('zmdi-chevron-up').toggleClass('zmdi-chevron-down');
        ibox.toggleClass('').toggleClass('border-bottom');
        setTimeout(function () {
            ibox.resize();
            ibox.find('[id^=map-]').resize();
        }, 50);
    });
    // Close ibox function
    $(document).on('click', '.close-link', function (e) {
        e.preventDefault();
        var content = $(this).closest('div.ibox');
        content.remove();
    });

    // Fullscreen ibox function
    $(document).on('click', '.fullscreen-link', function (e) {
        e.preventDefault();
        var ibox = $(this).closest('div.ibox');
        var button = $(this).find('i');
        $('body').toggleClass('fullscreen-ibox-mode');
        button.toggleClass('fa-expand').toggleClass('fa-compress');
        ibox.toggleClass('fullscreen');
        setTimeout(function () {
            $(window).trigger('resize');
        }, 100);
    });

    // Close menu in canvas mode
    $(document).on('click', '.close-canvas-menu', function (e) {
        e.preventDefault();
        $("body").toggleClass("mini-navbar");
        SmoothlyMenu();
    });

    // Run menu of canvas
    $('body.canvas-menu .sidebar-collapse').slimScroll({
        height: '100%',
        railOpacity: 0.9
    });

    // Open close right sidebar
    $('.right-sidebar-toggle').on('click', function (e) {
        e.preventDefault();
        $('#right-sidebar').toggleClass('sidebar-open');
    });

    // Initialize slimscroll for right sidebar
    $('.sidebar-container').slimScroll({
        height: '100%',
        railOpacity: 0.4,
        wheelStep: 10
    });

    // Open close small chat
    $('.open-small-chat').on('click', function (e) {
        e.preventDefault();
        $(this).children().toggleClass('fa-comments').toggleClass('fa-times');
        $('.small-chat-box').toggleClass('active');
    });

    // Initialize slimscroll for small chat
    $('.small-chat-box .content').slimScroll({
        height: '234px',
        railOpacity: 0.4
    });

    // Small todo handler
    $('.check-link').on('click', function () {
        var button = $(this).find('i');
        var label = $(this).next('span');
        button.toggleClass('fa-check-square').toggleClass('fa-square-o');
        label.toggleClass('todo-completed');
        return false;
    });

    // Append config box / Only for demo purpose
    // Uncomment on server mode to enable XHR calls
    //$.get("skin-config.html", function (data) {
    //    if (!$('body').hasClass('no-skin-config'))
    //        $('body').append(data);
    //});

    // Minimalize menu
    $('.navbar-minimalize').on('click', function (event) {
        event.preventDefault();
        $("body").toggleClass("mini-navbar");
        SmoothlyMenu();

    });

    // Tooltips demo
    $('.tooltip-demo').tooltip({
        selector: "[data-toggle=tooltip]",
        container: "body"
    });


    // Move right sidebar top after scroll
    $(window).scroll(function () {
        if ($(window).scrollTop() > 0 && !$('body').hasClass('fixed-nav')) {
            $('#right-sidebar').addClass('sidebar-top');
        } else {
            $('#right-sidebar').removeClass('sidebar-top');
        }
    });

    $("[data-toggle=popover]")
        .popover();

    // Add slimscroll to element
    $('.full-height-scroll').slimscroll({
        height: '100%'
    });
});



// Fixed Sidebar
$(window).bind("load", function () {
    if ($("body").hasClass('fixed-sidebar')) {
        $('.sidebar-collapse').slimScroll({
            height: '100%',
            railOpacity: 0.9
        });
    }
});

function fix_height() {
    var heightWithoutNavbar = $("body > #wrapper").height() - 62;
    $(".sidebar-panel").css("min-height", heightWithoutNavbar + "px");

    var navbarheight = $('nav.navbar-default').height();
    var wrapperHeight = $('#page-wrapper').height();

    if (navbarheight > wrapperHeight) {
        $('#page-wrapper').css("min-height", navbarheight + "px");
    }

    if (navbarheight < wrapperHeight) {
        $('#page-wrapper').css("min-height", $(window).height() + "px");
    }

    if ($('body').hasClass('fixed-nav')) {
        if (navbarheight > wrapperHeight) {
            $('#page-wrapper').css("min-height", navbarheight + "px");
        } else {
            $('#page-wrapper').css("min-height", $(window).height() - 60 + "px");
        }
    }

}

$(window).bind("load resize scroll", function () {

    // Full height of sidebar
    setTimeout(function () {
        if (!$("body").hasClass('body-small')) {
            fix_height();
        }
    });

});

// Minimalize menu when screen is less than 768px
$(window).bind("resize", function () {
    if ($(this).width() < 769) {
        $('body').addClass('body-small');
    } else {
        $('body').removeClass('body-small');
    }
});

// Local Storage functions
// Set proper body class and plugins based on user configuration
$(document).ready(function () {
    if (localStorageSupport()) {

        var collapse = localStorage.getItem("collapse_menu");
        var fixedsidebar = localStorage.getItem("fixedsidebar");
        var fixednavbar = localStorage.getItem("fixednavbar");
        var boxedlayout = localStorage.getItem("boxedlayout");
        var fixedfooter = localStorage.getItem("fixedfooter");

        var body = $('body');

        if (fixedsidebar === 'on') {
            body.addClass('fixed-sidebar');
            $('.sidebar-collapse').slimScroll({
                height: '100%',
                railOpacity: 0.9
            });
        }

        if (collapse === 'on') {
            if (body.hasClass('fixed-sidebar')) {
                if (!body.hasClass('body-small')) {
                    body.addClass('mini-navbar');
                }
            } else {
                if (!body.hasClass('body-small')) {
                    body.addClass('mini-navbar');
                }

            }
        }

        if (fixednavbar === 'on') {
            $(".navbar-static-top").removeClass('navbar-static-top').addClass('navbar-fixed-top');
            body.addClass('fixed-nav');
        }

        if (boxedlayout === 'on') {
            body.addClass('boxed-layout');
        }

        if (fixedfooter === 'on') {
            $(".footer").addClass('fixed');
        }
    }
});

// check if browser support HTML5 local storage
function localStorageSupport() {
    return (('localStorage' in window) && window['localStorage'] !== null);
}

// For demo purpose - animation css script
function animationHover(element, animation) {
    element = $(element);
    element.hover(
        function () {
            element.addClass('animated ' + animation);
        },
        function () {
            //wait for animation to finish before removing classes
            window.setTimeout(function () {
                element.removeClass('animated ' + animation);
            }, 2000);
        });
}

function SmoothlyMenu() {
    if (!$('body').hasClass('mini-navbar') || $('body').hasClass('body-small')) {
        // Hide menu in order to smoothly turn on when maximize menu
        $('#side-menu').hide();
        // For smoothly turn on menu
        setTimeout(
            function () {
                $('#side-menu').fadeIn(400);
            }, 200);
    } else if ($('body').hasClass('fixed-sidebar')) {
        $('#side-menu').hide();
        setTimeout(
            function () {
                $('#side-menu').fadeIn(400);
            }, 100);
    } else {
        // Remove all inline style from jquery fadeIn function to reset menu state
        $('#side-menu').removeAttr('style');
    }
}

// Dragable panels
function WinMove() {
    var element = "[class*=col]";
    var handle = ".ibox-title";
    var connect = "[class*=col]";
    $(element).sortable(
        {
            handle: handle,
            connectWith: connect,
            tolerance: 'pointer',
            forcePlaceholderSize: true,
            opacity: 0.8
        })
        .disableSelection();
}



var fireGlobalPlugins = function () {
    //fireDropzone();

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

    $("select.select2:not(.with-ajax)").each(function () {
        if (!$(this).data('select2-fired')) {
            let nullable = $(this).find('option[value=""]').length > 0;
            $(this).data('select2-fired', true).select2({
                placeholder: strings.pleaseSelect,
                allowClear: nullable,
                "language": {
                    "noResults": function () {
                        return strings.thereIsNoResult;
                    }
                }
            });
        }

    });

    $('select.select2.with-ajax').each(function () {
        if (!$(this).data('select2-fired')) {
            let $elm = $(this).prop({ 'autocomnplete': 'off', 'autocorrect': 'off' });
            let minimumInputLength = typeof $elm.data('min-length') !== 'undefined' ? $elm.data('min-length') : 2;
            let nullable = $(this).find('option[value=""]').length > 0;
            $elm.data('select2-fired', true).select2({
                placeholder: strings.pleaseSelect,
                searchInputPlaceholder: strings.searchHere,
                allowClear: nullable,
                language: {
                    noResults: function () {
                        return strings.thereIsNoResult;
                    },
                    searching: function () { return strings.searching; },
                    inputTooShort: function () { return ''; }
                },
                minimumInputLength: parseInt(minimumInputLength),
                ajax: {
                    url: $elm.attr('data-url'),
                    dataType: 'json',
                    data: function (params) {
                        var query = {
                            q: params.term
                        };
                        return query;
                    },
                    processResults: function (data) {
                        return {
                            results: data.map(x => ({ text: x.Text, id: x.Value }))
                        };
                    }
                }
            });
        }

    });

    //place holder for select2 input plugin
    //$(document).on("select2:open", function (event) {
    //    $('input.select2-search__field').attr('placeholder', strings.typeHere);
    //});
};

/*--------------------------------------
            notifications
---------------------------------------*/
$.fn.inlineNotify = function (type, message) {
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

var showNotif = function (type, message) {
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
        let opt = {
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
                if (rep.IsSuccessful === true) {
                    file.url = rep.Result;
                    if (typeof $elm.data('success-func') !== 'undefined') {
                        eval($elm.data('success-func'))(rep.Result);
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
        //if (typeof $elm.data('accept') !== 'undefined')
        //    eval($elm.data('accept'))(file);
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
                        if (!rep.IsSuccessful) {
                            drop.files.push(file);
                            drop.emit("addedfile", file);
                            drop.emit("thumbnail", file, file.url);
                            drop.emit('complete', file);
                            showNotif(notifyType.danger, rep.Message);
                        }
                        else if (typeof $elm.data('remove-func') !== 'undefined') {
                            eval($elm.data('remove-func'))(rep.Result);
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

    $('.multi-uploader').each(function () {
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

    if (typeof fireGlobalPlugins === 'function') fireGlobalPlugins();


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
        type: 'post',
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
