
var mode = '';
var displayInitineraryMode = 'directions'; // directions | default
var locations = [];
var map;
var markers = [];

function activeBarItem(id, active, activeBar) {
    var bar = $('.bar[data-id="' + id + '"]');
    if (bar.hasClass('open'))
        return false;

    var iconDefault = "http://mt.google.com/vt/icon?color=ff004C13&name=icons/spotlight/spotlight-waypoint-b.png";
    var iconActive = "http://mt.googleapis.com/vt/icon/name=icons/spotlight/spotlight-poi.png";

    var marker = null;

    for (var i = 0; i < markers.length; i++) {
        if (markers[i].barId == id) {
            marker = markers[i];
        }
    }

    if (marker) {
        if (active) {
            marker.setIcon(iconActive);
            if (activeBar) {
                $('#bar-list').scrollTo(bar, 400, {
                    offset: -40, onAfter: function () {
                        requestAnimationFrame(function () {
                            $('.bar').removeClass('active');
                            bar.addClass('active');
                        });
                    }
                });
            } else {
                map.setZoom(17);
                map.panTo(marker.position);
            }
        }
        else {
            marker.setIcon(iconDefault);
            bar.removeClass('active');
        }
    }
}

function initMap() {
    // https://snazzymaps.com/style/132/light-gray
    var mapStyle = [{ "featureType": "water", "elementType": "geometry.fill", "stylers": [{ "color": "#d3d3d3" }] }, { "featureType": "transit", "stylers": [{ "color": "#808080" }, { "visibility": "off" }] }, { "featureType": "road.highway", "elementType": "geometry.stroke", "stylers": [{ "visibility": "on" }, { "color": "#b3b3b3" }] }, { "featureType": "road.highway", "elementType": "geometry.fill", "stylers": [{ "color": "#ffffff" }] }, { "featureType": "road.local", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "color": "#ffffff" }, { "weight": 1.8 }] }, { "featureType": "road.local", "elementType": "geometry.stroke", "stylers": [{ "color": "#d7d7d7" }] }, { "featureType": "poi", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "color": "#ebebeb" }] }, { "featureType": "administrative", "elementType": "geometry", "stylers": [{ "color": "#a7a7a7" }] }, { "featureType": "road.arterial", "elementType": "geometry.fill", "stylers": [{ "color": "#ffffff" }] }, { "featureType": "road.arterial", "elementType": "geometry.fill", "stylers": [{ "color": "#ffffff" }] }, { "featureType": "landscape", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "color": "#efefef" }] }, { "featureType": "road", "elementType": "labels.text.fill", "stylers": [{ "color": "#696969" }] }, { "featureType": "administrative", "elementType": "labels.text.fill", "stylers": [{ "visibility": "on" }, { "color": "#737373" }] }, { "featureType": "poi", "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, { "featureType": "poi", "elementType": "labels", "stylers": [{ "visibility": "off" }] }, { "featureType": "road.arterial", "elementType": "geometry.stroke", "stylers": [{ "color": "#d6d6d6" }] }, { "featureType": "road", "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, {}, { "featureType": "poi", "elementType": "geometry.fill", "stylers": [{ "color": "#dadada" }] }];
    var mapOptions = {
        zoom: 3,
        styles: mapStyle,
        disableDefaultUI: true
    };
    map = new google.maps.Map(document.getElementById('bar-map'), mapOptions);
}
function initializeMap() {
    //create empty LatLngBounds object
    var bounds = new google.maps.LatLngBounds();
    var infowindow = new google.maps.InfoWindow();

    for (i = 0; i < locations.length; i++) {
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(locations[i].Latitude, locations[i].Longitude),
            map: map,
            icon: 'http://mt.google.com/vt/icon?color=ff004C13&name=icons/spotlight/spotlight-waypoint-b.png'
        });
        marker.barId = locations[i].Id;
        markers.push(marker);

        //extend the bounds to include each marker's position
        bounds.extend(marker.position);

        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                infowindow.setContent(locations[i].Name);
                infowindow.open(map, marker);
                openBarDetail(locations[i].Id);
            }
        })(marker, i));

        google.maps.event.addListener(marker, 'mouseover', (function (marker, i) {
            return function () {
                activeBarItem(marker.barId, true, true);
            }
        })(marker, i));
        google.maps.event.addListener(marker, 'mouseout', (function (marker, i) {
            return function () {
                activeBarItem(marker.barId, false, true);
            }
        })(marker, i));
    }

    for (i = 0; i < positions.length; i++) {
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(positions[i].Latitude, positions[i].Longitude),
            map: map,
            icon: 'http://mt.google.com/vt/icon?color=ff004C13&name=icons/spotlight/spotlight-waypoint-blue.png'
        });
        //markers.push(marker);
        bounds.extend(marker.position);
    }

    // need api key
    var markerCluster = new MarkerClusterer(map, markers);

    //now fit the map to the newly inclusive bounds
    map.fitBounds(bounds);

    if (locations.length > 0) {
        initBars();
        $('#maplistTrigger').show();
        $('#maplistTrigger').click(function () {
            $('.search-content').toggleClass('map');
            $('.search-content').toggleClass('list');

            if ($('.search-content').hasClass('list')) {
                $('#maplistTrigger .material-icons').text('map');
            } else {
                $('#maplistTrigger .material-icons').text('border_all');
            }
        });
    }
    setLoader(false);
}

