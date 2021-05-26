//    Main Namespace = SA
var dataAccess = {};

////    This object is responsible to provide constants throughout the project
////    SA Constants
dataAccess.constants = {

    login: "/User/Login",
    logout: "/User/Logout",
    forgotPassword: "/User/ForgotPassword",
    changePassword: "/User/ChangePassword",
    markVerified: "/User/markVerifiedUser",
    deleteVideo: "/Session/DeleteVideo",
    deleteUser: "/User/DeleteUser",
    addPhysician: "/Physician/addNewPhysician",
    editRadius: "/Session/ManageRadius",
    addNewDiscipline: "/ListManagement/addNewDiscipline",
    addNewTreatmentTeam: "/ListManagement/addNewTreatmentTeam",
    addNewCredential: "/ListManagement/addNewCredential",
    deleteSpecialty: "/ListManagement/deleteSpecialty",
    deleteDiscipline: "/ListManagement/deleteDiscipline",
    deleteTreatment: "/ListManagement/deleteTreatment",
    deleteCredential: "/ListManagement/deleteCredential",
    editListItem: "/ListManagement/Edit",
    EditPhysician: "/Physician/EditPhysician",
    EditUser: "/User/EditUser",
    EditUserStatus: "/User/ViewUser",
    sendNotifications: "/Notification/PushNotifcation"
};

////    This object is responsible to provide all authentication related stuff
////    Login Service

dataAccess.loginService = {
   
    login: function () {

        debugger;
        //alert(1);


        $("#LoginErrorMessage").text("");
        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }
        //$("#loading").show();

         $(".loading").fadeIn("slow");

            var remember = $('#ckRemember').prop('checked') ? true : false;
             //   debugger;
                //alert(2);
                $.ajax({
                    type: 'post',
                    data: { Email: $("#email").val(), Password: $("#password").val(), RememberMe: remember },
                    url: dataAccess.constants.login,
                    success: function (res) {
                        //alert(res);
                        debugger;
                        var Result = JSON.parse(res);
                       
                        if (Result === "404") {
                            debugger;
                            setTimeout(function () {
                                $(".loading").fadeOut("slow");
                            }, 3000);
                            $("#LoginErrorMessage").text(Result.Message);
                            return;
                        }
                        else if (Result === "400") {
                            debugger;
                            setTimeout(function () {
                                $(".loading").fadeOut("slow");
                            }, 3000);
                            $("#ForgotErrorMessage").text(Result.Message);
                            return;
                        }
                        else if (Result === "200") {
                            debugger;
                            //$("#loading").hide();
                            //UserManagemenListAndsearch
                            window.location.href = "/User/UserManagement";
                         //   $(".loading").fadeOut("slow");
                            setTimeout(function () {
                                $(".loading").fadeOut("slow");
                            }, 8000);
                        }
                    }
                });
    
    },

    forgotPassword: function () {
        //$('#preloader').fadeIn("slow");
        //NProgress.start();

        debugger;
        //alert(1);
        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        $.ajax({
            type: 'post',
            data: { email: $("#emailForForgotPassword").val() },
            url: dataAccess.constants.forgotPassword,
            success: function (res) {
               // alert(res);
                debugger;
              //  NProgress.done();
                var Result = JSON.parse(res);

                if (Result.Code === "400") {

                    swal("Server side alert 400", Result.Message, "warning");
                    return;

                } else if (Result.Code === "404") {

                    swal("Server side alert 404", Result.Message, "warning");
                    return;
                }

                else {
                    //  alert("Record deleted");
                    swal({ title: "Email Sent", text: Result.Message, type: "success", allowEscapeKey: false, allowOutsideClick: false },
                        function () {
                            $("#emailForForgotPassword").val("");
                        }
                    );
                }

            }
        });

    },

    logout: function () {
        debugger;
        swal({
            title: "Alert",
            text: "Are you sure you want to logout",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Logout",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    $.ajax({
                        type: 'get',
                        url: dataAccess.constants.logout,
                        async: false,
                        success: function (res) {
                            debugger;
                     //       var Result = JSON.parse(res);
                            if (res.Code === "200") {
                                swal({ title: "Done", text: "You are logged out!", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/User/Login';
                                        //  NProgress.done();
                                    }
                                );
                            }

                        }
                    });


                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  

    },

    markVerified: function (UserId, userStatus, previousCheckboxValue) {
        //$('#preloader').fadeIn("slow");
        //NProgress.start();
        debugger;


        swal({
            title: "Alert",
            text: "Are you sure you want to change user status?",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ok",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    debugger;
                    //var data = { UserId: UserId, isChecked: isChecked };
                    
                    $.ajax({
                        type: 'post',
                      //  dataType: 'html',
                      //  contentType: 'application/json; charset=utf-8',
                       // data: JSON.stringify(data),
                        //data: { Email: $("#email").val(), Password: $("#password").val(), RememberMe: remember }
                        data: { UserId: UserId, userStatus: userStatus },
                        url: dataAccess.constants.markVerified,
                        async: false,
                        //   traditional: true,
                        success: function (res) {
                            //  NProgress.done();
                            debugger;
                            var Result = JSON.parse(res);

                            if (Result.Code === "400") {

                                swal("Server side error 400", Result.Message, "error");
                                return;

                            } else if (Result.Code === "404") {
                                swal("Server side error 404", Result.Message, "error");
                                return;
                            }
                            else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Selected user status changed.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/User/UserManagement';
                                    }
                                );
                            }

                        }
                    });

                }
                else {
                  //  debugger;
                    if (userStatus == 0)
                        previousCheckboxValue.checked = true;
                    else
                        previousCheckboxValue.checked = false;
                       
                }
            });  

        //var data = { checkedList: arrayList };

        //$.ajax({
        //    type: 'post',
        //    dataType: 'html',
        //    contentType: 'application/json; charset=utf-8',
        //    data: JSON.stringify(data),
        //    url: dataAccess.constants.markVerified,
        // //   traditional: true,
        //    success: function (res) {
        //        //  NProgress.done();
        //        debugger;
        //        var Result = JSON.parse(res).toString();
        //        if (Result.Code === "404") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //   $('#preloader').fadeOut();
        //            return;
        //        }
        //        else if (Result.Code === "400") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //     $('#preloader').fadeOut();
        //            return;
        //        }
        //        else {
        //            alert("Selected users verified");
        //            window.location.href = '/User/ManagemenListAndsearch';
        //        }
        //    }
        //});
    }

 
};

