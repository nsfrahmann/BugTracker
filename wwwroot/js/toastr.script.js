(function ($) {
    "use strict";
       toastr.options = {
        "debug": false,
        "newestOnTop": true,
        "positionClass": "toast-bottom-right",
        "closeButton": true,
        "progressBar": false
    };
    $('.homerDemo1').on('load', function (event) {
        toastr.info('Info - This is a custom info notification');
    });
    $('.homerDemo2').on('click', function (event) {
        toastr.success('Success - This is a success notification');
    });
    $('.homerDemo3').on('click', function (event) {
        toastr.warning('Warning - This is a test notification');
    });
    $('.homerDemo4').on('click', function (event) {
        toastr.error('Error - This is a error notification');
    });

})(jQuery);
