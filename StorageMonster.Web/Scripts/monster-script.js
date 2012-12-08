var AccountsList = Backbone.Model.extend({
    initialize: function (url) {
        var uri = (!!url)?url.url:this.get('url');
        try {
            this.fetchData(uri);
        }catch(e){
            console.error(e);
        }
        console.log(this.get('accounts'));
    },
    defaults: {
        url: '/StorageAccounts/list/default' 
    },
    fetchData: function (uri) {
        var accounts = this;
        $.ajax({
            url: uri,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (!accounts.get('accounts')) {
                    accounts.set({ 'accounts': data });
                }
            },
            error: function () {
                throw "error fetch accounts";
            }
      });
    }
});

