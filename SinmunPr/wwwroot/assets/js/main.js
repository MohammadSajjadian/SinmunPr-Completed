(function($){
	"use strict";
	jQuery(document).on('ready', function () {

		// Mean Menu
		jQuery('.mean-menu').meanmenu({
			meanScreenWidth: "991"
        });

        // Header Sticky
		$(window).on('scroll',function() {
            if ($(this).scrollTop() > 120){  
                $('.sinmun-nav').addClass("is-sticky");
            }
            else{
                $('.sinmun-nav').removeClass("is-sticky");
            }
        });
        
        // Popup Video
		$('.popup-youtube').magnificPopup({
			disableOn: 320,
			type: 'iframe',
			mainClass: 'mfp-fade',
			removalDelay: 160,
			preloader: false,
			fixedContentPos: false
        });
        
        // Breaking News Slides
		$('.breaking-news-slides').owlCarousel({
			loop: true,
            nav: true,
            rtl: true,
			dots: false,
			autoplayHoverPause: true,
            autoplay: true,
            animateOut: 'slideOutDown',
            animateIn: 'flipInX',
            items: 1,
            navText: [
                "<i class='icofont-rounded-right'></i>",
                "<i class='icofont-rounded-left'></i>"
            ],
        });

        // Header Search
        $(".nav-search-button").on('click',  function() {
			$('.nav-search form').toggleClass('active');
		});
		$(".nav-search-close-button").on('click',  function() {
			$('.nav-search form').removeClass('active');
        });
        
        // Popular News Slides
		$('.popular-news-slides').owlCarousel({
			loop: true,
			nav: true,
            rtl: true,
			dots: false,
			autoplayHoverPause: true,
			autoplay: true,
            navText: [
                "<i class='icofont-rounded-right'></i>",
                "<i class='icofont-rounded-left'></i>"
            ],
			responsive: {
                0: {
                    items:1,
                },
                768: {
                    items:2,
                },
                1200: {
                    items:3,
				}
            }
        });
        
        // Health Lifestyle Slides
		$('.health-lifestyle-news-slides').owlCarousel({
			loop: true,
			nav: true,
            rtl: true,
			dots: false,
			autoplayHoverPause: true,
            autoplay: true,
            items: 1,
            navText: [
                "<i class='icofont-rounded-right'></i>",
                "<i class='icofont-rounded-left'></i>"
            ],
        });

        // Politics Slides
		$('.politics-news-slides').owlCarousel({
			loop: true,
			nav: true,
            rtl: true,
			dots: false,
			autoplayHoverPause: true,
            autoplay: true,
            items: 1,
            navText: [
                "<i class='icofont-rounded-right'></i>",
                "<i class='icofont-rounded-left'></i>"
            ],
        });

        // Gallery News Inner Slides
		$('.gallery-news-inner-slides').owlCarousel({
			loop: true,
			nav: true,
            rtl: true,
			dots: false,
			autoplayHoverPause: true,
            autoplay: true,
            items: 1,
            navText: [
                "<i class='icofont-rounded-right'></i>",
                "<i class='icofont-rounded-left'></i>"
            ],
        });

        // Blog Home Slides
		$('.blog-home-slides').owlCarousel({
			loop:true,
			nav:true,
            rtl: true,
			dots:false,
			smartSpeed: 2000,
			autoplayHoverPause:true,
			autoplay: true,
			navText: [
                "<i class='icofont-arrow-right'></i>",
                "<i class='icofont-arrow-left'></i>"
			],
			responsive:{
                0:{
                    items:1,
				},
				576:{
					items:2,
				},
                768:{
                    items:2,
                },
                1200:{
                    items:3,
				}
            }
        });
        
        // Count Time 
        function makeTimer() {
            var endTime = new Date("September 30, 2020 17:00:00 PDT");			
            var endTime = (Date.parse(endTime)) / 1000;
            var now = new Date();
            var now = (Date.parse(now) / 1000);
            var timeLeft = endTime - now;
            var days = Math.floor(timeLeft / 86400); 
            var hours = Math.floor((timeLeft - (days * 86400)) / 3600);
            var minutes = Math.floor((timeLeft - (days * 86400) - (hours * 3600 )) / 60);
            var seconds = Math.floor((timeLeft - (days * 86400) - (hours * 3600) - (minutes * 60)));
            if (hours < "10") { hours = "0" + hours; }
            if (minutes < "10") { minutes = "0" + minutes; }
            if (seconds < "10") { seconds = "0" + seconds; }
            $("#days").html(days + "<span>روز</span>");
            $("#hours").html(hours + "<span>ساعت</span>");
            $("#minutes").html(minutes + "<span>دقیقه</span>");
            $("#seconds").html(seconds + "<span>ثانیه</span>");
        }
		setInterval(function() { makeTimer(); }, 1000);

        // New News Slides
		$('.new-news-slides').owlCarousel({
			loop: true,
			nav: true,
            rtl: true,
			dots: false,
			autoplayHoverPause: true,
            autoplay: true,
            items: 1,
            animateOut: 'slideOutLeft',
            navText: [
                "<i class='icofont-rounded-right'></i>",
                "<i class='icofont-rounded-left'></i>"
            ],
        });

        // More News Slides
		$('.more-news-slides').owlCarousel({
			loop: true,
			nav: true,
            rtl: true,
			dots: false,
			autoplayHoverPause: true,
			autoplay: true,
            navText: [
                "<i class='icofont-rounded-right'></i>",
                "<i class='icofont-rounded-left'></i>"
            ],
			responsive: {
                0: {
                    items:1,
                },
                768: {
                    items:2,
                },
                1200: {
                    items:3,
				}
            }
        });

        // Default News Slides
		$('.default-news-slides').owlCarousel({
			loop: true,
			nav: true,
            rtl: true,
			dots: false,
			autoplayHoverPause: true,
            autoplay: true,
            navText: [
                "<i class='icofont-rounded-right'></i>",
                "<i class='icofont-rounded-left'></i>"
            ],
            responsive: {
                0: {
                    items:1,
                },
                768: {
                    items:2,
                },
                1200: {
                    items:3,
				}
            }
        });

        // MixItUp Shorting
		$(function(){
            $('.shorting').mixItUp();
        });
		
		// Popup Image
        $('.popup-btn').magnificPopup({
            type: 'image',
            gallery: {
                enabled: true,
            }
        });

        // Video News Slides
		$('.video-news-slides').owlCarousel({
			loop: true,
			nav: true,
            rtl: true,
			dots: false,
			autoplayHoverPause: true,
            autoplay: true,
            navText: [
                "<i class='icofont-rounded-right'></i>",
                "<i class='icofont-rounded-left'></i>"
            ],
            responsive: {
                0: {
                    items:1,
                },
                768: {
                    items:2,
                },
                1200: {
                    items:2,
				}
            }
        });
		
		// Subscribe form
		$(".newsletter-form").validator().on("submit", function (event) {
			if (event.isDefaultPrevented()) {
			// handle the invalid form...
				formErrorSub();
				submitMSGSub(false, "لطفاً ایمیل خود را به درستی وارد کنید.");
			} else {
				// everything looks good!
				event.preventDefault();
			}
		});
		function callbackFunction (resp) {
			if (resp.result === "success") {
				formSuccessSub();
			}
			else {
				formErrorSub();
			}
		}
		function formSuccessSub(){
			$(".newsletter-form")[0].reset();
			submitMSGSub(true, "با تشکر ثبت شد!");
			setTimeout(function() {
				$("#validator-newsletter").addClass('hide');
			}, 4000)
		}
		function formErrorSub(){
			$(".newsletter-form").addClass("animated shake");
			setTimeout(function() {
				$(".newsletter-form").removeClass("animated shake");
			}, 1000)
		}
		function submitMSGSub(valid, msg){
			if(valid){
				var msgClasses = "validation-success";
			} else {
				var msgClasses = "validation-danger";
			}
			$("#validator-newsletter").removeClass().addClass(msgClasses).text(msg);
		}
		// AJAX MailChimp
		$(".newsletter-form").ajaxChimp({
			url: "https://envytheme.us20.list-manage.com/subscribe/post?u=60e1ffe2e8a68ce1204cd39a5&amp;id=42d6d188d9", // Your url MailChimp
			callback: callbackFunction
		});

        // Go to Top
        $(function(){
            //Scroll event
            $(window).on('scroll', function(){
                var scrolled = $(window).scrollTop();
                if (scrolled > 300) $('.go-top').fadeIn('slow');
                if (scrolled < 300) $('.go-top').fadeOut('slow');
            });  
            //Click event
            $('.go-top').on('click', function() {
                $("html, body").animate({ scrollTop: "0" },  500);
            });
		});

	});
	
	// WOW JS
	$(window).on ('load', function (){
        if ($(".wow").length) { 
            var wow = new WOW({
            boxClass: 'wow', // animated element css class (default is wow)
            animateClass: 'animated', // animation css class (default is animated)
            offset: 20, // distance to the element when triggering the animation (default is 0)
            mobile: true, // trigger animations on mobile devices (default is true)
            live: true, // act on asynchronously loaded content (default is true)
          });
          wow.init();
        }
	});
}(jQuery));