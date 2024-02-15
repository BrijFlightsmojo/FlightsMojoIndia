$(document).ready(function () {
    $('.MobileJqueryUseOND').on('click', function (e) {
        if ($(window).width() < 765) {
            $(".MobileJqueryUseOND").addClass("MobileSearchAutoComplete");
            $(".mobile_cross").show();
            if ($("#fromCity").val().length == 3 && $("#spanDep").html() != "City Name") {
                $("#fromCity").val($("#fromCity").val() + " - " + $("#spanDep").html());
            }
            if ($("#toCity").val().length == 3 && $("#spanRet").html() != "City Name") {                     
                $("#toCity").val($("#toCity").val() + " - " + $("#spanRet").html());
            }
        }
    });

    $("#fromCity").focus(function () {
        $("#whereHeading").html("From where?");
    });
    $("#toCity").focus(function () {
        $("#whereHeading").html("Where to?");
    });

    $("#departure_date").focus(function () {
        var selData = $("#fromCity").val().split('-');
        $("#fromCity").val(selData[0].trim());

        var toData = $("#toCity").val().split('-');
        $("#toCity").val(toData[0].trim());
    });
    $('.MobileJqueryUseDate').on('click', function (e) {
        if ($(window).width() < 765) {
            $(".MobileJqueryUseDate").addClass("MobileSearchAutoComplete");

        }
    });
    $('.mobile_cross').on('click', function (e) {
        $(".MobileJqueryUseOND").removeClass("MobileSearchAutoComplete");
        $(".mobile_cross").hide();
        var selData = $("#fromCity").val().split('-');
        $("#fromCity").val(selData[0].trim());

        var toData = $("#toCity").val().split('-');
        $("#toCity").val(toData[0].trim());
    });
  
    setSearchEngine();
});

function ChangeTripType(id) {
    if (id == 1) {
        $("#divReturnDate").hide();
        $("#divMultiCity").hide();
        $("#divRoundOne").show();
        $('#hfTripType').val(1);
    }
    else if (id == 2) {
        $("#divReturnDate").show();
        $("#divMultiCity").hide();
        $("#divRoundOne").show();
        $('#hfTripType').val(2);
    }
    else if (id == 3) {
        $("#divMultiCity").show();
        $("#divRoundOne").hide();
        $('#hfTripType').val(3);
    }
}

