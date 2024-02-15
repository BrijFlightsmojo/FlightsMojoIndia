$(document).ready(function () {

    $(".travel-btn-down").click(function () {
        $(".passenger-div").css("display", "block");
        $(".airclass_popup").css("display", "none");
    });
    $(".close-txt").click(function () {
        $(".passenger-div").css("display", "none");
    });

    $(".CssCabinType").click(function () {
        $(".passenger-div").css("display", "none");
        $(".airclass_popup").css("display", "block");

    });

    $(".btn-class").click(function () {
        $(".airclass_popup").css("display", "none");
    });

    $("#se_txtPaxDetail").click(function () {
        $(".passenger-div2").slideDown();
    });
    $("#travellerDiv2").click(function () {
        $(".passenger-div").slideDown();
    });
    $(".closePaxDiv2").click(function () {
        $(".passenger-div2").slideUp();
    });
    $(".close_btn").click(function () {
        $(".searach_popup").hide();
    });
    SetWebsiteDeal();
    setCookiesDiv();
});



function LettersOnly(e) {
    var keycode;
    if (window.event) {
        keycode = window.event.keyCode;
    }
    else if (e) {
        keycode = e.which;
    }
    var k = parseInt(keycode);
    if ((k > 64 && k < 91) || (k > 96 && k < 123) || (k == 32) || (k == 8) || (k == 0)) {
        return true;
    }
    return false;
}
function LettersCommaOnly(e) {
    var keycode;
    if (window.event) {
        keycode = window.event.keyCode;
    }
    else if (e) {
        keycode = e.which;
    }
    var k = parseInt(keycode);
    if ((k > 64 && k < 91) || (k > 96 && k < 123) || (k == 32) || (k == 0) || (k == 8) || (k == 44) || (k == 45)) {
        return true;
    }
    return false;
}
function LettersCommaNoOnly(e) {
    var keycode;
    if (window.event) {
        keycode = window.event.keyCode;
    }
    else if (e) {
        keycode = e.which;
    }
    var k = parseInt(keycode);
    if ((k > 64 && k < 91) || (k > 96 && k < 123) || (k > 47 && k < 58) || (k == 32) || (k == 0) || (k == 8) || (k == 44) || (k == 45)) {
        return true;
    }
    return false;
}
function onlyNumerics(e) {

    var keycode;
    if (window.event) {
        keycode = window.event.keyCode;
    }
    else if (e) {
        keycode = e.which;
    }
    var k = parseInt(keycode);
    if (k > 47 && k < 58 || k == 13 || (k == 8) || (k == 127) || (k == 0))
        return true;
    else
        return false;
}
function ClearData(obj) {

    $("#" + obj).val("");
    $("#" + obj).focus()
}

