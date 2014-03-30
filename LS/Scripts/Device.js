(function ($) {
    Device = function (d, map, infoWindow) {
        this.map = map;
        this.infoWindow = infoWindow;
        this.hidden = false;

        $.extend(this, d);

//        this.marker = new google.maps.Marker({
//            map: this.map,
//            icon: '/thumbnail/load/' + this.uid,
//            position: new google.maps.LatLng(this.lat, this.lng),
//            title: this.uname
//        });
//        google.maps.event.addListener(this.marker, 'click', function (d) {
//            return function () {
//                d.clickMarker();
//            };
//        } (this));

        this._createListElement();
        this.listElement.click(function (d) {
            return function () {
                d.clickPortrait();
            };
        } (this));
        this.toggleButton.click(function (d) {
            return function (e) {
                e.stopPropagation();
                d.toggle();
            };
        } (this));
    };

    Device.prototype = {
        clickMarker: function () {
            this.focus();
            this.showInfo();
        },
        clickPortrait: function () {
            if (!this.hidden) {
                this.focus(true);
                this.showInfo();
            }
        },
        update: function (d) {
            if (this.lat != d.lat || this.lng != d.lng) {
                this.marker.setPosition(new google.maps.LatLng(d.lat, d.lng));
            }
            $.extend(this, d);
        },
        focus: function (zoom) {
            if (Device.top) {
                Device.top.marker.setZIndex(Device.topIndex);
            }
            Device.top = this;
            Device.topIndex = this.marker.getZIndex();
            this.marker.setZIndex(99999);

            this.map.panTo(this.marker.getPosition());
            if (zoom) {
                this.map.setZoom(15);
            }
        },
        showInfo: function () {
            this.infoWindow.setContent(this._makeInfoContent());
            this.infoWindow.open(this.map, this.marker);
        },
        _makeInfoContent: function () {
            var lat = Math.round(this.lat * 10000) / 10000;
            var lng = Math.round(this.lng * 10000) / 10000;
            var content = '<b>' + this.uname + '</b> is at: ' + lat + ', ' + lng;
            var a = '<a class="mapsLink" target="_blank" href="https://www.google.com/maps?ll=' + this.lat + ',' + this.lng + '&t=m&z=' + this.map.getZoom() + '">';
            var img = '<img src="/images/map_icon.jpg" alt="Link to Google Maps" title="Make a link to Google Maps" />';
            return content + a + img + '</a>';
        },
        toggle: function () {
            this.hidden = !this.hidden;
            if (!this.hidden) {
                this.marker.setMap(this.map);
                this.listElement.removeClass('disabled');
                this.clickPortrait();
            } else {
                this.marker.setMap(null);
                this.listElement.addClass('disabled');
            }
        },
        _createListElement: function () {
            var title = $(document.createElement('div')).addClass('title').append(this.userName);
            this.toggleButton = $(document.createElement('div')).addClass('toggle').attr('title', 'Toggle visibility'); //.on('mouseover mouseout', function (e) { e.stopPropagation(); });
            this.listElement = $(document.createElement('div')).addClass('portrait').attr('style', 'background-image: url(/thumbnail/load/' + this.userId + ')')
                .append(title).append(this.toggleButton)
                .data({
                    placement: 'top',
                    container: 'body',
                    content: this.userName
                }).on('mouseenter', function () { $(this).popover('show'); }).on('mouseleave', function () { $(this).popover('hide'); });
            return this.listElement;
        }
    };
})(jQuery);
