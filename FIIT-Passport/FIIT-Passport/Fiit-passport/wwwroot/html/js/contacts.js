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
    document.querySelector('.name_input').value = data.name;
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
        body: JSON.stringify({ SessionId: getCookie("idSession"), Name: document.querySelector('.name_input').value, 
                                Surname: document.querySelector('.last_name_input').value,
                                TelegramTag: document.querySelector('.telegram_input').value,
                                Email: document.querySelector('.email_input').value,
                                PhoneNumber: document.querySelector('.phone_input').value,
        })
    });
}