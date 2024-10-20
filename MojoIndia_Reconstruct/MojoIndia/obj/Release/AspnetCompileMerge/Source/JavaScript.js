﻿function getMomentMonthName(n) {
    var t = "January";
    switch (parseInt(n) + 1) {
        case 1:
            t = "January";
            break;

        case 2:
            t = "February";
            break;

        case 3:
            t = "March";
            break;

        case 4:
            t = "April";
            break;

        case 5:
            t = "May";
            break;

        case 6:
            t = "June";
            break;

        case 7:
            t = "July";
            break;

        case 8:
            t = "August";
            break;

        case 9:
            t = "September";
            break;

        case 10:
            t = "October";
            break;

        case 11:
            t = "November";
            break;

        case 12:
            t = "December";
    }
    return t;
}

function CheckForInputText(n) {
    const t = $(n).siblings(".fa-times-circle"), i = $(n).siblings(".error-message");
    $(n).val().length > 0 ? (t.show(), i.hide()) : t.hide();
}

function CheckDirectFlight() {
    $("#chkDirectFlight").is(":checked") ? $("#hdnDirectIndirect").val("On") : $("#hdnDirectIndirect").val("Off");
}

function ShowHideSearchEngineTab(n) {
    $(".pasenger-popup").css("display", "none");
    $("#T_RT,#T_OW,#T_MC").parent().removeClass("active");
    $("#Tp_roundtrip,#Tp_multicity").removeClass("in active");
    switch (n) {
        case "RT":
            $("#T_RT").parent().addClass("active");
            $("#Tp_roundtrip").addClass("in active");
            $("#Tp_roundtrip").show();
            $("#Tp_multicity").hide();
            $("#hdnTripTypeCode").val("1");
            $("#divReturnSection").show();
            $("#BtnSearchFare_RTOW").show();
            $("#BtnSearchFare_MC").hide();
            $("#H_TriptypeHeading").html("round trip");
            $(".iexchangeIcon").attr("src", "/Content/images/roundtrip-flightIcon.png");
            break;

        case "OW":
            $("#T_OW").parent().addClass("active");
            $("#Tp_roundtrip").addClass("in active");
            $("#Tp_roundtrip").show();
            $("#Tp_multicity").hide();
            $("#hdnTripTypeCode").val("2");
            $("#divReturnSection").hide();
            $("#BtnSearchFare_RTOW").show();
            $("#BtnSearchFare_MC").hide();
            $("#H_TriptypeHeading").html("one way");
            $(".iexchangeIcon").attr("src", "/Content/images/one-wayFlighticon.png");
            break;

        case "MC":
            $("#T_MC").parent().addClass("active");
            $("#Tp_multicity").addClass("in active");
            $("#Tp_roundtrip").hide();
            $("#Tp_multicity").show();
            $("#hdnTripTypeCode").val("3");
            $("#H_TriptypeHeading").html("multicity");
            $("#BtnSearchFare_RTOW").hide();
            $("#BtnSearchFare_MC").show();
    }
    DisableReturnField();
    SetRoundTripMultiCityControls(n);
}

function SetRoundTripMultiCityControls(n) {
    n == "RT" || n == "OW" ? ($("#txtOriginCode_0").val() != "" && ($("#txtOriginCode").val($("#txtOriginCode_0").val()),
    $("#hdnOriginCode").val($("#hdnOriginCode_0").val()), $("#spanOriginCityName").text($("#spanOriginCityName_0").text()),
    $("#txtOriginCode").next().show(), $("#spnOriginErrMsg").hide()), $("#txtDestCode_0").val() != "" && ($("#txtDestCode").val($("#txtDestCode_0").val()),
    $("#hdnDestinationCode").val($("#hdnDestinationCode_0").val()), $("#spanDestCityName").text($("#spanDestCityName_0").text()),
    $("#txtDestCode").next().show(), $("#spnDestErrMsg").hide()), $("#txtDepartDate_0").val() != "" && ($("#txtDepartDate").val($("#txtDepartDate_0").val()),
    $("#spnDepDateErrMsg").hide()), $("#txtPassengers").text($("#txtPassengers_MC").text()),
    $("#txtClassType").text($("#txtClassType_MC").text())) : n == "MC" && ($("#txtOriginCode").val() != "" && ($("#txtOriginCode_0").val($("#txtOriginCode").val()),
    $("#hdnOriginCode_0").val($("#hdnOriginCode").val()), $("#spanOriginCityName_0").text($("#spanOriginCityName").text()),
    $("#txtOriginCode_0").next().show(), $("#spnOriginErrMsg_0").hide()), $("#txtDestCode").val() != "" && ($("#txtDestCode_0").val($("#txtDestCode").val()),
    $("#hdnDestinationCode_0").val($("#hdnDestinationCode").val()), $("#spanDestCityName_0").text($("#spanDestCityName").text()),
    $("#txtDestCode_0").next().show(), $("#spnDestErrMsg_0").hide(), $("#hdnOriginCode_1").val() == "" && ($("#txtOriginCode_1").val($("#txtDestCode").val()),
    $("#hdnOriginCode_1").val($("#hdnDestinationCode").val()), $("#spanOriginCityName_1").text($("#spanDestCityName").text())),
    $("#txtOriginCode_1").next().show(), $("#spnOriginErrMsg_1").hide()), $("#txtDepartDate").val() != "" && ($("#txtDepartDate_0").val($("#txtDepartDate").val()),
    $("#spnDepDateErrMsg_0").hide()), $("#txtDepartDate_2").val() != "" ? ($("#AddSection_2").hide(),
    $("#RemoveSection_2").show(), $("#MC_Sector_3").css("display", "flex"), $("#hdnMC_Sec_2").val("1")) : ($("#AddSection_2").show(),
    $("#RemoveSection_2").hide(), $("#MC_Sector_3").hide(), $("#hdnMC_Sec_2").val("")),
    $("#txtClassType_MC").text($("#txtClassType").text()), $("#txtPassengers_MC").text($("#txtPassengers").text()));
}

function GetCabinClass(n) {
    switch (n) {
        case "Economy":
            return 1;

        case "Premium Economy":
        case "PremiumEconomy":
            return 2;

        case "Business":
            return 3;

        case "First":
            return 4;

        case "All":
            return 0;
    }
}

function GetCabinTypeForDesktop(n, t, i) {
    var r = parseInt($("#txtAdultPassenger").val()), u = parseInt($("#txtChildPassenger").val()), f = parseInt($("#txtInfantSeatPassenger").val()), e = parseInt($("#txtInfantPassenger").val()), o = r + u + e + f;
    $("#LstCabinClass").val(n);
    $("#txtClassType,#txtClassType_MC").text(t);
    $("#txtPassengers,#txtPassengers_MC").text(o + " Passenger(s)");
    $(i).parent().children().removeClass("active");
    $(i).addClass("active");
}

function RemoveSection3() {
    $("#MC_Sector_3").hide();
    $("#txtOriginCode_2").val("");
    $("#hdnOriginCode_2").val("");
    $("#spanOriginCityName_2").text("Airport/City Name");
    $("#txtOriginCode_2").next().hide();
    $("#spnOriginErrMsg_2").hide();
    $("#txtDestCode_2").val("");
    $("#hdnDestinationCode_2").val("");
    $("#spanDestCityName_2").text("Airport/City Name");
    $("#txtDestCode_2").next().hide();
    $("#spnDestErrMsg_2").hide();
    $("#txtDepartDate_2").val("");
    $("#spnDepDateErrMsg_2").hide();
    $("#AddSection_2").show();
    $("#hdnMC_Sec_2").val("");
}

function checkMulticityGetRT() {
    var n = "0";
    return $("#hdnMC_Sec_2").val() != "1" && $("#hdnOriginCode_0").val() == $("#hdnDestinationCode_1").val() && $("#hdnOriginCode_1").val() == $("#hdnDestinationCode_0").val() && (n = "1"),
    n;
}

function UpdatePassengerCount(n, t) {
    var f = 0, e = 0, o = 0, s = 0, h = 0, c = 0, u = $("#txtAdultPassenger").val(), v = $("#txtChildPassenger").val(), i = $("#txtInfantSeatPassenger").val(), r = $("#txtInfantPassenger").val(), l, a;
    switch (t) {
        case "ADT":
            f = u;
            n == 1 ? parseInt(i) + parseInt(v) + parseInt(u) < 9 && $("#txtAdultPassenger").val(parseInt(u) + 1) : f > e + 1 && ($("#txtAdultPassenger").val(parseInt(u) - 1),
            (parseInt(i) == parseInt(u) || parseInt(r) == parseInt(u) || parseInt(r) + parseInt(i) <= parseInt(u) * 2) && (parseInt($("#txtInfantPassenger").val()) > 0 && parseInt(i) < 1 && parseInt(r) == parseInt(u) ? $("#txtInfantPassenger").val(parseInt(r) - 1) : parseInt($("#txtInfantPassenger").val()) > 0 && parseInt(i) > 0 && (parseInt(r) + parseInt(i) > parseInt($("#txtAdultPassenger").val()) * 2 || parseInt(r) == parseInt(u)) && $("#txtInfantPassenger").val(parseInt(r) - 1),
            parseInt($("#txtInfantSeatPassenger").val()) > 0 && parseInt(r) < 1 && parseInt($("#txtInfantSeatPassenger").val()) > parseInt($("#txtAdultPassenger").val()) * 2 ? parseInt($("#txtInfantSeatPassenger").val()) > 1 ? $("#txtInfantSeatPassenger").val(parseInt(i) - 2) : $("#txtInfantSeatPassenger").val(parseInt(i) - 1) : parseInt($("#txtInfantSeatPassenger").val()) > 0 && parseInt(r) > 0 && parseInt(r) + parseInt(i) > parseInt($("#txtAdultPassenger").val()) * 2 && (parseInt($("#txtInfantSeatPassenger").val()) > 1 ? $("#txtInfantSeatPassenger").val(parseInt(i) - 2) : $("#txtInfantSeatPassenger").val(parseInt(i) - 1))));
            break;

        case "CHD":
            f = parseInt($("#txtChildPassenger").val());
            n == 1 ? parseInt(i) + parseInt($("#txtChildPassenger").val()) + parseInt($("#txtAdultPassenger").val()) < 9 && $("#txtChildPassenger").val(parseInt($("#txtChildPassenger").val()) + 1) : f > e && parseInt($("#txtChildPassenger").val()) > 0 && $("#txtChildPassenger").val(parseInt($("#txtChildPassenger").val()) - 1);
            break;

        case "INFL":
            parseInt($("#txtInfantPassenger").val()) <= parseInt($("#txtAdultPassenger").val()) && (f = parseInt($("#txtInfantPassenger").val()));
            n == 1 ? ValidateAdultIosIol() && $("#txtInfantPassenger").val(parseInt($("#txtInfantPassenger").val()) + 1) : f > e && parseInt($("#txtInfantPassenger").val()) > 0 && $("#txtInfantPassenger").val(parseInt($("#txtInfantPassenger").val()) - 1);
            break;

        case "INFS":
            parseInt($("#txtInfantPassenger").val()) + parseInt($("#txtInfantSeatPassenger").val()) <= parseInt($("#txtAdultPassenger").val()) * 2 && (f = parseInt($("#txtInfantSeatPassenger").val()));
            n == 1 ? parseInt($("#txtInfantSeatPassenger").val()) + parseInt($("#txtChildPassenger").val()) + parseInt($("#txtAdultPassenger").val()) < 9 && ValidateAdultIos() && $("#txtInfantSeatPassenger").val(parseInt($("#txtInfantSeatPassenger").val()) + 1) : parseInt($("#txtInfantSeatPassenger").val()) > 0 && $("#txtInfantSeatPassenger").val(parseInt($("#txtInfantSeatPassenger").val()) - 1);
    }
    o = parseInt($("#txtAdultPassenger").val());
    s = parseInt($("#txtChildPassenger").val());
    h = parseInt($("#txtInfantPassenger").val());
    c = parseInt($("#txtInfantSeatPassenger").val());
    l = o + s + h + c;
    a = $("#LstCabinClass").find(":selected").text();
    $("#txtClassType,#txtClassType_MC").text(a);
    $("#txtPassengers,#txtPassengers_MC").text(l + " Passenger(s)");
}

function ValidateAdultIos() {
    var n = !1;
    return parseInt($("#txtInfantPassenger").val()) <= parseInt($("#txtAdultPassenger").val()) && parseInt($("#txtInfantSeatPassenger").val()) < parseInt($("#txtAdultPassenger").val()) * 2 && parseInt($("#txtInfantPassenger").val()) + parseInt($("#txtInfantSeatPassenger").val()) < parseInt($("#txtAdultPassenger").val()) * 2 && (n = !0),
    n;
}

function ValidateAdultIosIol() {
    var n = !1;
    return parseInt($("#txtInfantPassenger").val()) < parseInt($("#txtAdultPassenger").val()) && parseInt($("#txtInfantPassenger").val()) + parseInt($("#txtInfantSeatPassenger").val()) < parseInt($("#txtAdultPassenger").val()) * 2 && (n = !0),
    n;
}

function DisableReturnField() {
    var t, i, n;
    if ($("#divReturnSection").prop("style") != null && $("#divReturnSection").prop("style") != undefined && ($(".lightpick").remove(),
    t = $("#txtDepartDate").val(), i = $("#txtReturnDate").val(), t = t == "" ? "01/01/2019" : t,
    i = i == "" ? "01/01/2019" : i, $("#hdnTripTypeCode").val() === "1" ? ($("#divReturnSection").prop("style").display = "inline",
    n = new Lightpick({
        field: document.getElementById("txtDepartDate"),
        secondField: document.getElementById("txtReturnDate"),
        startDate: t == "01/01/2019" ? null : moment(t),
        endDate: i == "01/01/2019" ? null : moment(i),
        numberOfMonths: 2,
        autoclose: !0,
        repick: !0,
        minDate: moment(),
        maxDate: moment().add(365, "day"),
        onSelect: function (t) {
            if ($("#spnDepDateErrMsg").hide(), $("#spnRetDateErrMsg").hide(), !n._opts.singleDate) if (n._opts.TargetDPId == "txtDepartDate") if (n._opts.minDate = moment(document.getElementById("txtDepartDate").value),
            document.getElementById("txtReturnDate").value == null || document.getElementById("txtReturnDate").value == "") this.hide(),
            setTimeout(function () {
                document.getElementById("txtReturnDate").click();
    }, 500); else {
                var i = document.getElementById("txtReturnDate").value.split("/"), r = i[0], u = i[1], f = i[2], e = new Date(parseInt(f), parseInt(r) - 1, parseInt(u));
                t > e ? (n._opts.startDate = moment(document.getElementById("txtDepartDate").value),
                n._opts.endDate = null, this.hide(), document.getElementById("txtReturnDate").value = "",
                setTimeout(function () {
                    document.getElementById("txtReturnDate").click();
    }, 500)) : this.hide();
    } else this.hide();
    }
    })) : $("#hdnTripTypeCode").val() === "2" && ($("#divReturnSection").prop("style").display = "none",
    n = new Lightpick({
        field: document.getElementById("txtDepartDate"),
        secondField: null,
        startDate: t == "01/01/2019" ? null : moment(t),
        numberOfMonths: 2,
        autoclose: !0,
        repick: !1,
        singleDate: !0,
        minDate: moment(),
        maxDate: moment().add(365, "day"),
        onSelect: function (t) {
            if ($("#spnDepDateErrMsg").hide(), $("#spnRetDateErrMsg").hide(), n._opts.singleDate) document.getElementById("txtReturnDate").value = "",
            this.hide(); else if (n._opts.TargetDPId == "txtDepartDate") if (n._opts.minDate = moment(document.getElementById("txtDepartDate").value),
            document.getElementById("txtReturnDate").value == null || document.getElementById("txtReturnDate").value == "") this.hide(),
            setTimeout(function () {
                document.getElementById("txtReturnDate").click();
    }, 500); else {
                var i = document.getElementById("txtReturnDate").value.split("/"), r = i[0], u = i[1], f = i[2], e = new Date(parseInt(f), parseInt(r) - 1, parseInt(u));
                t > e ? (n._opts.startDate = moment(document.getElementById("txtDepartDate").value),
                n._opts.endDate = null, this.hide(), document.getElementById("txtReturnDate").value = "",
                setTimeout(function () {
                    document.getElementById("txtReturnDate").click();
    }, 500)) : this.hide();
    } else this.hide();
    }
    })), $("#txtReturnDate").val() == "01/01/0001" && $("#txtReturnDate").val("")),
    $("#divDepartSecton_0").prop("style") != null && $("#divDepartSecton_0").prop("style") != undefined && $("#hdnTripTypeCode").val() === "3") {
        var t = $("#txtDepartDate_0").val(), r = $("#txtDepartDate_1").val(), i = $("#txtDepartDate_2").val();
        t = t == "" ? "01/01/2019" : t;
        r = r == "" ? "01/01/2019" : r;
        i = i == "" ? "01/01/2019" : i;
        n = new Lightpick({
            field: document.getElementById("txtDepartDate_0"),
            midField: document.getElementById("txtDepartDate_1"),
            secondField: document.getElementById("txtDepartDate_2"),
            startDate: t == null || t == "01/01/2019" ? null : moment(t),
            midDate: r == null || r == "01/01/2019" ? null : moment(r),
            endDate: i == null || i == "01/01/2019" ? null : moment(i),
            numberOfMonths: 2,
            autoclose: !0,
            repick: !0,
            singleDate: !1,
            multiDate: !0,
            minDate: moment(),
            maxDate: moment().add(365, "day"),
            onSelect: function (t, i) {
                if ($("#spnDepDateErrMsg_0").hide(), $("#spnDepDateErrMsg_1").hide(), $("#spnDepDateErrMsg_2").hide(),
                n._opts.TargetDPId == "txtDepartDate_0") {
                    if (n._opts.minDate = moment(document.getElementById("txtDepartDate_0").value),
                    document.getElementById("txtDepartDate_1").value != null && document.getElementById("txtDepartDate_1").value != "" || document.getElementById("txtDepartDate_2").value != null && document.getElementById("txtDepartDate_2").value != "") {
                        if (document.getElementById("txtDepartDate_1").value != null && document.getElementById("txtDepartDate_1").value != "" && (document.getElementById("txtDepartDate_2").value != null || document.getElementById("txtDepartDate_2").value != "")) {
                            var s = document.getElementById("txtDepartDate_1").value.split("/"), r = document.getElementById("txtDepartDate_2").value.split("/"), u = r[0], f = r[1], e = r[2], o = new Date(parseInt(e), parseInt(u) - 1, parseInt(f)), c = s[0], l = s[1], a = s[2], h = new Date(parseInt(a), parseInt(c) - 1, parseInt(l));
                            t > o && t > h ? (n._opts.startDate = moment(document.getElementById("txtDepartDate_0").value),
                            n._opts.midDate = null, n._opts.endDate = null, document.getElementById("txtDepartDate_1").value = "",
                            document.getElementById("txtDepartDate_2").value = "") : t > h && (n._opts.startDate = moment(document.getElementById("txtDepartDate_0").value),
                            n._opts.midDate = null, document.getElementById("txtDepartDate_1").value = "");
                        }
                        if (document.getElementById("txtDepartDate_1").value != null && document.getElementById("txtDepartDate_1").value != "" && (document.getElementById("txtDepartDate_2").value == null || document.getElementById("txtDepartDate_2").value == "")) {
                            var r = document.getElementById("txtDepartDate_1").value.split("/"), u = r[0], f = r[1], e = r[2], o = new Date(parseInt(e), parseInt(u) - 1, parseInt(f));
                            t > o && (n._opts.startDate = moment(document.getElementById("txtDepartDate_0").value),
                            n._opts.midDate = null, document.getElementById("txtDepartDate_1").value = "");
                        }
                        if (document.getElementById("txtDepartDate_1").value == null && document.getElementById("txtDepartDate_1").value == "" && (document.getElementById("txtDepartDate_2").value != null || document.getElementById("txtDepartDate_2").value != "")) {
                            var r = document.getElementById("txtDepartDate_2").value.split("/"), u = r[0], f = r[1], e = r[2], o = new Date(parseInt(e), parseInt(u) - 1, parseInt(f));
                            t > o && (n._opts.startDate = moment(document.getElementById("txtDepartDate_0").value),
                            n._opts.midDate = null, n._opts.endDate = null, document.getElementById("txtDepartDate_1").value = "",
                            document.getElementById("txtDepartDate_2").value = "");
                        }
                    }
                    this.hide();
                } else if (n._opts.TargetDPId == "txtDepartDate_1") {
                    if (n._opts.minDate = moment(document.getElementById("txtDepartDate_1").value),
                    document.getElementById("txtDepartDate_2").value != null && document.getElementById("txtDepartDate_2").value != "" && (document.getElementById("txtDepartDate_2").value != null || document.getElementById("txtDepartDate_2").value != "")) {
                        var r = document.getElementById("txtDepartDate_2").value.split("/"), u = r[0], f = r[1], e = r[2], o = new Date(parseInt(e), parseInt(u) - 1, parseInt(f));
                        i > o && (n._opts.midDate = moment(document.getElementById("txtDepartDate_1").value),
                        n._opts.endDate = null, document.getElementById("txtDepartDate_2").value = "");
                    }
                    this.hide();
                } else n._opts.TargetDPId == "txtDepartDate_2" && this.hide();
            }
        });
    }
}

function OpenTab(n) {
    n == "FlightTab" ? ($("#Hotels").hide(), $("#Flights").show(), $("#Hotels").removeClass("active"),
    $("#Flights").addClass("active"), document.getElementById("hoteltablink").removeAttribute("class"),
    document.getElementById("flighttablink").setAttribute("class", "active"), $("#FlightHomePageDeals").show(),
    $("#FlightOffers").show(), $("#HotelsExclusiveDeals").hide(), $("#HotelsHomePageDeals").hide(),
    DisableReturnField()) : (GetHotelWidgetAndDeals(), $("#Hotels").show(), $("#Flights").hide(),
    $("#Hotels").addClass("active"), $("#Flights").removeClass("active"), document.getElementById("hoteltablink").setAttribute("class", "active"),
    document.getElementById("flighttablink").removeAttribute("class"), $("#FlightHomePageDeals").hide(),
    $("#FlightOffers").hide(), $("#HotelsExclusiveDeals").show(), $("#HotelsHomePageDeals").show(),
    InitHotelDatePicker());
}

function GetHotelWidgetAndDeals() {
    return ($("#formHotelSearch") == null || $("#formHotelSearch") == undefined || $("#formHotelSearch").length == 0) && $.ajax({
        url: "/Flight/GetHotelWidgetAndDeals",
        type: "GET",
        cache: !1,
        dataType: "json",
        async: !1,
        contentType: "application/json; charset=utf-8",
        success: function (n) {
            $("#Hotels").html(n.DealContent1);
            $("#HotelDealsWrapper").html(n.DealContent2);
        },
        error: function () {
            $("#Hotels").html("");
            $("#HotelDealsWrapper").html("");
        }
    }), !0;
}

