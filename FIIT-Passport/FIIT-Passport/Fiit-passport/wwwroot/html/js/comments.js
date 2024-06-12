getPassport().then(data => {
    document.querySelector('.name_customer_value_cr').textContent = data.ordererName;
    document.querySelector('.name_project_value_cr').textContent = data.projectName;
    document.querySelector('.description_project_value_cr').textContent = data.projectDescription;
    document.querySelector('.name_value_cr').textContent = data.name;
    document.querySelector('.goal_project_value_cr').textContent = data.goal;
    document.querySelector('.result_project_value_cr').textContent = data.result;
    document.querySelector('.criteria_project_value_cr').textContent = data.acceptanceCriteria;
    document.querySelector('.max_value_cr').textContent = data.copiesNumber;
    document.querySelector('.address_value_cr').textContent = data.meetingLocation;
    document.querySelector('.last_name_value_cr').textContent = data.surname;
    document.querySelector('.telegram_value_cr').textContent = data.telegramTag;
    document.querySelector('.email_value_cr').textContent = data.email;
    document.querySelector('.phone_value_cr').textContent = data.phoneNumber;
})

document.addEventListener('mouseup', function(event) {
    let selection = window.getSelection();
    if (selection.isCollapsed){
        return;
    }
    event.preventDefault();
    let range = selection.getRangeAt(0);
    if (range.startContainer === range.endContainer &&
        (range.endContainer.parentNode.getAttribute('name') || range.endContainer.parentNode.parentNode.getAttribute('name'))) {
        const commentIcon = document.createElement('li');
        commentIcon.classList.add('comment_icon');

        const img = document.createElement('img');
        img.src = 'img/comment-rect-plus-32-regular.png';
        img.alt = 'Добавить комментарий';

        commentIcon.appendChild(img);
        commentIconList.appendChild(commentIcon);
        let position = range.getBoundingClientRect().top + window.pageYOffset;
        commentIcon.style.marginTop = `calc(${position}px - 5vw)`;
        commentIcon.addEventListener("click", async function(event) {
            let id = "";
            let leftBound = range.startOffset;
            let rightBound = range.endOffset;
            let prev = range.endContainer.parentElement.previousElementSibling;
            let name = range.endContainer.parentNode.getAttribute('name');
            let passportName;
            if (name){
                passportName = name;
            } else {
                passportName = range.endContainer.parentNode.parentNode.getAttribute('name');
            }
            if (prev && prev.tagName.toLowerCase() === "span") {
                let offset = weight[prev.id] - nameToNum[passportName];
                leftBound += offset;
                rightBound += offset;
            }
            id = await createComment(leftBound, rightBound, passportName);
            weight[id] = rightBound + nameToNum[passportName];
            let selectedText = range.toString();
            let span = CreateSpan(id, selectedText);
            range.deleteContents();
            AText(range, span);
            if (commentList.querySelector(`#${passportName}`)){
                fieldCommentBlock = commentList.querySelector(`#${passportName}`);
            }
            else {
                fieldCommentBlock = document.createElement('div');
                fieldCommentBlock.classList.add('comment_block');
                commentList.appendChild(fieldCommentBlock);
                paragraph = document.querySelector(`[name='${passportName}']`);
                let position = paragraph.getBoundingClientRect().top + window.pageYOffset;
                fieldCommentBlock.style.marginTop = `calc(${position}px - 5vw)`;
                fieldCommentBlock.id = passportName;
            }
            CreateAdminComment(span, '', id);
            SortComments();
            selection.removeAllRanges();
        });
    }
});

document.addEventListener('mousedown', function(event) {
    var button = document.querySelector('.comment_icon');
    if (!button) {
        return;
    }
    if (event.target instanceof HTMLLIElement || event.target instanceof HTMLImageElement){
        button.click();
        window.getSelection().removeAllRanges();
    }
    button.parentNode.removeChild(button);
})

function AText(range, span){ 
    if (range.startContainer.parentNode.tagName.toLowerCase() === 'a') {
        let aNode = range.startContainer.parentNode;
        let parrent = aNode.parentNode;
        let beforeText = aNode.textContent.substring(0, range.startOffset);
        let afterText = aNode.textContent.substring(range.endOffset);

        let beforeA = document.createElement('a');
        beforeA.textContent = beforeText;

        let afterA = document.createElement('a');
        afterA.textContent = afterText;

        parrent.insertBefore(beforeA, aNode);
        parrent.insertBefore(span, aNode);
        parrent.insertBefore(afterA, aNode);

        aNode.remove();
    } else {
        let textBeforeRange = range.startContainer.textContent.substring(0, range.startOffset);
        let textAfterRange = range.endContainer.textContent.substring(range.endOffset);

        range.startContainer.textContent = '';
    
        range.deleteContents();
        range.insertNode(span);
        

        if (textBeforeRange.trim().length > 0) {
            let beforeA = document.createElement('a');
            beforeA.textContent = textBeforeRange;
            range.startContainer.parentNode.insertBefore(beforeA, span);
        }

        if (textAfterRange.trim().length > 0) {
            let afterA = document.createElement('a');
            afterA.textContent = textAfterRange;
            range.startContainer.parentNode.insertBefore(afterA, span.nextSibling);
        }
    }
}