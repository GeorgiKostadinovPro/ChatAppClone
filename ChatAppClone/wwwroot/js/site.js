function toggleIcon(element) {
    console.log(element);
    element.classList.toggle('clicked');
    const icon = element.querySelector('.fa');

    if (icon.classList.contains('fa-bars')) {
        icon.classList.remove('fa-bars');
        icon.classList.add('fa-times');
    } else {
        icon.classList.remove('fa-times');
        icon.classList.add('fa-bars');
    }
}