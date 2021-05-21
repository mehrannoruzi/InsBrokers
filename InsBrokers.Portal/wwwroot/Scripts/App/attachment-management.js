///<reference path="../Libs/jquery-3.1.1.min.js" />
var attachments = [];
$(document).ready(function () {
    let uploadTemplate = (id, url, name) => `<div class="uploaded">
        <button type="button" class="btn-remove" data-attch-id="${id}"><i class="zmdi zmdi-close"></i></button>
        <img src="${url}" />
        <span class="name">${name}</span>
    </div>`;
    $(document).on("click", ".uploader", function () {
        let $elm = $(this);
        $elm.find('input').trigger('click');

    });
    $(document).on("click", ".input-uploader", function (e) { e.stopPropagation(); });
    $(document).on("change", ".input-uploader", function (e) {
        let $elm = $(this);
        let $wrapper = $elm.closest(".uploader");
        let file = this.files[0];

        if (file) {
            let data = new FormData();
            data.append("file", file);
            data.append("type", $elm.data("type"));
            $wrapper.loadOverStart();
            $.ajax({
                type: "POST",
                url: attachmanetConfig.uploadUrl,
                data: data,
                cache: false,
                processData: false,
                contentType: false,
                success: function (rep) {
                    e.target.value = null;
                    $wrapper.loadOverStop();
                    console.log(rep);
                    if (rep.IsSuccessful) {
                        let id = rep.Result.UserAttachmentId || rep.Result.RelativeAttachmentId;
                        let url = window.URL.createObjectURL(file);
                        let $wrapper = $elm.closest('.upload-wrapper');
                        let template = uploadTemplate(id, url, file.name);
                        $wrapper.append(template);
                        attachments.push({ type: rep.Result.UserAttachmentType, id: id });
                        console.log(attachments);
                    }
                    else showNotif(notifyType.danger, rep.Message);
                },
                error: function (err) {
                    e.target.value = null;
                    $wrapper.loadOverStop();
                    console.log(err);
                    showNotif(notifyType.danger, "خطایی رخ داده است، لطفا دوباره تلاش نمایید");
                }
            })
        }

    });
    $(document).on("click", ".btn-remove", function (e) {
        let $elm = $(this);
        let id = $elm.data("attch-id");
        let $uploaded = $elm.closest(".uploaded");
        $uploaded.loadOverStart();
        $.post(attachmanetConfig.removeUrl, { attachmentId: id })
            .done(function (rep) {
                $uploaded.loadOverStop();
                if (!rep.IsSuccessful) {
                    showNotif(notifyType.danger, rep.Message);
                    return;
                }
                let idx = attachments.findIndex(x => x.id === id);
                if (~idx)
                    attachmemts.splice(idx, 1);
                $uploaded.remove();
            })
            .fail(function (e) {
                $uploaded.loadOverStop();
                showNotif(notifyType.danger, "خطایی رخ داده است، دوباره تلاش نمایید");
            });

        console.log(attachments);
    });
});