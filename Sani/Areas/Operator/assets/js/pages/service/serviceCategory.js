// #region ServiceCategory
let serviceCatList = null;
$("#pid").on("change", (e) => {
    validateInput($(e.currentTarget), "لطفا یک مورد را انتخاب کنید.");
});
$("#title").on("keyup", (e) => {
    validateInput($(e.currentTarget), "این فیلد نمی تواند خالی باشد.");
});
//select all items
function ServiceCatList() {
    $.ajax({
        url: '/Service/ServiceCatList',
        type: 'POST',
        data: {},
        success: function (Resualt) {
            NoRecords(false);
            if (Resualt.length > 0) {
                serviceCatList = Resualt;
                let pList = $.grep(serviceCatList, (item) => {
                    return (item.id == item.pid)
                });
                var source = $('#serviceCatSource').html();
                var template = Handlebars.compile(source);
                var list = template({ serviceCatList: pList });
                $('#tbl-ServiceCategory tbody').html(list);
                $("#count").html(Resualt.length + " مورد ");
            } else {
                NoRecords(true);
            }
        },
        error: function () {
            notifiction(3, 'عدم فراخوانی رکورد ها');
        }
    })
}
//select childList
function childList(pid) {
    let cList = $.grep(serviceCatList, (item) => {
        return (item.pid == pid && item.id != pid);
    });
    var source = $('#serviceCatSource').html();
    var template = Handlebars.compile(source);
    var list = template({ serviceCatList: cList });
    $('#' + pid + ' td table tbody').html(list);
}
$("#tbl-ServiceCategory").delegate("a[data-toggle='collapse']", "click", (e) => {
    let dis = $(e.currentTarget);
    if (dis.children().hasClass("icon-arrow-left5")) {
        dis.children().removeClass("icon-arrow-left5");
        dis.children().addClass("icon-arrow-down5");
    } else {
        dis.children().removeClass("icon-arrow-down5");
        dis.children().addClass("icon-arrow-left5");
    }
});
//delete item
$("#tbl-ServiceCategory").delegate("a#delServiceCat", "click", function (e) {
    let dis = $(e.currentTarget),
        id = dis.attr("data-id"),
        title = dis.attr("data-title"),
        count = dis.attr("data-count"),
        childList = 0;
    $.each(serviceCatList, (i) => {
        if (serviceCatList[i].pid == id && serviceCatList[i].title != title) {
            childList++;
        }
    });
    if (count == "0" && childList == 0) {
        swal({
            title: "آیا از انجام این کار اطمینان دارید ؟",
            text: "شما قادر به بازیابی دوباره این رکورد نخواهید بود !",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#EF5350",
            confirmButtonText: "بله ، حذف شود",
            cancelButtonText: "خیر ، لغو شود",
            closeOnConfirm: false,
            closeOnCancel: false
        },
            function (isConfirm) {
                if (isConfirm) {
                    $.ajax({
                        url: '/Service/DeleteServiceCat',
                        type: 'POST',
                        data: { id: id },
                        success: function (Resualt) {
                            if (Resualt == "عملیات با موفقیت انجام شد.") {
                                ServiceCatList();
                                swal({
                                    title: "عملیات حذف انجام شد .",
                                    text: "رکورد مد نظر شما با موفقیت حذف گردید .",
                                    confirmButtonColor: "#66BB6A",
                                    confirmButtonText: "باشه",
                                    type: "success"
                                });
                            } else {
                                notifiction(1, Resualt);
                            }
                        },
                        error: function (Resualt) {
                            notifiction(1, Resualt);
                        }
                    });
                }
                else {
                    swal({
                        title: "در خواست لغو شد.",
                        text: "رکورد شما در صحت کامل قرار دارد :)",
                        confirmButtonColor: "#66BB6A",
                        confirmButtonText: "باشه",
                        type: "success"
                    });
                }
            });
    }
    else {
        notifiction(3, 'شما تنها قادر به حذف دسته هایی هستید که زیر دسته‌ای نداشته و تعداد اختصاص هایشان برابر با صفر باشد.');
    }
});
//edit item
let addEditServiceCatGet = (e) => {
    let form = $("#frmServiceCat");
    let t = $(e.currentTarget);
    let id = t.attr("data-id");
    let title = t.attr("data-title");
    let pid = t.attr("data-pid");
    let target = $("#multi-select");
    let name = "pid";

    inputRemoveError($("#pid"));
    inputRemoveError($("#price"));

    //add mode
    if (title == undefined && pid == undefined) {
        form.find("fieldset .form-group:nth-child(2)").removeClass("hidden");
        clearMultiSelect(target, name);
        $.each(serviceCatList, (i) => {
            fillSelectMulti(serviceCatList, serviceCatList[i], emptyId(), target, name);
        });
        target.prop("disabled", false);
        $("#id").val("");
        $("#title").val("");
        $("#addEditCat .modal-title").html("افزودن دسته جدید");
    }
    //edit mode
    else {
        form.find("fieldset .form-group:nth-child(2)").addClass("hidden");
        $("#id").val(id);
        $("#title").val(title);
        $("#addEditCat .modal-title").html(" ویرایش دسته " + title);
    }
};
$("#addEditServiceCat").on("click", (e) => {
    addEditServiceCatGet(e);
});
$("#tbl-ServiceCategory").delegate("a#addEditServiceCat", "click", (e) => {
    addEditServiceCatGet(e);
});
$("#btnAddEditServicat").on("click", () => {
    let modal = $("#addEditCat");
    let form = $("#frmServiceCat")
    let inputTitle = $("#title");
    let isInputTitleValid = validateInput(inputTitle, "این فیلد نمی تواند خالی باشد.","");
    if (isInputTitleValid) {
        const TblServiceCategory = form.serialize();
        $("modal-content").block({
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
        $.ajax({
            url: '/Service/AddEditServiceCat',
            type: 'POST',
            data: TblServiceCategory,
            success: function (Resualt) {
                debugger;
                if (Resualt == "عملیات با موفقیت انجام شد.") {
                    $("modal-content").unblock();
                    notifiction(0, Resualt);
                    modal.modal('hide');
                    ServiceCatList();
                }
                else {
                    notifiction(1, Resualt);
                }
            },
            error: function (Resualt) {
                modal.unblock();
                notifiction(3, Resualt);
            }
        })
    }
});
//#endregion ServiceCategory