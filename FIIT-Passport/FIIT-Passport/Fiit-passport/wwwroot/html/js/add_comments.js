let lastSelection = '';
let isImageAdded = false;
const commentIconList = document.querySelector('.comment_icon_list');
const commentList = document.querySelector('.comment_list');

document.addEventListener('selectionchange', () => {
    const selection = window.getSelection();
    const selectedText = selection.toString();

    if (selectedText && selectedText !== lastSelection && commentIconList && !isImageAdded) {
        const commentIcon = document.createElement('li');
        commentIcon.classList.add('comment_icon');

        const img = document.createElement('img');
        img.src = 'comment-rect-plus-32-regular.png';
        img.alt = 'Добавить комментарий';

        commentIcon.appendChild(img);
        commentIconList.appendChild(commentIcon);

        lastSelection = selectedText;
        isImageAdded = true;

        const range = selection.getRangeAt(0);
        let position = range.getBoundingClientRect().top + window.pageYOffset;
        commentIcon.style.marginTop = `calc(${position}px - 5vw)`;

        commentIcon.addEventListener('click', (e) => {
            const commentInput = document.createElement('textarea');
            commentInput.placeholder = 'Введите комментарий';
            commentInput.classList.add('comment_input');

            const commentItem = document.createElement('li');
            commentItem.classList.add('comment');

            const commentButton = document.createElement('button');
            commentButton.type = 'submit';
            commentButton.textContent = 'Опубликовать';
            commentButton.classList.add('save_comment');

            commentItem.appendChild(commentInput);
            commentItem.appendChild(commentButton);
            commentList.appendChild(commentItem);

            const selection = window.getSelection();
            const selectedText = selection.toString();
            const selectedSpan = document.createElement('span');
            selectedSpan.classList.add('selected_text');
            selectedSpan.textContent = selectedText;

            const range = selection.getRangeAt(0);

            range.deleteContents();
            range.insertNode(selectedSpan);

            commentList.appendChild(commentItem);

            const selected = document.querySelectorAll('.selected_text');

            selected.forEach((element, index) => {
                let position = 0;

                if (index > 0) {
                    const lastComment = commentList.lastChild;
                    position = Math.max(element.getBoundingClientRect().top + window.pageYOffset, lastComment.getBoundingClientRect().top + lastComment.getBoundingClientRect().height + 24 + window.pageYOffset);
                } else {
                    position = element.getBoundingClientRect().top + window.pageYOffset;
                }

                commentItem.style.marginTop = `calc(${position}px - 5vw)`;
            })

            commentInput.focus();

            commentIconList.removeChild(commentIcon);
        });
    } else if (!selectedText && isImageAdded) {
        while (commentIconList.firstChild) {
            commentIconList.removeChild(commentIconList.firstChild);
        }
        isImageAdded = false;
    }
});

function autoResizeTextarea(textarea) {
    textarea.style.height = "auto";
    textarea.style.height = textarea.scrollHeight + "px";
}

document.body.addEventListener('input', function(event) {
    if (event.target.tagName === 'TEXTAREA') {
        autoResizeTextarea(event.target);
    }
});
