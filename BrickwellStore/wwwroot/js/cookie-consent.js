// cookie-consent.js

$(document).ready(function () {
    // Check if user has previously consented to cookies
    var cookieConsent = getCookie("cookieConsent");
    if (cookieConsent === "accepted") {
        hideCookieConsentBanner();
    } else if (cookieConsent === "declined") {
        // Handle declined consent
    } else {
        showCookieConsentBanner();
    }

    // Handle click on Accept button
    $("#acceptCookiesBtn").click(function () {
        setCookie("cookieConsent", "accepted", 365);
        hideCookieConsentBanner();
    });

    // Handle click on Decline button
    $("#declineCookiesBtn").click(function () {
        setCookie("cookieConsent", "declined", 365);
        // Handle declined consent
    });

    function showCookieConsentBanner() {
        $("#cookieConsentBanner").show();
    }

    function hideCookieConsentBanner() {
        $("#cookieConsentBanner").hide();
    }

    function setCookie(name, value, days) {
        var expires = "";
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + (value || "") + expires + "; path=/";
    }

    function getCookie(name) {
        var nameEQ = name + "=";
        var cookies = document.cookie.split(';');
        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            while (cookie.charAt(0) === ' ') {
                cookie = cookie.substring(1, cookie.length);
            }
            if (cookie.indexOf(nameEQ) === 0) {
                return cookie.substring(nameEQ.length, cookie.length);
            }
        }
        return null;
    }
});
