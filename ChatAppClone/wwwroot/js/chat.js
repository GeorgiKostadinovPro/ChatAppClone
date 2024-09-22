const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();
connection.on("ReceiveMessage", function (message) {
    appendMessageToChat(message);
});

connection.start().then(function () {
    console.log("SignalR connection established");
}).catch(function (err) {
    console.error("SignalR connection error:", err.toString());
});

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

                const sendMessageButton = document.getElementById('send-message-button');
                const messageInput = document.getElementById('write-message-input');

                if (sendMessageButton && messageInput) {
                    sendMessageButton.replaceWith(sendMessageButton.cloneNode(true));
                    const newSendButton = document.getElementById('send-message-button');

                    newSendButton.addEventListener('click', () => sendMessage(messageInput));
                }

                setupColorListeners();

                connection.invoke("JoinChatGroup", chatId).then(function () {
                    console.log("Joined chat group: " + chatId);
                }).catch(function (err) {
                    console.error("Error joining chat group:", err.toString());
                });
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    });
});

function sendMessage(messageInput) {
    const messageContent = messageInput.value.trim();
    if (messageContent) {
        const chatId = document.getElementById("chatId").value;
        fetch(`/api/Message/CreateMessage`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ chatId: chatId, message: messageContent })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }

                messageInput.value = '';
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    }
}

function appendMessageToChat(message) {
    const chatArea = document.querySelector('.chat-area-main');
    const currUserId = document.getElementById("currUserId").value;
    const isOwner = message.creatorId == currUserId;
    const messageClass = isOwner ? "chat-msg owner" : "chat-msg";

    const messageDiv = `
        <div class="${messageClass}">
            <div class="chat-msg-profile">
                <img class="chat-msg-img" src="${message.creatorProfilePictureUrl}" alt="Profile Image">
                <div class="chat-msg-date">Message sent ${message.createdOn} ago</div>
            </div>
            <div class="chat-msg-content">
                <div class="chat-msg-text">${message.content}</div>
            </div>
        </div>
    `;

    chatArea.innerHTML += messageDiv;
}

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
