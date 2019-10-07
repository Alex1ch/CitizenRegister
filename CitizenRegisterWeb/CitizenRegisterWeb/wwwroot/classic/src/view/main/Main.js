/**
 * This class is the main view for the application. It is specified in app.js as the
 * "mainView" property. That setting automatically applies the "viewport"
 * plugin causing this view to become the body element (i.e., the viewport).
 *
 * TODO - Replace this content of this view to suite the needs of your application.
 */

/**
 * This class is the main view for the application. It is specified in app.js as the
 * "mainView" property. That setting automatically applies the "viewport"
 * plugin causing this view to become the body element (i.e., the viewport).
 *
 * TODO - Replace this content of this view to suite the needs of your application.
 */
//Ext.define('CitizenRegisterFront.view.main.Main', {
//    extend: 'Ext.grid.Panel',
//    xtype: 'app-main',

//    requires: [
//        'Ext.plugin.Viewport',
//        'Ext.window.MessageBox',

//        'CitizenRegisterFront.view.main.MainController',
//        'CitizenRegisterFront.view.main.MainModel',
//        'CitizenRegisterFront.view.main.List',
//        'CitizenRegisterFront.view.main.Search'
//    ],

//    controller: 'main',
//    viewModel: 'main',


//    items: [
//        {
//            xtype: 'Ext.container.Viewport',
//            layout: 'fit',
//        }
//    ],

//    //items: [
//    //    {
//    //        xtype: 'fieldset',
//    //        title: 'Условия поиска',
//    //        items: [
//    //            {
//    //                xtype: 'textfield',
//    //                fieldLabel: 'Фамилия',
//    //                name: 'surname',
//    //                allowBlank: true
//    //            }, {
//    //                xtype: 'textfield',
//    //                fieldLabel: 'Имя',
//    //                name: 'name',
//    //                allowBlank: true
//    //            }, {
//    //                xtype: 'textfield',
//    //                fieldLabel: 'Отчество',
//    //                name: 'middleName',
//    //                allowBlank: true
//    //            }, {
//    //                xtype: 'textfield',
//    //                fieldLabel: 'Дата Рождения После',
//    //                name: 'birthDateAfter',
//    //                allowBlank: true
//    //            }, {
//    //                xtype: 'textfield',
//    //                fieldLabel: 'Дата Рождения До',
//    //                name: 'birthDateBefore',
//    //                allowBlank: true
//    //            }
//    //        ]
//    //    }
//    //],

//    tabBar: {
//        flex: 1,
//        layout: {
//            align: 'stretch',
//            overflowHandler: 'none'
//        }
//    },

//    buttons: [
//        {
//            text: 'Submit',
//            disabled: false,
//            handler: function () {
//                var window = new Ext.create('CitizenRegisterFront.view.main.Search');
//                window.show();
//            }
//        }
//    ],

//    header: {
//        title: {
//            bind: {
//                text: '{name}'
//            },
//            flex: 1
//        }
//    }
//});



Ext.define('CitizenRegisterFront.view.main.Main', {
    extend: 'Ext.tab.Panel',
    xtype: 'app-main',

    reference: 'mainWindow',

    requires: [
        'Ext.plugin.Viewport',
        'Ext.window.MessageBox',

        'CitizenRegisterFront.view.main.MainController',
        'CitizenRegisterFront.view.main.MainModel',
        'CitizenRegisterFront.view.main.Search',
        'CitizenRegisterFront.view.main.List'
    ],

    controller: 'main',
    viewModel: 'main',

    ui: 'navigation',

    tabBarHeaderPosition: 1,
    titleRotation: 0,
    tabRotation: 0,

    header: {
        layout: {
            align: 'stretchmax'
        },
        title: {
            bind: {
                text: '{name}'
            },
            flex: 0
        }
    },

    tabBar: {
        flex: 1,
        layout: {
            align: 'stretch',
            overflowHandler: 'none'
        }
    },

    responsiveConfig: {
        tall: {
            headerPosition: 'top'
        },
        wide: {
            headerPosition: 'left'
        }
    },

    defaults: {
        bodyPadding: 20,
        tabConfig: {
            responsiveConfig: {
                wide: {
                    iconAlign: 'left',
                    textAlign: 'left'
                },
                tall: {
                    iconAlign: 'top',
                    textAlign: 'center',
                    width: 120
                }
            }
        }
    },

    items: [
        {
            title: 'Поиск',
            items: [{
                xtype: 'searchwindow'
            }]
        },
        {
            title: 'Список',
            items: [{
                xtype: 'mainlist',
            }],

            tabConfig: {
                listeners: {
                    click: function () {
                        console.log("check");
                        Ext.ComponentQuery.query('mainlist')[0].updateData();
                    }
                }
            }
        }
    ]
});
