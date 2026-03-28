// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.onscroll = function () { myFunction() };

var header = document.getElementById("myHeader");
var sticky = header.offsetTop;

function myFunction() {
    if (window.pageYOffset > sticky) {
        header.classList.add("sticky");
    } else {
        header.classList.remove("sticky");
    }
}


function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}
function confirmEdit(uniqueId, isDeleteClicked) {
    var MainSpan = 'mainSpan_' + uniqueId;
    var confirmEditSpan = 'confirmEdit_' + uniqueId;

    if (isDeleteClicked) {
        $('#' + MainSpan).hide();
        $('#' + confirmEditSpan).show();
    } else {
        $('#' + MainSpan).show();
        $('#' + confirmEditSpan).hide();
    }
}

