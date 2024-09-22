


document.querySelectorAll('.chat-card').forEach(chatElement => {
    chatElement.addEventListener('click', function () {
        const chatId = chatElement.querySelector('input').value;

        const noChatMessage = document.querySelector('.no-current-chat');
        noChatMessage.style.display = 'none';

        fetch(`/Chat/LoadChat?chatId=${chatId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.text();
            })
            .then(html => {
                const existingChatArea = document.querySelector('.chat-area');
                const existingChatDetails = document.querySelector('.detail-area');

                if (existingChatDetails) {
                    existingChatArea.remove();
                    existingChatDetails.remove();
                }

                document.querySelector('.conversation-area').insertAdjacentHTML('afterend', html);

                setupColorListeners();
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    });
});

const toggleButton = document.querySelector('.dark-light');
const appDivElement = document.querySelector('.app');

function setupColorListeners() {
    const colors = document.querySelectorAll('.color');

    colors.forEach(color => {
        color.addEventListener('click', e => {
            colors.forEach(c => c.classList.remove('selected'));
            const theme = color.getAttribute('data-color');
            document.body.setAttribute('data-theme', theme);
            color.classList.add('selected');
        });
    });
}

toggleButton.addEventListener('click', () => {
    appDivElement.classList.toggle('dark-mode');
});

