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
    document.querySelector('.name_customer_input').value = data.ordererName;
    document.querySelector('.name_project_input').value = data.projectName;
    document.querySelector('.description_project_input').value = data.projectDescription;
    document.querySelector('.name_input').value = data.name;
    document.querySelector('.goal_project_input').value = data.goal;
    document.querySelector('.result_project_input').value = data.result;
    document.querySelector('.criteria_project_input').value = data.acceptanceCriteria;
    document.querySelector('.max_input').value = data.copiesNumber;
    document.querySelector('.address_input').value = data.meetingLocation;
    document.querySelector('.last_name_input').value = data.surname;
    document.querySelector('.telegram_input').value = data.telegramTag;
    document.querySelector('.email_input').value = data.email;
    document.querySelector('.phone_input').value = data.phoneNumber;
});

function getCookie(name) {
    const cookie = document.cookie.match(new RegExp(name + '=(.+?)(;|$)'));
    if (cookie === null) {
        return undefined;
    }
    return cookie[1];
}

async function UpdatePassport() {
    await fetch('http://localhost:8888/api/passport/update',
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify({ SessionId: getCookie("idSession"), OrdererName: document.querySelector('.name_customer_input').value, 
                                ProjectName: document.querySelector('.name_project_input').value,   
                                ProjectDescription: document.querySelector('.description_project_input').value,
                                Goal: document.querySelector('.goal_project_input').value, 
                                Result: document.querySelector('.result_project_input').value,   
                                AcceptanceCriteria: document.querySelector('.criteria_project_input').value,
                                CopiesNumber: document.querySelector('.max_input').value, 
                                MeetingLocation: document.querySelector('.address_input').value, 
                                Name: document.querySelector('.name_input').value,
                                Surname: document.querySelector('.last_name_input').value,
                                TelegramTag: document.querySelector('.telegram_input').value,
                                Email: document.querySelector('.email_input').value,
                                PhoneNumber: document.querySelector('.phone_input').value,
        })
    });
}

async function ToContacts() {
    await UpdatePassport();
    window.location.href = 'contacts.html';
}

async function CheckAuthenticate() {
    await fetch('http://localhost:8888/api/passport/authenticate',
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify({ SessionId: getCookie("idSession"), OrdererName: document.querySelector('.name_customer_input').value, 
                                ProjectName: document.querySelector('.name_project_input').value,   
                                ProjectDescription: document.querySelector('.description_project_input').value,
                                Goal: document.querySelector('.goal_project_input').value, 
                                Result: document.querySelector('.result_project_input').value,   
                                AcceptanceCriteria: document.querySelector('.criteria_project_input').value,
                                CopiesNumber: document.querySelector('.max_input').value, 
                                MeetingLocation: document.querySelector('.address_input').value,
                                Name: document.querySelector('.name_input').value,
                                Surname: document.querySelector('.last_name_input').value,
                                TelegramTag: document.querySelector('.telegram_input').value,
                                Email: document.querySelector('.email_input').value,
                                PhoneNumber: document.querySelector('.phone_input').value,
        })
    }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось пройти процесс аутентификации');
        }
        return response.json();
    }).then(data => {
        console.log(data.state);
    });
}

async function SubmitPassport() {
    await fetch('http://localhost:8888/api/passport/confirm',
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify({ SessionId: getCookie("idSession"), OrdererName: document.querySelector('.name_customer_input').value, 
                                ProjectName: document.querySelector('.name_project_input').value,   
                                ProjectDescription: document.querySelector('.description_project_input').value,
                                Goal: document.querySelector('.goal_project_input').value, 
                                Result: document.querySelector('.result_project_input').value,   
                                AcceptanceCriteria: document.querySelector('.criteria_project_input').value,
                                CopiesNumber: document.querySelector('.max_input').value, 
                                MeetingLocation: document.querySelector('.address_input').value, 
                                Name: document.querySelector('.name_input').value,
                                Surname: document.querySelector('.last_name_input').value,
                                TelegramTag: document.querySelector('.telegram_input').value,
                                Email: document.querySelector('.email_input').value,
                                PhoneNumber: document.querySelector('.phone_input').value,
        })
    }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось пройти процесс аутентификации');
        }
        return response.json();
    }).then(data => {
        if (data.state === "Ok") {  
            window.location.href = 'request_send.html';
        }
        else {
            console.log(data.state);
        }
    });
}