function autoResizeTextarea(textarea) {
    textarea.style.height = "auto";
    textarea.style.height = textarea.scrollHeight + "px";
}

var textareas = document.querySelectorAll('textarea');

textareas.forEach(function(textarea) {
    autoResizeTextarea(textarea);

    textarea.addEventListener('input', function() {
        requestAnimationFrame(function() {
            autoResizeTextarea(textarea);
        });
    });
});