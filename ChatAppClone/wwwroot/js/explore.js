const inputElement = document.querySelector('.explore-users .search-content input');
const cancelBtn = document.querySelector('.explore-users .search-content .cancel-btn');

cancelBtn.addEventListener('click', (e) => {
    inputElement.value = '';
});

document.querySelectorAll('.user-card a').forEach(followLink => {
    followLink.addEventListener('click', function () {
        const userId = this.closest('.user-card').querySelector('input[type="hidden"]').value;
        const action = this.textContent.trim() === 'Follow' ? 'Follow' : 'Unfollow';

        fetch(`/api/UserFollows/${action}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userId)
        })
        .then(response => response.json())
        .then(data => {
                location.reload();
        })
        .catch(error => console.error('Error:', error));
    });
});
