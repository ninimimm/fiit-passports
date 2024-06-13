async function getNumbers() {
    return await fetch(`${security}://${ip}:${port}/api/get/numbers`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Не получилось получить номера')
        }
        return response.json();
    });
}

async function updateNumbers(projects) {
    return await fetch(`${security}://${ip}:${port}/api/update/numbers`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(projects)
    }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось сохранить номера')
        }
        return response;
    });
}