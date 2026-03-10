$(document).ready(async function () {

    $("#DateOfBirth").datepicker({
        dateFormat: "dd-mm-yy",
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+0",   // ย้อนหลัง 100 ปี ถึงปีปัจจุบัน
        maxDate: 0              // ห้ามเลือกวันอนาคต
    });



    let fileInput = document.getElementById("Profile_Image");
    let base64Image = "";

    if (fileInput.files.length > 0) {
        base64Image = await toBase64(fileInput.files[0]);
    }

    var data = {
        Firstname: $("#Firstname").val(),
        Lastname: $("#Lastname").val(),
        Email: $("#Email").val(),
        Phone_No: $("#Phone_No").val(),
        Profile_Image: base64Image,
        DateOfBirth: $("#DateOfBirth").val(),
        Occupation: $("#Occupation").val(),
        Gender: document.querySelector('input[name="Gender"]:checked')?.value
    };

    
});

async function submitProfile() {
    let fileInput = document.getElementById("Profile_Image");
    let base64Image = "";

    if (fileInput.files.length > 0) {
        base64Image = await toBase64(fileInput.files[0]);
    }

    //Convert DOB format
    var dob_split = $("#DateOfBirth").val().split('-')
    var dob = dob_split[2] + '-' + dob_split[1] + '-' + dob_split[0];

    var data = {
        Firstname: $("#Firstname").val(),
        Lastname: $("#Lastname").val(),
        Email: $("#Email").val(),
        Phone_No: $("#Phone_No").val(),
        Profile_Image: base64Image,
        DateOfBirth: dob,
        Occupation: $("#Occupation").val(),
        Gender: document.querySelector('input[name="Gender"]:checked')?.value
    };
    if (!checkEmptyData(data)) {
        return; // หยุด ไม่ส่งไปหลังบ้าน
    }

    // ส่งข้อมูลไปหลังบ้าน
    console.log("Sending:", data);

    const response = await fetch("/Profile/SaveProfile", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    });

    const result = await response.json();
    alert(result.message);
    location.reload();



}

function toBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

function checkEmptyData(data) {

    let isValid = true;

    // Firstname
    if (!data.Firstname) {
        showError("Firstname", "Please provide a valid Firstname");
        isValid = false;
    } else {
        showError("Firstname", "");
    }

    // Email
    if (!data.Email) {
        showError("Email", "Please provide a valid Email");
        isValid = false;
    }
    else if (!isValidEmail(data.Email)) {
        showError("Email", "Email format is invalid");
        isValid = false;
    }
    else {
        showError("Email", "");
    }

    // Phone
    if (!data.Phone_No) {
        showError("Phone_No", "Please provide a valid Phone Number");
        isValid = false;
    } else {
        showError("Phone_No", "");
    }

    // Date of Birth
    if (!data.DateOfBirth) {
        showError("DateOfBirth", "Please provide a valid Birth Date");
        isValid = false;
    } else {
        showError("DateOfBirth", "");
    }

    // Occupation
    if (!data.Occupation) {
        showError("Occupation", "Please select an Occupation");
        isValid = false;
    } else {
        showError("Occupation", "");
    }

    // Gender
    if (!data.Gender) {
        showError("Gender", "Please select Gender");
        isValid = false;
    } else {
        showError("Gender", "");
    }

    return isValid;
}

function showError(field, message) {
    document.getElementById(field + "-error").innerText = message;
}

function isValidEmail(email) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}

function clearProfile() {

    // ล้าง input type="text"
    $("#Firstname").val("");
    $("#Lastname").val("");
    $("#Email").val("");
    $("#Phone_No").val("");

    // ล้าง date
    $("#DateOfBirth").val("");

    // ล้าง dropdown
    $("#Occupation").val("");

    // ล้าง file input
    $("#Profile_Image").val("");

    // ล้าง radio
    const genderRadios = document.querySelectorAll('input[name="Gender"]');
    genderRadios.forEach(r => r.checked = false);

    // ล้าง error message ใต้ทุกช่อง
    const errorFields = [
        "Firstname",
        "Lastname",
        "Email",
        "Phone_No",
        "DateOfBirth",
        "Occupation",
        "Gender",
        "Profile_Image"
    ];

    errorFields.forEach(f => {
        const el = document.getElementById(f + "-error");
        if (el) el.innerText = "";
    });
}
