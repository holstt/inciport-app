
import 'package:flutter/material.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:location/location.dart' as loc;
import 'package:super_tooltip/super_tooltip.dart';
import 'package:synchronized/synchronized.dart';

import './inciport.dart' as iPort;
import './inciport_creation_screen.dart';
import './map_screen_bloc.dart';
import 'popup.dart';


class MyHomePage extends StatefulWidget {
  const MyHomePage({Key? key, required this.title}) : super(key: key);

  final String title;

  @override
  State<MyHomePage> createState() => MyHomePageState(title);
}

class MyHomePageState extends State<MyHomePage> {
  MyHomePageState(this.title);

  final String title;

  var lock = Lock();

  final MapScreenBloc bloc = MapScreenBloc();

  bool locationHasBeenSet = false;

  CameraPosition campos = const CameraPosition(target: LatLng(57.04241, 9.91908), zoom: 4); //default camera start position

  late LatLng inciportCoords;
  iPort.Address? inciportAddress;
  int? municipalityId;

  SuperTooltip? pinpopup;

  @override
  Widget build(BuildContext context) {
    if (!locationHasBeenSet) {
      getLocationStatus();
    }
    return Scaffold(
      appBar: AppBar(
        centerTitle: true,
        title: Text(title)
      ),
      body: GoogleMap(
        mapType: MapType.hybrid,
        initialCameraPosition: campos,
        onMapCreated: (GoogleMapController controller) {
          bloc.controller.complete(controller);
        },
        mapToolbarEnabled: false,
        zoomControlsEnabled: false,
        myLocationButtonEnabled: bloc.serviceEnabled == null ? false : bloc.serviceEnabled!,
        myLocationEnabled: true,
        markers: bloc.markers,
        onCameraMove: getMarkersIfNotBeingDone,
        onLongPress: _addMarker,
      ),
      floatingActionButton: FloatingActionButton(
        key: const Key('Add Inciport'),
        onPressed: !bloc.serverIsUp? null // make the add inciport button do nothing if the server is not up
            :
        inciportAddress == null ? () => showPopupWithText( // display popup when user have not set the location or shared theirs with the app
            'Please choose a location on the map by long pressing or allow app to use your location', context
        ) :
            () {Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => InciportCreationScreen(
              inciportLocation: iPort.Location(inciportAddress!, inciportCoords.longitude, inciportCoords.latitude)
          )),
        );},
        tooltip: 'Create Inciport',
        child: const Icon(Icons.add),
      ),
    );
  }

  void _addMarker(LatLng tappedPoint) async {
    final GoogleMapController controller = await bloc.controller.future;

    if (await controller.getZoomLevel() <= 14) {
      showPopupWithText('Please zoom in further before placing your pin', context);
      return;
    }

    bloc.markers.add(Marker(markerId: const MarkerId('incidentLocation'), position: tappedPoint));
    inciportCoords = tappedPoint;

    inciportAddress = await bloc.getAddress(tappedPoint);

    if (inciportAddress == null) {
      showPopupWithText(
          'Connection with backend is currently down, please check your connection'
              'if the problem persists you can call your municipality for enquiries',
          context);
    } else {
      setState(() {});
    }
  }

  void getLocationStatus() async {
    late loc.LocationData? _locationData;

    _locationData = await bloc.getLocation();

    if (_locationData == null) {
      return;
    }

    if (locationHasBeenSet != true) {
      locationHasBeenSet = true;

      final GoogleMapController controller = await bloc.controller.future;
      LatLng position = LatLng(
          _locationData.latitude!, _locationData.longitude!);

      inciportAddress = await bloc.getAddress(position); // set address initially to user's location
      inciportCoords = position; // set cords to be the user's location

      controller.moveCamera(CameraUpdate.newCameraPosition(
          CameraPosition(target: position, zoom: 14.4746)));

      setState(() {});
    }
  }

  void getMarkersIfNotBeingDone(CameraPosition mapPosition) async {
    if (lock.locked || mapPosition.zoom <= 12) { // Needs to be zoomed in less than the value 12 which is chosen at random
      return;
    } else {
      await lock.synchronized(() => {
        getMarkersForMuniAndSetState(mapPosition)
      });
    }
  }

  void getMarkersForMuniAndSetState (CameraPosition mapPosition) async {
    bool? updateState = await bloc.checkMuniAndGetMarkers(mapPosition, context);

    if (updateState == null) {
      showPopupWithText('Connection with backend is currently down, please check your connection'
          'if the problem persists you can call your municipality for enquiries', context);
    } else if (updateState) {
      setState(() {});
    }
  }

}