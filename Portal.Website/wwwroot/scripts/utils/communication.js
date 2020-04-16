var user = null;

var refreshToken = function () {
        if (user == null) {
            getUser();
        }
        var expireDate = Date.parse(user.expires_on);
        if (expireDate < new Date()) {
            $.ajax({
                url: "/.auth/refresh",
                success: function (result) {
                    user = result[0];
                },
                async: false
            });
        }
    };

   var getAccessToken = function() {
        refreshToken();
        return user.access_token;
    };

    var send = function (type, url, successFunc) {

            $.ajax({
                type: type,
                url: url,
                headers: {
                    'Authorization': 'Bearer ' + getAccessToken()
                },
            success: successFunc,
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
                console.log(xhr.responseText);
            }
        });
    };

    var get = function(url, successFunc) {
        send('GET', url, successFunc);
    };

    var post = function(url, data, successFunc) {

        return $.ajax({
            type: 'POST',
            contentType: 'application/json;',
            dataType: 'json',
            url: url,
            headers: {
                'Authorization': 'Bearer ' + getAccessToken()  
            },
            data: data,
            complete: successFunc
        });
    };

    var getUser = function() {
        if (user == null) {
            $.ajax({
                url: "/.auth/me",
                success: function(result) {
                    user = result[0];
                },
                async: false
            });
        }
    };

export { get, getAccessToken, post };

getUser();
