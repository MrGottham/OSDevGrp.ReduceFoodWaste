﻿(function($) {
    var methods = {
        show: function() {
            $("#loadProgressOuterContainer").fadeIn('fast');

            // Need to perform a 'fake' setTimeout here to force the browser to process everything after we have showed the elements.
            setTimeout(function() {
                if (/msie|trident|edge/g.test(window.navigator.userAgent.toLowerCase())) {
                    // IE specific hack to make the spinner continue spinning after submit.
                    var progressSpinner = $("#loadProgressOuterContainer #progressSpinner");
                    progressSpinner.remove();
                    $(".progressHorizontalCentering").append(progressSpinner);
                }
            }, 0);
        },

        hide: function() {
            $("#loadProgressOuterContainer").fadeOut('fast');
        }
    };

    function insertProgressHtmlOnPageAndShow() {
        var overlayElement = $("<div id='loadProgressOuterContainer'><div class='loadProgressOverlay'></div><div class='progressVerticalCentering'><div class='progressHorizontalCentering'><h2>Vent venligst, mens data hentes...</h2><br/><br/><img id='progressSpinner' src='../Images/ajax-loader_bigspinner_green.gif'/></div></div></div>");
        $('body').append(overlayElement);
    }

    $().ready(function() {
        // Runs on page load no matter what.
        insertProgressHtmlOnPageAndShow();
        window.onunload = function() {
            // Must hide when unloading page to prevent the overlay being visible when using browser Back button.
            methods.hide().call();
        }
    });

    $.fn.progressOverlay = function(method) {
        if (methods[method]) {
            methods[method].call();
        } else {
            $("The method named " + method + " does not exist on the progressOverlay plugin.");
        }
    };
})(jQuery);