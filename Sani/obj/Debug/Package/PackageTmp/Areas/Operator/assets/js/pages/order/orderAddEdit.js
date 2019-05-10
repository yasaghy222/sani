// #region General
$('.maxlength').maxlength({
    placement: 'bottom-left'
});
let id = $("#id").val(),
    oServiceList = [],
    oCustomerId = emptyId(),
    oAddressId = emptyId();
$(".tabbable .nav-tabs li a").on("click", (e) => {
    let t = e.currentTarget;
    let isActive = $(t).parent().hasClass("active");
    let dataTab = $(t).attr("data-tab");
    if (!isActive) {
        switch (dataTab) {
            case "service":
                swichTab(t);
                break;
            case "customer":
                if (oServiceList.length == 0) {
                    notifiction(3, 'لطفا حداقل یک خدمت را انتخاب نمایید.');
                }
                else {
                    swichTab(t);
                    OCustomerList();
                }
                break;
            case "address":
                if (oCustomerId == emptyId()) {
                    notifiction(3, 'لطفا مشتری را انتخاب نمایید.');
                }
                else {
                    swichTab(t);
                    OAddressList();
                }
                break;
            case "details":
                if (oAddressId == emptyId()) {
                    notifiction(3, 'لطفا آدرس را انتخاب نمایید.');
                }
                else {
                    swichTab(t);
                }
                break;
        }
    }
});
//#endregion

// #region Order Service
//order service
function OServiceList(bCode) {
    if (bCode != emptyId()) {
        //edit mode
        if (id != emptyId()) {
            $.ajax({
                url: '/Order/ServiceList',
                type: 'POST',
                data: { id: id, bCode: bCode },
                success: function (Resualt) {
                    var source = $('#OServiceSource').html();
                    var template = Handlebars.compile(source);
                    var list = template({ serviceList: Resualt });
                    oServiceList = Resualt;
                    $('#oService tbody').html(list);
                },
                error: function () {
                    notifiction(3, 'عدم فراخوانی رکورد ها');
                }
            });
        }
        //add mode
        else {
            $.ajax({
                url: '/Order/ServiceList',
                type: 'POST',
                data: { bCode: bCode },
                success: function (Resualt) {
                    var source = $('#OServiceSource').html();
                    var template = Handlebars.compile(source);
                    var list = template({ serviceList: Resualt });
                    $('#oService tbody').html(list);
                },
                error: function () {
                    notifiction(3, 'عدم فراخوانی رکورد ها');
                }
            });
        }
    }
    else {
        $('#oService tbody').html("");
    }
};

//find service
function FService() {
    let key = $('#FService').val();
    if (key != "") {
        //edit mode
        if (id != emptyId()) {
            $.ajax({
                url: '/Order/SearchInService',
                type: 'POST',
                data: { fillterBy: key, id: id },
                success: function (Resualt) {
                    var source = $('#OServiceSource').html();
                    var template = Handlebars.compile(source);
                    var list = template({ serviceList: Resualt });
                    oServiceList = Resualt;
                    $('#oService tbody').html(list);
                },
                error: function () {
                    notifiction(3, 'عدم فراخوانی رکورد ها');
                }
            });
        }
        //add mode
        else {
            $.ajax({
                url: '/Order/SearchInService',
                type: 'POST',
                data: { fillterBy: key },
                success: function (Resualt) {
                    var source = $('#OServiceSource').html();
                    var template = Handlebars.compile(source);
                    var list = template({ serviceList: Resualt });
                    $('#oService tbody').html(list);
                },
                error: function () {
                    notifiction(3, 'عدم فراخوانی رکورد ها');
                }
            });
        }
    }
    else {
        OServiceList();
    }
}

$("#bCode").on("change", (e) => {
    let value = $(e.currentTarget).val();
    if (value != emptyId()) {
        OServiceList($(e.currentTarget).val());
    }
    else {
        $('#oService tbody').html("");
    }
});

