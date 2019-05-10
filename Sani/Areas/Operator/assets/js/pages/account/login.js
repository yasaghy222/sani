$(() => {
    let notifiction = (type, message) => {
        switch (type) {
            case 0:
                $.jGrowl(message, { theme: 'alert-styled-left alert-arrow-left bg-success', header: 'ورود موفق', position: 'bottom-right', life: 5000 });
                break;
            case 1:
                $.jGrowl(message, { theme: 'alert-styled-left alert-arrow-left bg-danger', header: 'عدم موفقیت', position: 'bottom-right', life: 10000 });
                break;
            default:
                $.jGrowl(message, { theme: 'alert-styled-left alert-arrow-left bg-warning', header: 'هشدار', position: 'bottom-right', life: 8000 });
                break;
        }
    },
        msg = () => {
            let content = $("body");
            content.block({
                message: '<i class="icon-spinner2 spinner text-grey-300 text-size-xlarge"></i>',
                overlayCSS: {
                    backgroundColor: '#fff',
                    opacity: 0.8,
                    cursor: 'wait',
                    'box-shadow': '0 0 0 1px #ddd'
                },
                css: {
                    border: 0,
                    padding: 0,
                    backgroundColor: 'none'
                }
            });
            window.setTimeout(function () {
                content.unblock();
            }, 2000);
        },
        reload = () => {
            const panel = $(".content .panel");
            $(panel).block({
                message: '<i class="icon-spinner2 spinner"></i>',
                overlayCSS: {
                    backgroundColor: '#fff',
                    opacity: 0.8,
                    cursor: 'wait',
                    'box-shadow': '0 0 0 1px #ddd'
                },
                css: {
                    border: 0,
                    padding: 0,
                    backgroundColor: 'none'
                }
            });
            window.setTimeout(function () {
                $(panel).unblock();
            }, 2000);
        },
        validateForm = (form) => {
            let formValid = form.valid();
            if (formValid) {
                return true;
            }
            else {
                return false;
            }
        },
        form = $("#frmLogin");

    $("#login").on("click", () => {
        let isFormValid = validateForm(form),
            loginVar = form.serialize();
        if (isFormValid) {
            $.ajax({
                url: '/Account/Login',
                data: loginVar,
                type: 'POST',
                success: function (Resualt) {
                    if (Resualt != "success") {
                        notifiction(3, Resualt);
                    }
                    else {
                        notifiction(0, "در حال هدایت به پنل مدیریت.");
                        window.location.replace("/SS-OManage");
                    }
                },
                error: function () {
                    notifiction(3, 'مشکلی به وجود آمده است لطفا زمان دیگری اقدام نمایید.');
                }
            });
        }
    });
})