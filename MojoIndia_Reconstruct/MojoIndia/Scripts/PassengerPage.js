$(document).ready(function () {   
    DisplaySessionTimeout();
});

var sessionTimeout = 20;
function DisplaySessionTimeout() {
    sessionTimeout = sessionTimeout - 1;
    if (sessionTimeout >= 0)
        window.setTimeout("DisplaySessionTimeout()", 20000);
    else {
        $('#divSessionOut').show();
    }
}

function SearchAgain() {
    $('#divSessionOut').hide();
    if ($("#hfTripType").val() == "3") {
        var strHtml = "";
        var totSector = parseInt($("#totSector").val());
        var percent = "50%";
        if (totSector == 3)
            percent = "33%";
        else if (totSector == 4)
            percent = "25%";
        for (var i = 1; i <= totSector ; i++) {
            var fromDate = $("#departure_date" + i).val().split('-');
            var depDate = "";
            if (fromDate.length == 3) {
                depDate = months[parseInt(fromDate[1]) - 1] + " " + fromDate[2] + ", " + fromDate[0];
            }
            else {
                depDate = $("#departure_date" + i).val();
            }
            strHtml += "<div class='wait_multi-city' style='width:" + percent + "'><h3>" + getAirportCode($("#fromCity" + i).val()) + " - " + getAirportCode($("#toCity" + i).val()) + "</h3><p>" + depDate + "</p></div>"
        }
        $("#Multi_City_wait").html(strHtml);
        $("#oneway_roundTrip_wait1").hide();
        $("#oneway_roundTrip_wait2").hide();
        $("#Multi_City_wait").show();
    }
    else {
        var arrFromCity = $("#fromCity").val().split(',');
        var restFromCity = "";
        for (var i = 1; i < arrFromCity.length; i++) {
            restFromCity += ((i == 1 ? "" : ", ") + arrFromCity[i]);
        }
        var arrFCity = arrFromCity[0].split(')')
        var fromCityName = arrFCity.length >= 2 ? arrFCity[1] : $("#fromCity").val();
        var fromArpName = getAirportCode($("#fromCity").val());

        var arrToCity = $("#toCity").val().split(',');
        var restToCity = "";
        for (var i = 1; i < arrToCity.length; i++) {
            restToCity += ((i == 1 ? "" : ", ") + arrToCity[i]);
        }
        var arrTCity = arrToCity[0].split(')')
        var toCityName = arrTCity.length >= 2 ? arrTCity[1] : $("#toCity").val();
        var toArpName = getAirportCode($("#toCity").val());

        var fromDate = $("#departure_date").val().split('-');
        var fDate = "";
        if (fromDate.length == 3) {
            fDate = months[parseInt(fromDate[1]) - 1] + " " + fromDate[2] + ", " + fromDate[0];
        }
        else {
            fDate = $("#departure_date").val();
        }
        var toDate = $("#return_date").val().split('-');;
        var tDate = "";
        if (toDate.length == 3) {
            tDate = months[parseInt(toDate[1]) - 1] + " " + toDate[2] + ", " + toDate[0];
        }
        else {
            fDate = $("#departure_date").val();
        }
        $("#porgressBarOrg").html("<h3>" + fromArpName + "</h3><p>" + fromCityName + "</p><span>" + fromArpName + " - " + restFromCity + "</span>");
        $("#porgressBarDest").html("<h3>" + toArpName + "</h3><p>" + toCityName + "</p><span>" + toArpName + " - " + restToCity + "</span>");
        $("#porgressBarDate").html((fDate + " - " + ($('#hfTripType').val() == 1 ? "Oneway" : tDate)));
        $("#oneway_roundTrip_wait1").show();
        $("#oneway_roundTrip_wait2").show();
        $("#Multi_City_wait").hide();
    }

    $("#topDivWebsite").hide();
    $("#SearchingProgressBar").show();
}


