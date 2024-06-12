async function Redirect(page) {
    const form = document.forms[0];
    if (!form.checkValidity()){
        form.reportValidity();
        return false;
    }
    saveFields();
    await Update();
    window.location.href = page;
    return true;
}

async function RedirectNotValidation(page) {
    saveFields();
    window.location.href = page;
}