function submitForm() {
    var validator = $("#flightSearch").validate({
        showErrors: function () {
            if (this.settings.highlight) {
                for (var i = 0; this.errorList[i]; ++i) {
                    this.settings.highlight.call(this, this.errorList[i].element,
                        this.settings.errorClass, this.settings.validClass);
                }
            }
            if (this.settings.unhighlight) {
                for (var i = 0, elements = this.validElements() ; elements[i]; ++i) {
                    this.settings.unhighlight.call(this, elements[i],
                        this.settings.errorClass, this.settings.validClass);
                }
            }
        },
        rules: {
            fromCity: {
                required: true,
                minlength: 3
            },
            toCity: {
                required: true,
                minlength: 3
            },
            departure_date: {
                required: true,
            },
            return_date: {
                required: {
                    depends: function (element) {
                        if ($('#hfTripType').val() == '2') {
                            return true;
                        } else {
                            return false;
                        }
                    }
                }
            },
            Adult: {
                required: true,
                range: [1, 9]

            },
        },
        errorElement: "span",
        messages: {
            txtFrom: {
                required: "Please enter a valid Origin City.",
                minlength: "Origin City must consist of at least 3 characters"
            },
            txtTo: {
                required: "Please enter a valid Destination City.",
                minlength: "Destination City must consist of at least 3 characters",

            },
            departure_date: {
                required: "Please enter a valid Depart Date.",

            },
            return_date: {
                required: "Please enter a valid Return Date.",
            },
        }
    });

    if (validator.form()) {
       
        var validationFlag = true;
        var strError = "";
        try {
            if ($("#fromCity").val().trim().length < 3) {
                strError += " Atleast 3 characters required, departure city / airport code!";
                validationFlag = false;
            }
            if ($("#toCity").val().trim().length == 0 || $("#toCity").val().trim().length < 3) {
                strError += " Atleast 3 characters required return city / airport code!";
                validationFlag = false;
            }
            if ($.trim($("#fromCity").val()) == $.trim($("#toCity").val()) && ($.trim($("#toCity").val()).length > 0 || $.trim($("#fromCity").val()).length > 0)) {
                strError += " Source and Destination City can't be same !";
                validationFlag = false;
            }
            if ($("#departure_date").val() == "") {
                validationFlag = false;
                strError += "<p>&raquo; Departure date can't be blank!</p>";
            }

            if ($("#hfTripType").val() == "2") {
                if ($("#return_date").val() == "") {
                    validationFlag = false;
                    strError += "<p>&raquo; Return date can't be blank!</p>";
                }
                else {
                    var dd = $("#departure_date").val().split('-');
                    var dDate = new Date(parseInt(dd[2], 10), parseInt(dd[1], 10) - 1, parseInt(dd[0], 10));

                    var rd = $("#return_date").val().split('-');
                    var rDate = new Date(parseInt(rd[2], 10), parseInt(rd[1], 10) - 1, parseInt(rd[0], 10));
                    if (dDate > rDate) {
                        strError += "<p>&raquo; Returning date always equal or greater than departure date!</p>";
                        validationFlag = false;
                    }
                }
            }

            if (!validationFlag) {
                alert(strError);
            }
        }
        catch (err) {
            alert(err.toString());
            validationFlag = false;
        }
        if (validationFlag == true) {
            setLocalStorage();

            $("#SubmitSearchEngine").hide();
            $("#SubmitSearchProgress").show();
        }
        return validationFlag;
    }
    else
        return false;
}
function setLocalStorage() {

    try {
        var seg = [];
        var s = {
            originAirport: $("#fromCity").val().trim(),
            destinationAirport: $("#toCity").val().trim(),
            originAirportFull: $("#hfFromCity").val().trim(),
            destinationAirportFull: $("#hfToCity").val().trim(),
            travelDate: $("#departure_date").val().trim()
        }
        seg.push(s);
        if ($("#hfTripType").val() == "2") {
            var s1 = {
                originAirport: $("#toCity").val().trim(),
                destinationAirport: $("#fromCity").val().trim(),
                originAirportFull: $("#hfToCity").val().trim(),
                destinationAirportFull: $("#hfFromCity").val().trim(),
                travelDate: $("#return_date").val().trim()
            }
            seg.push(s1);
        }
        var itin = {
            segment: seg,
            tripType: $("#hfTripType").val(),
            adults: $("#Adult").val(),
            child: $("#Child").val(),
            infants: $("#Infant").val(),
            cabinType: $("#Cabin").val(),
            airline: ""
        }
        var itinerary = [];
        itinerary.push(itin)
        if (localStorage.getItem("f11ghtsm0j01nd1a898jhf98d") != null) {
            var ldata = JSON.parse(localStorage.getItem("f11ghtsm0j01nd1a898jhf98d"));
            var ctr = 0;
            for (var i = 0; i < ldata.length && ctr < 2; i++) {
                if (itin.segment[0].originAirport != ldata[i].segment[0].originAirport || itin.segment[0].destinationAirport != ldata[i].segment[0].destinationAirport || itin.segment[0].travelDate != ldata[i].segment[0].travelDate || itin.tripType != ldata[i].tripType) {
                    itinerary.push(ldata[i]);
                    ctr++;
                }
                else if (itin.tripType == ldata[i].tripType && ldata[i].tripType.tripType == "2") {
                    if (itin.segment[1].travelDate != ldata[i].segment[1].travelDate) {
                        itinerary.push(ldata[i]);
                        ctr++;
                    }
                }
            }
        }
        localStorage.setItem("f11ghtsm0j01nd1a898jhf98d", JSON.stringify(itinerary));

    } catch (e) {

    }
}
function setSearchEngine() {
   
    try {
        if ($("#HomePageindex").length != 0) {
            if (localStorage.getItem("f11ghtsm0j01nd1a898jhf98d") != null) {
                var itinerary = JSON.parse(localStorage.getItem("f11ghtsm0j01nd1a898jhf98d"));
                $("#fromCity").val(itinerary[0].segment[0].originAirport);
                $("#toCity").val(itinerary[0].segment[0].destinationAirport);
                $("#hfFromCity").val(itinerary[0].segment[0].originAirportFull);
                $("#hfToCity").val(itinerary[0].segment[0].destinationAirportFull);
                if ($("#hfFromCity").val() != "" && $("#hfFromCity").val().split('-').length >= 2) {
                    var selData = $("#hfFromCity").val().split('-');
                    var cityData = selData[1].trim().split(',');
                    $("#spanDep").html(cityData[0].trim() + ", " + cityData[1].trim());
                }

                if ($("#hfToCity").val() != "" && $("#hfToCity").val().split('-').length >= 2) {
                    var selData = $("#hfToCity").val().split('-');
                    var cityData = selData[1].trim().split(',');
                    $("#spanRet").html(cityData[0].trim() + ", " + cityData[1].trim());
                }
                var dd = itinerary[0].segment[0].travelDate.split('/');
                if (dd.length == 1)
                    dd = itinerary[0].segment[0].travelDate.split('-');
                var dDate = new Date(parseInt(dd[2], 10), parseInt(dd[1], 10) - 1, parseInt(dd[0], 10));
              
                if (minDate < dDate) {
                    $("#departure_date").val(itinerary[0].segment[0].travelDate);
                }
                if (itinerary[0].segment.length == 2) {
                    var dd1 = itinerary[0].segment[1].travelDate.split('/');
                    if (dd1.length == 1)
                        dd1 = itinerary[0].segment[1].travelDate.split('-');
                    var rDate = new Date(parseInt(dd1[2], 10), parseInt(dd1[1], 10) - 1, parseInt(dd1[0], 10));
                    if (dDate < rDate) {
                        $("#return_date").val(itinerary[0].segment[1].travelDate);
                    }
                    else {
                        var rd = $("#departure_date").val().split('/');
                        if (rd.length == 1)
                            rd = $("#departure_date").val().split('-');
                        var dDate = new Date(parseInt(rd[2], 10), parseInt(rd[1], 10) - 1, parseInt(rd[0], 10));
                        dDate.setDate(dDate.getDate() + 7);
                        var month = (dDate.getMonth() + 1)
                        var strMonth = month <= 9 ? ("0" + month) : month;
                        $("#return_date").val(dDate.getDate() + "-" + strMonth + "-" + dDate.getFullYear());
                    }
                }
                else {
                    var rd = $("#departure_date").val().split('/');
                    if (rd.length == 1)
                        rd = $("#departure_date").val().split('-');
                    var dDate = new Date(parseInt(rd[2], 10), parseInt(rd[1], 10) - 1, parseInt(rd[0], 10));
                    dDate.setDate(dDate.getDate() + 7);
                    var month = (dDate.getMonth() + 1)
                    var strMonth = month <= 9 ? ("0" + month) : month;
                    $("#return_date").val(dDate.getDate() + "-" + strMonth + "-" + dDate.getFullYear());
                }
                $("#Adult").val((itinerary[0].adults != null && itinerary[0].adults != undefined) ? itinerary[0].adults : "1");
                $("#Child").val((itinerary[0].child != null && itinerary[0].child != undefined) ? itinerary[0].child : "0");
                $("#Infant").val((itinerary[0].infants != null && itinerary[0].infants != undefined) ? itinerary[0].infants : "0");
                $("#Cabin").val((itinerary[0].cabinType != null && itinerary[0].cabinType != undefined) ? itinerary[0].cabinType : "1");
                $("#hfTripType").val(itinerary[0].tripType);

                ChangeTripType(itinerary[0].tripType);
                if ($("#hfTripType").val() == "2") {
                    document.getElementById("rbtnRt").checked = true;
                }
                else if ($("#hfTripType").val() == "1") {
                    document.getElementById("rbtnOw").checked = true;
                }
              
                setPaxDropBox();
                var str = "";
                for (var i = 0; i < itinerary.length; i++) {
                    str += "<li onclick=\"SearchFlightFromHistory(" + i + ")\" style='margin-left: 10px;'><a href='#' style='padding:5px;border-bottom:dashed 1px #ccc;'>";
                    str += "<span style='color:#1e3266; font-size:14px; font-weight:bold; '><i class='fa fa-plane'></i>" + itinerary[i].segment[0].originAirport + " - " + itinerary[i].segment[0].destinationAirport + "</span><span style='font-size:11px;color:#808080; padding-left:14px;'>(" + (itinerary[i].segment.length == 2 ? "Round Trip" : "One Way") + ")</span><i  style='padding-left:25px;'class='fa fa-angle-right'></i><p style='padding-left:20px;'>" + itinerary[i].segment[0].travelDate + (itinerary[i].segment.length == 2 ? (" - " + itinerary[i].segment[1].travelDate + " ") : "") + "</p></a></li>";

                }
                $("#totRecentSearch").html(itinerary.length);

                if (itinerary.length > 0) {
                    $("#totRecentSearch").addClass('Scratchpad');
                }
                else {
                    $("#totRecentSearch").removeClass('Scratchpad');
                }
                $("#MySearches").html(str);
                $("#SearchPad").show();
            }
            else {
                $("#SearchPad").hide();
            }
        }
        else {
            $("#SearchPad").hide();
        }

    } catch (e) {

    }
}