function initBars() {
    var html = '';
    for (var i = 0; i < locations.length; i++) {
        html += '<div data-id="' + locations[i].Id + '" class="bar mdl-card mdl-shadow--2dp mdl-cell mdl-cell--6-col mdl-cell--12-col-tablet mdl-cell--12-col-phone">';
        html += '   <div class="mdl-card__title" style="background: url(\'' + locations[i].Picture + '\') center / cover;">';
        html += '       <h2 class="mdl-card__title-text">' + locations[i].Name + '</h2>';
        html += '   </div>';
        html += '</div>';
    }
    $('#bar-list').html(html);
    $('.bar').click(function () {
        var bar = $(this);
        openBarDetail($(this).data('id'));
    });
    $('.bar').mouseover(function () {
        activeBarItem($(this).data('id'), true, false);
    });
    $('.bar').mouseout(function () {
        activeBarItem($(this).data('id'), false, false);
    });
}

function openBarDetail(id) {
    var bar = null;
    for (var i = 0; i < locations.length; i++) {
        if (locations[i].Id == id) {
            bar = locations[i];
            break;
        }
    }

    $('#bar-detail-title').attr('style', 'background: url(\'' + bar.Picture + '\') center / cover;');
    $('#bar-detail-name').html(bar.Name);
    $('#bar-detail-openingstatus-icon').html((bar.IsOpenNow ? "schedule" : "watch_later"));
    $('#bar-detail-openingstatus').html((bar.IsOpenNow ? "Ouvert" : "Ferm&eacute;") + ' en ce moment');
    $('#bar-detail-address-link').attr('href', 'http://maps.google.com/maps?daddr=' + bar.Latitude + ',' + bar.Longitude);
    $('#bar-detail-address').html(bar.Address);
    $('#bar-detail-foursquare-link').attr('href', bar.Foursquare);

    var hoursHtml = '';

    for (var i = 0; i < bar.OpeningTimes.length; i++) {
        hoursHtml += '<li class="mdl-list__item mdl-list__item--two-line ' + (bar.OpeningTimes[i].IsToday ? "active" : "") + '">';
        hoursHtml += '  <span class="mdl-list__item-primary-content">';
        hoursHtml += '      <i class="material-icons mdl-list__item-icon">' + (bar.OpeningTimes[i].IsOpen ? "local_bar" : "") + '</i>';
        hoursHtml += '      <span>' + bar.OpeningTimes[i].DayOfWeekMessage + '</span>';
        hoursHtml += '      <span class="mdl-list__item-sub-title">' + bar.OpeningTimes[i].HoursFormat + '</span>';
        hoursHtml += '  </span>';
        hoursHtml += '</li>';
    }
    $('#bar-detail-hours').html(hoursHtml);

    $('header').addClass('hide');
    $('#bar-detail').addClass('open');
    $('#bar-list').addClass('list-hide');

    getExternalBarDetail(bar);
}
function getExternalBarDetail(bar) {
    if (!bar.AlreadyDisplayExternalData) {
        $.get('/Location/GetExternalInfos?id=' + bar.Id, function (data) {
            bar.DataFoursquare = data.Foursquare;
            bar.DataGoogle = data.Google;
            bar.AlreadyDisplayExternalData = true;
            displayExternalBarDetail(bar);
        });
    } else {
        displayExternalBarDetail(bar);
    }
}
function displayExternalBarDetail(bar) {
    if (bar.DataFoursquare && bar.DataFoursquare.Id != '') {
        var hasDataFoursquare = false;

        $('#bar-external-foursquare-phone').css('display', 'none');
        $('#bar-external-foursquare-website').css('display', 'none');
        $('#bar-external-foursquare-price').css('display', 'none');

        if (bar.DataFoursquare.Price != 0) {
            hasDataFoursquare = true;
            $('#bar-external-foursquare-price').css('display', 'flex');
            var html = '';
            for (var i = 0; i < bar.DataFoursquare.Price; i++) {
                html += '<i class="material-icons mdl-list__item-icon">star</i>';
            }
            for (var i = 0; i < 4 - bar.DataFoursquare.Price; i++) {
                html += '<i class="material-icons mdl-list__item-icon">star_border</i>';
            }
            $('#bar-external-foursquare-price-value').html(html);
        }
        if (bar.DataFoursquare.Phone != null && bar.DataFoursquare.Phone != '') {
            hasDataFoursquare = true;
            $('#bar-external-foursquare-phone').css('display', 'flex');
            $('#bar-external-foursquare-phone-value').text(bar.DataFoursquare.Phone);
            $('#bar-external-foursquare-phone-control').attr('href', 'tel:' + bar.DataFoursquare.Phone);
        }
        if (bar.DataFoursquare.Website != null && bar.DataFoursquare.Website != '') {
            hasDataFoursquare = true;
            $('#bar-external-foursquare-website').css('display', 'flex');
            $('#bar-external-foursquare-website-value').text(bar.DataFoursquare.Website);
            $('#bar-external-foursquare-website-control').attr('href', bar.DataFoursquare.Website);
        }

        if (hasDataFoursquare)
            $('#bar-external-foursquare').show();
    }

    $('#bar-detail-google').hide();
    if (bar.DataGoogle && bar.DataGoogle.Subways.length > 0) {
        for (var i = 0; i < bar.DataGoogle.Subways.length; i++) {
            var html = '';
            html += '<li class="mdl-list__item"><span class="mdl-list__item-primary-content"><i class="material-icons mdl-list__item-icon">subway</i>';
            html += bar.DataGoogle.Subways[i].name
            html += '&nbsp;';
            for (var j = 0; j < bar.DataGoogle.Subways[i].lines.length; j++) {
                html += '<img src="/Content/img/ratp/M_' + bar.DataGoogle.Subways[i].lines[j] + '.png">';
            }
            html += '</span></li>';

            $('#bar-detail-google .mdl-list').html(html);
        }
        $('#bar-detail-google').show();
    }
}

