$(document).ready(function () {
    $('#upload').change(function () {

        var blobFile = $('#upload').prop("files")[0];
        var formData = new FormData();
        var byteArray;
        formData.append("Image", blobFile);

        $.ajax({
            url: resolveUrl("~/Perfil/saveProfilePicture"),
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                var reader = new FileReader();
                reader.readAsDataURL(blobFile);
                reader.onload = function () {
                    byteArray = reader.result;
                    $("#uploadedAvatar").attr("src", byteArray);
                };
            },
            error: function (msg) {
                Swal.fire(
                    'Error',
                    msg.responseText,
                    'error',
                    'timer:5000'
                ).then((dismiss) => {

                })
            }
        });
    });

    function getBase64(file) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            console.log(reader.result);
        };
        reader.onerror = function (error) {
            console.log('Error: ', error);
        };
    }

});