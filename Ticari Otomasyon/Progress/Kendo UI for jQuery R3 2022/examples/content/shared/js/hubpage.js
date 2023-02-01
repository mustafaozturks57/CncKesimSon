var carousel = function () {
    var NEXT = 'next';
    var PREV = 'prev';
    var ARROWS = '.kd-carousel-controls';
    var PREV_BUTTON = '.kd-carousel-control-prev';
    var NEXT_BUTTON = '.kd-carousel-control-next';
    var SLIDES = '.kd-carousel-slides';
    var SLIDES_ACTIVE = '.kd-carousel-slide.active';
    var SLIDE = '.kd-carousel-slide';
    var INDICATORS = '.kd-carousel-indicators';
    var INDICATOR = '.kd-carousel-indicator';
    var INDICATOR_ACTIVE = '.kd-carousel-indicator.active';
    var SLIDE_WRAP = '.kd-carousel-slide-wrap';
    var SLIDE_WRAP_ACTIVE = '.kd-carousel-slide-wrap.active';
    var ACTIVE = "active";
    var DISABLED = "disabled";
    var APP_CONTENT = ".kd-application-content";

    function init() {
        var slides = document.querySelector(SLIDES);

        if (slides) {
            wrapSlides();
            toggleActive();
            toggleArrows();
            renderIndicators();
            toggleIndicatorActive();
        }
    }

    function attachEventHandlers() {
        var prev = document.querySelector(PREV_BUTTON),
            next = document.querySelector(NEXT_BUTTON),
            slides = document.querySelectorAll(SLIDE);

        if (prev && next) {
            prev.addEventListener("click", prevClick);
            prev.addEventListener("keydown", function(ev) {
                    if (ev.keyCode === 13) {
                        prevClick(ev);
                    }
                });
            next.addEventListener("click", nextClick);
            next.addEventListener("keydown", function(ev) {
                    if (ev.keyCode === 13) {
                        nextClick(ev);
                    }
                });
        }

        if (slides && slides.length > 0) {
            Array.prototype.slice.call(slides)
                .forEach(function (slide) {
                    slide.addEventListener("focus", focusSlide);
                });
        }
    }

    function focusSlide (ev) {
        ev.target.querySelector(APP_CONTENT).style.display = "block";
        ev.target.addEventListener("focusout", focusoutSlide);
    }

    function focusoutSlide (ev) {
        var target = ev.currentTarget;

        setTimeout(function() {
            if (!target.contains(document.activeElement)) {
                target.removeEventListener("focusout", focusoutSlide);
                target.querySelector(APP_CONTENT).style = "";
            }
        });
    }

    function getVisibleCount() {
        var container = document.querySelector(SLIDES),
            active = document.querySelector(SLIDES_ACTIVE);

        return parseInt(container.offsetWidth / active.offsetWidth, 10);
    }

    function toggleArrows() {
        var arrowsContainer = document.querySelector(ARROWS),
            total = document.querySelectorAll(SLIDE).length,
            slideCount = getVisibleCount(),
            groups = Math.ceil(total / slideCount);

        if (arrowsContainer) {
            arrowsContainer.classList.toggle("hidden", groups <= 1);
        }
    }

    function renderIndicators() {
        var container = document.querySelector(INDICATORS),
            total = document.querySelectorAll(SLIDE).length,
            slideCount = getVisibleCount(),
            groups = Math.ceil(total / slideCount);

        if (!container) {
            return;
        }

        var fragment = document.createDocumentFragment();
        for (var i = 0; i < groups; i += 1) {
            var span = document.createElement("span");
            span.setAttribute("data-slide", i);
            span.classList.add("kd-carousel-indicator");

            fragment.appendChild(span);
        }

        container.innerHTML = "";
        container.appendChild(fragment);

        Array.prototype.slice.call(document.querySelectorAll(INDICATOR))
            .forEach(function (item) {
                item.addEventListener("click", indicatorClick);
            });

        container.classList.toggle("vs-hidden", groups <= 1);
    }

    function toggleActive() {
        var first = document.querySelector(SLIDE_WRAP);

        first.classList.add(ACTIVE);
    }

    function wrapSlides() {
        var slideWraps = document.querySelectorAll(SLIDE_WRAP),
            slides = Array.from(document.querySelectorAll(SLIDE)),
            visibleCount = getVisibleCount(),
            counter = 0,
            i, j, temp;

        unwrap(slideWraps);

        for (i = 0, j = slides.length; i < j; i += visibleCount) {
            temp = slides.slice(i, i + visibleCount);

            var wrapper = document.createElement('div');
            wrapper.classList.add("kd-carousel-slide-wrap");
            wrapper.setAttribute("data-slide", counter++);

            for (var k = 0; k < temp.length; k += 1) {
                wrapper.appendChild(temp[k]);
            }

            document.querySelector(SLIDES).appendChild(wrapper);
        }
    }

    function prevClick(ev) {
        var index = getActiveSlideIdx(),
            isDisabled = ev.target.parentElement.classList.contains(DISABLED);

        if (isDisabled) {
            return;
        }

        slide(--index, PREV);
    }

    function nextClick(ev) {
        var index = getActiveSlideIdx(),
            isDisabled = ev.target.parentElement.classList.contains(DISABLED);

        if (isDisabled) {
            return;
        }

        slide(++index, NEXT);
    }

    function getActiveSlideIdx() {
        var active = document.querySelector(SLIDE_WRAP_ACTIVE);

        return parseInt(active.dataset.slide);
    }

    function hideActiveSlide() {
        var active = document.querySelector(SLIDE_WRAP_ACTIVE);

        active.classList.toggle(ACTIVE);
    }

    function slide(index, direction) {
        var isNext = direction === NEXT,
            isPrev = direction === PREV,
            current = document.querySelector(SLIDE_WRAP_ACTIVE),
            next = document.querySelector(".kd-carousel-slide-wrap[data-slide='" + index + "']"),
            directionalClassNameCurrent = isNext ? "kd-slide-left" : "kd-slide-right",
            directionalClassNameNext = isNext ? "kd-slide-right" : "kd-slide-left",
            total = document.querySelectorAll(SLIDE_WRAP).length;

        if (isPrev && index < 0 || isNext && index >= total) {
            return;
        }

        toggleArrowsState(index);

        toggleIndicatorActive(index);

        hideActiveSlide();

        current.classList.add(directionalClassNameCurrent);
        next.classList.add(directionalClassNameNext);

        setTimeout(function () {
            current.classList.remove(directionalClassNameCurrent);
        }, 600);

        setTimeout(function () {
            next.classList.remove(directionalClassNameNext);
            next.classList.add(ACTIVE);
        }, 0);
    }

    function toggleArrowsState(index) {
        var total = document.querySelectorAll(SLIDE_WRAP).length - 1,
            nextArrow = document.querySelector(NEXT_BUTTON),
            backArrow = document.querySelector(PREV_BUTTON),
            nextDisabled = isNaN(index) || index >= total,
            prevDisabled = index <= 0 || isNaN(index);


        nextArrow.classList.toggle(DISABLED, nextDisabled);
        backArrow.classList.toggle(DISABLED, prevDisabled);

        if (nextDisabled) {
            nextArrow.setAttribute("aria-disabled", true);
        } else {
            nextArrow.removeAttribute("aria-disabled");
        }

        if (prevDisabled) {
            backArrow.setAttribute("aria-disabled", true);
        } else {
            backArrow.removeAttribute("aria-disabled");
        }
    }

    function indicatorClick(ev) {
        slide(parseInt(this.dataset.slide), NEXT);
    }

    function toggleIndicatorActive(index) {
        var indicatorActive = document.querySelectorAll(INDICATOR_ACTIVE),
            indicators = document.querySelectorAll(INDICATOR);

        if (!indicators.length) { return; }

        if (!indicatorActive.length) {
            indicators[0].classList.add(ACTIVE);
            return;
        }

        indicatorActive[0].classList.toggle(ACTIVE);

        if (indicators[index]) {
            indicators[index].classList.toggle(ACTIVE);
        }
    }

    function unwrap(wrappers) {
        for (var i = 0; i < wrappers.length; i++) {
            var docFrag = document.createDocumentFragment();
            var wrapper = wrappers[i];

            while (wrapper.firstChild) {
                var child = wrapper.removeChild(wrapper.firstChild);
                docFrag.appendChild(child);
            }

            wrapper.parentNode.replaceChild(docFrag, wrapper);
        }
    }

    return {
        init: init,
        slide: slide,
        attachEvents: attachEventHandlers
    };
}();

var activateResizeCarousel;

window.addEventListener('DOMContentLoaded', function () {
    carousel.init();
    carousel.attachEvents();
});

window.addEventListener('resize', function () {
    clearTimeout(activateResizeCarousel);

    activateResizeCarousel = setTimeout(function () {
        carousel.init();
    }, 500);
});


var isIE = /(msie\s|trident.*? rv:)([\w.]+)/i.test(window.navigator.userAgent);

if (isIE) {
    $(document).on("scroll", function () {
        var subNav = $('.kd-sub-nav'),
            scrollTop = $(document).scrollTop(),
            webinarBarHeight = $("#webinar-banner:visible").outerHeight() || 0;

        if (scrollTop >= webinarBarHeight) {
            subNav.addClass("kd-sub-nav-sticky");
        } else {
            subNav.removeClass("kd-sub-nav-sticky");
        }
    });
}
