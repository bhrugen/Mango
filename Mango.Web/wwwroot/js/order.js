$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    $("#tblData").DataTable({
        ajax: {
            url: "/Order/GetAllOrders",
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: "orderHeaderId" },
            { data: "email", width: "22%" },
            { data: "name", width: "18%" },
            { data: "phone", width: "14%" },
            { data: "status", width: "14%" },
            {
                data: "orderTotal",
                width: "10%",
                render: function (data, type) {
                    return type === "display" ? "$" + Number(data).toFixed(2) : data;
                }
            },
            {
                data: "orderHeaderId",
                width: "10%",
                render: function (data, type) {
                    return '<a href="/Order/OrderDetail?orderId='+data+'" class="btn btn-sm btn-gold">' +
                        '<i class="bi bi-eye me-1"></i>Details</a>';
                }
            },
        ]
    })
}