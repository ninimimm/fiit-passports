async function createComment(leftBound, rightBound, fieldName) {
    return await fetch(`${security}://${ip}:${port}/api/create/comment`,
        {
            method: 'POST',
            headers: {
            'Content-Type' : 'application/json'  
            },
            body: JSON.stringify({ sessionId: getCookie("idSession"),
                fieldName: fieldName,
                start: `${leftBound}`,
                end: `${rightBound}`,
                text: ""
                })
        }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось создать комментарий');
        }
        return response.json();
    }).then(data => {
        return data;
    });
}

async function getComments() {
    return await fetch(`${security}://${ip}:${port}/api/get/comment`,
        {
            method: 'POST',
            headers: {
            'Content-Type' : 'application/json'  
            },
            body: JSON.stringify({ sessionId: getCookie("idSession")})
        }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось получить комментарии');
        }
        return response.json();
    }); 
}

async function updateComment(id, text) {
    fetch(`${security}://${ip}:${port}/api/update/comment`,
        {
            method: 'POST',
            headers: {
            'Content-Type' : 'application/json'  
            },
            body: JSON.stringify({ id: `${id}`,
                text: text
                })
        }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось обновить текст коммента');
        }
        return response;
    });
}

async function deleteComment(id) {
    return await fetch(`${security}://${ip}:${port}/api/delete/comment`,
        {
            method: 'POST',
            headers: {
            'Content-Type' : 'application/json'  
            },
            body: JSON.stringify({ id : `${id}`})
        }).then(response => {
        if (!response.ok) {
            throw new Error('Не получилось удалить комментарий');
        }
        return response;
    });
}