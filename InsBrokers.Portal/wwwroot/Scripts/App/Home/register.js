///<reference path="../../Libs/jquery-3.1.1.min.js" />

$(document).ready(function () {
    var attachmemts = [];
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
                url: config.uploadUrl,
                data: data,
                cache: false,
                processData: false,
                contentType: false,
                success: function (rep) {
                    e.target.value = null;
                    $wrapper.loadOverStop();
                    console.log(rep);
                    if (rep.IsSuccessful) {
                        let url = window.URL.createObjectURL(file);
                        let $wrapper = $elm.closest('.upload-wrapper');
                        let template = uploadTemplate(rep.Result.UserAttachmentId, url, file.name);
                        $wrapper.append(template);
                        attachmemts.push({ type: rep.Result.UserAttachmentType, id: rep.Result.UserAttachmentId });
                        console.log(attachmemts);
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
        //let idx = $(".uploaded").index($uploaded);
        let idx = attachments.findIndex(x => x.id === id);
        if (~idx) {
            attachmemts.splice(idx, 1);
            $uploaded.remove();
        }
        console.log(attachmemts);
    });
    const validateFiles = () => {
        let types = [];
        $(".input-uploader").each(function () { types.push($(this).data("type")); });
        let isValid = true;
        for (let t of types) {
            if (!attachmemts.some(x => x.type === t)) isValid = false;
        }
        if (!isValid)
            showToast(notifyType.danger, "لطفا فایلهای مورد نیاز را آپلود نمایید");
        return isValid;
    }
    $('#home-auth-page').on('click', '#btn-sign-up', function () {

        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) return;
        if (!validateFiles()) return;
        let model = customSerialize($frm);
        model.UserAttachmentIds = attachmemts.map(x => x.id);
        ajaxBtn.inProgress($btn);
        console.log(model);
        $.ajax({
            type: 'POST',
            url: $frm.attr('action'),
            contentType: 'application/json; charset=utf-8;',
            data: JSON.stringify(model),
            success: function (data) {
                ajaxBtn.normal();
                if (data.IsSuccessful) {
                    window.location.href = data.Result;
                }
                else {
                    showToast(notifyType.danger, data.Message);
                }
            },
            error: function (e) {
                console.log(e);
            }
        });
    });
});