import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:inciport_app/config.dart';
import 'package:inciport_app/http_client.dart';
import 'package:inciport_app/inciport.dart';
import 'package:inciport_app/inciport_creation_screen.dart';
import 'package:inciport_app/inciport_creation_screen_bloc.dart';
import 'package:mockito/annotations.dart';
import 'package:mockito/mockito.dart';

import '../Unit_tests/inciport_creation_screen_test.mocks.dart';

var client = MockClient();

class MockInciportCreationScreenBloc extends InciportCreationScreenBloc {
  @override
  List<Categories> categories = <Categories>[Categories(id: 1, title: 'mainCategoryOne', subCategories: <Categories>[Categories(id: 2, title: 'subCategory')])];

  @override
  final httpClient = Mock_Http_client(client);

  @override
  List<File> images = <File>[File('assets/littleBlueCircle.png')];

  @override
  List<String> imagesBase64 = <String>['hej'];

}

class MockInciportCreationScreen extends CategorySelectorState {
  @override
  InciportCreationScreenBloc bloc = MockInciportCreationScreenBloc();

  MockInciportCreationScreen() : super(
      Location(
          Address(
              'mockStreet 2',
              'mockCity',
              'mockZip',
              'mockCountry',
              'mockMunicipality',
              municipalityId: 1
          ),
          1,  //long
          1  //lat
      )
  );

  @override // dont make calls to googleMaps api in the mock, therefore override to return an empty container
  Widget showInciportOnMap() {return Container(key: const Key('position'), height: 10, width: 10);}
}



@GenerateMocks([http.Client])
void main(){

  CategorySelectorState mockScreen = MockInciportCreationScreen();
  Widget screen = Container();

  setUp(() {

    when(client.get(Uri.parse('$baseUrl/1/categories')))
        .thenAnswer((_) async => http.Response('[{"subCategories": [{"id": 3,"title": "Vegetation near road"},{"id": 5,"title": "Hole"}], "id": 2,"title": "Road"}]', 200));

    screen = Builder(
        builder: (BuildContext context) {
          return MaterialApp(
            home: mockScreen.build(context),
          );
        }
    );

  });

  group('inciportCreationScreen Rendering', () {

    testWidgets('the screen renders without problems', (WidgetTester tester) async {
      await tester.pumpWidget(screen);
    });

    testWidgets('Appbar have the the correct title', (WidgetTester tester) async {
      await tester.pumpWidget(screen);

      expect(find.text('Inciport'), findsOneWidget);

    });

    testWidgets('Map showing the location chosen is rendered', (WidgetTester tester) async {
      await tester.pumpWidget(screen);

      expect(find.byKey(const Key('position')), findsOneWidget);
    });

    testWidgets('Dropdown menues for the choosing of categories exists', (WidgetTester tester) async {
      await tester.pumpWidget(screen);

      expect(find.byKey(const Key('MainCategories')), findsOneWidget);
    });

    testWidgets('Text fields is displayed', (WidgetTester tester) async {
      await tester.pumpWidget(screen);

      expect(find.byKey(const Key('description')), findsOneWidget);
      expect(find.byKey(const Key('nameField')), findsOneWidget);
      expect(find.byKey(const Key('phoneField')), findsOneWidget);
      expect(find.byKey(const Key('mailField')), findsOneWidget);

    });

    testWidgets('Done button is present', (WidgetTester tester) async {
      await tester.pumpWidget(screen);

      await tester.pump(const Duration(milliseconds: 100));

      expect(find.byKey(const Key('submit'), skipOffstage: false), findsOneWidget);
    });
  });

  group('inciportCreationScreen functionality', () {

    testWidgets('Submit incident report button works after entering valid information', (WidgetTester tester) async {
      await tester.pumpWidget(screen);

      await tester.enterText(find.byKey(const Key('description')), 'this is a description');
      await tester.enterText(find.byKey(const Key('nameField')), 'Uh My name is');
      await tester.enterText(find.byKey(const Key('phoneField')), '12345678');
      await tester.enterText(find.byKey(const Key('phoneField')), 'john.h@ssmail.com');

      await tester.pumpAndSettle(); //let the program process the entered information

      await tester.tap(find.byKey(const Key('submit')));

      expect(find.byKey(const Key('popup'), skipOffstage: false), findsNothing);
    });

    testWidgets('All Categories in dropdown menu is correct', (WidgetTester tester) async {
      await tester.pumpWidget(screen);

      await tester.tap(find.byKey(const Key('MainCategories'), skipOffstage: false));
      await tester.pumpAndSettle();

      expect(find.byKey(const Key('mainCategoryOne')), findsWidgets);
    });

  });
}