(function ($, W, D) {
    var JQUERY4U = {};
    JQUERY4U.UTIL =
        {
            setupFormValidation: function () {
                $("#Passenger").validate({
                    rules: {
                        'phoneNo': {
                            required: true,
                            minlength: 10
                        },
                        'emailID': {
                            required: true,
                            email: true
                        },
                        'GSTNo': {
                            required: {
                                depends: function (element) {
                                    if (document.getElementById('gst_question').checked) {
                                        return true;
                                    } else {
                                        return false;
                                    }
                                }
                            },
                            minlength: 15
                        },
                        'GSTCompany': {
                            required: {
                                depends: function (element) {
                                    if (document.getElementById('gst_question').checked) {
                                        return true;
                                    } else {
                                        return false;
                                    }
                                }
                            },
                            minlength: 5
                        },
                        'passengerDetails[0].gender': {
                            required: true,
                            sexreq: true
                        },
                        'passengerDetails[0].title': {
                            required: true,
                            titlereq: true
                        },
                        'passengerDetails[0].firstName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[0].lastName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[0].day': {
                            selectdate: true,
                            age_val: 0
                        },
                        'passengerDetails[0].month': {
                            selectmonth: true,
                            age_val: 0
                        },
                        'passengerDetails[0].year': {
                            selectyear: true,
                            age_val: 0
                        },
                        'passengerDetails[0].passportNumber': {
                            required: true
                        },
                        'passengerDetails[0].exDay': {
                            selectDateExpPassport: true
                        },
                        'passengerDetails[0].exMonth': {
                            selectMonthExpPassport: true
                        },
                        'passengerDetails[0].exYear': {
                            selectYearExpPassport: true
                        },
                        'passengerDetails[1].gender': {
                            required: true,
                            sexreq: true
                        },
                        'passengerDetails[1].title': {
                            required: true,
                            titlereq: true
                        },
                        'passengerDetails[1].firstName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[1].lastName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[1].day': {
                            selectdate: true,
                            age_val: 1
                        },
                        'passengerDetails[1].month': {
                            selectmonth: true,
                            age_val: 1
                        },
                        'passengerDetails[1].year': {
                            selectyear: true,
                            age_val: 1
                        },
                        'passengerDetails[1].passportNumber': {
                            required: true
                        },
                        'passengerDetails[1].exDay': {
                            selectDateExpPassport: true
                        },
                        'passengerDetails[1].exMonth': {
                            selectMonthExpPassport: true
                        },
                        'passengerDetails[1].exYear': {
                            selectYearExpPassport: true
                        },
                        'passengerDetails[2].gender': {
                            required: true,
                            sexreq: true
                        },
                        'passengerDetails[2].title': {
                            required: true,
                            titlereq: true
                        },
                        'passengerDetails[2].firstName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[2].lastName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[2].day': {
                            selectdate: true,
                            age_val: 2
                        },
                        'passengerDetails[2].month': {
                            selectmonth: true,
                            age_val: 2
                        },
                        'passengerDetails[2].year': {
                            selectyear: true,
                            age_val: 2
                        },
                        'passengerDetails[2].passportNumber': {
                            required: true
                        },
                        'passengerDetails[2].exDay': {
                            selectDateExpPassport: true
                        },
                        'passengerDetails[2].exMonth': {
                            selectMonthExpPassport: true
                        },
                        'passengerDetails[2].exYear': {
                            selectYearExpPassport: true
                        },
                        'passengerDetails[3].gender': {
                            required: true,
                            sexreq: true
                        },
                        'passengerDetails[3].title': {
                            required: true,
                            titlereq: true
                        },
                        'passengerDetails[3].firstName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[3].lastName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[3].day': {
                            selectdate: true,
                            age_val: 3
                        },
                        'passengerDetails[3].month': {
                            selectmonth: true,
                            age_val: 3
                        },
                        'passengerDetails[3].year': {
                            selectyear: true,
                            age_val: 3
                        },
                        'passengerDetails[3].passportNumber': {
                            required: true
                        },
                        'passengerDetails[3].exDay': {
                            selectDateExpPassport: true
                        },
                        'passengerDetails[3].exMonth': {
                            selectMonthExpPassport: true
                        },
                        'passengerDetails[3].exYear': {
                            selectYearExpPassport: true
                        },
                        'passengerDetails[4].gender': {
                            required: true,
                            sexreq: true
                        },
                        'passengerDetails[4].title': {
                            required: true,
                            titlereq: true
                        },
                        'passengerDetails[4].firstName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[4].lastName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[4].day': {
                            selectdate: true,
                            age_val: 4
                        },
                        'passengerDetails[4].month': {
                            selectmonth: true,
                            age_val: 4
                        },
                        'passengerDetails[4].year': {
                            selectyear: true,
                            age_val: 4
                        },
                        'passengerDetails[4].passportNumber': {
                            required: true
                        },
                        'passengerDetails[4].exDay': {
                            selectDateExpPassport: true
                        },
                        'passengerDetails[4].exMonth': {
                            selectMonthExpPassport: true
                        },
                        'passengerDetails[4].exYear': {
                            selectYearExpPassport: true
                        },
                        'passengerDetails[5].gender': {
                            required: true,
                            sexreq: true
                        },
                        'passengerDetails[5].title': {
                            required: true,
                            titlereq: true
                        },
                        'passengerDetails[5].firstName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[5].lastName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[5].day': {
                            selectdate: true,
                            age_val: 5
                        },
                        'passengerDetails[5].month': {
                            selectmonth: true,
                            age_val: 5
                        },
                        'passengerDetails[5].year': {
                            selectyear: true,
                            age_val: 5
                        },
                        'passengerDetails[5].passportNumber': {
                            required: true
                        },
                        'passengerDetails[5].exDay': {
                            selectDateExpPassport: true
                        },
                        'passengerDetails[5].exMonth': {
                            selectMonthExpPassport: true
                        },
                        'passengerDetails[5].exYear': {
                            selectYearExpPassport: true
                        },
                        'passengerDetails[6].gender': {
                            required: true,
                            sexreq: true
                        },
                        'passengerDetails[6].title': {
                            required: true,
                            titlereq: true
                        },
                        'passengerDetails[6].firstName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[6].lastName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[6].day': {
                            selectdate: true,
                            age_val: 6
                        },
                        'passengerDetails[6].month': {
                            selectmonth: true,
                            age_val: 6
                        },
                        'passengerDetails[6].year': {
                            selectyear: true,
                            age_val: 6
                        },
                        'passengerDetails[6].passportNumber': {
                            required: true
                        },
                        'passengerDetails[6].exDay': {
                            selectDateExpPassport: true
                        },
                        'passengerDetails[6].exMonth': {
                            selectMonthExpPassport: true
                        },
                        'passengerDetails[6].exYear': {
                            selectYearExpPassport: true
                        },
                        'passengerDetails[7].gender': {
                            required: true,
                            sexreq: true
                        },
                        'passengerDetails[7].title': {
                            required: true,
                            titlereq: true
                        },
                        'passengerDetails[7].firstName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[7].lastName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[7].day': {
                            selectdate: true,
                            age_val: 7
                        },
                        'passengerDetails[7].month': {
                            selectmonth: true,
                            age_val: 7
                        },
                        'passengerDetails[7].year': {
                            selectyear: true,
                            age_val: 7
                        },
                        'passengerDetails[7].passportNumber': {
                            required: true
                        },
                        'passengerDetails[7].exDay': {
                            selectDateExpPassport: true
                        },
                        'passengerDetails[7].exMonth': {
                            selectMonthExpPassport: true
                        },
                        'passengerDetails[7].exYear': {
                            selectYearExpPassport: true
                        },
                        'passengerDetails[8].gender': {
                            required: true,
                            sexreq: true
                        },
                        'passengerDetails[8].title': {
                            required: true,
                            titlereq: true
                        },
                        'passengerDetails[8].firstName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[8].lastName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true,
                            minlength: 2,
                            maxlength: 30
                        },
                        'passengerDetails[8].day': {
                            selectdate: true,
                            age_val: 8
                        },
                        'passengerDetails[8].month': {
                            selectmonth: true,
                            age_val: 8
                        },
                        'passengerDetails[8].year': {
                            selectyear: true,
                            age_val: 8
                        },
                        'passengerDetails[8].passportNumber': {
                            required: true
                        },
                        'passengerDetails[8].exDay': {
                            selectDateExpPassport: true
                        },
                        'passengerDetails[8].exMonth': {
                            selectMonthExpPassport: true
                        },
                        'passengerDetails[8].exYear': {
                            selectYearExpPassport: true
                        },
                        'paymentDetails.address1': {
                            required: true,
                            requiredTextvalue: true,
                        },
                        'paymentDetails.city': {
                            required: true,
                            requiredTextvalue: true,
                        },
                        'paymentDetails.country': {
                            required: true,
                            selectcountry: true
                        },
                        'paymentDetails.postalCode': {
                            required: true
                        },

                    },
                    onkeyup: false,
                    onblur: false,
                    onclick: false,
                    onfocusout: function (element) {
                        $(element).valid();
                    },
                    highlight: function (element) {

                        $(element).closest('.form-group').removeClass('valid').addClass('has-error');
                        $(element).addClass('error-class');

                        if ($(element).attr("name") == "passengerDetails[0].gender" || $(element).attr("name") == "passengerDetails[0].title" || $(element).attr("name") == "passengerDetails[0].firstName" || $(element).attr("name") == "passengerDetails[0].lastName" || $(element).attr("name") == "passengerDetails[0].day" || $(element).attr("name") == "passengerDetails[0].month" || $(element).attr("name") == "passengerDetails[0].year" || $(element).attr("name") == "passengerDetails[0].passportNumber" || $(element).attr("name") == "passengerDetails[0].exDay" || $(element).attr("name") == "passengerDetails[0].exMonth" || $(element).attr("name") == "passengerDetails[0].exYear") {
                            $("#errors0").addClass('help-block').removeClass('valid');
                        } else if ($(element).attr("name") == "passengerDetails[1].gender" || $(element).attr("name") == "passengerDetails[1].title" || $(element).attr("name") == "passengerDetails[1].firstName" || $(element).attr("name") == "passengerDetails[1].lastName" || $(element).attr("name") == "passengerDetails[1].day" || $(element).attr("name") == "passengerDetails[1].month" || $(element).attr("name") == "passengerDetails[1].year" || $(element).attr("name") == "passengerDetails[1].passportNumber" || $(element).attr("name") == "passengerDetails[1].exDay" || $(element).attr("name") == "passengerDetails[1].exMonth" || $(element).attr("name") == "passengerDetails[1].exYear") {
                            $("#errors1").addClass('help-block').removeClass('valid');
                        } else if ($(element).attr("name") == "passengerDetails[2].gender" || $(element).attr("name") == "passengerDetails[2].title" || $(element).attr("name") == "passengerDetails[2].firstName" || $(element).attr("name") == "passengerDetails[2].lastName" || $(element).attr("name") == "passengerDetails[2].day" || $(element).attr("name") == "passengerDetails[2].month" || $(element).attr("name") == "passengerDetails[2].year" || $(element).attr("name") == "passengerDetails[2].passportNumber" || $(element).attr("name") == "passengerDetails[2].exDay" || $(element).attr("name") == "passengerDetails[2].exMonth" || $(element).attr("name") == "passengerDetails[2].exYear") {
                            $("#errors2").addClass('help-block').removeClass('valid');
                        } else if ($(element).attr("name") == "passengerDetails[3].gender" || $(element).attr("name") == "passengerDetails[3].title" || $(element).attr("name") == "passengerDetails[3].firstName" || $(element).attr("name") == "passengerDetails[3].lastName" || $(element).attr("name") == "passengerDetails[3].day" || $(element).attr("name") == "passengerDetails[3].month" || $(element).attr("name") == "passengerDetails[3].year" || $(element).attr("name") == "passengerDetails[3].passportNumber" || $(element).attr("name") == "passengerDetails[3].exDay" || $(element).attr("name") == "passengerDetails[3].exMonth" || $(element).attr("name") == "passengerDetails[3].exYear") {
                            $("#errors3").addClass('help-block').removeClass('valid');
                        } else if ($(element).attr("name") == "passengerDetails[4].gender" || $(element).attr("name") == "passengerDetails[4].title" || $(element).attr("name") == "passengerDetails[4].firstName" || $(element).attr("name") == "passengerDetails[4].lastName" || $(element).attr("name") == "passengerDetails[4].day" || $(element).attr("name") == "passengerDetails[4].month" || $(element).attr("name") == "passengerDetails[4].year" || $(element).attr("name") == "passengerDetails[4].passportNumber" || $(element).attr("name") == "passengerDetails[4].exDay" || $(element).attr("name") == "passengerDetails[4].exMonth" || $(element).attr("name") == "passengerDetails[4].exYear") {
                            $("#errors4").addClass('help-block').removeClass('valid');
                        } else if ($(element).attr("name") == "passengerDetails[5].gender" || $(element).attr("name") == "passengerDetails[5].title" || $(element).attr("name") == "passengerDetails[5].firstName" || $(element).attr("name") == "passengerDetails[5].lastName" || $(element).attr("name") == "passengerDetails[5].day" || $(element).attr("name") == "passengerDetails[5].month" || $(element).attr("name") == "passengerDetails[5].year" || $(element).attr("name") == "passengerDetails[5].passportNumber" || $(element).attr("name") == "passengerDetails[5].exDay" || $(element).attr("name") == "passengerDetails[5].exMonth" || $(element).attr("name") == "passengerDetails[5].exYear") {
                            $("#errors5").addClass('help-block').removeClass('valid');
                        } else if ($(element).attr("name") == "passengerDetails[6].gender" || $(element).attr("name") == "passengerDetails[6].title" || $(element).attr("name") == "passengerDetails[6].firstName" || $(element).attr("name") == "passengerDetails[6].lastName" || $(element).attr("name") == "passengerDetails[6].day" || $(element).attr("name") == "passengerDetails[6].month" || $(element).attr("name") == "passengerDetails[6].year" || $(element).attr("name") == "passengerDetails[6].passportNumber" || $(element).attr("name") == "passengerDetails[6].exDay" || $(element).attr("name") == "passengerDetails[6].exMonth" || $(element).attr("name") == "passengerDetails[6].exYear") {
                            $("#errors6").addClass('help-block').removeClass('valid');
                        } else if ($(element).attr("name") == "passengerDetails[7].gender" || $(element).attr("name") == "passengerDetails[7].title" || $(element).attr("name") == "passengerDetails[7].firstName" || $(element).attr("name") == "passengerDetails[7].lastName" || $(element).attr("name") == "passengerDetails[7].day" || $(element).attr("name") == "passengerDetails[7].month" || $(element).attr("name") == "passengerDetails[7].year" || $(element).attr("name") == "passengerDetails[7].passportNumber" || $(element).attr("name") == "passengerDetails[7].exDay" || $(element).attr("name") == "passengerDetails[7].exMonth" || $(element).attr("name") == "passengerDetails[7].exYear") {
                            $("#errors7").addClass('help-block').removeClass('valid');
                        } else if ($(element).attr("name") == "passengerDetails[8].gender" || $(element).attr("name") == "passengerDetails[8].title" || $(element).attr("name") == "passengerDetails[8].firstName" || $(element).attr("name") == "passengerDetails[8].lastName" || $(element).attr("name") == "passengerDetails[8].day" || $(element).attr("name") == "passengerDetails[8].month" || $(element).attr("name") == "passengerDetails[8].year" || $(element).attr("name") == "passengerDetails[8].passportNumber" || $(element).attr("name") == "passengerDetails[8].exDay" || $(element).attr("name") == "passengerDetails[8].exMonth" || $(element).attr("name") == "passengerDetails[8].exYear") {
                            $("#errors8").addClass('help-block').removeClass('valid');
                        }
                    },
                    unhighlight: function (element) {

                        $(element).closest('.form-group').removeClass('has-error').addClass('valid');
                        $(element).removeClass('error-class');
                        $(element).removeClass('radio-class');
                    },
                    errorElement: 'div',
                    errorClass: 'help-block',
                    errorPlacement: function (error, element) {
                        if (element.attr("name") == "passengerDetails[0].gender" || element.attr("name") == "passengerDetails[0].title" || element.attr("name") == "passengerDetails[0].firstName" || element.attr("name") == "passengerDetails[0].lastName" || element.attr("name") == "passengerDetails[0].day" || element.attr("name") == "passengerDetails[0].month" || element.attr("name") == "passengerDetails[0].year" || element.attr("name") == "passengerDetails[0].passportNumber" || element.attr("name") == "passengerDetails[0].exDay" || element.attr("name") == "passengerDetails[0].exMonth" || element.attr("name") == "passengerDetails[0].exYear") {
                            $("#errors0").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#errors0");
                        } else if (element.attr("name") == "passengerDetails[1].gender" || element.attr("name") == "passengerDetails[1].title" || element.attr("name") == "passengerDetails[1].firstName" || element.attr("name") == "passengerDetails[1].lastName" || element.attr("name") == "passengerDetails[1].day" || element.attr("name") == "passengerDetails[1].month" || element.attr("name") == "passengerDetails[1].year" || element.attr("name") == "passengerDetails[1].passportNumber" || element.attr("name") == "passengerDetails[1].exDay" || element.attr("name") == "passengerDetails[1].exMonth" || element.attr("name") == "passengerDetails[1].exYear") {
                            $("#errors1").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#errors1");
                        } else if (element.attr("name") == "passengerDetails[2].gender" || element.attr("name") == "passengerDetails[2].title" || element.attr("name") == "passengerDetails[2].firstName" || element.attr("name") == "passengerDetails[2].lastName" || element.attr("name") == "passengerDetails[2].day" || element.attr("name") == "passengerDetails[2].month" || element.attr("name") == "passengerDetails[2].year" || element.attr("name") == "passengerDetails[2].passportNumber" || element.attr("name") == "passengerDetails[2].exDay" || element.attr("name") == "passengerDetails[2].exMonth" || element.attr("name") == "passengerDetails[2].exYear") {
                            $("#errors2").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#errors2");
                        } else if (element.attr("name") == "passengerDetails[3].gender" || element.attr("name") == "passengerDetails[3].title" || element.attr("name") == "passengerDetails[3].firstName" || element.attr("name") == "passengerDetails[3].lastName" || element.attr("name") == "passengerDetails[3].day" || element.attr("name") == "passengerDetails[3].month" || element.attr("name") == "passengerDetails[3].year" || element.attr("name") == "passengerDetails[3].passportNumber" || element.attr("name") == "passengerDetails[3].exDay" || element.attr("name") == "passengerDetails[3].exMonth" || element.attr("name") == "passengerDetails[3].exYear") {
                            $("#errors3").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#errors3");
                        } else if (element.attr("name") == "passengerDetails[4].gender" || element.attr("name") == "passengerDetails[4].title" || element.attr("name") == "passengerDetails[4].firstName" || element.attr("name") == "passengerDetails[4].lastName" || element.attr("name") == "passengerDetails[4].day" || element.attr("name") == "passengerDetails[4].month" || element.attr("name") == "passengerDetails[4].year" || element.attr("name") == "passengerDetails[4].passportNumber" || element.attr("name") == "passengerDetails[4].exDay" || element.attr("name") == "passengerDetails[4].exMonth" || element.attr("name") == "passengerDetails[4].exYear") {
                            $("#errors4").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#errors4");
                        } else if (element.attr("name") == "passengerDetails[5].gender" || element.attr("name") == "passengerDetails[5].title" || element.attr("name") == "passengerDetails[5].firstName" || element.attr("name") == "passengerDetails[5].lastName" || element.attr("name") == "passengerDetails[5].day" || element.attr("name") == "passengerDetails[5].month" || element.attr("name") == "passengerDetails[5].year" || element.attr("name") == "passengerDetails[5].passportNumber" || element.attr("name") == "passengerDetails[5].exDay" || element.attr("name") == "passengerDetails[5].exMonth" || element.attr("name") == "passengerDetails[5].exYear") {
                            $("#errors5").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#errors5");
                        } else if (element.attr("name") == "passengerDetails[6].gender" || element.attr("name") == "passengerDetails[6].title" || element.attr("name") == "passengerDetails[6].firstName" || element.attr("name") == "passengerDetails[6].lastName" || element.attr("name") == "passengerDetails[6].day" || element.attr("name") == "passengerDetails[6].month" || element.attr("name") == "passengerDetails[6].year" || element.attr("name") == "passengerDetails[6].passportNumber" || element.attr("name") == "passengerDetails[6].exDay" || element.attr("name") == "passengerDetails[6].exMonth" || element.attr("name") == "passengerDetails[6].exYear") {
                            $("#errors6").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#errors6");
                        } else if (element.attr("name") == "passengerDetails[7].gender" || element.attr("name") == "passengerDetails[7].title" || element.attr("name") == "passengerDetails[7].firstName" || element.attr("name") == "passengerDetails[7].lastName" || element.attr("name") == "passengerDetails[7].day" || element.attr("name") == "passengerDetails[7].month" || element.attr("name") == "passengerDetails[7].year" || element.attr("name") == "passengerDetails[7].passportNumber" || element.attr("name") == "passengerDetails[7].exDay" || element.attr("name") == "passengerDetails[7].exMonth" || element.attr("name") == "passengerDetails[7].exYear") {
                            $("#errors7").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#errors7");
                        } else if (element.attr("name") == "passengerDetails[8].gender" || element.attr("name") == "passengerDetails[8].title" || element.attr("name") == "passengerDetails[8].firstName" || element.attr("name") == "passengerDetails[8].lastName" || element.attr("name") == "passengerDetails[8].day" || element.attr("name") == "passengerDetails[8].month" || element.attr("name") == "passengerDetails[8].year" || element.attr("name") == "passengerDetails[8].passportNumber" || element.attr("name") == "passengerDetails[8].exDay" || element.attr("name") == "passengerDetails[8].exMonth" || element.attr("name") == "passengerDetails[8].exYear") {
                            $("#errors8").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#errors8");
                        } else if (element.attr("name") == "paymentDetails.expiryMonth" || element.attr("name") == "paymentDetails.expiryYear" || element.attr("name") == "paymentDetails.cardNumber" || element.attr("name") == "paymentDetails.cvvNo" || element.attr("name") == "paymentDetails.cardHolderName") {
                            $("#paymentError").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#paymentError");
                        } else if (element.attr("name") == "paymentDetails.country" || element.attr("name") == "paymentDetails.address1" || element.attr("name") == "paymentDetails.city" || element.attr("name") == "paymentDetails.postalCode") {
                            $("#billingError").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#billingError");
                        }
                        else if (element.attr("name") == "agree") {
                            document.getElementById("agreeError").style.display = 'block';
                            $("#agree").addClass('help-block');
                        }
                        else if (element.parent('.input-group').length) {
                            error.insertAfter(element.parent());
                        } else error.insertAfter(element);


                    },
                    messages: {
                        'phoneNo': "Please Enter Valid Contact Number!",
                        'emailID': "Please Enter valid Email ID!",
                        'GSTNo': "Please Enter Valid GST Number!",
                        'GSTCompany': "Please Enter Gst Registerd Cmompany Name!",
                        'passengerDetails[0].gender': {
                            required: " Please select the gender",
                        },
                        'passengerDetails[0].title': {
                            required: " Please select the title",
                        },
                        'passengerDetails[0].firstName': {
                            required: " Please provide the First name",
                        },
                        'passengerDetails[0].lastName': {
                            required: " Please provide the Last name",
                        },
                        'passengerDetails[0].passportNumber': {
                            required: " Please provide the passport no.",
                        },
                        'passengerDetails[1].gender': {
                            required: " Please select the gender",
                        },
                        'passengerDetails[1].title': {
                            required: " Please select the title",
                        },
                        'passengerDetails[1].firstName': {
                            required: " Please provide the First name",
                        },
                        'passengerDetails[1].lastName': {
                            required: " Please provide the Last name",
                        },
                        'passengerDetails[1].passportNumber': {
                            required: " Please provide the passport no.",
                        },
                        'passengerDetails[2].gender': {
                            required: " Please select the gender",
                        },
                        'passengerDetails[2].title': {
                            required: " Please select the title",
                        },
                        'passengerDetails[2].firstName': {
                            required: " Please provide the First name",
                        },
                        'passengerDetails[2].lastName': {
                            required: " Please provide the Last name",
                        },
                        'passengerDetails[2].passportNumber': {
                            required: " Please provide the passport no.",
                        },
                        'passengerDetails[3].gender': {
                            required: " Please select the gender",
                        },
                        'passengerDetails[3].title': {
                            required: " Please select the title",
                        },
                        'passengerDetails[3].firstName': {
                            required: " Please provide the First name",
                        },
                        'passengerDetails[3].lastName': {
                            required: " Please provide the Last name",
                        },
                        'passengerDetails[3].passportNumber': {
                            required: " Please provide the passport no.",
                        },
                        'passengerDetails[4].gender': {
                            required: " Please select the gender",
                        },
                        'passengerDetails[4].title': {
                            required: " Please select the title",
                        },
                        'passengerDetails[4].firstName': {
                            required: " Please provide the First name",
                        },
                        'passengerDetails[4].lastName': {
                            required: " Please provide the Last name",
                        },
                        'passengerDetails[4].passportNumber': {
                            required: " Please provide the passport no.",
                        },
                        'passengerDetails[5].gender': {
                            required: " Please select the gender",
                        },
                        'passengerDetails[5].title': {
                            required: " Please select the title",
                        },
                        'passengerDetails[5].firstName': {
                            required: " Please provide the First name",
                        },
                        'passengerDetails[5].lastName': {
                            required: " Please provide the Last name",
                        },
                        'passengerDetails[5].passportNumber': {
                            required: " Please provide the passport no.",
                        },
                        'passengerDetails[6].gender': {
                            required: " Please select the gender",
                        },
                        'passengerDetails[6].title': {
                            required: " Please select the title",
                        },
                        'passengerDetails[6].firstName': {
                            required: " Please provide the First name",
                        },
                        'passengerDetails[6].lastName': {
                            required: " Please provide the Last name",
                        },
                        'passengerDetails[6].passportNumber': {
                            required: " Please provide the passport no.",
                        },
                        'passengerDetails[7].gender': {
                            required: " Please select the gender",
                        },
                        'passengerDetails[7].title': {
                            required: " Please select the title",
                        },
                        'passengerDetails[7].firstName': {
                            required: " Please provide the First name",
                        },
                        'passengerDetails[7].lastName': {
                            required: " Please provide the Last name",
                        },
                        'passengerDetails[7].passportNumber': {
                            required: " Please provide the passport no.",
                        },
                        'passengerDetails[8].gender': {
                            required: " Please select the gender",
                        },
                        'passengerDetails[8].title': {
                            required: " Please select the title",
                        },
                        'passengerDetails[8].firstName': {
                            required: " Please provide the First name",
                        },
                        'passengerDetails[8].lastName': {
                            required: " Please provide the Last name",
                        },
                        'passengerDetails[8].passportNumber': {
                            required: " Please provide the passport no.",
                        },
                        'paymentDetails.country': {
                            required: " Please provide a billing country",
                        },
                        'paymentDetails.address1': {
                            required: " Please provide a billing address",
                        },
                        'paymentDetails.city': {
                            required: " Please provide a billing city",
                        },
                        'paymentDetails.postalCode': {
                            required: " Please provide billing postal code",
                        },
                    },
                    submitHandler: function (form) {
                        //$("#btn_disable").show();
                        //$("#btn_proceed").hide();

                        $("#SubmitSearchProgress").show();
                        $("#btnSubmit").hide();
                        $('#ResultProgressPopup').show();
                        form.submit();
                    }
                });
            }
        }

    $(D).ready(function ($) {
        $.validator.setDefaults({ ignore: '' });
        $.validator.messages.required = '';

        $.validator.addMethod("email", function (value, element) {
            return this.optional(element) || /^[a-zA-Z0-9._-]+@[a-zA-Z0-9-]+\.[a-zA-Z.]{2,5}$/i.test(value);
        }, " Please provide a valid email address!");

        $.validator.addMethod('sexreq', function (value) {
            return (value != '0' && value != '');
        }, " Please select the gender");
        $.validator.addMethod('titlereq', function (value) {
            return (value != '0' && value != '');
        }, " Please select the title");
        $.validator.addMethod('requiredTextvalue', function (value) {
            return (value != '');
        }, "");

        $.validator.addMethod("lettersonly", function (value, element) {
            if (value.length > 0) {
                return this.optional(element) || /^[a-z ]+$/i.test(value);
            } else {
                return false;
            }
        }, "Please provide Letters and spaces only");

        $.validator.addMethod('selectdate', function (value) {
            return (value != 'Day' && value != '0' && value != '');
        }, " Please select  the day of Date of Birth");

        $.validator.addMethod('selectmonth', function (value) {
            return (value != '0' && value != '');
        }, " Please select the Month of Birth");

        $.validator.addMethod('selectyear', function (value) {
            return (value != 'Year' && value != '0' && value != '');
        }, " Please select the Year of Birth ");

        $.validator.addMethod('selectDateExpPassport', function (value) {
            return (value != 'Day' && value != '0' && value != '');
        }, " Please select  the day of passport expiry");

        $.validator.addMethod('selectMonthExpPassport', function (value) {
            return (value != '0' && value != '');
        }, " Please select the Month of passport expiry");

        $.validator.addMethod('selectYearExpPassport', function (value) {
            return (value != 'Year' && value != '0' && value != '');
        }, " Please select the Year of passport expiry ");

        $.validator.addMethod('selectExpMonth', function (value) {
            return (value != '0' && value != '');
        }, " Please select card expiration month");

        $.validator.addMethod('selectExpYear', function (value) {
            return (value != 'Year' && value != '' && value != '0');
        }, " Please select card expiration year");
        $.validator.addMethod('selectcountry', function (value) {
            return (value != '0' && value != '');
        }, " Please select your billing Country");

        $.validator.addMethod("GSTNo", function (value, element) {
            return this.optional(element) || /^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$/.test(value);
        }, " Please provide a valid GST No.!");
        $.validator.addMethod("age_val", function (value, element, param) {

            var Age = $("#passengerDetails_" + param + "__passengerType").val();
            var agevalidate = "1";
            if (param == 0) {
                agevalidate = "0";
            }
            var html = " Age of Adult, should be greater than or equal to 12yrs at the time of travel.";

            if (Age == 'Adult') {
                html = " Age of Adult, should be greater than or equal to 18yrs at the time of travel.";
                if (false && param == 0) {
                    html = " Booking flights for an unaccompanied minor? Some airlines have restrictions on children under the age of 18 years traveling alone.";
                } else {
                    html = " Age of Adult, should be greater than or equal to 12yrs at the time of travel.";
                }
            } else if (Age == 'Child') {
                html = " Age of Child, should be less than 12yrs and greater than or equal to 2yrs at the time of travel.";
            } else if (Age == 'Infant') {
                html = "  Infant (on lap) should be under 2 years for the entire trip. Please search again with the child option in modify search.";
            } else if (Age == 'InfantWs') {
                html = "  Infant (on seat) should be under 2 years for the entire trip. Please search again with the child option in modify search.";
            }

            var year = $("#passengerDetails_" + param + "__year").val();
            var Month = $("#passengerDetails_" + param + "__month").val();
            var day = $("#passengerDetails_" + param + "__day").val();
            $("#error-message" + param + "").html("").removeClass("show").addClass("hide");
            if (Month != "" && day != "") {
                if (!isValidDate(year, Month, day)) {
                    $("#error-message" + param + "").html(" Please provide Valid Day of Date Of Birth.").removeClass("hide").addClass("show");
                    return false;
                } else if (!isOfAge(year, Month, day, Age, agevalidate)) {

                    $("#error-message" + param + "").html(html).removeClass("hide").addClass("show");
                    return false;
                } else {
                    $("#passengerDetails_" + param + "__year").removeClass("error-class");
                    var Month = $("#passengerDetails_" + param + "__month").removeClass("error-class");
                    var day = $("#passengerDetails_" + param + "__day").removeClass("error-class");
                    $("#error-message" + param + "").html("").removeClass("show").addClass("hide");
                    return true;
                }
            }
            else {
                return true;
            }
        }, "");
        JQUERY4U.UTIL.setupFormValidation();
    });
})(jQuery, window, document);


