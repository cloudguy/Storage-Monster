declare var MonsterApp;
declare var $;
declare var _;

declare module Backbone {
   export class Model {
       constructor (attr? , opts? );
       get(name: string): any;
       set(name: string, val: any): void;
       set(obj: any): void;
       save(attr? , opts? ): void;
       url(): string;
       parse(data: any): any;
       destroy(): void;
       bind(ev: string, f: Function, ctx?: any): void;
       toJSON(): any;
   }
   export class Collection {
       constructor (models? , opts? );
       bind(ev: string, f: Function, ctx?: any): void;
       collection: Model;
       length: number;
       create(attrs, opts? ): Collection;
       each(f: (elem: any) => void ): void;
       fetch(opts?: any): void;
       last(): any;
       last(n: number): any[];
       filter(f: (elem: any) => any): Collection;
       without(...values: any[]): Collection;
   }
   export class View {
       constructor (options? );
       $(selector: string): any;
       el: HTMLElement;
       $el: any;
       model: Model;
       remove(): void;
       delegateEvents: any;
       make(tagName: string, attrs? , opts? ): View;
       setElement(element: HTMLElement, delegate?: bool): void;
       tagName: string;
       events: any;

       static extend: any;
   }

   export class Router {
       static extend: any;
       constructor (options? );
       routes: any;
   }
}


interface IControllerDispatcher {       
    createController(controllerCreator): any;
}

module MonsterApp {

    export var ApplicationContext = {};

    class TemplateManager {
        cacheContainer: any;
        getTemplateCache() {
            if (!MonsterApp.Utils.isObjectPresent(this.cacheContainer))
                this.cacheContainer = $(MonsterApp.CssSelectors.TemplateCacheId);
            return this.cacheContainer;
        }

        getTemplateSelector(templateName: string) {
            return '#' + templateName;
        }

        saveTemplate(templateName: string, templateText: string) {
            return $('<script type="text/template" />')
                .attr('id', templateName)
                .html(templateText)
                .appendTo(this.getTemplateSelector(templateName));
        }

        getTemplate(templateName: string, templateCallback: (html: any) => any) {
            var templateId = this.getTemplateSelector(templateName);
            var template = this.getTemplateCache().find(templateId);

            if (template.length === 0) {
                //get it from server
                var templateText = "<div id='zz'>zzzzzzzz</div>";
                template = this.saveTemplate(templateName, templateText);
            }
            if ($.isFunction(templateCallback))
                templateCallback(template.html());
        }
    }


    export module Controllers {
        export class BaseController {
            _context: any;
            _view: any;
            _model: any;
            constructor(context) {
                this._context = context;
                if (!MonsterApp.Utils.isObjectPresent(this._context.MasterView)) {
                    this._context.MasterView = new MonsterApp.Views.MasterView();
                }
                this._context.MasterView.render();
                this._context.MainViewRendered = true;                
            }

            disposeMasterView() {
                if (!MonsterApp.Utils.isObjectPresent(this._context.MasterView))
                    return;
                this._context.MasterView.remove();
                this._context.MasterView.unbind();
                this._context.MasterView = null;
                this._context.MainViewRendered = false;
            }

            renderView() {               
                if (!MonsterApp.Utils.isObjectPresent(this._view))
                    return;
                this._view.render();
            }

            dispose() {
                if (MonsterApp.Utils.isObjectPresent(this._view)) {
                    this._view.remove();
                    this._view.unbind();
                    this._view = null;
                }
                if (MonsterApp.Utils.isObjectPresent(this._model)) {
                    this._model.unbind();
                    this._model = null;
                }
            }
        }

        export class DefaultController extends BaseController {
            constructor(context) { super(context); }
        }



        export class ProfileController extends BaseController {
            constructor(context) { super(context); }
            show() {
                this._model = new MonsterApp.Models.ProfileModel();
                this._view = new MonsterApp.Views.ProfileView({ model: this._model });
                this._model.bind('change', super.renderView, this);
                this._model.fetch();
                return this;
            };
        }
    }


    export module Models {
        export class ProfileModel extends Backbone.Model {
            urlRoot = 'account/profile';
            fetch() {
                var self = this;
                MonsterApp.Ajax({
                    url: this.url(),
                    success: function (data) {
                        self.set(self.parse(data));
                    }
                });
            }
        }
		
		export class StorageAccountModel extends Backbone.Model {
            //urlRoot = 'StorageAccounts/create';
            fetch() {
                var self = this;
                MonsterApp.Ajax({
                    url: this.url(),
                    success: function (data) {
                        self.set(self.parse(data));
                    }
                });
            }
        }




		export class StorageAccountsList extends Backbone.Collection {
		    model = StorageAccountModel;
		}

    }

    export module Views {
        export class BaseView extends Backbone.View {
            _templateName: string;
            static _templateManager: any = new TemplateManager();
            constructor(options?) { 
                super(options);                 
            }
            getTemplate(templateCallback) {
                BaseView._templateManager.getTemplate(this._templateName, templateCallback);
            }
        }

        export class MasterView extends BaseView {
            constructor(options?) {
                this._templateName = MonsterApp.TemplateNames.MasterViewTmpl;
                this.el = document.getElementById('mainContent');
                super(options);
                _.bindAll(this);
            }
            render() {
                this.getTemplate(this.renderTemplate);
            }
            renderTemplate(template: any) {
                var compiledTemplate = _.template(template);
                this.$el.html(compiledTemplate());
            }
        }

        export class ProfileView extends BaseView {
            constructor(options?) {
                this._templateName = MonsterApp.TemplateNames.ProfileTmpl;
                this.el = document.getElementById('innerContent');
                super(options);
                _.bindAll(this);
            }
            render() {
                this.getTemplate(this.renderTemplate);
            }

            renderTemplate(template: any) {
                var compiledTemplate = _.template(template);
                this.$el.html(compiledTemplate({ profile: this.model }));
                var form = $('#profileForm');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
                form.validate();
            }
        }
    }

    class ControllerDispatcher implements IControllerDispatcher {
        _context: any;
        constructor(context: any) { this._context = context; }

        createController(controllerCreator) {
            if (MonsterApp.Utils.isObjectPresent(this._context.CurrentController)) {
                this._context.CurrentController.dispose();
                this._context.CurrentController = null;
            }
            this._context.CurrentController = controllerCreator();
            return this._context.CurrentController;
        }
    }

    export class Router extends Backbone.Router {
        _controllerDispatcher: IControllerDispatcher;
        _context: any;
        constructor(options?) {
            this.routes = {
                "profile": "profile",
                "*actions": "defaultRoute"
            }
            super(options);
            this._context = ApplicationContext;
            this._controllerDispatcher = new ControllerDispatcher(this._context);            
        }


        profile() {
            var context = this._context;
            this._controllerDispatcher.createController(() =>
            {
                return new Controllers.ProfileController(context)
            }).show();

        }
        defaultRoute() {
            var context = this._context;
            this._controllerDispatcher.createController(() =>
            {
                return new Controllers.DefaultController(context)
            });
        }
    }
}
