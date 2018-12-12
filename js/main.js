document.addEventListener("DOMContentLoaded", function () {
    var elements = document.getElementsByTagName("INPUT");
    for (var i = 0; i < elements.length; i++) {
        elements[i].oninvalid = function (e) {
            e.target.setCustomValidity("");
            if (!e.target.validity.valid) {
                e.target.setCustomValidity("این فیلد نمی تواند خالی باشد");
            }
        };
        elements[i].oninput = function (e) {
            e.target.setCustomValidity("");
        };
    }
})
$(document).ready(function () {
    $(window).scroll(function () {
        var topoffset = $(window).scrollTop(),
            size = $(window).width(),
            nav = $("nav");
        if (topoffset > 85 && size > 1024) {
            nav.addClass('fixed');
        }
        else if (size > 1024) {
            nav.removeClass('fixed');
        }
    });
    $('.button-collapse').sideNav({
        menuWidth: 240,
        edge: 'right',
        closeOnClick: true,
        draggable: false
    });
});




