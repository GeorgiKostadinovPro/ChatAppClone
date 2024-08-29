const toggleButton = document.querySelector('.dark-light');
const colors = document.querySelectorAll('.color');
const appDivElement = document.querySelector('.app');
const headerElement = document.querySelector('.site-header');
const footerElement = document.querySelector('.site-footer');

colors.forEach(color => {
    color.addEventListener('click', e => {
        colors.forEach(c => c.classList.remove('selected'));
        const theme = color.getAttribute('data-color');
        document.body.setAttribute('data-theme', theme);
        color.classList.add('selected');
    });
});

toggleButton.addEventListener('click', () => {
    appDivElement.classList.toggle('dark-mode');
    headerElement.classList.toggle('dark-mode');
    footerElement.classList.toggle('dark-mode');
});