(function (n, t) {
    typeof exports == "object" && typeof module != "undefined" ? module.exports = t() : typeof define == "function" && define.amd ? define(t) : n.moment = t();
})(this, function () {
    "use strict";
    function t() {
        return bf.apply(null, arguments);
    }
    function oh(n) {
        bf = n;
    }
    function ft(n) {
        return n instanceof Array || Object.prototype.toString.call(n) === "[object Array]";
    }
    function vi(n) {
        return n != null && Object.prototype.toString.call(n) === "[object Object]";
    }
    function sh(n) {
        if (Object.getOwnPropertyNames) return Object.getOwnPropertyNames(n).length === 0;
        var t;
        for (t in n) if (n.hasOwnProperty(t)) return !1;
        return !0;
    }
    function b(n) {
        return n === void 0;
    }
    function dt(n) {
        return typeof n == "number" || Object.prototype.toString.call(n) === "[object Number]";
    }
    function tr(n) {
        return n instanceof Date || Object.prototype.toString.call(n) === "[object Date]";
    }
    function kf(n, t) {
        for (var r = [], i = 0; i < n.length; ++i) r.push(t(n[i], i));
        return r;
    }
    function l(n, t) {
        return Object.prototype.hasOwnProperty.call(n, t);
    }
    function wt(n, t) {
        for (var i in t) l(t, i) && (n[i] = t[i]);
        return l(t, "toString") && (n.toString = t.toString), l(t, "valueOf") && (n.valueOf = t.valueOf),
        n;
    }
    function et(n, t, i, r) {
        return go(n, t, i, r, !0).utc();
    }
    function hh() {
        return {
            empty: !1,
            unusedTokens: [],
            unusedInput: [],
            overflow: -2,
            charsLeftOver: 0,
            nullInput: !1,
            invalidMonth: null,
            invalidFormat: !1,
            userInvalidated: !1,
            iso: !1,
            parsedDateParts: [],
            meridiem: null,
            rfc2822: !1,
            weekdayMismatch: !1
        };
    }
    function u(n) {
        return n._pf == null && (n._pf = hh()), n._pf;
    }
    function fu(n) {
        if (n._isValid == null) {
            var t = u(n), r = df.call(t.parsedDateParts, function (n) {
                return n != null;
            }), i = !isNaN(n._d.getTime()) && t.overflow < 0 && !t.empty && !t.invalidMonth && !t.invalidWeekday && !t.weekdayMismatch && !t.nullInput && !t.invalidFormat && !t.userInvalidated && (!t.meridiem || t.meridiem && r);
            if (n._strict && (i = i && t.charsLeftOver === 0 && t.unusedTokens.length === 0 && t.bigHour === undefined),
            Object.isFrozen != null && Object.isFrozen(n)) return i;
            n._isValid = i;
        }
        return n._isValid;
    }
    function ir(n) {
        var t = et(NaN);
        return n != null ? wt(u(t), n) : u(t).userInvalidated = !0, t;
    }
    function eu(n, t) {
        var i, r, f;
        if (b(t._isAMomentObject) || (n._isAMomentObject = t._isAMomentObject), b(t._i) || (n._i = t._i),
        b(t._f) || (n._f = t._f), b(t._l) || (n._l = t._l), b(t._strict) || (n._strict = t._strict),
        b(t._tzm) || (n._tzm = t._tzm), b(t._isUTC) || (n._isUTC = t._isUTC), b(t._offset) || (n._offset = t._offset),
        b(t._pf) || (n._pf = u(t)), b(t._locale) || (n._locale = t._locale), rr.length > 0) for (i = 0; i < rr.length; i++) r = rr[i],
        f = t[r], b(f) || (n[r] = f);
        return n;
    }
    function yi(n) {
        eu(this, n);
        this._d = new Date(n._d != null ? n._d.getTime() : NaN);
        this.isValid() || (this._d = new Date(NaN));
        ur === !1 && (ur = !0, t.updateOffset(this), ur = !1);
    }
    function ot(n) {
        return n instanceof yi || n != null && n._isAMomentObject != null;
    }
    function d(n) {
        return n < 0 ? Math.ceil(n) || 0 : Math.floor(n);
    }
    function f(n) {
        var t = +n, i = 0;
        return t !== 0 && isFinite(t) && (i = d(t)), i;
    }
    function gf(n, t, i) {
        for (var e = Math.min(n.length, t.length), o = Math.abs(n.length - t.length), u = 0, r = 0; r < e; r++) (i && n[r] !== t[r] || !i && f(n[r]) !== f(t[r])) && u++;
        return u + o;
    }
    function ne(n) {
        t.suppressDeprecationWarnings === !1 && typeof console != "undefined" && console.warn && console.warn("Deprecation warning: " + n);
    }
    function g(n, i) {
        var r = !0;
        return wt(function () {
            var e, u, f, o;
            if (t.deprecationHandler != null && t.deprecationHandler(null, n), r) {
                for (e = [], f = 0; f < arguments.length; f++) {
                    if (u = "", typeof arguments[f] == "object") {
                        u += "\n[" + f + "] ";
                        for (o in arguments[0]) u += o + ": " + arguments[0][o] + ", ";
                        u = u.slice(0, -2);
                    } else u = arguments[f];
                    e.push(u);
                }
                ne(n + "\nArguments: " + Array.prototype.slice.call(e).join("") + "\n" + new Error().stack);
                r = !1;
            }
            return i.apply(this, arguments);
        }, i);
    }
    function te(n, i) {
        t.deprecationHandler != null && t.deprecationHandler(n, i);
        ou[n] || (ne(i), ou[n] = !0);
    }
    function st(n) {
        return n instanceof Function || Object.prototype.toString.call(n) === "[object Function]";
    }
    function ch(n) {
        var t, i;
        for (i in n) t = n[i], st(t) ? this[i] = t : this["_" + i] = t;
        this._config = n;
        this._dayOfMonthOrdinalParseLenient = new RegExp((this._dayOfMonthOrdinalParse.source || this._ordinalParse.source) + "|" + /\d{1,2}/.source);
    }
    function ie(n, t) {
        var r = wt({}, n), i;
        for (i in t) l(t, i) && (vi(n[i]) && vi(t[i]) ? (r[i] = {}, wt(r[i], n[i]), wt(r[i], t[i])) : t[i] != null ? r[i] = t[i] : delete r[i]);
        for (i in n) l(n, i) && !l(t, i) && vi(n[i]) && (r[i] = wt({}, r[i]));
        return r;
    }
    function su(n) {
        n != null && this.set(n);
    }
    function lh(n, t, i) {
        var r = this._calendar[n] || this._calendar.sameElse;
        return st(r) ? r.call(t, i) : r;
    }
    function ah(n) {
        var t = this._longDateFormat[n], i = this._longDateFormat[n.toUpperCase()];
        return t || !i ? t : (this._longDateFormat[n] = i.replace(/MMMM|MM|DD|dddd/g, function (n) {
            return n.slice(1);
        }), this._longDateFormat[n]);
    }
    function vh() {
        return this._invalidDate;
    }
    function yh(n) {
        return this._ordinal.replace("%d", n);
    }
    function ph(n, t, i, r) {
        var u = this._relativeTime[i];
        return st(u) ? u(n, t, i, r) : u.replace(/%d/i, n);
    }
    function wh(n, t) {
        var i = this._relativeTime[n > 0 ? "future" : "past"];
        return st(i) ? i(t) : i.replace(/%s/i, t);
    }
    function p(n, t) {
        var i = n.toLowerCase();
        ui[i] = ui[i + "s"] = ui[t] = n;
    }
    function nt(n) {
        return typeof n == "string" ? ui[n] || ui[n.toLowerCase()] : undefined;
    }
    function hu(n) {
        var r = {}, i, t;
        for (t in n) l(n, t) && (i = nt(t), i && (r[i] = n[t]));
        return r;
    }
    function w(n, t) {
        cu[n] = t;
    }
    function bh(n) {
        var t = [], i;
        for (i in n) t.push({
            unit: i,
            priority: cu[i]
        });
        return t.sort(function (n, t) {
            return n.priority - t.priority;
        }), t;
    }
    function lt(n, t, i) {
        var r = "" + Math.abs(n), u = t - r.length, f = n >= 0;
        return (f ? i ? "+" : "" : "-") + Math.pow(10, Math.max(0, u)).toString().substr(1) + r;
    }
    function r(n, t, i, r) {
        var u = r;
        typeof r == "string" && (u = function () {
            return this[r]();
        });
        n && (fi[n] = u);
        t && (fi[t[0]] = function () {
            return lt(u.apply(this, arguments), t[1], t[2]);
        });
        i && (fi[i] = function () {
            return this.localeData().ordinal(u.apply(this, arguments), n);
        });
    }
    function kh(n) {
        return n.match(/\[[\s\S]/) ? n.replace(/^\[|\]$/g, "") : n.replace(/\\/g, "");
    }
    function dh(n) {
        for (var t = n.match(ce), i = 0, r = t.length; i < r; i++) t[i] = fi[t[i]] ? fi[t[i]] : kh(t[i]);
        return function (i) {
            for (var f = "", u = 0; u < r; u++) f += st(t[u]) ? t[u].call(i, n) : t[u];
            return f;
        };
    }
    function er(n, t) {
        return n.isValid() ? (t = le(t, n.localeData()), lu[t] = lu[t] || dh(t), lu[t](n)) : n.localeData().invalidDate();
    }
    function le(n, t) {
        function r(n) {
            return t.longDateFormat(n) || n;
        }
        var i = 5;
        for (fr.lastIndex = 0; i >= 0 && fr.test(n) ;) n = n.replace(fr, r), fr.lastIndex = 0,
        i -= 1;
        return n;
    }
    function i(n, t, i) {
        yu[n] = st(t) ? t : function (n) {
            return n && i ? i : t;
        };
    }
    function nc(n, t) {
        return l(yu, n) ? yu[n](t._strict, t._locale) : new RegExp(tc(n));
    }
    function tc(n) {
        return gt(n.replace("\\", "").replace(/\\(\[)|\\(\])|\[([^\]\[]*)\]|\\(.)/g, function (n, t, i, r, u) {
            return t || i || r || u;
        }));
    }
    function gt(n) {
        return n.replace(/[-\/\\^$*+?.()|[\]{}]/g, "\\$&");
    }
    function h(n, t) {
        var i, r = t;
        for (typeof n == "string" && (n = [n]), dt(t) && (r = function (n, i) {
            i[t] = f(n);
        }), i = 0; i < n.length; i++) ar[n[i]] = r;
    }
    function wi(n, t) {
        h(n, function (n, i, r, u) {
            r._w = r._w || {};
            t(n, r._w, r, u);
        });
    }
    function ic(n, t, i) {
        t != null && l(ar, n) && ar[n](t, i._a, i, n);
    }
    function bi(n) {
        return vr(n) ? 366 : 365;
    }
    function vr(n) {
        return n % 4 == 0 && n % 100 != 0 || n % 400 == 0;
    }
    function fc() {
        return vr(this.year());
    }
    function ei(n, i) {
        return function (r) {
            return r != null ? (we(this, n, r), t.updateOffset(this, i), this) : yr(this, n);
        };
    }
    function yr(n, t) {
        return n.isValid() ? n._d["get" + (n._isUTC ? "UTC" : "") + t]() : NaN;
    }
    function we(n, t, i) {
        n.isValid() && !isNaN(i) && (t === "FullYear" && vr(n.year()) && n.month() === 1 && n.date() === 29 ? n._d["set" + (n._isUTC ? "UTC" : "") + t](i, n.month(), pr(i, n.month())) : n._d["set" + (n._isUTC ? "UTC" : "") + t](i));
    }
    function ec(n) {
        return (n = nt(n), st(this[n])) ? this[n]() : this;
    }
    function oc(n, t) {
        var r, i;
        if (typeof n == "object") for (n = hu(n), r = bh(n), i = 0; i < r.length; i++) this[r[i].unit](n[r[i].unit]); else if (n = nt(n),
        st(this[n])) return this[n](t);
        return this;
    }
    function sc(n, t) {
        return (n % t + t) % t;
    }
    function pr(n, t) {
        if (isNaN(n) || isNaN(t)) return NaN;
        var i = sc(t, 12);
        return n += (t - i) / 12, i === 1 ? vr(n) ? 29 : 28 : 31 - i % 7 % 2;
    }
    function hc(n, t) {
        return n ? ft(this._months) ? this._months[n.month()] : this._months[(this._months.isFormat || wu).test(t) ? "format" : "standalone"][n.month()] : ft(this._months) ? this._months : this._months.standalone;
    }
    function cc(n, t) {
        return n ? ft(this._monthsShort) ? this._monthsShort[n.month()] : this._monthsShort[wu.test(t) ? "format" : "standalone"][n.month()] : ft(this._monthsShort) ? this._monthsShort : this._monthsShort.standalone;
    }
    function lc(n, t, i) {
        var u, r, e, f = n.toLocaleLowerCase();
        if (!this._monthsParse) for (this._monthsParse = [], this._longMonthsParse = [],
        this._shortMonthsParse = [], u = 0; u < 12; ++u) e = et([2e3, u]), this._shortMonthsParse[u] = this.monthsShort(e, "").toLocaleLowerCase(),
        this._longMonthsParse[u] = this.months(e, "").toLocaleLowerCase();
        return i ? t === "MMM" ? (r = a.call(this._shortMonthsParse, f), r !== -1 ? r : null) : (r = a.call(this._longMonthsParse, f),
        r !== -1 ? r : null) : t === "MMM" ? (r = a.call(this._shortMonthsParse, f), r !== -1) ? r : (r = a.call(this._longMonthsParse, f),
        r !== -1 ? r : null) : (r = a.call(this._longMonthsParse, f), r !== -1) ? r : (r = a.call(this._shortMonthsParse, f),
        r !== -1 ? r : null);
    }
    function ac(n, t, i) {
        var r, u, f;
        if (this._monthsParseExact) return lc.call(this, n, t, i);
        for (this._monthsParse || (this._monthsParse = [], this._longMonthsParse = [], this._shortMonthsParse = []),
        r = 0; r < 12; r++) if ((u = et([2e3, r]), i && !this._longMonthsParse[r] && (this._longMonthsParse[r] = new RegExp("^" + this.months(u, "").replace(".", "") + "$", "i"),
        this._shortMonthsParse[r] = new RegExp("^" + this.monthsShort(u, "").replace(".", "") + "$", "i")),
        i || this._monthsParse[r] || (f = "^" + this.months(u, "") + "|^" + this.monthsShort(u, ""),
        this._monthsParse[r] = new RegExp(f.replace(".", ""), "i")), i && t === "MMMM" && this._longMonthsParse[r].test(n)) || i && t === "MMM" && this._shortMonthsParse[r].test(n) || !i && this._monthsParse[r].test(n)) return r;
    }
    function ke(n, t) {
        var i;
        if (!n.isValid()) return n;
        if (typeof t == "string") if (/^\d+$/.test(t)) t = f(t); else if (t = n.localeData().monthsParse(t),
        !dt(t)) return n;
        return i = Math.min(n.date(), pr(n.year(), t)), n._d["set" + (n._isUTC ? "UTC" : "") + "Month"](t, i),
        n;
    }
    function de(n) {
        return n != null ? (ke(this, n), t.updateOffset(this, !0), this) : yr(this, "Month");
    }
    function vc() {
        return pr(this.year(), this.month());
    }
    function yc(n) {
        return this._monthsParseExact ? (l(this, "_monthsRegex") || to.call(this), n ? this._monthsShortStrictRegex : this._monthsShortRegex) : (l(this, "_monthsShortRegex") || (this._monthsShortRegex = ge),
        this._monthsShortStrictRegex && n ? this._monthsShortStrictRegex : this._monthsShortRegex);
    }
    function pc(n) {
        return this._monthsParseExact ? (l(this, "_monthsRegex") || to.call(this), n ? this._monthsStrictRegex : this._monthsRegex) : (l(this, "_monthsRegex") || (this._monthsRegex = no),
        this._monthsStrictRegex && n ? this._monthsStrictRegex : this._monthsRegex);
    }
    function to() {
        function f(n, t) {
            return t.length - n.length;
        }
        for (var i = [], r = [], t = [], u, n = 0; n < 12; n++) u = et([2e3, n]), i.push(this.monthsShort(u, "")),
        r.push(this.months(u, "")), t.push(this.months(u, "")), t.push(this.monthsShort(u, ""));
        for (i.sort(f), r.sort(f), t.sort(f), n = 0; n < 12; n++) i[n] = gt(i[n]), r[n] = gt(r[n]);
        for (n = 0; n < 24; n++) t[n] = gt(t[n]);
        this._monthsRegex = new RegExp("^(" + t.join("|") + ")", "i");
        this._monthsShortRegex = this._monthsRegex;
        this._monthsStrictRegex = new RegExp("^(" + r.join("|") + ")", "i");
        this._monthsShortStrictRegex = new RegExp("^(" + i.join("|") + ")", "i");
    }
    function wc(n, t, i, r, u, f, e) {
        var o;
        return n < 100 && n >= 0 ? (o = new Date(n + 400, t, i, r, u, f, e), isFinite(o.getFullYear()) && o.setFullYear(n)) : o = new Date(n, t, i, r, u, f, e),
        o;
    }
    function ki(n) {
        var t, i;
        return n < 100 && n >= 0 ? (i = Array.prototype.slice.call(arguments), i[0] = n + 400,
        t = new Date(Date.UTC.apply(null, i)), isFinite(t.getUTCFullYear()) && t.setUTCFullYear(n)) : t = new Date(Date.UTC.apply(null, arguments)),
        t;
    }
    function wr(n, t, i) {
        var r = 7 + t - i, u = (7 + ki(n, 0, r).getUTCDay() - t) % 7;
        return -u + r - 1;
    }
    function io(n, t, i, r, u) {
        var s = (7 + i - r) % 7, h = wr(n, r, u), f = 1 + 7 * (t - 1) + s + h, e, o;
        return f <= 0 ? (e = n - 1, o = bi(e) + f) : f > bi(n) ? (e = n + 1, o = f - bi(n)) : (e = n,
        o = f), {
            year: e,
            dayOfYear: o
        };
    }
    function di(n, t, i) {
        var e = wr(n.year(), t, i), r = Math.floor((n.dayOfYear() - e - 1) / 7) + 1, f, u;
        return r < 1 ? (u = n.year() - 1, f = r + ti(u, t, i)) : r > ti(n.year(), t, i) ? (f = r - ti(n.year(), t, i),
        u = n.year() + 1) : (u = n.year(), f = r), {
            week: f,
            year: u
        };
    }
    function ti(n, t, i) {
        var r = wr(n, t, i), u = wr(n + 1, t, i);
        return (bi(n) - r + u) / 7;
    }
    function bc(n) {
        return di(n, this._week.dow, this._week.doy).week;
    }
    function kc() {
        return this._week.dow;
    }
    function dc() {
        return this._week.doy;
    }
    function gc(n) {
        var t = this.localeData().week(this);
        return n == null ? t : this.add((n - t) * 7, "d");
    }
    function nl(n) {
        var t = di(this, 1, 4).week;
        return n == null ? t : this.add((n - t) * 7, "d");
    }
    function tl(n, t) {
        return typeof n != "string" ? n : isNaN(n) ? (n = t.weekdaysParse(n), typeof n == "number") ? n : null : parseInt(n, 10);
    }
    function il(n, t) {
        return typeof n == "string" ? t.weekdaysParse(n) % 7 || 7 : isNaN(n) ? null : n;
    }
    function ku(n, t) {
        return n.slice(t, 7).concat(n.slice(0, t));
    }
    function rl(n, t) {
        var i = ft(this._weekdays) ? this._weekdays : this._weekdays[n && n !== !0 && this._weekdays.isFormat.test(t) ? "format" : "standalone"];
        return n === !0 ? ku(i, this._week.dow) : n ? i[n.day()] : i;
    }
    function ul(n) {
        return n === !0 ? ku(this._weekdaysShort, this._week.dow) : n ? this._weekdaysShort[n.day()] : this._weekdaysShort;
    }
    function fl(n) {
        return n === !0 ? ku(this._weekdaysMin, this._week.dow) : n ? this._weekdaysMin[n.day()] : this._weekdaysMin;
    }
    function el(n, t, i) {
        var f, r, e, u = n.toLocaleLowerCase();
        if (!this._weekdaysParse) for (this._weekdaysParse = [], this._shortWeekdaysParse = [],
        this._minWeekdaysParse = [], f = 0; f < 7; ++f) e = et([2e3, 1]).day(f), this._minWeekdaysParse[f] = this.weekdaysMin(e, "").toLocaleLowerCase(),
        this._shortWeekdaysParse[f] = this.weekdaysShort(e, "").toLocaleLowerCase(), this._weekdaysParse[f] = this.weekdays(e, "").toLocaleLowerCase();
        return i ? t === "dddd" ? (r = a.call(this._weekdaysParse, u), r !== -1 ? r : null) : t === "ddd" ? (r = a.call(this._shortWeekdaysParse, u),
        r !== -1 ? r : null) : (r = a.call(this._minWeekdaysParse, u), r !== -1 ? r : null) : t === "dddd" ? (r = a.call(this._weekdaysParse, u),
        r !== -1) ? r : (r = a.call(this._shortWeekdaysParse, u), r !== -1) ? r : (r = a.call(this._minWeekdaysParse, u),
        r !== -1 ? r : null) : t === "ddd" ? (r = a.call(this._shortWeekdaysParse, u), r !== -1) ? r : (r = a.call(this._weekdaysParse, u),
        r !== -1) ? r : (r = a.call(this._minWeekdaysParse, u), r !== -1 ? r : null) : (r = a.call(this._minWeekdaysParse, u),
        r !== -1) ? r : (r = a.call(this._weekdaysParse, u), r !== -1) ? r : (r = a.call(this._shortWeekdaysParse, u),
        r !== -1 ? r : null);
    }
    function ol(n, t, i) {
        var r, u, f;
        if (this._weekdaysParseExact) return el.call(this, n, t, i);
        for (this._weekdaysParse || (this._weekdaysParse = [], this._minWeekdaysParse = [],
        this._shortWeekdaysParse = [], this._fullWeekdaysParse = []), r = 0; r < 7; r++) if ((u = et([2e3, 1]).day(r),
        i && !this._fullWeekdaysParse[r] && (this._fullWeekdaysParse[r] = new RegExp("^" + this.weekdays(u, "").replace(".", "\\.?") + "$", "i"),
        this._shortWeekdaysParse[r] = new RegExp("^" + this.weekdaysShort(u, "").replace(".", "\\.?") + "$", "i"),
        this._minWeekdaysParse[r] = new RegExp("^" + this.weekdaysMin(u, "").replace(".", "\\.?") + "$", "i")),
        this._weekdaysParse[r] || (f = "^" + this.weekdays(u, "") + "|^" + this.weekdaysShort(u, "") + "|^" + this.weekdaysMin(u, ""),
        this._weekdaysParse[r] = new RegExp(f.replace(".", ""), "i")), i && t === "dddd" && this._fullWeekdaysParse[r].test(n)) || i && t === "ddd" && this._shortWeekdaysParse[r].test(n) || i && t === "dd" && this._minWeekdaysParse[r].test(n) || !i && this._weekdaysParse[r].test(n)) return r;
    }
    function sl(n) {
        if (!this.isValid()) return n != null ? this : NaN;
        var t = this._isUTC ? this._d.getUTCDay() : this._d.getDay();
        return n != null ? (n = tl(n, this.localeData()), this.add(n - t, "d")) : t;
    }
    function hl(n) {
        if (!this.isValid()) return n != null ? this : NaN;
        var t = (this.day() + 7 - this.localeData()._week.dow) % 7;
        return n == null ? t : this.add(n - t, "d");
    }
    function cl(n) {
        if (!this.isValid()) return n != null ? this : NaN;
        if (n != null) {
            var t = il(n, this.localeData());
            return this.day(this.day() % 7 ? t : t - 7);
        }
        return this.day() || 7;
    }
    function ll(n) {
        return this._weekdaysParseExact ? (l(this, "_weekdaysRegex") || gu.call(this), n ? this._weekdaysStrictRegex : this._weekdaysRegex) : (l(this, "_weekdaysRegex") || (this._weekdaysRegex = eo),
        this._weekdaysStrictRegex && n ? this._weekdaysStrictRegex : this._weekdaysRegex);
    }
    function al(n) {
        return this._weekdaysParseExact ? (l(this, "_weekdaysRegex") || gu.call(this), n ? this._weekdaysShortStrictRegex : this._weekdaysShortRegex) : (l(this, "_weekdaysShortRegex") || (this._weekdaysShortRegex = oo),
        this._weekdaysShortStrictRegex && n ? this._weekdaysShortStrictRegex : this._weekdaysShortRegex);
    }
    function vl(n) {
        return this._weekdaysParseExact ? (l(this, "_weekdaysRegex") || gu.call(this), n ? this._weekdaysMinStrictRegex : this._weekdaysMinRegex) : (l(this, "_weekdaysMinRegex") || (this._weekdaysMinRegex = so),
        this._weekdaysMinStrictRegex && n ? this._weekdaysMinStrictRegex : this._weekdaysMinRegex);
    }
    function gu() {
        function u(n, t) {
            return t.length - n.length;
        }
        for (var e = [], i = [], r = [], t = [], f, o, s, h, n = 0; n < 7; n++) f = et([2e3, 1]).day(n),
        o = this.weekdaysMin(f, ""), s = this.weekdaysShort(f, ""), h = this.weekdays(f, ""),
        e.push(o), i.push(s), r.push(h), t.push(o), t.push(s), t.push(h);
        for (e.sort(u), i.sort(u), r.sort(u), t.sort(u), n = 0; n < 7; n++) i[n] = gt(i[n]),
        r[n] = gt(r[n]), t[n] = gt(t[n]);
        this._weekdaysRegex = new RegExp("^(" + t.join("|") + ")", "i");
        this._weekdaysShortRegex = this._weekdaysRegex;
        this._weekdaysMinRegex = this._weekdaysRegex;
        this._weekdaysStrictRegex = new RegExp("^(" + r.join("|") + ")", "i");
        this._weekdaysShortStrictRegex = new RegExp("^(" + i.join("|") + ")", "i");
        this._weekdaysMinStrictRegex = new RegExp("^(" + e.join("|") + ")", "i");
    }
    function nf() {
        return this.hours() % 12 || 12;
    }
    function yl() {
        return this.hours() || 24;
    }
    function ho(n, t) {
        r(n, 0, 0, function () {
            return this.localeData().meridiem(this.hours(), this.minutes(), t);
        });
    }
    function co(n, t) {
        return t._meridiemParse;
    }
    function pl(n) {
        return (n + "").toLowerCase().charAt(0) === "p";
    }
    function wl(n, t, i) {
        return n > 11 ? i ? "pm" : "PM" : i ? "am" : "AM";
    }
    function vo(n) {
        return n ? n.toLowerCase().replace("_", "-") : n;
    }
    function kl(n) {
        for (var r = 0, i, t, f, u; r < n.length;) {
            for (u = vo(n[r]).split("-"), i = u.length, t = vo(n[r + 1]), t = t ? t.split("-") : null; i > 0;) {
                if (f = br(u.slice(0, i).join("-")), f) return f;
                if (t && t.length >= i && gf(u, t, !0) >= i - 1) break;
                i--;
            }
            r++;
        }
        return nr;
    }
    function br(n) {
        var t = null, i;
        if (!y[n] && typeof module != "undefined" && module && module.exports) try {
            t = nr._abbr;
            i = require;
            i("./locale/" + n);
            oi(t);
        } catch (r) { }
        return y[n];
    }
    function oi(n, t) {
        var i;
        return n && (i = b(t) ? bt(n) : tf(n, t), i ? nr = i : typeof console != "undefined" && console.warn && console.warn("Locale " + n + " not found. Did you forget to load it?")),
        nr._abbr;
    }
    function tf(n, t) {
        if (t !== null) {
            var r, i = ao;
            if (t.abbr = n, y[n] != null) te("defineLocaleOverride", "use moment.updateLocale(localeName, config) to change an existing locale. moment.defineLocale(localeName, config) should only be used for creating a new locale See http://momentjs.com/guides/#/warnings/define-locale/ for more info."),
            i = y[n]._config; else if (t.parentLocale != null) if (y[t.parentLocale] != null) i = y[t.parentLocale]._config; else if (r = br(t.parentLocale),
            r != null) i = r._config; else return gi[t.parentLocale] || (gi[t.parentLocale] = []),
            gi[t.parentLocale].push({
                name: n,
                config: t
            }), null;
            return y[n] = new su(ie(i, t)), gi[n] && gi[n].forEach(function (n) {
                tf(n.name, n.config);
            }), oi(n), y[n];
        }
        return delete y[n], null;
    }
    function dl(n, t) {
        if (t != null) {
            var i, r, u = ao;
            r = br(n);
            r != null && (u = r._config);
            t = ie(u, t);
            i = new su(t);
            i.parentLocale = y[n];
            y[n] = i;
            oi(n);
        } else y[n] != null && (y[n].parentLocale != null ? y[n] = y[n].parentLocale : y[n] != null && delete y[n]);
        return y[n];
    }
    function bt(n) {
        var t;
        if (n && n._locale && n._locale._abbr && (n = n._locale._abbr), !n) return nr;
        if (!ft(n)) {
            if (t = br(n), t) return t;
            n = [n];
        }
        return kl(n);
    }
    function gl() {
        return re(y);
    }
    function rf(n) {
        var i, t = n._a;
        return t && u(n).overflow === -2 && (i = t[at] < 0 || t[at] > 11 ? at : t[ht] < 1 || t[ht] > pr(t[tt], t[at]) ? ht : t[v] < 0 || t[v] > 24 || t[v] === 24 && (t[it] !== 0 || t[vt] !== 0 || t[ni] !== 0) ? v : t[it] < 0 || t[it] > 59 ? it : t[vt] < 0 || t[vt] > 59 ? vt : t[ni] < 0 || t[ni] > 999 ? ni : -1,
        u(n)._overflowDayOfYear && (i < tt || i > ht) && (i = ht), u(n)._overflowWeeks && i === -1 && (i = rc),
        u(n)._overflowWeekday && i === -1 && (i = uc), u(n).overflow = i), n;
    }
    function si(n, t, i) {
        return n != null ? n : t != null ? t : i;
    }
    function na(n) {
        var i = new Date(t.now());
        return n._useUTC ? [i.getUTCFullYear(), i.getUTCMonth(), i.getUTCDate()] : [i.getFullYear(), i.getMonth(), i.getDate()];
    }
    function uf(n) {
        var t, i, r = [], f, o, e;
        if (!n._d) {
            for (f = na(n), n._w && n._a[ht] == null && n._a[at] == null && ta(n), n._dayOfYear != null && (e = si(n._a[tt], f[tt]),
            (n._dayOfYear > bi(e) || n._dayOfYear === 0) && (u(n)._overflowDayOfYear = !0),
            i = ki(e, 0, n._dayOfYear), n._a[at] = i.getUTCMonth(), n._a[ht] = i.getUTCDate()),
            t = 0; t < 3 && n._a[t] == null; ++t) n._a[t] = r[t] = f[t];
            for (; t < 7; t++) n._a[t] = r[t] = n._a[t] == null ? t === 2 ? 1 : 0 : n._a[t];
            n._a[v] === 24 && n._a[it] === 0 && n._a[vt] === 0 && n._a[ni] === 0 && (n._nextDay = !0,
            n._a[v] = 0);
            n._d = (n._useUTC ? ki : wc).apply(null, r);
            o = n._useUTC ? n._d.getUTCDay() : n._d.getDay();
            n._tzm != null && n._d.setUTCMinutes(n._d.getUTCMinutes() - n._tzm);
            n._nextDay && (n._a[v] = 24);
            n._w && typeof n._w.d != "undefined" && n._w.d !== o && (u(n).weekdayMismatch = !0);
        }
    }
    function ta(n) {
        var t, o, f, i, r, e, h, s, l;
        t = n._w;
        t.GG != null || t.W != null || t.E != null ? (r = 1, e = 4, o = si(t.GG, n._a[tt], di(c(), 1, 4).year),
        f = si(t.W, 1), i = si(t.E, 1), (i < 1 || i > 7) && (s = !0)) : (r = n._locale._week.dow,
        e = n._locale._week.doy, l = di(c(), r, e), o = si(t.gg, n._a[tt], l.year), f = si(t.w, l.week),
        t.d != null ? (i = t.d, (i < 0 || i > 6) && (s = !0)) : t.e != null ? (i = t.e + r,
        (t.e < 0 || t.e > 6) && (s = !0)) : i = r);
        f < 1 || f > ti(o, r, e) ? u(n)._overflowWeeks = !0 : s != null ? u(n)._overflowWeekday = !0 : (h = io(o, f, i, r, e),
        n._a[tt] = h.year, n._dayOfYear = h.dayOfYear);
    }
    function yo(n) {
        var t, r, o = n._i, i = ia.exec(o) || ra.exec(o), s, e, f, h;
        if (i) {
            for (u(n).iso = !0, t = 0, r = kr.length; t < r; t++) if (kr[t][1].exec(i[1])) {
                e = kr[t][0];
                s = kr[t][2] !== !1;
                break;
            }
            if (e == null) {
                n._isValid = !1;
                return;
            }
            if (i[3]) {
                for (t = 0, r = ff.length; t < r; t++) if (ff[t][1].exec(i[3])) {
                    f = (i[2] || " ") + ff[t][0];
                    break;
                }
                if (f == null) {
                    n._isValid = !1;
                    return;
                }
            }
            if (!s && f != null) {
                n._isValid = !1;
                return;
            }
            if (i[4]) if (ua.exec(i[4])) h = "Z"; else {
                n._isValid = !1;
                return;
            }
            n._f = e + (f || "") + (h || "");
            ef(n);
        } else n._isValid = !1;
    }
    function ea(n, t, i, r, u, f) {
        var e = [oa(n), bu.indexOf(t), parseInt(i, 10), parseInt(r, 10), parseInt(u, 10)];
        return f && e.push(parseInt(f, 10)), e;
    }
    function oa(n) {
        var t = parseInt(n, 10);
        return t <= 49 ? 2e3 + t : t <= 999 ? 1900 + t : t;
    }
    function sa(n) {
        return n.replace(/\([^)]*\)|[\n\t]/g, " ").replace(/(\s\s+)/g, " ").replace(/^\s\s*/, "").replace(/\s\s*$/, "");
    }
    function ha(n, t, i) {
        if (n) {
            var r = du.indexOf(n), f = new Date(t[0], t[1], t[2]).getDay();
            if (r !== f) return u(i).weekdayMismatch = !0, i._isValid = !1, !1;
        }
        return !0;
    }
    function ca(n, t, i) {
        if (n) return wo[n];
        if (t) return 0;
        var r = parseInt(i, 10), u = r % 100, f = (r - u) / 100;
        return f * 60 + u;
    }
    function bo(n) {
        var t = po.exec(sa(n._i)), i;
        if (t) {
            if (i = ea(t[4], t[3], t[2], t[5], t[6], t[7]), !ha(t[1], i, n)) return;
            n._a = i;
            n._tzm = ca(t[8], t[9], t[10]);
            n._d = ki.apply(null, n._a);
            n._d.setUTCMinutes(n._d.getUTCMinutes() - n._tzm);
            u(n).rfc2822 = !0;
        } else n._isValid = !1;
    }
    function la(n) {
        var i = fa.exec(n._i);
        if (i !== null) {
            n._d = new Date(+i[1]);
            return;
        }
        if (yo(n), n._isValid === !1) delete n._isValid; else return;
        if (bo(n), n._isValid === !1) delete n._isValid; else return;
        t.createFromInputFallback(n);
    }
    function ef(n) {
        if (n._f === t.ISO_8601) {
            yo(n);
            return;
        }
        if (n._f === t.RFC_2822) {
            bo(n);
            return;
        }
        n._a = [];
        u(n).empty = !0;
        for (var i = "" + n._i, r, f, s, c = i.length, h = 0, o = le(n._f, n._locale).match(ce) || [], e = 0; e < o.length; e++) f = o[e],
        r = (i.match(nc(f, n)) || [])[0], r && (s = i.substr(0, i.indexOf(r)), s.length > 0 && u(n).unusedInput.push(s),
        i = i.slice(i.indexOf(r) + r.length), h += r.length), fi[f] ? (r ? u(n).empty = !1 : u(n).unusedTokens.push(f),
        ic(f, r, n)) : n._strict && !r && u(n).unusedTokens.push(f);
        u(n).charsLeftOver = c - h;
        i.length > 0 && u(n).unusedInput.push(i);
        n._a[v] <= 12 && u(n).bigHour === !0 && n._a[v] > 0 && (u(n).bigHour = undefined);
        u(n).parsedDateParts = n._a.slice(0);
        u(n).meridiem = n._meridiem;
        n._a[v] = aa(n._locale, n._a[v], n._meridiem);
        uf(n);
        rf(n);
    }
    function aa(n, t, i) {
        var r;
        return i == null ? t : n.meridiemHour != null ? n.meridiemHour(t, i) : n.isPM != null ? (r = n.isPM(i),
        r && t < 12 && (t += 12), r || t !== 12 || (t = 0), t) : t;
    }
    function va(n) {
        var t, e, f, r, i;
        if (n._f.length === 0) {
            u(n).invalidFormat = !0;
            n._d = new Date(NaN);
            return;
        }
        for (r = 0; r < n._f.length; r++) (i = 0, t = eu({}, n), n._useUTC != null && (t._useUTC = n._useUTC),
        t._f = n._f[r], ef(t), fu(t)) && (i += u(t).charsLeftOver, i += u(t).unusedTokens.length * 10,
        u(t).score = i, (f == null || i < f) && (f = i, e = t));
        wt(n, e || t);
    }
    function ya(n) {
        if (!n._d) {
            var t = hu(n._i);
            n._a = kf([t.year, t.month, t.day || t.date, t.hour, t.minute, t.second, t.millisecond], function (n) {
                return n && parseInt(n, 10);
            });
            uf(n);
        }
    }
    function pa(n) {
        var t = new yi(rf(ko(n)));
        return t._nextDay && (t.add(1, "d"), t._nextDay = undefined), t;
    }
    function ko(n) {
        var t = n._i, i = n._f;
        return (n._locale = n._locale || bt(n._l), t === null || i === undefined && t === "") ? ir({
            nullInput: !0
        }) : (typeof t == "string" && (n._i = t = n._locale.preparse(t)), ot(t)) ? new yi(rf(t)) : (tr(t) ? n._d = t : ft(i) ? va(n) : i ? ef(n) : wa(n),
        fu(n) || (n._d = null), n);
    }
    function wa(n) {
        var i = n._i;
        b(i) ? n._d = new Date(t.now()) : tr(i) ? n._d = new Date(i.valueOf()) : typeof i == "string" ? la(n) : ft(i) ? (n._a = kf(i.slice(0), function (n) {
            return parseInt(n, 10);
        }), uf(n)) : vi(i) ? ya(n) : dt(i) ? n._d = new Date(i) : t.createFromInputFallback(n);
    }
    function go(n, t, i, r, u) {
        var f = {};
        return (i === !0 || i === !1) && (r = i, i = undefined), (vi(n) && sh(n) || ft(n) && n.length === 0) && (n = undefined),
        f._isAMomentObject = !0, f._useUTC = f._isUTC = u, f._l = i, f._i = n, f._f = t,
        f._strict = r, pa(f);
    }
    function c(n, t, i, r) {
        return go(n, t, i, r, !1);
    }
    function is(n, t) {
        var r, i;
        if (t.length === 1 && ft(t[0]) && (t = t[0]), !t.length) return c();
        for (r = t[0], i = 1; i < t.length; ++i) (!t[i].isValid() || t[i][n](r)) && (r = t[i]);
        return r;
    }
    function ba() {
        var n = [].slice.call(arguments, 0);
        return is("isBefore", n);
    }
    function ka() {
        var n = [].slice.call(arguments, 0);
        return is("isAfter", n);
    }
    function da(n) {
        var i, r, t;
        for (i in n) if (!(a.call(hi, i) !== -1 && (n[i] == null || !isNaN(n[i])))) return !1;
        for (r = !1, t = 0; t < hi.length; ++t) if (n[hi[t]]) {
            if (r) return !1;
            parseFloat(n[hi[t]]) !== f(n[hi[t]]) && (r = !0);
        }
        return !0;
    }
    function ga() {
        return this._isValid;
    }
    function nv() {
        return rt(NaN);
    }
    function dr(n) {
        var t = hu(n), i = t.year || 0, r = t.quarter || 0, u = t.month || 0, f = t.week || t.isoWeek || 0, e = t.day || 0, o = t.hour || 0, s = t.minute || 0, h = t.second || 0, c = t.millisecond || 0;
        this._isValid = da(t);
        this._milliseconds = +c + h * 1e3 + s * 6e4 + o * 36e5;
        this._days = +e + f * 7;
        this._months = +u + r * 3 + i * 12;
        this._data = {};
        this._locale = bt();
        this._bubble();
    }
    function of(n) {
        return n instanceof dr;
    }
    function sf(n) {
        return n < 0 ? Math.round(-1 * n) * -1 : Math.round(n);
    }
    function us(n, t) {
        r(n, 0, 0, function () {
            var n = this.utcOffset(), i = "+";
            return n < 0 && (n = -n, i = "-"), i + lt(~~(n / 60), 2) + t + lt(~~n % 60, 2);
        });
    }
    function hf(n, t) {
        var i = (t || "").match(n);
        if (i === null) return null;
        var e = i[i.length - 1] || [], r = (e + "").match(fs) || ["-", 0, 0], u = +(r[1] * 60) + f(r[2]);
        return u === 0 ? 0 : r[0] === "+" ? u : -u;
    }
    function cf(n, i) {
        var r, u;
        return i._isUTC ? (r = i.clone(), u = (ot(n) || tr(n) ? n.valueOf() : c(n).valueOf()) - r.valueOf(),
        r._d.setTime(r._d.valueOf() + u), t.updateOffset(r, !1), r) : c(n).local();
    }
    function lf(n) {
        return -Math.round(n._d.getTimezoneOffset() / 15) * 15;
    }
    function tv(n, i, r) {
        var u = this._offset || 0, f;
        if (!this.isValid()) return n != null ? this : NaN;
        if (n != null) {
            if (typeof n == "string") {
                if (n = hf(lr, n), n === null) return this;
            } else Math.abs(n) < 16 && !r && (n = n * 60);
            return !this._isUTC && i && (f = lf(this)), this._offset = n, this._isUTC = !0,
            f != null && this.add(f, "m"), u !== n && (!i || this._changeInProgress ? ls(this, rt(n - u, "m"), 1, !1) : this._changeInProgress || (this._changeInProgress = !0,
            t.updateOffset(this, !0), this._changeInProgress = null)), this;
        }
        return this._isUTC ? u : lf(this);
    }
    function iv(n, t) {
        return n != null ? (typeof n != "string" && (n = -n), this.utcOffset(n, t), this) : -this.utcOffset();
    }
    function rv(n) {
        return this.utcOffset(0, n);
    }
    function uv(n) {
        return this._isUTC && (this.utcOffset(0, n), this._isUTC = !1, n && this.subtract(lf(this), "m")),
        this;
    }
    function fv() {
        if (this._tzm != null) this.utcOffset(this._tzm, !1, !0); else if (typeof this._i == "string") {
            var n = hf(gh, this._i);
            n != null ? this.utcOffset(n) : this.utcOffset(0, !0);
        }
        return this;
    }
    function ev(n) {
        return this.isValid() ? (n = n ? c(n).utcOffset() : 0, (this.utcOffset() - n) % 60 == 0) : !1;
    }
    function ov() {
        return this.utcOffset() > this.clone().month(0).utcOffset() || this.utcOffset() > this.clone().month(5).utcOffset();
    }
    function sv() {
        var n, t;
        return b(this._isDSTShifted) ? (n = {}, eu(n, this), n = ko(n), n._a ? (t = n._isUTC ? et(n._a) : c(n._a),
        this._isDSTShifted = this.isValid() && gf(n._a, t.toArray()) > 0) : this._isDSTShifted = !1,
        this._isDSTShifted) : this._isDSTShifted;
    }
    function hv() {
        return this.isValid() ? !this._isUTC : !1;
    }
    function cv() {
        return this.isValid() ? this._isUTC : !1;
    }
    function es() {
        return this.isValid() ? this._isUTC && this._offset === 0 : !1;
    }
    function rt(n, t) {
        var i = n, r = null, u, e, o;
        return of(n) ? i = {
            ms: n._milliseconds,
            d: n._days,
            M: n._months
        } : dt(n) ? (i = {}, t ? i[t] = n : i.milliseconds = n) : (r = os.exec(n)) ? (u = r[1] === "-" ? -1 : 1,
        i = {
            y: 0,
            d: f(r[ht]) * u,
            h: f(r[v]) * u,
            m: f(r[it]) * u,
            s: f(r[vt]) * u,
            ms: f(sf(r[ni] * 1e3)) * u
        }) : (r = ss.exec(n)) ? (u = r[1] === "-" ? -1 : 1, i = {
            y: ii(r[2], u),
            M: ii(r[3], u),
            w: ii(r[4], u),
            d: ii(r[5], u),
            h: ii(r[6], u),
            m: ii(r[7], u),
            s: ii(r[8], u)
        }) : i == null ? i = {} : typeof i == "object" && ("from" in i || "to" in i) && (o = lv(c(i.from), c(i.to)),
        i = {}, i.ms = o.milliseconds, i.M = o.months), e = new dr(i), of(n) && l(n, "_locale") && (e._locale = n._locale),
        e;
    }
    function ii(n, t) {
        var i = n && parseFloat(n.replace(",", "."));
        return (isNaN(i) ? 0 : i) * t;
    }
    function hs(n, t) {
        var i = {};
        return i.months = t.month() - n.month() + (t.year() - n.year()) * 12, n.clone().add(i.months, "M").isAfter(t) && --i.months,
        i.milliseconds = +t - +n.clone().add(i.months, "M"), i;
    }
    function lv(n, t) {
        var i;
        return n.isValid() && t.isValid() ? (t = cf(t, n), n.isBefore(t) ? i = hs(n, t) : (i = hs(t, n),
        i.milliseconds = -i.milliseconds, i.months = -i.months), i) : {
            milliseconds: 0,
            months: 0
        };
    }
    function cs(n, t) {
        return function (i, r) {
            var u, f;
            return r === null || isNaN(+r) || (te(t, "moment()." + t + "(period, number) is deprecated. Please use moment()." + t + "(number, period). See http://momentjs.com/guides/#/warnings/add-inverted-param/ for more info."),
            f = i, i = r, r = f), i = typeof i == "string" ? +i : i, u = rt(i, r), ls(this, u, n),
            this;
        };
    }
    function ls(n, i, r, u) {
        var o = i._milliseconds, f = sf(i._days), e = sf(i._months);
        n.isValid() && (u = u == null ? !0 : u, e && ke(n, yr(n, "Month") + e * r), f && we(n, "Date", yr(n, "Date") + f * r),
        o && n._d.setTime(n._d.valueOf() + o * r), u && t.updateOffset(n, f || e));
    }
    function av(n, t) {
        var i = n.diff(t, "days", !0);
        return i < -6 ? "sameElse" : i < -1 ? "lastWeek" : i < 0 ? "lastDay" : i < 1 ? "sameDay" : i < 2 ? "nextDay" : i < 7 ? "nextWeek" : "sameElse";
    }
    function vv(n, i) {
        var u = n || c(), f = cf(u, this).startOf("day"), r = t.calendarFormat(this, f) || "sameElse", e = i && (st(i[r]) ? i[r].call(this, u) : i[r]);
        return this.format(e || this.localeData().calendar(r, this, c(u)));
    }
    function yv() {
        return new yi(this);
    }
    function pv(n, t) {
        var i = ot(n) ? n : c(n);
        return this.isValid() && i.isValid() ? (t = nt(t) || "millisecond", t === "millisecond" ? this.valueOf() > i.valueOf() : i.valueOf() < this.clone().startOf(t).valueOf()) : !1;
    }
    function wv(n, t) {
        var i = ot(n) ? n : c(n);
        return this.isValid() && i.isValid() ? (t = nt(t) || "millisecond", t === "millisecond" ? this.valueOf() < i.valueOf() : this.clone().endOf(t).valueOf() < i.valueOf()) : !1;
    }
    function bv(n, t, i, r) {
        var u = ot(n) ? n : c(n), f = ot(t) ? t : c(t);
        return this.isValid() && u.isValid() && f.isValid() ? (r = r || "()", (r[0] === "(" ? this.isAfter(u, i) : !this.isBefore(u, i)) && (r[1] === ")" ? this.isBefore(f, i) : !this.isAfter(f, i))) : !1;
    }
    function kv(n, t) {
        var i = ot(n) ? n : c(n), r;
        return this.isValid() && i.isValid() ? (t = nt(t) || "millisecond", t === "millisecond" ? this.valueOf() === i.valueOf() : (r = i.valueOf(),
        this.clone().startOf(t).valueOf() <= r && r <= this.clone().endOf(t).valueOf())) : !1;
    }
    function dv(n, t) {
        return this.isSame(n, t) || this.isAfter(n, t);
    }
    function gv(n, t) {
        return this.isSame(n, t) || this.isBefore(n, t);
    }
    function ny(n, t, i) {
        var r, f, u;
        if (!this.isValid()) return NaN;
        if (r = cf(n, this), !r.isValid()) return NaN;
        f = (r.utcOffset() - this.utcOffset()) * 6e4;
        t = nt(t);
        switch (t) {
            case "year":
                u = af(this, r) / 12;
                break;

            case "month":
                u = af(this, r);
                break;

            case "quarter":
                u = af(this, r) / 3;
                break;

            case "second":
                u = (this - r) / 1e3;
                break;

            case "minute":
                u = (this - r) / 6e4;
                break;

            case "hour":
                u = (this - r) / 36e5;
                break;

            case "day":
                u = (this - r - f) / 864e5;
                break;

            case "week":
                u = (this - r - f) / 6048e5;
                break;

            default:
                u = this - r;
        }
        return i ? u : d(u);
    }
    function af(n, t) {
        var r = (t.year() - n.year()) * 12 + (t.month() - n.month()), i = n.clone().add(r, "months"), u, f;
        return t - i < 0 ? (u = n.clone().add(r - 1, "months"), f = (t - i) / (i - u)) : (u = n.clone().add(r + 1, "months"),
        f = (t - i) / (u - i)), -(r + f) || 0;
    }
    function ty() {
        return this.clone().locale("en").format("ddd MMM DD YYYY HH:mm:ss [GMT]ZZ");
    }
    function iy(n) {
        if (!this.isValid()) return null;
        var i = n !== !0, t = i ? this.clone().utc() : this;
        return t.year() < 0 || t.year() > 9999 ? er(t, i ? "YYYYYY-MM-DD[T]HH:mm:ss.SSS[Z]" : "YYYYYY-MM-DD[T]HH:mm:ss.SSSZ") : st(Date.prototype.toISOString) ? i ? this.toDate().toISOString() : new Date(this.valueOf() + this.utcOffset() * 6e4).toISOString().replace("Z", er(t, "Z")) : er(t, i ? "YYYY-MM-DD[T]HH:mm:ss.SSS[Z]" : "YYYY-MM-DD[T]HH:mm:ss.SSSZ");
    }
    function ry() {
        var n, t;
        if (!this.isValid()) return "moment.invalid(/* " + this._i + " */)";
        n = "moment";
        t = "";
        this.isLocal() || (n = this.utcOffset() === 0 ? "moment.utc" : "moment.parseZone",
        t = "Z");
        var i = "[" + n + '("]', r = 0 <= this.year() && this.year() <= 9999 ? "YYYY" : "YYYYYY", u = t + '[")]';
        return this.format(i + r + "-MM-DD[T]HH:mm:ss.SSS" + u);
    }
    function uy(n) {
        n || (n = this.isUtc() ? t.defaultFormatUtc : t.defaultFormat);
        var i = er(this, n);
        return this.localeData().postformat(i);
    }
    function fy(n, t) {
        return this.isValid() && (ot(n) && n.isValid() || c(n).isValid()) ? rt({
            to: this,
            from: n
        }).locale(this.locale()).humanize(!t) : this.localeData().invalidDate();
    }
    function ey(n) {
        return this.from(c(), n);
    }
    function oy(n, t) {
        return this.isValid() && (ot(n) && n.isValid() || c(n).isValid()) ? rt({
            from: this,
            to: n
        }).locale(this.locale()).humanize(!t) : this.localeData().invalidDate();
    }
    function sy(n) {
        return this.to(c(), n);
    }
    function ys(n) {
        var t;
        return n === undefined ? this._locale._abbr : (t = bt(n), t != null && (this._locale = t),
        this);
    }
    function ps() {
        return this._locale;
    }
    function li(n, t) {
        return (n % t + t) % t;
    }
    function bs(n, t, i) {
        return n < 100 && n >= 0 ? new Date(n + 400, t, i) - ws : new Date(n, t, i).valueOf();
    }
    function ks(n, t, i) {
        return n < 100 && n >= 0 ? Date.UTC(n + 400, t, i) - ws : Date.UTC(n, t, i);
    }
    function hy(n) {
        var i, r;
        if (n = nt(n), n === undefined || n === "millisecond" || !this.isValid()) return this;
        r = this._isUTC ? ks : bs;
        switch (n) {
            case "year":
                i = r(this.year(), 0, 1);
                break;

            case "quarter":
                i = r(this.year(), this.month() - this.month() % 3, 1);
                break;

            case "month":
                i = r(this.year(), this.month(), 1);
                break;

            case "week":
                i = r(this.year(), this.month(), this.date() - this.weekday());
                break;

            case "isoWeek":
                i = r(this.year(), this.month(), this.date() - (this.isoWeekday() - 1));
                break;

            case "day":
            case "date":
                i = r(this.year(), this.month(), this.date());
                break;

            case "hour":
                i = this._d.valueOf() - li(i + (this._isUTC ? 0 : this.utcOffset() * ci), nu);
                break;

            case "minute":
                i = this._d.valueOf() - li(i, ci);
                break;

            case "second":
                i = this._d.valueOf() - li(i, gr);
        }
        return this._d.setTime(i), t.updateOffset(this, !0), this;
    }
    function cy(n) {
        var i, r;
        if (n = nt(n), n === undefined || n === "millisecond" || !this.isValid()) return this;
        r = this._isUTC ? ks : bs;
        switch (n) {
            case "year":
                i = r(this.year() + 1, 0, 1) - 1;
                break;

            case "quarter":
                i = r(this.year(), this.month() - this.month() % 3 + 3, 1) - 1;
                break;

            case "month":
                i = r(this.year(), this.month() + 1, 1) - 1;
                break;

            case "week":
                i = r(this.year(), this.month(), this.date() - this.weekday() + 7) - 1;
                break;

            case "isoWeek":
                i = r(this.year(), this.month(), this.date() - (this.isoWeekday() - 1) + 7) - 1;
                break;

            case "day":
            case "date":
                i = r(this.year(), this.month(), this.date() + 1) - 1;
                break;

            case "hour":
                i = this._d.valueOf() + (nu - li(i + (this._isUTC ? 0 : this.utcOffset() * ci), nu) - 1);
                break;

            case "minute":
                i = this._d.valueOf() + (ci - li(i, ci) - 1);
                break;

            case "second":
                i = this._d.valueOf() + (gr - li(i, gr) - 1);
        }
        return this._d.setTime(i), t.updateOffset(this, !0), this;
    }
    function ly() {
        return this._d.valueOf() - (this._offset || 0) * 6e4;
    }
    function ay() {
        return Math.floor(this.valueOf() / 1e3);
    }
    function vy() {
        return new Date(this.valueOf());
    }
    function yy() {
        var n = this;
        return [n.year(), n.month(), n.date(), n.hour(), n.minute(), n.second(), n.millisecond()];
    }
    function py() {
        var n = this;
        return {
            years: n.year(),
            months: n.month(),
            date: n.date(),
            hours: n.hours(),
            minutes: n.minutes(),
            seconds: n.seconds(),
            milliseconds: n.milliseconds()
        };
    }
    function wy() {
        return this.isValid() ? this.toISOString() : null;
    }
    function by() {
        return fu(this);
    }
    function ky() {
        return wt({}, u(this));
    }
    function dy() {
        return u(this).overflow;
    }
    function gy() {
        return {
            input: this._i,
            format: this._f,
            locale: this._locale,
            isUTC: this._isUTC,
            strict: this._strict
        };
    }
    function tu(n, t) {
        r(0, [n, n.length], 0, t);
    }
    function np(n) {
        return ds.call(this, n, this.week(), this.weekday(), this.localeData()._week.dow, this.localeData()._week.doy);
    }
    function tp(n) {
        return ds.call(this, n, this.isoWeek(), this.isoWeekday(), 1, 4);
    }
    function ip() {
        return ti(this.year(), 1, 4);
    }
    function rp() {
        var n = this.localeData()._week;
        return ti(this.year(), n.dow, n.doy);
    }
    function ds(n, t, i, r, u) {
        var f;
        return n == null ? di(this, r, u).year : (f = ti(n, r, u), t > f && (t = f), up.call(this, n, t, i, r, u));
    }
    function up(n, t, i, r, u) {
        var e = io(n, t, i, r, u), f = ki(e.year, 0, e.dayOfYear);
        return this.year(f.getUTCFullYear()), this.month(f.getUTCMonth()), this.date(f.getUTCDate()),
        this;
    }
    function fp(n) {
        return n == null ? Math.ceil((this.month() + 1) / 3) : this.month((n - 1) * 3 + this.month() % 3);
    }
    function ep(n) {
        var t = Math.round((this.clone().startOf("day") - this.clone().startOf("year")) / 864e5) + 1;
        return n == null ? t : this.add(n - t, "d");
    }
    function op(n, t) {
        t[ni] = f(("0." + n) * 1e3);
    }
    function sp() {
        return this._isUTC ? "UTC" : "";
    }
    function hp() {
        return this._isUTC ? "Coordinated Universal Time" : "";
    }
    function cp(n) {
        return c(n * 1e3);
    }
    function lp() {
        return c.apply(null, arguments).parseZone();
    }
    function ih(n) {
        return n;
    }
    function iu(n, t, i, r) {
        var u = bt(), f = et().set(r, t);
        return u[i](f, n);
    }
    function rh(n, t, i) {
        if (dt(n) && (t = n, n = undefined), n = n || "", t != null) return iu(n, t, i, "month");
        for (var u = [], r = 0; r < 12; r++) u[r] = iu(n, r, i, "month");
        return u;
    }
    function pf(n, t, i, r) {
        var o, f, u, e;
        if (typeof n == "boolean" ? (dt(t) && (i = t, t = undefined), t = t || "") : (t = n,
        i = t, n = !1, dt(t) && (i = t, t = undefined), t = t || ""), o = bt(), f = n ? o._week.dow : 0,
        i != null) return iu(t, (i + f) % 7, r, "day");
        for (e = [], u = 0; u < 7; u++) e[u] = iu(t, (u + f) % 7, r, "day");
        return e;
    }
    function ap(n, t) {
        return rh(n, t, "months");
    }
    function vp(n, t) {
        return rh(n, t, "monthsShort");
    }
    function yp(n, t, i) {
        return pf(n, t, i, "weekdays");
    }
    function pp(n, t, i) {
        return pf(n, t, i, "weekdaysShort");
    }
    function wp(n, t, i) {
        return pf(n, t, i, "weekdaysMin");
    }
    function bp() {
        var n = this._data;
        return this._milliseconds = ct(this._milliseconds), this._days = ct(this._days),
        this._months = ct(this._months), n.milliseconds = ct(n.milliseconds), n.seconds = ct(n.seconds),
        n.minutes = ct(n.minutes), n.hours = ct(n.hours), n.months = ct(n.months), n.years = ct(n.years),
        this;
    }
    function uh(n, t, i, r) {
        var u = rt(t, i);
        return n._milliseconds += r * u._milliseconds, n._days += r * u._days, n._months += r * u._months,
        n._bubble();
    }
    function kp(n, t) {
        return uh(this, n, t, 1);
    }
    function dp(n, t) {
        return uh(this, n, t, -1);
    }
    function fh(n) {
        return n < 0 ? Math.floor(n) : Math.ceil(n);
    }
    function gp() {
        var r = this._milliseconds, n = this._days, t = this._months, i = this._data, u, f, e, s, o;
        return r >= 0 && n >= 0 && t >= 0 || r <= 0 && n <= 0 && t <= 0 || (r += fh(wf(t) + n) * 864e5,
        n = 0, t = 0), i.milliseconds = r % 1e3, u = d(r / 1e3), i.seconds = u % 60, f = d(u / 60),
        i.minutes = f % 60, e = d(f / 60), i.hours = e % 24, n += d(e / 24), o = d(eh(n)),
        t += o, n -= fh(wf(o)), s = d(t / 12), t %= 12, i.days = n, i.months = t, i.years = s,
        this;
    }
    function eh(n) {
        return n * 4800 / 146097;
    }
    function wf(n) {
        return n * 146097 / 4800;
    }
    function nw(n) {
        if (!this.isValid()) return NaN;
        var t, r, i = this._milliseconds;
        if (n = nt(n), n === "month" || n === "quarter" || n === "year") {
            t = this._days + i / 864e5;
            r = this._months + eh(t);
            switch (n) {
                case "month":
                    return r;

                case "quarter":
                    return r / 3;

                case "year":
                    return r / 12;
            }
        } else {
            t = this._days + Math.round(wf(this._months));
            switch (n) {
                case "week":
                    return t / 7 + i / 6048e5;

                case "day":
                    return t + i / 864e5;

                case "hour":
                    return t * 24 + i / 36e5;

                case "minute":
                    return t * 1440 + i / 6e4;

                case "second":
                    return t * 86400 + i / 1e3;

                case "millisecond":
                    return Math.floor(t * 864e5) + i;

                default:
                    throw new Error("Unknown unit " + n);
            }
        }
    }
    function tw() {
        return this.isValid() ? this._milliseconds + this._days * 864e5 + this._months % 12 * 2592e6 + f(this._months / 12) * 31536e6 : NaN;
    }
    function yt(n) {
        return function () {
            return this.as(n);
        };
    }
    function lw() {
        return rt(this);
    }
    function aw(n) {
        return n = nt(n), this.isValid() ? this[n + "s"]() : NaN;
    }
    function ri(n) {
        return function () {
            return this.isValid() ? this._data[n] : NaN;
        };
    }
    function gw() {
        return d(this.days() / 7);
    }
    function nb(n, t, i, r, u) {
        return u.relativeTime(t || 1, !!i, n, r);
    }
    function tb(n, t, i) {
        var r = rt(n).abs(), u = pt(r.as("s")), e = pt(r.as("m")), o = pt(r.as("h")), s = pt(r.as("d")), h = pt(r.as("M")), c = pt(r.as("y")), f = u <= ut.ss && ["s", u] || u < ut.s && ["ss", u] || e <= 1 && ["m"] || e < ut.m && ["mm", e] || o <= 1 && ["h"] || o < ut.h && ["hh", o] || s <= 1 && ["d"] || s < ut.d && ["dd", s] || h <= 1 && ["M"] || h < ut.M && ["MM", h] || c <= 1 && ["y"] || ["yy", c];
        return f[2] = t, f[3] = +n > 0, f[4] = i, nb.apply(null, f);
    }
    function ib(n) {
        return n === undefined ? pt : typeof n == "function" ? (pt = n, !0) : !1;
    }
    function rb(n, t) {
        return ut[n] === undefined ? !1 : t === undefined ? ut[n] : (ut[n] = t, n === "s" && (ut.ss = t - 1),
        !0);
    }
    function ub(n) {
        if (!this.isValid()) return this.localeData().invalidDate();
        var t = this.localeData(), i = tb(this, !n, t);
        return n && (i = t.pastFuture(+this, i)), t.postformat(i);
    }
    function ai(n) {
        return (n > 0) - (n < 0) || +n;
    }
    function uu() {
        if (!this.isValid()) return this.localeData().invalidDate();
        var t = ru(this._milliseconds) / 1e3, y = ru(this._days), r = ru(this._months), i, s, h;
        i = d(t / 60);
        s = d(i / 60);
        t %= 60;
        i %= 60;
        h = d(r / 12);
        r %= 12;
        var c = h, l = r, a = y, u = s, f = i, e = t ? t.toFixed(3).replace(/\.?0+$/, "") : "", n = this.asSeconds();
        if (!n) return "P0D";
        var p = n < 0 ? "-" : "", v = ai(this._months) !== ai(n) ? "-" : "", w = ai(this._days) !== ai(n) ? "-" : "", o = ai(this._milliseconds) !== ai(n) ? "-" : "";
        return p + "P" + (c ? v + c + "Y" : "") + (l ? v + l + "M" : "") + (a ? w + a + "D" : "") + (u || f || e ? "T" : "") + (u ? o + u + "H" : "") + (f ? o + f + "M" : "") + (e ? o + e + "S" : "");
    }
    var bf, df, rr, ur, ou, re, ue, fe, ee, oe, se, he, ui, cu, ar, pu, a, wu, be, bu, ge, no, ro, uo, du, fo, eo, oo, so, lo, po, wo, ns, ts, rs, hi, fs, os, ss, as, vs, vf, yf, gs, nh, kt, th, n, o, ct, pt, ut, ru, e;
    df = Array.prototype.some ? Array.prototype.some : function (n) {
        for (var i = Object(this), r = i.length >>> 0, t = 0; t < r; t++) if (t in i && n.call(this, i[t], t, i)) return !0;
        return !1;
    };
    rr = t.momentProperties = [];
    ur = !1;
    ou = {};
    t.suppressDeprecationWarnings = !1;
    t.deprecationHandler = null;
    re = Object.keys ? Object.keys : function (n) {
        var t, i = [];
        for (t in n) l(n, t) && i.push(t);
        return i;
    };
    ue = {
        sameDay: "[Today at] LT",
        nextDay: "[Tomorrow at] LT",
        nextWeek: "dddd [at] LT",
        lastDay: "[Yesterday at] LT",
        lastWeek: "[Last] dddd [at] LT",
        sameElse: "L"
    };
    fe = {
        LTS: "h:mm:ss A",
        LT: "h:mm A",
        L: "MM/DD/YYYY",
        LL: "MMMM D, YYYY",
        LLL: "MMMM D, YYYY h:mm A",
        LLLL: "dddd, MMMM D, YYYY h:mm A"
    };
    ee = "Invalid date";
    oe = "%d";
    se = /\d{1,2}/;
    he = {
        future: "in %s",
        past: "%s ago",
        s: "a few seconds",
        ss: "%d seconds",
        m: "a minute",
        mm: "%d minutes",
        h: "an hour",
        hh: "%d hours",
        d: "a day",
        dd: "%d days",
        M: "a month",
        MM: "%d months",
        y: "a year",
        yy: "%d years"
    };
    ui = {};
    cu = {};
    var ce = /(\[[^\[]*\])|(\\)?([Hh]mm(ss)?|Mo|MM?M?M?|Do|DDDo|DD?D?D?|ddd?d?|do?|w[o|w]?|W[o|W]?|Qo?|YYYYYY|YYYYY|YYYY|YY|gg(ggg?)?|GG(GGG?)?|e|E|a|A|hh?|HH?|kk?|mm?|ss?|S{1,9}|x|X|zz?|ZZ?|.)/g, fr = /(\[[^\[]*\])|(\\)?(LTS|LT|LL?L?L?|l{1,4})/g, lu = {}, fi = {};
    var ae = /\d/, k = /\d\d/, ve = /\d{3}/, au = /\d{4}/, or = /[+-]?\d{6}/, s = /\d\d?/, ye = /\d\d\d\d?/, pe = /\d\d\d\d\d\d?/, sr = /\d{1,3}/, vu = /\d{1,4}/, hr = /[+-]?\d{1,6}/, cr = /[+-]?\d+/, gh = /Z|[+-]\d\d:?\d\d/gi, lr = /Z|[+-]\d\d(?::?\d\d)?/gi, pi = /[0-9]{0,256}['a-z\u00A0-\u05FF\u0700-\uD7FF\uF900-\uFDCF\uFDF0-\uFF07\uFF10-\uFFEF]{1,256}|[\u0600-\u06FF\/]{1,256}(\s*?[\u0600-\u06FF]{1,256}){1,2}/i, yu = {};
    ar = {};
    var tt = 0, at = 1, ht = 2, v = 3, it = 4, vt = 5, ni = 6, rc = 7, uc = 8;
    r("Y", 0, 0, function () {
        var n = this.year();
        return n <= 9999 ? "" + n : "+" + n;
    });
    r(0, ["YY", 2], 0, function () {
        return this.year() % 100;
    });
    r(0, ["YYYY", 4], 0, "year");
    r(0, ["YYYYY", 5], 0, "year");
    r(0, ["YYYYYY", 6, !0], 0, "year");
    p("year", "y");
    w("year", 1);
    i("Y", cr);
    i("YY", s, k);
    i("YYYY", vu, au);
    i("YYYYY", hr, or);
    i("YYYYYY", hr, or);
    h(["YYYYY", "YYYYYY"], tt);
    h("YYYY", function (n, i) {
        i[tt] = n.length === 2 ? t.parseTwoDigitYear(n) : f(n);
    });
    h("YY", function (n, i) {
        i[tt] = t.parseTwoDigitYear(n);
    });
    h("Y", function (n, t) {
        t[tt] = parseInt(n, 10);
    });
    t.parseTwoDigitYear = function (n) {
        return f(n) + (f(n) > 68 ? 1900 : 2e3);
    };
    pu = ei("FullYear", !0);
    a = Array.prototype.indexOf ? Array.prototype.indexOf : function (n) {
        for (var t = 0; t < this.length; ++t) if (this[t] === n) return t;
        return -1;
    };
    r("M", ["MM", 2], "Mo", function () {
        return this.month() + 1;
    });
    r("MMM", 0, 0, function (n) {
        return this.localeData().monthsShort(this, n);
    });
    r("MMMM", 0, 0, function (n) {
        return this.localeData().months(this, n);
    });
    p("month", "M");
    w("month", 8);
    i("M", s);
    i("MM", s, k);
    i("MMM", function (n, t) {
        return t.monthsShortRegex(n);
    });
    i("MMMM", function (n, t) {
        return t.monthsRegex(n);
    });
    h(["M", "MM"], function (n, t) {
        t[at] = f(n) - 1;
    });
    h(["MMM", "MMMM"], function (n, t, i, r) {
        var f = i._locale.monthsParse(n, r, i._strict);
        f != null ? t[at] = f : u(i).invalidMonth = n;
    });
    wu = /D[oD]?(\[[^\[\]]*\]|\s)+MMMM?/;
    be = "January_February_March_April_May_June_July_August_September_October_November_December".split("_");
    bu = "Jan_Feb_Mar_Apr_May_Jun_Jul_Aug_Sep_Oct_Nov_Dec".split("_");
    ge = pi;
    no = pi;
    r("w", ["ww", 2], "wo", "week");
    r("W", ["WW", 2], "Wo", "isoWeek");
    p("week", "w");
    p("isoWeek", "W");
    w("week", 5);
    w("isoWeek", 5);
    i("w", s);
    i("ww", s, k);
    i("W", s);
    i("WW", s, k);
    wi(["w", "ww", "W", "WW"], function (n, t, i, r) {
        t[r.substr(0, 1)] = f(n);
    });
    ro = {
        dow: 0,
        doy: 6
    };
    r("d", 0, "do", "day");
    r("dd", 0, 0, function (n) {
        return this.localeData().weekdaysMin(this, n);
    });
    r("ddd", 0, 0, function (n) {
        return this.localeData().weekdaysShort(this, n);
    });
    r("dddd", 0, 0, function (n) {
        return this.localeData().weekdays(this, n);
    });
    r("e", 0, 0, "weekday");
    r("E", 0, 0, "isoWeekday");
    p("day", "d");
    p("weekday", "e");
    p("isoWeekday", "E");
    w("day", 11);
    w("weekday", 11);
    w("isoWeekday", 11);
    i("d", s);
    i("e", s);
    i("E", s);
    i("dd", function (n, t) {
        return t.weekdaysMinRegex(n);
    });
    i("ddd", function (n, t) {
        return t.weekdaysShortRegex(n);
    });
    i("dddd", function (n, t) {
        return t.weekdaysRegex(n);
    });
    wi(["dd", "ddd", "dddd"], function (n, t, i, r) {
        var f = i._locale.weekdaysParse(n, r, i._strict);
        f != null ? t.d = f : u(i).invalidWeekday = n;
    });
    wi(["d", "e", "E"], function (n, t, i, r) {
        t[r] = f(n);
    });
    uo = "Sunday_Monday_Tuesday_Wednesday_Thursday_Friday_Saturday".split("_");
    du = "Sun_Mon_Tue_Wed_Thu_Fri_Sat".split("_");
    fo = "Su_Mo_Tu_We_Th_Fr_Sa".split("_");
    eo = pi;
    oo = pi;
    so = pi;
    r("H", ["HH", 2], 0, "hour");
    r("h", ["hh", 2], 0, nf);
    r("k", ["kk", 2], 0, yl);
    r("hmm", 0, 0, function () {
        return "" + nf.apply(this) + lt(this.minutes(), 2);
    });
    r("hmmss", 0, 0, function () {
        return "" + nf.apply(this) + lt(this.minutes(), 2) + lt(this.seconds(), 2);
    });
    r("Hmm", 0, 0, function () {
        return "" + this.hours() + lt(this.minutes(), 2);
    });
    r("Hmmss", 0, 0, function () {
        return "" + this.hours() + lt(this.minutes(), 2) + lt(this.seconds(), 2);
    });
    ho("a", !0);
    ho("A", !1);
    p("hour", "h");
    w("hour", 13);
    i("a", co);
    i("A", co);
    i("H", s);
    i("h", s);
    i("k", s);
    i("HH", s, k);
    i("hh", s, k);
    i("kk", s, k);
    i("hmm", ye);
    i("hmmss", pe);
    i("Hmm", ye);
    i("Hmmss", pe);
    h(["H", "HH"], v);
    h(["k", "kk"], function (n, t) {
        var i = f(n);
        t[v] = i === 24 ? 0 : i;
    });
    h(["a", "A"], function (n, t, i) {
        i._isPm = i._locale.isPM(n);
        i._meridiem = n;
    });
    h(["h", "hh"], function (n, t, i) {
        t[v] = f(n);
        u(i).bigHour = !0;
    });
    h("hmm", function (n, t, i) {
        var r = n.length - 2;
        t[v] = f(n.substr(0, r));
        t[it] = f(n.substr(r));
        u(i).bigHour = !0;
    });
    h("hmmss", function (n, t, i) {
        var r = n.length - 4, e = n.length - 2;
        t[v] = f(n.substr(0, r));
        t[it] = f(n.substr(r, 2));
        t[vt] = f(n.substr(e));
        u(i).bigHour = !0;
    });
    h("Hmm", function (n, t) {
        var i = n.length - 2;
        t[v] = f(n.substr(0, i));
        t[it] = f(n.substr(i));
    });
    h("Hmmss", function (n, t) {
        var i = n.length - 4, r = n.length - 2;
        t[v] = f(n.substr(0, i));
        t[it] = f(n.substr(i, 2));
        t[vt] = f(n.substr(r));
    });
    lo = /[ap]\.?m?\.?/i;
    var bl = ei("Hours", !0), ao = {
        calendar: ue,
        longDateFormat: fe,
        invalidDate: ee,
        ordinal: oe,
        dayOfMonthOrdinalParse: se,
        relativeTime: he,
        months: be,
        monthsShort: bu,
        week: ro,
        weekdays: uo,
        weekdaysMin: fo,
        weekdaysShort: du,
        meridiemParse: lo
    }, y = {}, gi = {}, nr;
    var ia = /^\s*((?:[+-]\d{6}|\d{4})-(?:\d\d-\d\d|W\d\d-\d|W\d\d|\d\d\d|\d\d))(?:(T| )(\d\d(?::\d\d(?::\d\d(?:[.,]\d+)?)?)?)([\+\-]\d\d(?::?\d\d)?|\s*Z)?)?$/, ra = /^\s*((?:[+-]\d{6}|\d{4})(?:\d\d\d\d|W\d\d\d|W\d\d|\d\d\d|\d\d))(?:(T| )(\d\d(?:\d\d(?:\d\d(?:[.,]\d+)?)?)?)([\+\-]\d\d(?::?\d\d)?|\s*Z)?)?$/, ua = /Z|[+-]\d\d(?::?\d\d)?/, kr = [["YYYYYY-MM-DD", /[+-]\d{6}-\d\d-\d\d/], ["YYYY-MM-DD", /\d{4}-\d\d-\d\d/], ["GGGG-[W]WW-E", /\d{4}-W\d\d-\d/], ["GGGG-[W]WW", /\d{4}-W\d\d/, !1], ["YYYY-DDD", /\d{4}-\d{3}/], ["YYYY-MM", /\d{4}-\d\d/, !1], ["YYYYYYMMDD", /[+-]\d{10}/], ["YYYYMMDD", /\d{8}/], ["GGGG[W]WWE", /\d{4}W\d{3}/], ["GGGG[W]WW", /\d{4}W\d{2}/, !1], ["YYYYDDD", /\d{7}/]], ff = [["HH:mm:ss.SSSS", /\d\d:\d\d:\d\d\.\d+/], ["HH:mm:ss,SSSS", /\d\d:\d\d:\d\d,\d+/], ["HH:mm:ss", /\d\d:\d\d:\d\d/], ["HH:mm", /\d\d:\d\d/], ["HHmmss.SSSS", /\d\d\d\d\d\d\.\d+/], ["HHmmss,SSSS", /\d\d\d\d\d\d,\d+/], ["HHmmss", /\d\d\d\d\d\d/], ["HHmm", /\d\d\d\d/], ["HH", /\d\d/]], fa = /^\/?Date\((\-?\d+)/i;
    po = /^(?:(Mon|Tue|Wed|Thu|Fri|Sat|Sun),?\s)?(\d{1,2})\s(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s(\d{2,4})\s(\d\d):(\d\d)(?::(\d\d))?\s(?:(UT|GMT|[ECMP][SD]T)|([Zz])|([+-]\d{4}))$/;
    wo = {
        UT: 0,
        GMT: 0,
        EDT: -240,
        EST: -300,
        CDT: -300,
        CST: -360,
        MDT: -360,
        MST: -420,
        PDT: -420,
        PST: -480
    };
    t.createFromInputFallback = g("value provided is not in a recognized RFC2822 or ISO format. moment construction falls back to js Date(), which is not reliable across all browsers and versions. Non RFC2822/ISO date formats are discouraged and will be removed in an upcoming major release. Please refer to http://momentjs.com/guides/#/warnings/js-date/ for more info.", function (n) {
        n._d = new Date(n._i + (n._useUTC ? " UTC" : ""));
    });
    t.ISO_8601 = function () { };
    t.RFC_2822 = function () { };
    ns = g("moment().min is deprecated, use moment.max instead. http://momentjs.com/guides/#/warnings/min-max/", function () {
        var n = c.apply(null, arguments);
        return this.isValid() && n.isValid() ? n < this ? this : n : ir();
    });
    ts = g("moment().max is deprecated, use moment.min instead. http://momentjs.com/guides/#/warnings/min-max/", function () {
        var n = c.apply(null, arguments);
        return this.isValid() && n.isValid() ? n > this ? this : n : ir();
    });
    rs = function () {
        return Date.now ? Date.now() : +new Date();
    };
    hi = ["year", "quarter", "month", "week", "day", "hour", "minute", "second", "millisecond"];
    us("Z", ":");
    us("ZZ", "");
    i("Z", lr);
    i("ZZ", lr);
    h(["Z", "ZZ"], function (n, t, i) {
        i._useUTC = !0;
        i._tzm = hf(lr, n);
    });
    fs = /([\+\-]|\d\d)/gi;
    t.updateOffset = function () { };
    os = /^(\-|\+)?(?:(\d*)[. ])?(\d+)\:(\d+)(?:\:(\d+)(\.\d*)?)?$/;
    ss = /^(-|\+)?P(?:([-+]?[0-9,.]*)Y)?(?:([-+]?[0-9,.]*)M)?(?:([-+]?[0-9,.]*)W)?(?:([-+]?[0-9,.]*)D)?(?:T(?:([-+]?[0-9,.]*)H)?(?:([-+]?[0-9,.]*)M)?(?:([-+]?[0-9,.]*)S)?)?$/;
    rt.fn = dr.prototype;
    rt.invalid = nv;
    as = cs(1, "add");
    vs = cs(-1, "subtract");
    t.defaultFormat = "YYYY-MM-DDTHH:mm:ssZ";
    t.defaultFormatUtc = "YYYY-MM-DDTHH:mm:ss[Z]";
    vf = g("moment().lang() is deprecated. Instead, use moment().localeData() to get the language configuration. Use moment().locale() to change languages.", function (n) {
        return n === undefined ? this.localeData() : this.locale(n);
    });
    var gr = 1e3, ci = 60 * gr, nu = 60 * ci, ws = 3506328 * nu;
    for (r(0, ["gg", 2], 0, function () {
        return this.weekYear() % 100;
    }), r(0, ["GG", 2], 0, function () {
        return this.isoWeekYear() % 100;
    }), tu("gggg", "weekYear"), tu("ggggg", "weekYear"), tu("GGGG", "isoWeekYear"),
    tu("GGGGG", "isoWeekYear"), p("weekYear", "gg"), p("isoWeekYear", "GG"), w("weekYear", 1),
    w("isoWeekYear", 1), i("G", cr), i("g", cr), i("GG", s, k), i("gg", s, k), i("GGGG", vu, au),
    i("gggg", vu, au), i("GGGGG", hr, or), i("ggggg", hr, or), wi(["gggg", "ggggg", "GGGG", "GGGGG"], function (n, t, i, r) {
        t[r.substr(0, 2)] = f(n);
    }), wi(["gg", "GG"], function (n, i, r, u) {
        i[u] = t.parseTwoDigitYear(n);
    }), r("Q", 0, "Qo", "quarter"), p("quarter", "Q"), w("quarter", 7), i("Q", ae),
    h("Q", function (n, t) {
        t[at] = (f(n) - 1) * 3;
    }), r("D", ["DD", 2], "Do", "date"), p("date", "D"), w("date", 9), i("D", s),
    i("DD", s, k), i("Do", function (n, t) {
        return n ? t._dayOfMonthOrdinalParse || t._ordinalParse : t._dayOfMonthOrdinalParseLenient;
    }), h(["D", "DD"], ht), h("Do", function (n, t) {
        t[ht] = f(n.match(s)[0]);
    }), yf = ei("Date", !0), r("DDD", ["DDDD", 3], "DDDo", "dayOfYear"), p("dayOfYear", "DDD"),
    w("dayOfYear", 4), i("DDD", sr), i("DDDD", ve), h(["DDD", "DDDD"], function (n, t, i) {
        i._dayOfYear = f(n);
    }), r("m", ["mm", 2], 0, "minute"), p("minute", "m"), w("minute", 14), i("m", s),
    i("mm", s, k), h(["m", "mm"], it), gs = ei("Minutes", !1), r("s", ["ss", 2], 0, "second"),
    p("second", "s"), w("second", 15), i("s", s), i("ss", s, k), h(["s", "ss"], vt),
    nh = ei("Seconds", !1), r("S", 0, 0, function () {
        return ~~(this.millisecond() / 100);
    }), r(0, ["SS", 2], 0, function () {
        return ~~(this.millisecond() / 10);
    }), r(0, ["SSS", 3], 0, "millisecond"), r(0, ["SSSS", 4], 0, function () {
        return this.millisecond() * 10;
    }), r(0, ["SSSSS", 5], 0, function () {
        return this.millisecond() * 100;
    }), r(0, ["SSSSSS", 6], 0, function () {
        return this.millisecond() * 1e3;
    }), r(0, ["SSSSSSS", 7], 0, function () {
        return this.millisecond() * 1e4;
    }), r(0, ["SSSSSSSS", 8], 0, function () {
        return this.millisecond() * 1e5;
    }), r(0, ["SSSSSSSSS", 9], 0, function () {
        return this.millisecond() * 1e6;
    }), p("millisecond", "ms"), w("millisecond", 16), i("S", sr, ae), i("SS", sr, k),
    i("SSS", sr, ve), kt = "SSSS"; kt.length <= 9; kt += "S") i(kt, /\d+/);
    for (kt = "S"; kt.length <= 9; kt += "S") h(kt, op);
    th = ei("Milliseconds", !1);
    r("z", 0, 0, "zoneAbbr");
    r("zz", 0, 0, "zoneName");
    n = yi.prototype;
    n.add = as;
    n.calendar = vv;
    n.clone = yv;
    n.diff = ny;
    n.endOf = cy;
    n.format = uy;
    n.from = fy;
    n.fromNow = ey;
    n.to = oy;
    n.toNow = sy;
    n.get = ec;
    n.invalidAt = dy;
    n.isAfter = pv;
    n.isBefore = wv;
    n.isBetween = bv;
    n.isSame = kv;
    n.isSameOrAfter = dv;
    n.isSameOrBefore = gv;
    n.isValid = by;
    n.lang = vf;
    n.locale = ys;
    n.localeData = ps;
    n.max = ts;
    n.min = ns;
    n.parsingFlags = ky;
    n.set = oc;
    n.startOf = hy;
    n.subtract = vs;
    n.toArray = yy;
    n.toObject = py;
    n.toDate = vy;
    n.toISOString = iy;
    n.inspect = ry;
    n.toJSON = wy;
    n.toString = ty;
    n.unix = ay;
    n.valueOf = ly;
    n.creationData = gy;
    n.year = pu;
    n.isLeapYear = fc;
    n.weekYear = np;
    n.isoWeekYear = tp;
    n.quarter = n.quarters = fp;
    n.month = de;
    n.daysInMonth = vc;
    n.week = n.weeks = gc;
    n.isoWeek = n.isoWeeks = nl;
    n.weeksInYear = rp;
    n.isoWeeksInYear = ip;
    n.date = yf;
    n.day = n.days = sl;
    n.weekday = hl;
    n.isoWeekday = cl;
    n.dayOfYear = ep;
    n.hour = n.hours = bl;
    n.minute = n.minutes = gs;
    n.second = n.seconds = nh;
    n.millisecond = n.milliseconds = th;
    n.utcOffset = tv;
    n.utc = rv;
    n.local = uv;
    n.parseZone = fv;
    n.hasAlignedHourOffset = ev;
    n.isDST = ov;
    n.isLocal = hv;
    n.isUtcOffset = cv;
    n.isUtc = es;
    n.isUTC = es;
    n.zoneAbbr = sp;
    n.zoneName = hp;
    n.dates = g("dates accessor is deprecated. Use date instead.", yf);
    n.months = g("months accessor is deprecated. Use month instead", de);
    n.years = g("years accessor is deprecated. Use year instead", pu);
    n.zone = g("moment().zone is deprecated, use moment().utcOffset instead. http://momentjs.com/guides/#/warnings/zone/", iv);
    n.isDSTShifted = g("isDSTShifted is deprecated. See http://momentjs.com/guides/#/warnings/dst-shifted/ for more information", sv);
    o = su.prototype;
    o.calendar = lh;
    o.longDateFormat = ah;
    o.invalidDate = vh;
    o.ordinal = yh;
    o.preparse = ih;
    o.postformat = ih;
    o.relativeTime = ph;
    o.pastFuture = wh;
    o.set = ch;
    o.months = hc;
    o.monthsShort = cc;
    o.monthsParse = ac;
    o.monthsRegex = pc;
    o.monthsShortRegex = yc;
    o.week = bc;
    o.firstDayOfYear = dc;
    o.firstDayOfWeek = kc;
    o.weekdays = rl;
    o.weekdaysMin = fl;
    o.weekdaysShort = ul;
    o.weekdaysParse = ol;
    o.weekdaysRegex = ll;
    o.weekdaysShortRegex = al;
    o.weekdaysMinRegex = vl;
    o.isPM = pl;
    o.meridiem = wl;
    oi("en", {
        dayOfMonthOrdinalParse: /\d{1,2}(th|st|nd|rd)/,
        ordinal: function (n) {
            var t = n % 10, i = f(n % 100 / 10) === 1 ? "th" : t === 1 ? "st" : t === 2 ? "nd" : t === 3 ? "rd" : "th";
            return n + i;
        }
    });
    t.lang = g("moment.lang is deprecated. Use moment.locale instead.", oi);
    t.langData = g("moment.langData is deprecated. Use moment.localeData instead.", bt);
    ct = Math.abs;
    var iw = yt("ms"), rw = yt("s"), uw = yt("m"), fw = yt("h"), ew = yt("d"), ow = yt("w"), sw = yt("M"), hw = yt("Q"), cw = yt("y");
    var vw = ri("milliseconds"), yw = ri("seconds"), pw = ri("minutes"), ww = ri("hours"), bw = ri("days"), kw = ri("months"), dw = ri("years");
    return pt = Math.round, ut = {
        ss: 44,
        s: 45,
        m: 45,
        h: 22,
        d: 26,
        M: 11
    }, ru = Math.abs, e = dr.prototype, e.isValid = ga, e.abs = bp, e.add = kp, e.subtract = dp,
    e.as = nw, e.asMilliseconds = iw, e.asSeconds = rw, e.asMinutes = uw, e.asHours = fw,
    e.asDays = ew, e.asWeeks = ow, e.asMonths = sw, e.asQuarters = hw, e.asYears = cw,
    e.valueOf = tw, e._bubble = gp, e.clone = lw, e.get = aw, e.milliseconds = vw, e.seconds = yw,
    e.minutes = pw, e.hours = ww, e.days = bw, e.weeks = gw, e.months = kw, e.years = dw,
    e.humanize = ub, e.toISOString = uu, e.toString = uu, e.toJSON = uu, e.locale = ys,
    e.localeData = ps, e.toIsoString = g("toIsoString() is deprecated. Please use toISOString() instead (notice the capitals)", uu),
    e.lang = vf, r("X", 0, 0, "unix"), r("x", 0, 0, "valueOf"), i("x", cr), i("X", /[+-]?\d+(\.\d{1,3})?/),
    h("X", function (n, t, i) {
        i._d = new Date(parseFloat(n, 10) * 1e3);
    }), h("x", function (n, t, i) {
        i._d = new Date(f(n));
    }), t.version = "2.24.0", oh(c), t.fn = n, t.min = ba, t.max = ka, t.now = rs, t.utc = et,
    t.unix = cp, t.months = ap, t.isDate = tr, t.locale = oi, t.invalid = ir, t.duration = rt,
    t.isMoment = ot, t.weekdays = yp, t.parseZone = lp, t.localeData = bt, t.isDuration = of,
    t.monthsShort = vp, t.weekdaysMin = wp, t.defineLocale = tf, t.updateLocale = dl,
    t.locales = gl, t.weekdaysShort = pp, t.normalizeUnits = nt, t.relativeTimeRounding = ib,
    t.relativeTimeThreshold = rb, t.calendarFormat = av, t.prototype = n, t.HTML5_FMT = {
        DATETIME_LOCAL: "YYYY-MM-DDTHH:mm",
        DATETIME_LOCAL_SECONDS: "YYYY-MM-DDTHH:mm:ss",
        DATETIME_LOCAL_MS: "YYYY-MM-DDTHH:mm:ss.SSS",
        DATE: "YYYY-MM-DD",
        TIME: "HH:mm",
        TIME_SECONDS: "HH:mm:ss",
        TIME_MS: "HH:mm:ss.SSS",
        WEEK: "GGGG-[W]WW",
        MONTH: "YYYY-MM"
    }, t;
}), function (n, t) {
    if (typeof define == "function" && define.amd) define(["moment"], function (n) {
        return t(n);
    }); else if (typeof module == "object" && module.exports) {
        var i = typeof window != "undefined" && typeof window.moment != "undefined" ? window.moment : require("moment");
        module.exports = t(i);
    } else n.Lightpick = t(n.moment);
}(this, function (n) {
    "use strict";
    var t = window.document, o = {
        field: null,
        midField: null,
        secondField: null,
        firstDay: 1,
        parentEl: "body",
        lang: "auto",
        format: "MM/DD/YYYY",
        separator: " / ",
        numberOfMonths: 2,
        numberOfColumns: 2,
        singleDate: !0,
        multiDate: !1,
        autoclose: !0,
        repick: !1,
        startDate: null,
        midDate: null,
        endDate: null,
        minDate: null,
        maxDate: null,
        disableDates: null,
        selectForward: !1,
        selectBackward: !1,
        minDays: null,
        maxDays: null,
        hoveringTooltip: !1,
        hideOnBodyClick: !0,
        footer: !1,
        disabledDatesInRange: !0,
        tooltipNights: !1,
        orientation: "auto",
        disableWeekends: !1,
        inline: !1,
        dropdowns: {
            years: !1,
            months: !1
        },
        locale: {
            buttons: {
                prev: "&leftarrow;",
                next: "&rightarrow;",
                close: "&times;",
                reset: "Reset",
                apply: "Apply"
            },
            tooltip: {
                one: "day",
                other: "days"
            },
            tooltipOnDisabled: null,
            pluralize: function (n, t) {
                return (typeof n == "string" && (n = parseInt(n, 10)), n === 1 && "one" in t) ? t.one : "other" in t ? t.other : "";
            }
        },
        onSelect: null,
        onOpen: null,
        onClose: null,
        onError: null,
        TargetDPId: null
    }, f = function (n) {
        var r = new Date(n.calendar[0]._d.getFullYear(), n.calendar[0]._d.getMonth() - 1, 28), i;
        n.calendar[1] != null && (i = new Date(n.calendar[1]._d.getFullYear(), n.calendar[1]._d.getMonth(), 1));
        var u = new Date(n.minDate._d.getFullYear(), n.minDate._d.getMonth(), 1), f = new Date(n.maxDate._d.getFullYear(), n.maxDate._d.getMonth(), 1), t = '<div class="lightpick__toolbar">';
        return t += '<div class="lightpick_selecteddate_header"><span class="lightpick_selecteddate_From_Date"></span><span class="lightpick_selecteddate_Separator">=></span><span class="lightpick_selecteddate_To_Date"></span></div>',
        t += "", t += r >= u ? '<button type="button" class="lightpick__previous-action">' + n.locale.buttons.prev + "</button>" : '<button type="button" class="lightpick__previous-action" style="visibility :hidden" >' + n.locale.buttons.prev + "</button>",
        t += n.calendar[1] == null ? '<button type="button" class="lightpick__next-action">' + n.locale.buttons.next + "</button>" : i <= f ? '<button type="button" class="lightpick__next-action">' + n.locale.buttons.next + "</button>" : '<button type="button" class="lightpick__next-action"  style="visibility :hidden" >' + n.locale.buttons.next + "</button>",
        t + ((!n.autoclose && !n.inline ? '<button type="button" class="lightpick__close-action">' + n.locale.buttons.close + "</button>" : "") + "</div>");
    }, s = function (n, t, i) {
        return new Date(1970, 0, t).toLocaleString(n.lang, {
            weekday: i ? "short" : "long"
        });
    }, u = function (i, r, u, f) {
        var o, c, l, s, h;
        if (u) return "<div></div>";
        var r = n(r), a = n(r).subtract(1, "month"), v = n(r).add(1, "month"), e = {
            time: n(r).valueOf(),
            className: ["lightpick__day", "is-available"]
        };
        if (i.multiDate) {
            if (f instanceof Array || Object.prototype.toString.call(f) === "[object Array]" ? (f = f.filter(function (n) {
                return ["lightpick__day", "is-available", "is-previous-month", "is-next-month"].indexOf(n) >= 0;
            }), e.className = e.className.concat(f)) : e.className.push(f), i.disableDates) for (o = 0; o < i.disableDates.length; o++) i.disableDates[o] instanceof Array || Object.prototype.toString.call(i.disableDates[o]) === "[object Array]" ? (c = n(i.disableDates[o][0]),
            l = n(i.disableDates[o][1]), c.isValid() && l.isValid() && r.isBetween(c, l, "day", "[]") && e.className.push("is-disabled")) : n(i.disableDates[o]).isValid() && n(i.disableDates[o]).isSame(r, "day") && e.className.push("is-disabled"),
            e.className.indexOf("is-disabled") >= 0 && (i.locale.tooltipOnDisabled && (!i.startDate || r.isAfter(i.startDate) || i.startDate && i.endDate) && e.className.push("disabled-tooltip"),
            e.className.indexOf("is-start-date") >= 0 ? (this.setStartDate(null), this.setMidDate(null),
            this.setEndDate(null)) : e.className.indexOf("is-mid-date") >= 0 ? (this.setMidDate(null),
            this.setEndDate(null)) : e.className.indexOf("is-end-date") >= 0 && this.setEndDate(null));
            i.minDays && i.startDate && !i.midDate && !i.endDate && r.isBetween(n(i.startDate).subtract(i.minDays - 1, "day"), n(i.startDate).add(i.minDays - 1, "day"), "day") && (e.className.push("is-disabled"),
            i.selectForward && r.isSameOrAfter(i.startDate) && (e.className.push("is-forward-selected"),
            e.className.push("is-in-range")));
            i.maxDays && i.startDate && !i.midDate && !i.endDate && (r.isSameOrBefore(n(i.startDate).subtract(i.maxDays, "day"), "day") ? e.className.push("is-disabled") : r.isSameOrAfter(n(i.startDate).add(i.maxDays, "day"), "day") && e.className.push("is-disabled"));
            i.repick && (i.minDays || i.maxDays) && i.startDate && i.endDate ? (s = n(i.repickTrigger == i.field ? i.endDate : i.startDate),
            i.minDays && r.isBetween(n(s).subtract(i.minDays - 1, "day"), n(s).add(i.minDays - 1, "day"), "day") && e.className.push("is-disabled"),
            i.maxDays && (r.isSameOrBefore(n(s).subtract(i.maxDays, "day"), "day") ? e.className.push("is-disabled") : r.isSameOrAfter(n(s).add(i.maxDays, "day"), "day") && e.className.push("is-disabled"))) : i.repick && (i.minDays || i.maxDays) && i.startDate && i.midDate && !i.endDate && (s = n(i.repickTrigger == i.field ? i.midDate : i.startDate),
            i.minDays && r.isBetween(n(s).subtract(i.minDays - 1, "day"), n(s).add(i.minDays - 1, "day"), "day") && e.className.push("is-disabled"),
            i.maxDays && (r.isSameOrBefore(n(s).subtract(i.maxDays, "day"), "day") ? e.className.push("is-disabled") : r.isSameOrAfter(n(s).add(i.maxDays, "day"), "day") && e.className.push("is-disabled")));
            r.isSame(new Date(), "day") && e.className.push("is-today");
            r.isSame(i.startDate, "day") && e.className.push("is-start-date");
            r.isSame(i.midDate, "day") && e.className.push("is-mid-date");
            r.isSame(i.endDate, "day") && e.className.push("is-end-date");
            i.startDate && i.endDate && r.isBetween(i.startDate, i.endDate, "day", "[]") ? e.className.push("is-in-range") : i.startDate && i.midDate && !i.endDate && r.isBetween(i.startDate, i.midDate, "day", "[]") && e.className.push("is-in-range");
            n().isSame(r, "month") || (a.isSame(r, "month") ? e.className.push("is-previous-month") : v.isSame(r, "month") && e.className.push("is-next-month"));
            i.minDate && r.isBefore(i.minDate, "day") && e.className.push("is-disabled");
            i.maxDate && r.isAfter(i.maxDate, "day") && e.className.push("is-disabled");
            i.selectForward && !i.singleDate && i.startDate && i.midDate && !i.endDate && r.isBefore(i.startDate, "day") ? e.className.push("is-disabled") : i.selectForward && !i.singleDate && i.startDate && !i.midDate && !i.endDate && r.isBefore(i.startDate, "day") && e.className.push("is-disabled");
            i.selectBackward && !i.singleDate && i.startDate && i.midDate && !i.endDate && r.isAfter(i.startDate, "day") ? e.className.push("is-disabled") : i.selectBackward && !i.singleDate && i.startDate && !i.midDate && !i.endDate && r.isAfter(i.startDate, "day") && e.className.push("is-disabled");
            i.disableWeekends && (r.isoWeekday() == 6 || r.isoWeekday() == 7) && e.className.push("is-disabled");
            e.className = e.className.filter(function (n, t, i) {
                return i.indexOf(n) === t;
            });
            e.className.indexOf("is-disabled") >= 0 && e.className.indexOf("is-available") >= 0 && e.className.splice(e.className.indexOf("is-available"), 1);
            h = t.createElement("div");
            h.className = e.className.join(" ");
            h.innerHTML = r.get("date");
            h.setAttribute("data-time", e.time);
        } else {
            if (f instanceof Array || Object.prototype.toString.call(f) === "[object Array]" ? (f = f.filter(function (n) {
                return ["lightpick__day", "is-available", "is-previous-month", "is-next-month"].indexOf(n) >= 0;
            }), e.className = e.className.concat(f)) : e.className.push(f), i.disableDates) for (o = 0; o < i.disableDates.length; o++) i.disableDates[o] instanceof Array || Object.prototype.toString.call(i.disableDates[o]) === "[object Array]" ? (c = n(i.disableDates[o][0]),
            l = n(i.disableDates[o][1]), c.isValid() && l.isValid() && r.isBetween(c, l, "day", "[]") && e.className.push("is-disabled")) : n(i.disableDates[o]).isValid() && n(i.disableDates[o]).isSame(r, "day") && e.className.push("is-disabled"),
            e.className.indexOf("is-disabled") >= 0 && (i.locale.tooltipOnDisabled && (!i.startDate || r.isAfter(i.startDate) || i.startDate && i.endDate) && e.className.push("disabled-tooltip"),
            e.className.indexOf("is-start-date") >= 0 ? (this.setStartDate(null), this.setEndDate(null)) : e.className.indexOf("is-end-date") >= 0 && this.setEndDate(null));
            i.minDays && i.startDate && !i.endDate && r.isBetween(n(i.startDate).subtract(i.minDays - 1, "day"), n(i.startDate).add(i.minDays - 1, "day"), "day") && (e.className.push("is-disabled"),
            i.selectForward && r.isSameOrAfter(i.startDate) && (e.className.push("is-forward-selected"),
            e.className.push("is-in-range")));
            i.maxDays && i.startDate && !i.endDate && (r.isSameOrBefore(n(i.startDate).subtract(i.maxDays, "day"), "day") ? e.className.push("is-disabled") : r.isSameOrAfter(n(i.startDate).add(i.maxDays, "day"), "day") && e.className.push("is-disabled"));
            i.repick && (i.minDays || i.maxDays) && i.startDate && i.endDate && (s = n(i.repickTrigger == i.field ? i.endDate : i.startDate),
            i.minDays && r.isBetween(n(s).subtract(i.minDays - 1, "day"), n(s).add(i.minDays - 1, "day"), "day") && e.className.push("is-disabled"),
            i.maxDays && (r.isSameOrBefore(n(s).subtract(i.maxDays, "day"), "day") ? e.className.push("is-disabled") : r.isSameOrAfter(n(s).add(i.maxDays, "day"), "day") && e.className.push("is-disabled")));
            r.isSame(new Date(), "day") && e.className.push("is-today");
            r.isSame(i.startDate, "day") && e.className.push("is-start-date");
            r.isSame(i.midDate, "day") && e.className.push("is-mid-date");
            r.isSame(i.endDate, "day") && e.className.push("is-end-date");
            i.startDate && i.endDate && r.isBetween(i.startDate, i.endDate, "day", "[]") && e.className.push("is-in-range");
            n().isSame(r, "month") || (a.isSame(r, "month") ? e.className.push("is-previous-month") : v.isSame(r, "month") && e.className.push("is-next-month"));
            i.minDate && r.isBefore(i.minDate, "day") && e.className.push("is-disabled");
            i.maxDate && r.isAfter(i.maxDate, "day") && e.className.push("is-disabled");
            i.selectForward && !i.singleDate && i.startDate && !i.endDate && r.isBefore(i.startDate, "day") && e.className.push("is-disabled");
            i.selectBackward && !i.singleDate && i.startDate && !i.endDate && r.isAfter(i.startDate, "day") && e.className.push("is-disabled");
            i.disableWeekends && (r.isoWeekday() == 6 || r.isoWeekday() == 7) && e.className.push("is-disabled");
            e.className = e.className.filter(function (n, t, i) {
                return i.indexOf(n) === t;
            });
            e.className.indexOf("is-disabled") >= 0 && e.className.indexOf("is-available") >= 0 && e.className.splice(e.className.indexOf("is-available"), 1);
            h = t.createElement("div");
            h.className = e.className.join(" ");
            h.innerHTML = r.get("date");
            h.setAttribute("data-time", e.time);
        }
        return h.outerHTML;
    }, c = function (i, r) {
        for (var o = n(i), u = t.createElement("select"), e, f = 0; f < 12; f++) o.set("month", f),
        e = t.createElement("option"), e.value = o.toDate().getMonth(), e.text = getMomentMonthName(o.toDate().getMonth()),
        f === i.toDate().getMonth() && e.setAttribute("selected", "selected"), u.appendChild(e);
        return u.className = "lightpick__select lightpick__select-months", u.dir = "rtl",
        r.dropdowns && r.dropdowns.months || (u.disabled = !0), u.outerHTML;
    }, l = function (i, r) {
        var s = n(i), o = t.createElement("select"), u = r.dropdowns && r.dropdowns.years ? r.dropdowns.years : null, h = u && u.min ? u.min : 1900, c = u && u.max ? u.max : parseInt(n().format("YYYY")), f, e;
        for (parseInt(i.format("YYYY")) < h && (h = parseInt(i.format("YYYY"))), parseInt(i.format("YYYY")) > c && (c = parseInt(i.format("YYYY"))),
        f = h; f <= c; f++) s.set("year", f), e = t.createElement("option"), e.value = s.toDate().getFullYear(),
        e.text = s.toDate().getFullYear(), f === i.toDate().getFullYear() && e.setAttribute("selected", "selected"),
        o.appendChild(e);
        return o.className = "lightpick__select lightpick__select-years", r.dropdowns && r.dropdowns.years || (o.disabled = !0),
        o.outerHTML;
    }, r = function (t, i) {
        for (var r = "", k = n(i.calendar[0]), e, a, w, nt, v, d, o, b, y, h = 0; h < i.numberOfMonths; h++) {
            for (e = n(k), r += '<section class="lightpick__month">', r += '<div class="lightpick__month-title-bar">',
            r += '<div class="lightpick__month-title">' + c(e, i) + l(e, i) + "</div>", i.numberOfMonths === 1 && (r += f(i, "days")),
            r += "</div>", r += '<div class="lightpick__days-of-the-week">', a = i.firstDay + 4; a < 7 + i.firstDay + 4; ++a) r += '<div class="lightpick__day-of-the-week" title="' + s(i, a) + '">' + s(i, a, !0) + "</div>";
            if (r += "</div>", r += '<div class="lightpick__days">', e.isoWeekday() !== i.firstDay) {
                var g = e.isoWeekday() - i.firstDay > 0 ? e.isoWeekday() - i.firstDay : e.isoWeekday(), p = n(e).subtract(g, "day"), w = p.daysInMonth();
                for (o = p.get("date") ; o <= w; o++) r += u(i, p, h > 0, "is-previous-month"), p.add(1, "day");
            }
            for (w = e.daysInMonth(), nt = new Date(), o = 0; o < w; o++) r += u(i, e), e.add(1, "day");
            if (v = n(e), d = 7 - v.isoWeekday() + i.firstDay, d < 7) for (o = v.get("date") ; o <= d; o++) r += u(i, v, h < i.numberOfMonths - 1, "is-next-month"),
            v.add(1, "day");
            r += "</div>";
            r += "</section>";
            k.add(1, "month");
        }
        i.calendar[1] = n(k);
        t.querySelector(".lightpick__toolbar").innerHTML = f(i, "days");
        t.querySelector(".lightpick__months").innerHTML = r;
        i.multiDate ? ($(".lightpick_selecteddate_Separator").hide(), $(".lightpick_selecteddate_To_Date").hide(),
        $(".lightpick_selecteddate_From_Date").addClass("selecteddate_highlighter"), i.TargetDPId == "txtDepartDate_0" && (i.startDate != null ? $(".lightpick_selecteddate_From_Date").html(n(i.startDate).format("ddd DD MMM, YY")) : $(".lightpick_selecteddate_From_Date").html(n().format("ddd DD MMM, YY"))),
        i.TargetDPId == "txtDepartDate_1" && (i.midDate != null ? $(".lightpick_selecteddate_From_Date").html(n(i.midDate).format("ddd DD MMM, YY")) : i.startDate != null ? $(".lightpick_selecteddate_From_Date").html(n(i.startDate).format("ddd DD MMM, YY")) : $(".lightpick_selecteddate_From_Date").html(n().format("ddd DD MMM, YY"))),
        i.TargetDPId == "txtDepartDate_2" && (i.endDate != null ? $(".lightpick_selecteddate_From_Date").html(n(i.endDate).format("ddd DD MMM, YY")) : i.midDate != null ? $(".lightpick_selecteddate_From_Date").html(n(i.midDate).format("ddd DD MMM, YY")) : i.startDate != null ? $(".lightpick_selecteddate_From_Date").html(n(i.startDate).format("ddd DD MMM, YY")) : $(".lightpick_selecteddate_From_Date").html(n().format("ddd DD MMM, YY")))) : ($(".lightpick_selecteddate_Separator").show(),
        $(".lightpick_selecteddate_To_Date").show(), b = '<span class="lightpick_selecteddate_From_Date_Header">Depart</span>',
        y = '<span class="lightpick_selecteddate_To_Date_Header">Return</span>', (i.TargetDPId == "tbCheckIn" || i.TargetDPId == "tbCheckOut") && (b = '<span class="lightpick_selecteddate_From_Date_Header">Check-in</span>',
        y = '<span class="lightpick_selecteddate_To_Date_Header">Check-out</span>'), i.TargetDPId == "txtDepartDate" || i.TargetDPId == "tbCheckIn" ? ($(".lightpick_selecteddate_From_Date").addClass("selecteddate_highlighter"),
        $(".lightpick_selecteddate_To_Date").removeClass("selecteddate_highlighter")) : ($(".lightpick_selecteddate_From_Date").removeClass("selecteddate_highlighter"),
        $(".lightpick_selecteddate_To_Date").addClass("selecteddate_highlighter")), i.startDate != null ? $(".lightpick_selecteddate_From_Date").html(b + n(i.startDate).format("ddd DD MMM, YY")) : $(".lightpick_selecteddate_From_Date").html(b + n().format("ddd DD MMM, YY")),
        i.singleDate == !1 && i.endDate != null ? $(".lightpick_selecteddate_To_Date").html(y + n(i.endDate).format("ddd DD MMM, YY")) : i.singleDate == !1 && i.endDate == null ? i.startDate != null ? $(".lightpick_selecteddate_To_Date").html(y + n(i.startDate).format("ddd DD MMM, YY")) : $(".lightpick_selecteddate_To_Date").html(y + n().format("ddd DD MMM, YY")) : ($(".lightpick_selecteddate_To_Date").html(""),
        $(".lightpick_selecteddate_To_Date").hide(), $(".lightpick_selecteddate_Separator").hide()));
    }, i = function (n, t) {
        var i = n.querySelectorAll(".lightpick__day");
        [].forEach.call(i, function (n) {
            n.outerHTML = u(t, parseInt(n.getAttribute("data-time")), !1, n.className.split(" "));
        });
        e(n, t);
    }, e = function (t, i) {
        if (i.multiDate) {
            if (i.disabledDatesInRange || !i.startDate || !i.midDate || i.endDate || !i.disableDates) return;
            if (i.disabledDatesInRange || !i.startDate || i.midDate || i.endDate || !i.disableDates) return;
            var e = t.querySelectorAll(".lightpick__day"), f = i.disableDates.map(function (n) {
                return n instanceof Array || Object.prototype.toString.call(n) === "[object Array]" ? n[0] : n;
            }), r = n(f.filter(function (t) {
                return n(t).isBefore(i.startDate);
            }).sort(function (t, i) {
                return n(i).isAfter(n(t));
            })[0]), u = n(f.filter(function (t) {
                return n(t).isAfter(i.startDate);
            }).sort(function (t, i) {
                return n(t).isAfter(n(i));
            })[0]);
            [].forEach.call(e, function (t) {
                var f = n(parseInt(t.getAttribute("data-time")));
                (r && f.isBefore(r) && i.startDate.isAfter(r) || u && f.isAfter(u) && u.isAfter(i.startDate)) && (t.classList.remove("is-available"),
                t.classList.add("is-disabled"));
            });
        } else {
            if (i.disabledDatesInRange || !i.startDate || i.endDate || !i.disableDates) return;
            var e = t.querySelectorAll(".lightpick__day"), f = i.disableDates.map(function (n) {
                return n instanceof Array || Object.prototype.toString.call(n) === "[object Array]" ? n[0] : n;
            }), r = n(f.filter(function (t) {
                return n(t).isBefore(i.startDate);
            }).sort(function (t, i) {
                return n(i).isAfter(n(t));
            })[0]), u = n(f.filter(function (t) {
                return n(t).isAfter(i.startDate);
            }).sort(function (t, i) {
                return n(t).isAfter(n(i));
            })[0]);
            [].forEach.call(e, function (t) {
                var f = n(parseInt(t.getAttribute("data-time")));
                (r && f.isBefore(r) && i.startDate.isAfter(r) || u && f.isAfter(u) && u.isAfter(i.startDate)) && (t.classList.remove("is-available"),
                t.classList.add("is-disabled"));
            });
        }
    }, h = function (r) {
        var u = this, e = u.config(r), o;
        u.el = t.createElement("section");
        u.el.className = "lightpick lightpick--" + e.numberOfColumns + "-columns is-hidden";
        e.inline && (u.el.className += " lightpick--inlined");
        o = '<div class="lightpick__inner">' + (e.numberOfMonths > 1 ? f(e, "days") : "") + '<div class="lightpick__months"></div><div class="lightpick__tooltip" style="visibility: hidden"></div>';
        e.footer && (o += '<div class="lightpick__footer">', e.footer === !0 ? (o += '<button type="button" class="lightpick__reset-action">' + e.locale.buttons.reset + "</button>",
        o += '<div class="lightpick__footer-message"></div>', o += '<button type="button" class="lightpick__apply-action">' + e.locale.buttons.apply + "</button>") : o += e.footer,
        o += "</div>");
        o += "</div>";
        u.el.innerHTML = o;
        e.parentEl instanceof Node ? e.parentEl.appendChild(u.el) : e.parentEl === "body" && e.inline ? e.field.parentNode.appendChild(u.el) : t.querySelector(e.parentEl).appendChild(u.el);
        u._onMouseDown = function (t) {
            var f, r, e, o;
            if (u.isShowing && (t = t || window.event, f = t.target || t.srcElement, f)) if (t.stopPropagation(),
            f.classList.contains("lightpick__select") || t.preventDefault(), r = u._opts, r.multiDate) if (f.classList.contains("lightpick__day") && f.classList.contains("is-available")) {
                if (e = n(parseInt(f.getAttribute("data-time"))), !r.disabledDatesInRange && r.disableDates && r.startDate && !r.midDate) {
                    var s = e.isAfter(r.startDate) ? n(r.startDate) : n(e), h = e.isAfter(r.startDate) ? n(e) : n(r.startDate), c = r.disableDates.filter(function (t) {
                        if (t instanceof Array || Object.prototype.toString.call(t) === "[object Array]") {
                            var i = n(t[0]), r = n(t[1]);
                            return i.isValid() && r.isValid() && (i.isBetween(s, h, "day", "[]") || r.isBetween(s, h, "day", "[]"));
                        }
                        return n(t).isBetween(s, h, "day", "[]");
                    });
                    if (c.length) {
                        u.setStartDate(null);
                        u.setMidDate(null);
                        u.setEndDate(null);
                        f.dispatchEvent(new Event("mousedown"));
                        u.el.querySelector(".lightpick__tooltip").style.visibility = "hidden";
                        i(u.el, r);
                        return;
                    }
                }
                !r.startDate && !r.midDate && !r.endDate || r.startDate && r.midDate && r.endDate ? r.repick && r.startDate && r.midDate && r.endDate ? (r.repickTrigger === r.field ? (u.setStartDate(e),
                f.classList.add("is-start-date")) : r.repickTrigger === r.midField ? (u.setMidDate(e),
                f.classList.add("is-mid-date")) : (u.setEndDate(e), f.classList.add("is-end-date")),
                r.autoclose && setTimeout(function () {
                    u.hide();
                }, 100)) : r.repick && !r.startDate && r.midDate && r.endDate ? (r.TargetDPId == "txtDepartDate_0" && (u.setStartDate(e),
                f.classList.add("is-start-date")), r.singleDate && r.autoclose ? setTimeout(function () {
                    u.hide();
                }, 100) : (!r.singleDate || r.inline) && i(u.el, r)) : r.repick && r.startDate && !r.midDate && r.endDate ? (r.TargetDPId == "txtDepartDate_1" && (u.setMidDate(e),
                f.classList.add("is-mid-date")), r.singleDate && r.autoclose ? setTimeout(function () {
                    u.hide();
                }, 100) : (!r.singleDate || r.inline) && i(u.el, r)) : (r.TargetDPId == "txtDepartDate_0" ? (u.setStartDate(e),
                u.setMidDate(null), u.setEndDate(null), f.classList.add("is-start-date")) : r.TargetDPId == "txtDepartDate_1" ? (u.setStartDate(null),
                u.setMidDate(e), u.setEndDate(null), f.classList.add("is-mid-date")) : r.TargetDPId == "txtDepartDate_2" && (u.setStartDate(null),
                u.setMidDate(null), u.setEndDate(e), f.classList.add("is-end-date")), r.autoclose ? setTimeout(function () {
                    u.hide();
                }, 100) : (!r.singleDate || r.inline) && (i(u.el, r), setTimeout(function () {
                    u.hide();
                }, 100))) : r.startDate && !r.midDate && !r.endDate || r.startDate && r.midDate && !r.endDate || r.startDate && !r.midDate && r.endDate || !r.startDate && r.midDate && !r.endDate ? (r.TargetDPId == "txtDepartDate_0" ? (u.setStartDate(e),
                f.classList.add("is-start-date")) : r.TargetDPId == "txtDepartDate_1" ? (u.setMidDate(e),
                f.classList.add("is-mid-date")) : r.TargetDPId == "txtDepartDate_2" && (u.setEndDate(e),
                f.classList.add("is-end-date")), r.autoclose ? setTimeout(function () {
                    u.hide();
                }, 100) : i(u.el, r)) : r.startDate || r.midDate || !r.endDate ? !r.startDate && r.midDate && r.endDate && (r.TargetDPId == "txtDepartDate_0" && (u.setStartDate(e),
                f.classList.add("is-start-date")), r.TargetDPId == "txtDepartDate_1" ? (u.setStartDate(null),
                u.setMidDate(e), f.classList.add("is-mid-date")) : r.TargetDPId == "txtDepartDate_2" && (u.setStartDate(null),
                u.setEndDate(e), f.classList.add("is-end-date"))) : (r.TargetDPId == "txtDepartDate_0" && (u.setStartDate(e),
                f.classList.add("is-start-date")), r.TargetDPId == "txtDepartDate_1" ? (u.setStartDate(null),
                u.setMidDate(e), f.classList.add("is-mid-date")) : r.TargetDPId == "txtDepartDate_2" && (u.setStartDate(null),
                u.setMidDate(null), u.setEndDate(e), f.classList.add("is-end-date")));
                r.disabledDatesInRange || u.el.querySelectorAll(".lightpick__day.is-available").length === 0 && (u.setStartDate(null),
                i(u.el, r), r.footer && (typeof u._opts.onError == "function" ? u._opts.onError.call(u, "Invalid range") : (o = u.el.querySelector(".lightpick__footer-message"),
                o && (o.innerHTML = r.locale.not_allowed_range, setTimeout(function () {
                    o.innerHTML = "";
                }, 3e3)))));
            } else f.classList.contains("lightpick__previous-action") ? u.prevMonth() : f.classList.contains("lightpick__next-action") ? u.nextMonth() : f.classList.contains("lightpick__close-action") || f.classList.contains("lightpick__apply-action") ? u.hide() : f.classList.contains("lightpick__reset-action") && u.reset(); else if (f.classList.contains("lightpick__day") && f.classList.contains("is-available")) {
                if (e = n(parseInt(f.getAttribute("data-time"))), !r.disabledDatesInRange && r.disableDates && r.startDate) {
                    var s = e.isAfter(r.startDate) ? n(r.startDate) : n(e), h = e.isAfter(r.startDate) ? n(e) : n(r.startDate), c = r.disableDates.filter(function (t) {
                        if (t instanceof Array || Object.prototype.toString.call(t) === "[object Array]") {
                            var i = n(t[0]), r = n(t[1]);
                            return i.isValid() && r.isValid() && (i.isBetween(s, h, "day", "[]") || r.isBetween(s, h, "day", "[]"));
                        }
                        return n(t).isBetween(s, h, "day", "[]");
                    });
                    if (c.length) {
                        u.setStartDate(null);
                        u.setEndDate(null);
                        f.dispatchEvent(new Event("mousedown"));
                        u.el.querySelector(".lightpick__tooltip").style.visibility = "hidden";
                        i(u.el, r);
                        return;
                    }
                }
                r.singleDate || !r.startDate && !r.endDate || r.startDate && r.endDate ? r.repick && r.startDate && r.endDate ? (r.repickTrigger === r.field ? (u.setStartDate(e),
                f.classList.add("is-start-date")) : (u.setEndDate(e), f.classList.add("is-end-date")),
                r.startDate.isAfter(r.endDate), r.autoclose && setTimeout(function () {
                    u.hide();
                }, 100)) : r.repick && !r.startDate && r.endDate ? (r.TargetDPId == "txtDepartDate" && (u.setStartDate(e),
                f.classList.add("is-start-date")), r.TargetDPId == "tbCheckIn" && (u.setStartDate(e),
                f.classList.add("is-start-date")), r.singleDate && r.autoclose ? setTimeout(function () {
                    u.hide();
                }, 100) : (!r.singleDate || r.inline) && i(u.el, r)) : (r.TargetDPId == "txtDepartDate" ? (u.setStartDate(e),
                u.setEndDate(null), f.classList.add("is-start-date")) : r.TargetDPId == "txtReturnDate" && (u.setStartDate(null),
                u.setEndDate(e), f.classList.add("is-end-date")), r.TargetDPId == "tbCheckIn" ? (u.setStartDate(e),
                u.setEndDate(null), f.classList.add("is-start-date")) : r.TargetDPId == "tbCheckOut" && (u.setStartDate(null),
                u.setEndDate(e), f.classList.add("is-end-date")), r.singleDate && r.autoclose ? setTimeout(function () {
                    u.hide();
                }, 100) : (!r.singleDate || r.inline) && i(u.el, r)) : r.startDate && !r.endDate ? (r.TargetDPId == "txtDepartDate" ? (u.setStartDate(e),
                f.classList.add("is-start-date")) : r.TargetDPId == "txtReturnDate" && (u.setEndDate(e),
                f.classList.add("is-end-date")), r.TargetDPId == "tbCheckIn" ? (u.setStartDate(e),
                f.classList.add("is-start-date")) : r.TargetDPId == "tbCheckOut" && (u.setEndDate(e),
                f.classList.add("is-end-date")), r.startDate.isAfter(r.endDate), r.autoclose ? setTimeout(function () {
                    u.hide();
                }, 100) : i(u.el, r)) : !r.startDate && r.endDate && (r.TargetDPId == "txtDepartDate" ? (u.setStartDate(e),
                f.classList.add("is-start-date")) : r.TargetDPId == "txtReturnDate" && (u.setStartDate(null),
                u.setEndDate(e), f.classList.add("is-end-date")), r.TargetDPId == "tbCheckIn" ? (u.setStartDate(e),
                f.classList.add("is-start-date")) : r.TargetDPId == "tbCheckOut" && (u.setStartDate(null),
                u.setEndDate(e), f.classList.add("is-end-date")));
                r.disabledDatesInRange || u.el.querySelectorAll(".lightpick__day.is-available").length === 0 && (u.setStartDate(null),
                i(u.el, r), r.footer && (typeof u._opts.onError == "function" ? u._opts.onError.call(u, "Invalid range") : (o = u.el.querySelector(".lightpick__footer-message"),
                o && (o.innerHTML = r.locale.not_allowed_range, setTimeout(function () {
                    o.innerHTML = "";
                }, 3e3)))));
            } else f.classList.contains("lightpick__previous-action") ? u.prevMonth() : f.classList.contains("lightpick__next-action") ? u.nextMonth() : f.classList.contains("lightpick__close-action") || f.classList.contains("lightpick__apply-action") ? u.hide() : f.classList.contains("lightpick__reset-action") && u.reset();
        };
        u._onMouseEnter = function (t) {
            var r, i, f, o, e, h, s;
            if (u.isShowing && (t = t || window.event, r = t.target || t.srcElement, r) && (i = u._opts,
            !i.multiDate)) {
                if (r.classList.contains("lightpick__day") && r.classList.contains("disabled-tooltip") && i.locale.tooltipOnDisabled) {
                    u.showTooltip(r, i.locale.tooltipOnDisabled);
                    return;
                }
                if (u.hideTooltip(), i.singleDate || !i.startDate && !i.endDate) return;
                if (!r.classList.contains("lightpick__day") && !r.classList.contains("is-available")) return;
                if (i.startDate && !i.endDate || i.repick) {
                    if (f = n(parseInt(r.getAttribute("data-time"))), !f.isValid()) return;
                    o = i.startDate && !i.endDate || i.repick && i.repickTrigger === i.secondField ? i.startDate : i.endDate;
                    e = u.el.querySelectorAll(".lightpick__day");
                    [].forEach.call(e, function (t) {
                        var r = n(parseInt(t.getAttribute("data-time")));
                        t.classList.remove("is-flipped");
                        r.isValid() && r.isSameOrAfter(o, "day") && r.isSameOrBefore(f, "day") ? (t.classList.add("is-in-range"),
                        i.repickTrigger === i.field && r.isSameOrAfter(i.endDate) ? t.classList.remove("is-in-range") : i.repickTrigger !== i.field || i.endDate || t.classList.remove("is-in-range")) : r.isValid() && r.isSameOrAfter(f, "day") && r.isSameOrBefore(o, "day") ? (t.classList.add("is-in-range"),
                        (i.startDate && !i.endDate || i.repickTrigger === i.secondField) && r.isSameOrBefore(i.startDate) && t.classList.remove("is-in-range")) : t.classList.remove("is-in-range");
                        i.startDate && i.endDate && i.repick && i.repickTrigger === i.field ? t.classList.remove("is-start-date") : !i.startDate && i.endDate && i.repick && i.repickTrigger === i.field ? t.classList.remove("is-start-date") : t.classList.remove("is-end-date");
                    });
                    i.hoveringTooltip && (e = Math.abs(f.isAfter(o) ? f.diff(o, "day") : o.diff(f, "day")),
                    i.tooltipNights || (e += 1), h = u.el.querySelector(".lightpick__tooltip"), e > 0 && !r.classList.contains("is-disabled") ? (s = "",
                    typeof i.locale.pluralize == "function" && (s = i.locale.pluralize.call(u, e, i.locale.tooltip)),
                    u.showTooltip(r, e + " " + s)) : u.hideTooltip());
                    i.startDate && i.endDate && i.repick && i.repickTrigger === i.field ? r.classList.add("is-start-date") : !i.startDate && i.endDate && i.repick && i.repickTrigger === i.field ? r.classList.add("is-start-date") : r.classList.add("is-end-date");
                }
            }
        };
        u._onChange = function (n) {
            n = n || window.event;
            var t = n.target || n.srcElement;
            t && (t.classList.contains("lightpick__select-months") ? u.gotoMonth(t.value) : t.classList.contains("lightpick__select-years") && u.gotoYear(t.value));
        };
        u._onInputChange = function (n) {
            var t = n.target || n.srcElement;
            u._opts.singleDate && (u._opts.autoclose || u.gotoDate(e.field.value));
            u.syncFields();
            u.isShowing || u.show();
        };
        u._onInputFocus = function (i) {
            var r = i.target || i.srcElement;
            u._opts.multiDate ? (u.hide(), r.id == "txtDepartDate_0" ? u._opts.minDate = n() : r.id == "txtDepartDate_1" ? u._opts.minDate = t.getElementById("txtDepartDate_0").value != null && t.getElementById("txtDepartDate_0").value != "" ? n(t.getElementById("txtDepartDate_0").value) : n() : r.id == "txtDepartDate_2" && (u._opts.minDate = t.getElementById("txtDepartDate_1").value != null && t.getElementById("txtDepartDate_1").value != "" ? n(t.getElementById("txtDepartDate_1").value) : t.getElementById("txtDepartDate_0").value != null && t.getElementById("txtDepartDate_0").value != "" ? n(t.getElementById("txtDepartDate_0").value) : n())) : (u.hide(),
            r.id == "txtDepartDate" ? u._opts.minDate = n() : r.id == "txtReturnDate" && (u._opts.minDate = t.getElementById("txtDepartDate").value != null && t.getElementById("txtDepartDate").value != "" ? n(t.getElementById("txtDepartDate").value) : n()),
            r.id == "tbCheckIn" ? u._opts.minDate = n() : r.id == "tbCheckOut" && (u._opts.minDate = t.getElementById("tbCheckIn").value != null && t.getElementById("tbCheckIn").value != "" ? n(t.getElementById("tbCheckIn").value).add(1, "day") : n()));
            u._onInputClick(i);
        };
        u._onInputClick = function (n) {
            $("#divPassengerDDL").hide();
            $("#divClassTypeDDL").hide();
            $("#btnSearchFlights").removeAttr("disabled");
            var t = n.target || n.srcElement;
            u._opts.TargetDPId = t.id;
            setTimeout(function () {
                u.show(t);
            }, 10);
        };
        u._onClick = function (n) {
            n = n || window.event;
            var i = n.target || n.srcElement, t = i;
            if (i) if (e.multiDate) {
                do if (t.classList && t.classList.contains("lightpick") || t === e.field || e.midField && t === e.midField || e.secondField && t === e.secondField) return; while (t = t.parentNode);
                u.isShowing && e.hideOnBodyClick && i !== e.field && t !== e.field && u.hide();
            } else {
                do if (t.classList && t.classList.contains("lightpick") || t === e.field || e.secondField && t === e.secondField) return; while (t = t.parentNode);
                u.isShowing && e.hideOnBodyClick && i !== e.field && t !== e.field && u.hide();
            }
        };
        u.showTooltip = function (n, t) {
            var i = u.el.querySelector(".lightpick__tooltip"), h = u.el.classList.contains("lightpick--inlined"), r = n.getBoundingClientRect(), e = h ? u.el.parentNode.getBoundingClientRect() : u.el.getBoundingClientRect(), o = r.left - e.left + r.width / 2, s = r.top - e.top, f;
            i.style.visibility = "visible";
            i.textContent = t;
            f = i.getBoundingClientRect();
            s -= f.height;
            o -= f.width / 2;
            setTimeout(function () {
                i.style.top = s + "px";
                i.style.left = o + "px";
            }, 10);
        };
        u.hideTooltip = function () {
            var n = u.el.querySelector(".lightpick__tooltip");
            n.style.visibility = "hidden";
        };
        u.el.addEventListener("mousedown", u._onMouseDown, !0);
        u.el.addEventListener("mouseenter", u._onMouseEnter, !0);
        u.el.addEventListener("touchend", u._onMouseDown, !0);
        u.el.addEventListener("change", u._onChange, !0);
        e.inline ? u.show() : u.hide();
        e.field.addEventListener("change", u._onInputChange);
        e.field.addEventListener("click", u._onInputClick);
        e.field.addEventListener("focus", u._onInputFocus);
        e.multiDate && e.midField && (e.midField.addEventListener("change", u._onInputChange),
        e.midField.addEventListener("click", u._onInputClick), e.midField.addEventListener("focus", u._onInputFocus));
        e.secondField && (e.secondField.addEventListener("change", u._onInputChange), e.secondField.addEventListener("click", u._onInputClick),
        e.secondField.addEventListener("focus", u._onInputFocus));
    };
    return h.prototype = {
        config: function (t) {
            var i = $.extend({}, o, t), r;
            return i.field = i.field && i.field.nodeName ? i.field : null, i.calendar = [n().set("date", 1)],
            i.numberOfMonths === 1 && i.numberOfColumns > 1 && (i.numberOfColumns = 1), i.minDate = i.minDate && n(i.minDate).isValid() ? n(i.minDate) : null,
            i.maxDate = i.maxDate && n(i.maxDate).isValid() ? n(i.maxDate) : null, i.lang === "auto" && (r = navigator.language || navigator.userLanguage,
            i.lang = r ? r : "en-US"), i.secondField && i.singleDate && (i.singleDate = !1),
            i.hoveringTooltip && i.singleDate && (i.hoveringTooltip = !1), Object.prototype.toString.call(t.locale) === "[object Object]" && (i.locale = $.extend({}, o.locale, t.locale)),
            !i.repick || i.secondField || i.multiDate || (i.repick = !1), i.inline && (i.autoclose = !1,
            i.hideOnBodyClick = !1), this._opts = $.extend({}, i), this.syncFields(), this.setStartDate(this._opts.startDate, !0),
            this.setMidDate(this._opts.midDate, !0), this.setEndDate(this._opts.endDate, !0),
            this._opts;
        },
        syncFields: function () {
            var t;
            this._opts.multiDate ? this._opts.singleDate || this._opts.midField || this._opts.secondField ? (n(this._opts.field.value, this._opts.format).isValid() && (this._opts.startDate = n(this._opts.field.value, this._opts.format)),
            this._opts.midfield && n(this._opts.midfield.value, this._opts.format).isValid() && (this._opts.midDate = n(this._opts.midfield.value, this._opts.format)),
            this._opts.secondField && n(this._opts.secondField.value, this._opts.format).isValid() && (this._opts.endDate = n(this._opts.secondField.value, this._opts.format))) : (t = this._opts.field.value.split(this._opts.separator),
            t.length === 2 && (n(t[0], this._opts.format).isValid() && (this._opts.startDate = n(t[0], this._opts.format)),
            n(t[0], this._opts.format).isValid() && (this._opts.midDate = n(t[1], this._opts.format)),
            this._opts.secondField && n(t[1], this._opts.format).isValid() && (this._opts.endDate = n(t[1], this._opts.format)))) : this._opts.singleDate || this._opts.secondField ? (n(this._opts.field.value, this._opts.format).isValid() && (this._opts.startDate = n(this._opts.field.value, this._opts.format)),
            this._opts.secondField && n(this._opts.secondField.value, this._opts.format).isValid() && (this._opts.endDate = n(this._opts.secondField.value, this._opts.format))) : (t = this._opts.field.value.split(this._opts.separator),
            t.length === 2 && (n(t[0], this._opts.format).isValid() && (this._opts.startDate = n(t[0], this._opts.format)),
            n(t[1], this._opts.format).isValid() && (this._opts.endDate = n(t[1], this._opts.format))));
        },
        swapDate: function () {
            var t = n(this._opts.startDate);
            this.setDateRange(this._opts.endDate, t);
        },
        gotoToday: function () {
            this.gotoDate(new Date());
        },
        gotoDate: function (t) {
            var t = n(t);
            t.isValid() || (t = n());
            t.set("date", 1);
            this._opts.calendar = [n(t)];
            r(this.el, this._opts);
        },
        gotoMonth: function (n) {
            isNaN(n) || (this._opts.calendar[0].set("month", n), r(this.el, this._opts));
        },
        gotoYear: function (n) {
            isNaN(n) || (this._opts.calendar[0].set("year", n), r(this.el, this._opts));
        },
        prevMonth: function () {
            this._opts.calendar[0] = n(this._opts.calendar[0]).subtract(1, "month");
            r(this.el, this._opts);
            e(this.el, this._opts);
        },
        nextMonth: function () {
            this._opts.calendar[0] = n(this._opts.calendar[0]).add(1, "month");
            r(this.el, this._opts);
            e(this.el, this._opts);
        },
        updatePosition: function () {
            var e;
            if (!this.el.classList.contains("lightpick--inlined")) {
                if (this.el.classList.remove("is-hidden"), this._opts.multiDate) {
                    if (this._opts.TargetDPId == "txtDepartDate_0") var i = this._opts.field.getBoundingClientRect(), r = this.el.getBoundingClientRect(), n = this._opts.orientation.split(" "), u = 0, f = 0; else if (this._opts.TargetDPId == "txtDepartDate_1") var i = this._opts.midField.getBoundingClientRect(), r = this.el.getBoundingClientRect(), n = this._opts.orientation.split(" "), u = 0, f = 0; else if (this._opts.TargetDPId == "txtDepartDate_2") var i = this._opts.secondField.getBoundingClientRect(), r = this.el.getBoundingClientRect(), n = this._opts.orientation.split(" "), u = 0, f = 0;
                } else {
                    if (this._opts.TargetDPId == "txtDepartDate") var i = this._opts.field.getBoundingClientRect(), r = this.el.getBoundingClientRect(), n = this._opts.orientation.split(" "), u = 0, f = -100; else if (this._opts.TargetDPId == "txtReturnDate") var i = this._opts.secondField.getBoundingClientRect(), r = this.el.getBoundingClientRect(), n = this._opts.orientation.split(" "), u = 0, f = 0;
                    if (this._opts.TargetDPId == "tbCheckIn") var i = this._opts.field.getBoundingClientRect(), r = this.el.getBoundingClientRect(), n = this._opts.orientation.split(" "), u = 0, f = -100; else if (this._opts.TargetDPId == "tbCheckOut") var i = this._opts.secondField.getBoundingClientRect(), r = this.el.getBoundingClientRect(), n = this._opts.orientation.split(" "), u = 0, f = 0;
                }
                n[0] != "auto" && /top|bottom/.test(n[0]) ? (u = i[n[0]] + window.pageYOffset, n[0] == "top" && (u -= r.height)) : u = i.bottom + r.height > window.innerHeight && window.pageYOffset > r.height ? i.top + window.pageYOffset - r.height : i.bottom + window.pageYOffset;
                /left|right/.test(n[0]) || n[1] && n[1] != "auto" && /left|right/.test(n[1]) ? (f = /left|right/.test(n[0]) ? i[n[0]] + window.pageXOffset : i[n[1]] + window.pageXOffset,
                (n[0] == "right" || n[1] == "right") && (f -= r.width)) : f = i.left + r.width > window.innerWidth ? i.right + window.pageXOffset - r.width : i.left + window.pageXOffset;
                this.el.classList.add("is-hidden");
                e = t.getElementById("hdnSearchPageType");
                e != null && e.value == "MDF" && (u = u - window.pageYOffset);
                this.el.style.top = u + "px";
                this.el.style.left = f + "px";
                this.el.setAttribute("data-top", u);
                this.el.setAttribute("data-left", f);
            }
        },
        setStartDate: function (t, i) {
            var r = n(t, n.ISO_8601), u = n(t, this._opts.format);
            if (!r.isValid() && !u.isValid()) {
                this._opts.startDate = null;
                this._opts.field.value = "";
                return;
            }
            this._opts.startDate = n(r.isValid() ? r : u);
            this._opts.field.value = this._opts.singleDate || this._opts.secondField ? this._opts.startDate.format(this._opts.format) : this._opts.startDate.format(this._opts.format);
            this._opts.multiDate ? i || typeof this._opts.onSelect != "function" || this._opts.onSelect.call(this, this.getStartDate(), this.getMidDate(), this.getEndDate()) : i || typeof this._opts.onSelect != "function" || this._opts.onSelect.call(this, this.getStartDate(), this.getEndDate());
        },
        setMidDate: function (t, i) {
            var r = n(t, n.ISO_8601), u = n(t, this._opts.format);
            if (!r.isValid() && !u.isValid()) {
                this._opts.midDate = null;
                this._opts.midField ? this._opts.midField.value = "" : !this._opts.singleDate && this._opts.startDate && (this._opts.field.value = this._opts.startDate.format(this._opts.format));
                return;
            }
            this._opts.midDate = n(r.isValid() ? r : u);
            this._opts.midField ? (this._opts.startDate && (this._opts.field.value = this._opts.startDate.format(this._opts.format)),
            this._opts.midField.value = this._opts.midDate.format(this._opts.format)) : this._opts.field.value = this._opts.startDate.format(this._opts.format) + this._opts.separator + this._opts.midDate.format(this._opts.format);
            i || typeof this._opts.onSelect != "function" || this._opts.onSelect.call(this, this.getStartDate(), this.getMidDate(), this.getEndDate());
        },
        setEndDate: function (t, i) {
            var r = n(t, n.ISO_8601), u = n(t, this._opts.format);
            if (!r.isValid() && !u.isValid()) {
                this._opts.endDate = null;
                this._opts.secondField ? this._opts.secondField.value = "" : !this._opts.singleDate && this._opts.startDate && (this._opts.field.value = this._opts.startDate.format(this._opts.format));
                return;
            }
            this._opts.endDate = n(r.isValid() ? r : u);
            this._opts.secondField ? (this._opts.startDate && (this._opts.field.value = this._opts.startDate.format(this._opts.format)),
            this._opts.secondField.value = this._opts.endDate.format(this._opts.format)) : this._opts.field.value = this._opts.startDate.format(this._opts.format) + this._opts.separator + this._opts.endDate.format(this._opts.format);
            this._opts.multiDate ? i || typeof this._opts.onSelect != "function" || this._opts.onSelect.call(this, this.getStartDate(), this.getMidDate(), this.getEndDate()) : i || typeof this._opts.onSelect != "function" || this._opts.onSelect.call(this, this.getStartDate(), this.getEndDate());
        },
        setDate: function (n, t) {
            this._opts.singleDate && (this.setStartDate(n, t), this.isShowing && i(this.el, this._opts));
        },
        setDateRange: function (n, t, r) {
            this._opts.singleDate || (this.setStartDate(n, !0), this.setEndDate(t, !0), this.isShowing && i(this.el, this._opts),
            r || typeof this._opts.onSelect != "function" || this._opts.onSelect.call(this, this.getStartDate(), this.getEndDate()));
        },
        setDisableDates: function (n) {
            this._opts.disableDates = n;
            this.isShowing && i(this.el, this._opts);
        },
        getStartDate: function () {
            return n(this._opts.startDate).isValid() ? this._opts.startDate : null;
        },
        getMidDate: function () {
            return n(this._opts.midDate).isValid() ? this._opts.midDate : null;
        },
        getEndDate: function () {
            return n(this._opts.endDate).isValid() ? this._opts.endDate : null;
        },
        getDate: function () {
            return n(this._opts.startDate).isValid() ? this._opts.startDate : null;
        },
        toString: function (t) {
            return this._opts.singleDate ? n(this._opts.startDate).isValid() ? this._opts.startDate.format(t) : "" : n(this._opts.startDate).isValid() && n(this._opts.endDate).isValid() ? this._opts.startDate.format(t) : n(this._opts.startDate).isValid() && !n(this._opts.endDate).isValid() ? this._opts.startDate.format(t) : !n(this._opts.startDate).isValid() && n(this._opts.endDate).isValid() ? this._opts.endDate.format(t) : "";
        },
        show: function (i) {
            if (this._opts.multiDate) this.isShowing || (this.isShowing = !0, this._opts.repick && (this._opts.repickTrigger = i),
            this.syncFields(), this._opts.secondField && this._opts.secondField === i && this._opts.endDate ? this.gotoDate(this._opts.midDate) : this._opts.secondField && this._opts.secondField === i ? this.gotoDate(this._opts.midDate) : this._opts.midField && this._opts.midField === i && this._opts.midDate ? this.gotoDate(this._opts.startDate) : this.gotoDate(this._opts.startDate),
            t.addEventListener("click", this._onClick), this.updatePosition(), this.el.classList.remove("is-hidden"),
            typeof this._opts.onOpen == "function" && this._opts.onOpen.call(this), t.activeElement && t.activeElement != t.body && t.activeElement.blur()),
            $(".lightpick_selecteddate_Separator").hide(), $(".lightpick_selecteddate_To_Date").hide(),
            $(".lightpick_selecteddate_From_Date").addClass("selecteddate_highlighter"), this._opts.field === i && (this._opts.startDate != null ? $(".lightpick_selecteddate_From_Date").html(n(this._opts.startDate).format("ddd DD MMM, YY")) : $(".lightpick_selecteddate_From_Date").html(n().format("ddd DD MMM, YY"))),
            this._opts.midField === i && (this._opts.midDate != null ? $(".lightpick_selecteddate_From_Date").html(n(this._opts.midDate).format("ddd DD MMM, YY")) : this._opts.startDate != null ? $(".lightpick_selecteddate_From_Date").html(n(this._opts.startDate).format("ddd DD MMM, YY")) : $(".lightpick_selecteddate_From_Date").html(n().format("ddd DD MMM, YY"))),
            this._opts.secondField === i && (this._opts.endDate != null ? $(".lightpick_selecteddate_From_Date").html(n(this._opts.endDate).format("ddd DD MMM, YY")) : this._opts.midDate != null ? $(".lightpick_selecteddate_From_Date").html(n(this._opts.midDate).format("ddd DD MMM, YY")) : this._opts.startDate != null ? $(".lightpick_selecteddate_From_Date").html(n(this._opts.startDate).format("ddd DD MMM, YY")) : $(".lightpick_selecteddate_From_Date").html(n().format("ddd DD MMM, YY"))); else {
                this.isShowing || (this.isShowing = !0, this._opts.repick && (this._opts.repickTrigger = i),
                this.syncFields(), this._opts.secondField && this._opts.secondField === i && this._opts.endDate ? this.gotoDate(this._opts.startDate) : this.gotoDate(this._opts.startDate),
                t.addEventListener("click", this._onClick), this.updatePosition(), this.el.classList.remove("is-hidden"),
                typeof this._opts.onOpen == "function" && this._opts.onOpen.call(this), t.activeElement && t.activeElement != t.body && t.activeElement.blur());
                $(".lightpick_selecteddate_Separator").show();
                $(".lightpick_selecteddate_To_Date").show();
                var u = '<span class="lightpick_selecteddate_From_Date_Header">Depart</span>', r = '<span class="lightpick_selecteddate_To_Date_Header">Return</span>';
                (this._opts.TargetDPId == "tbCheckIn" || this._opts.TargetDPId == "tbCheckOut") && (u = '<span class="lightpick_selecteddate_From_Date_Header">Check-in</span>',
                r = '<span class="lightpick_selecteddate_To_Date_Header">Check-out</span>');
                this._opts.field === i ? ($(".lightpick_selecteddate_From_Date").addClass("selecteddate_highlighter"),
                $(".lightpick_selecteddate_To_Date").removeClass("selecteddate_highlighter")) : ($(".lightpick_selecteddate_From_Date").removeClass("selecteddate_highlighter"),
                $(".lightpick_selecteddate_To_Date").addClass("selecteddate_highlighter"));
                this._opts.startDate != null ? $(".lightpick_selecteddate_From_Date").html(u + n(this._opts.startDate).format("ddd DD MMM, YY")) : $(".lightpick_selecteddate_From_Date").html(u + n().format("ddd DD MMM, YY"));
                this._opts.singleDate == !1 && this._opts.endDate != null ? $(".lightpick_selecteddate_To_Date").html(r + n(this._opts.endDate).format("ddd DD MMM, YY")) : this._opts.singleDate == !1 && this._opts.endDate == null ? this._opts.startDate != null ? $(".lightpick_selecteddate_To_Date").html(r + n(this._opts.startDate).format("ddd DD MMM, YY")) : $(".lightpick_selecteddate_To_Date").html(r + n().format("ddd DD MMM, YY")) : ($(".lightpick_selecteddate_To_Date").html(""),
                $(".lightpick_selecteddate_To_Date").hide(), $(".lightpick_selecteddate_Separator").hide());
            }
        },
        hide: function () {
            this.isShowing && (this.isShowing = !1, t.removeEventListener("click", this._onClick),
            this.el.classList.add("is-hidden"), this.el.querySelector(".lightpick__tooltip").style.visibility = "hidden",
            typeof this._opts.onClose == "function" && this._opts.onClose.call(this));
        },
        destroy: function () {
            var n = this._opts;
            this.hide();
            this.el.removeEventListener("mousedown", self._onMouseDown, !0);
            this.el.removeEventListener("mouseenter", self._onMouseEnter, !0);
            this.el.removeEventListener("touchend", self._onMouseDown, !0);
            this.el.removeEventListener("change", self._onChange, !0);
            n.field.removeEventListener("change", this._onInputChange);
            n.field.removeEventListener("click", this._onInputClick);
            n.field.removeEventListener("focus", this._onInputFocus);
            n.midField && (n.midField.removeEventListener("change", this._onInputChange), n.midField.removeEventListener("click", this._onInputClick),
            n.midField.removeEventListener("focus", this._onInputFocus));
            n.secondField && (n.secondField.removeEventListener("change", this._onInputChange),
            n.secondField.removeEventListener("click", this._onInputClick), n.secondField.removeEventListener("focus", this._onInputFocus));
            this.el.parentNode && this.el.parentNode.removeChild(this.el);
        },
        reset: function () {
            this.setStartDate(null, !0);
            this.setMidDate(null, !0);
            this.setEndDate(null, !0);
            i(this.el, this._opts);
            this._opts.multiDate ? typeof this._opts.onSelect == "function" && this._opts.onSelect.call(this, this.getStartDate(), this.getMidDate(), this.getEndDate()) : typeof this._opts.onSelect == "function" && this._opts.onSelect.call(this, this.getStartDate(), this.getEndDate());
            this.el.querySelector(".lightpick__tooltip").style.visibility = "hidden";
        },
        reloadOptions: function (n) {
            this._opts = $.extend({}, this._opts, n);
        }
    }, h;
});

$(window).scroll(function () {
    var n = document.getElementById("hdnSearchPageType"), t, i;
    n != null && n.value == "MDF" && (t = $(".lightpick--2-columns").attr("data-top"),
    i = $(".lightpick--2-columns").attr("data-left"), $(".lightpick--2-columns").css("top", parseInt(t)),
    $(".lightpick--2-columns").css("left", parseInt(i)), $(".lightpick--2-columns").css("position", "fixed"));
});

$(".m-loca input").each(function () {
    CheckForInputText(this);
});

$(".m-loca input").on("change keyup", function () {
    CheckForInputText(this);
});

$(".cityName input").each(function () {
    CheckForInputText(this);
});

$(".cityName input").on("change keyup", function () {
    CheckForInputText(this);
});

$(document).ready(function () {
    $(".demo-label").click(function () {
        $(this).closest(".m-loca").find("input").val("");
        $(this).closest(".m-loca").find("span[class='spanCity']").text("Airport/City Name");
        $(this).closest(".m-loca").find(".demo-label").removeClass("input-has-value");
        $(this).closest(".cityName").find("input").val("");
        $(this).closest(".cityName").find("span[class='spanCity']").text("Airport/City Name");
        $(this).closest(".cityName").find(".demo-label").removeClass("input-has-value");
        $(this).hide();
        $(this).closest(".m-loca").find("span[class='spanHotelCountry']").text("Country Name");
    });
    $("#txtOriginCode").val().length ? $("#txtOriginCode").next().show() : $("#txtOriginCode").next().hide();
    $("#txtDestCode").val().length ? $("#txtDestCode").next().show() : $("#txtDestCode").next().hide();
    $("#txtOriginCode_0").val().length ? $("#txtOriginCode_0").next().show() : $("#txtOriginCode_0").next().hide();
    $("#txtDestCode_0").val().length ? $("#txtDestCode_0").next().show() : $("#txtDestCode_0").next().hide();
    $("#txtOriginCode_1").val().length ? $("#txtOriginCode_1").next().show() : $("#txtOriginCode_1").next().hide();
    $("#txtDestCode_1").val().length ? $("#txtDestCode_1").next().show() : $("#txtDestCode_1").next().hide();
    $("#txtOriginCode_2").val().length ? $("#txtOriginCode_2").next().show() : $("#txtOriginCode_2").next().hide();
    $("#txtDestCode_2").val().length ? $("#txtDestCode_2").next().show() : $("#txtDestCode_2").next().hide();
    $(".iexchangeIcon").click(function () {
        var n = document.getElementById("txtOriginCode").value, t = document.getElementById("hdnOriginCode").value, i = document.getElementById("spanOriginCityName").innerHTML, r = document.getElementById("txtDestCode").value, u = document.getElementById("hdnDestinationCode").value, f = document.getElementById("spanDestCityName").innerHTML;
        document.getElementById("txtOriginCode").value = r;
        document.getElementById("hdnOriginCode").value = u;
        document.getElementById("spanOriginCityName").innerHTML = f;
        document.getElementById("txtDestCode").value = n;
        document.getElementById("hdnDestinationCode").value = t;
        document.getElementById("spanDestCityName").innerHTML = i;
        document.getElementById("txtOriginCode_0").value = r;
        document.getElementById("hdnOriginCode_0").value = u;
        document.getElementById("spanOriginCityName_0").innerHTML = f;
        document.getElementById("txtDestCode_0").value = n;
        document.getElementById("hdnDestinationCode_0").value = t;
        document.getElementById("spanDestCityName_0").innerHTML = i;
        $("#spnOriginErrMsg").hide();
        $("#spnDestErrMsg").hide();
    });
    $("#rdoCabin1").click(function () {
        GetCabinTypeForDesktop(1, "Economy", this);
        $("#divClassTypeDDL").hide();
    });
    $("#rdoCabin2").click(function () {
        GetCabinTypeForDesktop(2, "Premium Economy", this);
        $("#divClassTypeDDL").hide();
    });
    $("#rdoCabin3").click(function () {
        GetCabinTypeForDesktop(3, "Business", this);
        $("#divClassTypeDDL").hide();
    });
    $("#rdoCabin4").click(function () {
        GetCabinTypeForDesktop(4, "First", this);
        $("#divClassTypeDDL").hide();
    });
    $("#txtPreferredAirlines").autocomplete({
        autoFocus: !0,
        minLength: 2,
        source: function (n, t) {
            var i = document.getElementById("txtPreferredAirlines").value;
            $.ajax({
                type: "POST",
                global: !1,
                contentType: "application/json;",
                url: "../Custom/GetAirlineListSuggestion",
                data: "{searchCode:'" + i + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (n) {
                    var i = JSON.parse(n);
                    t(i);
                },
                error: function () { }
            });
        },
        focus: function () {
            return !1;
        },
        change: function () {
            return $("#txtPreferredAirlines").val().trim() == "" && $("#hdnPreferredAirline").val(""),
            !1;
        },
        blur: function (n, t) {
            return $("#txtPreferredAirlines").val(t.item.Name), $("#hdnPreferredAirline").val(t.item.Code),
            !1;
        },
        select: function (n, t) {
            return $("#txtPreferredAirlines").val(t.item.Name), $("#hdnPreferredAirline").val(t.item.Code),
            !1;
        }
    }).autocomplete("instance")._renderItem = function (n, t) {
        var i = t.Name + " (" + t.Code + ")", r = i;
        return $("<li class='airlineList'>").append(" ").append(r).appendTo(n);
    };
    $("#liAdvanceSearchOption").on("click", function () {
        $(this).parents("ul").next().slideToggle();
        $("#divPassengerDDL,#divClassTypeDDL").toggleClass("pasengerChange");
        $(this).text($(this).text() == "Advanced Search Option (-)" ? "Advanced Search Option (+)" : "Advanced Search Option (-)");
    });
    $("#txtOriginCode").keyup(function () {
        $(this).val().length <= 1 && ($("#spanOriginCityName").text("Airport/City Name"),
        $("#hdnOriginCode").val(""), $("#spanOriginCityName_0").text("Airport/City Name"),
        $("#hdnOriginCode_0").val(""));
        $("#spnOriginErrMsg").hide();
    });
    $("#txtDestCode").keyup(function () {
        $(this).val().length <= 1 && ($("#spanDestCityName").text("Airport/City Name"),
        $("#hdnDestinationCode").val(""), $("#spanDestCityName_0").text("Airport/City Name"),
        $("#hdnDestinationCode_0").val(""));
        $("#spnDestErrMsg").hide();
    });
    $("#txtOriginCode_0").on("keyup", function () {
        $(this).val().length <= 1 && ($("#spanOriginCityName_0").text("Airport/City Name"),
        $("#hdnOriginCode_0").val(""), $("#spanOriginCityName").text("Airport/City Name"),
        $("#hdnOriginCode").val(""));
        $("#spnOriginErrMsg_0").hide();
    });
    $("#txtDestCode_0").on("keyup", function () {
        $(this).val().length <= 1 && ($("#spanDestCityName_0").text("Airport/City Name"),
        $("#hdnDestinationCode_0").val(""), $("#spanDestCityName").text("Airport/City Name"),
        $("#hdnDestinationCode").val(""));
        $("#spnDestErrMsg_0").hide();
    });
    $("#txtOriginCode_1").on("keyup", function () {
        $(this).val().length <= 1 && ($("#spanOriginCityName_1").text("Airport/City Name"),
        $("#hdnOriginCode_1").val(""));
        $("#spnOriginErrMsg_1").hide();
    });
    $("#txtDestCode_1").on("keyup", function () {
        $(this).val().length <= 1 && ($("#spanDestCityName_1").text("Airport/City Name"),
        $("#hdnDestinationCode_1").val(""));
        $("#spnDestErrMsg_1").hide();
    });
    $("#txtOriginCode_2").on("keyup", function () {
        $(this).val().length <= 1 && ($("#spanOriginCityName_2").text("Airport/City Name"),
        $("#hdnOriginCode_2").val(""));
        $("#spnOriginErrMsg_2").hide();
    });
    $("#txtDestCode_2").on("keyup", function () {
        $(this).val().length <= 1 && ($("#spanDestCityName_2").text("Airport/City Name"),
        $("#hdnDestinationCode_2").val(""));
        $("#spnDestErrMsg_2").hide();
    });
    $(".ui-autocomplete-input").focus(function () {
        $(".pasenger-popup").css("display", "none");
    });
    $("#AddSection_2").on("click", function () {
        $("#hdnDestinationCode_1").val() != "" && $("#txtDestCode_1").val() != "" && ($("#txtOriginCode_2").val($("#txtDestCode_1").val()),
        $("#spanOriginCityName_2").text($("#spanDestCityName_1").text()), $("#hdnOriginCode_2").val($("#hdnDestinationCode_1").val()),
        $("#txtOriginCode_2").next().show());
        $("#txtDestCode_2").val("");
        $("#spanDestCityName_2").text("Airport/City Name");
        $("#hdnDestinationCode_2").val("");
        $("#txtDestCode_2").next().hide();
        $("#txtDepartDate_2").val("");
        $("#MC_Sector_3").css("display", "flex");
        $("#AddSection_2").css("display", "none");
        $("#RemoveSection_2").show();
        $("#hdnMC_Sec_2").val("1");
    });
    $("#txtClassType").click(function () {
        $("#divClassTypeDDL").toggle();
        $("#divPassengerDDL").css("display", "none");
    });
    $("#txtClassType_MC").click(function () {
        $("#divClassTypeDDL").toggle();
        $("#divPassengerDDL").css("display", "none");
    });
    $("#txtPassengers").click(function () {
        $("#divPassengerDDL").toggle();
        $("#divClassTypeDDL").css("display", "none");
    });
    $("#txtPassengers_MC").click(function () {
        $("#divPassengerDDL").toggle();
        $("#divClassTypeDDL").css("display", "none");
    });
    $("#btnPassengerDone").click(function () {
        $("#divPassengerDDL").hide();
    });
    $("#BtnSearchFare_RTOW").click(function (n) {
        var r = window.location.hostname.toLowerCase(), t = !0, i;
        return $("#txtOriginCode").val() == "" ? ($("#hdnOriginCode").val(""), t = !1, $("#spnOriginErrMsg").text("Please select origin"),
        $("#spnOriginErrMsg").show(), $("#txtOriginCode").focus()) : $("#hdnOriginCode").val() == "" && (t = !1,
        $("#spnOriginErrMsg").text("Please enter valid origin"), $("#spnOriginErrMsg").show(),
        $("#txtOriginCode").focus()), t && ($("#txtDestCode").val() == "" ? (t = !1, $("#hdnDestinationCode").val(""),
        $("#spnDestErrMsg").text("Please select destination"), $("#spnDestErrMsg").show(),
        $("#txtDestCode").focus()) : $("#hdnDestinationCode").val() == "" && (t = !1, $("#spnDestErrMsg").text("Please enter valid destination"),
        $("#spnDestErrMsg").show(), $("#txtDestCode").focus())), t && $("#hdnOriginCode").val() != "" && $("#hdnDestinationCode").val() != "" && ($("#hdnOriginCode").val() == $("#hdnDestinationCode").val() ? (t = !1,
        $("#spnDestErrMsg").text("Origin and destination can not be same"), $("#spnDestErrMsg").show(),
        $("#txtDestCode").focus()) : t = !0), t && $("#txtDepartDate").val() == "" && (t = !1,
        $("#spnDepDateErrMsg").show(), $("#txtDepartDate").focus()), $("#hdnTripTypeCode").val() === "1" && t && $("#txtReturnDate").val() == "" && (t = !1,
        $("#spnRetDateErrMsg").show(), $("#txtReturnDate").focus()), t && ($("#BtnSearchFare_RTOW").attr("disabled", "disabled").text("Searching..."),
        i = $("#formFlightSearchEngine").serialize(), $.post("/Search/AirSearch/", i, function (n) {
            n == "Success" ? window.parent ? window.parent.location = "/air/listing/" + $("#hdnFlightUniqueCode").val() : window.location = "/air/listing/" + $("#hdnFlightUniqueCode").val() : window.parent ? window.parent.location = window.parent.location : window.location = window.location;
        })), n.stopPropagation(), n.preventDefault(), !1;
    });
    $("#BtnSearchFare_MC").click(function (n) {
        var h = window.location.hostname.toLowerCase(), t = !0, u, f, e, o;
        if ($("#txtOriginCode_0").val() == "" ? ($("#hdnOriginCode_0").val(""), t = !1,
        $("#spnOriginErrMsg_0").text("Please select origin"), $("#spnOriginErrMsg_0").show(),
        $("#txtOriginCode_0").focus()) : $("#hdnOriginCode_0").val() == "" ? (t = !1, $("#spnOriginErrMsg_0").text("Please enter valid origin"),
        $("#spnOriginErrMsg_0").show(), $("#txtOriginCode_0").focus()) : $("#txtDestCode_0").val() == "" ? (t = !1,
        $("#hdnDestinationCode_0").val(""), $("#spnDestErrMsg_0").text("Please select destination"),
        $("#spnDestErrMsg_0").show(), $("#txtDestCode_0").focus()) : $("#hdnDestinationCode_0").val() == "" ? (t = !1,
        $("#spnDestErrMsg_0").text("Please enter valid destination"), $("#spnDestErrMsg_0").show(),
        $("#txtDestCode_0").focus()) : $("#txtOriginCode_1").val() == "" ? ($("#hdnOriginCode_1").val(""),
        t = !1, $("#spnOriginErrMsg_1").text("Please select origin"), $("#spnOriginErrMsg_1").show(),
        $("#txtOriginCode_1").focus()) : $("#hdnOriginCode_1").val() == "" ? (t = !1, $("#spnOriginErrMsg_1").text("Please enter valid origin"),
        $("#spnOriginErrMsg_1").show(), $("#txtOriginCode_1").focus()) : $("#txtDestCode_1").val() == "" ? (t = !1,
        $("#hdnDestinationCode_1").val(""), $("#spnDestErrMsg_1").text("Please select destination"),
        $("#spnDestErrMsg_1").show(), $("#txtDestCode_1").focus()) : $("#hdnDestinationCode_1").val() == "" ? (t = !1,
        $("#spnDestErrMsg_1").text("Please enter valid destination"), $("#spnDestErrMsg_1").show(),
        $("#txtDestCode_1").focus()) : $("#hdnMC_Sec_2").val() == "1" && $("#txtOriginCode_2").val() == "" ? ($("#hdnOriginCode_2").val(""),
        t = !1, $("#spnOriginErrMsg_2").text("Please select origin"), $("#spnOriginErrMsg_2").show(),
        $("#txtOriginCode_2").focus()) : $("#hdnMC_Sec_2").val() == "1" && $("#hdnOriginCode_2").val() == "" ? (t = !1,
        $("#spnOriginErrMsg_2").text("Please enter valid origin"), $("#spnOriginErrMsg_2").show(),
        $("#txtOriginCode_2").focus()) : $("#hdnMC_Sec_2").val() == "1" && $("#txtDestCode_2").val() == "" ? (t = !1,
        $("#hdnDestinationCode_2").val(""), $("#spnDestErrMsg_2").text("Please select destination"),
        $("#spnDestErrMsg_2").show(), $("#txtDestCode_2").focus()) : $("#hdnMC_Sec_2").val() == "1" && $("#hdnDestinationCode_2").val() == "" && (t = !1,
        $("#spnDestErrMsg_2").text("Please enter valid destination"), $("#spnDestErrMsg_2").show(),
        $("#txtDestCode_2").focus()), t && ($("#hdnOriginCode_0").val() == $("#hdnDestinationCode_0").val() ? (t = !1,
        $("#spnDestErrMsg_0").text("Origin and destination can not be same"), $("#spnDestErrMsg_0").show(),
        $("#txtDestCode_0").focus()) : $("#hdnOriginCode_1").val() == $("#hdnDestinationCode_1").val() ? (t = !1,
        $("#spnDestErrMsg_1").text("Origin and destination can not be same"), $("#spnDestErrMsg_1").show(),
        $("#txtDestCode_1").focus()) : $("#hdnMC_Sec_2").val() == "1" && $("#hdnOriginCode_2").val() == $("#hdnDestinationCode_2").val() ? (t = !1,
        $("#spnDestErrMsg_2").text("Origin and destination can not be same"), $("#spnDestErrMsg_2").show(),
        $("#txtDestCode_2").focus()) : t = !0), t && ($("#txtDepartDate_0").val() == "" && (t = !1,
        $("#spnDepDateErrMsg_0").show(), $("#txtDepartDate_0").focus()), $("#txtDepartDate_1").val() == "" && (t = !1,
        $("#spnDepDateErrMsg_1").show(), $("#txtDepartDate_1").focus()), $("#hdnMC_Sec_2").val() == "1" && $("#txtDepartDate_2").val() == "" && (t = !1,
        $("#spnDepDateErrMsg_2").show(), $("#txtDepartDate_2").focus())), t) {
            $("#BtnSearchFare_MC").attr("disabled", "disabled").text("Searching...");
            var s = $("#formFlightSearchEngine").serialize(), i = "0", r = "MultiAirSearch";
            i = checkMulticityGetRT();
            i == "1" && (r = "AirSearch", u = $("#hdnOriginCode_0").val(), $("#hdnOriginCode").val(u),
            f = $("#hdnDestinationCode_0").val(), $("#hdnDestinationCode").val(f), e = $("#txtDepartDate_0").val(),
            $("#txtDepartDate").val(e), o = $("#txtDepartDate_1").val(), $("#txtReturnDate").val(o),
            $("#hdnTripTypeCode").val("1"));
            $.post("/Search/" + r + "/", s, function (n) {
                n == "Success" ? window.parent ? window.parent.location = "/air/listing/" + $("#hdnFlightUniqueCode").val() : window.location = "/air/listing/" + $("#hdnFlightUniqueCode").val() : window.parent ? window.parent.location = window.parent.location : window.location = window.location;
            });
        }
        return n.stopPropagation(), n.preventDefault(), !1;
    });
    $("select#LstCabinClass").change(function () {
        var n = $("#LstCabinClass option:selected").text(), t = $("#txtAdultPassenger").val(), i = $("#txtChildPassenger").val(), r = $("#txtInfantSeatPassenger").val(), u = $("#txtInfantPassenger").val(), f = parseInt(t) + parseInt(i) + parseInt(r) + parseInt(u);
        $("#txtClassType,#txtClassType_MC").val(n);
        $("#txtPassengers,#txtPassengers_MC").val(f + " Passenger(s)");
    });
});

var AutoSuggest_AC, AutoSuggest_CIN, AutoSuggest_CON, AutoSuggest_Target, AutoSuggest_Counter = 0;

$(document).ready(function () {
    $("#txtOriginCode").autocomplete({
        autoFocus: !0,
        minLength: 3,
        source: function (n, t) {
            var i = document.getElementById("txtOriginCode").value.trim();
            $.ajax({
                type: "POST",
                global: !1,
                url: "/Flight/GetAutoSuggestion",
                data: "{searchCode:'" + i + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (n) {
                    if (AutoSuggest_Counter = 0, n.toString().indexOf("Invalid") == -1) {
                        var i = JSON.parse(n);
                        t(i);
                    }
                },
                error: function () { }
            });
        },
        focus: function () {
            return !1;
        },
        blur: function (n, t) {
            return $("#txtOriginCode").val(t.item.AC), $("#spanOriginCityName").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode").val(t.item.AC), $("#txtOriginCode_0").val(t.item.AC), $("#spanOriginCityName_0").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode_0").val(t.item.AC), !1;
        },
        select: function (n, t) {
            return $("#txtOriginCode").val(t.item.AC), $("#spanOriginCityName").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode").val(t.item.AC), $("#txtOriginCode_0").val(t.item.AC), $("#spanOriginCityName_0").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode_0").val(t.item.AC), n.key != "Tab" && $("#txtDestCode").focus(),
            !1;
        },
        close: function () {
            if ($("#hdnOriginCode").val().trim() == "" && AutoSuggest_Target == "txtOriginCode" && $("#txtOriginCode").val().length > 2 && AutoSuggest_AC != "") return $("#txtOriginCode").val(AutoSuggest_AC),
            $("#spanOriginCityName").text(AutoSuggest_CIN + ", " + AutoSuggest_CON), $("#hdnOriginCode").val(AutoSuggest_AC),
            $("#txtOriginCode_0").val(AutoSuggest_AC), $("#spanOriginCityName_0").text(AutoSuggest_CIN + ", " + AutoSuggest_CON),
            $("#hdnOriginCode_0").val(AutoSuggest_AC), !1;
        }
    }).autocomplete("instance")._renderItem = function (n, t) {
        var i, r, u;
        return AutoSuggest_Counter == 0 && ($("#hdnOriginCode").val(""), AutoSuggest_Target = "txtOriginCode",
        AutoSuggest_AC = t.AC, AutoSuggest_CIN = t.CIN, AutoSuggest_CON = t.CON), AutoSuggest_Counter = 1,
        i = "", i = t.COA == null || t.COA == undefined || t.COA == "" ? t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + ", " + t.Country + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>",
        r = i, u = r.replace(new RegExp(this.term, "gi"), "<span class='highlight-auto-list'>$&</span>"),
        $("<li class='airList " + (t.P == "1" || t.P == "100" ? "parent-auto-list" : "child-auto-list") + "'>").append(" ").append(u).appendTo(n);
    };
    $("#txtDestCode").autocomplete({
        autoFocus: !0,
        minLength: 3,
        source: function (n, t) {
            var i = document.getElementById("txtDestCode").value.trim();
            $.ajax({
                type: "POST",
                global: !1,
                url: "/Flight/GetAutoSuggestion",
                data: "{searchCode:'" + i + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (n) {
                    if (AutoSuggest_Counter = 0, n.toString().indexOf("Invalid") == -1) {
                        var i = JSON.parse(n);
                        t(i);
                    }
                },
                error: function () { }
            });
        },
        focus: function () {
            return !1;
        },
        blur: function (n, t) {
            return $("#txtDestCode").val(t.item.AC), $("#spanDestCityName").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode").val(t.item.AC), $("#txtDestCode_0").val(t.item.AC), $("#spanDestCityName_0").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode_0").val(t.item.AC), !1;
        },
        select: function (n, t) {
            return $("#txtDestCode").val(t.item.AC), $("#spanDestCityName").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode").val(t.item.AC), $("#txtDestCode_0").val(t.item.AC), $("#spanDestCityName_0").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode_0").val(t.item.AC), $("#txtDepartDate").val() == "" && n.key != "Tab" && $("#txtDepartDate").focus(),
            !1;
        },
        close: function () {
            if ($("#hdnDestinationCode").val().trim() == "" && AutoSuggest_Target == "txtDestCode" && $("#txtDestCode").val().length > 2 && AutoSuggest_AC != "") return $("#txtDestCode").val(AutoSuggest_AC),
            $("#spanDestCityName").text(AutoSuggest_CIN + ", " + AutoSuggest_CON), $("#hdnDestinationCode").val(AutoSuggest_AC),
            $("#txtDestCode_0").val(AutoSuggest_AC), $("#spanDestCityName_0").text(AutoSuggest_CIN + ", " + AutoSuggest_CON),
            $("#hdnDestinationCode_0").val(AutoSuggest_AC), !1;
        }
    }).autocomplete("instance")._renderItem = function (n, t) {
        var i, r, u;
        return AutoSuggest_Counter == 0 && ($("#hdnDestinationCode").val(""), AutoSuggest_Target = "txtDestCode",
        AutoSuggest_AC = t.AC, AutoSuggest_CIN = t.CIN, AutoSuggest_CON = t.CON), AutoSuggest_Counter = 1,
        i = "", i = t.COA == null || t.COA == undefined || t.COA == "" ? t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + ", " + t.Country + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>",
        r = i, u = r.replace(new RegExp(this.term, "gi"), "<span class='highlight-auto-list'>$&</span>"),
        $("<li class='airList " + (t.P == "1" || t.P == "100" ? "parent-auto-list" : "child-auto-list") + "'>").append(" ").append(u).appendTo(n);
    };
    $("#txtOriginCode_0").autocomplete({
        autoFocus: !0,
        minLength: 3,
        source: function (n, t) {
            var i = document.getElementById("txtOriginCode_0").value.trim();
            $.ajax({
                type: "POST",
                global: !1,
                url: "/Flight/GetAutoSuggestion",
                data: "{searchCode:'" + i + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (n) {
                    if (AutoSuggest_Counter = 0, n.toString().indexOf("Invalid") == -1) {
                        var i = JSON.parse(n);
                        t(i);
                    }
                },
                error: function () { }
            });
        },
        focus: function () {
            return !1;
        },
        blur: function (n, t) {
            return $("#txtOriginCode_0").val(t.item.AC), $("#spanOriginCityName_0").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode_0").val(t.item.AC), $("#txtOriginCode").val(t.item.AC), $("#spanOriginCityName").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode").val(t.item.AC), !1;
        },
        select: function (n, t) {
            return $("#txtOriginCode_0").val(t.item.AC), $("#spanOriginCityName_0").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode_0").val(t.item.AC), $("#txtOriginCode").val(t.item.AC), $("#spanOriginCityName").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode").val(t.item.AC), n.key != "Tab" && $("#txtDestCode_0").focus(),
            !1;
        },
        close: function () {
            if ($("#hdnOriginCode_0").val().trim() == "" && AutoSuggest_Target == "txtOriginCode_0" && $("#txtOriginCode_0").val().length > 2 && AutoSuggest_AC != "") return $("#txtOriginCode_0").val(AutoSuggest_AC),
            $("#spanOriginCityName_0").text(AutoSuggest_CIN + ", " + AutoSuggest_CON), $("#hdnOriginCode_0").val(AutoSuggest_AC),
            $("#txtOriginCode").val(AutoSuggest_AC), $("#spanOriginCityName").text(AutoSuggest_CIN + ", " + AutoSuggest_CON),
            $("#hdnOriginCode").val(AutoSuggest_AC), !1;
        }
    }).autocomplete("instance")._renderItem = function (n, t) {
        var i, r, u;
        return AutoSuggest_Counter == 0 && ($("#hdnOriginCode_0").val(""), AutoSuggest_Target = "txtOriginCode_0",
        AutoSuggest_AC = t.AC, AutoSuggest_CIN = t.CIN, AutoSuggest_CON = t.CON), AutoSuggest_Counter = 1,
        i = "", i = t.COA == null || t.COA == undefined || t.COA == "" ? t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + ", " + t.Country + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>",
        r = i, u = r.replace(new RegExp(this.term, "gi"), "<span class='highlight-auto-list'>$&</span>"),
        $("<li class='airList " + (t.P == "1" || t.P == "100" ? "parent-auto-list" : "child-auto-list") + "'>").append(" ").append(u).appendTo(n);
    };
    $("#txtDestCode_0").autocomplete({
        autoFocus: !0,
        minLength: 3,
        source: function (n, t) {
            var i = document.getElementById("txtDestCode_0").value.trim();
            $.ajax({
                type: "POST",
                global: !1,
                url: "/Flight/GetAutoSuggestion",
                data: "{searchCode:'" + i + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (n) {
                    if (AutoSuggest_Counter = 0, n.toString().indexOf("Invalid") == -1) {
                        var i = JSON.parse(n);
                        t(i);
                    }
                },
                error: function () { }
            });
        },
        focus: function () {
            return !1;
        },
        blur: function (n, t) {
            return $("#txtDestCode_0").val(t.item.AC), $("#spanDestCityName_0").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode_0").val(t.item.AC), $("#txtDestCode").val(t.item.AC), $("#spanDestCityName").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode").val(t.item.AC), $("#txtOriginCode_1").val(t.item.AC), $("#spanOriginCityName_1").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode_1").val(t.item.AC), !1;
        },
        select: function (n, t) {
            return $("#txtDestCode_0").val(t.item.AC), $("#spanDestCityName_0").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode_0").val(t.item.AC), $("#txtDestCode").val(t.item.AC), $("#spanDestCityName").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode").val(t.item.AC), $("#txtOriginCode_1").val(t.item.AC), $("#spanOriginCityName_1").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode_1").val(t.item.AC), $("#txtDepartDate_0").val() == "" && n.key != "Tab" && $("#txtDepartDate_0").focus(),
            !1;
        },
        close: function () {
            if ($("#hdnDestinationCode_0").val().trim() == "" && AutoSuggest_Target == "txtDestCode_0" && $("#txtDestCode_0").val().length > 2 && AutoSuggest_AC != "") return $("#txtDestCode_0").val(AutoSuggest_AC),
            $("#spanDestCityName_0").text(AutoSuggest_CIN + ", " + AutoSuggest_CON), $("#hdnDestinationCode_0").val(AutoSuggest_AC),
            $("#txtDestCode").val(AutoSuggest_AC), $("#spanDestCityName").text(AutoSuggest_CIN + ", " + AutoSuggest_CON),
            $("#hdnDestinationCode").val(AutoSuggest_AC), !1;
        }
    }).autocomplete("instance")._renderItem = function (n, t) {
        var i, r, u;
        return AutoSuggest_Counter == 0 && ($("#hdnDestinationCode_0").val(""), AutoSuggest_Target = "txtDestCode_0",
        AutoSuggest_AC = t.AC, AutoSuggest_CIN = t.CIN, AutoSuggest_CON = t.CON), AutoSuggest_Counter = 1,
        i = "", i = t.COA == null || t.COA == undefined || t.COA == "" ? t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + ", " + t.Country + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>",
        r = i, u = r.replace(new RegExp(this.term, "gi"), "<span class='highlight-auto-list'>$&</span>"),
        $("<li class='airList " + (t.P == "1" || t.P == "100" ? "parent-auto-list" : "child-auto-list") + "'>").append(" ").append(u).appendTo(n);
    };
    $("#txtOriginCode_1").autocomplete({
        autoFocus: !0,
        minLength: 3,
        source: function (n, t) {
            var i = document.getElementById("txtOriginCode_1").value.trim();
            $.ajax({
                type: "POST",
                global: !1,
                url: "/Flight/GetAutoSuggestion",
                data: "{searchCode:'" + i + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (n) {
                    if (AutoSuggest_Counter = 0, n.toString().indexOf("Invalid") == -1) {
                        var i = JSON.parse(n);
                        t(i);
                    }
                },
                error: function () { }
            });
        },
        focus: function () {
            return !1;
        },
        blur: function (n, t) {
            return $("#txtOriginCode_1").val(t.item.AC), $("#spanOriginCityName_1").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode_1").val(t.item.AC), !1;
        },
        select: function (n, t) {
            return $("#txtOriginCode_1").val(t.item.AC), $("#spanOriginCityName_1").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode_1").val(t.item.AC), n.key != "Tab" && $("#txtDestCode_1").focus(),
            !1;
        },
        close: function () {
            if ($("#hdnOriginCode_1").val().trim() == "" && AutoSuggest_Target == "txtOriginCode_1" && $("#txtOriginCode_1").val().length > 2 && AutoSuggest_AC != "") return $("#txtOriginCode_1").val(AutoSuggest_AC),
            $("#spanOriginCityName_1").text(AutoSuggest_CIN + ", " + AutoSuggest_CON), $("#hdnOriginCode_1").val(AutoSuggest_AC),
            !1;
        }
    }).autocomplete("instance")._renderItem = function (n, t) {
        var i, r, u;
        return AutoSuggest_Counter == 0 && ($("#hdnOriginCode_1").val(""), AutoSuggest_Target = "txtOriginCode_1",
        AutoSuggest_AC = t.AC, AutoSuggest_CIN = t.CIN, AutoSuggest_CON = t.CON), AutoSuggest_Counter = 1,
        i = "", i = t.COA == null || t.COA == undefined || t.COA == "" ? t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + ", " + t.Country + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>",
        r = i, u = r.replace(new RegExp(this.term, "gi"), "<span class='highlight-auto-list'>$&</span>"),
        $("<li class='airList " + (t.P == "1" || t.P == "100" ? "parent-auto-list" : "child-auto-list") + "'>").append(" ").append(u).appendTo(n);
    };
    $("#txtDestCode_1").autocomplete({
        autoFocus: !0,
        minLength: 3,
        source: function (n, t) {
            var i = document.getElementById("txtDestCode_1").value.trim();
            $.ajax({
                type: "POST",
                global: !1,
                url: "/Flight/GetAutoSuggestion",
                data: "{searchCode:'" + i + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (n) {
                    if (AutoSuggest_Counter = 0, n.toString().indexOf("Invalid") == -1) {
                        var i = JSON.parse(n);
                        t(i);
                    }
                },
                error: function () { }
            });
        },
        focus: function () {
            return !1;
        },
        blur: function (n, t) {
            return $("#txtDestCode_1").val(t.item.AC), $("#spanDestCityName_1").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode_1").val(t.item.AC), $("#hdnMC_Sec_2").val() == "1" && ($("#txtOriginCode_2").val(t.item.AC),
            $("#spanOriginCityName_2").text(t.item.CIN + ", " + t.item.CON), $("#hdnOriginCode_2").val(t.item.AC)),
            !1;
        },
        select: function (n, t) {
            return $("#txtDestCode_1").val(t.item.AC), $("#spanDestCityName_1").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode_1").val(t.item.AC), $("#hdnMC_Sec_2").val() == "1" && ($("#txtOriginCode_2").val(t.item.AC),
            $("#spanOriginCityName_2").text(t.item.CIN + ", " + t.item.CON), $("#hdnOriginCode_2").val(t.item.AC)),
            $("#txtDepartDate_1").val() == "" && n.key != "Tab" && $("#txtDepartDate_1").focus(),
            !1;
        },
        close: function () {
            if ($("#hdnDestinationCode_1").val().trim() == "" && AutoSuggest_Target == "txtDestCode_1" && $("#txtDestCode_1").val().length > 2 && AutoSuggest_AC != "") return $("#txtDestCode_1").val(AutoSuggest_AC),
            $("#spanDestCityName_1").text(AutoSuggest_CIN + ", " + AutoSuggest_CON), $("#hdnDestinationCode_1").val(AutoSuggest_AC),
            $("#hdnMC_Sec_2").val() == "1" && ($("#txtOriginCode_2").val(AutoSuggest_AC), $("#spanOriginCityName_2").text(AutoSuggest_CIN + ", " + AutoSuggest_CON),
            $("#hdnOriginCode_2").val(AutoSuggest_AC)), !1;
        }
    }).autocomplete("instance")._renderItem = function (n, t) {
        var i, r, u;
        return AutoSuggest_Counter == 0 && ($("#hdnDestinationCode_1").val(""), AutoSuggest_Target = "txtDestCode_1",
        AutoSuggest_AC = t.AC, AutoSuggest_CIN = t.CIN, AutoSuggest_CON = t.CON), AutoSuggest_Counter = 1,
        i = "", i = t.COA == null || t.COA == undefined || t.COA == "" ? t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + ", " + t.Country + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>",
        r = i, u = r.replace(new RegExp(this.term, "gi"), "<span class='highlight-auto-list'>$&</span>"),
        $("<li class='airList " + (t.P == "1" || t.P == "100" ? "parent-auto-list" : "child-auto-list") + "'>").append(" ").append(u).appendTo(n);
    };
    $("#txtOriginCode_2").autocomplete({
        autoFocus: !0,
        minLength: 3,
        source: function (n, t) {
            var i = document.getElementById("txtOriginCode_2").value.trim();
            $.ajax({
                type: "POST",
                global: !1,
                url: "/Flight/GetAutoSuggestion",
                data: "{searchCode:'" + i + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (n) {
                    if (AutoSuggest_Counter = 0, n.toString().indexOf("Invalid") == -1) {
                        var i = JSON.parse(n);
                        t(i);
                    }
                },
                error: function () { }
            });
        },
        focus: function () {
            return !1;
        },
        blur: function (n, t) {
            return $("#txtOriginCode_2").val(t.item.AC), $("#spanOriginCityName_2").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode_2").val(t.item.AC), !1;
        },
        select: function (n, t) {
            return $("#txtOriginCode_2").val(t.item.AC), $("#spanOriginCityName_2").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnOriginCode_2").val(t.item.AC), n.key != "Tab" && $("#txtDestCode_2").focus(),
            !1;
        },
        close: function () {
            if ($("#hdnOriginCode_2").val().trim() == "" && AutoSuggest_Target == "txtOriginCode_2" && $("#txtOriginCode_2").val().length > 2 && AutoSuggest_AC != "") return $("#txtOriginCode_2").val(AutoSuggest_AC),
            $("#spanOriginCityName_2").text(AutoSuggest_CIN + ", " + AutoSuggest_CON), $("#hdnOriginCode_2").val(AutoSuggest_AC),
            !1;
        }
    }).autocomplete("instance")._renderItem = function (n, t) {
        var i, r, u;
        return AutoSuggest_Counter == 0 && ($("#hdnOriginCode_2").val(""), AutoSuggest_Target = "txtOriginCode_2",
        AutoSuggest_AC = t.AC, AutoSuggest_CIN = t.CIN, AutoSuggest_CON = t.CON), AutoSuggest_Counter = 1,
        i = "", i = t.COA == null || t.COA == undefined || t.COA == "" ? t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + ", " + t.Country + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>",
        r = i, u = r.replace(new RegExp(this.term, "gi"), "<span class='highlight-auto-list'>$&</span>"),
        $("<li class='airList " + (t.P == "1" || t.P == "100" ? "parent-auto-list" : "child-auto-list") + "'>").append(" ").append(u).appendTo(n);
    };
    $("#txtDestCode_2").autocomplete({
        autoFocus: !0,
        minLength: 3,
        source: function (n, t) {
            var i = document.getElementById("txtDestCode_2").value.trim();
            $.ajax({
                type: "POST",
                global: !1,
                url: "/Flight/GetAutoSuggestion",
                data: "{searchCode:'" + i + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (n) {
                    if (AutoSuggest_Counter = 0, n.toString().indexOf("Invalid") == -1) {
                        var i = JSON.parse(n);
                        t(i);
                    }
                },
                error: function () { }
            });
        },
        focus: function () {
            return !1;
        },
        blur: function (n, t) {
            return $("#txtDestCode_2").val(t.item.AC), $("#spanDestCityName_2").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode_2").val(t.item.AC), !1;
        },
        select: function (n, t) {
            return $("#txtDestCode_2").val(t.item.AC), $("#spanDestCityName_2").text(t.item.CIN + ", " + t.item.CON),
            $("#hdnDestinationCode_2").val(t.item.AC), $("#txtDepartDate_2").val() == "" && n.key != "Tab" && $("#txtDepartDate_2").focus(),
            !1;
        },
        close: function () {
            if ($("#hdnDestinationCode_2").val().trim() == "" && AutoSuggest_Target == "txtDestCode_2" && $("#txtDestCode_2").val().length > 2 && AutoSuggest_AC != "") return $("#txtDestCode_2").val(AutoSuggest_AC),
            $("#spanDestCityName_2").text(AutoSuggest_CIN + ", " + AutoSuggest_CON), $("#hdnDestinationCode_2").val(AutoSuggest_AC),
            !1;
        }
    }).autocomplete("instance")._renderItem = function (n, t) {
        var i, r, u;
        return AutoSuggest_Counter == 0 && ($("#hdnDestinationCode_2").val(""), AutoSuggest_Target = "txtDestCode_2",
        AutoSuggest_AC = t.AC, AutoSuggest_CIN = t.CIN, AutoSuggest_CON = t.CON), AutoSuggest_Counter = 1,
        i = "", i = t.COA == null || t.COA == undefined || t.COA == "" ? t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + ", " + t.Country + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + ", " + t.Country + "</li>" : t.P == "1" && t.P2 == "1" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.CityCode + "), " + t.CON + "</li>" : t.P != "1" && t.P2 != "100" ? "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>" : "<li>" + t.CIN + ", " + t.AN + " (" + t.AC + "), " + t.CON + "</li>",
        r = i, u = r.replace(new RegExp(this.term, "gi"), "<span class='highlight-auto-list'>$&</span>"),
        $("<li class='airList " + (t.P == "1" || t.P == "100" ? "parent-auto-list" : "child-auto-list") + "'>").append(" ").append(u).appendTo(n);
    };
});

$("#txtOriginCode").focusout(function () {
    setTimeout(function () {
        $("#txtOriginCode").val().trim() != "" && $("#hdnOriginCode").val().trim() == "" ? ($("#txtOriginCode").val(""),
        $("#spanOriginCityName").text("Airport/City Name"), $("#txtOriginCode").next().hide(),
        $("#txtOriginCode_0").val(""), $("#spanOriginCityName_0").text("Airport/City Name"),
        $("#txtOriginCode_0").next().hide()) : $("#txtOriginCode").val().trim() != "" && $("#hdnOriginCode").val().trim() != "" && ($("#txtOriginCode").val($("#hdnOriginCode").val().trim()),
        $("#txtOriginCode_0").val($("#hdnOriginCode_0").val().trim()));
    }, 100);
});

$("#txtDestCode").focusout(function () {
    setTimeout(function () {
        $("#txtDestCode").val().trim() != "" && $("#hdnDestinationCode").val().trim() == "" ? ($("#txtDestCode").val(""),
        $("#spanDestCityName").text("Airport/City Name"), $("#txtDestCode").next().hide(),
        $("#txtDestCode_0").val(""), $("#spanDestCityName_0").text("Airport/City Name"),
        $("#txtDestCode_0").next().hide()) : $("#txtDestCode").val().trim() != "" && $("#hdnDestinationCode").val().trim() != "" && ($("#txtDestCode").val($("#hdnDestinationCode").val().trim()),
        $("#txtDestCode_0").val($("#hdnDestinationCode_0").val().trim()));
    }, 100);
});

$("#txtOriginCode_0").focusout(function () {
    setTimeout(function () {
        $("#txtOriginCode_0").val().trim() != "" && $("#hdnOriginCode_0").val().trim() == "" ? ($("#txtOriginCode_0").val(""),
        $("#spanOriginCityName_0").text("Airport/City Name"), $("#txtOriginCode_0").next().hide(),
        $("#txtOriginCode").val(""), $("#spanOriginCityName").text("Airport/City Name"),
        $("#txtOriginCode").next().hide()) : $("#txtOriginCode_0").val().trim() != "" && $("#hdnOriginCode_0").val().trim() != "" && ($("#txtOriginCode_0").val($("#hdnOriginCode_0").val().trim()),
        $("#txtOriginCode").val($("#hdnOriginCode").val().trim()));
    }, 100);
});

$("#txtDestCode_0").focusout(function () {
    setTimeout(function () {
        $("#txtDestCode_0").val().trim() != "" && $("#hdnDestinationCode_0").val().trim() == "" ? ($("#txtDestCode_0").val(""),
        $("#spanDestCityName_0").text("Airport/City Name"), $("#txtDestCode_0").next().hide(),
        $("#txtDestCode").val(""), $("#spanDestCityName").text("Airport/City Name"), $("#txtDestCode").next().hide()) : $("#txtDestCode_0").val().trim() != "" && $("#hdnDestinationCode_0").val().trim() != "" && ($("#txtDestCode_0").val($("#hdnDestinationCode_0").val().trim()),
        $("#txtDestCode").val($("#hdnDestinationCode").val().trim()));
    }, 100);
});

$("#txtOriginCode_1").focusout(function () {
    setTimeout(function () {
        $("#txtOriginCode_1").val().trim() != "" && $("#hdnOriginCode_1").val().trim() == "" ? ($("#txtOriginCode_1").val(""),
        $("#spanOriginCityName_1").text("Airport/City Name"), $("#txtOriginCode_1").next().hide()) : $("#txtOriginCode_1").val().trim() != "" && $("#hdnOriginCode_1").val().trim() != "" && $("#txtOriginCode_1").val($("#hdnOriginCode_1").val().trim());
    }, 100);
});

$("#txtDestCode_1").focusout(function () {
    setTimeout(function () {
        $("#txtDestCode_1").val().trim() != "" && $("#hdnDestinationCode_1").val().trim() == "" ? ($("#txtDestCode_1").val(""),
        $("#spanDestCityName_1").text("Airport/City Name"), $("#txtDestCode_1").next().hide()) : $("#txtDestCode_1").val().trim() != "" && $("#hdnDestinationCode_1").val().trim() != "" && $("#txtDestCode_1").val($("#hdnDestinationCode_1").val().trim());
    }, 100);
});

$("#txtOriginCode_2").focusout(function () {
    setTimeout(function () {
        $("#txtOriginCode_2").val().trim() != "" && $("#hdnOriginCode_2").val().trim() == "" ? ($("#txtOriginCode_2").val(""),
        $("#spanOriginCityName_2").text("Airport/City Name"), $("#txtOriginCode_2").next().hide()) : $("#txtOriginCode_2").val().trim() != "" && $("#hdnOriginCode_2").val().trim() != "" && $("#txtOriginCode_2").val($("#hdnOriginCode_2").val().trim());
    }, 100);
});

$("#txtDestCode_2").focusout(function () {
    setTimeout(function () {
        $("#txtDestCode_2").val().trim() != "" && $("#hdnDestinationCode_2").val().trim() == "" ? ($("#txtDestCode_2").val(""),
        $("#spanDestCityName_2").text("Airport/City Name"), $("#txtDestCode_2").next().hide()) : $("#txtDestCode_2").val().trim() != "" && $("#hdnDestinationCode_2").val().trim() != "" && $("#txtDestCode_2").val($("#hdnDestinationCode_2").val().trim());
    }, 100);
});