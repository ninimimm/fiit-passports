fetch('http://localhost:8888/api/passport/get',
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify({ sessionId: getCookie("idSession") })
    }).then(response => {
    if (!response.ok) {
        throw new Error('Не получилось получить паспорт');
    }
    return response.json();
}).then(data => {
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
});

uploadComments('input');

document.addEventListener('mouseup', function(event) {
    let selection = window.getSelection();
    if (selection.isCollapsed){
        return;
    }
    event.preventDefault();
    let range = selection.getRangeAt(0);
    if (range.startContainer === range.endContainer) {
        let button = document.createElement("button");
        event.target.parentNode.appendChild(button);
        button.classList.add('accept_request');
        button.textContent = "Создать комментарий";
        button.style.position = 'absolute';
        let rect = range.getBoundingClientRect();
        button.style.top = (rect.bottom + window.scrollY + 10) + 'px';
        button.style.left = (((rect.left + rect.right) / 2) + window.scrollX - button.offsetWidth / 2) + 'px';
        button.addEventListener("mousedown", async function(event) {
            let id = "";
            let leftBound = range.startOffset;
            let rightBound = range.endOffset;
            let prev = range.endContainer.previousSibling;
            let passportName = range.endContainer.parentNode.getAttribute('name');
            if (prev) {
                let offset = weight[prev.id] - nameToNum[passportName];
                leftBound += offset;
                rightBound += offset;
            }
            id = await CreateComment(leftBound, rightBound, passportName);
            console.log('id залупы мочи');
            console.log(id);
            weight[id] = rightBound + nameToNum[passportName];
            let selectedText = range.toString();
            let span = CreateSpan(id, selectedText);
            range.deleteContents();
            range.insertNode(span);
            CreatePageComment(span, '', id, 'input');
            SortComments();
            selection.removeAllRanges();
            await event.target.parentNode.removeChild(button);
            index++;
        });
    }
});

document.addEventListener('mousedown', function(event) {
    if (event.target instanceof HTMLButtonElement) {
        return;
    }
    var button = document.querySelector('.accept_request');
    if (!button) {
        return;
    }
    button.parentNode.removeChild(button);
})