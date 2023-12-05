
import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:inciport_app/config.dart';
import 'package:inciport_app/http_client.dart';
import 'package:mockito/annotations.dart';
import 'package:mockito/mockito.dart';

import 'inciport_creation_screen_test.mocks.dart';


@GenerateMocks([http.Client])
void main() {
  final client = MockClient();

  when(client.get(Uri.parse(baseUrl)))
      .thenAnswer((_) async => http.Response('[{"id": 1, "name": "mockMunicipality"}]', 200));

  when(client.get(Uri.parse('$baseUrl/1/categories')))
      .thenAnswer((_) async => http.Response('[{"subCategories": [{"id": 3,"title": "Vegetation near road"},{"id": 5,"title": "Hole"}], "id": 2,"title": "Road"}]', 200));

  // use this for http client function calls to make sure backend is not called
  Mock_Http_client mockClient = Mock_Http_client(client);


  test('recieveMunicipalityMap returns the correct Map', () async {
    Map<String, int> muniMap = Map();
    muniMap['mockMunicipality'] = 1;

    expect(await mockClient.recieveMunicipalityMap(), muniMap);

  });

  test('recieve categories returns a correct List of Categories', () async {
    List<Categories> categoryList = <Categories>[];
    List<Categories> subcategoryList = <Categories>[];

    subcategoryList.add(Categories(id: 3, title: 'Vegetation near road'));
    subcategoryList.add(Categories(id: 5, title: 'Hole'));

    categoryList.add(Categories(
        id: 2,
        title: 'Road',
        subCategories: subcategoryList
    ));

    List<Categories> gottenList = await mockClient.recieveCategories(1);

    expect(gottenList[0].title.toString(), categoryList[0].title.toString());
    expect(gottenList[0].id.toString(), categoryList[0].id.toString());
    expect(gottenList[0].subCategories![0].title.toString(), categoryList[0].subCategories![0].title.toString());
    expect(gottenList[0].subCategories![0].id.toString(), categoryList[0].subCategories![0].id.toString());
    expect(gottenList[0].subCategories![1].title.toString(), categoryList[0].subCategories![1].title.toString());
    expect(gottenList[0].subCategories![1].id.toString(), categoryList[0].subCategories![1].id.toString());

  });

}
