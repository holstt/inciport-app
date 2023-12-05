
import 'package:flutter_test/flutter_test.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:inciport_app/inciport.dart';
import 'package:inciport_app/map_screen_bloc.dart';

import 'inciport_creation_screen_test.mocks.dart';

class MockMapScreenBloc extends MapScreenBloc {
  @override
  Future<Address> getAddress(LatLng coords) async {
    return Address(
        'mockStreet 2',
        'mockCity',
        'mockZip',
        'mockCountry',
        'mockMunicipality',
        municipalityId: 1
    );
  }

  @override
  Future<String?> getMunicipalityName(double latitude, double long) async {
    return 'mockMunicipality';
  }
}

void main() {

  MockMapScreenBloc bloc = MockMapScreenBloc();
  LatLng coords = const LatLng(1, 1);
  final client = MockClient();


  test('checkMuniAndGetMarkers with no muniMap', () async {
    bool? answer = await bloc.checkMuniAndGetMarkers(const CameraPosition(target: LatLng(1, 1)), null);

    expect(answer!, false);
  });

  test('checkMuniAndGetMarkers where the city have already been visited', () async {
    bloc.visitedMunicipalities.add('mockMunicipality');

    bool? answer = await bloc.checkMuniAndGetMarkers(const CameraPosition(target: LatLng(1, 1)), null);

    expect(answer!, false);
  });

}