
var user={
    init: function() {
        user.registerEvents();
        console.log("hello");

},
    registerEvents:function() {
        $('.btn-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            console.log(id);
            $.ajax({
                url: '/Admin/Category/Status',
                data: { id: id },
                type: 'PUT',
                dataType: 'json',
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Disabled');
                        btn.toggleClass('ds-setting btn-status');
                        window.location.reload();
                       // document.getElementById("btn-status").style.background = 'red';
                    }
                    else {
                        btn.text('Active');
                        btn.toggleClass('pd-setting btn-status');
                        window.location.reload();

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