function SearchFlightFromHistory(itin) {
    try {
        if ($("#HomePageindex").length != 0) {
            if (localStorage.getItem("f11ghtsm0j01nd1a898jhf98d") != null) {
                var itinerary = JSON.parse(localStorage.getItem("f11ghtsm0j01nd1a898jhf98d"));
                $("#fromCity").val(itinerary[itin].segment[0].originAirport);
                $("#toCity").val(itinerary[itin].segment[0].destinationAirport);

                var dd = itinerary[itin].segment[0].travelDate.split('/');
                if (dd.length == 1)
                    dd = itinerary[itin].segment[0].travelDate.split('-');
                var dDate = new Date(parseInt(dd[2], 10), parseInt(dd[1], 10) - 1, parseInt(dd[0], 10));
                if (minDate < dDate) {
                    $("#departure_date").val(itinerary[itin].segment[0].travelDate);
                }
                if (itinerary[itin].segment.length == 2) {
                    var dd1 = itinerary[itin].segment[1].travelDate.split('/');
                    if (dd1.length == 1)
                        dd1 = itinerary[itin].segment[1].travelDate.split('-');
                    var rDate = new Date(parseInt(dd1[2], 10), parseInt(dd1[1], 10) - 1, parseInt(dd1[0], 10));
                    if (dDate < rDate) {
                        $("#return_date").val(itinerary[itin].segment[1].travelDate);
                    }
                    else {
                        var rd = $("#departure_date").val().split('/');
                        if (rd.length == 1)
                            rd = $("#departure_date").val().split('-');
                        var dDate = new Date(parseInt(rd[2], 10), parseInt(rd[1], 10) - 1, parseInt(rd[0], 10));
                        dDate.setDate(dDate.getDate() + 7);
                        var month = (dDate.getMonth() + 1)
                        var strMonth = month <= 9 ? ("0" + month) : month;
                        $("#return_date").val(dDate.getDate()+ "-" + strMonth + "-" + dDate.getFullYear() );
                    }
                }
                else {
                    var rd = $("#departure_date").val().split('/');
                    if (rd.length == 1)
                        rd = $("#departure_date").val().split('-');
                    var dDate = new Date(parseInt(rd[2], 10), parseInt(rd[1], 10) - 1, parseInt(rd[0], 10));
                    dDate.setDate(dDate.getDate() + 7);
                    var month = (dDate.getMonth() + 1)
                    var strMonth = month <= 9 ? ("0" + month) : month;
                    $("#return_date").val(dDate.getDate() + "-" + strMonth + "-" + dDate.getFullYear());
                }

                $("#Adult").val((itinerary[itin].adults != null && itinerary[itin].adults != undefined) ? itinerary[itin].adults : "1");
                $("#Child").val((itinerary[itin].child != null && itinerary[itin].child != undefined) ? itinerary[itin].child : "0");
                $("#Infant").val((itinerary[itin].infants != null && itinerary[itin].infants != undefined) ? itinerary[itin].infants : "0");
                $("#Cabin").val((itinerary[itin].cabinType != null && itinerary[itin].cabinType != undefined) ? itinerary[itin].cabinType : "1");
                $("#hfTripType").val(itinerary[itin].tripType);

               
                $("#flightSearch").submit();
            }
        }
    } catch (e) {
        $(".searchpop1").hide();
    }
}

