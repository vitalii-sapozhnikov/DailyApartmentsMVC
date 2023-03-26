// Auto-complete input for country
$(function () {
    $("#country").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Home/CountryAutoComplete",
                dataType: "json",
                data: {
                    search: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 3, // Minimum length of input before autocomplete starts
        menu: '<ul class="ui-autocomplete ui-menu"></ul>',
        open: function (event, ui) {
            // Move the dropdown menu below the input field
            var $menu = $(event.target).data("ui-autocomplete").menu.element;
            $menu.position({
                my: "left top",
                at: "left bottom",
                of: event.target
            });

        }
    });
});

// Auto-complete input for city
$(function () {
    $("#city").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Home/CityAutoComplete",
                dataType: "json",
                data: {
                    search: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 3, // Minimum length of input before autocomplete starts
        menu: '<ul class="ui-autocomplete ui-menu"></ul>',
        open: function (event, ui) {
            // Move the dropdown menu below the input field
            var $menu = $(event.target).data("ui-autocomplete").menu.element;
            $menu.position({
                my: "left top",
                at: "left bottom",
                of: event.target
            });

        }
    });
});


$(function () {
    $('#daterange').daterangepicker({
        opens: 'left',
        locale: {
            format: 'DD/MM/YYYY'
        },
        minDate: moment()
    }, function (start, end, label) {
        //console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
    });
});