function initBarMap(data) {
    locations = data;

    $('#filterTrigger').show();

    if (locations.length > 0) {
        initializeMap();
    } else {
        r(function () {
            toastAlert("Aucun bar n'est disponible pour cette recherche");
            setLoader(false);
        });
    }
}
function r(f) { /in/.test(document.readyState) ? setTimeout('r(' + f + ')', 9) : f() }

var directionDisplay;
var directionsService;
var polyline = null;
var infowindow = new google.maps.InfoWindow();

function getBarsFromLocation(lat, lng) {
    $.get('/Location/Geolocation?latitude=' + lat + '&longitude=' + lng, function (data) {
        initBarMap(data);
    });
}

function computeMidWay() {
    directionsDisplay = new google.maps.DirectionsRenderer({ suppressMarkers: true });
    polyline = new google.maps.Polyline({
        path: [],
        strokeColor: "#FF4081",
        strokeOpacity: 0.5,
        strokeWeight: 4
    });
    if (displayInitineraryMode == 'directions')
        directionsDisplay.setMap(map);
    calcRoute();
}
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
function calcRoute() {
    var start = getParameterByName('latA') + ',' + getParameterByName('lngA');
    var end = getParameterByName('latB') + ',' + getParameterByName('lngB');
    var travelMode = google.maps.DirectionsTravelMode.TRANSIT;

    var request = {
        origin: start,
        destination: end,
        travelMode: travelMode
    };
    directionsService = new google.maps.DirectionsService();
    directionsService.route(request, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            polyline.setPath([]);
            var bounds = new google.maps.LatLngBounds();
            startLocation = new Object();
            endLocation = new Object();
            directionsDisplay.setDirections(response);
            var route = response.routes[0];

            // For each route, display summary information.
            var path = response.routes[0].overview_path;
            var legs = response.routes[0].legs;
            for (i = 0; i < legs.length; i++) {
                if (i == 0) {
                    startLocation.latlng = legs[i].start_location;
                    startLocation.address = legs[i].start_address;
                    //getBarsFromLocation(legs[i].start_location.lat(), legs[i].start_location.lng());
                }
                endLocation.latlng = legs[i].end_location;
                endLocation.address = legs[i].end_address;
                var steps = legs[i].steps;
                for (j = 0; j < steps.length; j++) {
                    var nextSegment = steps[j].path;
                    for (k = 0; k < nextSegment.length; k++) {
                        polyline.getPath().push(nextSegment[k]);
                        bounds.extend(nextSegment[k]);
                    }
                }
            }
            if (displayInitineraryMode == 'default')
                polyline.setMap(map);

            computeTotalDistance(response);
        } else {
            console.log('directions response', status);
        }
    });
}

