
var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            console.log(id);
            $.ajax({
                url: '/Admin/Product/Status',
                data: { id: id },
                type: 'PUT',
                dataType: 'json',
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Disabled');
                        // document.getElementById("btn-status").style.background = 'red';
                    }
                    else {
                        btn.text('Active');
                        // document.getElementById("btn-status").style.background = 'greend';
                    }
                },
                error: function (ex) {
                    alert(ex);
                }
            });
        });
    }

}
user.init();