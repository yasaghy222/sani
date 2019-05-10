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

    // #region AddEditForce
    let form = $("#TblForce"),
        validatePhoneNumber = (t) => {
            let tempvalid = false;
            if (t != undefined && t.val() != "") {
                let reg = /(\+98|0|e*|E*)?9\d{9}/gm;
                if (validateInput(t, "فرمت ورودی صحیح نمی باشد.", "", reg)) {
                    $.ajax({
                        url: '/Force/IsExist',
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
                if ($("#phoneNumber").val() != "") {
                    tempvalid = validatePhoneNumber($("#phoneNumber"));
                }
                else {
                    tempvalid = validateInput($("#phoneNumber"), "این فیلد نمی تواند خالی باشد.");
                }
            }
            return tempvalid;
        },
        validateTelNumber = () => {
            let t = $("#telNumber"),
                tempvalid = false;
            if (t.val() != "") {
                let reg = /(\+98|0|e*|E*)\d{9}/gm;
                tempvalid = validateInput(t, "فرمت ورودی صحیح نمی باشد.", "", reg);
            }
            else {
                tempvalid = validateInput(t, "این فیلد نمی تواند خالی باشد.");
            }
            return tempvalid;
        },
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
        saveForm = (e) => {
            let select = $("#bCode"),
                formValid = form.valid(),
                isBCodeValid = validateInput(select, "لطفا یک مورد را انتخاب کنید.", "0"),
                isPhoneValid = validatePhoneNumber(),
                isTellValid = validateTelNumber(),
                isImageValid = validateImage($("#image"), $("#id").val()),
                isValid = false;
            if (formValid && isBCodeValid && isPhoneValid && isTellValid && isImageValid) {
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
                let formData = new FormData(),
                    targetId = $(e.currentTarget).attr("id");

                if ($("#id").val() == emptyId()) {
                    formData.append("image", $("#image")[0].files[0]);
                }
                formData.append("id", $('#id').val());
                formData.append("name", $('#name').val());
                formData.append("phoneNumber", $('#phoneNumber').val());
                formData.append("telNumber", $('#telNumber').val());
                formData.append("nationalCode", $('#nationalCode').val());
                formData.append("birthDate", $('#birthDate').val());
                formData.append("address", $('#address').val());
                formData.append("bCode", $('#bCode').val());

                $.ajax({
                    url: '/Force/AddEdit',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (Resualt) {
                        if (Resualt.resualt == "عملیات با موفقیت انجام شد.") {
                            $("#id").val(Resualt.id);
                            $(".tab-content").unblock();
                            btnSaveOnOff();
                            notifiction(0, Resualt.resualt);
                            if (targetId == "btnSaveNext") {
                                pageLoad('/Operator/Force', 'لیست تعرفه ها', 'serviceList');
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
                $.jGrowl('ابتدا مشخصات نیرو را ثبت نمائید.', { theme: 'alert-styled-left alert-arrow-left bg-warning', header: 'هشدار', position: 'bottom-right', life: 10000 });
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
    form.delegate("#phoneNumber", "keyup", (e) => {
        let t = $(e.currentTarget);
        inputRemoveError(t);
        validateInput(t, "این فیلد نمی تواند خالی باشد.");
        if ((e.which && e.which == 9) || (e.keyCode && e.keyCode == 9)) {
            validatePhoneNumber(t);
        }
    });
    form.delegate("#phoneNumber", "focusout", (e) => {
        let t = $(e.currentTarget);
        validatePhoneNumber(t);
    });
    form.delegate("#telNumber", "keyup", (e) => {
        let t = $(e.currentTarget);
        inputRemoveError(t);
        validateInput(t, "این فیلد نمی تواند خالی باشد.");
        if ((e.which && e.which == 9) || (e.keyCode && e.keyCode == 9)) {
            validateTelNumber();
        }
    });
    form.delegate("#telNumber", "focusout", (e) => {
        validateTelNumber();
    });
    form.delegate("#image", "change", (e) => {
        validateImage($("#image"), $("#id").val());
    });

    $("#btnSave").on("click", (e) => {
        saveForm(e);
    });
    $("#btnSaveNext").on("click", (e) => {
        saveForm(e);
    });
    //#endregion

    // #region ForceExp
    let expList = () => {
        $.ajax({
            url: '/Force/ExpList',
            type: 'POST',
            data: { id: $("#id").val() },
            success: function (Resualt) {
                NoRecords(false);
                if (Resualt.length > 0) {
                    var source = $('#forceExpSource').html();
                    var template = Handlebars.compile(source);
                    var list = template({ forceExpList: Resualt });
                    $('#tbl-forceExp tbody').html(list);
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
        formMode,
        expForm = $("#frmForceExp"),
        addEditForceExpGet = (e) => {
            let t = $(e.currentTarget),
                catId = t.attr("data-catId"),
                cent = t.attr("data-cent"),
                target = $('#multi-select'),
                name = "catId";

            //add mode
            if (cent == undefined && catId == undefined) {
                form.find("fieldset .form-group:nth-child(2)").removeClass("hidden");
                clearMultiSelect(target, name);
                drawSelect('/Base/ServiceCatSelect', emptyId(), target, false, name, true);
                $("#cent").val("");
                $("#addEditAssign .modal-title").html("ثبت تخصص جدید");
                formMode = false;
            }
            //edit mode
            else {
                form.find("fieldset .form-group:nth-child(2)").add("hidden");
                drawSelect('/Base/ServiceCatSelect', catId, target, false, name, true);
                $("#cent").val(cent);
                $("#addEditAssign .modal-title").html("ویرایش تخصص");
                formMode = true;
            }
        };
    $("#tbl-forceExp").delegate("a#addEditForceExp", "click", (e) => {
        addEditForceExpGet(e);
    });
    $("#addEditForceExp").on("click", (e) => {
        addEditForceExpGet(e);
    });
    expForm.delegate("#catId", "change", (e) => {
        $("#catId[checked='checked']").removeAttr("checked");
        $(e.currentTarget).attr("checked", "checked");
        validateInput($(e.currentTarget), "لطفا یک مورد را انتخاب کنید.", emptyId());
    });
    expForm.delegate("#cent", "keyup", (e) => {
        validateInput($(e.currentTarget), "این فیلد نمی تواند خالی باشد.");
    });
    let addEditForceExp = () => {
        let modal = $("#addEditExp");
        let select = $("#catId[checked='checked']");
        let isSelectValid = validateInput(select, "لطفا یک مورد را انتخاب کنید.", emptyId());;
        let inputCent = $("#cent");
        let isInputCentValid = validateInput(inputCent, "این فیلد نمی تواند خالی باشد.");
        if (isSelectValid && isInputCentValid) {
            const ViewTblForceExp = {
                forceId: $("#id").val(),
                catId: select.val(),
                cent: $("#cent").val(),
            };
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
                url: '/Force/AddEditForceExp',
                type: 'POST',
                data: {
                    model: ViewTblForceExp,
                    formMode: formMode
                },
                success: function (Resualt) {
                    if (Resualt == "عملیات با موفقیت انجام شد.") {
                        $("modal-content").unblock();
                        notifiction(0, Resualt);
                        modal.modal('hide');
                        expList();
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
    $("#btnAddEditExp").on("click", () => {
        addEditForceExp();
    });
    $("#tbl-forceExp").delegate("a#delForceExp", "click", (e) => {
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
                        url: '/Force/DeleteForceExp',
                        type: 'POST',
                        data: { catId: dis.attr("data-catId"), forceId: dis.attr("data-forceId") },
                        success: function (Resualt) {
                            if (Resualt == "عملیات با موفقیت انجام شد.") {
                                expList();
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

    // #region ForceDoc
    let docList = () => {
        $.ajax({
            url: '/Force/DocList',
            type: 'POST',
            data: { id: $("#id").val() },
            success: function (Resualt) {
                NoRecords(false);
                if (Resualt.length > 0) {
                    var source = $('#forceDocSource').html();
                    var template = Handlebars.compile(source);
                    var list = template({ forceDocList: Resualt });
                    $('#tbl-forceDoc tbody').html(list);
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
        docForm = $("#frmForceDoc"),
        addEditForceDocGet = (e) => {
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
                $("#title").val("");
                $("#docId").val(emptyId());
                img.attr("src", "/Areas/Operator/assets/images/demoUpload.jpg");
                $("#addEditAssign .modal-title").html("ثبت مدرک جدید");
            }
            //edit mode
            else {
                $("#btns").hide();
                $("#title").val(title);
                $("#docId").val(id);
                img.attr("src", path);
                $("#addEditAssign .modal-title").html("ویرایش مدرک");
            }
        };
    $("#tbl-forceDoc").delegate("a#addEditForceDoc", "click", (e) => {
        addEditForceDocGet(e);
    });
    $("#addEditForceDoc").on("click", (e) => {
        addEditForceDocGet(e);
    });
    docForm.delegate("#title", "keyup", (e) => {
        validateInput($(e.currentTarget), "این فیلد نمی تواند خالی باشد.");
    });
    docForm.delegate("#path", "change", (e) => {
        validateImage($("#path"), $("#docId").val());
    });
    let addEditForceDoc = () => {
        let modal = $("#addEditDoc"),
            title = $("#title"),
            isImageValid = validateImage($("#path"), $("#docId").val()),
            isTitleValid = validateInput(title, "این فیلد نمی تواند خالی باشد.");
        if (isTitleValid && isImageValid) {
            let formData = new FormData();
            if ($("#docId").val() == emptyId()) {
                formData.append("img", $("#path")[0].files[0]);
            }
            formData.append("id", $('#docId').val());
            formData.append("forceId", $('#id').val());
            formData.append("title", title.val());
            $(".modal-content").block({
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
                url: '/Force/AddEditForceDoc',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (Resualt) {
                    if (Resualt == "عملیات با موفقیت انجام شد.") {
                        $(".modal-content").unblock();
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
        addEditForceDoc();
    });
    $("#tbl-forceDoc").delegate("a#delForceDoc", "click", (e) => {
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
                        url: '/Force/DeleteForceDoc',
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