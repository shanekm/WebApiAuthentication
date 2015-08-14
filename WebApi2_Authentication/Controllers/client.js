$(document).ready(function() {
    
    // delete product
    // success function will read HTTP 204 Ok() response
    // DO NOT use this:
    //      $.ajax("/api/products/"), {
    //      data: { id: data.ProductId } as Api will read this as Get method and get list of products
    deleteProduct = function(data) {
        $.ajax("/api/products/" + data.ProductId), {
            type: "DELETE",
            success: function() {
                products.remove(data);
            }
        }
    }

    // get list of products - returns data
    getProducts = function() {
        $.ajax("/api/products/"), {
            success:function(data) {
                products.removeAll();
                for (var i = 0; i < data.length; i++) {
                    products.push(data[i]);
                }
            }
        }
    }

    ko.applyBindings();
})