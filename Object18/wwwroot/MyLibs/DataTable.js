var sortOrder = [{ keyName: "", keySort: "" }];
var filterIndex = 0;
var selectedIds = [];
var activePage = 1;

var filterOperators = [
    {
        name: "شامل میشود",
        value: "=*"
    },
    {
        name: "شامل نمیشود",
        value: "!*"
    },
    {
        name: "برابر با",
        value: "="
    },
    {
        name: "نابرابر با",
        value: "!*"
    },
    {
        name: "بزرگتر از",
        value: ">"
    },
    {
        name: "کوچکتر از",
        value: "<"
    }
];

function GetFilterData() {
    var formResult = $('form').serializeArray();
    var keyNames = [];
    var keyOperators = [];

    keyNames = formResult.filter(form => form.name === "keyName")
        .map(form => form.value);
    keyOperators = formResult.filter(form => form.name === "keyOperator")
        .map(form => form.value);

    var finalData = [];

    for (var j = 0; j < keyNames.length; j++) {
        finalData.push(
            {
                keyName: keyNames[j],
                keyOperator: keyOperators[j],
                keyType: filters.filter(item => item.name === keyNames[j])[0].type,
                keyValue: Array.isArray($("#" + "value" + j).val())
                    ? $("#" + "value" + j).val()
                    : [$("#" + "value" + j).val()]
            });
    }
    return finalData;
}

function generateFilter() {
    var result = '';
    var keyNameOptions = '';
    var keyOperatorOptions = '';

    for (var i = 0; i < filters.length; i++) {
        keyNameOptions += '<option ' +
            'value="' +
            filters[i].name +
            '">' +
            filters[i].label +
            '</option>';

        if (i <  filterOperators.length) {
            keyOperatorOptions += '<option ' +
                'value="' +
                filterOperators[i].value +
                '">' +
                filterOperators[i].name +
                '</option>';
        }
    }
    result += '<div class="col-sm-2 mb-2">' +
        '<select id="' +
        filterIndex +
        '" name="keyName" class="form-control" onchange="onKeyNameChange(this)">' +
        keyNameOptions +
        '</select>' +
        '</div>';

    result += '<div class="col-sm-2 mb-2">' +
        '<select id="operator' +
        filterIndex +
        '" name="keyOperator" class="form-control ">' +
        keyOperatorOptions +
        '</select>' +
        '</div>';

    result += '<div class="col-sm-2 mb-2">' +
        '<input id="value' +
        filterIndex +
        '" name="keyValue" type="text" class="form-control">' +
        '</div>';
    filterIndex++;
    $(result).appendTo('#advancedSearch');
}

function onKeyNameChange(input) {
    var selectedKeyName = input.value;

    var selectedFilterType = filters.find(x => x.name === selectedKeyName).type;
    var operatorId = "operator" + input.id;
    var $el = $("#" + operatorId);
    $el.empty();

    var $inputValue = $("#" + "value" + input.id);

    const textTypeOperators = [
        {
            name: "شامل میشود",
            value: "=*"
        },
        {
            name: "شامل نمیشود",
            value: "!*"
        },
        {
            name: "برابر با",
            value: "="
        },
        {
            name: "نابرابر با",
            value: "!*"
        }
    ];

    const numberTypeOperators = [
        {
            name: "برابر با",
            value: "="
        },
        {
            name: "نابرابر با",
            value: "!*"
        },
        {
            name: "بزرگتر از",
            value: ">"
        },
        {
            name: "کوچکتر از",
            value: "<"
        }
    ];

    const listTypeOperators = [
        {
            name: "شامل میشود",
            value: "=*"
        },
        {
            name: "شامل نمیشود",
            value: "!*"
        }
    ];

    if (selectedFilterType === "text") {
        $.each(textTypeOperators,
            function (key, value) {
                $el.append($("<option></option>")
                    .attr("value", value.value).text(textTypeOperators[key].name));
            });
        $inputValue.attr('type', 'text');
    } else if (selectedFilterType === "number") {
        $.each(numberTypeOperators,
            function (key, value) {
                $el.append($("<option></option>")
                    .attr("value", value.value).text(numberTypeOperators[key].name));
            });
        $inputValue.attr('type', 'number');
    } else if (selectedFilterType === "date") {
        $.each(numberTypeOperators,
            function (key, value) {
                $el.append($("<option></option>")
                    .attr("value", value.value).text(numberTypeOperators[key].name));
            });

        $inputValue.attr('type', 'text');
        $inputValue.persianDatepicker({
            cellWidth: 50,
            cellHeight: 32,
            fontSize: 18,
            formatDate: "YYYY/0M/0D"
        });
    }
    else if (selectedFilterType === "list") {
        $.each(listTypeOperators,
            function (key, value) {
                $el.append($("<option></option>")
                    .attr("value", value.value).text(listTypeOperators[key].name));
            });

        var select = '';
        select =
            '<select id="value' + input.id + '" class="js-example-basic-single col-sm-12" name="keyValue" multiple="multiple">'

            + '</select>';

        $inputValue.replaceWith(select);

        var selectedFilterDataSourceUrl = filters.find(x => x.name === selectedKeyName).dataSourceUrl;

        $('.js-example-basic-single').select2({
            ajax: {
                //headers: {
                //    RequestVerificationToken: $(".AntiForge" + " input").val()
                //},
                type: "Get",
                dataType: "json",
                url: selectedFilterDataSourceUrl,
                contentType: "application/json",
                processResults: function (data) {
                    return {
                        results: data
                    };
                }
            }
        });
    };
}

function onPageClick(input) {
    var pageId;

    if (input === "next") {
        pageId = activePage + 1;
    } else if (input === "previous") {
        pageId = activePage - 1;
    } else {
        pageId = input;
    }

    var data = {
        filters: GetFilterData(),
        sortOrder: sortOrder,
        takeEntity: $('select[name="takeEntity"]').val(),
        pageId: pageId
    };
    fetchDataFromServer(data);
}

function refresh() {
    $('form').trigger('reset');
    $('select[name="sortOrder"]').prop('selectedIndex', 0);
    $('select[name="takeEntity"]').prop('selectedIndex', 1);

    var data = {
        filters: GetFilterData(),
        sortOrder: sortOrder,
        takeEntity: $('select[name="takeEntity"]').val()
    };
    fetchDataFromServer(data);
}

function search() {
    var data = {
        filters: GetFilterData(),
        sortOrder: sortOrder,
        takeEntity: $('select[name="takeEntity"]').val()
    };
    fetchDataFromServer(data);
}

function fetchDataFromServer(data) {
    $.ajax({
        headers: {
            RequestVerificationToken: $(".AntiForge" + " input").val()
        },
        type: "POST",
        dataType: "json",
        url: AjaxUrl,
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (result) {
            gridOptions.api.setRowData(result.records);
            activePage = result.activePage;

            if (result.activePage === 1) {
                $('#previous').addClass('disabled');
            } else {
                $('#previous').removeClass('disabled');
            }

            if (result.activePage === result.endPage) {
                $('#next').addClass('disabled');
            } else {
                $('#next').removeClass('disabled');
            }

            $('.pages').remove();
            var pagination = '';
            for (var i = result.startPage; i <= result.endPage; i++) {
                if (i === result.activePage) {
                    pagination += '<li class="page-item pages active"><button class="page-link">' + i + '</button></li>';
                } else {
                    pagination += '<li class="page-item pages"><button class="page-link" onclick="onPageClick(' + i + ')" >' + i + '</button></li>';
                }
            }
            $('#previous').after(pagination);
        },
        error: function (e) {
            console.log(e.responseText);
        }
    });
}