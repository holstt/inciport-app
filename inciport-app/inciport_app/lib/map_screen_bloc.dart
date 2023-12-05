import 'dart:async';

import 'package:flutter/material.dart';
import 'package:google_geocoding/google_geocoding.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:inciport_app/http_client.dart';
import 'package:location/location.dart' as loc;
import 'package:super_tooltip/super_tooltip.dart';

import 'inciport.dart';

class MapScreenBloc {
  MapScreenBloc();

  final Completer<GoogleMapController> controller = Completer();

  bool? serviceEnabled;
  Http_client httpClient = Http_client();
  bool serverIsUp = true;
  Map<String, int>? muniMap;
  List<String> visitedMunicipalities = <String>[];
  Set<Marker> markers = {};
  var googleGeocoding = GoogleGeocoding("---");

  Future<String?> getMunicipalityName (double latitude, double longitude) async {
    GeocodingResponse? result = await googleGeocoding.geocoding.getReverse(LatLon(
        latitude,
        longitude
    ));

    var results = result!.results!;

    String muniName = "";

    for (var data in results) {
      for(var i = 0; i < data.types!.length; i++) {
        if (data.types![i] == "administrative_area_level_2") {
          muniName = data.addressComponents![i].longName!;
        }
      }
    }
    if (muniName != "") {
      muniName = muniName.replaceAll(" Municipality", "");
    } else {
      return null;
    }
    return muniName;
  }

  Future<loc.LocationData?> getLocation() async {

    loc.Location location = loc.Location();
    late loc.PermissionStatus _permissionGranted;


    serviceEnabled = await location.serviceEnabled();
    if (!serviceEnabled!) {
      serviceEnabled = await location.requestService();
      if (!serviceEnabled!) {
        return null;
      }
    }

    _permissionGranted = await location.hasPermission();
    if (_permissionGranted == loc.PermissionStatus.denied) {
      _permissionGranted = await location.requestPermission();
      if (_permissionGranted != loc.PermissionStatus.granted) {
        return null;
      }
    }

    return location.getLocation();
  }

  Future<Address?> getAddress(LatLng coords) async {
    GeocodingResponse? result = await googleGeocoding.geocoding.getReverse(LatLon(
        coords.latitude,
        coords.longitude
    ));

    muniMap ??= await httpClient.recieveMunicipalityMap();

    if (muniMap == null) {
      serverIsUp = false;
      return null;
    }

    var results = result!.results!;

    int? zipId, countryId, muniId;

    for (var i = 0; i < results.length; i++) { // find placement of data in results from geodata
      if (results[i].types![0] == "postal_code") {
        zipId = i;
      } else if (results[i].types![0] == "administrative_area_level_2") {
        muniId = i;
      } else if (results[i].types![0] == "country") {
        countryId = i;
      }
    }

    String municipalityName = results[muniId!].addressComponents![0].longName!.replaceAll(" Municipality", ""); // get muniName and remove Municipality from the end

    //Dette er virkeligt grimt
    return Address(
        results[0].addressComponents![1].longName! + results[0].addressComponents![0].longName!, //streetname + street number
        results[zipId!].addressComponents![1].longName!, // city (always second element in postal code result)
        results[zipId].addressComponents![0].longName!, // postal code/zipcode
        results[countryId!].addressComponents![0].longName!, // country
        municipalityName, // municipality
        municipalityId: muniMap![municipalityName]
    );
  }

  Future<bool?> checkMuniAndGetMarkers(CameraPosition mapPosition, BuildContext? context) async {

    muniMap ??= await httpClient.recieveMunicipalityMap();

    if (muniMap == null) {
      serverIsUp = false;
      return null;
    }

    String? muniName = await getMunicipalityName(mapPosition.target.latitude, mapPosition.target.longitude);

    if (muniName == null) { // if there is no municipality name in the query
      return false;
    }

    if (!visitedMunicipalities.contains(muniName)) { // check if we visited muni already so we don't have to call backend repeatedly
      visitedMunicipalities.add(muniName);

      int? muniId = muniMap![muniName];

      if (muniId != null) { // check if muni exist within our system
        List<Inciport> inciports = await httpClient.recieveInciports(muniId);

        for (Inciport report in inciports) {
          markers.add(Marker(
              markerId: MarkerId(report.id.toString()),
              position: LatLng(report.location.latitude, report.location.longitude),
              icon: BitmapDescriptor.defaultMarkerWithHue(BitmapDescriptor.hueBlue),
              onTap: () => pinPopupWindow(report, context!)
          ));
        }
        return true;
      }
    }
    return false;
  }

  void pinPopupWindow(Inciport report, BuildContext context) {
    String subCatTitle = report.mainCategory.subCategory == null ? '' : 'Sub Category: ${report.mainCategory.subCategory!.title!}';

    SuperTooltip tooltip = SuperTooltip(
        content: Material(
          child: Text(
              'Main category: ${report.mainCategory.title}    Status: ${report.status}\n'
                  '$subCatTitle    id: ${report.id}\n \n'

                  '${report.location.address.street} \n'
                  'Submitted: 05/12 - 2021 \n'
                  'Modified: 07/12 - 2021 \n \n'

                  '${report.description}'
          ),
        ),
        showCloseButton: ShowCloseButton.inside,
        popupDirection: TooltipDirection.up,
        hasShadow: true,
        maxWidth: MediaQuery.of(context).size.width - 20
    );

    tooltip.show(context);
  }

}




