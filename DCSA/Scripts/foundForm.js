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
                    document.getElementById(data.ValidFieldID).innerText = data.Message;
                    $("#" + data.FieldID).css('border-color', 'red');
                    $("#example-basic-t-" + data.Step).click();
                    $("#example-basic-t-" + data.Step).addClass('vactive').parent().siblings().find('a').removeClass('vactive');
                    $(window).scrollTop($('#' + data.FieldID).offset().top);

                    if (data.FieldID == "NationalID") {
                        var id = document.getElementsByName("NationalID");
                        for (var i = 0; i < id.length; i++) {
                            $(id[i]).css('border-color', 'red');
                        }
                    }

                    if (data.FieldID == "PhonNo") {
                        var bit = document.getElementsByName("PhonNo");
                        for (var i = 0; i < bit.length; i++) {
                            $(bit[i]).css('border-color', 'red');
                        }
                    }

                    if (data.FieldID == "Gender" || data.FieldID == "HealthStat" || data.FieldID == "IsKnown" || data.FieldID == "ReportBit") {
                        var inputs = document.getElementsByName(data.FieldID);
                        for (var i = 0; i < inputs.length; i++) {
                            $(inputs[i]).css('border-color', 'red');
                            $(inputs[i].labels[0]).css('color', 'red');
                        }
                    }

                    if (data.FieldID == "GoverID" || data.FieldID == "DistrictID" || data.FieldID == "DisTypeID" || data.FieldID == "FoundGoverID" || data.FieldID == "FoundDistrictID" || data.FieldID == "KnowType" || data.FieldID == "ExistGoverID" || data.FieldID == "ExistDistrictID") {
                        $('#' + data.FieldID).siblings(".select2-container").css('border', '1px solid red');
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

function OpenModal() {
    document.getElementById('ModalPerson').innerText = document.getElementById('PersonName').value;

    var nationalID = document.getElementsByName('NationalID');
    nationalId = "";
    for (var m = 0; m < nationalID.length; m++) {
        nationalId += nationalID[m].value;
    }

    var phonNo = document.getElementsByName('PhonNo');
    var phonno = "";
    for (var i = 0; i < phonNo.length; i++) {
        phonno += phonNo[i].value;
    }

    document.getElementById('ModalNationalID').innerText = nationalId;
    document.getElementById('ModalPhonNo').innerText = phonno;
    document.getElementById('ModalPGover').innerText = $('#GoverID option:selected').text();
    document.getElementById('ModalPDis').innerText = $('#DistrictID option:selected').text();
    document.getElementById('ModalPAddress').innerText = document.getElementById('Address').value;
    document.getElementById('ModalCName').innerText = document.getElementById('ChildName').value;
    document.getElementById('ModalCSource').innerText = $('#ChildNameSource option:selected').text();
    document.getElementById('ModalCImage').innerText = document.getElementById('file1').value;
    if (document.getElementById('inlineRadio1').checked == true)
        document.getElementById('ModalCType').innerText = "ذكر";
    if (document.getElementById('inlineRadio2').checked == true)
        document.getElementById('ModalCType').innerText = "انثى";
    var HS = document.getElementsByName('HealthStat');
    var hs = "";
    for (var y = 0; y < HS.length; y++) {
        if (HS[y].checked == true) {
            var id = HS[y].value;
            if (y == HS.length - 1)
                hs += document.getElementById('CheckBoxLabel-' + id).innerText;
            else
                hs += document.getElementById('CheckBoxLabel-' + id).innerText + ' - ';
        }
    }
    var distype = $('#DisTypeID option:selected').toArray().map(item => item.text).join();
    distype = distype.split(',');
    var DisType = "";
    for (var j = 0; j < distype.length; j++) {
        if (j == distype.length - 1)
            DisType += distype[j];
        else
            DisType += distype[j] + ' - ';
    }
    document.getElementById('ModalCHS').innerText = hs;
    if (HS[2].checked == true)
        document.getElementById('ModalCDisability').innerText = DisType;
    else
        document.getElementById('ModalCDisability').innerText = "";

    document.getElementById('ModalCAge').innerText = document.getElementById('Age').value;
    document.getElementById('ModalKnow').innerText = $('#KnowType option:selected').text();
    document.getElementById('ModalFGover').innerText = $('#FoundGoverID option:selected').text();
    document.getElementById('ModalFDis').innerText = $('#FoundDistrictID option:selected').text();
    document.getElementById('ModalFAddress').innerText = document.getElementById('FoundAddress').value;
    document.getElementById('ModalEGover').innerText = $('#ExistGoverID option:selected').text();
    document.getElementById('ModalEDis').innerText = $('#ExistDistrictID option:selected').text();
    document.getElementById('ModalEAddress').innerText = document.getElementById('ExistAddress').value;
    if (document.getElementById('inlineRadio15').checked == true)
        document.getElementById('ModalIsKnown').innerText = "لا";
    if (document.getElementById('inlineRadio20').checked == true)
        document.getElementById('ModalIsKnown').innerText = "نعم";
    document.getElementById('ModalClothDes').innerText = document.getElementById('floatingTextarea2').value;
    document.getElementById('ModalSpecial').innerText = document.getElementById('floatingTextarea3').value;
    if (document.getElementById('inlineRadio6').checked == true)
        document.getElementById('ModalReport').innerText = "لا";
    if (document.getElementById('inlineRadio7').checked == true)
        document.getElementById('ModalReport').innerText = "نعم";
    if (document.getElementById('inlineRadio8').checked == true)
        document.getElementById('ModalReport').innerText = "لا اعرف";

    if (document.getElementById('inlineRadio7').checked == true) {
        document.getElementById('ModalRNo').innerText = document.getElementById('ReportNo').value;
        document.getElementById('ModalPolice').innerText = document.getElementById('PoliceStation').value;
        document.getElementById('ModalRDate').innerText = document.getElementById('ReportDate').value;
        document.getElementById('ModalRImage').innerText = document.getElementById('file3').value;
    }
    else {
        document.getElementById('ModalRNo').innerText = "";
        document.getElementById('ModalPolice').innerText = "";
        document.getElementById('ModalRDate').innerText = "";
        document.getElementById('ModalRImage').innerText = "";
    }  
    var docs = document.getElementsByName('AttachmentDoc');
    var names = document.getElementsByName('AttachmentName');
    var content = "";
    for (var n = 0; n < docs.length; n++) {
        if (names[n].value != "" && names[n].value != null)
            content += '<span >' + docs[n].files[0].name + '</span><label style="margin-right:30px">' + names[n].value + '</label><br />';
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
                if (data.ValidFieldID != "") {
                    document.getElementById(data.ValidFieldID).innerText = data.Message;
                    $("#" + data.FieldID).css('border-color', 'red');
                    $("#example-basic-t-" + data.Step).click();
                    $("#example-basic-t-" + data.Step).addClass('vactive').parent().siblings().find('a').removeClass('vactive');
                    $(window).scrollTop($('#' + data.FieldID).offset().top);

                    if (data.FieldID == "NationalID") {
                        var id = document.getElementsByName("NationalID");
                        for (var i = 0; i < id.length; i++) {
                            $(id[i]).css('border-color', 'red');
                        }
                    }

                    if (data.FieldID == "PhonNo") {
                        var bit = document.getElementsByName("PhonNo");
                        for (var i = 0; i < bit.length; i++) {
                            $(bit[i]).css('border-color', 'red');
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

function ValidateFirstStep() {
   
    var formdata = new FormData($('form').get(0));


    var tag = document.getElementById('updateProgress');
    tag.style.display = 'block';

    $.ajax({
        url: FirstValidateURL,
        type: 'POST',
        data: formdata,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.IsValid) {
            


                tag.style.display = 'none';
            }
            else {
                //swal({
                //    backdrop: false,
                //    title: "خطأ!",
                //    text: data.Message,
                //    type: "error",
                //});
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
                    $("#example-basic-t-" + data.Step).click();
                    $("#example-basic-t-" + data.Step).addClass('vactive').parent().siblings().find('a').removeClass('vactive');
                    $(window).scrollTop($('#' + data.FieldID).offset().top);

                    if (data.FieldID == "NationalID") {
                        var id = document.getElementsByName("NationalID");
                        for (var i = 0; i < id.length; i++) {
                            $(id[i]).css('border-color', 'red');
                        }
                    }

                    if (data.FieldID == "PhonNo") {
                        var bit = document.getElementsByName("PhonNo");
                        for (var i = 0; i < bit.length; i++) {
                            $(bit[i]).css('border-color', 'red');
                        }
                    }

                   
                    if (data.FieldID == "GoverID" || data.FieldID == "DistrictID" ) {
                        $('#' + data.FieldID).siblings(".select2-container").css('border', '1px solid red');
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

function ValidateSecondStep() {
   
    var formdata = new FormData($('form').get(0));


    var tag = document.getElementById('updateProgress');
    tag.style.display = 'block';

    $.ajax({
        url: SecondValidateURL,
        type: 'POST',
        data: formdata,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.IsValid) {



                tag.style.display = 'none';
            }
            else {
                //swal({
                //    backdrop: false,
                //    title: "خطأ!",
                //    text: data.Message,
                //    type: "error",
                //});
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
                    $("#example-basic-t-" + data.Step).click();
                    $("#example-basic-t-" + data.Step).addClass('vactive').parent().siblings().find('a').removeClass('vactive'); 
                    $(window).scrollTop($('#' + data.FieldID).offset().top);

                  

                    if (data.FieldID == "Gender" || data.FieldID == "HealthStat" || data.FieldID == "IsKnown" || data.FieldID == "ReportBit") {
                        var inputs = document.getElementsByName(data.FieldID);
                        for (var i = 0; i < inputs.length; i++) {
                            $(inputs[i]).css('border-color', 'red');
                            $(inputs[i].labels[0]).css('color', 'red');
                        }
                    }

                    if (data.FieldID == "DisTypeID" || data.FieldID == "FoundGoverID" || data.FieldID == "FoundDistrictID" || data.FieldID == "KnowType" || data.FieldID == "ExistGoverID" || data.FieldID == "ExistDistrictID") {
                        $('#' + data.FieldID).siblings(".select2-container").css('border', '1px solid red');
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

function ValidationRefresh(inputid, validid, step) {


    if (inputid == "NationalID") {
        var ids = document.getElementsByName("NationalID");
        for (var i = 0; i < ids.length; i++) {
            $(ids[i]).css('border-color', '');
        }
    }

    if (inputid == "PhonNo") {
        var bit = document.getElementsByName("PhonNo");
        for (var i = 0; i < bit.length; i++) {
            $(bit[i]).css('border-color', '');
        }
    }

    if (inputid == "Gender" || inputid == "HealthStat" || inputid == "IsKnown" || inputid == "ReportBit") {
        var inputs = document.getElementsByName(inputid);
        for (var i = 0; i < inputs.length; i++) {
            $(inputs[i]).css('border-color', '');
            $(inputs[i].labels[0]).css('color', '');
        }
    }


    if (inputid == "GoverID" || inputid == "DistrictID" || inputid == "DisTypeID" || inputid == "FoundGoverID" || inputid == "FoundDistrictID" || inputid == "KnowType" || inputid == "ExistDistrictID" || inputid == "ExistGoverID") {
        $('#' + inputid).siblings(".select2-container").css('border', '');
    }

    document.getElementById(validid).innerText = "";
    $("#" + inputid).css('border-color', '');
    $("#example-basic-t-" + step).removeClass('vactive');
}


$(document).ready(function () {
$("#FoundChildLi").addClass('active-link');
	$("#example-basic").steps({
		headerTag: "h3",
		bodyTag: "section",
        autoFocus: true,
        //enableFinishButton: false,
		onInit: function (event, current) {
            $('.actions > ul > li:first-child').attr('style', 'display:none');
            $('.actions > ul > li:last-child').attr('onclick', 'OpenModal()');
            $('.actions > ul > li:first-child a').addClass('step-previous-btn');

            $("#example-basic-t-0").addClass('active').parent().siblings().find('a').removeClass('active');
            //$(".steps > ul > li").removeClass('disabled');

		},
        onStepChanged: function (event, current, next) {

            if (current == 0)
                $("#example-basic-t-0").addClass('active').parent().siblings().find('a').removeClass('active');
            else if (current == 1) {
                $('.actions > ul > li:nth-child(2)').attr('onclick', 'ValidateSecondStep()');
                $("#example-basic-t-1").addClass('active').parent().siblings().find('a').removeClass('active');
            }
            else if (current == 2) {
                $('.actions > ul > li:nth-child(2)').attr('onclick', 'ValidateFirstStep()');
                $("#example-basic-t-2").addClass('active').parent().siblings().find('a').removeClass('active');
            }

			if (current > 0) {
                $('.actions > ul > li:first-child').attr('style', '');
			} else {
                $('.actions > ul > li:first-child').attr('style', 'display:none');
			}
		}

	});
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
                $idnum.eq($idnum.index(this) - 1).val("");
			}
		}

		if (ignoreChar(e))
			return false;
		else if (this.value.length == this.maxLength) {
            $(this).next('input').focus();
            $(this).next('input').val("");
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


	//$(".steps ul li a").click(function () {

	//	v



	//});

    var Attachmentsinput = document.getElementById('inlineRadio10');

    if (Attachmentsinput.checked) {


        document.getElementById('Attachments-input').style.display = 'block';
        document.getElementById("AddAttachButton").disabled = false;


    } else {

        document.getElementById('Attachments-input').style.display = 'none';
        document.getElementById("AddAttachButton").disabled = true;


    }


    $("#inlineCheckbox-3").click(function () {
        $("#Obstruction-input").toggleClass("input-show");
    });


    $("#GoverID").select2();
    $("#DistrictID").select2();
    $("#ChildNameSource").select2();
    $("#DisTypeID").select2();
    $("#KnowType").select2();
    $("#FoundGoverID").select2();
    $("#FoundDistrictID").select2();
    $("#ExistGoverID").select2();
    $("#ExistDistrictID").select2();



    //$('#Birthdate').datetimepicker({
         //format: 'MM/DD/YYYY'
    //});

    $('#ReportDate').datetimepicker({
        format: 'MM/DD/YYYY',
        date: now

    });

    

});

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;

}


function reportcheck() {

	var yesinput = document.getElementById('inlineRadio7');

	if (yesinput.checked) {


		document.getElementById('RecordDate-input').style.display = 'block';

	} else {

		document.getElementById('RecordDate-input').style.display = 'none';

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

function HealthCheck(input) {
    if (input.checked && input.id.split('-')[1] == 1) {
        document.getElementById("inlineCheckbox-2").checked = false;
        document.getElementById("inlineCheckbox-3").checked = false;
        $("#Obstruction-input").removeClass("input-show");
        document.getElementById("inlineCheckbox-2").disabled = true;
        document.getElementById("inlineCheckbox-3").disabled = true;
    } else {
        document.getElementById("inlineCheckbox-2").disabled = false;
        document.getElementById("inlineCheckbox-3").disabled = false;
    }
}


function FilterDistricts(id1, id2, URL) {
    var selectedval = $('#' + id1).val();

    $("#" + id2).empty();

    $("#" + id2).append(new Option("المركز", "-1"));
    $.ajax({
        type: "Get",
        url: URL,
        data: { GoverID: selectedval },
        success: function (data) {
            for (var obj in data) {
                $("#" + id2).append(new Option(data[obj].Name, data[obj].ID));
            }


        }
    });
}