function getRequired() {
    if (document.getElementById("agree").checked) {
        document.getElementById("agreeError").style.display = 'none';
    } else {
        document.getElementById("agreeError").style.display = 'block';
    }
}
function isOfAge(year, month, day, type, agevalidate) {

    var age = 18;
    var nextage = 150;
    if (type == 'Adult') {
        age = 12;
        if (false && agevalidate == 0) {
            age = 18;
        }
        nextage = 150;
    } else if (type == 'Child') {
        age = 2;
        nextage = 12;
    } else if (type == 'Infant') {
        age = 0;
        nextage = 2;
    } else if (type == 'InfantWs') {
        age = 0;
        nextage = 2;
    }

    var mydates = document.getElementById("ageDate").value;
    var res = mydates.split("-");
    var mydate = new Date();
    mydate.setFullYear(year, month - 1, day);
    var currdate = new Date(res[2], res[0] - 1, res[1]);
    currdate.setFullYear(currdate.getFullYear() - age);
    var nextdate = new Date(res[2], res[0] - 1, res[1]);
    nextdate.setFullYear(nextdate.getFullYear() - nextage);

    if ((mydate - nextdate) < 0) {
        return false;
    }
    if ((currdate - mydate) < 0) {
        return false;
    }
    var spdate = new Date();
    var sdd = spdate.getDate();
    var smm = spdate.getMonth();
    var syyyy = spdate.getFullYear();
    var today = new Date(syyyy, smm, sdd).getTime();
    if (mydate > today) {
        return false;
    }
    return true;
}
function isValidDate(year, month, day) {
    var listofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    if (month == 1 || month > 2) {
        if (day > listofDays[month - 1]) {
            return false;
        }
    }
    if (month == 2) {
        var lyear = false;
        if ((!(year % 4) && year % 100) || !(year % 400)) {
            lyear = true;
        }
        if ((lyear == false) && (day >= 29)) {
            return false;
        }
        if ((lyear == true) && (day > 29)) {
            return false;
        }
    }
    return true;
}