$(document).ready(function () {
    var weekday = new Array(7);
    weekday[0] = "Sunday";
    weekday[1] = "Monday";
    weekday[2] = "Tuesday";
    weekday[3] = "Wednesday";
    weekday[4] = "Thursday";
    weekday[5] = "Friday";
    weekday[6] = "Saturday";

    $("#departure_date").datepicker({
        numberOfMonths: $(window).width() < 765 ? 1 : 2,
        dateFormat: "dd-mm-yy",
        minDate: minDate,
        maxDate: maxDate,
        showButtonPanel: $(window).width() < 765,
        onClose: function () {
            var dateDepMin = $('#departure_date').datepicker("getDate");
            if (dateDepMin == null) {
                dateDepMin = minDate;
            }
            if ($("#hfTripType").val() == "2") {
                var dateRetMin = $('#return_date').datepicker("getDate");
                var dMin = new Date(dateDepMin.getFullYear(), dateDepMin.getMonth(), dateDepMin.getDate());
                if (dateRetMin != null) {
                    $("#return_date").datepicker("change", {
                        minDate: new Date(dateDepMin)
                    });
                    var rMin = new Date(dateRetMin.getFullYear(), dateRetMin.getMonth(), dateRetMin.getDate());
                    if (dMin > rMin) {
                        $('#return_date').val($.datepicker.formatDate('dd-mm-yy', new Date(dMin)));
                        $("#return_date").focus();
                    }
                }
                else {
                    if ($('#departure_date').val() != "") {
                        $('#return_date').val($.datepicker.formatDate('dd-mm-yy', new Date(dMin)));
                        $("#return_date").focus();
                    }
                }
            }
            if ($("#hfTripType").val() == "1") {
                if ($(window).width() < 765) {
                    $(".MobileJqueryUseOND").removeClass("MobileSearchAutoComplete");
                    $(".MobileJqueryUseDate").removeClass("MobileSearchAutoComplete");
                }
            }
        }
    });
    $("#return_date").datepicker({
        numberOfMonths: $(window).width() < 765 ? 1 : 2,
        dateFormat: "dd-mm-yy",
        minDate: minDate,
        maxDate: maxDate,
        showButtonPanel: $(window).width() < 765,
        onClose: function () {
            var dateDepMin = $('#departure_date').datepicker("getDate");
            if (dateDepMin == null) {
                dateDepMin = minDate;
            }
            var dateRetMin = $('#return_date').datepicker("getDate");
            var dMin = new Date(dateDepMin.getFullYear(), dateDepMin.getMonth(), dateDepMin.getDate());
            if (dateRetMin != null) {
                var rMin = new Date(dateRetMin.getFullYear(), dateRetMin.getMonth(), dateRetMin.getDate());
                if (dMin > rMin) {
                    alert('Returning date always equal or greater than departure date!');
                    $("#return_date").val('');
                }
            }
            if ($(window).width() < 765) {
                $(".MobileJqueryUseOND").removeClass("MobileSearchAutoComplete");
                $(".MobileJqueryUseDate").removeClass("MobileSearchAutoComplete");
            }
        }
    });
 $("#se_departure_date").datepicker({
        numberOfMonths: $(window).width() < 765 ? 1 : 2,
        dateFormat: "yy-mm-dd",
        minDate: minDate,
        maxDate: maxDate,
        showButtonPanel: $(window).width() < 765,
        onClose: function () {
            var dateDepMin = $('#se_departure_date').datepicker("getDate");
            if (dateDepMin == null) {
                dateDepMin = minDate;
            }
            var dateRetMin = $('#se_return_date').datepicker("getDate");
            var dMin = new Date(dateDepMin.getFullYear(), dateDepMin.getMonth(), dateDepMin.getDate());
            if (dateRetMin != null) {
                $("#se_return_date").datepicker("change", {
                    minDate: new Date(dateDepMin)
                });
                var rMin = new Date(dateRetMin.getFullYear(), dateRetMin.getMonth(), dateRetMin.getDate());
                if (dMin > rMin) {
                    $('#se_return_date').val($.datepicker.formatDate('yy-mm-dd', new Date(dMin)));
                    $("#se_return_date").focus();
                }
            }
            else {
                if ($('#se_departure_date').val() != "") {
                    $('#se_return_date').val($.datepicker.formatDate('yy-mm-dd', new Date(dMin)));
                    $("#se_return_date").focus();
                }
            }

            if ($("#hfTripType").val() == "1") {
                if ($(window).width() < 765) {
                    $(".MobileJqueryUseOND").removeClass("MobileSearchAutoComplete");
                    $(".MobileJqueryUseDate").removeClass("MobileSearchAutoComplete");
                }
            }
        }
    });
    $("#se_return_date").datepicker({
        numberOfMonths: $(window).width() < 765 ? 1 : 2,
        dateFormat: "yy-mm-dd",
        minDate: minDate,
        maxDate: maxDate,
        showButtonPanel: $(window).width() < 765,
        onClose: function () {
            var dateDepMin = $('#se_departure_date').datepicker("getDate");
            if (dateDepMin == null) {
                dateDepMin = minDate;
            }
            var dateRetMin = $('#se_return_date').datepicker("getDate");
            var dMin = new Date(dateDepMin.getFullYear(), dateDepMin.getMonth(), dateDepMin.getDate());
            if (dateRetMin != null) {
                var rMin = new Date(dateRetMin.getFullYear(), dateRetMin.getMonth(), dateRetMin.getDate());
                if (dMin > rMin) {
                    alert('Returning date always equal or greater than departure date!');
                    $("#se_return_date").val('');
                }
            }
            if ($(window).width() < 765) {
                $(".MobileJqueryUseOND").removeClass("MobileSearchAutoComplete");
                $(".MobileJqueryUseDate").removeClass("MobileSearchAutoComplete");
            }
        }
    });
});

