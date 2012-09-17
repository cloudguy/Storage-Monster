function WidgetFactory() {
    var widgetFactory = {};

    widgetFactory.CreateWidgets = function (selector) {
        var widgetsArray = new Array();
        selector.each(function() {
            switch($(this).attr('data-widget'))
            {
                case 'menu': widgetsArray.push(new MenuWidget($(this)));
                    break;
                case 'folderwidget': widgetsArray.push(new FolderWidget($(this)));
                    break;
                case 'profilewidget': widgetsArray.push(new ProfileWidget($(this)));
                    break;
            }
        });
        return widgetsArray;
    };

    return widgetFactory;
}

function Widget() {
    var widget = {};    
    widget.Submit = function () { return widget; }
    widget.Reload = function () { return widget; }
    widget.Destroy = function () { return widget; }
    widget.SetWorkspace = function (workspaceWidget) {
        widget.Workspace = workspaceWidget;
        return widget;
    }
    return widget;
}


function ControlWidget(selector) {
    var controlWidget = new Widget();
    controlWidget.WidgetSelector = selector;
    controlWidget.Init = function (selector) { return controlWidget; }
    controlWidget.WaitBinder = {
        onNavigateHandler: function () {
            controlWidget.WidgetSelector.removeClass("disablable-enabled");
            controlWidget.WidgetSelector.addClass("disablable");
            controlWidget.WidgetSelector.addClass("disablable-disabled");
        },
        afterNavigateHandler: function () {
            controlWidget.WidgetSelector.removeClass("disablable");
            controlWidget.WidgetSelector.removeClass("disablable-disabled");
            controlWidget.WidgetSelector.addClass("disablable-enabled");  
        },
        Bind: function (workspace) {
            $(workspace).bind("onnavigate", this.onNavigateHandler);
            $(workspace).bind("afternavigate", this.afterNavigateHandler);
        },
        UnBind: function (workspace) {
            $(workspace).unbind("onnavigate", this.onNavigateHandler);
            $(workspace).unbind("afternavigate", this.afterNavigateHandler);  
        }
    };
    return controlWidget;
}

function Workspace(workspaceSelector) {
    var workspace = new ControlWidget(workspaceSelector);
    var isNavigating = false;
    workspace.WidgetCollection = new Array();
    workspace.WidgetFactory = new WidgetFactory();
    workspace.AddWidget = function (widget) {
        widget.SetWorkspace(workspace);
        workspace.WidgetCollection.push(widget);
        return workspace;
    }

    workspace.Reload = function () {
        workspace.WidgetCollection.forEach(function (w) { w.Reload() });
        return workspace;
    }

    workspace.RemoveWidget = function (widget) {
        var newWidgetCollection = new Array();
        for (w in WidgetCollection) {
            if (w != widget)
                newWidgetCollection.push(w);
        }
        workspace.WidgetCollection = newWidgetCollection;
        return workspace;
    }

    history.replaceState({ ajaxed: true, url: window.location.href }, "Storage Monster");
    //console.log("replaced with : " + window.location.href);

    workspace.Init = function (workspaceSelector) {
        var pushStateSupported = window.history
                    && window.history.pushState && window.history.replaceState &&
        // pushState isn't reliable on iOS until 5.
                    !navigator.userAgent.match(/((iPod|iPhone|iPad).+\bOS\s+[1-4]|WebApps\/.+CFNetwork)/);
        if (!pushStateSupported)
            return workspace;

        

        window.onpopstate = function (event) {
            //console.log("onpop location: " + document.location);
            //console.log("onpop state: " + JSON.stringify(event.state));
            
            if (event.state)
                window.document.location = event.state.url;            
        };

        workspace.WidgetSelector = workspaceSelector;

        var widgetSelectors = workspace.WidgetSelector.find('[data-widget]');

        workspace.WidgetCollection.every(function (widget) {
            widget.Destroy();
            return true;
        });

        workspace.WidgetCollection.length = 0;

        workspace.WidgetFactory.CreateWidgets(widgetSelectors)
                .every(function (widget) {
                    workspace.AddWidget(widget);
                    return true;
                });
        workspace.Destroy = function () {
            workspace.WidgetCollection
            .every(function (widget) {
                widget.Destroy();
                return true;
            });
        }
        return workspace;
    }

    workspace.AjaxNavigate = function (options) {
        if (typeof options == 'undefined')
            return workspace;
        if (isNavigating)
            return workspace;
        $.ajax({
            url: options.url,
            type: options.type,
            data: options.data,
            beforeSend: function (xhr) {
                isNavigating = true;
                $(workspace).trigger("onnavigate");
                if (typeof options.beforeSend == 'function')
                    options.beforeSend(xhr);
            },
            error: function (xhr) {
                $(workspace).trigger("error");
                if (typeof options.error == 'function')
                    options.error(xhr);
            },
            success: function (data) {
                if (data.Unauthorized) {
                    $('#logOnControl').remove();
                    $('#ajaxLogOnContainer').monsterhtml(data.LogOnPage);
                    $('#ajaxLogOnContainer').modal(
                        { backdrop: "static", keyboard: false }
                    );
                    return;
                }

                if (data.Error) {
                    $(workspace).trigger("error");
                    if (typeof options.error == 'function')
                        options.error();
                    return;
                }

                history.pushState({ ajaxed: true, url: options.url }, "Storage Monster", options.url);
                //console.log("pushed : " + options.url);
                workspace.ProccessAjaxNavigate(data);

                if (options.scrollToTop)
                    $('html, body').animate({ scrollTop: 0 }, 'slow');

                if (typeof options.success == 'function')
                    options.success(data);
            },
            complete: function (xhr) {
                isNavigating = false;
                $(workspace).trigger("afternavigate");
                if (typeof options.complete == 'function')
                    options.complete(xhr);
            }
        });
    }

    workspace.ProccessAjaxNavigate = function (data) {
        if (typeof data == 'undefined')
            return workspace;
        if (data.MainPanelHtml) {
            $('#mainpanel').empty().monsterhtml(data.MainPanelHtml);            
        }
        workspace.Init(workspace.WidgetSelector);
    }

    return workspace.Init(workspace.WidgetSelector);   
}

