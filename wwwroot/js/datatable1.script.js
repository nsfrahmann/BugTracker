(function ($) {
    "use strict";
    var editor;
    $('#example1').DataTable({
        dom:

            "<'row align-items-center'<'col'B><'col-auto pt-3'f>>" +
            "<'row mt-3'<'col-sm-12't>>" +
            "<'row'<'col-sm-6'i><'col-sm-6 mt-2'p>>",
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        responsive: true,
        lengthChange: [10, 25, 50],
        lengthMenu: [10, 25, 50],
        language: {
            lengthMenu: "Projects per Page _MENU_"
        }
    });

    $(".before-me").before($(".test"));

})(jQuery);

//< 'col-sm-3 pt-2 before-me'l >