import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:http/http.dart' as http;
import 'package:inciport_app/inciport.dart';
import 'package:inciport_app/map_screen.dart';
import 'package:inciport_app/map_screen_bloc.dart';
import 'package:mockito/annotations.dart';

class MockMapScreenBloc extends MapScreenBloc {

  @override
  List<String> visitedMunicipalities = <String>['mockMunicipality'];

  @override
  Future<bool> checkMuniAndGetMarkers(CameraPosition mapPosition, BuildContext? context) async => true;

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

class MockMapScreen extends MyHomePageState {
  MockMapScreen() : super('Inciport');

  @override
  LatLng inciportCoords = const LatLng(1, 1);

  @override
  Address? inciportAddress = Address(
      'mockStreet 2',
      'mockCity',
      'mockZip',
      'mockCountry',
      'mockMunicipality',
      municipalityId: 1
  );

  @override
  MapScreenBloc bloc = MockMapScreenBloc();

  @override
  bool locationHasBeenSet = true;
}

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Inciport',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: const MyHomePage(title: 'Inciport'),
    );
  }
}

Future<Address> mockGetAddress(LatLng coords) async {
  return Address(
      'mockStreet 2',
      'mockCity',
      'mockZip',
      'mockCountry',
      'mockMunicipality',
      municipalityId: 1
  );
}



@GenerateMocks([http.Client])
void main() {

  LatLng coords = const LatLng(1, 1);

  MyHomePageState mockScreen = MockMapScreen();

  Widget screen = Container();
  
  setUp(() {

    screen = Builder(
        builder: (BuildContext context) {
          return MaterialApp(
            home: mockScreen.build(context),
          );
        }
    );


  });

  group('mapScreen Rendering', () {

    testWidgets('Map screen shows title', (WidgetTester tester) async {
      await tester.pumpWidget(const MyApp());

      expect(find.text('Inciport'), findsOneWidget);
    });

    testWidgets('Map screen shows a map', (WidgetTester tester) async {
      await tester.pumpWidget(screen);

      expect(find.byType(GoogleMap), findsOneWidget);
    });

    testWidgets('Map screen shows a map', (WidgetTester tester) async {
      await tester.pumpWidget(screen);

      expect(find.byType(GoogleMap), findsOneWidget);
    });

    testWidgets('Add an inciport button being rendered', (WidgetTester tester) async {
      await tester.pumpWidget(screen);

      expect(find.byKey(const Key('Add Inciport')), findsOneWidget);
    });
  });

  group('mapScreen Functionality', () {

    testWidgets('Popup shows when no location have been set ', (WidgetTester tester) async {
      await tester.pumpWidget(const MyApp());

      await tester.tap(find.byKey(const Key('Add Inciport')));

      await tester.pump(const Duration(seconds: 2));

      expect(find.byKey(const Key('popup'), skipOffstage: false), findsWidgets);
    });
  });
}
