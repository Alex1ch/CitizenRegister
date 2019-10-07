/*
 * This file launches the application by asking Ext JS to create
 * and launch() the Application class.
 */
Ext.application({
    extend: 'CitizenRegisterFront.Application',

    name: 'CitizenRegisterFront',

    requires: [
        // This will automatically load all classes in the CitizenRegisterFront namespace
        // so that application classes do not need to require each other.
        'CitizenRegisterFront.*'
    ],

    // The name of the initial view to create.
    mainView: 'CitizenRegisterFront.view.main.Main'
    //mainView: 'CitizenRegisterFront.view.main.Search'
});
