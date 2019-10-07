/**
 * This class is the controller for the main view for the application. It is specified as
 * the "controller" of the Main view class.
 */
Ext.define('CitizenRegisterFront.view.main.MainController', {
    extend: 'Ext.app.ViewController',

    requires: [
        'CitizenRegisterFront.view.main.Edit'
        ],

    alias: 'controller.main',

    onItemSelected: function (sender, record) {
        console.log(record);
        var editWindow = new Ext.create('CitizenRegisterFront.view.main.Edit');
        editWindow.show();
    },

    onConfirm: function (choice) {
        if (choice === 'yes') {
            //
        }
    }
});
