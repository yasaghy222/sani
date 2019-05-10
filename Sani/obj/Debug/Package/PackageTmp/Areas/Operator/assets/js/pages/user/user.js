// #region User
$('.maxlength').maxlength({
    placement: 'bottom-left'
});
let userList = null;
// page table variable
let pageTableVariable = { PageSize: 10, PageIndex: 1, OrderBy: 0, OrderType: false }
let count = 0;

//select all items
function UserList(pVariable) {
    $('#searchUser').val("");
    if (pVariable == null) {
        pVariable = pageTableVariable;
        $('#tbl-user th.sorting_desc').removeClass("sorting_desc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-user th.sorting_asc').removeClass("sorting_asc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-user th[data-sort="0"]').removeClass("sorting").attr("data-sort-type", "false").addClass("sorting_desc");
    };
    $.ajax({
        url: '/User/UsersList',
        type: 'POST',
        data: pVariable,
        success: function (Resualt) {
            NoRecords(false);
            if (Resualt.Records.length > 0) {
                userList = Resualt.Records;
                var source = $('#userSource').html();
                var template = Handlebars.compile(source);
                var list = template({ userList: Resualt.Records });
                $('#tbl-user tbody').html(list);
                count = Resualt.Total;
                CreatePagingInfo(count, Resualt.Records.length, '#tbl_User_info', pageTableVariable);
            } else {
                NoRecords(true);
            }
        },
        error: function () {
            notifiction(3, 'عدم فراخوانی رکورد ها');
        }
    })
}

//go to next page's of items
$("#userPaging").delegate('a.paginate_button', "click", function () {
    var newIndex = $(this).attr("data-index");
    pageTableVariable.PageIndex = newIndex;
    UserList(pageTableVariable);
})

//change pagesize's of items
$("#pagesize").change(function () {
    var newPageSize = $(this).val();
    pageTableVariable.PageSize = newPageSize;
    pageTableVariable.PageIndex = 1;
    UserList(pageTableVariable);
})

//orderby items list
$('#tbl-user th').click(function (e) {
    let isSrortEnable = $(e.currentTarget).hasClass("sorting_disabled");
    if (!isSrortEnable) {
        var newOrderby = $(this).attr("data-sort");
        var newOrderType = $(this).attr("data-sort-type");
        $('#tbl-user th.sorting_desc').removeClass("sorting_desc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-user th.sorting_asc').removeClass("sorting_asc").attr("data-sort-type", "").addClass("sorting");
        if (newOrderType == "true") {
            pageTableVariable.OrderType = false;
            $(this).removeClass("sorting sorting_asc").attr("data-sort-type", "false").addClass("sorting_desc");
        }
        else if (newOrderType == "false") {
            pageTableVariable.OrderType = true;
            $(this).removeClass("sorting sorting_desc").attr("data-sort-type", "true").addClass("sorting_asc");
        }
        else {
            pageTableVariable.OrderType = false;
            $(this).removeClass("sorting").attr("data-sort-type", "false").addClass("sorting_desc");
        }
        pageTableVariable.OrderBy = newOrderby;
        UserList(pageTableVariable);
    }
})

//search item
function SearchUser() {
    pageTableVariable.PageSize = 10;
    pageTableVariable.PageIndex = 1;
    let key = $('#searchuser').val();
    if (key != "") {
        $.ajax({
            url: '/User/SearchInUsers',
            type: 'POST',
            data: { fillterBy: key },
            success: function (Resualt) {
                NoRecords(false);
                var source = $('#userSource').html();
                var template = Handlebars.compile(source);
                var list = template({ userList: Resualt });
                $('#tbl-user tbody').html(list);
                if (Resualt.length > 0) {
                    CreatePagingInfo(Resualt.length);
                } else {
                    NoRecords(true);
                }
            },
            error: function () {
                notifiction(3, 'عدم فراخوانی رکورد ها');
            }
        });
    }
    else {
        UserList();
    }
}

//disable item
$("#tbl-user").delegate("a#disableUser", "click", function (e) {
    let dis = $(e.currentTarget);
    $.ajax({
        url: '/User/DisableUser',
        type: 'POST',
        data: { id: dis.attr("data-id") },
        success: function (Resualt) {
            if (Resualt == "عملیات با موفقیت انجام شد.") {
                UserList();
            } else {
                notifiction(1, Resualt);
            }
        },
        error: function (Resualt) {
            notifiction(1, Resualt);
        }
    });
})


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
    clearForm = () => {
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
    clearForm();
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
                    UserList();
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
//#endregion User