$(document).ready(function () {
$("#AdviceLi").addClass('active-link');
    var $idnum = $("#idnum").find("input");
    var $mobilenum = $("#mobilenum").find("input");

   

    //bind events
    $idnum.on('keyup', processInput);
    $mobilenum.on('keyup', processInput1);

    //define methods for national ID
    function processInput(e) {
        var x = e.charCode || e.keyCode;
        if ((x == 8 || x == 46) && this.value.length == 0) {
            var indexNum = $idnum.index(this);
            if (indexNum != 0) {
                $idnum.eq($idnum.index(this) - 1).focus();
            }
        }

        if (ignoreChar(e))
            return false;
        else if (this.value.length == this.maxLength) {
            $(this).next('input').focus();
        }
    }


    //define methods for Phone number
    function processInput1(e) {
        var x = e.charCode || e.keyCode;
        if ((x == 8 || x == 46) && this.value.length == 0) {
            var indexNum = $mobilenum.index(this);
            if (indexNum != 0) {
                $mobilenum.eq($mobilenum.index(this) - 1).focus();
                $mobilenum.eq($mobilenum.index(this) - 1).val("");
            }
        }

        if (ignoreChar(e))
            return false;
        else if (this.value.length == this.maxLength) {
            $(this).next('input').focus();
            $(this).next('input').val("");
        }
    }



    function ignoreChar(e) {
        var x = e.charCode || e.keyCode;
        if (x == 37 || x == 38 || x == 39 || x == 40)
            return true;
        else
            return false
    }

});


function OpenModal() {
    document.getElementById('ModalName').innerText = document.getElementById('PersonName').value;

    var phone = document.getElementsByName('PhonNo');
    var phoneno = "";
    for (var i = 0; i < phone.length; i++) {
        phoneno += phone[i].value;
    }
    document.getElementById('ModalPhone').innerText = phoneno;

    document.getElementById('ModalAddress').innerText = document.getElementById('Address').value;

    if (document.getElementById('Mother').checked == true)
        document.getElementById('ModalType').innerText = document.getElementById('Mother').value;
    if (document.getElementById('Child').checked == true)
        document.getElementById('ModalType').innerText = document.getElementById('Child').value;

    document.getElementById('ModalDescription').innerText = document.getElementById('floatingTextarea3').value;
    document.getElementById('ModalChildName').innerText = document.getElementById('ChildName').value;
    document.getElementById('ModalChildAge').innerText = document.getElementById('Age').value;

    if (document.getElementById('inlineRadio3').checked == true)
        document.getElementById('ModalConn').innerText = "لا";
    if (document.getElementById('inlineRadio4').checked == true)
        document.getElementById('ModalConn').innerText = "نعم";

    if (document.getElementById('inlineRadio3').checked == true)
        document.getElementById('ModalFileNo').innerText = "";
    else
        document.getElementById('ModalFileNo').innerText = document.getElementById('FileNo').value;


    document.getElementById("RecapResponse").value = $('#g-recaptcha-response').val();

    var formdata = new FormData($('form').get(0));
    var tag = document.getElementById('updateProgress');
    tag.style.display = 'block';


    $.ajax({
        url: validUrl,
        type: 'POST',
        data: formdata,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.IsValid) {
                $("#staticBackdrop").modal('show');


                tag.style.display = 'none';
            }
            else {
                swal({
                    title: "خطأ!",
                    text: data.Message,
                    type: "error",
                    confirmButtonText: "Ok"
                }, function () {
                    $('html, body').animate({
                        scrollTop: $("#" + data.FieldID).offset().top
                    }, 2000);
                    window.scrollTo(0, 100);
                });
                tag.style.display = 'none';

                if (data.ValidFieldID != "") {
                    document.getElementById(data.ValidFieldID).innerText = data.Message;
                    $("#" + data.FieldID).css('border-color', 'red');
                    $(window).scrollTop($('#' + data.FieldID).offset().top);

                    if (data.FieldID == "PhonNo") {
                        var bit = document.getElementsByName("PhonNo");
                        for (var i = 0; i < bit.length; i++) {
                            $(bit[i]).css('border-color', 'red');
                        }
                    }

                    if (data.FieldID == "PC" || data.FieldID == "Type") {
                        var inputs = document.getElementsByName(data.FieldID);
                        for (var i = 0; i < inputs.length; i++) {
                            $(inputs[i]).css('border-color', 'red');
                            $(inputs[i].labels[0]).css('color', 'red');
                        }
                    }
                }

            }
        },
        error: function () {

            swal({
                title: "خطأ!",
                text: "حدث خطأ",
                type: "error",
            });
            tag.style.display = 'none';
        }
    });

}


