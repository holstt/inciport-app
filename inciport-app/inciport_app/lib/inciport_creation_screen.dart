import 'dart:async';
import 'dart:core';

import 'package:flutter/material.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:image_picker/image_picker.dart';

import './inciport.dart';
import 'config.dart';
import 'inciport_creation_screen_bloc.dart';
import 'popup.dart';


class InciportCreationScreen extends StatefulWidget {
  const InciportCreationScreen({Key? key, required this.inciportLocation}) : super(key: key);
  final Location inciportLocation;
  @override
  State<InciportCreationScreen> createState() => CategorySelectorState(inciportLocation);
}

class CategorySelectorState extends State<InciportCreationScreen> {
  CategorySelectorState(this.location); // constructor

  final Location location;

  final InciportCreationScreenBloc bloc = InciportCreationScreenBloc();

  String mainCategoryDropDownText = 'Choose Main Category';
  String subCategoryDropDownText = 'Choose a Sub Category';
  String description = '';

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(centerTitle: true, title: const Text('Inciport', style: TextStyle(fontWeight: FontWeight.bold))),
      body: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Expanded(
              child: _buildSettings(),
            ),
          ]),
    );
  }

  Widget _buildSettings() {
    return ListView(
      children: <Widget>[
        showInciportOnMap(),
        const Text('Category', style: TextStyle(fontWeight: FontWeight.bold)),
        _mainCategorySelection(),
        _subCategorySelection(),
        _description(),
        const Text('Press the + to add a picture', style: TextStyle(fontWeight: FontWeight.bold)),
        _pictureDisplayInRow(1),
        if (bloc.images.length >= 2) _pictureDisplayInRow(3),
        const Text('Contact Information', style: TextStyle(fontWeight: FontWeight.bold)),
        _nameField(),
        _phoneNrField(),
        _mailField(),
        _doneButton()
      ],
    );
  }

  Widget showInciportOnMap() {
    final Completer<GoogleMapController> _controller = Completer();
    return InkWell(
        key: const Key('position'),
        child: Container(
          child: GoogleMap(
            zoomControlsEnabled: false,
            mapType: MapType.hybrid,
            initialCameraPosition: CameraPosition(
                target: LatLng(location.latitude, location.longitude),
                zoom: 17
            ),
            onMapCreated: (GoogleMapController controller) {
              _controller.complete(controller);
            },
            markers: {Marker(
              markerId: const MarkerId('inciport Location'),
              position: LatLng(location.latitude, location.longitude),
            )},
          ),
          height: 150,
        ),
    );
  }

  Widget _mainCategorySelection() {
    if (bloc.categories.isEmpty) {
      getCategoriesAndSetState();
    }
    return Container(
        decoration: BoxDecoration(
          border: Border.all(
            color: Colors.lightBlue,
          ),
          borderRadius: BorderRadius.circular(10.0),
        ),
        child: Center(
          child: DropdownButton<String>(
              key: const Key('MainCategories'),
              hint: Text(mainCategoryDropDownText, style: const TextStyle(color: Colors.black)),
              value: null,
              icon: const Icon(Icons.arrow_downward),
              isExpanded: true,
              iconSize: 24,
              elevation: 16,
              onChanged: (String? value) {
                setState(() {
                  mainCategoryDropDownText = value!;
                  subCategoryDropDownText = 'Choose a Sub Category';
                  bloc.subCategoryId = null;
                });
              },
              items: bloc.categoryListToMainCategoryItems()
          ),
        )
    );
  }

  void getCategoriesAndSetState() async {
    if (await bloc.getAvailableCategories(location)) {
      setState(() {});
    } else {
      showPopupWithText('Fetch of possible categories for the incident report failed, please contact a inciport provider', context, restartapp: true);
    }
  }

  Widget _subCategorySelection() {
    if (bloc.categories.isNotEmpty &&
        bloc.categories[bloc.indexOfMainCategory].subCategories != null) {
      return Container(
          decoration: BoxDecoration(
            border: Border.all(
              color: Colors.lightBlue,
            ),
            borderRadius: BorderRadius.circular(10.0),
          ),
          child: Center(
            child: DropdownButton<String>(
                key: const Key('SubCategories'),
                hint: Text(subCategoryDropDownText, style: const TextStyle(color: Colors.black)),
                value: null,
                icon: const Icon(Icons.arrow_downward),
                isExpanded: true,
                iconSize: 24,
                elevation: 16,
                onChanged: (String? value) {
                  setState(() {
                    subCategoryDropDownText = value!;
                  });
                },
                items: bloc.categoryListToSubCategoryItems()
            ),
          )
      );
    } else {
      return const Padding(padding: EdgeInsets.all(30));
    }
  }

  Widget _description() {
    return TextField(
      key: const Key('description'),
      maxLines: null,
      controller: bloc.descriptionController,
      decoration: const InputDecoration(
        border: OutlineInputBorder(),
        labelText: 'Enter a description here', labelStyle: TextStyle(color: Colors.black),
      ),
      onChanged: (text) => {bloc.isDoneButtonDisabled = !bloc.enableSubmitButton()}
    );
  }

  Widget _nameField() {
    return TextField(
      key: const Key('nameField'),
      controller: bloc.nameFieldController,
      decoration: const InputDecoration(
        border: OutlineInputBorder(),
        labelText: 'Your name',
      ),
    );
  }

  Widget _phoneNrField() {
    return TextField(
      key: const Key('phoneField'),
      controller: bloc.phoneNrFieldController,
      decoration: const InputDecoration(
        border: OutlineInputBorder(),
        labelText: 'Your phone number',
      ),
    );
  }

  Widget _mailField() {
    return TextField(
      key: const Key('mailField'),
      controller: bloc.mailFieldController,
      decoration: const InputDecoration(
        border: OutlineInputBorder(),
        labelText: 'Your email',
      ),
    );
  }

  Widget _doneButton() {
    return ElevatedButton(
      key: const Key('submit'),
      style: raisedButtonStyle,
      onPressed: bloc.isDoneButtonDisabled ? null : () {
        String? sendStatus = bloc.checkInfoAndSend(location);
        if (sendStatus != null) {
          showPopupWithText(sendStatus, context);
        } else {
          Navigator.pop(context);
          showPopupWithText('Incident report submitted, your submitted incident report should be visible on the map in a couple of minutes', context, restartapp: true);
        }
      },
      child: const Text('Submit incident report'),
    );
  }

  Widget _pictureDisplayInRow(int firstPictureNum) {
    return Row(
      children: [
        pictureOrIcon(firstPictureNum),
        pictureOrIcon(firstPictureNum + 1)
      ],
    );
  }

  Widget pictureOrIcon(int pictureNumber) {
    if (pictureNumber <= bloc.images.length) { // return selected image
      return Expanded(child: Image.file(bloc.images[pictureNumber - 1]));
    } else if (pictureNumber == bloc.images.length + 1) { // return plus icon
      return Expanded(
          child: PopupMenuButton(
              icon: const Icon(Icons.add),
              itemBuilder: (context) =>
              [
                PopupMenuItem(
                    child: const Text("Picture from Camera"),
                    value: 1,
                    onTap: () => getImageFromCamera()
                ),
                PopupMenuItem(
                    child: const Text("Image from Gallery"),
                    value: 2,
                    onTap: () => getImageFromGallery()
                )
              ]
          )
      );
    } else {
      return Container();
    }
  }

  void getImageFromGallery() async {
    final ImagePicker picker = ImagePicker();

    XFile? img = await picker.pickImage(source: ImageSource.gallery);
    if (bloc.checkIfImageAndStoreIt(img)){
      setState(() {});
    }
  }

  void getImageFromCamera() async {
    final ImagePicker picker = ImagePicker();

    XFile? img = await picker.pickImage(source: ImageSource.camera);

    if (bloc.checkIfImageAndStoreIt(img)){
      setState(() {});
    }
  }


}
