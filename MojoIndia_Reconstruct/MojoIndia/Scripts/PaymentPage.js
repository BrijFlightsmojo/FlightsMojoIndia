
(function ($, W, D) {
    var JQUERY4U = {};
    JQUERY4U.UTIL =
        {
            setupFormValidation: function () {
                $("#payment").validate({
                    rules: {

                        'paymentDetails.cardNumber': {
                            required: true,
                            number: true,
                            minlength: 15,
                            maxlength: 16,
                        },
                        'paymentDetails.cardHolderName': {
                            required: true,
                            requiredTextvalue: true,
                            lettersonly: true
                        },
                        'paymentDetails.expiryMonth': {
                            selectExpMonth: true,
                        },
                        'paymentDetails.expiryYear': {
                            selectExpYear: true,
                            CCExp: {
                                month: '#paymentDetails_expiryMonth',
                                year: '#paymentDetails_expiryYear'
                            }
                        },
                        'paymentDetails.cvvNo': {
                            required: true,
                            ccvnumeric: true,
                            minlength: 3,
                            totCvv: true
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
                        if ($(element).attr("name") == "paymentDetails.expiryMonth" || $(element).attr("name") == "paymentDetails.expiryYear" || $(element).attr("name") == "paymentDetails.cardNumber" || $(element).attr("name") == "paymentDetails.cvvNo" || $(element).attr("name") == "paymentDetails.cardHolderName") {
                            $("#paymentError").addClass('help-block');
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
                        if (element.attr("name") == "paymentDetails.expiryMonth" || element.attr("name") == "paymentDetails.expiryYear" || element.attr("name") == "paymentDetails.cardNumber" || element.attr("name") == "paymentDetails.cvvNo" || element.attr("name") == "paymentDetails.cardHolderName") {
                            $("#paymentError").addClass('help-block');
                            error.css('float', 'none');
                            element.append($('<br />'));
                            error.appendTo("#paymentError");
                        } else if (element.attr("name") == "agree") {
                            document.getElementById("agreeError").style.display = 'block';
                            $("#agree").addClass('help-block');
                        }
                        else if (element.parent('.input-group').length) {
                            error.insertAfter(element.parent());
                        } else error.insertAfter(element);
                    },
                    messages: {
                        'paymentDetails.cardNumber': {
                            required: " Please provide valid credit or debit card number",
                            minlength: " Please provide valid credit or debit card number",
                            maxlength: " Please provide valid credit or debit card number"
                        },
                        'paymentDetails.cardHolderName': {
                            required: " Please provide Card Holder Name",
                        },
                        'paymentDetails.cvvNo': {
                            required: " Please provide card verification number",
                            ccvnumeric: " Please provide a valid card verification number",
                            minlength: " Please provide a valid card verification number",
                            maxlength: " Please provide a valid card verification number",
                        },
                    },
                    submitHandler: function (form) {
                        if (true || document.getElementById("agree").checked) {
                            ShowProPopup();
                            form.submit();
                        }
                        else {
                            document.getElementById("agreeError").style.display = 'block';
                        }
                    }
                });
            }
        }

    $(D).ready(function ($) {
        $.validator.setDefaults({ ignore: '' });
        $.validator.messages.required = '';

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
        $.validator.addMethod('selectExpMonth', function (value) {
            return (value != '0' && value != '');
        }, " Please select card expiration month");

        $.validator.addMethod('selectExpYear', function (value) {
            return (value != 'Year' && value != '' && value != '0');
        }, " Please select card expiration year");

        jQuery.validator.addMethod('CCExp', function (value, element, params) {
            var minMonth = new Date().getMonth() + 1;
            var minYear = new Date().getFullYear();
            var month = parseInt($(params.month).val(), 10);
            var year = parseInt($(params.year).val(), 10);
            return (year > minYear || (year === minYear && month >= minMonth));
        }, 'Your Credit Card Expiration date is invalid.');

        $.validator.addMethod("ccvnumeric", function (value, element) {
            if (value.length > 0) {
                return this.optional(element) || /^[-+]?\d*\.?\d*$/.test(value);
            } else {
                return false;
            }
        }, " Please provide only numbers into card verification number");

        $.validator.addMethod("totCvv", function (value, element) {
            var cardType = creditCardTypeFromNumber($("#paymentDetails_cardNumber").val());
            if (value.length == 4 && cardType == "AX") {
                return true;
            }
            else if (value.length == 3 && cardType != "AX") {
                return true;
            }
            else {
                return false;
            }
        }, " Please provide right card verification number");

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

function creditCardTypeFromNumber(num) {

    if (num.charAt(0) === '4') {
        return "VI";
    }
    else if (num.length >= 2 && (num.substr(0, 2) === '34' || num.substr(0, 2) === '37')) {
        return "AX";
    }
    else if (num.charAt(0) === '5') {
        return "CA";
    }
    else if (num.charAt(0) === '6') {
        return "DS";
    }
    else if (num.length >= 2 && (num.substr(0, 2) === '35')) {
        return "JC";
    }
    else if (num.length >= 2 && (num.substr(0, 2) === '36' || num.substr(0, 2) === '38')) {
        return "DC";
    }
    else {
        return "";
    }
}


