/*Anchor Navigation Script*/
//function BlazorScrollToId(id) {
//    const element = document.getElementById(id);
//    if (element instanceof HTMLElement) {
//        element.scrollIntoView({
//            behavior: "smooth",
//            block: "start",
//            inline: "nearest"
//        });
//    }
//}

/*Anchor Navigation Script - адаптированный код для отступа от верха*/
//function BlazorScrollToId(id) {
//    const element = document.getElementById(id);
//    if (element instanceof HTMLElement) {
//        // Получаем текущую позицию прокрутки страницы
//        const currentScrollPosition = window.pageYOffset || document.documentElement.scrollTop;

//        // Получаем позицию анкора на странице
//        const elementPosition = element.getBoundingClientRect().top + currentScrollPosition;

//        // Вычисляем новую позицию прокрутки с учетом отступа
//        const newPosition = elementPosition - 100;

//        // Прокручиваем страницу к новой позиции
//        window.scrollTo({
//            top: newPosition,
//            behavior: "smooth"
//        });
//    }
//}

//function moveValue() {
//    var destinationInput = document.getElementById('messageServiceInput');
//    setTimeout(function () {
//        destinationInput.classList.add('highlight');
//    }, 300);
//    setTimeout(function () {
//        destinationInput.classList.remove('highlight');
//    }, 1000);
//}


