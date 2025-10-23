document.addEventListener('DOMContentLoaded', () => {

    ///
    ///   SIGNALR HUB BUILDFER & INITIALIZATION
    /// 

    const chatConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    chatConnection.on("StartChat", function (chat) {
        appendToConversationArea(chat);

        chatConnection.invoke("JoinChatAsync", chat.id).then(function () {
            console.log("Joined chat group: " + chat.id);
        }).catch(function (err) {
            console.error("Error joining chat group:", err.toString());
        });
    });

    chatConnection.on("DeleteChat", function (chatId) {
        chatConnection.invoke("LeaveChatAsync", chatId).then(function () {
            console.log("Leave chat group: " + chatId);

            // Remove the chat card from both users
            removeChatCard(chatId);
            showNoChatCard();

            // If user is in the deleted chat, remove it
            const currChat = document.querySelector(".chat-area #chatId");

            if (currChat && currChat.value == chatId) {
                document.querySelector('.chat-area').style.display = 'none';
                document.querySelector('.detail-area').style.display = 'none';
                document.querySelector('.no-current-chat').style.display = 'block';
            }
        }).catch(function (err) {
            console.error("Error leaving chat group:", err.toString());
        });
    });

    chatConnection.on("ReceiveMessage", function (message) {
        appendMessageToChat(message);
    });

    chatConnection.start().then(function () {
        console.log("SignalR connection established");

        document.querySelectorAll('.conversation-area .chat-card').forEach((chatElement) => {
            const chatId = chatElement.querySelector('#chatCardId').value;

            chatConnection.invoke("JoinChatAsync", chatId).then(function () {
                console.log("Joined chat group: " + chatId);
            }).catch(function (err) {
                console.error("Error joining chat group:", err.toString());
            });
        });
    }).catch(function (err) {
        console.error("SignalR connection error:", err.toString());
    });

    ///
    ///   CHAT API HANDLERS
    /// 

    document.querySelectorAll('.chat-card').forEach((chatElement) => {
        chatElement.addEventListener('click', () => loadChat(chatElement));
    });

    function loadChat(chatElement) {
        const chatId = chatElement.querySelector('#chatCardId').value;

        const noChatMessage = document.querySelector('.no-current-chat');
        noChatMessage.style.display = 'none';

        fetch(`/Chat/Load?chatId=${chatId}`)
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

                const deleteButton = document.querySelector('.delete-btn');

                if (deleteButton) {
                    deleteButton.addEventListener('click', (e) => deleteChat(e));
                }

                setupColorListeners();
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    }

    function sendMessage(messageContent) {
        if (messageContent) {
            const chatId = document.getElementById("chatId").value;
            fetch(`/api/MessageApi/Create`, {
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
                .catch(() => {
                    window.location.href = '/Home/Error?statusCode=400';
                });
        }
    }

    function deleteChat(e) {
        e.preventDefault();

        const chatId = e.currentTarget.dataset.chatId;
        if (!chatId) return;

        fetch('/Chat/Delete', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: new URLSearchParams({ chatId: chatId })
        })
            .then((res) => {
                if (!res.ok) {
                    throw new Error('Network response was not ok');
                }
            })
            .catch(() => {
                window.location.href = '/Home/Error?statusCode=400';
            });
    }

    ///
    ///   HELPER HANDLERS
    ///

    function appendToConversationArea(chat) {
        const conversationArea = document.querySelector('.conversation-area');

        const chatCard = document.createElement('div');
        chatCard.classList.add('msg', 'online', 'chat-card');
        chatCard.addEventListener('click', () => loadChat(chatCard));

        chatCard.innerHTML = `
            <input id="chatCardId" type="hidden" value="${chat.id}" />
                <img class="msg-profile" src=${chat.imageUrl} alt="" />
                <div class="msg-detail">
                    <div class="msg-username">${chat.name}</div>
                    <div class="msg-content">
                        <span class="msg-message msg-last-message">${chat.lastMessage}</span>
                        <span class="msg-date msg-last-active">${chat.lastActive}</span>
                    </div>
                </div>
        `;

        hideNoChatCard();

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

    function removeChatCard(chatId) {
        const chat = document.querySelector(`.chat-card input[value="${chatId}"]`);
        if (chat) chat.closest('.chat-card').remove();
    }

    function hideNoChatCard() {
        const noChatCard = document.querySelector('.no-chats-card');

        if (noChatCard) {
            noChatCard.style.display = 'none';
        }
    }

    function showNoChatCard() {
        const noChatCard = document.querySelector('.no-chats-card');
        const chatCards = document.querySelectorAll('.conversation-area .chat-card');

        if (!noChatCard) return;

        if (chatCards.length <= 1) {
            noChatCard.style.display = 'block';
        } else {
            noChatCard.style.display = 'none';
        }
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
