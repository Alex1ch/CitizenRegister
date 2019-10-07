/**
 * This view is an example list of people.
 */
Ext.define('CitizenRegisterFront.view.main.List', {
    extend: 'Ext.grid.Panel',
    xtype: 'mainlist',

    requires: [
        'CitizenRegisterFront.store.Citizen'
    ],

    layout: 'fit',

    title: 'Список анкет',
    multiSelect: true, 
        
    store: {
        type: 'citizen'
    },

    columns: [
        { text: 'Фамилия', dataIndex: 'surname', flex: 1  },
        { text: 'Имя', dataIndex: 'name', flex: 1 },
        { text: 'Отчество', dataIndex: 'middleName', flex: 1 },
        { text: 'Дата Рождения', dataIndex: 'birthDate', renderer: function (value) { return Ext.Date.format(value, 'd.m.Y D') }, flex: 1 }
    ],

    updateData: function () {

        if (typeof this.pageSize === "undefined")
            this.pageSize = 14;

        if (typeof this.currentPage === "undefined")
            this.currentPage = {value: 0};

        if (typeof this.formData === "undefined")
            this.formData = {};


        var pageSize = this.pageSize;
        var currentPage = this.currentPage;
        var formData = this.formData;
        var store = this.getStore();
        var window = this;

        Ext.Ajax.request({
            method: "POST",
            url: Ext.String.format('api/citizen/find?count={0}&offset={1}', pageSize, currentPage.value * pageSize),

            jsonData: formData,

            success: function(response, opts) {
                var obj = Ext.decode(response.responseText);

                store.setData(obj);
                store.sync();
            },

            failure: function (response, opts) {
                if (currentPage.value <= 0) {
                    store.setData([]);
                    store.sync();
                } else {
                    currentPage.value -= 1;
                    window.updateData();
                }
            }
        });
    },

    buttons: [
        {
            text: 'Печать',
            handler: function () {
                var record = this.up('panel').getSelectionModel().getSelection()[0];

                var renderWindow = new Ext.create('CitizenRegisterFront.view.main.Report');
                renderWindow.show();
                renderWindow.init();
            }
        },
        {
            text: 'Удалить',

            handler: function () {
                var records = this.up('panel').getSelectionModel().getSelection();
                var success = true;
                var tabPanel = Ext.ComponentQuery.query('mainlist')[0];

                Ext.Msg.confirm('Подтвердите удаление', 'Вы уверены, что хотите удалить записи?', function (choice) {
                    if (choice === 'no') return;
                    
                    Ext.each(records, function (record) {
                        Ext.Ajax.request({
                            async: false,
                            method: "DELETE",
                            url: Ext.String.format('api/citizen?id={0}', record.data.id),

                            success: function (response, opts) {
                            },

                            failure: function (response, opts) {
                                success = false;
                            }
                        });
                    });

                    if (!success)
                        Ext.Msg.alert('Ошибка', 'Не удалось удалить одну или несколько записей. ');
                    else
                        tabPanel.updateData();
                });
            }
        },
        {
            text: 'Редактировать',
            handler: function () {
                var record = this.up('panel').getSelectionModel().getSelection()[0];

                var editWindow = new Ext.create('CitizenRegisterFront.view.main.Edit');
                editWindow.show();
                editWindow.init(record);
            }
        },
        {
            text: 'Добавить',
            handler: function() {
                var editWindow = new Ext.create('CitizenRegisterFront.view.main.Edit');
                editWindow.show();
                editWindow.init();
            }
        },
        {
            text: '<- Назад',
            handler: function () {
                var currentPage = this.up('panel').currentPage;
                if (currentPage.value <= 0) return;
                
                currentPage.value -= 1;

                var pageSize = this.up('panel').pageSize;
                var currentPage = this.up('panel').currentPage;
                var formData = this.up('panel').formData;
                var store = this.up('panel').getStore();

                Ext.Ajax.request({
                    method: "POST",
                    url: Ext.String.format('api/citizen/find?count={0}&offset={1}', pageSize, currentPage.value * pageSize),

                    jsonData: formData,

                    success: function (response, opts) {
                        var obj = Ext.decode(response.responseText);

                        store.setData(obj);
                        store.sync();
                    },

                    failure: function (response, opts) {
                    }
                });
            }
        },
        {
            text: 'Далее ->',
            handler: function () {
                var currentPage = this.up('panel').currentPage;

                currentPage.value += 1;

                var pageSize = this.up('panel').pageSize;
                var currentPage = this.up('panel').currentPage;
                var formData = this.up('panel').formData;
                var store = this.up('panel').getStore();

                Ext.Ajax.request({
                    method: "POST",
                    url: Ext.String.format('api/citizen/find?count={0}&offset={1}', pageSize, currentPage.value * pageSize),

                    jsonData: formData,

                    success: function (response, opts) {
                        var obj = Ext.decode(response.responseText);
                        
                        store.setData(obj);
                        store.sync();
                    },

                    failure: function (response, opts) {
                        currentPage.value -= 1;
                    }
                });
            }
        }
    ]
});
