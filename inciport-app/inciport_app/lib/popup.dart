import 'package:flutter/material.dart';

import 'config.dart';
import 'main.dart';

void showPopupWithText (String text, BuildContext context, {bool? restartapp}) {

  showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
            key: const Key('popup'),
            content: Container(
                height: MediaQuery.of(context).size.height / 3,
                child: Column(
                  children: <Widget>[
                    Center(
                        child: Text(text)
                    ),
                    Expanded(
                        child: Container()
                    ),
                    ElevatedButton(
                      style: raisedButtonStyle,
                      onPressed: () => {
                        Navigator.of(context).pop(),
                        if (restartapp != null && restartapp == true) {
                          RestartWidget.restartApp(context)
                        }
                      },
                      child: const Text('OK'),
                    ),
                  ],
                )
            )
        );
      }
  );
}