$(document).ready(function () {
    var status = new URLSearchParams(window.location.search).get("status") || "all";
    loadDataTable(status);
});

var statusBadges = {
    Approved: "bg-success",
    ReadyForPickup: "bg-warning text-dark",
    Completed: "bg-primary",
    Cancelled: "bg-danger",
    Refunded: "bg-danger"
};

function loadDataTable(status) {
    $("#tblData").DataTable({
        order: [[0, "desc"]],
        ajax: { url: "/Order/GetAll?status=" + encodeURIComponent(status) },
        columns: [
            { data: "orderHeaderId", width: "8%" },
            { data: "email", width: "22%" },
            { data: "name", width: "18%" },
            { data: "phone", width: "14%" },
            {
                data: "status",
                width: "14%",
                render: function (data, type) {
                    // Only decorate for display; sorting/filtering use the raw status value.
                    if (type !== "display") return data;
                    var css = statusBadges[data] || "bg-secondary";
                    var label = data === "ReadyForPickup" ? "Ready for Pickup" : data;
                    return '<span class="badge ' + css + '">' + label + "</span>";
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
                orderable: false,
                searchable: false,
                render: function (data) {
                    return '<a href="/Order/OrderDetail?orderId=' + data + '" class="btn btn-sm btn-gold">' +
                        '<i class="bi bi-eye me-1"></i>Details</a>';
                }
            }
        ]
    });
}
