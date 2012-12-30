var App = {
    Models: {},
    Routers: {},
    Collections: {},
    Views: {}
};



var viewEvents = _.extend({}, Backbone.Events);
viewEvents.bind("storageaccount:selected", function (storageAccount) {
    alert("account selected: " + storageAccount.get('name'));
});

accSyncStab = function (method, model) {
    if (method === 'read') {
        setTimeout(function() {
            var m1 = new App.Models.StorageAccount({ name: "Yandex disk", id: 1 });
            var m2 = new App.Models.StorageAccount({ name: "Dropbox", id: 2 });
            model.reset([m1, m2]);
        }, 2000);
    }
};

App.Router = Backbone.Router.extend({
    routes: {
        "profile": "profile",
        "logoff":"logoFF",
        "storageaccount/:id": "storageAccount",
        "storageaccount/:id/*url": "storageAccount",
        "*actions": "defaultRoute"
    },
    storageAccount: function (id, url) {
        $('#route_result').text('storageaccount: ' + id + ' ' + url);
        window.accColllection.get(id).select();
    },
    profile: function () {
        $('#route_result').text('profile');
    },
    defaultRoute: function (actions) {
        $('#route_result').text('default: '+actions);
        //window.accCollection = new App.Collections.StorageAccountsCollection();
        //window.accCollection.fetch();
    }
});

App.Models.StorageAccount = Backbone.Model.extend({
    select: function () {
        this.set({ selected: true });
        //this.collection.selectStorageAccount(this);
        viewEvents.trigger("storageaccount:selected", this);
    }
});

App.Collections.StorageAccountsCollection = Backbone.Collection.extend({
    model: App.Models.StorageAccount,
    sync: accSyncStab,
    initialize: function () {
        this.storageAccountsView = new App.Views.StorageAccountsView({collection : this });
        this.bind("reset", this.storageAccountsView.renderList, this.storageAccountsView);
    },
    //selectStorageAccount: function (storageAccount) {
    //   //
    //}
});

App.Views.StorageAccountsView = Backbone.View.extend({
    el: "#storageaccounts",
    model: App.Collections.StorageAccountsCollection,
    events: {
        "click a#refreshStorageAccounts": "refreshAccounts",
        "click a.storageAccountLink": "selectAccount"
    },
    initialize: function () {
        this.renderWait();
    },
    renderWait: function() {
        this.$el.html("LOADING......");
    },
    renderList: function (collection) {
        var compiledTemplate = _.template($("#TStorageAccountsList").html());
        this.$el.html(compiledTemplate({ accounts: collection.models }));
    },
    selectAccount: function (e) {
        e.preventDefault();
        window.router.navigate("/storageaccount/" + $(e.currentTarget).data("id"), { trigger: true, replace: true });
    },
    refreshAccounts: function (e) {
        this.renderWait();
        e.preventDefault();
        this.collection.fetch();
    }
});

//AppView = Backbone.View.extend({
//    el: "#application",
//    initialize: function(){
//        this.accountsCollection = new App.Collections.StorageAccountsCollection();
//        this.accountsCollection.fetch();
//    }
//});


