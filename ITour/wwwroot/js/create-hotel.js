$("#CreateHotelBtn").on('click', function () {
    var url = "?handler=CreateHotel";
    $.getJSON(url,
        {
            name: $("[HotelName]").val(),
            nameEn: $("[HotelNameEn]").val(),
            resortId: $("[ResortId]").val()
        }, function (data) {
            var items = "<option value=''>- Выберите -</option>";
            $("[HotelId]").empty();
            var selected = "";
            $.each(data, function (i, hotel) {
                if (hotel.selected)
                    selected = "selected";
                items += "<option " + selected + " value='" + hotel.value + "'>" + hotel.text + "</option>";
            });
            $("[HotelId]").html(items);
        });
    $("#CreateHotelModal").modal('hide')
});