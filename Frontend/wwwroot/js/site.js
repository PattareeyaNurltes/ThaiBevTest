// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    var userId = $("#userId").val();

    $.ajax({
        url: "/Profile/GetProfileImage/" + userId,
        type: "GET",
        success: function (resp) {
            
            if (resp && resp.profile_image) {
                localStorage.setItem("profile_image", resp.profile_image);

                $('.profile-circle').css({
                    'background-image': 'url(' + resp.profile_image + ')',
                    'background-size': 'cover',
                    'background-position': 'center',
                    'color': 'transparent' // ซ่อนตัวอักษร
                });

            }
        },
        error: function (xhr) {
            console.error("Error loading profile image:", xhr.status);
        }
    });
});