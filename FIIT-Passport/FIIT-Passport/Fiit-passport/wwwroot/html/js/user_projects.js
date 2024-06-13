window.onload = function() {
    if (localStorage.getItem('auth') === null || localStorage.getItem('auth') === undefined) {
        return;
    }
    getPassports(localStorage.getItem('auth')).then(data => {
        const block = document.querySelector('.all_requests');
        let index = 1;
        data.forEach(passport => {
            savePassport(passport);
            const requestDiv = document.createElement('div');
            requestDiv.className = 'request';
            const requestText = document.createTextNode(`Паспорт №${index}`);
            requestDiv.appendChild(requestText);
            const nameRequestDiv = document.createElement('div');
            nameRequestDiv.className = 'name_request';
            const nameRequestText = document.createTextNode(passport.projectName);
            nameRequestDiv.appendChild(nameRequestText);
            requestDiv.appendChild(nameRequestDiv);
            block.appendChild(requestDiv);

            let pageName = '';
            let status = passport.status;
            if (status === 3){
                pageName = 'userPagesHTML/edit_request.html';
            }
            else if (status === 2){
                pageName  = "userPagesHTML/request_check.html";
            }
            else {
                pageName = 'userPagesHTML/passport_send.html';
            }
            requestDiv.addEventListener('click', function() {
                let currentDate = new Date();
                currentDate.setFullYear(currentDate.getFullYear() + 10);
                const date = currentDate.toUTCString();
                document.cookie = `idSession=${passport.sessionId}; expires=${date}`;
                window.location.href = pageName;
            })
            index++;
        });
    });
};

function savePassport(passport) {
    localStorage.setItem(passport.sessionId, JSON.stringify(passport));
}