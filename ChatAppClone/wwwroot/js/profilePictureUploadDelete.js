$(document).ready(function () {
    $('#upload-button').on('click', function (e) {
        e.preventDefault();

        var fileInput = $('#profile-picture-input')[0];
        var file = fileInput.files[0];

        var formData = new FormData();
        formData.append('file', file);

        $('#loading-overlay').show();

        $.ajax({
            url: '@Url.Action("UploadProfilePicture", "User")',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    $('#validation-message').text(response.error);
                }
            },
            error: function () {
                $('#validation-message').text('An error occurred while uploading the picture.');
            },
            complete: function () {
                $('#loading-overlay').hide();
            }
        });
    });

    $('#remove-profile-picture-btn').on('click', function (e) {
        e.preventDefault();

        $('#removePictureModal').modal('hide');

        $('#loading-overlay').show();

        $.ajax({
            url: '@Url.Action("DeleteProfilePicture", "User")',
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    $('#validation-message').text(response.error);
                }
            },
            error: function () {
                $('#validation-message').text('An error occurred while deleting the picture.');
            },
            complete: function () {
                $('#loading-overlay').hide();
            }
        });
    });

    $('#remove-button').on('click', function (e) {
        e.preventDefault();

        $('#removePictureModal').modal('show');
    });

    $('#removePictureModal .close').on('click', function () {
        $('#removePictureModal').modal('hide');
    });

    document.getElementById('removePictureModal').addEventListener('show.bs.modal', function () {
        document.body.classList.add('modal-open');
    });

    document.getElementById('removePictureModal').addEventListener('hidden.bs.modal', function () {
        document.body.classList.remove('modal-open');
    });
});
