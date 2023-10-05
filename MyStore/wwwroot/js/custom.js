function setActive() {

    
    // Get the current page name (URL pathname) using window.location
    var currentPageName = window.location.pathname;
    console.log(currentPageName);

    if (currentPageName == "/Index" || currentPageName == "/") var element = document.getElementById("homeContainer").classList.add("active");
    else if (currentPageName == "/Account/Register") var element = document.getElementById("registerContainer").classList.add("active");
    else if (currentPageName == "/Account/Login") var element = document.getElementById("loginContainer").classList.add("active");
    else if (currentPageName.startsWith("/Clients")) var element = document.getElementById("clientsContainer").classList.add("active");
    else if (currentPageName == "/Privacy") var element = document.getElementById("privacyContainer").classList.add("active");
    else if (currentPageName == "/Account/Profile") var element = document.getElementById("profileContainer").classList.add("active");

    //element.classList.add("active");
    // Note: The JavaScript code in this onclick event does not return anything to the server
};

// Use the DOMContentLoaded event to execute the function when the page loads
document.addEventListener('DOMContentLoaded', function () {
    setActive();
});

function logoutUser()
{
    $.ajax({
        type: "GET", // Or "GET" depending on your requirements
        url: "/Account/Profile.cshtml.cs/Logout", // Replace with the actual Razor Page and function names
        success: function (data) {
            // Handle the success response if needed
            console.log("Click event handled successfully" + data);
   
        },
        error: function (error) {
            // Handle the error response if needed
            console.error("Error handling click event: ", error);
        }
    });

}