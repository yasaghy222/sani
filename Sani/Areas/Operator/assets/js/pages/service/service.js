// #region Service

// page table variable
let pageTableVariable = { PageSize: 10, PageIndex: 1, OrderBy: 0, OrderType: false }
let count = 0;

//select all items
function ServiceList(pVariable) {
    $('#searchservice').val("");
    if (pVariable == null) {
        pVariable = pageTableVariable;
        $('#tbl-Service th.sorting_desc').removeClass("sorting_desc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-Service th.sorting_asc').removeClass("sorting_asc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-Service th[data-sort="0"]').removeClass("sorting").attr("data-sort-type", "false").addClass("sorting_desc");
    };
    $.ajax({
        url: '/Service/ServiceList',
        type: 'POST',
        data: pVariable,
        success: function (Resualt) {
            NoRecords(false);
            if (Resualt.Records.length > 0) {
                var source = $('#serviceSource').html();
                var template = Handlebars.compile(source);
                var list = template({ serviceList: Resualt.Records });
                $('#tbl-Service tbody').html(list);
                count = Resualt.Total;
                CreatePagingInfo(count, Resualt.Records.length, '#tbl_Service_info', pageTableVariable);
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
$("#sercivePaging").delegate('a.paginate_button', "click", function () {
    var newIndex = $(this).attr("data-index");
    pageTableVariable.PageIndex = newIndex;
    ServiceList(pageTableVariable);
})

//change pagesize's of items
$("#pagesize").change(function () {
    var newPageSize = $(this).val();
    pageTableVariable.PageSize = newPageSize;
    pageTableVariable.PageIndex = 1;
    ServiceList(pageTableVariable);
})

//orderby items list
$('#tbl-Service th').click(function (e) {
    let isSrortEnable = $(e.currentTarget).hasClass("sorting_disabled");
    if (!isSrortEnable) {
        var newOrderby = $(this).attr("data-sort");
        var newOrderType = $(this).attr("data-sort-type");
        $('#tbl-Service th.sorting_desc').removeClass("sorting_desc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-Service th.sorting_asc').removeClass("sorting_asc").attr("data-sort-type", "").addClass("sorting");
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
        ServiceList(pageTableVariable);
    }
})

//search item
function Searchservice() {
    pageTableVariable.PageSize = 10;
    pageTableVariable.PageIndex = 1;
    let key = $('#searchservice').val();
    if (key != "") {
        $.ajax({
            url: '/Service/SearchInServices',
            type: 'POST',
            data: { fillterBy: key },
            success: function (Resualt) {
                NoRecords(false);
                var source = $('#serviceSource').html();
                var template = Handlebars.compile(source);
                var list = template({ serviceList: Resualt });
                $('#tbl-Service tbody').html(list);
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
        ServiceList();
    }
}

//delete item
$("#tbl-Service").delegate("a#delService", "click", function () {
    var dis = $(this);
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
                    url: '/Service/DeleteService',
                    type: 'POST',
                    data: { id: dis.attr("data-id") },
                    success: function (Resualt) {
                        if (Resualt == "عملیات با موفقیت انجام شد.") {
                            ServiceList();
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
})

//#endregion Service