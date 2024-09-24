const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

notificationConnection.on("ReceiveNotification", function (message) {
    let notificationCountElement = document.querySelector('.notifications-tab');
    let currentCount = parseInt(notificationCountElement.textContent);
    notificationCountElement.textContent = currentCount + 1;

    showNotificationModal(message);
});

notificationConnection.start().then(function () {
    console.log("SignalR connection established");
}).catch(function (err) {
    console.error("SignalR connection error:", err.toString());
});

function showNotificationModal(message) {
    const modal = document.createElement('div');
    modal.classList.add('notification-modal');

    const infoIcon = document.createElement('i');
    infoIcon.classList.add('fa-solid', 'fa-circle-info');

    const messageParagraph = document.createElement('p');
    messageParagraph.textContent = message;

    const closeIcon = document.createElement('i');
    closeIcon.classList.add('fa-solid', 'fa-times');

    closeIcon.addEventListener('click', () => {
        modal.remove();
    });

    modal.appendChild(infoIcon);
    modal.appendChild(messageParagraph);
    modal.appendChild(closeIcon);

    document.body.appendChild(modal);

    setTimeout(() => {
        modal.remove();
    }, 3000);
}

function toggleIcon(element) {
    element.classList.toggle('clicked');
    const icon = element.querySelector('.fa');

    if (icon.classList.contains('fa-bars')) {
        icon.classList.remove('fa-bars');
        icon.classList.add('fa-times');
    } else {
        icon.classList.remove('fa-times');
        icon.classList.add('fa-bars');
    }

    const mobileNav = document.querySelector('.site-nav .mobile-nav');
    if (mobileNav.classList.contains('show')) {
        mobileNav.classList.remove('show');
    } else {
        mobileNav.classList.add('show');
    }
}