var orgIndex = 0;
var destIndex = 0;
var selectFrom = false;
var selectTo = false;
function monkeyPatchAutocomplete() {
    var oldFn = $.ui.autocomplete.prototype._renderItem;
    $.ui.autocomplete.prototype._renderItem = function (ul, item) {
        var re = new RegExp("^" + this.term, "i");
        var t = item.label.replace(re, "<span>" + this.term + "</span>");
        return $("<li></li>").data("item.autocomplete", item).append("<a>" + t + "</a>").appendTo(ul)
    }
}

$(document).ready(function () {
    monkeyPatchAutocomplete();/**/
    var actionUrl = '/service/GetCity';

    $('#fromCity').autocomplete({
        selectFirst: true,
        highlight: true,
        source: function (request, response) {
            var url = actionUrl + "/" + request.term;
            $.ajax({
                url: url,
                type: 'GET',
                success: function (data) {
                    orgIndex = 0;
                    response($.map(data, function (item) {
                        return {
                            label: (item.cityName + " (" + item.airportCode + ")<br>" + item.airportName + ", " + item.countryName+"<img src='/images/country/"+item.countryCode+".svg'>")
                        }
                    }));
                }
            })
        },
        open: function (event, ui) { selectFrom = true; },
        select: function (event, ui) {
            $("#hfFromCity").val(ui.item.label);
            var selData = ui.item.label.split('-');
            $(this).val(selData[0].trim());
            var cityData = selData[1].trim().split(',');
            $("#spanDep").html(cityData[0].trim() + ", " + cityData[1].trim());
            orgIndex = 1;
            selectFrom = false;
            if ($(window).width() < 765) {
                $(this).val(selData[0].trim() + " - " + $("#spanDep").html());
            }
            $("#toCity").focus();
            return false;
        },
        close: function (event, ui) {
            selectFrom = false;
        },
        minLength: 0,
        autoFocus: true
    }).blur(function () {
        if (selectFrom) {
            $("#hfFromCity").val($('ul.ui-autocomplete li:first a').text());
            var selData = $('ul.ui-autocomplete li:first a').text().split('-');
            $("#fromCity").val(selData[0].trim());
            var cityData = selData[1].trim().split(',');
            $("#spanDep").html(cityData[0].trim() + ", " + cityData[1].trim());
            if ($(window).width() < 765) {
                $(this).val(selData[0].trim() + " - " + $("#spanDep").html());
            }
            orgIndex = 1;
        }
    });

    $("#toCity").autocomplete({
        selectFirst: true,
        source: function (request, response) {
            var url = actionUrl + "/" + request.term;
            $.ajax({
                url: url,
                type: 'GET',
                success: function (data) {
                    destIndex = 0;
                    response($.map(data, function (item) {
                        return {
                            label: ("" + item.airportCode + " - " + item.cityName + ", " + item.airportName + ", " + item.countryName)
                        }
                    }));
                }
            })
        },
        open: function (event, ui) {
            selectTo = true;
        },
        select: function (event, ui) {
            $("#hfToCity").val(ui.item.label);
            var selData = ui.item.label.split('-');
            $(this).val(selData[0].trim());
            var cityData = selData[1].trim().split(',');
            $("#spanRet").html(cityData[0].trim() + ", " + cityData[1].trim());
            destIndex = 1;
            selectTo = false;
            if ($(window).width() < 765) {
                $(this).val(selData[0].trim() + " - " + $("#spanRet").html());
            }
            if ($(window).width() < 765) {
                $(".MobileJqueryUseOND").removeClass("MobileSearchAutoComplete");
                $(".MobileJqueryUseDate").addClass("MobileSearchAutoComplete");
                $(".mobile_cross").hide();
                $("#departure_date").focus();
            }

            return false;
        },
        close: function (event, ui) {
            selectTo = false;
        },
        minLength: 0,
        autoFocus: true
    }).blur(function () {
        if (selectTo) {
            $("#hfToCity").val($('ul.ui-autocomplete li:first a').text());
            var selData = $('ul.ui-autocomplete li:first a').text().split('-');
            $("#toCity").val(selData[0].trim());
            var cityData = selData[1].trim().split(',');
            $("#spanRet").html(cityData[0].trim() + ", " + cityData[1].trim());
            destIndex = 1;
        }
    });
});



