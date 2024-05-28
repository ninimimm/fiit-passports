let selectedElements = document.querySelectorAll('.selected_text, .selected_block');
const commentList = document.querySelector('.comment_list');
const commentIconList = document.querySelector('.comment_icon_list');
let index = 0;
let weight = {};
let nameToNum = {};
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

            fieldCommentBlock = document.createElement('div');
            fieldCommentBlock.classList.add('comment_block');
            commentList.appendChild(fieldCommentBlock);
            paragraph = document.querySelector(`[name='${element.fieldName}']`);
            fieldCommentBlock.id = element.fieldName;
            textHtml = paragraph.textContent;
            paragraph.innerHTML = '';
            lastEnd = 0;
        }
        weight[element.id] = element.endIndex + nameToNum[element.fieldName];
        createAText(paragraph, textHtml.substring(lastEnd, element.startIndex));
        let span = CreateSpan(element.id, textHtml.substring(element.startIndex, element.endIndex));
        paragraph.appendChild(span);
        lastEnd = element.endIndex;
        CreateAdminComment(span, element.textComment, element.id);
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

function CreateAdminComment(span, commentText, id){
    const commentItem = document.createElement('li');
    commentItem.classList.add('comment');

    const commentInput = document.createElement('textarea');
    commentInput.placeholder = 'Введите комментарий';
    commentInput.value = commentText;
    commentInput.classList.add('comment_input');

    commentInput.addEventListener('input', function() {
        commentInput.style.height = "auto";
        commentInput.style.height = (commentInput.scrollHeight) + "px";
        SortComments();
    })


    const img = document.createElement('img');
    img.src = 'img/minus-circle-svgrepo-com.svg';
    img.alt = 'Удалить комментарий';
    img.classList.add('save_comment');

    commentItem.appendChild(commentInput);
    commentItem.appendChild(img);

    commentInput.focus();

    commentItem.id = id;
    commentItem.addEventListener('click', function() {
        const isActive = commentItem.classList.contains('active');
        const allComments = document.querySelectorAll('.comment');
        const selectedElements = document.querySelectorAll('.selected_text');
        if (!isActive) {
            commentItem.parentNode.classList.add('active');
            commentItem.parentNode.childNodes.forEach(element => {
                element.classList.remove('active');
            })
            selectedElements.forEach(selectedElement => {
                if (selectedElement.parentNode.getAttribute('name') === commentItem.parentNode.id) {
                    selectedElement.classList.remove('active');
                }
            });
            span.classList.add('active');
            commentItem.classList.add('active');
        } else {
            commentItem.parentNode.classList.remove('active');
            span.classList.remove('active');
            commentItem.classList.remove('active');
        }
    });
    span.addEventListener('click', function() {
        const isActive = commentItem.classList.contains('active');
        const allComments = document.querySelectorAll('.comment');
        const selectedElements = document.querySelectorAll('.selected_text');
        if (!isActive) {
            commentItem.parentNode.classList.add('active');
            commentItem.parentNode.childNodes.forEach(element => {
                element.classList.remove('active');
            })
            selectedElements.forEach(selectedElement => {
                if (selectedElement.parentNode.getAttribute('name') === commentItem.parentNode.id) {
                    selectedElement.classList.remove('active');
                }
            });
            span.classList.add('active');
            commentItem.classList.add('active');
        } else {
            commentItem.parentNode.classList.remove('active');
            span.classList.remove('active');
            commentItem.classList.remove('active');
        }
    });
    
    commentInput.addEventListener('blur', function(event) {
        UpdateComment(commentItem.id, commentInput.value);
    });
    let bigger = null;
    console.log(weight);
    fieldCommentBlock.childNodes.forEach(element => {
        if (weight[element.id] > weight[commentItem.id] && bigger == null){
            bigger = element;
        }
    });
    if (bigger){
        fieldCommentBlock.insertBefore(commentItem, bigger);
    } else {
        fieldCommentBlock.appendChild(commentItem);  
    }

    img.addEventListener('click', async function(event) {
        event.stopPropagation();
        DeleteComment(id);
        delete weight[id];
        commentItem.parentNode.removeChild(commentItem);
        let allText = '';
        if (span.previousSibling != null && span.previousSibling.tagName != undefined &&
            span.previousSibling.tagName.toLowerCase() === 'a'){
            allText += span.previousSibling.textContent;
            span.previousSibling.remove();
        }
        allText += span.textContent;
        if (span.nextSibling != null && span.nextSibling.tagName != undefined &&
            span.nextSibling.tagName.toLowerCase() === 'a'){
            allText += span.nextSibling.textContent;
            span.nextSibling.remove();
        }
        let newATag = document.createElement('a');
        newATag.textContent = allText;
        span.parentNode.replaceChild(newATag, span);
        SortComments();
    })
    
    commentInput.focus();
    commentInput.style.height = (commentInput.scrollHeight) + "px";
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
    comments = Array.prototype.slice.call(document.querySelectorAll('.comment_block'));
    comments.sort(function(a, b) {
        return nameToNum[a.id] - nameToNum[b.id];
    });
    comments[0].style.marginTop = `calc(${document.querySelector(`[name=${comments[0].id}]`).getBoundingClientRect().top + window.pageYOffset}px - 5vw)`;
    for (let i = 1; i < comments.length; i++) {
        let position = document.querySelector(`[name=${comments[i].id}]`).getBoundingClientRect().top + window.pageYOffset;
        comments[i].style.marginTop = `calc(${position}px - 5vw)`
        if (comments[i].getBoundingClientRect().top < comments[i - 1].getBoundingClientRect().bottom) {
            comments[i].style.marginTop = `calc(${comments[i - 1].getBoundingClientRect().top + comments[i - 1].getBoundingClientRect().height + 24 + window.pageYOffset}px - 4vw)`;
        }
    }
}