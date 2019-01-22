$(document).ready(function () {
    if ($("[CountryId]").val() === '') {
        $("[ResortId]").empty();
        $("[HotelId]").empty();
    }

    $("[CountryId]").change(function () {
        $("[HotelId]").empty();
        var url = "?handler=CascadingResorts";
        $.getJSON(url, { countryId: $("[CountryId]").val() }, function (data) {
            var items = "<option value=''>- Выберите -</option>";
            $("[ResortId]").empty();
            $.each(data, function (i, resort) {
                items += "<option value='" + resort.value + "'>" + resort.text + "</option>";
            });
            $("[ResortId]").html(items);
        });
    });

    $("[ResortId]").change(function () {
        var url = "?handler=CascadingHotels";
        $.getJSON(url, { resortId: $("[ResortId]").val() }, function (data) {
            var items = "<option value=''>- Выберите -</option>";
            $("[HotelId]").empty();
            $.each(data, function (i, hotel) {
                items += "<option value='" + hotel.value + "'>" + hotel.text + "</option>";
            });
            $("[HotelId]").html(items);
        });
    });
});