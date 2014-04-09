(function ($) {
    Device = function (x) {
        this.hidden = false;
        $.extend(this, x);
    };

    Device.prototype = {
        update: function (x) {
            var changed = this.lat != x.lat || this.lng != x.lng;
            $.extend(this, x);

            return changed;
        }
    };
})(jQuery);