function MinusPax(pax) {
    var adult = parseInt($("#Adult").val());
    var child = parseInt($("#Child").val());
    var infant = parseInt($("#Infant").val());
    var totpax = adult + child + infant;

    if (pax == "Adult") {
        if (adult > 1) {
            if ((adult * 2) == child) {
                alert("Please decrease child first!!");
            }
            else if (adult == (infant)) {
                alert("Please decrease infant first!!");
            }
            else {
                $("#" + pax).val(adult - 1);
            }
        }
    }
    else if (pax == "Child") {
        if (child > 0) {
            $("#" + pax).val(child - 1);
        }
    }
    else if (pax == "Infant") {
        if (infant > 0) {
            $("#" + pax).val(infant - 1);
        }
    }
    else if (pax == "InfantWs") {
        if (infant > 0) {
            $("#" + pax).val(infant - 1);
        }
    }
    setPaxDropBox();
}
function PlusPax(pax) {
    var adult = parseInt($("#Adult").val());
    var child = parseInt($("#Child").val());
    var infant = parseInt($("#Infant").val());

    var totpax = adult + child + infant;
    if (totpax <= 8) {
        if (pax == "Adult") {
            $("#" + pax).val((adult + 1).toString());
        }
        else if (pax == "Child") {
            if (child < (adult * 2)) {
                $("#" + pax).val((child + 1).toString());
            }
        }
        else if (pax == "Infant") {
            if ((infant) < (adult)) {
                $("#" + pax).val((infant + 1).toString());
            }
        }
    }
    setPaxDropBox();
}
function setPaxDropBox() {
    var adult = parseInt($("#Adult").val());
    var child = parseInt($("#Child").val());
    var infant = parseInt($("#Infant").val());
    var totpax = adult + child + infant;
    $("#spanPaxDetail").html(totpax.toString() + " Passenger(s), "+$("#Cabin option:selected").text() );
}




