// #region Force
$('.maxlength').maxlength({
    placement: 'bottom-left'
});
let forceList = null;
// page table variable
let pageTableVariable = { PageSize: 10, PageIndex: 1, OrderBy: 0, OrderType: false }
let count = 0;

//select all items
function ForceList(pVariable) {
    $('#searchForce').val("");
    if (pVariable == null) {
        pVariable = pageTableVariable;
        $('#tbl-force th.sorting_desc').removeClass("sorting_desc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-force th.sorting_asc').removeClass("sorting_asc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-force th[data-sort="0"]').removeClass("sorting").attr("data-sort-type", "false").addClass("sorting_desc");
    };
    $.ajax({
        url: '/Force/ForceList',
        type: 'POST',
        data: pVariable,
        success: function (Resualt) {
            NoRecords(false);
            if (Resualt.Records.length > 0) {
                forceList = Resualt.Records;
                var source = $('#forceSource').html();
                var template = Handlebars.compile(source);
                var list = template({ forceList: Resualt.Records });
                $('#tbl-force tbody').html(list);
                count = Resualt.Total;
                CreatePagingInfo(count, Resualt.Records.length, '#tbl_force_info', pageTableVariable);
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
$("#forcePaging").delegate('a.paginate_button', "click", function () {
    var newIndex = $(this).attr("data-index");
    pageTableVariable.PageIndex = newIndex;
    ForceList(pageTableVariable);
})

//change pagesize's of items
$("#pagesize").change(function () {
    var newPageSize = $(this).val();
    pageTableVariable.PageSize = newPageSize;
    pageTableVariable.PageIndex = 1;
    ForceList(pageTableVariable);
})

//orderby items list
$('#tbl-force th').click(function (e) {
    let isSrortEnable = $(e.currentTarget).hasClass("sorting_disabled");
    if (!isSrortEnable) {
        var newOrderby = $(this).attr("data-sort");
        var newOrderType = $(this).attr("data-sort-type");
        $('#tbl-force th.sorting_desc').removeClass("sorting_desc").attr("data-sort-type", "").addClass("sorting");
        $('#tbl-force th.sorting_asc').removeClass("sorting_asc").attr("data-sort-type", "").addClass("sorting");
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
        ForceList(pageTableVariable);
    }
})

//search item
function SearchForce() {
    pageTableVariable.PageSize = 10;
    pageTableVariable.PageIndex = 1;
    let key = $('#searchforce').val();
    if (key != "") {
        $.ajax({
            url: '/Force/SearchInForce',
            type: 'POST',
            data: { fillterBy: key },
            success: function (Resualt) {
                NoRecords(false);
                var source = $('#forceSource').html();
                var template = Handlebars.compile(source);
                var list = template({ forceList: Resualt });
                $('#tbl-force tbody').html(list);
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
        ForceList();
    }
}

//disable item
$("#tbl-force").delegate("#disableForce", "click", (e) => {
    let dis = $(e.currentTarget);
    $.ajax({
        url: '/Force/DisableForce',
        type: 'POST',
        data: { id: dis.attr("data-id") },
        success: function (Resualt) {
            if (Resualt == "عملیات با موفقیت انجام شد.") {
                ForceList();
            } else {
                notifiction(1, Resualt);
            }
        },
        error: function (Resualt) {
            notifiction(1, Resualt);
        }
    });
})
//#endregion Force