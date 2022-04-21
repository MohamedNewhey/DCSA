
function AddToCart(id, Name, Type) {
    var model = new Object();
    model.ID = id;
    model.Name = Name;
    var Amount = 0;
    if (Type == 1) {
        Amount = document.getElementById("donateNumber-" + id).value;
    }
    else
        Amount = document.getElementById("PriceValue-" + id).innerText;

    model.Amount = Amount
    $.ajax({
        type: "Post",

        url: CartURL,
        data: { model: model },
        success: function (data) {

            swal("نجاح!", "تم اضافة للسلة", "success");

            document.getElementById("CartItemsSpan").innerText = data.Count;

        }
    });
}


function AddValueSpan(Stockvalue, WantedCard, left) {
    var CurrentArrow = parseInt(document.getElementById("ArrowNo-" + WantedCard).innerText);
    CurrentArrow++;
    if (CurrentArrow * Stockvalue >= left) {
        return;
    }
    document.getElementById("PriceValue-" + WantedCard).innerText = CurrentArrow * Stockvalue;
    document.getElementById("ArrowNo-" + WantedCard).innerText = CurrentArrow;
}

function MinusValueSpan(Stockvalue, WantedCard) {
    var CurrentArrow = parseInt(document.getElementById("ArrowNo-" + WantedCard).innerText);
    CurrentArrow--;
    if (CurrentArrow <= 0) {
        return;
    }
    document.getElementById("PriceValue-" + WantedCard).innerText = CurrentArrow * Stockvalue;
    document.getElementById("ArrowNo-" + WantedCard).innerText = CurrentArrow;
}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;

}


function RemoveFromCart(id,name) {

    swal({
        title: "هل انت متأكد؟",
        text: "هل تريد مسح " + name + " ؟",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "نعم",
        closeOnConfirm: false
    }, function () {
        $.ajax({
            type: "Post",
            url: DeleteURL,
            data: { id: id },
            success: function (data) {
                $('#DataDiv').html(data);
                swal("نجاح!", "تم المسح", "success");
              
            }
        });

    });

}


function CollectFreeDonation() {
    var Amount = 0;
    Amount = parseInt(document.getElementById("FreeDonationInput").value);
    if (Amount <= 0) {
        swal("فشل!", "من فضلك ادخل قيمة اكبر من ال 0", "error");
    }
    $.ajax({
        type: "Post",
        url: FreeDonation,
        data: { Amount: Amount },
        success: function (data) {

            if (RefreshLink != "No") {
                $.ajax({
                    type: "Get",
                    url: RefreshLink,
                    success: function (ContentData) {
                        $('#DataDiv').html(ContentData);
                        swal("نجاح!", "تم اضافة للسلة", "success");

                        document.getElementById("CartItemsSpan").innerText = data.Count;
                        $("#FastDonation").modal('hide');
                    }
                });
            }
            else {
                swal("نجاح!", "تم اضافة للسلة", "success");

                document.getElementById("CartItemsSpan").innerText = data.Count;
                $("#FastDonation").modal('hide');
            }
        }
    });

}


function GiftClick(CauseID) {
    //Reseting the validation
    document.getElementById('ReceiverName-' + CauseID).style.border = "";
    document.getElementById('Receriveremail-' + CauseID).style.border = "";
    document.getElementById('DonationMessage-' + CauseID).style.border = "";
    document.getElementById('DonationAmount-' + CauseID).style.border = "";

    //Collecting the data
    var Sender = $("#SenderName-" + CauseID).val();
    var RName = $("#ReceiverName-" + CauseID).val();
    var REmail = $("#Receriveremail-" + CauseID).val();
    var DMessage = $("#DonationMessage-" + CauseID).val();
    var DAmount = $("#DonationAmount-" + CauseID).val();

    //Validation
    if (RName == "") {
        document.getElementById('ReceiverName-' + CauseID).style.border = "1px solid red";
        return;
    }
    if (DAmount == "") {
        document.getElementById('DonationAmount-' + CauseID).style.border = "1px solid red";
        return;
    }
    if (DMessage == "") {
        document.getElementById('DonationMessage-' + CauseID).style.border = "1px solid red";
        return;
    }
    if (REmail == "") {
        document.getElementById('Receriveremail-' + CauseID).style.border = "1px solid red";
        return;
    } else {
        if (!isEmail(REmail)) {
            document.getElementById('Receriveremail-' + CauseID).style.border = "1px solid red";
            return;
        }
    }

    var GiftModel = new Object();
    GiftModel.Sender = Sender;
    GiftModel.RName = RName;
    GiftModel.REmail = REmail;
    GiftModel.DMessage = DMessage;
    GiftModel.DAmount = DAmount;
    GiftModel.CauseID = CauseID;


    $.ajax({
        type: "Post",
        url: GiftLink,
        data: { model: GiftModel },
        success: function (data) {
            swal("نجاح!", "تم اضافة للسلة", "success");
            document.getElementById("CartItemsSpan").innerText = data.Count;
            ToggleCard('GiftCard-'+CauseID);
            
        }
    });
}

function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

