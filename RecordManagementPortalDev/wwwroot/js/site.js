// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toggle(id) {
    var state = document.getElementById(id).style.display;
    if (state == 'block') {
        document.getElementById(id).style.display = 'none';
    } else {
        document.getElementById(id).style.display = 'block';
    }
}

function FunCust() {	
	//$("#ULCustomerCode").change(function () {
	var custcode = $("#ULCustomerCode").val();
		$("#ULDepartmentCode").empty();
		$.post("/Customer/GetDept",
			{ custcode: custcode },
			function (response) {
				$("#ULDepartmentCode").append($('<option value="">--All Departments--</option>'));
				$.each(response, function (key, entry) {
					$("#ULDepartmentCode").append($('<option></option>').attr('value', entry.value).text(entry.value));
				})
			});
	////});
}
	   