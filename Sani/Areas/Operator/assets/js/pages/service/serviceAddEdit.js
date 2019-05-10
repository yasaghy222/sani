$(() => {
    // #region General
    $(".styled").uniform({
        radioClass: 'choice'
    });
    $(".touchspin").TouchSpin({
        min: 2500,
        max: 200000,
        step: 2500
    });
    $('.maxlength').maxlength({
        placement: 'bottom-left'
    });
    //#endregion

    // #region AddEditService
    let form = $("#TblService");
    let saveForm = (e) => {
        let select = $("#catId[checked='checked']");
        let formValid = form.valid();
        let inputValid = validateInput(select, "لطفا یک مورد را انتخاب کنید.", emptyId());
        let isValid = false;
        if (formValid && inputValid) {
            isValid = true;
        }
        let targetId = $(e.currentTarget).attr("id");
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
            const TblService = form.serialize();
            $.ajax({
                url: '/Service/AddEdit',
                type: 'POST',
                data: TblService,
                success: function (Resualt) {
                    if (Resualt.resualt == "عملیات با موفقیت انجام شد.") {
                        $("#id").val(Resualt.id);
                        $(".tab-content").unblock();
                        btnSaveOnOff();
                        notifiction(0, Resualt.resualt);
                        if (targetId == "btnSaveNext") {
                            pageLoad('/Operator/Service', 'لیست تعرفه ها', 'serviceList');
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
            if (id == emptyId()) {
                $.jGrowl('ابتدا مشخصات سرویس را ثبت نمائید.', { theme: 'alert-styled-left alert-arrow-left bg-warning', header: 'هشدار', position: 'bottom-right', life: 10000 });
            }
            else {
                swichTab(t);
                btnSaveOnOff();
                if (dataTab == "assign") {
                    assignServiceList();
                }
            }
        }
    });
    $(form).delegate("#catId", "change", (e) => {
        $("#catId[checked='checked']").removeAttr("checked");
        $(e.currentTarget).attr("checked", "checked");
        validateInput($(e.currentTarget), "لطفا یک مورد را انتخاب کنید.", emptyId());
    });
    $("#btnSave").on("click", (e) => {
        saveForm(e);
    });
    $("#btnSaveNext").on("click", (e) => {
        saveForm(e);
    });
    //#endregion

    // #region AddEditAssignService
    let assignServiceList = () => {
        $.ajax({
            url: '/Service/ServiceAssignList',
            type: 'POST',
            data: { id: $("#id").val() },
            success: function (Resualt) {
                NoRecords(false);
                if (Resualt.length > 0) {
                    var source = $('#serviceAssignSource').html();
                    var template = Handlebars.compile(source);
                    var list = template({ serviceAssignList: Resualt });
                    $('#tbl-branchService tbody').html(list);
                    $("#count").html(Resualt.length + " مورد ");
                } else {
                    $("#count").html("");
                    NoRecords(true);
                }
            },
            error: function () {
                notifiction(1, 'عدم فراخوانی رکورد ها');
            }
        })
    };
    let formMode;
    let addEditServiceAssignGet = (e) => {
        let t = $(e.currentTarget);
        let price = t.attr("data-price");
        let bTitle = t.attr("data-bTitle");
        let bCode = t.attr("data-bCode");
        let target = $('#bCode');
        //add mode
        if (price == undefined && bTitle == undefined) {
            clearSelect(target);
            drawSelect('/Base/BranchSelect', 0, target, true);
            target.prop("disabled", false);
            $("#price").val("");
            $("#addEditAssign .modal-title").html("ثبت اختصاص جدید");
            formMode = false;
        }
        //edit mode
        else {
            clearSelect(target);
            drawSelect('/Base/BranchSelect', bCode, target, true);
            $("#bCode").prop("disabled", true);
            $("#price").val(price);
            $("#addEditAssign .modal-title").html(" ویرایش اختصاص به " + bTitle);
            formMode = true;
        }
    };
    $("#tbl-branchService").delegate("a#addEditServiceAssign", "click", (e) => {
        addEditServiceAssignGet(e);
    });
    $("#addEditServiceAssign").on("click", (e) => {
        addEditServiceAssignGet(e);
    });
    $("#bCode").on("change", (e) => {
        validateInput($(e.currentTarget), "لطفا یک مورد را انتخاب کنید.", '0');
    });
    $("#price").on("keyup", (e) => {
        validateInput($(e.currentTarget), "این فیلد نمی تواند خالی باشد.");
    });
    let addEditAssignService = () => {
        let modal = $("#addEditAssign");
        let select = $("#bCode");
        debugger;
        let isSelectValid = validateInput(select, "لطفا یک مورد را انتخاب کنید.", '0');;
        let inputPrice = $("#price");
        let isInputPriceValid = validateInput(inputPrice, "این فیلد نمی تواند خالی باشد.");
        if (isSelectValid && isInputPriceValid) {
            const ViewTblBranchService = {
                bCode: $("#bCode").val(),
                serviceId: $("#id").val(),
                price: $("#price").val(),
                formMode: formMode
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
                url: '/Service/AddEditServiceAssign',
                type: 'POST',
                data: ViewTblBranchService,
                success: function (Resualt) {
                    if (Resualt == "عملیات با موفقیت انجام شد.") {
                        $("modal-content").unblock();
                        notifiction(0, Resualt);
                        modal.modal('hide');
                        assignServiceList();
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
    $("#btnAddEditAssign").on("click", () => {
        addEditAssignService();
    });
    $("#tbl-branchService").delegate("a#delServiceAssign", "click", (e) => {
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
                        url: '/Service/DeleteServiceAssign',
                        type: 'POST',
                        data: { bCode: dis.attr("data-id"), serviceId: $("#id").val() },
                        success: function (Resualt) {
                            if (Resualt == "عملیات با موفقیت انجام شد.") {
                                assignServiceList();
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
    });
    //#endregion
})