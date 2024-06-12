async function create() {
    await createPassport().then(data => {
        let currentDate = new Date();
        currentDate.setFullYear(currentDate.getFullYear() + 10);
        const date = currentDate.toUTCString();
        document.cookie = `idSession=${data.SessionId}; expires=${date}`;
        window.location.href = 'createPassportHTML/about_project.html';
    });
    localStorage.removeItem('about_project');
    localStorage.removeItem('check_request');
    localStorage.removeItem('contacts');
    localStorage.removeItem('details_project');
    localStorage.removeItem('others_project');
}

window.onload = async function() {
    if (getCookie("idSession") !== undefined) {
        await getPassport().then(data => {
            if (data.status === 0) {
                let lastForm = document.querySelector('.fill_form');
                var buttonContainer = document.createElement("div");
                buttonContainer.innerHTML = '<form action="createPassportHTML/about_project.html"><button type="submit" class="fill_form" style="margin-left: 5vw; margin-right: -1vw">Изменить анкету</button></form>';
                lastForm.parentNode.insertBefore(buttonContainer.firstChild, lastForm.nextSibling);
            };
        });
    }
};
