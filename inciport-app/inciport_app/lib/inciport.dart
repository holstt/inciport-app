import 'dart:convert';

enum ReportStatus {
  Recieved,
  InProgress,
  Rejected,
  Completed
}

class Inciport {
  final int? id;
  final String? status;
  final Category mainCategory;
  final Location location;
  final String description;
  final ContactInformation? contactInformation;
  final List<String>? imageStrings; //both for imageurls when getting and imageBase64 when posting from the webserver
  final DateTime? timestampCreatedUtc;
  final AssignedTeam? assignedTeam;

  // positional arguments used both when posting and recieving
  // named arguments only used when recieving
  Inciport(this.mainCategory, this.location,
      this.description, this.contactInformation, this.imageStrings,
      {this.id, this.status, this.timestampCreatedUtc, this.assignedTeam});

  factory Inciport.fromJson(Map<String, dynamic> json) => Inciport(

    Category.fromJson(json["chosenMainCategory"]),
    Location.fromJson(json["location"]),
    json["description"],
    json["contactInformation"] == null ? null : ContactInformation.fromJson(json["contactInformation"]),
    List<String>.from(json["imageUrls"].map((x) => x)),

    status: json["status"],
    id: json["id"],
    timestampCreatedUtc: DateTime.parse(json["timestampCreatedUtc"]),
    assignedTeam: json["assignedTeam"] == null ? null : AssignedTeam.fromJson(json["assignedTeam"]),
  );

  Map<String, dynamic> toJson() => {
    //'header' : {"Content-Type": "application/json"},
    'chosenMainCategory' : mainCategory,
    'location' : location,
    'description' : description,
    'contactInformation' : contactInformation,
    'imagesBase64' : imageStrings
  };
}

class Category {
  final Category? subCategory;
  final int id;
  final String? title;
  final String? description;

  Category(this.id, {this.title, this.description, this.subCategory});

  factory Category.fromRawJson(String str) => Category.fromJson(json.decode(str));

  String toRawJson() => json.encode(toJson());

  factory Category.fromJson(Map<String, dynamic> json) => Category(
    json["id"],
    subCategory: json["chosenSubCategory"] == null ? null : Category.fromJson(json["chosenSubCategory"]),
    title: json["title"],
    description: json["description"],
  );

  Map<String, dynamic> toJson() => {
    "id": id,
    if (subCategory != null) "chosenSubCategory": subCategory
  };
}


class Location {
  Address address;
  double longitude;
  double latitude;

  Location(this.address, this.longitude, this.latitude);

  factory Location.fromJson(Map<String, dynamic> json) => Location(
    Address.fromJson(json["address"]),
    json["longitude"].toDouble(),
    json["latitude"].toDouble(),
  );

  Map<String, dynamic> toJson() =>
      {
        'address' : address,
        'longitude': longitude,
        'latitude': latitude
      };
}

class Address {
  String street;
  String city;
  String zipCode;
  String country;
  String municipality;
  int? municipalityId;

  Address(this.street, this.city, this.zipCode, this.country, this.municipality, {this.municipalityId});

  factory Address.fromJson(Map<String, dynamic> json) => Address(
    json["street"],
    json["city"],
    json["zipCode"],
    json["country"],
    json["municipality"]
  );

  Map<String, dynamic> toJson() =>
      {
        'street': street,
        'city': city,
        'zipCode': zipCode,
        'country': country,
        'municipality': municipality
      };
}

class ContactInformation {
  String? name;
  String? phoneNumber;
  String? email;

  ContactInformation(this.name, this.phoneNumber, this.email);

  factory ContactInformation.fromJson(Map<String, dynamic> json) => ContactInformation(
    json["name"],
    json["phoneNumber"],
    json["email"],
  );

  Map<String, dynamic> toJson() =>
      {
        'name': name,
        'phoneNumber': phoneNumber,
        'email': email
      };
}

class AssignedTeam {
  final int id;
  final String name;

  AssignedTeam({
    required this.id,
    required this.name,
  });

  factory AssignedTeam.fromRawJson(String str) => AssignedTeam.fromJson(json.decode(str));

  String toRawJson() => json.encode(toJson());

  factory AssignedTeam.fromJson(Map<String, dynamic> json) => AssignedTeam(
    id: json["id"],
    name: json["name"],
  );

  Map<String, dynamic> toJson() => {
    "id": id,
    "name": name,
  };
}
