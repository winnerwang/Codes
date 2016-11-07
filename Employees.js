$(document).ready(function () {
    var speed = 500;
    //$.showWidnow();
    $("#btnShow").click(function (event) {
        event.stopPropagation();
        //alert("3423");
        //$("#SearchEmployee").fadeIn(500);
        //$("#Search").show();
        //searcht
        $.showWidnow();
    });

    //$("#btnNewCall").click(function (event) {
        
    //    $.showNewCallWindow();
    //});

});

$.showWidnow = function () {
    //$('.theme-popover-mask').fadeIn(100);

    //$("#mask", window.parent.document).fadeIn(100);
    $("#SearchEmployee").fadeIn(200);
}

$.closeWidnow = function () {


    //$('.theme-popover-mask').fadeOut(100);
    //$("#mask", window.parent.document).fadeOut(100);
    $("#SearchEmployee").fadeOut(200);
}
$.showCallWindow = function (IndexGUID, EmployeeID,FirstName, LastName, DOB,UserID) {

    $("#ContentPlaceHolder1_empFirstName").val(FirstName);
    $("#ContentPlaceHolder1_empLastName").val(LastName);
    $("#ContentPlaceHolder1_EmpID").val(EmployeeID);
    $("#ContentPlaceHolder1_EmpDOB").val(DOB);
    //$("#ContentPlaceHolder1_SendReminder_1").attr("checked", false);
    //$("#ContentPlaceHolder1_SendReminder_2").attr("checked", false);
    //$("#ContentPlaceHolder1_SendReminder_3").attr("checked", true);
    $("#ContentPlaceHolder1_IndexGUID").val(IndexGUID);
    $("#ContentPlaceHolder1_UserID").val(UserID);
    $.showWindow1("MakeAppts");

}

$.showNewCallWindow = function (UserID) {

    $("#ContentPlaceHolder1_empFirstName").val("");
    $("#ContentPlaceHolder1_empLastName").val("");
    $("#ContentPlaceHolder1_EmpID").val("");
    $("#ContentPlaceHolder1_EmpDOB").val("");
    //$("#ContentPlaceHolder1_SendReminder_1").attr("checked", false);
    //$("#ContentPlaceHolder1_SendReminder_2").attr("checked", false);
    //$("#ContentPlaceHolder1_SendReminder_3").attr("checked", true);
    $("#ContentPlaceHolder1_IndexGUID").val("");
    $("#ContentPlaceHolder1_UserID").val(UserID);
    $.showWindow1("MakeAppts");

}

$.MakeAppts = function () {

    var IndexGUID = $("#ContentPlaceHolder1_IndexGUID").val();
    var UserID = $("#ContentPlaceHolder1_UserID").val();
    var EmpID = $("#ContentPlaceHolder1_EmpID").val();
    var FirstName = $("#ContentPlaceHolder1_empFirstName").val().replace("\'", "\\\'");
    var LastName = $("#ContentPlaceHolder1_empLastName").val().replace("\'", "\\\'");
    $.ajax({
        type: "post",
        async: false,
        url: "Employees.aspx/doNewCall",
        data: "{'IndexGUID':'" + IndexGUID + "','UserID':'" + UserID  + "','EmployeeID':'" + EmpID + "','FirstName':'" + FirstName + "','LastName':'" + LastName + "'}",
        //data: '{"IndexGUID":"' + IndexGUID + '","UserID":"' + UserID + '","EmployeeID":"' + EmpID + '","FirstName":"' + FirstName + '","LastName":"' + LastName + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        
        success: function (data) {
            var stu = data.d;
            $.each(stu, function (index, va) {
                var obj = va
                if (obj.attributes == "") {

                    $(obj.ID).html(obj.html);
                } else {
                    $(obj.ID).attr("checked", true);
                }

            })

            $.closeWidnow1("MakeAppts");
            window.location.href = window.location.href;
        },
        error: function (err) {
            //$("#msg").css("color", "#FF0000").html("access faield:" + err);
            alert(err);
        }

    });

}
$.showWindow1 = function (ID) {

    $('.theme-popover-mask').fadeIn(1);

    $("#mask", window.parent.document).fadeIn(1);

    $("#" + ID).slideDown(1);
}

$.closeWidnow1 = function (ID) {


    $('.theme-popover-mask').fadeOut(1);
    $("#mask", window.parent.document).fadeOut(1);
    $("#" + ID).slideUp(1);
}