// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $(document).find("#putCoins").prop('disabled', true);
    
    $(document).on('click', '#plus', function (e) {
        e.stopPropagation()
        var count = $(document).find("#coinsCount");
        $(count).val(parseInt($(count).val()) + 1);
    });
    $(document).on('click', '#minus', function (e) {
        e.stopPropagation()
        var count = $(document).find("#coinsCount");
        $(count).val(parseInt($(count).val()) - 1);
        if ($(count).val() == 0) {
            $(count).val(1);
        }
    });

    $('.denomination').bind('click', function (e) {
        e.stopPropagation();
        var dVal = parseInt(e.target.id);
        var count = $(document).find("#coinsCount");
        var cVal = parseInt($(count).val());
        $.ajax({
            url: '/Home/PutCoins',
            type: 'PUT',
            data: JSON.stringify({ denomination: dVal, amount: cVal }),
            accepts: "application/json",
            contentType: "application/json",
            success: function (data) {
                $(document).find("#errorMessage").attr('hidden', true);
                location.reload();
            },
            error: function (jq, message) {
                $(document).find("#errorMessage").attr('hidden', false);
                $(document).find("#errorMessage").text(message);
            }
        });
    });
    $(document).on('click', '#returnCoins', function (e) {
        e.stopPropagation();
        $.ajax({
            url: '/Home/ReturnCoins',
            type: 'DELETE',
            data: {},
            accepts: "application/json",
            contentType: "application/json",
            success: function (data) {
                $(document).find("#errorMessage").attr('hidden', true);
                location.reload();
            },
            error: function (jq, message) {
                $(document).find("#errorMessage").attr('hidden', false);
                $(document).find("#errorMessage").text(message);
            }
        });
    });
    $('.purchase').bind('click', function (e) {
        e.stopPropagation();
        var productName = e.target.id;
        $.ajax({
            url: '/Home/Purchase',
            type: 'POST',
            data: JSON.stringify({ productName: productName }),
            accepts: "application/json",
            contentType: "application/json",
            success: function (data) {
                var message = data.message;
                var failed = data.failed;
                var change = data.change;
                if (change && !failed) {
                    message = message ? message : '';
                    change.forEach((c) => message += ' Your change: ' + c.amount + ' x ' + c.denomination + 'c  ');
                }
                if (failed) {
                    $(document).find("#errorMessage").attr('hidden', false);
                    $(document).find("#errorMessage").text(message);
                } else {
                    $(document).find("#errorMessage").attr('hidden', true);
                    $(document).find("#changeMessage").attr('hidden', false);
                    $(document).find("#changeMessage").text(message);
                }
                $.ajax({
                    url: '/Home/GetDeposit',
                    type: 'GET',
                    success: function (data) { $(document).find("#depositAmount").text(data.totalAmount); }
                });
            },
            error: function (jq, message) {
                $(document).find("#errorMessage").attr('hidden', false);
                $(document).find("#errorMessage").text(message);
            }
        });
    });
});