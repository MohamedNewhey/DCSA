
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