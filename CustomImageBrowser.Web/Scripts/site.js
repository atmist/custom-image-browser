$(function () {
    tinymce.init({
        selector: "textarea",
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks code fullscreen",
            "insertdatetime media table contextmenu paste"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link imagebrowser",

        height: '600',
        setup: function (ed) {
            ed.addButton('imagebrowser', {
                title: 'Insert Image',
                icon: 'image',
                onclick: function () {
                    $.get($.url('Media/ImageBrowser'), function (html) {
                        var popup = $('<img>').magnificPopup({
                            items: {
                                src: html
                            },
                            modal: true,
                            callbacks: {
                                open: function () {
                                    $(document).trigger('init-uploader');
                                }
                            }
                        });
                        $.magnificPopup.instance.close()
                        popup.magnificPopup('open');
                    });
                }
            });
        }
    });

    $(document).on('init-uploader', function () {
        if ($("#file-uploader").length > 0) {
            var loading = $('.image-browser-controls .spinner-container');
            var uploader = new qq.FileUploader({
                element: document.getElementById('file-uploader'),
                allowedExtensions: ['jpg', 'jpeg', 'png'],
                sizeLimit: 10485760,
                action: $.url('Media/Upload'),
                onSubmit: function (file, extension) {
                    if (loading.length) {
                        loading.addClass('loading');
                    }
                },
                onComplete: function (id, fileName, responseJSON) {
                    if (loading.length) {
                        loading.removeClass('loading');
                    }

                    $('.image-browser-empty').removeClass('active');
                    $("#image-browser").append(responseJSON.data).addClass('active');
                },
                onCancel: function (id, fileName) {
                    if (loading.length) {
                        loading.removeClass('loading');
                    }
                }
            });
        }
    });
});

//For selecting an image and inserting it into the editor
$(document).on('click', '.image-browser-img', function () {
    if ($.magnificPopup != undefined) {
        $.magnificPopup.instance.close();
    }

    var img = $(this).find('img');
    img = '<img src="' + img.attr('data-url') + '" alt="' + img.prop('alt') + '"/>';
    tinyMCE.activeEditor.execCommand('mceInsertContent', false, img);
});


//Deleting an image in the image browser
$(document).on('submit', '.image-browser-delete', function () {
    var form = $(this);
    $.ajax({
        type: 'POST',
        cache: 'false',
        url: form.prop('action'),
        data: form.serialize(),
        success: function (responseJson) {
            $('#image-browser-' + responseJson.data).remove();
            if ($('#image-browser').children(':not(.image-browser-empty, .image-browser-tip)').length <= 0) {
                $('.image-browser-empty').addClass('active');
                $('#image-browser').removeClass('active');
            }
        }
    });
    return false;
});
