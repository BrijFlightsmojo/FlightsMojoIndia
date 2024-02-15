$(document).ready(function () {
    $('.MobileJqueryUseOND').on('click', function (e) {
        if ($(window).width() < 765) {            
            $('#mobAutoCompleteFrom').show();
            $('#fromCityMob').focus();          
            $('#MobCrossFrom').show();
        }
    });
    $('.MobileJqueryUseOND2').on('click', function (e) {
        if ($(window).width() < 765) {
            $('#mobAutoCompleteTo').show();
            $('#toCityMob').focus();
            $('#MobCrossTo').show();
        }
    });
   

    
   
    $('#MobCrossFrom').on('click', function (e) {
        $('#mobAutoCompleteFrom').hide();    
        $('#MobCrossFrom').hide();
    });
    $('#MobCrossTo').on('click', function (e) {
        $('#mobAutoCompleteTo').hide();
        $('#MobCrossTo').hide();
    });
    setSearchEngine();
    setPaxDropBox();
});

function ChangeTripType(id) {
    $("#anchor1").removeClass("active");
    $("#anchor2").removeClass("active");
    $("#anchor"+id).addClass("active");
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
            if ($("#hfFromCity").val().trim().length < 3) {
                strError += " Atleast 3 characters required, departure city / airport code!";
                validationFlag = false;
            }
            if ($("#hfToCity").val().trim().length == 0 || $("#hfToCity").val().trim().length < 3) {
                strError += " Atleast 3 characters required return city / airport code!";
                validationFlag = false;
            }
            if ($.trim($("#hfFromCity").val()) == $.trim($("#hfToCity").val()) && ($.trim($("#hfToCity").val()).length > 0 || $.trim($("#hfFromCity").val()).length > 0)) {
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
            originAirport: GetAirportCode($("#hfFromCity").val().trim()),
            destinationAirport: GetAirportCode($("#hfToCity").val().trim()),
            originAirportFull: $("#hfFromCity").val().trim(),
            destinationAirportFull: $("#hfToCity").val().trim(),
            travelDate: $("#departure_date").val().trim()
        }
        seg.push(s);
        if ($("#hfTripType").val() == "2") {
            var s1 = {
                originAirport: GetAirportCode($("#hfToCity").val().trim()),
                destinationAirport: GetAirportCode($("#hfFromCity").val().trim()),
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
        if (localStorage.getItem("f11ghtsm0j01nd1a898jhf98dPac") != null) {
            var ldata = JSON.parse(localStorage.getItem("f11ghtsm0j01nd1a898jhf98dPac"));
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
        localStorage.setItem("f11ghtsm0j01nd1a898jhf98dPac", JSON.stringify(itinerary));

    } catch (e) {

    }
}
function GetAirportCode(str)
{
    var strArr = str.split('-');
    if (strArr.length > 1) {
        return strArr[0].trim();
    }
    else {
        return strArr[0].trim().substring(0, 3);
    }
}
const arrMonths = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
function getMonthNo(mName)
{
    for (var i = 0; i < arrMonths.length; i++) {
        if (mName == arrMonths[i])
        {
            return i;
        }
    }
    return 0;
}
function setSearchEngine() {
   
    try {
        if ($("#HomePageindex").length != 0) {
            if (localStorage.getItem("f11ghtsm0j01nd1a898jhf98dPac") != null) {
                var itinerary = JSON.parse(localStorage.getItem("f11ghtsm0j01nd1a898jhf98dPac"));
              
                $("#hfFromCity").val(itinerary[0].segment[0].originAirportFull);
                $("#hfToCity").val(itinerary[0].segment[0].destinationAirportFull);

                $("#fromCityMob").val(itinerary[0].segment[0].originAirportFull);
                $("#toCityMob").val(itinerary[0].segment[0].destinationAirportFull);

                $("#spanFromArpMob").html(itinerary[0].segment[0].originAirport);
                $("#spanToArpMob").html(itinerary[0].segment[0].destinationAirport);

                var selFromData = itinerary[0].segment[0].originAirportFull.split('-');
                var selFromCity = selFromData[1].split(',');
                $("#fromCity").val(selFromCity[0].trim());
                if ($(window).width() < 765) {
                    $("#spanDep").html(selFromCity[0].trim());
                }
                else {
                    $("#spanDep").html("[" + selFromData[0] + "]" + selFromCity[1].trim());
                }
                var selToData = itinerary[0].segment[0].destinationAirportFull.split('-');
                var selToCity = selToData[1].split(',');
                $("#toCity").val(selToCity[0].trim());
              
                if ($(window).width() < 765) {
                    $("#spanRet").html(selToCity[0].trim());
                }
                else {
                    $("#spanRet").html("[" + selToData[0] + "]" + selToCity[1].trim());
                }

                var dd = itinerary[0].segment[0].travelDate.split(' ');
                /*const result = new Date('2022', '2', '28')*/
                var dDate = new Date(parseInt("20" + dd[2], 10), parseInt(getMonthNo(dd[1]), 10), parseInt(dd[0], 10));
              
                if (minDate < dDate) {
                    $("#departure_date").val(itinerary[0].segment[0].travelDate);
                }
                if (itinerary[0].segment.length == 2) {
                    var dd1 = itinerary[0].segment[0].travelDate.split(' ');                   
                    var rDate = new Date(parseInt("20" + dd1[2], 10), parseInt(getMonthNo(dd1[1]), 10), parseInt(dd1[0], 10));
                    if (dDate < rDate) {
                        $("#return_date").val(itinerary[0].segment[1].travelDate);
                    }
                    else {
                        var rd = $("#departure_date").val().split(' ');
                        if (rd.length == 3) {
                            var dDate = new Date(parseInt("20" + rd[2], 10), parseInt(getMonthNo(rd[1]), 10), parseInt(rd[0], 10));
                            dDate.setDate(dDate.getDate() + 7);
                            var month = (dDate.getMonth() + 1)
                            $("#return_date").val(dDate.getDate() + " " + arrMonths[month] + " " + dDate.getFullYear().toString().substr(-2));
                        }
                    }
                }
                else {
                    var rd = $("#departure_date").val().split(' ');
                    if (rd.length == 3) {
                        var dDate = new Date(parseInt("20" + rd[2], 10), parseInt(getMonthNo(rd[1]), 10), parseInt(rd[0], 10));
                        dDate.setDate(dDate.getDate() + 7);
                        var month = (dDate.getMonth() + 1)
                        $("#return_date").val(dDate.getDate() + " " + arrMonths[month] + " " + dDate.getFullYear().toString().substr(-2));
                    }
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
            if (localStorage.getItem("f11ghtsm0j01nd1a898jhf98dPac") != null) {
                var itinerary = JSON.parse(localStorage.getItem("f11ghtsm0j01nd1a898jhf98dPac"));
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

    $("#departure_date").datepicker({
        numberOfMonths: $(window).width() < 765 ? 1 : 1,
        dateFormat: "dd M y",
        minDate: minDate,
        maxDate: maxDate,
        showButtonPanel: true,
        closeText: "Close",
        beforeShow: function () { $(window).width() < 765 ? $('#CalOverLay').show() : $('#CalOverLay').hide(); },
        onClose: function () {
            $('#CalOverLay').hide();
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
        }
    });
    $("#return_date").datepicker({
        numberOfMonths: $(window).width() < 765 ? 1 : 1,
        dateFormat: "dd M y",
        minDate: minDate,
        maxDate: maxDate,
        closeText: "Close",
        showButtonPanel: true,
        beforeShow: function () { $(window).width() < 765 ? $('#CalOverLay').show() : $('#CalOverLay').hide(); },
        onClose: function () {
            $('#CalOverLay').hide();
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
        }
    });
    $("#se_departure_date").datepicker({
        numberOfMonths: $(window).width() < 765 ? 1 : 1,
        dateFormat: "dd M y",
        minDate: minDate,
        maxDate: maxDate,
        showButtonPanel: true,
        beforeShow: function () { $(window).width() < 765 ? $('#CalOverLay').show() : $('#CalOverLay').hide(); },
        onClose: function () {
            $('#CalOverLay').hide();
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
        }
    });
    $("#se_return_date").datepicker({
        numberOfMonths: $(window).width() < 765 ? 1 : 1,
        dateFormat: "dd M y",
        minDate: minDate,
        maxDate: maxDate,
        showButtonPanel: true,
        beforeShow: function () { $(window).width() < 765 ? $('#CalOverLay').show() : $('#CalOverLay').hide(); },
        onClose: function () {
            $('#CalOverLay').hide();
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
        selectFirst: false,
        highlight: false,
        source: function (request, response) {
            var url = actionUrl + "/" + request.term;
            $.ajax({
                url: url,
                type: 'GET',
                success: function (data) {
                    orgIndex = 0;
                    response($.map(data, function (item) {
                        return {
                            label: ("" + item.airportCode + " - " + item.cityName +  ", " + item.countryName)
                        }
                    }));
                }
            })
        },
        open: function (event, ui) { selectFrom = true; },
        select: function (event, ui) {
            $("#hfFromCity").val(ui.item.label);
            var selData = ui.item.label.split('-');
            var selCity = selData[1].split(',');
            $(this).val(selCity[0].trim());
            var cityData = selData[1].trim().split(',');
            if (selData[0]!="")
            $("#spanDep").html("[" + selData[0] + "]"  + cityData[1].trim());
            orgIndex = 1;
            selectFrom = false;
            return false;
        },
        close: function (event, ui) {
            selectFrom = false;
        },
        minLength: 0,
        autoFocus: false
    }).bind('focus', function () {
        if (!$(this).val().trim())
            $(this).keydown();
    });
  
    $("#toCity").autocomplete({
        selectFirst: false,
        highlight: false,
        source: function (request, response) {
            var url = actionUrl + "/" + request.term;
            $.ajax({
                url: url,
                type: 'GET',
                success: function (data) {
                    destIndex = 0;
                    response($.map(data, function (item) {
                        return {
                            label: ("" + item.airportCode + " - " + item.cityName +  ", " + item.countryName)
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
            var selCity = selData[1].split(',');
            $(this).val(selCity[0].trim());
            var cityData = selData[1].trim().split(',');
            if (selData[0] != "")
            $("#spanRet").html("[" + selData[0] + "]" + cityData[1].trim());
            destIndex = 1;
            selectTo = false;
            return false;
        },
        close: function (event, ui) {
            selectTo = false;
        },
        minLength: 0,
        autoFocus: false
    }).bind('focus', function () {
        if (!$(this).val().trim())
            $(this).keydown();
    });


    $('#fromCityMob').autocomplete({
        selectFirst: false,
        highlight: false,
        source: function (request, response) {
            var url = actionUrl + "/" + request.term;
            $.ajax({
                url: url,
                type: 'GET',
                success: function (data) {
                    orgIndex = 0;
                    response($.map(data, function (item) {
                        return {
                            label: ("" + item.airportCode + " - " + item.cityName +  ", " + item.countryName)
                        }
                    }));
                }
            })
        },
        open: function (event, ui) { selectFrom = true; },
        select: function (event, ui) {
            $("#hfFromCity").val(ui.item.label);
            $("#fromCityMob").val(ui.item.label);
            var selData = ui.item.label.split('-');
            var selCity = selData[1].split(',');
            $("#spanFromArpMob").html(selData[0].trim());          
            $("#spanDep").html(selCity[0].trim());
            $('#mobAutoCompleteFrom').hide();
            orgIndex = 1;
            selectFrom = false;
            return false;
        },
        close: function (event, ui) {
            selectFrom = false;
        },
        minLength: 0,
        autoFocus: false
    }).bind('focus', function () {
        if (!$(this).val().trim())
            $(this).keydown();
    });

    $("#toCityMob").autocomplete({
        selectFirst: false,
        highlight: false,
        source: function (request, response) {
            var url = actionUrl + "/" + request.term;
            $.ajax({
                url: url,
                type: 'GET',
                success: function (data) {
                    destIndex = 0;
                    response($.map(data, function (item) {
                        return {
                            label: ("" + item.airportCode + " - " + item.cityName +  ", " + item.countryName)
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
            $("#toCityMob").val(ui.item.label);
            var selData = ui.item.label.split('-');
            var selCity = selData[1].split(',');
            $("#spanToArpMob").html(selData[0].trim());
            $("#spanRet").html(selCity[0].trim());
            $('#mobAutoCompleteTo').hide();
            destIndex = 1;
            selectTo = false;
            return false;
        },
        close: function (event, ui) {
            selectTo = false;
        },
        minLength: 0,
        autoFocus: false
    }).bind('focus', function () {
        if (!$(this).val().trim())
            $(this).keydown();
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
function empytText(textName, spanName,hiddenName)
{
    $("#" + textName).val("");
    $("#" + spanName).html("City Name");
    $("#" + hiddenName).val("");
    $("#" + textName).focus();
}

function empytTextMob(textName,spanArpName, spanName, hiddenName) {
    $("#" + textName).val("");
    $("#" + spanName).html("City");
    $("#" + spanArpName).html("City Name");
    $("#" + hiddenName).val("");
    $("#" + textName).focus();
}