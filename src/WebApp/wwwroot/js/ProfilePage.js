
function ban_user(user_id) {
    $.ajax({
        type: "Put",
        url: "/Account/Ban",
        data: { id: user_id },
        async: true,
        dataType: "text",

        success: function (msg) {
            location.reload(true);
        }
    });
}