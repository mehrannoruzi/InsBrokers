﻿///<reference path="../../Libs/jquery-3.1.1.min.js" />

$(document).ready(function () {
    var attachmemts = [];
    let uploadTemplate = (url, name) => `<div class="uploaded">
        <button type="button" class="btn-remove"><i class="zmdi zmdi-close"></i></button>
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
                        let template = uploadTemplate(url, file.name);
                        $wrapper.append(template);
                        attachmemts.push(rep.Result.UserAttachmentId);
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
        let $uploaded = $elm.closest(".uploaded");
        let idx = $(".uploaded").index($uploaded);
        attachmemts.splice(idx, 1);
        $uploaded.remove();
        console.log(attachmemts);
    });
    $('#home-auth-page').on('click', '#btn-sign-up', function () {
        let $btn = $(this);
        let $frm = $btn.closest('form');
        if (!$frm.valid()) return;
        let model = customSerialize($frm);
        model.UserAttachmentIds = attachmemts;
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