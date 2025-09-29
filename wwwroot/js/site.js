// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Admin, Student, and Instructor sidebar toggle for mobile
(function(){
  var toggle = document.getElementById('sidebarToggle');
  var adminSidebar = document.querySelector('.admin-sidebar');
  var studentSidebar = document.querySelector('.student-sidebar');
  var instructorSidebar = document.querySelector('.instructor-sidebar');

  if (toggle) {
    toggle.addEventListener('click', function(){
      if (adminSidebar) {
        adminSidebar.classList.toggle('open');
      }
      if (studentSidebar) {
        studentSidebar.classList.toggle('open');
      }
      if (instructorSidebar) {
        instructorSidebar.classList.toggle('open');
      }
    });
  }
})();