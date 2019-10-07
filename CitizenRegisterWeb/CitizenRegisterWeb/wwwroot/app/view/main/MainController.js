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
    
});
