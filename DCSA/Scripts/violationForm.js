$(document).ready(function () {
	$("#AbuseLi").addClass('active-link');
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


	
    var Attachmentsinput = document.getElementById('inlineRadio10');

    if (Attachmentsinput.checked) {


        document.getElementById('Attachments-input').style.display = 'block';
        document.getElementById("AddAttachButton").disabled = false;


    } else {

        document.getElementById('Attachments-input').style.display = 'none';
        document.getElementById("AddAttachButton").disabled = true;


    }
	


});


function OpenModal() {
    document.getElementById('ModalPerson').innerText = document.getElementById('PersonName').value;
    var phonNo = document.getElementsByName('PhonNo');
    var phonno = "";
    for (var i = 0; i < phonNo.length; i++) {
        phonno += phonNo[i].value;
    }
    document.getElementById('ModalPhon').innerText = phonno;
    document.getElementById('ModalCName').innerText = document.getElementById('ChildName').value;
    document.getElementById('ModalCAge').innerText = document.getElementById('Age').value;
    document.getElementById('ModalPlace').innerText = document.getElementById('floatingTextarea2').value;
    document.getElementById('ModalDes').innerText = document.getElementById('floatingTextarea3').value;
    if (document.getElementById('inlineRadio3').checked == true)
        document.getElementById('ModalConn').innerText = "لا";
    if (document.getElementById('inlineRadio4').checked == true)
        document.getElementById('ModalConn').innerText = "نعم";

    if (document.getElementById('inlineRadio4').checked == true)
        document.getElementById('ModalFileNo').innerText = document.getElementById('FileNo').value;
    else
        document.getElementById('ModalFileNo').innerText = "";

    var docs = document.getElementsByName('AttachmentDoc');
    var names = document.getElementsByName('AttachmentName');
    var content = "";
    for (var n = 0; n < docs.length; n++) {
        if (names[n].value != "" && names[n].value != null)
            content += '<span >' + docs[n].files[0].name + '</span><label style="margin-right:30px">' + names[n].value + '</label><br />'
    }

    document.getElementById('ModalAttachment').innerHTML = content;

    document.getElementById("RecapResponse").value = $('#g-recaptcha-response').val();

    var formdata = new FormData($('form').get(0));

    var tag = document.getElementById('updateProgress');
    tag.style.display = 'block';

    $.ajax({
        url: validateUrl,
        type: 'POST',
        data: formdata,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.IsValid) {
                

                $('#staticBackdrop').modal('show');
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
                $("#AttachmentName-0").prop('disabled', false);
                $("#AttachmentDoc-0").prop('disabled', false);
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

                    if (data.FieldID == "PC") {
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


    function reportcheck() {

	var yesinput = document.getElementById('inlineRadio4');

	if (yesinput.checked) {


        document.getElementById('center-input').style.display = 'block';

    } else {

        document.getElementById('center-input').style.display = 'none';

    }
}

function reportcheckTab3() {

    var Attachmentsinput = document.getElementById('inlineRadio10');

    if (Attachmentsinput.checked) {


        document.getElementById('Attachments-input').style.display = 'block';
        document.getElementById("AddAttachButton").disabled = false;


    } else {

        document.getElementById('Attachments-input').style.display = 'none';
        document.getElementById("AddAttachButton").disabled = true;


    }
}

function DeleteAttach(id) {
    $("#AttachmentDiv-" + id).remove();
    $("#Attach-" + id).remove();


}

var i = 1;

function SaveAttachment() {

    if (document.getElementById("AttachmentName-0").value == "" || document.getElementById("AttachmentName-0").value == null) {
        swal({
            title: "خطأ!",
            text: "من فضلك ادخل اسم المرفق",
            type: "error",
        });

        document.getElementById("validAttachName").innerText = "من فضلك ادخل اسم المرفق";
        $("#AttachmentName-0").css('border-color', 'red');

        return;
    }

    if (document.getElementById("AttachmentDoc-0").value == "" || document.getElementById("AttachmentDoc-0").value == null) {
        swal({
            title: "خطأ!",
            text: "من فضلك اختر المرفق",
            type: "error",
        });

        document.getElementById("validAttach").innerText = "من فضلك اختر المرفق";
        $("#file-0").css('border-color', 'red');

        return;
    }

    if (i == 3) {
        swal({
            title: "خطأ!",
            text: "لا يمكن اضافة اكثر من مرفقين",
            type: "error",
        });
        return;
    }



    $("#AttachmentDiv-0").clone().appendTo('#sec3').attr("id", "AttachmentDiv-" + i);
    document.getElementById("AttachmentDiv-" + i).style.display = "none";
    $("#AttachmentDiv-0").find('input').val("");


    var attName = $("#AttachmentDiv-" + i).find('input');
    var props = $(attName[2]).prop('files');

    var mark = '<tr class="edit-tr" id="Attach-' + i + '">';
    mark += '<td class="first-tr"><img class="add-img" id="attachmentPhoto-' + i + '">' + props[0].name + '</td>';
    mark += '<td class="sec-tr">' + $(attName[0]).val() + '</td>';
    mark += '<td class="five-tr"><i class="uil uil-trash-alt"></i>  <a class="ahover" onclick="DeleteAttach(' + i + ')">مسح</a> </td>'
    mark += '</tr>';

    var table = $("#AttachTable tbody");
    if (attName != "" && attName != null && props[0].name != "" && props[0].name != null) {
        table.append(mark);
    }

    i++;

}

function CollectData() {
    if (document.getElementById("AttachmentName-0").value != "" || document.getElementById("AttachmentDoc-0").value != "") {
        $("#AttachmentName-0").prop('disabled', true);
        $("#AttachmentDoc-0").prop('disabled', true);
    }
    document.getElementById("RecapResponse").value = $('#g-recaptcha-response').val();


    var formdata = new FormData($('form').get(0));

    var tag = document.getElementById('updateProgress');
    tag.style.display = 'block';

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
                $("#AttachmentName-0").prop('disabled', false);
                $("#AttachmentDoc-0").prop('disabled', false);
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

                    if (data.FieldID == "PC") {
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

    if (inputid == "PC") {
        var inputs = document.getElementsByName(inputid);
        for (var i = 0; i < inputs.length; i++) {
            $(inputs[i]).css('border-color', '');
            $(inputs[i].labels[0]).css('color', '');
        }
    }

    document.getElementById(validid).innerText = "";
    $("#" + inputid).css('border-color', '');
}
