$(document).ready(function () {
    $('.mail-container > td:not(.mail-delete-action)').on('click', function () {
        var mailBody = $(this).parent().find('.message-body').html();
        $('#mailBody').html(mailBody);
        showMessageModule();
    });

    $("#goBack").on("click", function () {
        $('#mailBody').html('');
        showMailListModule();
    });

    $('.mail-delete').on('click', function () {
        if (!confirm('Are you sure you want to delete this message?')) {
            return false;
        }
        var element = $(this);
        var data = {
            uId: $(this).attr('data-id'),
            folder: $(this).attr('data-folder')
        };

        $.post('/Mail/Delete', data, function (result) {
            if (result.success) {
                element.parent().parent().fadeOut();
            } else {
                alert("There was an error. Please contact an Admin!");
            }
        }, 'json');
    });

    $('#compose').on('click', function () {
        showComposeModule();
    });

    $("#backToMails").on("click", function () {
        showMailListModule();
    });

    $("#sendMailForm").on("submit", function (e) {
        e.preventDefault();

        if (validateData()) {
            var data = $(this).serialize();
            //make call
            sendMessage(data);
        } else {
            alert("All data is required!!");
            return false;
        }
    });
});

function sendMessage(data) {
    $.post('/Mail/Compose', data, function (result) {
        if (result.success) {
            alert("Message sent!");
            showMailListModule();
        } else {
            alert("There was an error. Please contact an Admin!");
        }
        return false;
    }, 'json');
}

function validateData() {
    var to = $("#sendMailForm input[name='to']").val();
    var subject = $("#sendMailForm input[name='subject']").val();
    var body = $("#sendMailForm textarea[id='composeBody']").val();

    if (to && subject && body) return true;
    return false;
}

function showComposeModule() {
    $('.table').hide();
    $('#goBack').hide();
    $('#mailBody').hide();
    $('#composeBodyWrapper').show();
    $('#noMails').hide();
}

function showMailListModule() {
    $('#mailBody').hide();
    $('#goBack').hide();
    $('.table').show();
    $('#composeBodyWrapper').hide();
}

function showMessageModule() {
    $('.table').hide();
    $('#goBack').show();
    $('#mailBody').show();
    $('#composeBodyWrapper').hide();
    $('#noMails').hide();
}