$("#oService").delegate("#minus", "click", (e) => {
    let t = $(e.currentTarget),
        input = t.parent().parent().find("input"),
        value = input.val();
    if (value > 0) {
        value--;
        input.val(value);
    }
});

$("#oService").delegate("#plus", "click", (e) => {
    let t = $(e.currentTarget),
        input = t.parent().parent().find("input"),
        value = input.val();
    value++;
    input.val(value);
});

$("#oService").delegate("#number", "keyup", (e) => {
    let t = $(e.currentTarget),
        value = t.val();
    if ((e.which && e.which == 69) || (e.keyCode && e.keyCode == 69) || value < 0) {
        t.val("0");
    }
});

$("#oService").delegate("#number", "change", (e) => {
    let t = $(e.currentTarget),
        value = t.val();
});

$("#oService").delegate("#number", "focusout", (e) => {
    let t = $(e.currentTarget),
        value = t.val();
    if (value == "") {
        t.val("0");
    }
});

$("#goCustomer").on("click", () => {
    let inputs = $("#oService input"),
        tempValid = false;
    oServiceList = [];
    $.each(inputs, (i, item) => {
        let value = $(item).val();
        if (value > 0) {
            tempValid = true;
            oServiceList.push({
                id: $(item).attr("data-id"),
                count: value,
                price: $(item).attr("data-price")
            });
        }
    });
    $("#assignCount").html(oServiceList.length);
    if (tempValid) {
        $(".tabbable .nav-tabs li a[data-tab='customer']").click();
    }
    else {
        notifiction(3, 'لطفا حداقل یک خدمت را انتخاب نمایید.');
    }
});
//#endregion

// #region Order Customer
//select order customer
function OCustomerList() {
    if (oCustomerId == emptyId()) {
        //edit mode
        if (id != emptyId()) {
            $.ajax({
                url: '/Order/CustomerList',
                type: 'POST',
                data: { id: id },
                success: function (Resualt) {
                    var source = $('#OCustomerSource').html();
                    var template = Handlebars.compile(source);
                    var list = template({ customerList: Resualt });
                    $('#oCustomer tbody').html(list);
                    oCustomerList = Resualt;
                    oCustomerId = Resualt[0].id;
                    $("#oCustomer input").attr("checked", "checked");
                    $("#oCustomer input").parent().addClass("checked");
                },
                error: function () {
                    notifiction(3, 'عدم فراخوانی رکورد ها');
                }
            });
        }
        //add mode
        //else {
        //    $.ajax({
        //        url: '/Order/CustomerList',
        //        type: 'POST',
        //        success: function (Resualt) {
        //            var source = $('#OCustomerSource').html(),
        //                template = Handlebars.compile(source),
        //                list = template({ customerList: Resualt });
        //            $('#oCustomer tbody').html(list);
        //        },
        //        error: function () {
        //            notifiction(3, 'عدم فراخوانی رکورد ها');
        //        }
        //    });
        //}
    }
};

//find customer
function FCustomer(key) {
    if (key == undefined) {
        key = $("#FCustomer").val();
    }
    if (key != "") {
        $.ajax({
            url: '/Order/SearchInCustomer',
            type: 'POST',
            data: { fillterBy: key },
            success: function (Resualt) {
                var source = $('#OCustomerSource').html();
                var template = Handlebars.compile(source);
                var list = template({ customerList: Resualt });
                $('#oCustomer tbody').html(list);
            },
            error: function () {
                notifiction(3, 'عدم فراخوانی رکورد ها');
            }
        });
    }
    else {
        $('#oCustomer tbody').html("");
    }
}

