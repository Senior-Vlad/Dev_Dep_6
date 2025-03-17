// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    let toggleBtn = document.getElementById("toggleNavbar");
    let navbarContent = document.getElementById("navbarContent");

    toggleBtn.addEventListener("click", function () {
        navbarContent.classList.toggle("navbar-visible");
    });
});