function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;

}

function isChild(input) {
    if (input.checked == true) {
        document.getElementById('ChildDataDiv').style.display = 'block';
    }
    else {
        document.getElementById('ChildDataDiv').style.display = 'none';
    }
}


function reportcheck() {

	var yesinput = document.getElementById('inlineRadio4');

	if (yesinput.checked) {


        document.getElementById('center-input').style.display = 'block';

    } else {

        document.getElementById('center-input').style.display = 'none';

    }
}

function typeCheck(input) {
    if (input.checked && input.id == "Mother")
    document.getElementById("Child").checked = false;

    if (input.checked && input.id == "Child")
        document.getElementById("Mother").checked = false;
    
}

function CollectData() {
    document.getElementById("RecapResponse").value = $('#g-recaptcha-response').val();

    var formdata = new FormData($('form').get(0));
    var tag = document.getElementById('updateProgress');
    tag.style.display = 'block';

    //res = $('#g-recaptcha-response').val();

    //if (res == "" || res == undefined || res.length == 0) {
    //    swal({
    //        title: "خطأ!",
    //        text: "Fail",
    //        type: "error",
    //    });
    //    tag.style.display = 'none';
    //    return;
    //}

    $.ajax({
        url: url,
        type: 'POST',
        data: formdata,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.IsValid) {
                swal({
                    title: data.Message,
                    text: "نجاح",
                    type: "success",
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "تم",
                    closeOnConfirm: false
                }, function () {
                    window.location.reload();

                });


                tag.style.display = 'none';            }
            else {
                swal({
                    title: "خطأ!",
                    text: data.Message,
                    type: "error",
                    confirmButtonText: "Ok"
                }, function () {
                    $('html, body').animate({
                        scrollTop: $("#" + data.FieldID).offset().top
                    }, 2000);
                    window.scrollTo(0, 100);
                });
                tag.style.display = 'none';

                if (data.ValidFieldID != "") {
                    $(window).scrollTop($('#' + data.FieldID).offset().top);

                    document.getElementById(data.ValidFieldID).innerText = data.Message;
                    $("#" + data.FieldID).css('border-color', 'red');

                    if (data.FieldID == "PhonNo") {
                        var bit = document.getElementsByName("PhonNo");
                        for (var i = 0; i < bit.length; i++) {
                            $(bit[i]).css('border-color', 'red');
                        }
                    }

                    if (data.FieldID == "PC" || data.FieldID == "Type") {
                        var inputs = document.getElementsByName(data.FieldID);
                        for (var i = 0; i < inputs.length; i++) {
                            $(inputs[i]).css('border-color', 'red');
                            $(inputs[i].labels[0]).css('color', 'red');
                        }
                    }
                }

            }
        },
        error: function () {

            swal({
                title: "خطأ!",
                text: "حدث خطأ",
                type: "error",
            });
            tag.style.display = 'none';
        }
    });

}



function ValidationRefresh(inputid, validid) {


    if (inputid == "PhonNo") {
        var bit = document.getElementsByName("PhonNo");
        for (var i = 0; i < bit.length; i++) {
            $(bit[i]).css('border-color', '');
        }
    }

    if (inputid == "PC" || inputid == "Type") {
        var inputs = document.getElementsByName(inputid);
        for (var i = 0; i < inputs.length; i++) {
            $(inputs[i]).css('border-color', '');
            $(inputs[i].labels[0]).css('color', '');
        }
    }
    

    document.getElementById(validid).innerText = "";
    $("#" + inputid).css('border-color', '');
    
}
