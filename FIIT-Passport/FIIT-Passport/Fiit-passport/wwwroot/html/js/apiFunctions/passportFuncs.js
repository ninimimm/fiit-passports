async function createPassport() {
    return await fetch(`http://${ip}:${port}/api/create/passport`,
        {
            method: 'POST'
        }).then(response => {
            if (!response.ok) {
                throw new Error('Не получилось создать паспорт');
            }
            return response.json();
    });
}

async function getPassport() {
    return await fetch(`http://${ip}:${port}/api/get/passport`,
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
    });
}

async function getPassports() {
    return await fetch(`http://${ip}:${port}/api/get/passports`,
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify({ sessionId: getCookie("idSession") })
    }).then(response => {
    if (!response.ok) {
        throw new Error('Не получилось получить паспорта');
    }
    return response.json();
    });
}

async function updatePassport(data) {
    return await fetch(`http://${ip}:${port}/api/update/passport`,
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify(data)
    }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось сохранить паспорт');
        }
        return response;
    });
}

async function authenticate(data) {
    return await fetch(`http://${ip}:${port}/api/authenticate`,
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify(data)
    }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось пройти аутентификацию');
        }
        return response;
    });
}

async function confirm(data) {
    return await fetch(`http://${ip}:${port}/api/confirm/passport`,
    {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json'  
        },
        body: JSON.stringify(data)
    }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось принять аутентификацию');
        }
        return response;
    });;
}