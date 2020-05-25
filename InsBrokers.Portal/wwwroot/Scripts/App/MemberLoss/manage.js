/// <reference path="../../Libs/jquery-3.1.1.min.js" />
var $fileBox = '<div class="col-12 col-sm-4 uploaded new">' +
    '<button class="btn-remove"><i class="zmdi zmdi-close"></i></button>' +
    '<img class="w-100" src="{0}"/>' +
    '</div>';
var assets = [];
$(document).ready(function () {
    $('#modal').on('change', '#LossType', function (event) {
        $('#asset-description').text($('#modal #LossType option:selected').data('description'));
    });
    $('#modal').on('click', '.btn-remove', function (e) {
        e.stopPropagation();
        let $btn = $(this);
        let $box = $btn.closest('.uploaded');

        let removeUrl = $btn.data('url');
        console.log(removeUrl);
        if (removeUrl) {
            ajaxBtn.inProgress($btn);
            $.post(removeUrl)
                .done(function (data) {
                    ajaxBtn.normal();
                    if (data.IsSuccessful) {
                        $box.remove();
                    }
                    else showNotif(notifyType.danger, data.Message);
                })
                .fail(function (e) {
                    ajaxBtn.normal();
                });
        }
        else {
            let idx = $('#modal .uploaded.new').index($box);
            $box.remove();
            assets.splice(idx, 1);
        }
    });
    $('#modal').on('click', '#uploader', function (e) {
        e.stopPropagation();
        $('#input-file').trigger('click');
    });
    $('#modal').on('change', '#input-file', function (event) {
        event.stopPropagation();
        var $i = $(this);
        var file = this.files[0];
        var reader = new FileReader();
        reader.onload = function (e) {
            var fileType = getFileType(file.name);
            var url = '';
            if (fileType === fileTypes.Image) url = e.target.result;
            else url = getDefaultImageUrl(file.name);

            $('#files').append($fileBox.replace('{0}', url));
            $i.val('');
            assets.push(file);
        };
        reader.readAsDataURL(file);
        //$('').trigger('click');
    });

    //submit view
    $(document).on('click', '.btn-submit-loss', function () {
        let $btn = $(this);
        let $frm = $(this).closest('form');
        let frmData = new FormData();
        var model = customSerialize($frm);
        for (var k in model)
            frmData.append(k, model[k]);
        for (var i = 0; i < assets.length; i++)
            frmData.append('Files', assets[i]);
        console.log(frmData);
        console.log(assets);
        ajaxBtn.inProgress($btn);
        $.ajax({
            type: 'POST',
            url: $frm.attr('action'),
            //contentType: 'application/json; charset=utf-8;',
            data: frmData,
            contentType: false,
            processData: false,
            success: function (rep) {
                ajaxBtn.normal();
                if (!rep.IsSuccessful)
                    showNotif(notifyType.danger, rep.Message);
                else {
                    $('#modal').modal('hide');
                }
            },
            error: function (e) {
                ajaxBtn.normal();

            }
        });
    });

});

