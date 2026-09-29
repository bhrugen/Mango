$(document).ready(function () {
    var prarms = new URLSearchParams(window.location.search);
    var status = prarms.get("status") || "all";
    var myOrder = prarms.get("myorder");
    loadDataTable(status, myOrder);
});

var statusBadges = {
    Approved: "bg-success",
    ReadyForPickup: "bg-warning text-dark",
    Completed: "bg-primary",
    Cancelled: "bg-danger",
    Refunded: "bg-danger"
};

function loadDataTable(status, myOrder) {
    $("#tblData").DataTable({
        ajax: {
            url: "/Order/GetAllOrders?status="+status+"&myOrder="+myOrder,
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: "orderHeaderId" },
            { data: "email", width: "22%" },
            { data: "name", width: "18%" },
            { data: "phone", width: "14%" },
            {
                data: "status",
                width: "10%",
                render: function (data, type) {
                    var css = statusBadges[data] || "bg-secondary";
                    return '<span class="badge '+css+'">'+data+"</span>";
                }
            },
            
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