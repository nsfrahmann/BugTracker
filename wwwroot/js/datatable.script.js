(function ($) {
    "use strict";
var editor;
 $('#example').DataTable({
     dom:
         
         "<'row align-items-center'<'col-sm-4'B><'col pt-2 before-me'l><'col-auto pt-2'f>>" +
         "<'row mt-3'<'col-sm-12't>>" +
         "<'row'<'col-sm-6'i><'col-sm-6 mt-2'p>>",
                buttons: [
                    'copy', 'csv', 'excel', 'pdf', 'print'
                ],
     responsive: true,
     lengthChange: [10, 25, 50],
     lengthMenu: [10, 25, 50],
     language: {
         lengthMenu: "Tickets per Page _MENU_"
     }
 });
    $('.test').removeClass('d-none')
    $(".before-me").before($(".test"));

})(jQuery);
