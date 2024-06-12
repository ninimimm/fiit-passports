const nameToText = {};
document.querySelectorAll('.comment').forEach(element => {
    let span = document.querySelector(`#${element.id}`);
    let input = span.parentNode;
    element.addEventListener('click', function() {
        const isActive = commentItem.classList.contains('active');
        const allComments = document.querySelectorAll('.comment');
        const selectedElements = document.querySelectorAll('.selected_text');
        if (!isActive) {
            nameToText[input.getAttribure('name')] = input.value;
            input.nodeName = 'div';
            selectedElements.forEach(selectedElement => {
                selectedElement.classList.remove('active');
            });
            allComments.forEach(comment => {
                comment.classList.remove('active');
            });
            span.classList.add('active');
            commentItem.classList.add('active');
        } else {
            input.nodeName = 'input';
            input.value = nameToText[input.getAttribure('name')];
            span.classList.remove('active');
            commentItem.classList.remove('active');
        }
    });
})

window.onload = function() {
    resizeTextares();
    document.body.addEventListener('input', function(event) {
        if (event.target.tagName === 'TEXTAREA') {
            autoResizeTextarea(event.target);
        }
    });

    window.addEventListener('resize', function(event) {
        resizeTextares(event.target);
    });
};