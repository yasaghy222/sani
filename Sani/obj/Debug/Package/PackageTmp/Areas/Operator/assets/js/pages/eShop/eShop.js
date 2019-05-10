// #region EShop
$('.maxlength').maxlength({
    placement: 'bottom-left'
});
let eShopList = null;
// page table variable
let pageTableVariable = { PageSize: 10, PageIndex: 1, OrderBy: 0, OrderType: false }
let count = 0;

//select all items
function EShopList(pVariable) {
    $('#searchEShop').val("");
    if (pVariable == null) {
        pVariable = pageTableVariable;
        $('#tbl-eShop th.sorting_desc').removeClass("sorting_desc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-eShop th.sorting_asc').removeClass("sorting_asc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-eShop th[data-sort="0"]').removeClass("sorting").attr("data-sort-type", "false").addClass("sorting_desc");
    };
    $.ajax({
        url: '/EShop/EShopList',
        type: 'POST',
        data: pVariable,
        success: function (Resualt) {
            NoRecords(false);
            if (Resualt.Records.length > 0) {
                eShopList = Resualt.Records;
                var source = $('#eShopSource').html();
                var template = Handlebars.compile(source);
                var list = template({ eShopList: Resualt.Records });
                $('#tbl-eShop tbody').html(list);
                count = Resualt.Total;
                CreatePagingInfo(count, Resualt.Records.length, '#tbl_eShop_info', pageTableVariable);
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
$("#eShopPaging").delegate('a.paginate_button', "click", function () {
    var newIndex = $(this).attr("data-index");
    pageTableVariable.PageIndex = newIndex;
    EShopList(pageTableVariable);
})

//change pagesize's of items
$("#pagesize").change(function () {
    var newPageSize = $(this).val();
    pageTableVariable.PageSize = newPageSize;
    pageTableVariable.PageIndex = 1;
    EShopList(pageTableVariable);
})

//orderby items list
$('#tbl-eShop th').click(function (e) {
    let isSrortEnable = $(e.currentTarget).hasClass("sorting_disabled");
    if (!isSrortEnable) {
        var newOrderby = $(this).attr("data-sort");
        var newOrderType = $(this).attr("data-sort-type");
        $('#tbl-eShop th.sorting_desc').removeClass("sorting_desc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-eShop th.sorting_asc').removeClass("sorting_asc").attr("data-sort-type", "").addClass("sorting");
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
        EShopList(pageTableVariable);
    }
})

//search item
function SearchEShop() {
    pageTableVariable.PageSize = 10;
    pageTableVariable.PageIndex = 1;
    let key = $('#searcheShop').val();
    if (key != "") {
        $.ajax({
            url: '/EShop/SearchInEShop',
            type: 'POST',
            data: { fillterBy: key },
            success: function (Resualt) {
                NoRecords(false);
                var source = $('#eShopSource').html();
                var template = Handlebars.compile(source);
                var list = template({ eShopList: Resualt });
                $('#tbl-eShop tbody').html(list);
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
        EShopList();
    }
}

//disable item
$("#tbl-eShop").delegate("#disableEShop", "click", (e) => {
    let dis = $(e.currentTarget);
    $.ajax({
        url: '/EShop/DisableEShop',
        type: 'POST',
        data: { id: dis.attr("data-id") },
        success: function (Resualt) {
            if (Resualt == "عملیات با موفقیت انجام شد.") {
                EShopList();
            } else {
                notifiction(1, Resualt);
            }
        },
        error: function (Resualt) {
            notifiction(1, Resualt);
        }
    });
})
//#endregion EShop