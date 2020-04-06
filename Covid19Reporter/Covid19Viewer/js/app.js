(function () {
    'use strict';

    var mymap = L.map('mapid').setView([48.8566, 2.3522], 12);

    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoiamFjcXVlc2thbmciLCJhIjoiY2s4azY4Y2tpMDFodTNlczdmZjNuajczeCJ9.4jtaO63UWDfL9OL_-33x-A'
    }).addTo(mymap);

    var greeIcon = L.icon({
        iconUrl: 'https://cdn.rawgit.com/pointhi/leaflet-color-markers/master/img/marker-icon-green.png',
        shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
    });

    var redIcon = L.icon({
        iconUrl: 'https://cdn.rawgit.com/pointhi/leaflet-color-markers/master/img/marker-icon-red.png',
        shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
    });

    var api = '/api'

    $(document).ready(function () {
        $.getJSON(`${api}/patients`, function (patients) {
            $.each(patients, function (_, patient) {
                var latLng = [patient.position.Latitude, patient.position.Longitude];
                L.marker(latLng, {
                    icon: patient.isSuspected ? redIcon : greeIcon
                }).addTo(mymap).on('click', function () {
                    $.getJSON(`${api}/patients/${patient.id}`, function (list) {
                        var details = list[0];
                        var content = `<strong>${details.userName}</strong><hr>`;
                        $.each(details.records, function (_, record) {
                            var symptoms = [];
                            if (record.Symptoms & (1 << 0)) {
                                symptoms.push('fièvre');
                            }
                            if (record.Symptoms & (1 << 1)) {
                                symptoms.push('toux');
                            }
                            if (record.Symptoms & (1 << 2)) {
                                symptoms.push('mal de crâne');
                            }
                            if (record.Symptoms & (1 << 3)) {
                                symptoms.push('mal à respirer');
                            }
                            if (record.Symptoms & (1 << 16)) {
                                symptoms.push('autres');
                            }
                            if (symptoms.length === 0) {
                                symptoms.push('rien')
                            }
                            var symptoms2 = symptoms.join(', ')

                            content += `<p>
<i>${record.SubmitTime}</i><br/>
Symptômes : <b><i>${symptoms2}</i></b><br/>
Recommendation : <b><i>${record.Recommendation}</i></b><br/>
</p>`
                        });

                        L.popup()
                            .setLatLng(latLng)
                            .setContent(content)
                            .openOn(mymap);
                    });
                });
            });
        });
    });
})();