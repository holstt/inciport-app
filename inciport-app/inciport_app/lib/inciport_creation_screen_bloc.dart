import 'dart:convert';
import 'dart:core';
import 'dart:io';

import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import 'package:inciport_app/popup.dart';

import 'http_client.dart';
import 'inciport.dart';

class InciportCreationScreenBloc {
  InciportCreationScreenBloc(); // constructor

  final TextEditingController descriptionController = TextEditingController();
  final TextEditingController nameFieldController = TextEditingController();
  final TextEditingController phoneNrFieldController = TextEditingController();
  final TextEditingController mailFieldController = TextEditingController();

  Http_client httpClient = Http_client();

  List<String> imagesBase64 = <String>[];
  List<File> images = <File>[];

  List<Categories> categories = <Categories>[];

  bool isDoneButtonDisabled = true;

  int? mainCategoryId;
  int? subCategoryId;
  int indexOfMainCategory = 0;

// TODO make the overlaps of the following 2 methods into functions
  List<DropdownMenuItem<String>> categoryListToMainCategoryItems() {
    List<DropdownMenuItem<String>> mainCategories = [];

    if (categories.isNotEmpty) { // check needed since getting categories is done async
      for (var i = 0; i < categories.length; i++) {
        mainCategories.add(
            DropdownMenuItem(
                key: Key(categories[i].title),
                value: categories[i].title,
                child: Text(categories[i].title, style: const TextStyle(color: Colors.black),),
                onTap: () => {mainCategoryId = categories[i].id,
                  indexOfMainCategory = i,
                isDoneButtonDisabled = !enableSubmitButton()}
            )
        );
      }
    } else {
      mainCategories.add(const DropdownMenuItem(child: Text('on its way'), value: '1'));
    }
    return mainCategories;
  }

  List<DropdownMenuItem<String>> categoryListToSubCategoryItems() {
    List<DropdownMenuItem<String>> subCategories = [];

    if (categories.isNotEmpty) { // check needed since getting categories is done async
      for (var i = 0; i < categories[indexOfMainCategory].subCategories!.length; i++) {
        subCategories.add(
            DropdownMenuItem(
                value: categories[indexOfMainCategory].subCategories![i].title,
                child: Text(categories[indexOfMainCategory].subCategories![i].title, style: const TextStyle(color: Colors.black),),
                onTap: () => {subCategoryId = categories[indexOfMainCategory].subCategories![i].id,
                isDoneButtonDisabled = !enableSubmitButton()}
            )
        );
      }
    } else {
      subCategories.add(const DropdownMenuItem(child: Text('Menu items are comming'), value: '1'));
    }
    return subCategories;
  }

  String? checkInfoAndSend (Location location) {
    // returns null if nothing went wrong else return the message that should be in the popup

    String name = nameFieldController.value.text;
    String phoneNumber = phoneNrFieldController.value.text;
    String mail = mailFieldController.value.text;

    if (name.length > 256) {
      return "Please enter a valid Name";
    } else if (phoneNumber.length != 8) {
      return "Please enter a valid phone number";
    } else if (mail.length > 256) {
      return "Please enter a valid Email";
    }

    if (name != '' || phoneNumber != '' || mail != '') {
      if (name == '' || phoneNumber == '' || mail == '') {
        return "Please enter all contact information or nothing";
      }
    }

    httpClient.sendInciport(
        Inciport(
            subCategoryId != null
                ? Category(
                mainCategoryId!, subCategory: Category(subCategoryId!))
                : Category(mainCategoryId!), //categoryIds
            location, // location
            descriptionController.value.text.toString(), // description
            ContactInformation(
                name, // name
                phoneNumber, // phoneNr
                mail // mail
            ),
            imagesBase64.isEmpty ? null : imagesBase64 // assets in base 64
        )
    );
  }

  bool checkIfImageAndStoreIt (XFile? img) {
    if (img != null) {
      File image = File(img.path);
      images.add(image);
      imagesBase64.add(base64Encode(image.readAsBytesSync()));
      return true;
    }
    else {
      return false;
    }
  }

  Future<bool> getAvailableCategories(Location location) async {
    List<Categories>? availableCategories = await httpClient.recieveCategories(
        location.address.municipalityId!);

    if (availableCategories != null) {
      categories = availableCategories;
      return true;
    } else {
      return false;
    }
  }

  bool enableSubmitButton () {
    if (categories.isEmpty) {
      return false;
    }
    if (descriptionController.value.toString() != '' &&
        mainCategoryId != null &&
        (categories[indexOfMainCategory].subCategories == null || subCategoryId != null)
    ) {
      return true;
    } else {
      return false;
    }
  }
}