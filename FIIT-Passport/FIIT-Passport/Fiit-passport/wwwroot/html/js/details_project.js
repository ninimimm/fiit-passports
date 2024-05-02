fetch('http://158.160.88.146:8888/api/passport/get',
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
    document.querySelector('.goal_project_input').value = data.goal;
    document.querySelector('.result_project_input').value = data.result;
    document.querySelector('.criteria_project_input').value = data.acceptanceCriteria;
});

function getCookie(name) {
    const cookie = document.cookie.match(new RegExp(name + '=(.+?)(;|$)'));
    if (cookie === null) {
        return undefined;
    }
    return cookie[1];
}

async function UpdatePassport() {
    await fetch('http://158.160.88.146:8888/api/passport/update',
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify({ SessionId: getCookie("idSession"), Goal: document.querySelector('.goal_project_input').value, 
                                Result: document.querySelector('.result_project_input').value,   
                                AcceptanceCriteria: document.querySelector('.criteria_project_input').value
        })
    });
}   
