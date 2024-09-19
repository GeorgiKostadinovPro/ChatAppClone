const inputElement = document.querySelector('.explore-users .search-content input');
const cancelBtn = document.querySelector('.explore-users .search-content .cancel-btn');

cancelBtn.addEventListener('click', (e) => {
    inputElement.value = '';
});