/* Preloader */
window.removePreloader = function () {
    let preloader = document.querySelector('#preloader');
    if (preloader) {
        preloader.remove();
    }
};

window.initJsFunctions = function () {

    /* Easy selector helper function - вспомогательная функция */
    const select = (el, all = false) => {
        el = el.trim()
        if (all) {
            return [...document.querySelectorAll(el)]
        } else {
            return document.querySelector(el)
        }
    }

    /* Scrolls to an element with header offset - вспомогательная функция */
    const scrollto = (el) => {
        let header = select('#header')
        let offset = header.offsetHeight
        let elementPos = select(el).offsetTop
        window.scrollTo({
            top: elementPos - offset,
            behavior: 'smooth'
        })
    }

    /* Easy on scroll event listener - вспомогательная функция */
    const onscroll = (el, listener) => {
        el.addEventListener('scroll', listener)
    }

    /* Toggle .header-scrolled class to #header when page is scrolled */
    let selectHeader = select('#header')
    let selectTopbar = select('#topbar')
    if (selectHeader) {
        const headerScrolled = () => {
            if (window.scrollY > 100) {
                selectHeader.classList.add('header-scrolled')
                if (selectTopbar) {
                    selectTopbar.classList.add('topbar-scrolled')
                }
            } else {
                selectHeader.classList.remove('header-scrolled')
                if (selectTopbar) {
                    selectTopbar.classList.remove('topbar-scrolled')
                }
            }
        }
        window.addEventListener('load', headerScrolled)
        onscroll(document, headerScrolled)
    }

    /* Back to top button */
    let backtotop = select('.back-to-top')
    if (backtotop) {
        const toggleBacktotop = () => {
            if (window.scrollY > 100) {
                backtotop.classList.add('active')
            } else {
                backtotop.classList.remove('active')
            }
        }
        window.addEventListener('load', toggleBacktotop)
        onscroll(document, toggleBacktotop)
    }

    /* Clients slider */
    new Swiper('.clients-slider', {
        speed: 600,
        loop: true,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false
        },
        slidesPerView: 'auto',
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
            clickable: true
        },
        breakpoints: {
            320: {
                slidesPerView: 1,
                spaceBetween: 20
            },
            1200: {
                slidesPerView: 2,
                spaceBetween: 20
            }
        }
    });

    /* Share button */
    var shareButtons = document.querySelectorAll('.shareButton');
    shareButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            var pageURL;
            try {
                pageURL = document.location.href;
            } catch (e) {
                pageURL = 'https://ecoplaza.pro';
            }
            if (navigator.share) {
                navigator.share({
                    title: 'Экологическая компания',
                    text: 'Разработка и согласование природоохранной документации',
                    url: pageURL,
                });
            }
        });
    });
};



/* Text highlighting */
function moveValue() {
    var destinationInput = document.getElementById('messageServiceInput');
    setTimeout(function () {
        destinationInput.classList.add('highlight');
    }, 300);
    setTimeout(function () {
        destinationInput.classList.remove('highlight');
    }, 1000);
}