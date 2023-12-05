import 'dart:async';
import 'dart:convert';

import 'package:http/http.dart' as http;

import './config.dart';
import 'inciport.dart';

class Http_client {
  Http_client(); // constructor

  Future<void> sendInciport(Inciport report) async {

    var url = Uri.parse('$baseUrl/${report.location.address.municipalityId}/inciports');
    var response = await http.post(url, headers: {"Content-Type": "application/json"}, body: jsonEncode(report));
    print('Response status: ${response.statusCode}');
    print('Response body: ${response.body}');

  }

  Future<List<Categories>?> recieveCategories(int muniId) async {
    List<Categories> categories = <Categories>[];
    final url = Uri.parse('$baseUrl/$muniId/categories');
    final response = await http.get(url);

    print('Response status: ${response.statusCode}');
    print('Response body: ${response.body}');

    if (response.statusCode != 200) {
      return null;
    }

    List<dynamic> list = json.decode(response.body);

    for(var i = 0; i < list.length; i++) {
      categories.add(Categories.fromJson(list[i]));
    }

    return categories;
  }

  Future<Map<String, int>?> recieveMunicipalityMap() async {
    final url = Uri.parse(baseUrl);
    final response = await http.get(url);

    if (response.statusCode != 200) {
      return null;
    }

    List<Municipality> muniList = <Municipality>[];
    List<dynamic> list = json.decode(response.body);

    for(var i = 0; i < list.length; i++) {
      muniList.add(Municipality.fromJson(list[i]));
    }

    Map<String, int> muniMap = {};
    for (Municipality muni in muniList) {
      muniMap[muni.name] = muni.id;
    }

    return muniMap;
  }

  Future<List<Inciport>> recieveInciports(int muniId) async {
    List<Inciport> inciports = <Inciport>[];
    final url = Uri.parse('$baseUrl/$muniId/inciports');
    final response = await http.get(url);

    print('Response status: ${response.statusCode}');
    print('Response body: ${response.body}');

    List<dynamic> list = json.decode(response.body);

    for(var i = 0; i < list.length; i++) {
      inciports.add(Inciport.fromJson(list[i]));
    }

    return inciports;
  }
}

// needed when extracting all possible categories, since the json recieved is different
class Categories {

  final List<Categories>? subCategories;
  final int id;
  final String title;

  Categories({
    this.subCategories,
    required this.id,
    required this.title,
  });

  factory Categories.fromRawJson(String str) => Categories.fromJson(json.decode(str));

  factory Categories.fromJson(Map<String, dynamic> json) => Categories(
    subCategories: json["subCategories"] == null ? null : List<Categories>.from(json["subCategories"].map((x) => Categories.fromJson(x))),
    id: json["id"],
    title: json["title"],
  );
}

class Municipality {

  final int id;
  final String name;

  Municipality(this.id, this.name);

  factory Municipality.fromJson(Map<String, dynamic> json) => Municipality(
      json["id"],
      json["name"]
  );

}


