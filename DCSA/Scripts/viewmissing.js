$(document).ready(function () {

	$("#example-basic").steps({
		headerTag: "h3",
		bodyTag: "section",
        autoFocus: true,
        enableFinishButton: false,
		onInit: function (event, current) {
            $('.actions > ul > li:first-child').attr('style', 'display:none');
            //$('.actions > ul > li:last-child').attr('onclick', 'CollectData()');
            $('.actions > ul > li:first-child a').addClass('step-previous-btn');

            $("#example-basic-t-0").addClass('active').parent().siblings().find('a').removeClass('active');
            $(".steps > ul > li").removeClass('disabled');
		},
		onStepChanged: function (event, current, next) {

            //var ButtonCheck = document.getElementById('rem');

            //if (current == 1 && ButtonCheck == null) {
            //    // console.log("ok");
            //    var saveA = $("<button>").attr('id', "rem").attr('class', 'finishbtn').attr('type', 'button').attr('onclick', 'CollectData()').text("حفظ");
            //    var saveBtn = $("<li>").attr("aria-disabled", false).append(saveA);

            //    $(document).find(".actions ul").prepend(saveBtn)
            //} else {
            //    $(".actions").find("").remove();
            //}

			if (current==0)
				$("#example-basic-t-0").addClass('active').parent().siblings().find('a').removeClass('active');
			else if (current == 1) 
				$("#example-basic-t-1").addClass('active').parent().siblings().find('a').removeClass('active');
			else if (current == 2)
				$("#example-basic-t-2").addClass('active').parent().siblings().find('a').removeClass('active');
			//$(".steps ul li a").addClass('active').parent().siblings().find('a').removeClass('active');

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




    if (document.getElementById("inlineCheckbox-3").checked){
         $("#Obstruction-input").toggleClass("input-show");
    }

    

$("#DisTypeID").select2();
$("#GoverID").select2();
$("#DistrictID").select2();
$("#LossClass").select2();
$("#LossGoverID").select2();
    $("#LossDistrictID").select2();

  
    $('#Birthdate').datepicker({
        autoclose: true,
        todayHighlight: true
    });

   

    $('#LossDate').datepicker({
        autoclose: true,
        todayHighlight: true
    });

    $('#ReportDate').datepicker({
        autoclose: true,
        todayHighlight: true
    });

    var yesinput = document.getElementById('inlineRadio7');

    if (yesinput.checked) {


        document.getElementById('RecordDate-input').style.display = 'block';

    } else {

        document.getElementById('RecordDate-input').style.display = 'none';

    }

});













