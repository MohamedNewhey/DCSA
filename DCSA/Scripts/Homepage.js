


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

            swal("نجاح!", data.Message, "success");

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