let validatePhoneNumber = (t) => {
    let tempvalid = false;
    if (t != undefined && t.val() != "") {
        let reg = /(\+98|0|e*|E*)?9\d{9}/gm;
        if (validateInput(t, "فرمت ورودی صحیح نمی باشد.", "", reg)) {
            $.ajax({
                url: '/User/IsExist',
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
    clearCustomerForm = () => {
        inputRemoveError($("#name"));
        inputRemoveError($("#phoneNumber"));
        inputRemoveError($("#locationId"));
        $("#name").val("");
        $("#phoneNumber").val("");
        clearSelect($("#locationId"));
        drawSelect('/Base/CitySelect', 0, $("#locationId"), true);
    };

//getAddUserForm
$("#addUser").on("click", (e) => {
    clearCustomerForm();
})

//validate form input
$("#name").on("keyup", (e) => {
    validateInput($(e.currentTarget), "این فیلد نمی تواند خالی باشد.");
});
$("#phoneNumber").on("keyup", (e) => {
    let t = $(e.currentTarget);
    inputRemoveError(t);
    validateInput(t, "این فیلد نمی تواند خالی باشد.");
    if ((e.which && e.which == 9) || (e.keyCode && e.keyCode == 9)) {
        validatePhoneNumber(t);
    }
});
$("#phoneNumber").on("focusout", (e) => {
    let t = $(e.currentTarget);
    validatePhoneNumber(t);
});
$("#locationId").on("change", (e) => {
    validateInput($(e.currentTarget), "لطفا یک مورد را انتخاب کنید.", "0");
});

//addUser
$("#btnAddUser").on("click", (e) => {
    let isValid = false,
        form = $("#frmUser"),
        modal = $("#addUserForm"),
        isNameValid = validateInput($("#name"), "این فیلد نمی تواند خالی باشد."),
        isPhoneValid = validatePhoneNumber(),
        isCityValid = validateInput($("#locationId"), " لطفا یک مورد را انتخاب کنید.", "0");

    if (isNameValid && isPhoneValid && isCityValid) {
        isValid = true;
    }

    if (isValid) {
        const TblCustomer = form.serialize();
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
            url: '/User/Add',
            type: 'POST',
            data: TblCustomer,
            success: function (Resualt) {
                if (Resualt == "عملیات با موفقیت انجام شد.") {
                    $("modal-content").unblock();
                    notifiction(0, Resualt);
                    modal.modal('hide');
                    FCustomer($("#phoneNumber").val());
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

$("#oCustomer").delegate("input", "change", (e) => {
    if (id == emptyId()) {
        let t = $(e.currentTarget),
            checked = $("#oCustomer .choice span.checked");
        checked.removeClass("checked");
        t.parent().toggleClass("checked");
        oCustomerId = t.val();
    }
});

$("#oCustomer").delegate("input", "click", (e) => {
    if (id == emptyId()) {
        let t = $(e.currentTarget),
            checked = $("#oCustomer .choice span.checked");
        checked.removeClass("checked");
        t.parent().toggleClass("checked");
        oCustomerId = t.val();
    }
});

$("#goService").on("click", () => {
    $(".tabbable .nav-tabs li a[data-tab='service']").click();
});

$("#goAddress").on("click", () => {
    if (oCustomerId != emptyId()) {
        $(".tabbable .nav-tabs li a[data-tab='address']").click();
    }
    else {
        notifiction(3, 'لطفا مشتری را انتخاب نمایید.');
    }
});
//#endregion

// #region Order Address
//select order customer
function OAddressList() {
    if (oAddressId == emptyId()) {
        //edit mode
        if (id != emptyId()) {
            $.ajax({
                url: '/Order/AddressList',
                type: 'POST',
                data: { factId: id, customerId: oCustomerId },
                success: function (Resualt) {
                    var source = $('#OAddressSource').html();
                    var template = Handlebars.compile(source);
                    var list = template({ addressList: Resualt });
                    $('#oAddress tbody').html(list);
                },
                error: function () {
                    notifiction(3, 'عدم فراخوانی رکورد ها');
                }
            });
        }
        //add mode
        else {
            $.ajax({
                url: '/Order/AddressList',
                type: 'POST',
                data: { customerId: oCustomerId },
                success: function (Resualt) {
                    if (Resualt.length > 0) {
                        var source = $('#OAddressSource').html(),
                            template = Handlebars.compile(source),
                            list = template({ addressList: Resualt });
                        $('#oAddress tbody').html(list);
                    } else {
                        notifiction(3, 'کاربر آدرس ثبت شده ای ندارد.');
                        $('#oAddress tbody').html("");
                    }
                    getLocation();
                },
                error: function () {
                    notifiction(3, 'عدم فراخوانی رکورد ها');
                }
            });
        }
    }
};

let marker = null,
    lat = 36.3729149,
    lng = 59.4783693,
    map = new L.Map('map', {
        key: 'web.HaN0rZtSg5UhQ5gakASSplCk8tCnjHI1Sv1juvcL',
        maptype: 'dreamy',
        poi: true,
        traffic: false,
        center: [lat, lng],
        zoom: 15
    }),
    getLocation = () => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(setLocation);
        } else {
            map.panTo(new l.latitude(lat, lng));
        }
    },
    setLocation = (position) => {
        lat = position.coords.latitude;
        lng = position.coords.longitude;
        map.panTo(new L.LatLng(lat, lng));
    },
    clearAddressForm = () => {
        getLocation();
        inputRemoveError($("#address"));
        inputRemoveError($("#frmAddress #locationId"));
        $("#address").val("");
        if (marker != null) {
            map.removeLayer(marker);
            marker = null;
        };
        clearSelect($("#frmAddress #locationId"));
        drawSelect('/Base/CitySelect', 0, $("#frmAddress #locationId"), true);
    },
    validateMap = () => {
        let tempValid = false;
        if (marker != null) {
            tempValid = true;
            inputRemoveError($("#map"));
        } else {
            inputAddError($("#map"), "لطفا آدرس دقیق را انتخاب نمائید.");
        }
        return tempValid;
    };

//getAddAddressForm
$("#addAddress").on("click", (e) => {
    clearAddressForm();
});

//validate form input
$("#address").on("keyup", (e) => {
    validateInput($(e.currentTarget), "این فیلد نمی تواند خالی باشد.");
});
$("#locationId").on("change", (e) => {
    validateInput($(e.currentTarget), "لطفا یک مورد را انتخاب کنید.", "0");
});

map.on('click', function (e) {
    if (marker == null) {
        marker = L.marker(e.latlng).addTo(map);
        inputRemoveError($("#map"));
    }
    else {
        map.removeLayer(marker);
        marker = null;
    }
});

//add Address
$("#btnAddAddress").on("click", (e) => {
    let isValid = false,
        form = $("#frmAddress"),
        modal = $("#addAddressForm"),
        isAddressValid = validateInput($("#address"), "این فیلد نمی تواند خالی باشد."),
        isCityValid = validateInput($("#frmAddress #locationId"), " لطفا یک مورد را انتخاب کنید.", "0"),
        isMapValid = validateMap();

    if (isAddressValid && isCityValid && isMapValid) {
        isValid = true;
    }

    if (isValid) {
        const TblAddress = {
            id: oAddressId,
            customerId: oCustomerId,
            locationId: $("#frmAddress #locationId").val(),
            address: $("#address").val(),
            lat: "" + marker._latlng.lat + "",
            lng: "" + marker._latlng.lng + ""
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
            url: '/Order/AddAddress',
            type: 'POST',
            data: TblAddress,
            success: function (Resualt) {
                if (Resualt == "عملیات با موفقیت انجام شد.") {
                    $("modal-content").unblock();
                    notifiction(0, Resualt);
                    modal.modal('hide');
                    OAddressList();
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

//delete address
$("#oAddress").delegate("a#delAddress", "click", (e) => {
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
                    url: '/Order/DelAddress',
                    type: 'POST',
                    data: { id: dis.attr("data-id") },
                    success: function (Resualt) {
                        if (Resualt == "عملیات با موفقیت انجام شد.") {
                            OAddressList();
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

$("#oAddress").delegate("input", "change", (e) => {
    if (id == emptyId()) {
        let t = $(e.currentTarget),
            checked = $("#oAddress .choice span.checked");
        checked.removeClass("checked");
        t.parent().toggleClass("checked");
        oAddressId = t.val();
    }
});

$("#oAddress").delegate("input", "click", (e) => {
    if (id == emptyId()) {
        let t = $(e.currentTarget),
            checked = $("#oAddress .choice span.checked");
        checked.removeClass("checked");
        t.parent().toggleClass("checked");
        oAddressId = t.val();
    }
});

$("#backCustomer").on("click", () => {
    $(".tabbable .nav-tabs li a[data-tab='customer']").click();
});

$("#goDetails").on("click", () => {
    let input = $("#oAddress").find("input[checked='checked']");
    if (input.length > 0) {
        oAddressId = input.val();
    }
    if (oAddressId != emptyId()) {
        $(".tabbable .nav-tabs li a[data-tab='details']").click();
    }
    else {
        notifiction(3, 'لطفا آدرس را انتخاب نمایید.');
    }
});
//#endregion//#region Details$("#oDetails").delegate("#reserveStatus", "change", (e) => {    $("#pickDate").toggleClass("hidden");
});$("#oDetails").delegate("#forceAssign", "change", (e) => {    if ($("#pickForce").hasClass("hidden")) {
        clearSelect($("#forceId"));
        drawSelect('/Base/AForceSelect', emptyId(), $("#forceId"), true);
    };    $("#pickForce").toggleClass("hidden");
});$("#backAddress").on("click", () => {
    $(".tabbable .nav-tabs li a[data-tab='address']").click();
});let FindingForce = () => {    $.ajax({
        url: '/Order/FindingForce',
        type: 'POST',
        data: { factorId: id, forceId: $("forceId").val() },
        success: function (Resualt) {
            switch (Resualt) {
                case "found and send":
                    notifiction(0, "لیستی از نیروها ‍‍‍‍‍‍‍‍پیدا شده و پیام برای آنها ارسال گردید.");
                    break;
                case "message is send":
                    notifiction(0, "پیام برای نیروی انتخاب شده ارسال گردید.");
                    break;
                case "failed":
                    notification(1, "سفارش یافت نشد!");
                    break;
                case "not found":
                    notifiction(1, "نیروایی یافت نشد و یا پیام ارسال نشد.");
                case "message is not send":
                    notification(1, "پیام به نیروی انتخاب شده ارسال نگردید.");
            }
        },
        error: function (Resualt) {
            notifiction(1, Resualt);
        }
    });
}$("#finish").on("click", () => {
    swal({
        title: "آیا از ثبت این سفارش اطمینان دارید ؟",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#EF5350",
        confirmButtonText: "بله ، ثبت شود",
        cancelButtonText: "بررسی مجدد",
        closeOnConfirm: true,
        closeOnCancel: true
    },
        function (isConfirm) {
            let FactorVariable = {
                id: id,
                customerId: oCustomerId,
                isPrint: $("#isPrint").val(),
                description: $("#description").val(),
                registerDate: $("#registerDate").val(),
                bCode: $("#bCode").val(),
                addressId: oAddressId,
                services: oServiceList
            };
            if (isConfirm) {
                $.ajax({
                    url: '/Order/AddEditOrder',
                    type: 'POST',
                    data: FactorVariable,
                    success: function (Resualt) {
                        if (Resualt != emptyId()) {
                            let text = "";
                            text = (FactorVariable.forceId == emptyId()) ? "در حال یافتن نیرو." : "درحال ارسال پیام به نیرو";
                            swal({
                                title: "سفارش با موفقیت ثبت گردید .",
                                text: text,
                                confirmButtonColor: "#66BB6A",
                                confirmButtonText: "باشه",
                                type: "success"
                            });
                            pageLoad('/Operator/Order', 'لیست سفارشات', 'orderList');
                            
                            FindingForce();
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
        });
});//#endregion