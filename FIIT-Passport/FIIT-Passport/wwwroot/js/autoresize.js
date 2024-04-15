function autoResizeTextarea(textarea) {
    textarea.style.height = "auto"; // Устанавливаем высоту на авто, чтобы пересчитать высоту текстовой области
    textarea.style.height = textarea.scrollHeight + "px"; // Устанавливаем высоту текстовой области равной ее содержимому
}

// Получаем все текстовые области на странице
var textareas = document.querySelectorAll('textarea');

// Проходимся по каждой текстовой области и применяем автоматическое изменение высоты
textareas.forEach(function(textarea) {
    autoResizeTextarea(textarea);

    // Слушаем событие ввода текста в каждой текстовой области и вызываем функцию автоматического изменения высоты
    textarea.addEventListener('input', function() {
        autoResizeTextarea(textarea);
    });
});