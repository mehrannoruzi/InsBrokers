/// <reference path="../../Libs/jquery-3.1.1.min.js" />
var $fileBox = '<div class="col-12 col-sm-4 uploaded new m-b">' +
    '<button class="btn-remove"><i class="zmdi zmdi-close"></i></button>' +
    '<img src="{0}"/>' +
    '</div>';
var assets = [];
$(document).ready(function () {
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
                    console.log(data);
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

});

