

$('#dropDown1').change(function () {

    var id = $('#dropDown1').val();

    $.ajax({

        url: '/Order/AltMarkaGetır',

        data: { p: id },

        type: "POST",

        dataType: "Json",

        success: function (data) {

            console.log(data);

            $('#dropDown2').empty();

            for (var i = 0; i < data.length; i++) {

                $('#dropDown2').append("<option value='" + data[i].Value + "'>" + data[i].Text + "</Option>");

            }

            $("#dropDown2").trigger("liszt:updated");

        }

    });


});




   

