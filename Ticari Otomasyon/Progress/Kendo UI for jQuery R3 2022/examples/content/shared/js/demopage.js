var sidebar = {
    resize: function () {
        var element = $(".kd-sidebar").eq(0),
            subNav = $('.kd-sub-nav'),
            scrollTop = $(document).scrollTop(),
            webinarBarHeight = $("#webinar-banner:visible").outerHeight() || 0,
            navHeight = $("#js-tlrk-nav").outerHeight(),
            subNavHeight = subNav.outerHeight(),
            isMobileSidebar = $(window).width() < 1240,
            isIE = /(msie\s|trident.*? rv:)([\w.]+)/i.test(window.navigator.userAgent),
            height, top;

        if (element.length) {
            height = isMobileSidebar ?  $('.kd-demo-content .container').outerHeight() + 64
                : $('html').outerHeight() - navHeight - subNavHeight - (scrollTop >= webinarBarHeight ? 0 : webinarBarHeight);

            top = navHeight + subNavHeight + (scrollTop >= webinarBarHeight ? 0 : webinarBarHeight - scrollTop);

            element.css("top", isMobileSidebar ? "" : top);
            element.height(height);
        }

        if (isIE && scrollTop >= webinarBarHeight ) {
            subNav.addClass("kd-sub-nav-sticky");
        } else {
            subNav.removeClass("kd-sub-nav-sticky");
        }
    },

    closeActiveDropDowns: function () {
        var sidebar = $(".kd-sidebar").eq(0),
            ddlWrapper = $('.tabstrip .dropdown-wrapper'),
            dllPopup = $('.tabstrip .dropdown');

        if (sidebar.hasClass("expanded")) {
            ddlWrapper.removeClass("active");
            dllPopup.removeClass("active");
        }
    },

    toggle: function (ev) {
        ev.stopPropagation();
        var element = $(".kd-sidebar").eq(0);

        if (element.length) {
            element.height($('.kd-demo-content .container').outerHeight() +  64);

            element.toggleClass("expanded");

            sidebar.closeActiveDropDowns();
        }
    },

    expandActiveCategory: function () {
        var sidebar = $(".kd-sidebar").eq(0),
            activeRootCategory = sidebar.find(".root-nav-widgets.active").eq(0),
            expanded = $(".root-nav-widgets.expanded");

        if (expanded.length) {
            for (var i = 0; i < expanded.length; i += 1) {
                var category = $(expanded[i]);

                if (category[0] !== activeRootCategory[0]) {
                    category.slideToggle().removeClass("expanded");
                }
            }
        }

        if (activeRootCategory && !activeRootCategory.hasClass("expanded")) {
            activeRootCategory.slideToggle().addClass("expanded");
            sidebar.animate({
                scrollTop: activeRootCategory.position().top
            }, 500);
        }
    },

    switchView: function (ev) {
        var button = $(this),
            sidebarContainer = $(".kd-sidebar-container").eq(0),
            sidebarRootNav = $(".kd-sidebar-root-nav"),
            arrowPrev = $(".kd-sidebar-toggle .arrow-prev"),
            arrowNext = $(".kd-sidebar-toggle .arrow-next"),
            currentWidgetName = $(".kd-sidebar-current").data("value");

        if (button.hasClass("back-nav")) {
            button
                .removeClass("back-nav")
                .addClass("forward-nav")
                .children(".text").text(currentWidgetName)
                .attr("title", "See " + currentWidgetName + " demos");

            sidebarRootNav.attr("aria-hidden", false);
            sidebarRootNav.find("a").removeAttr("tabindex");

            sidebarContainer.addClass("root");
            sidebar.expandActiveCategory();

            arrowPrev.hide();
            arrowNext.css("display", "flex");
        }
        else {
            button
                .removeClass("forward-nav")
                .addClass("back-nav")
                .children(".text").text("All Components")
                .attr("title", "See all components");

            sidebarContainer.removeClass("root");

            setTimeout(function() {
                sidebarRootNav.attr("aria-hidden", true);
                sidebarRootNav.find("a").attr("tabindex", "-1");
            }, 300);

            arrowPrev.css("display", "flex");
            arrowNext.hide();
        }

        ev.preventDefault();
    },

    expandCategories: function (ev) {
        var expanded = $(".root-nav-widgets.expanded"),
            list = $(ev.target).siblings('ul');

        for (var i = 0; i < expanded.length; i += 1) {
            var item = expanded[i];
            if (list[0] != item) {
                $(item).slideToggle().toggleClass("expanded");
            }
        }

        list.stop(true).slideToggle().toggleClass("expanded");
    },

    init: function() {
        this.resize();

        $(".kd-side-nav-toggle").click(this.toggle);
        $(".kd-sidebar-toggle a").click(this.switchView);
        $(".kd-sidebar-root-nav").find('h3').on("click", this.expandCategories);
    }
};

$(document).ready(function () {
    sidebar.init();

    $(".kd-breadcrumb-nav .active").click(function(ev) {
        ev.preventDefault();
    });
});

$(document).on("click", function (ev) {
    ev.stopPropagation();
    var sidebar = $(".kd-sidebar");

    if (sidebar.is(":visible") && !sidebar.is(ev.target) && sidebar.has(ev.target).length === 0) {
        sidebar.toggleClass("expanded");
    }
});

$(window).on("resize scroll", sidebar.resize);