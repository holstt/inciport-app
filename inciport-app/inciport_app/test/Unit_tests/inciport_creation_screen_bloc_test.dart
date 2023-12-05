
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:image_picker/image_picker.dart';
import 'package:inciport_app/http_client.dart';
import 'package:inciport_app/inciport_creation_screen_bloc.dart';

class MockInciportCreationScreenBloc extends InciportCreationScreenBloc {}

void main() {
  InciportCreationScreenBloc bloc = MockInciportCreationScreenBloc();

  setUp(() {
    bloc.categories.add(Categories(id: 1, title: 'mockIce',
        subCategories: <Categories>[Categories(id: 3, title: 'mockSubCat'),Categories(id: 4, title: 'mockSubCat2')]));
    bloc.categories.add(Categories(id: 21, title: 'mockCatWithoutSubCat'));
  });

  test('categoryListToMainCategoryItems() test', () {
    List<DropdownMenuItem<String>> list = bloc.categoryListToMainCategoryItems();

    List<DropdownMenuItem<String>> compareList = <DropdownMenuItem<String>>[];
    compareList.add(const DropdownMenuItem<String>(
        value: "mockIce",
        child: Text("mockIce", style: TextStyle(color: Colors.black)),
    ));

    compareList.add(const DropdownMenuItem<String>(
    value: 'mockCatWithoutSubCat',
        child: Text('mockCatWithoutSubCat', style: TextStyle(color: Colors.black)),
    ));

    expect(list[0].child.toString(), compareList[0].child.toString());
    expect(list[1].value.toString(), compareList[1].value.toString());
  });

  test('categoryListToSubCategoryItems() test with maincategory with subcategories', () {
    List<DropdownMenuItem<String>> list = <DropdownMenuItem<String>>[];
    bloc.indexOfMainCategory = 0;
    list = bloc.categoryListToSubCategoryItems();

    List<DropdownMenuItem<String>> compareList = <DropdownMenuItem<String>>[];
    compareList.add(const DropdownMenuItem<String>(
      value: "mockSubCat",
      child: Text("mockSubCat", style: TextStyle(color: Colors.black)),
    ));

    compareList.add(const DropdownMenuItem<String>(
      value: 'mockSubCat2',
      child: Text('mockSubCat2', style: TextStyle(color: Colors.black)),
    ));

    expect(list[0].child.toString(), compareList[0].child.toString());
    expect(list[1].value.toString(), compareList[1].value.toString());
  });

  //checkIfImageAndStoreIt
  test('checkIfImageAndStoreIt test', () {
    expect(bloc.checkIfImageAndStoreIt(null), false);
    expect(bloc.checkIfImageAndStoreIt(XFile('assets/littleBlueCircle.png')), true);
    expect(bloc.images.isNotEmpty, true);
  });

  test('checkIfImageAndStoreIt test', () {
    expect(bloc.checkIfImageAndStoreIt(null), false);
    expect(bloc.checkIfImageAndStoreIt(XFile('assets/littleBlueCircle.png')), true);
    expect(bloc.images.isNotEmpty, true);
  });



}
