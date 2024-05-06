const columns = document.querySelectorAll('.column');
let dragged = null;

columns.forEach((column) => {
    column.addEventListener('dragstart', (e) => {
        dragged = e.target;
    });

    column.addEventListener('dragover', (e) => {
        e.preventDefault();
    });

    column.addEventListener('drop', (e) => {
       if (dragged && dragged.parentNode !== e.target && e.target.classList.contains('column')) {
           dragged.parentNode.removeChild(dragged);
           e.target.appendChild(dragged);
       }
    });
});