

 $(document).ready(function(){

		$(".support-txt").hover(function(){

			$(".contact-div").css("display", "block");
			$(".contact-div").css("-webkit-transition" , "width 2s");
			$(".contact-div").css("transition" , "width 7s");

		});

		$(".support-txt").mouseleave(function(){

			$(".contact-div").css("display", "none");
		});

		

		// On click change button color js start here


		// $('.trip-buttons').click(function(){
		// 	$('.trip-buttons').removeClass('selected-menu');
		// 	$(this).addClass('selected-menu');

		// })

		$('.trip-btn-style').click(function(){
			$('.trip-btn-style').removeClass('selected-menu');
			$(this).addClass('selected-menu');

		})

		/* On click change button color js end here*/

		$('.travel-btn-down').click(function()
		{
			$('.passenger-div').toggle();
		});


		$('.close-txt').click(function()
		{
			$('.passenger-div').hide();
		});	

		$(".round-trip-btn").click(function(){
        $(".main-search-div").show();
         $(".multi-city-div").hide();

         });

         $(".one-way-btn").click(function(){
        $(".main-search-div").show();
         $(".multi-city-div").hide();

         });	

		
		$(".multi-city-btn").click(function(){
        $(".main-search-div").hide();
        $(".multi-city-div").show();


         });
});







// Disable return index script start here


$(".one-way-btn").click(function()
 {
 	document.getElementById("return_date").disabled=true;
 });

  $(".round-trip-btn").click(function()
 {
 	document.getElementById("return_date").disabled=false;
 });

   $(".multi-city-btn").click(function()
 {
 	document.getElementById("return_date").disabled=false;
 });


// // Disable return index script end here

 




 // slide-up js start here

	 // Slider1 start here

	 $(".btn-up1").click(function()
	 {
	 	     $(this).hide();
	 	     $('.btn-down1').show();
	 	     $('.slide-up1').animate({height:"100%"},600).trigger("to.owl.carousel");
	       
	         
	    });

	 $(".btn-down1").click(function()
	 { 
	 	     $('.btn-up1').show();
	 	     $(this).hide();
	 	     $('.slide-up1').animate({height:"15%"},600);
	       
	    });
	// Slider1 end here


	// Slider2 start here

	$(".btn-up2").click(function()
	 {
	 	     $(this).hide();
	 	     $('.btn-down2').show();
	 	     $('.slide-up2').animate({height:"100%"},600);
	       
	         
	    });

	 $(".btn-down2").click(function()
	 { 
	 	     $('.btn-up2').show();
	 	     $(this).hide();
	 	     $('.slide-up2').animate({height:"15%"},600);
	       
	    });

	// Slider2 end here


	// Slider3 start here

	$(".btn-up3").click(function()
	 {
	 	     $(this).hide();
	 	     $('.btn-down3').show();
	 	     $('.slide-up3').animate({height:"100%"},600);
	       
	         
	    });

	 $(".btn-down3").click(function()
	 { 
	 	     $('.btn-up3').show();
	 	     $(this).hide();
	 	     $('.slide-up3').animate({height:"15%"},600);
	       
	    });

	// Slider3 end here

	// Slider4 start here

	$(".btn-up4").click(function()
	 {
	 	     $(this).hide();
	 	     $('.btn-down4').show();
	 	     $('.slide-up4').animate({height:"100%"},600);
	       
	         
	    });

	 $(".btn-down4").click(function()
	 { 
	 	     $('.btn-up4').show();
	 	     $(this).hide();
	 	     $('.slide-up4').animate({height:"15%"},600);
	       
	    });

	// Slider4 endh ere


	// Slider5 start here

	$(".btn-up5").click(function()
	 {
	 	     $(this).hide();
	 	     $('.btn-down5').show();
	 	     $('.slide-up5').animate({height:"100%"},600);
	       
	         
	    });

	 $(".btn-down5").click(function()
	 { 
	 	     $('.btn-up5').show();
	 	     $(this).hide();
	 	     $('.slide-up5').animate({height:"15%"},600);
	       
	    });

$(".btn-down6").click(function()
	 { 
	 	     $('.btn-up6').show();
	 	     $(this).hide();
	 	     $('.slide-up6').animate({height:"15%"},600);
	       
	    });


$(".btn-down7").click(function()
	 { 
	 	     $('.btn-up7').show();
	 	     $(this).hide();
	 	     $('.slide-up7').animate({height:"15%"},600);
	       
	    });

$(".btn-down8").click(function()
	 { 
	 	     $('.btn-up8').show();
	 	     $(this).hide();
	 	     $('.slide-up8').animate({height:"15%"},600);
	       
	    });



	// Slider5 end here

 // slide-up js end here



// Increase and Decrease the pessenger js start here

  // Adult number script start here

		function incrementValue()
		{
		    var value = parseInt(document.getElementById('qtyadult').value, 10);
		    document.getElementById("qtyadult").value = "1";
		    var value ;
		    value = isNaN(value) ? 0 : value;
		    if(value<9)
		    {
		       value++;	
		    }
		    
		    document.getElementById('qtyadult').value = value;
		}


		function decrementValue()
		{
		    var value = parseInt(document.getElementById('qtyadult').value, 10);
		    value = isNaN(value) ? 0 : value;
		    if(value>1)
		    {
		       value--;	
		    }
		    
		    document.getElementById('qtyadult').value = value;
		}   
	// Adult number script end here


	// Children number script start here

	   function cincrementValue()
		{
		    var value = parseInt(document.getElementById('qtychildren').value, 10);
		    // document.getElementById("qtychildren").value = "1";
		    var value ;
		    value = isNaN(value) ? 0 : value;
		    if(value<9)
		    {
		       value++;	
		    }
		    
		    document.getElementById('qtychildren').value = value;
		}


		function cdecrementValue()
		{
		    var value = parseInt(document.getElementById('qtychildren').value, 10);
		    value = isNaN(value) ? 0 : value;
		    if(value>0)
		    {
		       value--;	
		    }
		    
		    document.getElementById('qtychildren').value = value;
		} 

	// Children number script end here


	// Infants number script start here

	   function iincrementValue()
		{
		    var value = parseInt(document.getElementById('qtyinfants').value, 10);
		    // document.getElementById("qtychildren").value = "1";
		    var value ;
		    value = isNaN(value) ? 0 : value;
		    if(value<9)
		    {
		       value++;	
		    }
		    
		    document.getElementById('qtyinfants').value = value;
		}


		function icdecrementValue()
		{
		    var value = parseInt(document.getElementById('qtyinfants').value, 10);
		    value = isNaN(value) ? 0 : value;
		    if(value>0)
		    {
		       value--;	
		    }
		    
		    document.getElementById('qtyinfants').value = value;
		} 

	// Infants number script end here

// Increase and Decrease the pessenger js start here


