(function($) {
$('.owl-carousel').owlCarousel({
    center: true,
    items:5,
    loop:true,
    margin:0,
	
	 nav:true,
    navText: [
      "<span class='fa fa-angle-left'></span>","<span class='fa fa-angle-right '></span>"],
    responsive:{
        600:{
            items:4
        }
    }
});
$('#AgreeGeneralConditions').click(

             function () {
                 if ($(this).is(':checked')) {
                     $('#tc_msg').hide();
                     //$('#tc_msg').html('');
                 }
                 else {
                     $('#tc_msg').show();
                     //$('#tc_msg').html('Please Agree Terms and Conditions');                     
                 }

             }
           );
$('#PermissionToCollectFutureInvoices').click(

         function () {
             if ($(this).is(':checked')) {
                 $('#permission_msg').hide();
                 //$('#tc_msg').html('');
             }
             else {
                 $('#permission_msg').show();
                 //$('#tc_msg').html('Please Agree Terms and Conditions');                     
             }

         }

       );


$('#example1').datepicker({
    format: "dd/mm/yyyy",
    autoclose: true
});
$('#example1').on('change', function () {
    $('.datepicker').hide();
});
})(jQuery);