var totalDist = 0;
var totalTime = 0;
function computeTotalDistance(result) {
    totalDist = 0;
    totalTime = 0;
    var myroute = result.routes[0];
    for (i = 0; i < myroute.legs.length; i++) {
        totalDist += myroute.legs[i].distance.value;
        totalTime += myroute.legs[i].duration.value;
    }
    putMarkerOnRoute(50);

    totalDist = totalDist / 1000;
}
function putMarkerOnRoute(percentage) {
    var distance = (percentage / 100) * totalDist;
    var time = ((percentage / 100) * totalTime / 60).toFixed(2);
    var latlng = polyline.GetPointAtDistance(distance);

    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        icon: 'http://mt.google.com/vt/icon/name=icons/spotlight/spotlight-ad.png'
    });
    markers.push(marker);

    getBarsFromLocation(latlng.lat(), latlng.lng());
}

$(function () {
    $('#bar-detail .close-button').click(function (e) {
        $('#bar-detail').removeClass('open');
        $('#bar-list').removeClass('list-hide');
        $('header').removeClass('hide');
        e.preventDefault();
        return false;
    });
});
function initApp() {
    setLoader(true);
    initMap();
    var query = window.location.href.split('?')[1];
    switch (mode) {
        case "Default":
            $.get('/Location/Default?' + query, function (data) {
                initBarMap(data);
            });
            break;
        case "Geolocation":
            $.get('/Location/Geolocation?' + query, function (data) {
                initBarMap(data);
            });
            break;
        case "Midway":
            computeMidWay();
            break;
    }
}

function setLoader(isVisible) {
    if (isVisible) {
        $('#loader').fadeIn();
    }
    else {
        $('#loader').fadeOut();
    }
}