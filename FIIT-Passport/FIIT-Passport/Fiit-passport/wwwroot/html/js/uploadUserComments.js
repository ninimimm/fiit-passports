let selectedElements = document.querySelectorAll('.selected_text, .selected_block');
const commentList = document.querySelector('.comment_list');
const commentIconList = document.querySelector('.comment_icon_list');
let index = 0;
let weight = {};
let nameToNum = {}
let num = 0;
let fieldCommentBlock = null;
document.body.querySelectorAll('[name]').forEach(element => {
    let name = element.getAttribute('name');
    if(name.length > 0) {
        nameToNum[name] = 10 ** 6 * (++num);
    }
})
GetComments().then(allComments => {
    allComments.sort(function(a, b) {
        if (a.fieldName === b.fieldName) {
            return a.startIndex - b.startIndex;
        }
        return a.fieldName < b.fieldName;
    })

    let lastName = null;
    let lastEnd = 0;
    let textHtml = "";
    let paragraph = null;
    allComments.forEach(element => {
        if (lastName !== element.fieldName) {
            if (lastName !== null) {
                createAText(paragraph, textHtml.substring(lastEnd));
            }
            lastName = element.fieldName;
            console.log(document.title);
            if (document.title === 'Редактировать анкету') {
                fieldCommentBlock = document.createElement('div');
                fieldCommentBlock.classList.add('comment_block');
                commentList.appendChild(fieldCommentBlock);
                paragraph = document.createElement('div');
                let input = document.querySelector(`[name='${element.fieldName}']`);
                let position = input.getBoundingClientRect().top + window.pageYOffset;
                fieldCommentBlock.style.marginTop = `calc(${position}px - 5vw)`;
                fieldCommentBlock.id = element.fieldName;
                paragraph.classList.add(`${input.className}`);
                paragraph.style.display = 'none';
                input.insertAdjacentElement('afterend', paragraph);
                textHtml = input.value;
            } else {
                paragraph = document.querySelector(`[name='${element.fieldName}']`);
                if (paragraph instanceof HTMLInputElement) {
                    textHtml = paragraph.value;
                } else {
                    textHtml = paragraph.textContent;
                }
            }
            paragraph.innerHTML = '';
            lastEnd = 0;
        }
        weight[element.id] = element.endIndex + nameToNum[element.fieldName];
        createAText(paragraph, textHtml.substring(lastEnd, element.startIndex));
        let span = CreateSpan(element.id, textHtml.substring(element.startIndex, element.endIndex));
        paragraph.appendChild(span);
        lastEnd = element.endIndex;
        CreateUserComment(span, element.textComment, element.id);
    });
    if (lastName !== null) {
        createAText(paragraph, textHtml.substring(lastEnd));
        SortComments();
    }
});

function createAText(paragraph, text){
    if (text.length == 0){
        return;
    }
    let div = document.createElement('a');
    div.textContent = text;
    paragraph.appendChild(div);
}

function CreateUserComment(span, commentText, id) {
    const commentItem = document.createElement('div');
    commentItem.id = id;
    commentItem.textContent = commentText;
    commentItem.classList.add('comment');
    commentItem.addEventListener('click', function() {
        const isActive = commentItem.classList.contains('active');
        const allComments = document.querySelectorAll('.comment');
        const selectedElements = document.querySelectorAll('.selected_text');
        if (!isActive) {
            if (document.title === "Редактировать анкету"){
                span.parentNode.style.display = 'inline';
                commentItem.parentNode.classList.add('active');
                span.parentNode.previousElementSibling.style.display = 'none';
                commentItem.parentNode.childNodes.forEach(element => {
                    element.classList.remove('active');
                })
                selectedElements.forEach(selectedElement => {
                    if (selectedElement.parentNode.previousElementSibling.getAttribute('name') === commentItem.parentNode.id) {
                        selectedElement.classList.remove('active');
                    }
                });
            } else {
                selectedElements.forEach(selectedElement => {
                    selectedElement.classList.remove('active');
                });
                allComments.forEach(comment => {
                    comment.classList.remove('active');
                });
            }
            span.classList.add('active');
            commentItem.classList.add('active');
        } else {
            if (document.title === "Редактировать анкету"){
                span.parentNode.style.display = 'none';
                span.parentNode.previousElementSibling.style.display = 'inline';
                commentItem.parentNode.classList.remove('active');
            }
            span.classList.remove('active');
            commentItem.classList.remove('active');
        }
    });
    span.addEventListener('click', function() {
        const isActive = commentItem.classList.contains('active');
        const allComments = document.querySelectorAll('.comment');
        const selectedElements = document.querySelectorAll('.selected_text');
        if (!isActive) {
            selectedElements.forEach(selectedElement => {
                selectedElement.classList.remove('active');
            });
            allComments.forEach(comment => {
                comment.classList.remove('active');
            });
            span.classList.add('active');
            commentItem.classList.add('active');
        } else {
            span.classList.remove('active');
            commentItem.classList.remove('active');
        }
    });
    commentItem.addEventListener('blur', function(event) {
        UpdateComment(commentItem.id, commentItem.value);
    });
    let position = span.getBoundingClientRect().top + window.pageYOffset;
    if (document.title === "Редактировать анкету") {
        fieldCommentBlock.appendChild(commentItem);
    } else {
        commentList.appendChild(commentItem);
        commentItem.style.marginTop = `calc(${position}px - 5vw)`;
    }
}

function CreateSpan(id, selectedText) {
    let span = document.createElement('span');
    span.textContent = selectedText;
    span.id = id;
    span.classList.add('selected_text');
    return span;
}

function SortComments() {
    let comments = null;
    if (document.title === "Редактировать анкету") {
        comments = Array.prototype.slice.call(document.querySelectorAll('.comment_block'));
    } else {
        comments = Array.prototype.slice.call(document.querySelectorAll('.comment'));
    }
    if (comments.length > 1) {
        comments.sort(function(a, b) {
            if (document.title === "Редактировать анкету") {
                return nameToNum[a.id] - nameToNum[b.id];
            } else {
                return weight[a.id] - weight[b.id];
            }
        });
        for (let i = 1; i < comments.length; i++) {
            if (comments[i].getBoundingClientRect().top < comments[i - 1].getBoundingClientRect().bottom) {
                find = true;
                comments[i].style.marginTop = `calc(${comments[i - 1].getBoundingClientRect().top + comments[i - 1].getBoundingClientRect().height + 24 + window.pageYOffset}px - 5vw)`;
            }
        }
    }
}