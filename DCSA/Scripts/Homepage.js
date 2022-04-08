

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