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
    var firstDiv = document.querySelector('div');
    var firstSection = document.querySelector('section');
    var pageName = document.querySelector('h1');
    switch(data.status) {
        case 2:
            firstDiv.classList.add('request_check_block');
            firstDiv.classList.remove('request_send_block');
            firstSection.classList.add('request_check');
            firstSection.classList.remove('request_send');
            pageName.textContent = 'Анкета проверяется';
            break;
        case 3:
            firstDiv.classList.add('request_checked_block');
            firstDiv.classList.remove('request_send_block');
            firstSection.classList.add('request_checked');
            firstSection.classList.remove('request_send');
            pageName.textContent = 'Анкета проверена';
            break;
    }
    document.querySelector('.name_customer_value').textContent = data.ordererName;
    document.querySelector('.name_project_value').textContent = data.projectName;
    document.querySelector('.description_project_value').textContent = data.projectDescription;
    document.querySelector('.name_value').textContent = data.name;
    document.querySelector('.goal_project_value').textContent = data.goal;
    document.querySelector('.result_project_value').textContent = data.result;
    document.querySelector('.criteria_project_value').textContent = data.acceptanceCriteria;
    document.querySelector('.max_value').textContent = data.copiesNumber;
    document.querySelector('.address_value').textContent = data.meetingLocation;
    document.querySelector('.last_name_value').textContent = data.surname;
    document.querySelector('.telegram_value').textContent = data.telegramTag;
    document.querySelector('.email_value').textContent = data.email;
    document.querySelector('.phone_value').textContent = data.phoneNumber;
});

function getCookie(name) {
    const cookie = document.cookie.match(new RegExp(name + '=(.+?)(;|$)'));
    if (cookie === null) {
        return undefined;
    }
    return cookie[1];
}
