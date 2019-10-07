/**
 * This view is an example list of people.
 */
Ext.define('CitizenRegisterFront.view.main.Edit', {
    extend: 'Ext.window.Window',
    xtype: 'edit-window',
    
    title: 'Анкета Гражданина',

    height: 350,
    width: 500,
    modal: true,
    
    items: [
        {
            xtype: 'textfield',
            padding: '14 0 0 14',
            id: 'editSurname',
            fieldLabel: 'Фамилия',
            name: 'surname',
            allowBlank: true
        }, {
            xtype: 'textfield',
            padding: '0 0 0 14',
            id: 'editName',
            fieldLabel: 'Имя',
            name: 'name',
            allowBlank: true
        }, {
            xtype: 'textfield',
            padding: '0 0 0 14',
            id: 'editMiddleName',
            fieldLabel: 'Отчество',
            name: 'middleName',
            allowBlank: true
        }, {
            xtype: 'datefield',
            padding: '0 0 0 14',
            id: 'editBirthDate',
            //format: 'd-m-Y',
            fieldLabel: 'Дата Рождения',
            name: 'birthDate',
            allowBlank: true
        }
    ],

    buttons: [
        {
            text: 'Сохранить',
            id: 'saveButton',
            handler: function () {
                var record = this.up('window').record;
                var window = this.up('window');
                var mainlist = Ext.ComponentQuery.query('mainlist')[0];

                var formData = {};

                formData['surname'] = Ext.getCmp('editSurname').getValue();
                formData['name'] = Ext.getCmp('editName').getValue();
                formData['middleName'] = Ext.getCmp('editMiddleName').getValue();
                formData['birthDate'] = Ext.Date.localToUtc(Ext.getCmp('editBirthDate').getValue());

                console.log(formData);

                if (typeof record === "undefined") {

                    Ext.Ajax.request({
                        method: "POST",
                        url: 'api/citizen',
                        
                        jsonData: formData,

                        success: function (response, opts) {
                            window.close()
                            Ext.Msg.alert('Успех', 'Новая анкета успешно создана');
                            mainlist.updateData();
                        },

                        failure: function (response, opts) {
                            Ext.Msg.alert('Ошибка', 'Не удалось создать новую анкету');
                        }
                    });
                } else {
                    formData['id'] = record.data.id;

                    if (formData['surname'] == record.data.surname &&
                        formData['name'] == record.data.name &&
                        formData['middleName'] == record.data.middleName &&
                        formData['birthDate'] == record.data.birthDate) return;


                    Ext.Ajax.request({
                        method: "PUT",
                        url: 'api/citizen',

                        jsonData: formData,

                        success: function (response, opts) {
                            window.close()
                            Ext.Msg.alert('Успех', 'Анкета успешно сохранена');
                            mainlist.updateData();
                        },

                        failure: function (response, opts) {
                            Ext.Msg.alert('Ошибка', 'Не удалось сохранить анкету');
                        }
                    });
                }
            }
        }
    ],

    init: function (record) {
        console.log(record);
        if (typeof record === "undefined") {
            Ext.getCmp('saveButton').setText('Создать');

        } else {
            this.record = record;
            Ext.getCmp('editSurname').setValue(record.data.surname);
            Ext.getCmp('editName').setValue(record.data.name);
            Ext.getCmp('editMiddleName').setValue(record.data.middleName);
            Ext.getCmp('editBirthDate').setValue(record.data.birthDate);
        }
    },
});
