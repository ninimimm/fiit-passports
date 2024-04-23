document.addEventListener('DOMContentLoaded', function() {
    const selectedElements = document.querySelectorAll('.selected_text, .selected_block');
    const commentList = document.querySelector('.comment_list');
    const inputs = document.querySelectorAll('input, textarea');

    selectedElements.forEach((element, index) => {
        const commentItem = document.createElement('li');
        commentItem.textContent = `Комментарий к элементу ${index + 1}... Какой-то комментарий... Какой-то комментарий... Какой-то комментарий`;
        commentItem.classList.add('comment');

        commentItem.addEventListener('click', function() {
            const allComments = document.querySelectorAll('.comment');
            allComments.forEach(comment => {
                comment.classList.remove('active');
            });
            selectedElements.forEach(selectedElement => {
                selectedElement.classList.remove('active');
            });
            commentItem.classList.add('active');
            element.classList.add('active');
        });

        element.addEventListener('click', function() {
            const isActive = element.classList.contains('active');
            if (!isActive) {
                element.classList.add('active');
                commentItem.classList.add('active');
            } else {
                element.classList.remove('active');
                commentItem.classList.remove('active');
            }
        });

        let position = 0;

        if (index > 0) {
            const lastComment = commentList.lastChild;
            position = Math.max(element.getBoundingClientRect().top + window.pageYOffset, lastComment.getBoundingClientRect().top + lastComment.getBoundingClientRect().height + 24 + window.pageYOffset);
        } else {
            position = element.getBoundingClientRect().top + window.pageYOffset;
        }

        commentList.appendChild(commentItem);
        commentItem.style.marginTop = `calc(${position}px - 5vw)`;
    });

    inputs.forEach(input => {
        input.addEventListener('focus', function() {
            const allComments = document.querySelectorAll('.comment');
            allComments.forEach(comment => {
                comment.classList.remove('active');
            });
        });
    });
});