function se_MinusPax(pax) {
    var adult = parseInt($("#se_Adult").val());
    var child = parseInt($("#se_Child").val());
    var infant = parseInt($("#se_Infant").val());
    var InfantWs = parseInt($("#se_InfantWs").val());
    var totpax = adult + child + infant + InfantWs;

    if (pax == "se_Adult") {
        if (adult > 1) {
            if ((adult * 2) == child) {
                alert("Please decrease child first!!");
            }
            else if (adult == (infant + InfantWs)) {
                alert("Please decrease infant first!!");
            }
            else {
                $("#" + pax).val(adult - 1);
            }
        }
    }
    else if (pax == "se_Child") {
        if (child > 0) {
            $("#" + pax).val(child - 1);
        }
    }
    else if (pax == "se_Infant") {
        if (infant > 0) {
            $("#" + pax).val(infant - 1);
        }
    }
    else if (pax == "se_InfantWs") {
        if (infant > 0) {
            $("#" + pax).val(infant - 1);
        }
    }
    se_setPaxDropBox();
}
function se_PlusPax(pax) {
    var adult = parseInt($("#se_Adult").val());
    var child = parseInt($("#se_Child").val());
    var infant = parseInt($("#se_Infant").val());
    var InfantWs = parseInt($("#se_InfantWs").val());
    var totpax = adult + child + infant + InfantWs;
    if (totpax <= 8) {
        if (pax == "se_Adult") {
            $("#" + pax).val((adult + 1).toString());
        }
        else if (pax == "se_Child") {
            if (child < (adult * 2)) {
                $("#" + pax).val((child + 1).toString());
            }
        }
        else if (pax == "se_Infant") {
            if ((infant + InfantWs) < (adult)) {
                $("#" + pax).val((infant + 1).toString());
            }
        }
    }
    se_setPaxDropBox();
}
function se_setPaxDropBox() {
    var adult = parseInt($("#se_Adult").val());
    var child = parseInt($("#se_Child").val());
    var infant = parseInt($("#se_Infant").val());
    var InfantWs = parseInt($("#se_InfantWs").val());
    var totpax = adult + child + infant + InfantWs;

    $("#se_txtPaxDetail").val(totpax.toString() + " Traveler / " + $("#se_Cabin option:selected").text());
}