var DealData = [];
function SetWebsiteDeal() {

    if ($("#hfdealType").length > 0 && $("#ulDealDetails").length > 0)
        try {
            var jsonDate = {
                dealType: $("#hfdealType").val(),
                origin: $("#hforigin").val(),
                destination: $("#hfdestination").val(),
                airline: $("#hfairline").val(),
                tripType: $("#hftripType").val(),
                cabinClass: $("#hfcabinClass").val()
            };
            $.ajax({
                type: "POST",
                url: "/service/getWebsiteDeal",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(jsonDate),
                responseType: "json",
                success: function (response) {
                    DealData = response;
                    var htmlDataTot = "";
                    if (response.length > 0) {
                        for (var i = 0; i < response.length; i++) {
                            var htmlData = "";
                            htmlData += "<li>";
                            htmlData += "    <div class='airline-logo'><img src='/images/flight_small/" + response[i].airline.code + ".gif' alt=''></div>";
                            htmlData += "    <div class='airport'>";
                            htmlData += "        <p><span>" + response[i].origin.airportCode + "</span> " + response[i].origin.cityName + "</p>";
                            htmlData += "        <label>" + getDateDDMMMyyyyy(response[i].depDate) + "</label>";
                            htmlData += "    </div>";
                            htmlData += "    <div class='aeroplane'><img src='/images/airplane.png' alt=''></div>";
                            htmlData += "    <div class='airport return'>";
                            htmlData += "        <p><span>" + response[i].destination.airportCode + "</span> " + response[i].destination.cityName + "</p>";
                            htmlData += "        <label>" + (response[i].tripType == "OneWay" ? "One Way" : getDateDDMMMyyyyy(response[i].retDate)) + "</label>";
                            htmlData += "    </div>";
                            //htmlData += "    <div class='price eng-open' onclick='OpenSearchEnging(this," + i + ")'>";
                            htmlData += "    <div class='price eng-open' onclick=\"OpenPage(this,'" + response[i].deepLink + "')\">";
                           // htmlData += "    <div class='price eng-open'>";
                            htmlData += "        ₹" + response[i].totalFare + "";
                            //htmlData += "<a href="/flight/searchFlightResult?org=""+ response[i].origin.airportCode+"&dest="+ response[i].destination.airportCode+"&depdate="+ response[i].destination.airportCode+"&retdate=&tripType=O&adults=1&child=&infants=&cabin=1&utm_source=1016&currency=inr">"  "" + response[i].origin.cityName + "</a>";
                            htmlData += "    </div>";
                            htmlData += "</li>";
                            i++;
                            if (i < response.length) {
                                htmlData += "<li>";
                                htmlData += "    <div class='airline-logo'><img src='/images/flight_small/" + response[i].airline.code + ".gif' alt=''></div>";
                                htmlData += "    <div class='airport'>";
                                htmlData += "        <p><span>" + response[i].origin.airportCode + "</span> " + response[i].origin.cityName + "</p>";
                                htmlData += "        <label>" + getDateDDMMMyyyyy(response[i].depDate) + "</label>";
                                htmlData += "    </div>";
                                htmlData += "    <div class='aeroplane'><img src='/images/airplane.png' alt=''></div>";
                                htmlData += "    <div class='airport return'>";
                                htmlData += "        <p><span>" + response[i].destination.airportCode + "</span> " + response[i].destination.cityName + "</p>";
                                htmlData += "        <label>" + (response[i].tripType == "OneWay" ? "One Way" : getDateDDMMMyyyyy(response[i].retDate)) + "</label>";
                                htmlData += "    </div>";
                                htmlData += "    <div class='price eng-open' onclick=\"OpenPage(this,'" + response[i].deepLink + "')\">";
                                htmlData += "        ₹" + response[i].totalFare + "";
                                htmlData += "    </div>";
                                htmlData += "</li>";
                                htmlDataTot += htmlData;
                            }
                        }
                        $("#ulDealDetails").html(htmlDataTot)
                    }
                    else {
                        $("#ulDealDetails").html("");
                    }
                },
                error: function (response) {

                    $("#ulDealDetails").html("");
                }
            })
        } catch (i) {
            $("#ulDealDetails").html("");
        }
}
function OpenPage(ctr, deepLink) {
    $(ctr).html("Wait.........")
    window.location.replace(deepLink);

}
var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec'];
function getDateDDMMMyyyyy(dt) {

    var fromDate = dt.split('-');
    var fDate = "";
    if (fromDate.length == 3) {
        fDate = fromDate[2] + " " + months[parseInt(fromDate[1]) - 1] + " " + fromDate[0];
    }
    else {
        fDate = dt;
    }
    return fDate;
}

function OpenSearchEnging(ctr, seq) {
    var deal = $(ctr).offset();
    var outerWidth = $(ctr).width();
    var innerWidth = $(".searach_popup").width();

    if ($(window).width() < 767) {
        var small = $(".searach_popup").css({ position: "absolute", top: deal.top + 65, left: (deal.left + (outerWidth - innerWidth)) - 50 }); //left:deal.left + 250
        $(".searach_popup").show();
    }
    else {
        var small = $(".searach_popup").css({ position: "absolute", top: deal.top + 65, left: (deal.left + (outerWidth - innerWidth) - 50) }); //left:deal.left + 250
        $(".searach_popup").show();
    }
    $("#seFrom").html(DealData[seq].origin.cityName);
    $("#seTo").html(DealData[seq].destination.cityName);
    $("#se_fromCity").val(("(" + DealData[seq].origin.airportCode + ") " + DealData[seq].origin.cityName + ", " + DealData[seq].origin.airportName + (DealData[seq].origin.stateName == "" ? "" : (", " + DealData[seq].origin.stateName)) + ", " + DealData[seq].origin.countryName));
    $("#se_toCity").val(("(" + DealData[seq].destination.airportCode + ") " + DealData[seq].destination.cityName + ", " + DealData[seq].destination.airportName + (DealData[seq].destination.stateName == "" ? "" : (", " + DealData[seq].destination.stateName)) + ", " + DealData[seq].destination.countryName));
    $("#se_hfTripType").val((DealData[seq] == "OneWay" ? "1" : "2"));
    $("#se_departure_date").val(DealData[seq].depDate);
    $("#se_return_date").val(DealData[seq].retDate);
}


function setCookiesDiv() {
    if (localStorage.getItem("fl1ght3m0j0Ind1ACookies") != null) {
        $("#divCookies").hide();
        $("#divCookies1").hide();
    }
    else {
        if ($(window).width() < 765) { $("#divCookies1").show(); }
        else { $("#divCookies").show(); }
    }
}
function setCookies() {
    localStorage.setItem("fl1ght3m0j0Ind1ACookies", true);
    $("#divCookies").hide();
    $("#divCookies1").hide();
}
