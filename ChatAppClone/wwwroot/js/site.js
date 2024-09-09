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
