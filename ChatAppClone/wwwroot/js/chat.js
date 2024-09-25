document.addEventListener('DOMContentLoaded', () => {

    const chatConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    chatConnection.on("StartChat", function (chat) {
        appendToConversationArea(chat);
    });

    chatConnection.on("ReceiveMessage", function (message) {
        appendMessageToChat(message);
    });

    chatConnection.start().then(function () {
        console.log("SignalR connection established");
    }).catch(function (err) {
        console.error("SignalR connection error:", err.toString());
    });


    document.querySelectorAll('.chat-card').forEach((chatElement) => {
        chatElement.addEventListener('click', () => loadChat(chatElement));
    });

    function loadChat(chatElement) {
        const chatId = chatElement.querySelector('input').value;

        const noChatMessage = document.querySelector('.no-current-chat');
        noChatMessage.style.display = 'none';

        fetch(`/Chat/LoadChat?chatId=${chatId}`)
            .then((res) => {
                if (!res.ok) {
                    throw new Error('Network response was not ok');
                }
                return res.text();
            })
            .then((html) => {
                const existingChatArea = document.querySelector('.chat-area');
                const existingChatDetails = document.querySelector('.detail-area');

                if (existingChatDetails) {
                    existingChatArea.remove();
                    existingChatDetails.remove();
                }

                document.querySelector('.conversation-area').insertAdjacentHTML('afterend', html);

                const messageInput = document.getElementById('write-message-input');
                const emojiArea = $(messageInput).emojioneArea();
                const emojiAreaInstance = emojiArea.data("emojioneArea");

                const sendMessageButton = document.getElementById('send-message-button');

                sendMessageButton.addEventListener('click', () => {
                    const messageContent = emojiAreaInstance.getText();
                    sendMessage(messageContent);
                    emojiAreaInstance.setText('');
                });

                emojiAreaInstance.on('keydown', function (editor, event) {
                    if (event.key === 'Enter') {
                        event.preventDefault();
                        const messageContent = emojiAreaInstance.getText();
                        sendMessage(messageContent);
                        emojiAreaInstance.setText('');
                    }
                });

                setupColorListeners();

                chatConnection.invoke("JoinChatGroup", chatId).then(function () {
                    console.log("Joined chat group: " + chatId);
                }).catch(function (err) {
                    console.error("Error joining chat group:", err.toString());
                });
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    }

    function sendMessage(messageContent) {
        if (messageContent) {
            const chatId = document.getElementById("chatId").value;
            fetch(`/api/Message/CreateMessage`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ chatId: chatId, message: messageContent })
            })
                .then((res) => {
                    if (!res.ok) {
                        throw new Error('Network response was not ok');
                    }
                })
                .catch(error => {
                    console.error('There was a problem with the fetch operation:', error);
                });
        }
    }

    function appendToConversationArea(chat) {
        const conversationArea = document.querySelector('.conversation-area');
        const noChatCard = document.querySelector('.no-chats-card');

        const chatCard = document.createElement('div');
        chatCard.classList.add('msg', 'online', 'chat-card');
        chatCard.addEventListener('click', () => loadChat(chatCard));

        chatCard.innerHTML = `
            <input type="hidden" value="${chat.id}" />
                <img class="msg-profile" src=${chat.imageUrl} alt="" />
                <div class="msg-detail">
                    <div class="msg-username">${chat.name}</div>
                    <div class="msg-content">
                        <span class="msg-message msg-last-message">${chat.lastMessage}</span>
                        <span class="msg-date msg-last-active">${chat.lastActive}</span>
                    </div>
                </div>
        `;

        if (noChatCard) {
            noChatCard.style.display = 'none';
        }
        
        conversationArea.insertBefore(chatCard, conversationArea.firstChild);
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
                    <div class="chat-msg-date">sent ${message.createdOn}</div>
                </div>
                <div class="chat-msg-content">
                    <div class="chat-msg-text">${message.content}</div>
                </div>
            </div>
        `;

        chatArea.innerHTML += messageDiv;

        document.querySelector('.msg-last-message').textContent
            = message.content.length < 30 ? message.content : message.content.slice(0, 30) + "...";

        document.querySelector('.msg-last-active').textContent = message.createdOn;
    }

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

    const toggleButton = document.querySelector('.dark-light');
    const appDivElement = document.querySelector('.app');

    toggleButton.addEventListener('click', () => {
        appDivElement.classList.toggle('dark-mode');
    });

});

