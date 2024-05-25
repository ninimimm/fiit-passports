const nameToText = {};
console.log(document.querySelectorAll('.comment'));
document.querySelectorAll('.comment').forEach(element => {
    console.log(element);
    let span = document.querySelector(`#${element.id}`);
    let input = span.parentNode;
    element.addEventListener('click', function() {
        const isActive = commentItem.classList.contains('active');
        const allComments = document.querySelectorAll('.comment');
        const selectedElements = document.querySelectorAll('.selected_text');
        if (!isActive) {
            nameToText[input.getAttribure('name')] = input.value;
            input.nodeName = 'div';
            console.log(input);
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