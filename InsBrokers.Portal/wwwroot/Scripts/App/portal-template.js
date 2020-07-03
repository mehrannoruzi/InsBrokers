///<reference path="../Libs/jquery-3.1.1.min.js" />

$(document).ready(function () {
    setActiveNavLink();
});

function setActiveNavLink() {
    if (location.pathname === '/') {
        $('header .navlink a').first().parent().addClass('current');
        return;
    }
    let url = window.location.href;
    $('header .navlink a:not([href="/"])').each(function () {
        let href = $(this).attr('href');
        if (url.indexOf(href) > -1) {
            $(this).parent().addClass('current');
            return false;
        }
    });
}