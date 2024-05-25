fetch('http://51.250.123.70:8888/api/passport/get',
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
    document.querySelector('.name_customer_input').value = data.ordererName;
    document.querySelector('.name_project_input').value = data.projectName;
    document.querySelector('.description_project_input').value = data.projectDescription;
});

async function UpdatePassport() {
    await fetch('http://51.250.123.70:8888/api/passport/update',
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify({ SessionId: getCookie("idSession"), OrdererName: document.querySelector('.name_customer_input').value, 
                                ProjectName: document.querySelector('.name_project_input').value,   
                                ProjectDescription: document.querySelector('.description_project_input').value
        })
    });
}