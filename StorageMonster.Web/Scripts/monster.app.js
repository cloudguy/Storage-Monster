var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MonsterApp;
(function (MonsterApp) {
    MonsterApp.ApplicationContext = {
    };
    var TemplateManager = (function () {
        function TemplateManager() { }
        TemplateManager.prototype.getTemplateCache = function () {
            if(!MonsterApp.Utils.isObjectPresent(this.cacheContainer)) {
                this.cacheContainer = $(MonsterApp.CssSelectors.TemplateCacheId);
            }
            return this.cacheContainer;
        };
        TemplateManager.prototype.getTemplateSelector = function (templateName) {
            return '#' + templateName;
        };
        TemplateManager.prototype.saveTemplate = function (templateName, templateText) {
            return $('<script type="text/template" />').attr('id', templateName).html(templateText).appendTo(this.getTemplateSelector(templateName));
        };
        TemplateManager.prototype.getTemplate = function (templateName, templateCallback) {
            var templateId = this.getTemplateSelector(templateName);
            var template = this.getTemplateCache().find(templateId);
            if(template.length === 0) {
                var templateText = "<div id='zz'>zzzzzzzz</div>";
                template = this.saveTemplate(templateName, templateText);
            }
            if($.isFunction(templateCallback)) {
                templateCallback(template.html());
            }
        };
        return TemplateManager;
    })();    
    (function (Controllers) {
        var BaseController = (function () {
            function BaseController(context) {
                this._context = context;
                if(!MonsterApp.Utils.isObjectPresent(this._context.MasterView)) {
                    this._context.MasterView = new MonsterApp.Views.MasterView();
                }
                this._context.MasterView.render();
                this._context.MainViewRendered = true;
            }
            BaseController.prototype.disposeMasterView = function () {
                if(!MonsterApp.Utils.isObjectPresent(this._context.MasterView)) {
                    return;
                }
                this._context.MasterView.remove();
                this._context.MasterView.unbind();
                this._context.MasterView = null;
                this._context.MainViewRendered = false;
            };
            BaseController.prototype.renderView = function () {
                if(!MonsterApp.Utils.isObjectPresent(this._view)) {
                    return;
                }
                this._view.render();
            };
            BaseController.prototype.dispose = function () {
                if(MonsterApp.Utils.isObjectPresent(this._view)) {
                    this._view.remove();
                    this._view.unbind();
                    this._view = null;
                }
                if(MonsterApp.Utils.isObjectPresent(this._model)) {
                    this._model.unbind();
                    this._model = null;
                }
            };
            return BaseController;
        })();
        Controllers.BaseController = BaseController;        
        var DefaultController = (function (_super) {
            __extends(DefaultController, _super);
            function DefaultController(context) {
                        _super.call(this, context);
            }
            return DefaultController;
        })(BaseController);
        Controllers.DefaultController = DefaultController;        
        var ProfileController = (function (_super) {
            __extends(ProfileController, _super);
            function ProfileController(context) {
                        _super.call(this, context);
            }
            ProfileController.prototype.show = function () {
                this._model = new MonsterApp.Models.ProfileModel();
                this._view = new MonsterApp.Views.ProfileView({
                    model: this._model
                });
                this._model.bind('change', _super.prototype.renderView, this);
                this._model.fetch();
                return this;
            };
            return ProfileController;
        })(BaseController);
        Controllers.ProfileController = ProfileController;        
    })(MonsterApp.Controllers || (MonsterApp.Controllers = {}));
    var Controllers = MonsterApp.Controllers;
    (function (Models) {
        var ProfileModel = (function (_super) {
            __extends(ProfileModel, _super);
            function ProfileModel() {
                _super.apply(this, arguments);

                this.urlRoot = 'account/profile';
            }
            ProfileModel.prototype.fetch = function () {
                var self = this;
                MonsterApp.Ajax({
                    url: this.url(),
                    success: function (data) {
                        self.set(self.parse(data));
                    }
                });
            };
            return ProfileModel;
        })(Backbone.Model);
        Models.ProfileModel = ProfileModel;        
        var StorageAccountModel = (function (_super) {
            __extends(StorageAccountModel, _super);
            function StorageAccountModel() {
                _super.apply(this, arguments);

            }
            StorageAccountModel.prototype.fetch = function () {
                var self = this;
                MonsterApp.Ajax({
                    url: this.url(),
                    success: function (data) {
                        self.set(self.parse(data));
                    }
                });
            };
            return StorageAccountModel;
        })(Backbone.Model);
        Models.StorageAccountModel = StorageAccountModel;        
        var StorageAccountsList = (function (_super) {
            __extends(StorageAccountsList, _super);
            function StorageAccountsList() {
                _super.apply(this, arguments);

                this.model = StorageAccountModel;
            }
            return StorageAccountsList;
        })(Backbone.Collection);
        Models.StorageAccountsList = StorageAccountsList;        
    })(MonsterApp.Models || (MonsterApp.Models = {}));
    var Models = MonsterApp.Models;
    (function (Views) {
        var BaseView = (function (_super) {
            __extends(BaseView, _super);
            function BaseView(options) {
                        _super.call(this, options);
            }
            BaseView._templateManager = new TemplateManager();
            BaseView.prototype.getTemplate = function (templateCallback) {
                BaseView._templateManager.getTemplate(this._templateName, templateCallback);
            };
            return BaseView;
        })(Backbone.View);
        Views.BaseView = BaseView;        
        var MasterView = (function (_super) {
            __extends(MasterView, _super);
            function MasterView(options) {
                this._templateName = MonsterApp.TemplateNames.MasterViewTmpl;
                this.el = document.getElementById('mainContent');
                        _super.call(this, options);
                _.bindAll(this);
            }
            MasterView.prototype.render = function () {
                this.getTemplate(this.renderTemplate);
            };
            MasterView.prototype.renderTemplate = function (template) {
                var compiledTemplate = _.template(template);
                this.$el.html(compiledTemplate());
            };
            return MasterView;
        })(BaseView);
        Views.MasterView = MasterView;        
        var ProfileView = (function (_super) {
            __extends(ProfileView, _super);
            function ProfileView(options) {
                this._templateName = MonsterApp.TemplateNames.ProfileTmpl;
                this.el = document.getElementById('innerContent');
                        _super.call(this, options);
                _.bindAll(this);
            }
            ProfileView.prototype.render = function () {
                this.getTemplate(this.renderTemplate);
            };
            ProfileView.prototype.renderTemplate = function (template) {
                var compiledTemplate = _.template(template);
                this.$el.html(compiledTemplate({
                    profile: this.model
                }));
                var form = $('#profileForm');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
                form.validate();
            };
            return ProfileView;
        })(BaseView);
        Views.ProfileView = ProfileView;        
    })(MonsterApp.Views || (MonsterApp.Views = {}));
    var Views = MonsterApp.Views;
    var ControllerDispatcher = (function () {
        function ControllerDispatcher(context) {
            this._context = context;
        }
        ControllerDispatcher.prototype.createController = function (controllerCreator) {
            if(MonsterApp.Utils.isObjectPresent(this._context.CurrentController)) {
                this._context.CurrentController.dispose();
                this._context.CurrentController = null;
            }
            this._context.CurrentController = controllerCreator();
            return this._context.CurrentController;
        };
        return ControllerDispatcher;
    })();    
    var Router = (function (_super) {
        __extends(Router, _super);
        function Router(options) {
            this.routes = {
                "profile": "profile",
                "*actions": "defaultRoute"
            };
                _super.call(this, options);
            this._context = MonsterApp.ApplicationContext;
            this._controllerDispatcher = new ControllerDispatcher(this._context);
        }
        Router.prototype.profile = function () {
            var context = this._context;
            this._controllerDispatcher.createController(function () {
                return new Controllers.ProfileController(context);
            }).show();
        };
        Router.prototype.defaultRoute = function () {
            var context = this._context;
            this._controllerDispatcher.createController(function () {
                return new Controllers.DefaultController(context);
            });
        };
        return Router;
    })(Backbone.Router);
    MonsterApp.Router = Router;    
})(MonsterApp || (MonsterApp = {}));
