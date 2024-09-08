const cancelBtnElement = document.querySelector('.cancel-btn');

cancelBtnElement.addEventListener('click', (e) => {
    e.preventDefault();

    const inputElements = document.querySelectorAll('#change-password-form input');
    const spanElements = document.querySelectorAll('#change-password-form span');

    inputElements.forEach((input) => {
        input.value = '';
    });

    spanElements.forEach((span) => {
        span.style.display = 'none';
    });
});
