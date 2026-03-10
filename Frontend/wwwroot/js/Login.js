$(document).ready(function () {

        $("#loginForm").submit(function (e) {
            e.preventDefault(); // ป้องกัน submit แบบปกติ


            var data = {
                Username: $("#username").val(),
                Password: $("#password").val()
            };

            $.ajax({
                url: "/Login/Authenticate",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(data),
                success: function (resp) {
                    if (resp.isSuccess) {
                        // ไปหน้า Dashboard
                        window.location.href = "/Home/Index";
                    } else {
                        alert(resp.errorMessage || "Login failed");
                    }
                },
                error: function (xhr) {
                    alert("Server error: " + xhr.status);
                }
            });
        });

    });