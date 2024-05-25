async function CreateComment(leftBound, rightBound, fieldName) {
    return await fetch('http://localhost:8888/api/comment/create',
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