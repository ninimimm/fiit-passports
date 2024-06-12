function autoResizeTextarea(textarea) {
    textarea.style.height = "auto";
    textarea.style.height = textarea.scrollHeight + 10 + "px";
}
function resizeTextares() {
    const textareas = document.querySelectorAll('textarea');
    textareas.forEach(function(textarea) {
        autoResizeTextarea(textarea);

        textarea.addEventListener('input', function() {
            requestAnimationFrame(function() {
                autoResizeTextarea(textarea);
            });
        });
    });
}
