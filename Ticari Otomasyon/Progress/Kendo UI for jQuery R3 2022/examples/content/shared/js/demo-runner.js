(function() {
    var DOT = ".",
        SPACE = " ",
        HASH = "#",
        ID = "id",
        VAL = "val",

        ARIA_ACTIVEDESCENDANT = "aria-activedescendant",

        LINK = "kd-link",
        TABSTRIP = "tabstrip",
        TABSTRIP_TAB = "tabstrip-tab",
        TABSTRIP_ITEM = "tabstrip-item",
        TABSTRIP_SWITCH = "tabstrip-switch",
        TRY_KENDO = "try-kendo",
        DROPDOWN_WRAPPER = "dropdown-wrapper",
        DROPDOWN_TOGGLE = "dropdown-toggle",
        DROPDOWN = "dropdown",
        EDIT_DROPDOWN = "edit-dropdown",
        EDIT_MENU = "edit-example-menu",
        ACTIVE_EDIT = "active-edit",
        EDIT_FOCUS = "kd-edit-focus",
        ACTIVE = "active",
        THEME_CHOOSER = "theme-chooser-dropdown",
        THEME_CHOOSER_BTN = "theme-chooser-toggle-btn",
        THEME_CHOOSER_HEADER = "theme-chooser-header",
        THEME_CHOOSER_WRAPPER = "theme-chooser-wrapper",
        THEMES_LIST = "themes-list",
        THEME_COL = "theme-col",
        THEME = "kd-theme",
        ACTIVE_THEME = "active-theme",
        THEME_FOCUS = "kd-theme-focus",
        THEME_SELECTED = "kd-theme-selected",
        THEME_TYPE = "theme-type",
        DEMO_BTN_WRAPPER = "demo-button-wrapper",
        TYPE_CHOOSER = "type-chooser",
        EXAMPLE_CONSOLE = "kd-example-console",
        CLEAR = "clear",
        THEMEBUILDER = "theme-builder",
        THEMEBUILDER_BTN = "tb-button",
        SWITCH_SELECTED_OP = "switch-option-selected",
        SOURCE_PANE = "source-code-pane",

        CLICK = "click",
        KEYDOWN = "keydown",
        RESIZE = "resize",
        BLUR = "blur",
        READY = "ready",
        KENDO_READY = "kendoReady",

        RUN_DEMO_BTN = "runDemo",

        CHART = "k-chart",

        DROPDOWN_SELECTOR = DOT + DROPDOWN_WRAPPER + SPACE + DOT + DROPDOWN,

        shouldClose = true;

    $(document).on(KENDO_READY, function () {
        $(window).on(RESIZE, function () {
            if (window.kendo) {
                kendo.resize($(DOT + CHART));
            }
        });
    });

    $(document).on(READY, function () {
        var runDemoBtn = $(HASH + RUN_DEMO_BTN);

        if (runDemoBtn.length && isMobile()) {
            $(DOT + DEMO_BTN_WRAPPER).show();
            $(HASH + RUN_DEMO_BTN).on(CLICK, loadDemo);
        } else {
            loadDemo();
        }

        $(DOT + TABSTRIP + SPACE + DOT + TABSTRIP_TAB)
            .on(CLICK, tabStripSelect)
            .on(KEYDOWN, function(e) {
                if (e.keyCode === kendo.keys.ENTER) {
                    tabStripSelect(e);
                }
            });

        $(DOT + TRY_KENDO)
            .on(CLICK, openInDojo)
            .on(KEYDOWN, function(e) {
                if (e.keyCode === kendo.keys.ENTER) {
                    openInDojo(e);
                }
            });

        $(DOT + TABSTRIP + SPACE + DOT + DROPDOWN_TOGGLE).on(CLICK, toggleDropdown);
        $(DOT + THEME_CHOOSER + SPACE + DOT + THEME).on(CLICK, themeChooserChange);
        $(DOT + TYPE_CHOOSER).on(CLICK, themeTypeChange);

        $(DOT + EXAMPLE_CONSOLE + SPACE + DOT + CLEAR)
            .on(CLICK, clearConsole)
            .on(KEYDOWN, function(e) {
                if (e.keyCode === kendo.keys.ENTER) {
                    clearConsole(e);
                }
            });

        $(DOT + TABSTRIP_SWITCH + ">div")
            .on(CLICK, toggleSwitcher)
            .on(KEYDOWN, function(e) {
                if (e.keyCode === kendo.keys.ENTER) {
                    toggleSwitcher(e);
                }
            });

        // Close dropdown popups
        $(document).on(CLICK, function (ev) {
            var target = $(ev.target);
            if (target.parents(DOT + DROPDOWN_WRAPPER).length || target.is(DOT + DROPDOWN_TOGGLE)) {
                shouldClose = false;
                return;
            }

            $(DOT + DROPDOWN + DOT + ACTIVE).removeClass(ACTIVE);
            $(DOT + DROPDOWN_WRAPPER + DOT + ACTIVE).removeClass(ACTIVE);

            releaseThemeFocus();
            releaseEditFocus();
        });

        $(DOT + EDIT_DROPDOWN + SPACE + DOT + DROPDOWN_TOGGLE)
            .on(KEYDOWN, editKeyDown)
            .on(BLUR, function(e) {
                var target  = $(e.currentTarget);

                if (shouldClose) {
                    target.siblings(DOT + DROPDOWN).removeClass(ACTIVE);
                    releaseEditFocus();
                } else {
                    shouldClose = true;
                    return;
                }
            });

        $(DOT + TABSTRIP + SPACE + DOT + THEME_CHOOSER_BTN)
            .on(KEYDOWN, themeChooserKeyDown)
            .on(BLUR, function() {
                setTimeout(function() {
                    var themeBuilderBtn = $(DOT + THEME_CHOOSER_HEADER + SPACE + DOT + THEMEBUILDER + SPACE + DOT + THEMEBUILDER_BTN);

                    if (document.activeElement === themeBuilderBtn[0]) {
                        themeBuilderBtn.off(BLUR).on(BLUR, function() {
                            setTimeout(function() {
                                if (document.activeElement !== document.querySelector(DOT + THEME_CHOOSER_BTN)) {
                                    closeThemeChoser();
                                }
                            });
                        });
                    } else {
                        closeThemeChoser();
                    }
                });
            });
    });

    //Dropdown toggle
    function toggleDropdown(e) {
        var target = $(e.target).closest(DOT + DROPDOWN_TOGGLE),
            dropdown = target.parents(DOT + DROPDOWN_WRAPPER),
            popup = dropdown.find(DOT + DROPDOWN),
            selected;

        $(DROPDOWN_SELECTOR).removeClass(ACTIVE);
        popup.eq(0).toggleClass(ACTIVE);

        if (dropdown.hasClass(THEME_CHOOSER_WRAPPER)) {
            releaseThemeFocus();

            target.attr(ARIA_ACTIVEDESCENDANT, ACTIVE_THEME);
            selected = popup.find(DOT + THEME_SELECTED);

            if (selected.length === 0) {
                selected = popup.find(DOT + THEME).first();
            }

            selected.addClass(THEME_FOCUS).attr(ID, ACTIVE_THEME);
        } else if (dropdown.hasClass(EDIT_DROPDOWN)) {
            releaseEditFocus();

            target.attr(ARIA_ACTIVEDESCENDANT, ACTIVE_EDIT);
            selected = popup.find("li").first();

            selected.addClass(EDIT_FOCUS).attr(ID, ACTIVE_EDIT);
        }
    }
    //End

    // Edit dropdown logic
    function releaseEditFocus() {
        var popup = $(HASH + EDIT_MENU),
            button = $(DOT + EDIT_DROPDOWN + SPACE + DOT + DROPDOWN_TOGGLE);

        button.removeAttr(ARIA_ACTIVEDESCENDANT);
        popup.find(HASH + ACTIVE_EDIT).removeAttr(ID);
        popup.find(DOT + EDIT_FOCUS).removeClass(EDIT_FOCUS);
    }

    function editKeyDown(e) {
        var key = e.keyCode,
            keys = kendo.keys,
            current = $(HASH + ACTIVE_EDIT),
            popup = $(DOT + EDIT_DROPDOWN + SPACE + DOT + DROPDOWN);

        if (key === keys.ESC || (key === keys.UP && e.altKey)) {
            $(DROPDOWN_SELECTOR).removeClass(ACTIVE);
            releaseEditFocus();
        } else if (key === keys.ENTER && popup.hasClass(ACTIVE)) {
            current.find(DOT + LINK)[0].click();
        } else if (key === keys.ENTER || (key === keys.DOWN && e.altKey)) {
            toggleDropdown(e);
        } else if (key === keys.DOWN || key === keys.UP) {
            e.preventDefault();
            releaseEditFocus();

            current.siblings().first();
            $(DOT + EDIT_DROPDOWN + SPACE + DOT + DROPDOWN_TOGGLE).attr(ARIA_ACTIVEDESCENDANT, ACTIVE_EDIT);

            current.siblings().first().addClass(EDIT_FOCUS).attr(ID, ACTIVE_EDIT);
        }
    }
    //End

    // Open in Dojo
    function openInDojo(e) {
        e.preventDefault();
        if (!window.dojo) {
            var scripts = $("#dojo-js").toArray();
            var dfd = $.Deferred();
            loadScripts(scripts, dfd);
        }

        postToDojo(e);
    }

    function postToDojo(e) {
        var button = $(e.target).closest(DOT + TRY_KENDO);

        $.get(button.data("url")).done(function (data) {
            window.dojo.postSnippet($(data).text(), window.location.href);
        });
    }
    // End

    // Run to click logic
    function loadStyles() {
        $("link[data-href]").each(function (index, link) {
            $(link).attr("href", $(link).attr("data-href"));
        });
    }

    function loadDemo() {
        $(DOT + DEMO_BTN_WRAPPER).hide();

        loadStyles();
        var scripts = $("script[data-src]").toArray();
        var dfd = $.Deferred();

        dfd.done(function () {
            $(".kd-loader-wrap").hide();

            if (IS_ANGULARJS_EXAMPLE) {
                return;
            }

            resetThemableOptions();

            $("#demo-runner").html($("#demoCode").text());
            $(document).trigger(KENDO_READY);
        });

        loadScripts(scripts, dfd);
    }

    function loadScripts(scripts, dfd) {
        if (!scripts.length) {
            return;
        }

        var script = scripts.shift();
        $(script).on("load", function (e) {
            // Configure DOJO
            if ($(script).is("#dojo-js")) {
                dojo.configuration = {
                    url: DOJO_ROOT,
                    cdnRoot: CDN_ROOT
                };
            }

            if (scripts.length == 0) {
                dfd.resolve();
            }
            loadScripts(scripts, dfd);
        });
        $(script).attr("src", $(script).attr("data-src"));
        $(script).removeAttr("data-src");
    }

    function resetThemableOptions() {
        var themeName = window.selectedTheme;
        var themable = ["Chart", "TreeMap", "Diagram", "StockChart", "Sparkline", "RadialGauge", "LinearGauge", "ArcGauge", "CircularGauge", "CircularProgressbar"];

        if (kendo.dataviz && themeName) {
            var isSass = /(default-|bootstrap-|material-|classic-|fluent-)/.test(themeName);
            if (isSass) {
                kendo.dataviz.autoTheme(true);
            }

            for (var i = 0; i < themable.length; i++) {
                var widget = kendo.dataviz.ui[themable[i]] || kendo.ui[themable[i]];

                if (widget) {
                    widget.fn.options.theme = isSass ? "sass" : themeName;
                }
            }
        }
    }
    // End

    // Tabstrip logic
    function tabStripSelect(ev) {
        var tab = $(ev.target).closest(DOT + TABSTRIP_ITEM);
        var tabstrip = tab.closest(DOT + TABSTRIP);
        var panes = tabstrip.siblings(".tabstrip-pane");
        var targetPane = $( "#" + tab.data("container"));

        if (tab.is(DOT + ACTIVE)) {
            return;
        }

        var activeElements = tabstrip.find(DOT + TABSTRIP_ITEM + DOT + ACTIVE).add($.grep(panes, function (item) {
            return $(item).is(DOT + ACTIVE);
        }));

        activeElements.removeClass(ACTIVE);

        tab.add(targetPane).addClass(ACTIVE);

        if (targetPane.is(HASH + SOURCE_PANE) && !targetPane.find(DOT + ACTIVE).length) {
            loadFirstSource(targetPane);

            var tagHelper = $('#source-code-pane [data-container="taghelper-cshtml"]');
            if (tagHelper.length) {
            tagHelper.insertAfter(tagHelper.siblings().first());
            tagHelper.hide();

                if (typeof (Storage) !== "undefined") {
                    if (sessionStorage.isTagHelper) {
                        $(DOT + TABSTRIP_SWITCH + ">div").last().click();
                    }
                }
            }

            return;
        }

        if (tab.is("[data-url]")) {
            loadSource(tab, targetPane);
        }
    }

    function loadFirstSource(sourcePane) {
        var srcTabstrip = sourcePane.find(DOT + TABSTRIP);
        var firstTab = srcTabstrip.find(DOT + TABSTRIP_ITEM).eq(0);
        var firstPane = $(HASH + firstTab.data("container"));

        firstTab.add(firstPane).addClass(ACTIVE);

        loadSource(firstTab, firstPane);
    }

    function loadSource(tab, pane) {
        var url = tab.data("url");

        if (!window.prettyPrint) {
            loadPrettify();
        }

        $.get(url).done(function (data) {
            tab.removeAttr("data-url");
            if (url.indexOf("http") === 0) {
                data = "<pre class='prettyprint'>" + htmlEncode(data) + "</div>";
            }


            pane.html(data);
            addCopyButton(pane);

            if (window.prettyPrint) {
                prettyPrint();
            }
        });
    }

    function htmlEncode(value) {
        var ampRegExp = /&/g,
            ltRegExp = /</g,
            quoteRegExp = /"/g,
            aposRegExp = /'/g,
            gtRegExp = />/g;

        return ("" + value).replace(ampRegExp, "&amp;").replace(ltRegExp, "&lt;").replace(gtRegExp, "&gt;").replace(quoteRegExp, "&quot;").replace(aposRegExp, "&#39;");
    }

    function loadPrettify() {
        var scripts = $("#prettify-js").toArray();
        var dfd = $.Deferred();
        loadScripts(scripts, dfd);

        dfd.done(function () {
            prettyPrint();
        });
    }
    // End

    // ThemeChooser logic
    function setThemeSwatch(choosenTheme) {
        window.location.search = "autoRun=true&theme=" + choosenTheme;
    }

    function themeChooserChange(ev) {
        var choosenTheme = $(ev.target).closest("li[data-val]").data(VAL);
        setThemeSwatch(choosenTheme);
    }

    function themeTypeChange(ev) {
        var target = $(ev.target),
            list = $(DOT + THEME_CHOOSER + SPACE + DOT + THEMES_LIST),
            tabs = $(DOT + TYPE_CHOOSER + SPACE + DOT + THEME_TYPE),
            chosenType = target.data().select;

        if (target.hasClass(ACTIVE)) {
            return;
        }

        list.hide();
        tabs.toggleClass(ACTIVE);
        $("." + chosenType).show();
    }

    function closeThemeChoser() {
        if (shouldClose) {
            $(DOT + THEME_CHOOSER_WRAPPER + SPACE + DOT + DROPDOWN).removeClass("active");
            releaseThemeFocus();
        } else {
            shouldClose = true;
        }
    }

    function releaseThemeFocus() {
        var popup = $(DOT + THEMES_LIST),
            button = $(DOT + THEME_CHOOSER_BTN);

        button.removeAttr(ARIA_ACTIVEDESCENDANT);
        popup.find(HASH + ACTIVE_THEME).removeAttr(ID);
        popup.find(DOT + THEME_FOCUS).removeClass(THEME_FOCUS);
    }

    function themeChooserKeyDown(e) {
        var key = e.keyCode,
            keys = kendo.keys,
            current = $(HASH + ACTIVE_THEME),
            popup = $(DOT + THEME_CHOOSER),
            next, nextGroup, choosenTheme;

        if (key === keys.ESC || (key === keys.UP && e.altKey)) {
            $(DROPDOWN_SELECTOR).removeClass(ACTIVE);
            releaseThemeFocus();
        } else if (key === keys.ENTER && popup.hasClass(ACTIVE)) {
            choosenTheme = current.data(VAL);
            setThemeSwatch(choosenTheme);
        } else if (key === keys.ENTER || (key === keys.DOWN && e.altKey)) {
            toggleDropdown(e);
        } else if (key === keys.DOWN) {
            e.preventDefault();

            next = current.next();

            if (next.length === 0) {
                next = current.closest(DOT + THEME_COL).next().find(DOT + THEME).eq(0);
            }
            if (next.length === 0) {
                next = current.closest(DOT + THEMES_LIST).find(DOT + THEME).eq(0);
            }

            releaseThemeFocus();
            moveToTheme(next);
        } else if (key === keys.UP) {
            e.preventDefault();

            next = current.prev();

            if (next.length === 0) {
                next = current.closest(DOT + THEME_COL).prev().find(DOT + THEME).last();
            }
            if (next.length === 0) {
                next = current.closest(DOT + THEMES_LIST).find(DOT + THEME).last();
            }

            releaseThemeFocus();
            moveToTheme(next);
        } else if (key === keys.RIGHT) {
            e.preventDefault();

            nextGroup = current.closest(DOT + THEME_COL).next();
            if (nextGroup.length === 0) {
                return;
            }

            next = nextGroup.find(DOT + THEME).eq(current.index());

            if (next.length === 0) {
                next = nextGroup.find(DOT + THEME).last();
            }

            releaseThemeFocus();
            moveToTheme(next);
        } else if (key === keys.LEFT) {
            e.preventDefault();

            nextGroup = current.closest(DOT + THEME_COL).prev();
            if (nextGroup.length === 0) {
                return;
            }

            next = nextGroup.find(DOT + THEME).eq(current.index());

            if (next.length === 0) {
                next = nextGroup.find(DOT + THEME).last();
            }

            releaseThemeFocus();
            moveToTheme(next);
        }
    }

    function moveToTheme(next) {
        $(DOT + THEME_CHOOSER_BTN).attr(ARIA_ACTIVEDESCENDANT, ACTIVE_THEME);

        next.addClass(THEME_FOCUS);
        next.attr(ID, ACTIVE_THEME);
    }
    // End

    // Console
    function clearConsole() {
        kendoConsole.clear();
    }

    // TagHelper Switcher
    function toggleSwitcher(e) {
        var target = $(e.currentTarget);
        if (!target.hasClass(SWITCH_SELECTED_OP)) {
            var current = $(DOT + SWITCH_SELECTED_OP);

            current.removeClass(SWITCH_SELECTED_OP);
            current.attr("aria-pressed", "false");

            target.addClass(SWITCH_SELECTED_OP);
            target.attr("aria-pressed", "true");

            var container = $(HASH + SOURCE_PANE);
            var htmlHelper = container.find(DOT + TABSTRIP_ITEM).first();
            var tagHelper = container.find(DOT + TABSTRIP_ITEM + '[data-container="taghelper-cshtml"]');

            if (target.text() == "TagHelper") {
                tagHelper.show();
                tagHelper.click();
                htmlHelper.hide();

                if (typeof (Storage) !== "undefined") {
                    sessionStorage.isTagHelper = true;
                }
            }
            else {
                htmlHelper.show();
                htmlHelper.click();
                tagHelper.hide();

                if (typeof (Storage) !== "undefined") {
                    sessionStorage.removeItem('isTagHelper');
                }
            }
        }
    }
    function addCopyButton(element) {
        $(element).prepend('<span class="copy-code-btn k-icon k-input-icon k-i-copy" style="float:right; cursor: pointer;" title="Copy Code"></span>');
        $('.copy-code-btn').click(copyCode);
    };

    function copyCode() {
        const pre = $(this).parent();
        var copyBtn = $('.copy-code-btn');

        navigator.clipboard.writeText(pre.text()).then(function () {
            copyBtn.attr("title", "Copied!");
            copyBtn.removeClass("k-i-copy").addClass("k-i-check");

            setTimeout(function () {
                copyBtn.attr("title", "Copy Code");
                copyBtn.removeClass("k-i-check").addClass("k-i-copy");
            }, 2000);
        });
    };
}());