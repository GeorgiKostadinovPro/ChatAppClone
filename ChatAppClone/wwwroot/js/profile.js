$(document).ready(function () {
    $('.menu').on('click', function () {
        $('.bar').toggleClass('animate');
        $('.expand-menu').toggleClass('animate');
        $('.expand-menu .nav-link').toggleClass('animate');
        setTimeout(function () {
            $('.expand-menu .nav-link').toggleClass('animate-show');
        }, 500);
    });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#imagePreview').css('background-image', 'url(' + e.target.result + ')');
                $('#imagePreview').hide();
                $('#imagePreview').fadeIn(650);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#imageUpload").change(function () {
        readURL(this);
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
