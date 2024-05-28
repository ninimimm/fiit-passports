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
    let div = document.querySelector('.name_request');
    div.textContent = data.projectName;
    let pageName = '';
    let status = data.status;
    if (status === 2){
        pageName = 'edit_request.html';
    }
    else if (status === 1){
        pageName  = "request_check.html";
    }
    else {
        pageName = 'request_send.html';
    }
    div.addEventListener('click', function() {
        window.location.href = pageName;
    })
});