dataAccess.userService = {

    //not in use
    EditUser: function () {
        //debugger;
        //alert(1);
        var data = {
            UserId: $('#txtUserId').val(),
            Status: $("#ddlUserStatus option:selected").val(),
            UserStatus: $("#ddlIsActive option:selected").val()
        };


        //$('#myfrm').submit(function (e) {
        //    e.preventDefault();
        //    if ($('#myfrm').parsley().isValid()) {

        $.ajax({
            url: dataAccess.constants.EditUser,
            type: 'POST',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            success: function (res) {
                var Result = JSON.parse(res);
                //debugger;
                //alert(1);
                //$('#preloader').fadeOut();
                //$("#ModalHeading").text("Message");
                //$(".ErrorMessage").text("Successfully Added");
                //$('#ErrorModal').modal('show');

                //$('#ErrorModal').on('hidden.bs.modal', function () {
                if (Result.Code === "400") {
                    return;
                } else if (Result.Code === "200") {
                    window.location.href = '/Physician/physiciansManagementList';
                }
                //});
            }
        });
    },

    EditUserStatus: function () {
        debugger;


        swal({
            title: "Confirmation?",
            text: "are you sure you want to edit record!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Edit",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    

                    debugger;
                    var data = {
                        UserId: $('#txtUserId').val(),
                        UserStatus: $("#ddlUserStatus option:selected").val(),
                        IsActive: $("#ddlIsActive option:selected").val() == 0 ? false : true
                    };

                    $.ajax({
                        url: dataAccess.constants.EditUserStatus,
                        async: false,
                        type: 'POST',
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        datatype: "json",
                        success: function (res) {
                            var Result = JSON.parse(res);
                            debugger;

                            if (Result.Code === "404") {
                                swal("Server side error 404", Result.Message, "error");
                                return;
                            }
                            else if (Result.Code === "400") {
                                swal("Server side error 400", Result.Message, "error");
                                return;
                            }
                            else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Edit", text: "Data has been successfully edited!", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/User/ManagemenListAndsearch';
                                    }
                                );
                            }

                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  

        //var data = {
        //    UserId: $('#txtUserId').val(),
        //    UserStatus: $("#ddlUserStatus option:selected").val(),
        //    IsActive: $("#ddlIsActive option:selected").val()
        //};

        //$.ajax({
        //    url: dataAccess.constants.EditUserStatus,
        //    type: 'POST',
        //    data: JSON.stringify(data),
        //    contentType: "application/json; charset=utf-8",
        //    datatype: "json",
        //    success: function (res) {
        //        var Result = JSON.parse(res);
        //        //debugger;
        //        //alert(1);
        //        //$('#preloader').fadeOut();
        //        //$("#ModalHeading").text("Message");
        //        //$(".ErrorMessage").text("Successfully Added");
        //        //$('#ErrorModal').modal('show');

        //        //$('#ErrorModal').on('hidden.bs.modal', function () {
        //        if (Result.Code === "400") {
        //            return;
        //        } else if (Result.Code === "200") {
        //            window.location.href = '/Physician/physiciansManagementList';
        //        }
        //        //});
        //    }
        //});

    },

    deleteUser: function (UserId) {
        //  $('#preloader').fadeIn("slow");
        //alert(1);
        debugger;

        swal({
            title: "Alert",
            text: "Are you sure you want to delete this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Delete",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    var data = {
                        UserId: UserId
                    };

                    $.ajax({
                        url: dataAccess.constants.deleteUser,
                        async: false,
                        type: 'POST',
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        datatype: "json",
                        success: function (res) {
                            var Result = JSON.parse(res);
                            debugger;
                            if (Result.Code === "400") {

                                swal("Server side error", Result.Message, "error");
                                return;

                            } else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is deleted.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/User/UserManagement';
                                    }
                                );
                            }
                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });


    }

};

dataAccess.SessionService = {

    deleteVideo: function (archiveId) {
        //  $('#preloader').fadeIn("slow");
        //alert(1);
        //debugger;

        swal({
            title: "Alert",
            text: "Are you sure you want to delete this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Delete",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    var data = {
                        ArchiveId: archiveId
                    };

                    $.ajax({
                        url: dataAccess.constants.deleteVideo,
                        async: false,
                        type: 'POST',
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        datatype: "json",
                        success: function (res) {
                            var Result = JSON.parse(res);
                            debugger;
                            if (Result.Code === "400") {

                                swal("Server side error", Result.Message, "error");
                                return;

                            } else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is deleted.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/Session/VideoManagement';
                                    }
                                );
                            }
                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  


    },

    addPhysician: function (selectedCredentials, selectedLanguages, selectedDisciplines, selectedSpecialties, selectedTreatmentTeams) {


        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }
      //  $(".loading").fadeIn("slow");

        debugger;
     //   alert(1);

        //var data = new FormData();
        //var files = $("#imageUpload").get(0).files;

       // var imgToUploadd = $("#imageUpload").get(0).files[0];

        //var file = $("#imageUpload").get(0).files[0];
        //var formData = new FormData();
        //formData.set("file", file, file.name);

           
        //var dataa = {
        // //   imageUpload: formData,
        //    physicianCredentials: JSON.stringify(selectedCredentials),
        //    Status: $("#ddlStatus option:selected").val(),
        //    FirstName: $("#txtFirstName").val(),
        //    LastName: $("#txtLastName").val(),
        //    DirectPhone: $("#txtDirectPhone").val(),
        //    OfficePhone: $("#txtOfficePhone").val(),
        //    Email: $("#txtEmail").val(),
        //    physicianLanguage: JSON.stringify(selectedLanguages),
        //    JoiningDate: $("#txtJoiningDate").val(),
        //    physicianDiscipline: JSON.stringify(selectedDisciplines),
        //    physicianSpecialties: JSON.stringify(selectedSpecialties),
        //    physicianTreatmentTeam: JSON.stringify(selectedTreatmentTeams)
        //};


        swal({
            title: "Alert",
            text: "Are you sure you want to add this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Add",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    var file = $("#imageUpload").get(0).files;

                    var data = new FormData;
                    data.append("imageUpload", file[0]);
                    data.append("physicianCredentials", JSON.stringify(selectedCredentials));
                    data.append("Status", $("#ddlStatus option:selected").val());
                    data.append("FirstName", $("#txtFirstName").val());
                    data.append("LastName", $("#txtLastName").val());
                    data.append("DirectPhone", $("#txtDirectPhone").val());
                    data.append("OfficePhone", $("#txtOfficePhone").val());
                    data.append("Email", $("#txtEmail").val());
                    data.append("physicianLanguage", JSON.stringify(selectedLanguages));
                    data.append("JoiningDate", $("#txtJoiningDate").val());
                    data.append("physicianDiscipline", JSON.stringify(selectedDisciplines));
                    data.append("physicianSpecialties", JSON.stringify(selectedSpecialties));
                    data.append("physicianTreatmentTeam", JSON.stringify(selectedTreatmentTeams));

                    //  data.append("physician", dataa);

                    $.ajax({
                        url: dataAccess.constants.addPhysician,
                        async: false,
                        type: 'POST',
                        data: data,
                        //  contentType: "multipart/form-data",
                        processData: false,  // tell jQuery not to process the data
                        contentType: false,
                        //    datatype: "json",
                        success: function (res) {
                            var Result = JSON.parse(res);
                            debugger;

                            if (Result.Code === "400") {

                                swal("Server side error", Result.Message, "error");
                                $("#ErrorMessage").text(Result.Message);
                                return;

                            } else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is added.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/Physician/physiciansManagementList';
                                    }
                                );
                            }
                            //});
                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  

      //  var file = $("#imageUpload").get(0).files;

      //  var data = new FormData;
      //  data.append("imageUpload", file[0]);
      //  data.append("physicianCredentials", JSON.stringify(selectedCredentials));
      //  data.append("Status", $("#ddlStatus option:selected").val());
      //  data.append("FirstName", $("#txtFirstName").val());
      //  data.append("LastName", $("#txtLastName").val());
      //  data.append("DirectPhone", $("#txtDirectPhone").val());
      //  data.append("OfficePhone", $("#txtOfficePhone").val());
      //  data.append("Email", $("#txtEmail").val());
      //  data.append("physicianLanguage", JSON.stringify(selectedLanguages));
      //  data.append("JoiningDate", $("#txtJoiningDate").val());
      //  data.append("physicianDiscipline", JSON.stringify(selectedDisciplines));
      //  data.append("physicianSpecialties", JSON.stringify(selectedSpecialties));
      //  data.append("physicianTreatmentTeam", JSON.stringify(selectedTreatmentTeams));

      ////  data.append("physician", dataa);

      //          $.ajax({
      //              url: dataAccess.constants.addPhysician,
      //              type: 'POST',
      //              data: data,
      //            //  contentType: "multipart/form-data",
      //              processData: false,  // tell jQuery not to process the data
      //              contentType: false,
      //          //    datatype: "json",
      //              success: function (res) {
      //                  var Result = JSON.parse(res);
      //                  debugger;
      //                  //$('#preloader').fadeOut();
      //                  //$("#ModalHeading").text("Message");
      //                  //$(".ErrorMessage").text("Successfully Added");
      //                  //$('#ErrorModal').modal('show');

      //                  //$('#ErrorModal').on('hidden.bs.modal', function () {
      //                  if (Result.Code === "400") {
      //                      $("#ErrorMessage").text(Result.Message);
      //                      setTimeout(function () {
      //                          $(".loading").fadeOut("slow");
      //                      }, 2000);
      //                      return;
      //                  } else if (Result.Code === "200") {
      //                      $("#ErrorMessage").text(Result.Message);
      //                    //  window.location.href = '/Physician/physiciansManagementList';
      //                      window.location.href = '/User/MainDashboard';
      //                      setTimeout(function () {
      //                          $(".loading").fadeOut("slow");
      //                      }, 2000);
      //                  }
      //                  //});
      //              }
      //          });

 


    },

    EditPhysician: function (selectedCredentials, selectedLanguages, selectedDisciplines, selectedSpecialties, selectedTreatmentTeams) {

        debugger;
     //   alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }
       // $(".loading").fadeIn("slow");

        //var data = {
        //    PhysicianId: $('#txtPhysicianId').val(),
        //    ProfileImage: $("#imageUpload").val(),
        //    physicianCredentials: JSON.stringify(selectedCredentials),
        //    Status: $("#ddlStatus option:selected").val(),
        //    FirstName: $("#txtFirstName").val(),
        //    LastName: $("#txtLastName").val(),
        //    DirectPhone: $("#txtDirectPhone").val(),
        //    OfficePhone: $("#txtOfficePhone").val(),
        //    Email: $("#txtEmail").val(),
        //    physicianLanguage: JSON.stringify(selectedLanguages),
        //    JoiningDate: $("#txtJoiningDate").val(),
        //    physicianDiscipline: JSON.stringify(selectedDisciplines),
        //    physicianSpecialties: JSON.stringify(selectedSpecialties),
        //    physicianTreatmentTeam: JSON.stringify(selectedTreatmentTeams)

        //};


        swal({
            title: "Alert",
            text: "Are you sure you want to edit this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Edit",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    debugger;
                    var file = $("#imageUpload").get(0).files;

                    var data = new FormData;
                    data.append("ProfileImage", $('#txtProfileImage').val());
                    data.append("PhysicianId", $('#txtPhysicianId').val());
                    data.append("imageUpload", file[0]);
                    data.append("physicianCredentials", JSON.stringify(selectedCredentials));
                    data.append("Status", $("#ddlStatus option:selected").val());
                    data.append("FirstName", $("#txtFirstName").val());
                    data.append("LastName", $("#txtLastName").val());
                    data.append("DirectPhone", $("#txtDirectPhone").val());
                    data.append("OfficePhone", $("#txtOfficePhone").val());
                    data.append("Email", $("#txtEmail").val());
                    data.append("physicianLanguage", JSON.stringify(selectedLanguages));
                    data.append("JoiningDate", $("#txtJoiningDate").val());
                    data.append("physicianDiscipline", JSON.stringify(selectedDisciplines));
                    data.append("physicianSpecialties", JSON.stringify(selectedSpecialties));
                    data.append("physicianTreatmentTeam", JSON.stringify(selectedTreatmentTeams));


                    //$('#myfrm').submit(function (e) {
                    //    e.preventDefault();
                    //    if ($('#myfrm').parsley().isValid()) {

                    $.ajax({
                        url: dataAccess.constants.EditPhysician,
                        async: false,
                        type: 'POST',
                        data: data,
                        //   contentType: "application/json; charset=utf-8",
                        processData: false,  // tell jQuery not to process the data
                        contentType: false,
                        //       datatype: "json",
                        success: function (res) {
                            var Result = JSON.parse(res);
                            debugger;

                            if (Result.Code === "400") {

                                swal("Server side error", Result.Message, "error");
                                $("#ErrorMessage").text(Result.Message);
                                return;

                            }

                            else if (Result.Code === "404") {

                                swal("Server side error", Result.Message, "error");
                                $("#ErrorMessage").text(Result.Message);
                                return;

                            }
                            else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is updated.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/Physician/physiciansManagementList';
                                    }
                                );
                            }

                        }
                    });


                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  

        //var file = $("#imageUpload").get(0).files;

        //var data = new FormData;
        //data.append("ProfileImage", $('#txtProfileImage').val());
        //data.append("PhysicianId", $('#txtPhysicianId').val());
        //data.append("imageUpload", file[0]);
        //data.append("physicianCredentials", JSON.stringify(selectedCredentials));
        //data.append("Status", $("#ddlStatus option:selected").val());
        //data.append("FirstName", $("#txtFirstName").val());
        //data.append("LastName", $("#txtLastName").val());
        //data.append("DirectPhone", $("#txtDirectPhone").val());
        //data.append("OfficePhone", $("#txtOfficePhone").val());
        //data.append("Email", $("#txtEmail").val());
        //data.append("physicianLanguage", JSON.stringify(selectedLanguages));
        //data.append("JoiningDate", $("#txtJoiningDate").val());
        //data.append("physicianDiscipline", JSON.stringify(selectedDisciplines));
        //data.append("physicianSpecialties", JSON.stringify(selectedSpecialties));
        //data.append("physicianTreatmentTeam", JSON.stringify(selectedTreatmentTeams));


        ////$('#myfrm').submit(function (e) {
        ////    e.preventDefault();
        ////    if ($('#myfrm').parsley().isValid()) {

        //        $.ajax({
        //            url: dataAccess.constants.EditPhysician,
        //            type: 'POST',
        //            data: data,
        //         //   contentType: "application/json; charset=utf-8",
        //            processData: false,  // tell jQuery not to process the data
        //            contentType: false,
        //     //       datatype: "json",
        //            success: function (res) {
        //                var Result = JSON.parse(res);
        //                //debugger;
        //                //alert(1);
        //                //$('#preloader').fadeOut();
        //                //$("#ModalHeading").text("Message");
        //                //$(".ErrorMessage").text("Successfully Added");
        //                //$('#ErrorModal').modal('show');

        //                //$('#ErrorModal').on('hidden.bs.modal', function () {
        //                if (Result.Code === "400") {
        //                    setTimeout(function () {
        //                        $(".loading").fadeOut("slow");
        //                    }, 2000);
        //                    return;
        //                } else if (Result.Code === "200") {
        //                    //debugger;
        //                    //alert(1);
        //                    //window.location.href = '/Physician/physiciansManagementList';
        //                    window.location.href = '/User/MainDashboard';
        //                    setTimeout(function () {
        //                        $(".loading").fadeOut("slow");
        //                    }, 2000);
        //                }
        //                //});
        //            }
        //        });

        //    }

        //});

    }


};

dataAccess.RadiusManagementService = {


    editRadius: function () {

        debugger;
        //alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }


        swal({
            title: "Alert",
            text: "Are you sure you want to change this record?",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Change",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    debugger;
                    //, previousRadiusId: document.getElementById("txtPreviousValue").textContent 
                    //var data = { radiusId: $("#ddlManageRadius option:selected").val(), previousRadiusId: $("#ddlPreviousValue").val() };

                    //if ($("#txtPreviousId").val() === "undefined")
                    //    var data = { radiusValue: $("#txtManageRadius").val(), PreviousId: 0 };
                    //else
                        var data = { radiusValue: $("#txtManageRadius").val(), PreviousId: $("#txtPreviousId").val() };
                //    var getdata = { specialty: data };

                    $.ajax({
                        type: 'post',
                        data: data,
                        url: dataAccess.constants.editRadius,
                        async: false,
                        success: function (res) {
                            debugger;
                            //alert(2);
                            //  NProgress.done();
                            var Result = JSON.parse(res);
                            if (Result.Code === "404") {
                                swal("Alert!", Result.Message, "warning");
                                return;
                            }
                            else if (Result.Code === "400") {
                                swal("Alert!", Result.Message, "warning");
                                return;
                            }
                            else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is added.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/Session/ManageRadius';
                                    }
                                );
                            }


                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });



    },

    addSpecialty: function () {

        debugger;
        //alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }


        swal({
            title: "Alert",
            text: "Are you sure you want to add this record?",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Add",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    debugger;
                    var data = { SpecialtyTitle: $("#txtSpecialty").val() };

                    var getdata = { specialty: data };

                    $.ajax({
                        type: 'post',
                        data: getdata,
                        url: dataAccess.constants.addNewSpecialty,
                        async: false,
                        success: function (res) {
                            debugger;
                            //alert(2);
                            //  NProgress.done();
                            var Result = JSON.parse(res);
                            if (Result.Code === "404") {
                                swal("Alert!", Result.Message, "warning");
                                return;
                            }
                            else if (Result.Code === "400") {
                                swal("Alert!", Result.Message, "warning");
                                return;
                            }
                            else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is added.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/ListManagement/PhysicianListManagement#tab1';
                                    }
                                );
                            }

                           
                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  


        //var data = { SpecialtyTitle: $("#txtSpecialty").val() };

        //var getdata = { specialty: data };

        //$.ajax({
        //    type: 'post',
        //    data: getdata,
        //    url: dataAccess.constants.addNewSpecialty,
        //    success: function (res) {
        //        debugger;
        //        alert(2);
        //        //  NProgress.done();
        //        var Result = JSON.parse(res).toString();
        //        if (Result.Code === "404") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //   $('#preloader').fadeOut();
        //            return;
        //        }
        //        else if (Result.Code === "400") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //     $('#preloader').fadeOut();
        //            return;
        //        }
        //        else {
        //            window.location.href = '/ListManagement/PhysicianListManagement#tab1';
        //        }
        //    }
        //});

    },

    addDiscipline: function () {

        //debugger;
        //alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        swal({
            title: "Alert?",
            text: "Are you sure you want to add this record?",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Add",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    var data = { DisciplineTitle: $("#txtDiscipline").val() };

                    var getdata = { discipline: data };

                    $.ajax({
                        type: 'post',
                        data: getdata,
                        url: dataAccess.constants.addNewDiscipline,
                        async: false,
                        success: function (res) {
                            debugger;
                        //    alert(2);
                            //  NProgress.done();
                            var Result = JSON.parse(res);
                            if (Result.Code === "404") {
                                swal("Alert!", Result.Message, "warning");
                                return;
                            }
                            else if (Result.Code === "400") {
                                swal("Alert!", Result.Message, "warning");
                                return;
                            }
                            else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is added.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/ListManagement/PhysicianListManagement#tab2';
                                    }
                                );
                            }

                        }
                    });


                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  


        //var data = { DisciplineTitle: $("#txtDiscipline").val() };

        //var getdata = { discipline: data };

        //$.ajax({
        //    type: 'post',
        //    data: getdata,
        //    url: dataAccess.constants.addNewDiscipline,
        //    success: function (res) {
        //        debugger;
        //        alert(2);
        //        //  NProgress.done();
        //        var Result = JSON.parse(res).toString();
        //        if (Result.Code === "404") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //   $('#preloader').fadeOut();
        //            return;
        //        }
        //        else if (Result.Code === "400") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //     $('#preloader').fadeOut();
        //            return;
        //        }
        //        else {
        //           window.location.href = '/ListManagement/PhysicianListManagement#tab2';
        //            //$('#tab2').tab('show');
        //        }
        //    }
        //});

    },

    addTreatmentTeam: function () {

        debugger;
        //alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        swal({
            title: "Alert",
            text: "Are you sure you want to add this record?",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Add",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    debugger;
                    var data = { DiseaseTitle: $("#txtTreatment").val() };

                    var getdata = { treatment: data };

                    $.ajax({
                        type: 'post',
                        data: getdata,
                        url: dataAccess.constants.addNewTreatmentTeam,
                        async: false,
                        success: function (res) {
                            debugger;
                            //alert(2);
                            //  NProgress.done();
                            var Result = JSON.parse(res);
                            if (Result.Code === "404") {
                                swal("Alert!", Result.Message, "warning");
                                return;
                            }
                            else if (Result.Code === "400") {
                                swal("Alert!", Result.Message, "warning");
                                return;
                            }
                            else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is added.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/ListManagement/PhysicianListManagement#tab3';
                                    }
                                );
                            }

                        }
                    });


                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  


        //   var data = { DiseaseTitle: $("#txtTreatment").val() };

        //   var getdata = { treatment: data };

        //$.ajax({
        //    type: 'post',
        //    data: getdata,
        //    url: dataAccess.constants.addNewTreatmentTeam,
        //    success: function (res) {
        //        debugger;
        //        alert(2);
        //        //  NProgress.done();
        //        var Result = JSON.parse(res).toString();
        //        if (Result.Code === "404") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //   $('#preloader').fadeOut();
        //            return;
        //        }
        //        else if (Result.Code === "400") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //     $('#preloader').fadeOut();
        //            return;
        //        }
        //        else {
        //            window.location.href = '/ListManagement/PhysicianListManagement#tab3';
        //        }
        //    }
        //});

    },

    addCredential: function () {

        //debugger;
        //alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        swal({
            title: "Alert",
            text: "Are you sure you want to add this record?",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Add",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    var data = { CredentialsTitle: $("#txtCredential").val() };

                    var getdata = { credentials: data };

                    $.ajax({
                        type: 'post',
                        data: getdata,
                        url: dataAccess.constants.addNewCredential,
                        async: false,
                        success: function (res) {
                            //debugger;
                            //alert(2);
                            //  NProgress.done();
                            var Result = JSON.parse(res);
                            if (Result.Code === "404") {
                                swal("Alert!", Result.Message, "warning");
                                return;
                            }
                            else if (Result.Code === "400") {
                                swal("Alert!", Result.Message, "warning");
                                return;
                            }
                            else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is added.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/ListManagement/PhysicianListManagement#tab4';
                                    }
                                );
                            }

                        }
                    });


                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  


        //var data = { CredentialsTitle: $("#txtCredential").val() };

        //var getdata = { credentials: data };

        //$.ajax({
        //    type: 'post',
        //    data: getdata,
        //    url: dataAccess.constants.addNewCredential,
        //    success: function (res) {
        //        debugger;
        //        alert(2);
        //        //  NProgress.done();
        //        var Result = JSON.parse(res).toString();
        //        if (Result.Code === "404") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //   $('#preloader').fadeOut();
        //            return;
        //        }
        //        else if (Result.Code === "400") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //     $('#preloader').fadeOut();
        //            return;
        //        }
        //        else {
        //            window.location.href = '/ListManagement/PhysicianListManagement#tab4';
        //        }
        //    }
        //});

    },

    deleteSpecialty: function (SpecialtyId) {
        //  $('#preloader').fadeIn("slow");
        //alert(1);
        //debugger;

        swal({
            title: "Alert",
            text: "Are you sure you want to delete this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Delete",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    var data = {
                        SpecialtyId: SpecialtyId
                    };

                          $.ajax({
                              url: dataAccess.constants.deleteSpecialty,
                              async: false,
                            type: 'POST',
                            data: JSON.stringify(data),
                            contentType: "application/json; charset=utf-8",
                            datatype: "json",
                            success: function (res) {
                                var Result = JSON.parse(res);
                                debugger;
                                if (Result.Code === "400") {

                                    swal("Server side error", Result.Message, "error");
                                    return;

                                } else if (Result.Code === "200") {
                                    //  alert("Record deleted");
                                    swal({ title: "Done", text: "Record is deleted", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                        function () {
                                            window.location.href = '/ListManagement/PhysicianListManagement#tab1';
                                            location.reload();
                                        }
                                    );
                                }
                            }
                        });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  


        //var data = {
        //    SpecialtyId: SpecialtyId
        //};

        //$.ajax({
        //    url: dataAccess.constants.deleteSpecialty,
        //    type: 'POST',
        //    data: JSON.stringify(data),
        //    contentType: "application/json; charset=utf-8",
        //    datatype: "json",
        //    success: function (res) {
        //        var Result = JSON.parse(res);
        //        debugger;
        //        if (Result.Code === "400") {

        //            $('#ErrorModal').on('hidden.bs.modal', function () {
        //                $('#preloader').fadeOut();
        //                $("#ModalHeading").text("Message");

        //                $(".ErrorMessage").text(Result.Message);
        //                $('#ErrorModal').modal('show');

        //            });
        //            return;
        //        } else if (Result.Code === "200") {
        //            alert("Record deleted");
        //            window.location.href = '/ListManagement/PhysicianListManagement#tab1';
        //            location.reload(); 
        //        }
        //    }
        //});

    },

    deleteDiscipline: function (DisciplineId) {
        //  $('#preloader').fadeIn("slow");
        //alert(1);
        //debugger;


        swal({
            title: "Alert",
            text: "Are you sure you want to delete this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Delete",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    var data = {
                        DisciplineId: DisciplineId
                    };

                    $.ajax({
                        url: dataAccess.constants.deleteDiscipline,
                        async: false,
                        type: 'POST',
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        datatype: "json",
                        success: function (res) {
                            var Result = JSON.parse(res);
                            debugger;
                            if (Result.Code === "400") {

                                swal("Server side error", Result.Message, "error");
                                return;

                            } else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is deleted", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/ListManagement/PhysicianListManagement#tab2';
                                        location.reload();
                                    }
                                );
                            }
                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  

        //var data = {
        //    DisciplineId: DisciplineId
        //};

        //$.ajax({
        //    url: dataAccess.constants.deleteDiscipline,
        //    type: 'POST',
        //    data: JSON.stringify(data),
        //    contentType: "application/json; charset=utf-8",
        //    datatype: "json",
        //    success: function (res) {
        //        var Result = JSON.parse(res);
        //        debugger;
        //        if (Result.Code === "400") {

        //            $('#ErrorModal').on('hidden.bs.modal', function () {
        //                $('#preloader').fadeOut();
        //                $("#ModalHeading").text("Message");

        //                $(".ErrorMessage").text(Result.Message);
        //                $('#ErrorModal').modal('show');

        //            });
        //            return;
        //        } else if (Result.Code === "200") {
        //            alert("Record deleted");
        //            window.location.href = '/ListManagement/PhysicianListManagement#tab2';
        //            location.reload(); 
        //            //setTimeout(function () {// wait for 5 secs(2)
        //            //    location.reload(); // then reload the page.(3)
        //            //}, 2000); 
        //        }
        //    }
        //});

    },

    deleteTreatment: function (TreatmentId) {
        //  $('#preloader').fadeIn("slow");
        //alert(1);
        //debugger;



        swal({
            title: "Alert?",
            text: "Are you sure you want to delete this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Delete",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    var data = {
                        TreatmentId: TreatmentId
                    };

                    $.ajax({
                        url: dataAccess.constants.deleteTreatment,
                        async: false,
                        type: 'POST',
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        datatype: "json",
                        success: function (res) {
                            var Result = JSON.parse(res);
                            debugger;
                            if (Result.Code === "400") {

                                swal("Server side error", Result.Message, "error");
                                return;

                            } else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is deleted", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/ListManagement/PhysicianListManagement#tab3';
                                        location.reload();
                                    }
                                );
                            }
                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  

        //var data = {
        //    TreatmentId: TreatmentId
        //};

        //$.ajax({
        //    url: dataAccess.constants.deleteDiscipline,
        //    type: 'POST',
        //    data: JSON.stringify(data),
        //    contentType: "application/json; charset=utf-8",
        //    datatype: "json",
        //    success: function (res) {
        //        var Result = JSON.parse(res);
        //        debugger;
        //        if (Result.Code === "400") {

        //            $('#ErrorModal').on('hidden.bs.modal', function () {
        //                $('#preloader').fadeOut();
        //                $("#ModalHeading").text("Message");

        //                $(".ErrorMessage").text(Result.Message);
        //                $('#ErrorModal').modal('show');

        //            });
        //            return;
        //        } else if (Result.Code === "200") {
        //            alert("Record deleted");
        //            window.location.href = '/ListManagement/PhysicianListManagement#tab3';
        //            location.reload(); 
        //        }
        //    }
        //});

    },
    
    deleteCredential: function (CredentialId) {
        //  $('#preloader').fadeIn("slow");
        //alert(1);
        //debugger;

        swal({
            title: "Alert",
            text: "Are you sure you want to delete this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Delete",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    var data = {
                        CredentialId: CredentialId
                    };

                    $.ajax({
                        url: dataAccess.constants.deleteCredential,
                        async: false,
                        type: 'POST',
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        datatype: "json",
                        success: function (res) {
                            var Result = JSON.parse(res);
                            debugger;
                            if (Result.Code === "400") {

                                swal("Server side error", Result.Message, "error");
                                return;

                            } else if (Result.Code === "200") {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is deleted", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/ListManagement/PhysicianListManagement#tab4';
                                        location.reload();
                                    }
                                );
                            }
                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  


        //var data = {
        //    CredentialId: CredentialId
        //};

        //$.ajax({
        //    url: dataAccess.constants.deleteCredential,
        //    type: 'POST',
        //    data: JSON.stringify(data),
        //    contentType: "application/json; charset=utf-8",
        //    datatype: "json",
        //    success: function (res) {
        //        var Result = JSON.parse(res);
        //        debugger;
        //        if (Result.Code === "400") {

        //            $('#ErrorModal').on('hidden.bs.modal', function () {
        //                $('#preloader').fadeOut();
        //                $("#ModalHeading").text("Message");

        //                $(".ErrorMessage").text(Result.Message);
        //                $('#ErrorModal').modal('show');

        //            });
        //            return;
        //        } else if (Result.Code === "200") {
        //            alert("Record deleted");
        //            window.location.href = '/ListManagement/PhysicianListManagement#tab4';
        //            location.reload(); 
        //        }
        //    }
        //});

    },

    editListManagement: function () {

        debugger;
        //alert(1);


        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        swal({
            title: "Alert",
            text: "Are you sure you want to edit this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Edit",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    var dataSpecialty = { SpecialtyId: $("#txtSpecialtyId").val(), SpecialtyTitle: $("#txtSpecialtyTitle").val() };

                    var dataDiscipline = { DisciplineId: $("#txtDisciplineId").val(), DisciplineTitle: $("#txtDisciplineTitle").val() };

                    var dataTreatmentTeam = { TreatmentTeamId: $("#txtTreatmentTeamId").val(), DiseaseTitle: $("#txtDiseaseTitle").val() };

                    var dataCredentials = { CredentialsId: $("#txtCredentialsId").val(), CredentialsTitle: $("#txtCredentialsTitle").val() };

                    var getdata;
                    if (dataSpecialty.SpecialtyId !== undefined)
                        getdata = { specialties: dataSpecialty };
                    else if (dataDiscipline.DisciplineId !== undefined)
                        getdata = { discipline: dataDiscipline };
                    else if (dataTreatmentTeam.TreatmentTeamId !== undefined)
                        getdata = { treatmentTeam: dataTreatmentTeam };
                    else if (dataCredentials.CredentialsId !== undefined)
                        getdata = { credentials: dataCredentials };


                    $.ajax({
                        type: 'post',
                        data: getdata,
                        url: dataAccess.constants.editListItem,
                        async: false,
                        success: function (res) {
                            debugger;
                            //alert(2);
                            //  NProgress.done();
                            var Result = JSON.parse(res);

                            if (Result.Code === "400") {

                                swal("Server side error", Result.Message, "error");
                                return;

                            } else if (Result.Code === "404") {

                                swal("Server side error", Result.Message, "error");
                                return;
                            }

                            else {
                                //  alert("Record deleted");
                                swal({ title: "Done", text: "Record is updated.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/ListManagement/PhysicianListManagement#tab1';
                                    }
                                );
                            }


                        }
                    });


                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            },
        )  

        //var dataSpecialty = { SpecialtyId: $("#txtSpecialtyId").val(), SpecialtyTitle: $("#txtSpecialtyTitle").val() };

        //var dataDiscipline = { DisciplineId: $("#txtDisciplineId").val(), DisciplineTitle: $("#txtDisciplineTitle").val() };

        //var dataTreatmentTeam = { TreatmentTeamId: $("#txtTreatmentTeamId").val(), DiseaseTitle: $("#txtDiseaseTitle").val() };

        //var dataCredentials = { CredentialsId: $("#txtCredentialsId").val(), CredentialsTitle: $("#txtCredentialsTitle").val() };

        //var getdata;
        //if (dataSpecialty.SpecialtyId !== undefined)
        //    getdata = { specialties: dataSpecialty };
        //else if (dataDiscipline.DisciplineId !== undefined)
        //    getdata = { discipline: dataDiscipline };
        //else if (dataTreatmentTeam.TreatmentTeamId !== undefined)
        //    getdata = { treatmentTeam: dataTreatmentTeam };
        //else if (dataCredentials.CredentialsId !== undefined)
        //     getdata = { credentials: dataCredentials };


        //$.ajax({
        //    type: 'post',
        //    data: getdata,
        //    url: dataAccess.constants.editListItem,
        //    success: function (res) {
        //        debugger;
        //        alert(2);
        //        //  NProgress.done();
        //        var Result = JSON.parse(res);
        //        if (Result.Code === "404") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //   $('#preloader').fadeOut();
        //            return;
        //        }
        //        else if (Result.Code === "400") {
        //            $("#ForgotErrorMessage").text(Result.Message);
        //            //     $('#preloader').fadeOut();
        //            return;
        //        }
        //        else {
        //             window.location.href = '/ListManagement/PhysicianListManagement#tab1';
        //        }
        //    }
        //});

    }



};

dataAccess.changePasswordService = {

    updatePassword: function () {
      //  $('#preloader').fadeIn("slow");

        //debugger;
        //alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        var data = {
            OldPassword: $("#CurrentPassword").val(),
            NewPassword: $("#NewPassword").val(),
            ConfirmPassword: $("#ConfirmPassword").val()
        };

        var getdata = { model: data };

        $.ajax({
            type: 'post',
            data: getdata,
            url: dataAccess.constants.changePassword,
            success: function (res) {
                var Result = JSON.parse(res);
                //debugger;
                //alert(1);
                if (Result.Code === "404") {
                //    $("#ErrorMessage").text(Result.Message);
                    swal("Server side error 404", Result.Message, "warning");
                    //   $('#preloader').fadeOut();
                    return;
                }
                else if (Result.Code === "400") {
                 //   $("#ErrorMessage").text(Result.Message);
                    swal("Server side error 400", Result.Message, "warning");
                    //     $('#preloader').fadeOut();
                    return;
                }
                else {
                    //window.location.href = '/User/Login';
                    $("#CurrentPassword").val("");
                    $("#NewPassword").val("");
                    $("#ConfirmPassword").val("");
               //   $("#ErrorMessage").text(Result.Message);
                    swal("Done", Result.Message, "success");
                }

              //  $('#preloader').fadeOut();
                //if (res === "Password has been successfully updated.")
                //    $("#ModalHeading").text("Success!");
                //else
                //    $("#ModalHeading").text("Message");

                //$(".ErrorMessage").text(Result.Message);
                //$('#ErrorModal').modal('show');

                //$('#ErrorModal').on('hidden.bs.modal', function () {
                //    if (res === "Password Successfully updated")
                //        window.location.href = '/User/Login';
                //});
            },
            error: function () {
                alert("error");
            }
        });
    }
};

dataAccess.Notifications = {


    send: function (data) {

      //  $('#preloader').fadeIn("slow");

        //let Reciever;

        //if ($(".SendTo option:selected").text() === "Specific Driver") {
        //    Reciever = $('.Drivers option:selected').val();
        //} else if ($(".SendTo option:selected").text() === "All Drivers") {
        //    Reciever = "All";
        //}
        debugger;
        alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        //var model = {
        //    Title: $("#txtHeading").val(),
        //    Message: $("#txtMessage").val(),
        //    Data: data,
        //    Reciever: "All",
        //    Sender: "Admin"
        //};

        $.ajax({
            url: dataAccess.constants.sendNotifications,
            type: 'POST',
          //  data: JSON.stringify(model),
            data: { title: $("#txtTitle").val(), message: $("#txtMessage").val() },
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            success: function (res) {
                var Result = JSON.parse(res);
                alert(1);
                debugger;
                $('#preloader').fadeOut();
                $("#ModalHeading").text("Message");

                $(".ErrorMessage").text(Result.Message);
                $('#ErrorModal').modal('show');

                $('#ErrorModal').on('hidden.bs.modal', function () {
                    if (Result.Code === "400") {
                        return;
                    } else if (Result.Code === "200") {
                        window.location.href = '/Dashboard/Notifications';
                    }
                });
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    },

    sendNotification : function () {

        //  $('#preloader').fadeIn("slow");

        //let Reciever;

        //if ($(".SendTo option:selected").text() === "Specific Driver") {
        //    Reciever = $('.Drivers option:selected').val();
        //} else if ($(".SendTo option:selected").text() === "All Drivers") {
        //    Reciever = "All";
        //}
        debugger;
        //alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        //var model = {
        //    Title: $("#txtTitle").val(),
        //    Message: $("#txtMessage").val(),
        //    Data: data,
        //    Reciever: "All",
        //    Sender: "Admin"
        //};

        swal({
            title: "Alert",
            text: "Are you sure you want to send notification?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Send",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {


                    $.ajax({
                        url: dataAccess.constants.sendNotifications,
                        async: false,
                        type: 'POST',
                        //   data: JSON.stringify(model),
                        data: { title: $("#txtTitle").val(), message: $("#txtMessage").val() },
                        //    contentType: "application/json; charset=utf-8",
                        //   datatype: "json",
                        success: function (res) {
                            var Result = JSON.parse(res);
                         //   alert(Result);
                            debugger;

                            swal({ title: "Done", text: "Notification is sent successfully.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                function () {
                                    $("#txtTitle").val("");
                                    $("#txtMessage").val("");
                                    window.location.href = '/Notification/PushNotifcation';
                                }
                            );

                            //if (Result.Code === "404") {
                            //    //    $("#ErrorMessage").text(Result.Message);
                            //    swal("Server side error 404", Result.Message, "warning");
                            //    //   $('#preloader').fadeOut();
                            //    return;
                            //}
                            //else if (Result.Code === "400") {
                            //    //   $("#ErrorMessage").text(Result.Message);
                            //    swal("Server side error 400", Result.Message, "warning");
                            //    //     $('#preloader').fadeOut();
                            //    return;
                            //}
                            //else {
                            //    //window.location.href = '/User/Login';
                            //    $("#CurrentPassword").val("");
                            //    $("#NewPassword").val("");
                            //    $("#ConfirmPassword").val("");
                            //    //   $("#ErrorMessage").text(Result.Message);
                            //   // swal("Done", Result.Message, "success");
                            //    swal({ title: "Done", text: "Notification sent.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                            //        function () {
                            //            $("#txtTitle").val("");
                            //            $("#txtMessage").val("");
                            //            window.location.href = '/Notification/PushNotifcation';
                            //        }
                            //    );
                            //}

                            
                          
                        },
                        error: function (response) {
                            alert(response.responseText);
                        }
                    });
                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            },
        );  

    
    }

};


dataAccess.firebase = {

    firebaseConfiguration: function () {

        //alert(3);
        //debugger;

        if (firebase.apps.length === 0) {
            firebase.initializeApp({
                apiKey: "AIzaSyDAIClUTArtgilemoNhhG3DL5lUWbeNjVk",
                authDomain: "cancerdirectory-51565.firebaseapp.com",
                projectId: "cancerdirectory-51565"
            });
        } else {
            firebase.app();
        }

        dataAccess.Temp.db = firebase.firestore();
        return dataAccess.Temp.db;
    }

};

function getNotifiyMessage(that) {

    var mytitle = 'Regular Success';
    var mytext = 'That thing that you were trying to do worked!';

    return { title: mytitle, text: mytext, type: 'success', styling: 'bootstrap3' };
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}
