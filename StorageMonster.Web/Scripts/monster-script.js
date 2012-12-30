function MonsterAjax(options,url) {
    if (!!!options)
        return;

    $.ajax({
        url: url,
        type: options.type,
        data: options.data,
        beforeSend: options.beforeSend,
        error: options.error,
        success: function (data) {
            options.success(data);
        },
        complete: options.complete
    });
}

var AccountsList = Backbone.Model.extend({
    initialize: function (url, ajaxCallback) {
        if (!$.isFunction(ajaxCallback))
            return;
        var uri = (!!url)?url.url:this.get('url');
        try {
            ajaxCallback(this.get('ajaxOptions'), uri);
        }catch(e){
            console.error(e);
        }
        
    },
    defaults: {
        url:'/',
        ajaxOptions : {
            type: 'POST',
            data: 'json',
            error: ajaxError(),
            success:ajaxSuccess (data),
            complete:ajaxComplete
        }
    },
    ajaxError:function() {
        
    } 
      );
    }
});

