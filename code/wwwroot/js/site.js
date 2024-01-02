// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




document.addEventListener("DOMContentLoaded", function() {
    var navToggle = document.querySelector('.nav-toggle');
    var nav = document.querySelector('.nav');

    navToggle.addEventListener('click', function() {
        nav.classList.toggle('nav--visible');
    });
});
