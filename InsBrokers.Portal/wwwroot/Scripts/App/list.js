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