function MenuWidget(menuSelector) {
    var menuWidget = new ControlWidget(menuSelector);
    menuWidget.Init = function (menuSelector) {
        menuWidget.Destroy();
        menuWidget.WidgetSelector = menuSelector;
        var links = menuSelector.find('a.ajax_menu_link');
        links.on('click', function () {
            var url = $(this).attr('href');
            var caller = $(this);
            menuWidget.Workspace.AjaxNavigate({
                url: url,
                type: 'GET',               
                success: function (data) {                   
                    menuSelector.find('li').removeClass('active');
                    caller.closest('li').addClass('active');                    
                }
            });
            return false;
        });
        menuWidget.Destroy = function () {
            links.off('click');
        }
        return menuWidget;
    }
    
    return menuWidget.Init(menuWidget.WidgetSelector);
}

function FolderWidget(folderSelector) {
    var folderWidget = new ControlWidget(folderSelector);

    folderWidget.SetWorkspace = function (workspaceWidget) {
        folderWidget.Workspace = workspaceWidget;
        folderWidget.WaitBinder.Bind(folderWidget.Workspace);
    };
    folderWidget.Init = function (folderSelector) {
        folderWidget.Destroy();
        folderWidget.WidgetSelector = folderSelector;

        var links = folderWidget.WidgetSelector.find('a.ajax_folder_link');
        links.on('click', function () {
            var url = $(this).attr('href');
            var caller = $(this);
            folderWidget.Workspace.AjaxNavigate({
                url: url,
                type: 'GET'               
            });           
            return false;
        });

        folderWidget.Destroy = function () {
            folderWidget.WaitBinder.UnBind(folderWidget.Workspace);  
            links.off('click');
        }
        return folderWidget;
    }
    return folderWidget.Init(folderWidget.WidgetSelector);
}

function ProfileWidget(profileSelector) {
    var profileWidget = new ControlWidget(profileSelector);

    profileWidget.SetWorkspace = function (workspaceWidget) {
        profileWidget.Workspace = workspaceWidget;
        profileWidget.WaitBinder.Bind(profileWidget.Workspace);
    };
    profileWidget.Init = function (profileSelector) {
        profileWidget.Destroy();
        profileWidget.WidgetSelector = profileSelector;

        var forms = profileWidget.WidgetSelector.find('form');
        forms.on('submit', function () {
            var url = $(this).attr('action');
            var caller = $(this);
            profileWidget.Workspace.AjaxNavigate({
                url: url,
                type: 'POST',
                data: $(this).serialize(),
                scrollToTop: true
            });
            return false;
        });

        profileWidget.Destroy = function () {
            profileWidget.WaitBinder.UnBind(profileWidget.Workspace);
            forms.off('submit');
        }
        return profileWidget;
    }
    return profileWidget.Init(profileWidget.WidgetSelector);
}