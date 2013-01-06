MonsterApp.TemplateManager = {
    getTemplateCache: function () {
        if (!MonsterApp.Utils.isPresent(this.cache))
            this.cache = $(MonsterApp.CssSelectors.TemplateCacheId);
        return this.cache;
    },
    getTemplateSelector: function (templateName) {
        return '#' + templateName;
    },
    getTemplate: function (templateName, templateCallback) {
        var templateId = this.getTemplateSelector(templateName);
        var template = this.getTemplateCache().find(templateId);
        
        if (template.length === 0) {
            //get it from server
            var templateText = "<div id='zz'>zzzzzzzz</div>";
            template = this.addTemplate(templateName, templateText);
        }
        if ($.isFunction(templateCallback))
            templateCallback(template.html());
    },
    addTemplate: function (templateName, templateText) {
        return  $("<div />").attr('id', templateName).html(templateText).appendTo(this.getTemplateSelector());
    }
};

MonsterApp.BaseController = function (context) {
    var self = this;
    if (context.MainViewRendered === false) {
        if (!MonsterApp.Utils.isPresent(context.MasterView)) {
            context.MasterView = new MonsterApp.Views.MasterView();
        }
        context.MasterView.render();
        context.MainViewRendered = true;
    }

    this.dispose = function() {
        if (MonsterApp.Utils.isPresent(self.View)) {
            self.View.remove();
            self.View.unbind();
            self.View = null;
        }
        if (MonsterApp.Utils.isPresent(self.Model)) {
            self.Model.unbind();
            self.Model = null;
        }
    };
};

MonsterApp.ProfileController = function (context) {
    MonsterApp.BaseController.call(this, context);

    this.RenderView = function () {
        if (!MonsterApp.Utils.isPresent(this.View))
            return;
        this.View.render();
    };

    this.Show = function () {
        this.Model = new MonsterApp.Models.StorageAccount();
        this.View = new MonsterApp.Views.ProfileView({ model: this.Model });
        this.Model.bind('change', this.RenderView);
        this.Model.fetch();
        return this;
    };
    _.bindAll(this);
};


MonsterApp.DefaultController = function (context) {
    MonsterApp.BaseController.call(this, context);
};


MonsterApp.Views.BaseView = Backbone.View.extend({
    getTemplate: function(templateCallback) {
        MonsterApp.TemplateManager.getTemplate(this.templateName, templateCallback);
    }
});


MonsterApp.Views.ProfileView = MonsterApp.Views.BaseView.extend({
    el: '#innerContent',
    templateName: MonsterApp.TemplateNames.ProfileTmpl,
    initialize: function () {
        _.bindAll(this);
    },
    render: function () {
        this.getTemplate(this.renderTemplate);
    },
    renderTemplate: function (template) {
        var compiledTemplate = _.template(template);
        this.$el.html(compiledTemplate({ profile: this.model }));
        var form = $('#profileForm');
        form.removeData('validator');
        form.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse(form);
        form.validate();
    }
});


MonsterApp.Models.StorageAccount = Backbone.Model.extend({
    fetch: function () {
        var self = this;
        MonsterApp.Ajax({
            url: this.url(),
            success: function(data) {
                self.set(self.parse(data));
            }
        });
    },
    urlRoot: 'account/profile'
});

MonsterApp.Views.MasterView = MonsterApp.Views.BaseView.extend({
    el: '#mainContent',
    templateName: MonsterApp.TemplateNames.MasterViewTmpl,
    initialize: function() {
        _.bindAll(this);
    },
    render: function () {
        this.getTemplate(this.renderTemplate);
    },
    renderTemplate: function (template) {
        var compiledTemplate = _.template(template);
        this.$el.html(compiledTemplate());
    }
});

MonsterApp.Router = Backbone.Router.extend({
    routes: {
        "profile": "profile",
        "*actions": "defaultRoute"
    },
    profile: function () {
        this.disposeCurrentController();
        MonsterApp.CurrentController = new MonsterApp.ProfileController(MonsterApp.Context).Show();
    },
    defaultRoute: function () {
        this.disposeCurrentController();
        MonsterApp.CurrentController = new MonsterApp.DefaultController(MonsterApp.Context);
    },
    disposeCurrentController: function () {
        if (MonsterApp.Utils.isPresent(MonsterApp.CurrentController)){
            MonsterApp.CurrentController.dispose();
        }
    }
});
