// Auto-complete input for country
$(function () {
    $("#country").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Guest/CountryAutoComplete",
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
                url: "/Guest/CityAutoComplete",
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

// Date Range
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

// Register form switch
$(function () {
    $('#isPropertyOwner').change(function () {

        if ($(this).is(':checked')) {
            $("#isPropertyOwner").val(true);

            $('#property-owner-form').show();
            $('#guest-form').hide();

            // set required attribute for owner form inputs
            $('#OwnerName').prop('required', true);
            $('#OwnerSurname').prop('required', true);
            $('#OwnerEmail').prop('required', true);
            $('#OwnerUserName').prop('required', true);
            $('#OwnerPassword').prop('required', true);
            $('#OwnerPassportID').prop('required', true);
            $('#OwnerTaxNumber').prop('required', true);
            $('#OwnerPhoneNumber').prop('required', true);
            

            // remove required attribute for guest form inputs
            $('#Name').prop('required', false);
            $('#Surname').prop('required', false);
            $('#Email').prop('required', false);
            $('#UserName').prop('required', false);
            $('#Password').prop('required', false);
            $('#DateOfBirth').prop('required', false);
        } else {
            $("#isPropertyOwner").val(false);

            $('#guest-form').show();
            $('#property-owner-form').hide();

            // set required attribute for guest form inputs
            $('#Name').prop('required', true);
            $('#Surname').prop('required', true);
            $('#Email').prop('required', true);
            $('#UserName').prop('required', true);
            $('#Password').prop('required', true);

            // remove required attribute for owner form inputs
            $('#OwnerName').prop('required', false);
            $('#OwnerSurname').prop('required', false);
            $('#OwnerEmail').prop('required', false);
            $('#OwnerUserName').prop('required', false);
            $('#OwnerPassword').prop('required', false);
            $('#OwnerPassportID').prop('required', false);
            $('#OwnerTaxNumber').prop('required', false);
            $('#OwnerPhoneNumber').prop('required', false);

        }
    });
});


$(function () {
    $('#isPO').change(function () {
        if ($(this).is(':checked')) {
            $("#isPO").val(true);
        }
        else {
            $("#isPO").val(false);
        }
    });
});



const actualBtn = document.getElementById('actual-btn');

const fileChosen = document.getElementById('file-chosen');

actualBtn.addEventListener('change', function () {
    if (this.files.length == 1)
        fileChosen.textContent = this.files[0].name
    else
        fileChosen.textContent = `${this.files.length} фотографії`
})


const actualBtn1 = document.getElementById('certificate-input');

const fileChosen1 = document.getElementById('file-chosen1');

actualBtn1.addEventListener('change', function () {
    fileChosen1.textContent = this.files[0].name;
})





