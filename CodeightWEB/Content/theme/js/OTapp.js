// replace these values with those generated in your TokBox Account


var sessionId = "1_MX40NzAzMjY1NH5-MTYxMzM4MjkwNjcxMH40VFRsZUJVd2U0R1pnN0lyKzJRQzluTHd-QX4";
var token = "T1==cGFydG5lcl9pZD00NzAzMjY1NCZzaWc9ZjdhMjBhMWFiY2IyNWY0ODJhNGQ0NDQ0YzRhZDM1MDM5ODMyMmUwYzpzZXNzaW9uX2lkPTFfTVg0ME56QXpNalkxTkg1LU1UWXhNek00TWprd05qY3hNSDQwVkZSc1pVSlZkMlUwUjFwbk4wbHlLekpSUXpsdVRIZC1RWDQmY3JlYXRlX3RpbWU9MTYxMzM4Mjk0MiZub25jZT05ODU5MTYmcm9sZT1QVUJMSVNIRVI=";
var baseUrl = "https://localhost:44397/"



$(document).ready(function () {

    var data = {
        UserId: $('#txtUserId').val(),
        Status: $("#ddlUserStatus option:selected").val(),
        UserStatus: $("#ddlIsActive option:selected").val()
    };

    $.ajax({
        type: "POST",
        url: baseUrl + "Session/CreateSession",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            //Clear all previous list of members  
            $("#MemberList").empty();

            //Display Asp.Net Web API response in console log   
            //You can see console log value in developer tool   
            //by pressing F12 function key.  
            console.log(response);


            // Variable created to store <li>Memeber Detail</li>  
            var ListValue = "";

            //Variable created to iterate the json array values.  
            var i;

            //Generic loop to iterate the response arrays.  
            for (i = 0; i < response.length; ++i) {
                ListValue += "<li>" + response[i].MemberName + " --- " + response[i].PhoneNumber
            }

            //Add/Append the formatted values of ListValue variable into ID called "MemberList"  
            $("#MemberList").append(ListValue);
        },
        failure: function (response) {
            alert(response.responseText);
            alert("Failure");
        },
        error: function (response) {
            alert(response);
            alert("Error");
        }
    });
}); 



debugger;
alert(1);
var apiKey = "47032654";
var sessionId = "1_MX40NzAzMjY1NH5-MTYxMzM4MjkwNjcxMH40VFRsZUJVd2U0R1pnN0lyKzJRQzluTHd-QX4";
var token = "T1==cGFydG5lcl9pZD00NzAzMjY1NCZzaWc9ZjdhMjBhMWFiY2IyNWY0ODJhNGQ0NDQ0YzRhZDM1MDM5ODMyMmUwYzpzZXNzaW9uX2lkPTFfTVg0ME56QXpNalkxTkg1LU1UWXhNek00TWprd05qY3hNSDQwVkZSc1pVSlZkMlUwUjFwbk4wbHlLekpSUXpsdVRIZC1RWDQmY3JlYXRlX3RpbWU9MTYxMzM4Mjk0MiZub25jZT05ODU5MTYmcm9sZT1QVUJMSVNIRVI=";

// Handling all of our errors here by alerting them
function handleError(error) {
    if (error) {
        alert(error.message);
    }
}

// (optional) add server code here
initializeSession();

function initializeSession() {

    debugger;
    alert(2);

    var session = OT.initSession(apiKey, sessionId);

    // Subscribe to a newly created stream
    session.on('streamCreated', function (event) {
        session.subscribe(event.stream, 'subscriber', {
            insertMode: 'append',
            width: '100%',
            height: '100%'
        }, handleError);
    });

    // Create a publisher
    var publisher = OT.initPublisher('publisher', {
        insertMode: 'append',
        width: '100%',
        height: '100%'
    }, handleError);

    // Connect to the session
    session.connect(token, function (error) {

        debugger;
        alert(3);
        // If the connection is successful, initialize a publisher and publish to the session
        if (error) {
            handleError(error);
        } else {
            session.publish(publisher, handleError);
        }
    });
}