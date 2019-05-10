$(() => {
    // #region General
    $(".styled").uniform({
        radioClass: 'choice'
    });
    $('.maxlength').maxlength({
        placement: 'bottom-left'
    });
    $(".touchspin").TouchSpin({
        min: 0,
        max: 100,
        step: 5
    });
    //#endregion

    // #region AddEditEShop
    let form = $("#TblEShop"),
        validatePhoneNumber = (t) => {
            let tempvalid = false;
            if (t != undefined && t.val() != "") {
                let reg = /(\+98|0|e*|E*)?9\d{9}/gm;
                if (validateInput(t, "فرمت ورودی صحیح نمی باشد.", "", reg)) {
                    $.ajax({
                        url: '/EShop/IsExist',
                        type: 'POST',
                        data: { id: $("#id").val(), phoneNumber: t.val() },
                        success: function (Resualt) {
                            if (Resualt) {
                                return validateInput(t, "این شماره قبلا ثبت شده است.", t.val());
                            }
                            else {
                                inputRemoveError(t);
                            }
                        },
                        error: function (Resualt) {
                            modal.unblock();
                            notifiction(3, Resualt);
                        }
                    });
                    tempvalid = true;
                }
            }
            else {
                if ($("#managerPhoneNumber").val() != "") {
                    tempvalid = validatePhoneNumber($("#managerPhoneNumber"));
                }
                else {
                    tempvalid = validateInput($("#managerPhoneNumber"), "این فیلد نمی تواند خالی باشد.");
                }
            }
            return tempvalid;
        },
        validateTelNumber = (t) => {
            let tempvalid = false;
            if (t.val() != "") {
                let reg = /(\+98|0|e*|E*)\d{9}/gm;
                tempvalid = validateInput(t, "فرمت ورودی صحیح نمی باشد.", "", reg);
            }
            else {
                tempvalid = validateInput(t, "این فیلد نمی تواند خالی باشد.");
            }
            return tempvalid;
        },
        saveForm = (e) => {
            let select = $("#bCode"),
                formValid = form.valid(),
                isBCodeValid = validateInput(select, "لطفا یک مورد را انتخاب کنید.", "0"),
                isPhoneValid = validatePhoneNumber(),
                isManagerTelValid = validateTelNumber($("#managerTel")),
                isshopTellValid = validateTelNumber($("#shopTel")),
                isValid = false;
            if (formValid && isBCodeValid && isPhoneValid && isshopTellValid && isManagerTelValid) {
                isValid = true;
            }
            if (isValid) {
                btnSaveOnOff();
                $(".tab-content").block({
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
                let viewTblEShop = form.serialize(),
                    targetId = $(e.currentTarget).attr("id");

                $.ajax({
                    url: '/EShop/AddEdit',
                    type: 'POST',
                    data: viewTblEShop,
                    success: function (Resualt) {
                        if (Resualt.resualt == "عملیات با موفقیت انجام شد.") {
                            $("#id").val(Resualt.id);
                            $(".tab-content").unblock();
                            btnSaveOnOff();
                            notifiction(0, Resualt.resualt);
                            if (targetId == "btnSaveNext") {
                                pageLoad('/Operator/EShop', 'لیست تعرفه ها', 'serviceList');
                            }
                        } else {
                            notifiction(1, Resualt);
                            $(".tab-content").unblock();
                        }
                    },
                    error: function (Resualt) {
                        $(".tab-content").unblock();
                        notifiction(1, Resualt);
                    }
                });
            }
        };
    $(".tabbable .nav-tabs li a").on("click", (e) => {
        let t = e.currentTarget;
        let isActive = $(t).parent().hasClass("active");
        let dataTab = $(t).attr("data-tab");
        let id = $("#id").val();
        if (!isActive) {
            if (id == "00000000-0000-0000-0000-000000000000") {
                $.jGrowl('ابتدا مشخصات الکتریکی را ثبت نمائید.', { theme: 'alert-styled-left alert-arrow-left bg-warning', header: 'هشدار', position: 'bottom-right', life: 10000 });
            }
            else {
                swichTab(t);
                btnSaveOnOff();
                switch (dataTab) {
                    case "expertise":
                        expList();
                        break;
                    case "documents":
                        docList();
                    default:
                        break;
                }
            }
        }
    });
    form.delegate("#bCode", "change", (e) => {
        validateInput($(e.currentTarget), "لطفا یک مورد را انتخاب کنید.", "0");
    });
    form.delegate("#managerPhoneNumber", "keyup", (e) => {
        let t = $(e.currentTarget);
        inputRemoveError(t);
        validateInput(t, "این فیلد نمی تواند خالی باشد.");
        if ((e.which && e.which == 9) || (e.keyCode && e.keyCode == 9)) {
            validatePhoneNumber(t);
        }
    });
    form.delegate("#managerPhoneNumber", "focusout", (e) => {
        let t = $(e.currentTarget);
        validatePhoneNumber(t);
    });
    form.delegate("#shopTel", "keyup", (e) => {
        let t = $(e.currentTarget);
        inputRemoveError(t);
        validateInput(t, "این فیلد نمی تواند خالی باشد.");
        if ((e.which && e.which == 9) || (e.keyCode && e.keyCode == 9)) {
            validateTelNumber($("#shopTel"));
        }
    });
    form.delegate("#shopTel", "focusout", (e) => {
        validateTelNumber($("#shopTel"));
    });
    form.delegate("#managerTel", "keyup", (e) => {
        let t = $(e.currentTarget);
        inputRemoveError(t);
        validateInput(t, "این فیلد نمی تواند خالی باشد.");
        if ((e.which && e.which == 9) || (e.keyCode && e.keyCode == 9)) {
            validateTelNumber($("#managerTel"));
        }
    });
    form.delegate("#managerTel", "focusout", (e) => {
        validateTelNumber($("#managerTel"));
    });
    $("#btnSave").on("click", (e) => {
        saveForm(e);
    });
    $("#btnSaveNext").on("click", (e) => {
        saveForm(e);
    });
    //#endregion

    // #region EShopDoc
    let docList = () => {
        $.ajax({
            url: '/EShop/DocList',
            type: 'POST',
            data: { id: $("#id").val() },
            success: function (Resualt) {
                NoRecords(false);
                if (Resualt.length > 0) {
                    var source = $('#eShopDocSource').html();
                    var template = Handlebars.compile(source);
                    var list = template({ eShopDocList: Resualt });
                    $('#tbl-eShopDoc tbody').html(list);
                    $("#cent").html(Resualt.length + " مورد ");
                } else {
                    $("#count").html("");
                    NoRecords(true);
                }
            },
            error: function () {
                notifiction(1, 'عدم فراخوانی رکورد ها');
            }
        })
    },
        docForm = $("#frmEShopDoc"),
        validateImage = (target, id) => {
            let tempValid = false;
            if (id != emptyId()) {
                tempValid = true;
            }
            else {
                if (target.val() != "") {
                    let t = target.val().split('.').pop().toLowerCase(),
                        s = target[0].files[0].size;
                    s /= 1024;

                    if ((t != 'jpg' || t != 'jpeg') && s > 2048) {
                        inputAddError(target, "فرمت یا اندازه تصویر مجاز نمی باشد.");
                    }
                    else {
                        inputRemoveError(target);
                        tempValid = true;
                    }
                }
                else {
                    tempValid = validateInput(target, " لطفا تصویر را انتخاب نمایید.");
                }
            }
            return tempValid;
        },
        addEditEShopDocGet = (e) => {
            let t = $(e.currentTarget),
                id = t.attr("data-id"),
                path = t.attr("data-path"),
                title = t.attr("data-title"),
                img = $("#docImg");

            inputRemoveError($("#title"));
            inputRemoveError($("#path"));

            //add mode
            if (id == undefined) {
                $("#btns").show();
                $("#frmEShopDoc #title").val("");
                $("#docId").val(emptyId());
                img.attr("src", "/Areas/Operator/assets/images/demoUpload.jpg");
                $("#addEditAssign .modal-title").html("ثبت مدرک جدید");
            }
            //edit mode
            else {
                $("#btns").hide();
                $("#frmEShopDoc #title").val(title);
                $("#docId").val(id);
                img.attr("src", path);
                $("#addEditAssign .modal-title").html("ویرایش مدرک");
            }
        };
    $("#tbl-eShopDoc").delegate("a#addEditEShopDoc", "click", (e) => {
        addEditEShopDocGet(e);
    });
    $("#addEditEShopDoc").on("click", (e) => {
        addEditEShopDocGet(e);
    });
    docForm.delegate("#title", "keyup", (e) => {
        validateInput($(e.currentTarget), "این فیلد نمی تواند خالی باشد.");
    });
    docForm.delegate("#path", "change", (e) => {
        validateImage($("#path"), $("#docId").val());
    });
    let addEditEShopDoc = () => {
        let modal = $("#addEditDoc"),
            title = $("#frmEShopDoc #title"),
            isImageValid = validateImage($("#path"), $("#docId").val()),
            isTitleValid = validateInput(title, "این فیلد نمی تواند خالی باشد.");
        if (isTitleValid && isImageValid) {
            let formData = new FormData();
            if ($("#docId").val() == emptyId()) {
                formData.append("img", $("#path")[0].files[0]);
            }
            formData.append("id", $('#docId').val());
            formData.append("eShopId", $('#id').val());
            formData.append("title", title.val());
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
                url: '/EShop/AddEditEShopDoc',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (Resualt) {
                    if (Resualt == "عملیات با موفقیت انجام شد.") {
                        $("modal-content").unblock();
                        notifiction(0, Resualt);
                        modal.modal('hide');
                        docList();
                    }
                    else {
                        notifiction(1, Resualt);
                    }
                },
                error: function (Resualt) {
                    modal.unblock();
                    notifiction(1, Resualt);
                }
            })
        }
    }
    $("#btnAddEditDoc").on("click", () => {
        addEditEShopDoc();
    });
    $("#tbl-eShopDoc").delegate("a#delEShopDoc", "click", (e) => {
        var dis = $(e.currentTarget);
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
                        url: '/EShop/DeleteEShopDoc',
                        type: 'POST',
                        data: { id: dis.attr("data-id") },
                        success: function (Resualt) {
                            if (Resualt == "عملیات با موفقیت انجام شد.") {
                                docList();
                                swal({
                                    title: "عملیات حذف انجام شد .",
                                    text: "رکورد مد نظر شما با موفقیت حذف گردید .",
                                    confirmButtonColor: "#66BB6A",
                                    confirmButtonText: "باشه",
                                    type: "success"
                                });
                            }
                            else {
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
    })
    //#endregion
})