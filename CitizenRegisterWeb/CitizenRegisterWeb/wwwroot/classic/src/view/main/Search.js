
Ext.define('CitizenRegisterFront.view.main.Search', {
    extend: 'Ext.form.Panel',
    xtype: 'searchwindow',

    requires: [
        'Ext.plugin.Viewport',
        'Ext.window.MessageBox',
    ],
    
    items: [
        {
            xtype: 'fieldset',
            title: 'Условия поиска',
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'Фамилия',
                    name: 'surname',
                    allowBlank: true
                }, {
                    xtype: 'textfield',
                    fieldLabel: 'Имя',
                    name: 'name',
                    allowBlank: true
                }, {
                    xtype: 'textfield',
                    fieldLabel: 'Отчество',
                    name: 'middleName',
                    allowBlank: true
                }, {
                    xtype: 'datefield',
                    //format: 'd-m-Y',
                    fieldLabel: 'Дата Рождения После',
                    name: 'birthDateAfter',
                    allowBlank: true
                }, {
                    xtype: 'datefield',
                    //format: 'd-m-Y',
                    fieldLabel: 'Дата Рождения До',
                    name: 'birthDateBefore',
                    allowBlank: true
                }
            ]
        }
    ],

    buttons: [
        {
            text: 'Сбросить Форму',
            handler: function() {
                this.up('form').getForm().reset();
            }
        }, {
            text: 'Отправить',
            formBind: true, //only enabled once the form is valid
            disabled: true,
            handler: function () {
                var formData = this.up('form').getForm().getValues();
                
                if (formData["birthDateAfter"] === "")
                    delete formData["birthDateAfter"];

                if (formData["birthDateBefore"] === "")
                    delete formData["birthDateBefore"];
                
                var pageSize = 14;// HOW!?!??!?!?!?! this.up("panel").getViewModel().data.pageSize;

                Ext.Ajax.request({
                    method: "POST",
                    url: Ext.String.format('api/citizen/find?count={0}', pageSize),

                    jsonData: formData,

                    success: function (response, opts) {
                        var obj = Ext.decode(response.responseText);

                        var tabPanel = Ext.ComponentQuery.query('app-main')[0];
                        var mainlist = Ext.ComponentQuery.query('mainlist')[0];

                        mainlist.currentPage = { value: 0 };
                        mainlist.pageSize = pageSize;
                        mainlist.formData = formData;

                        var store = mainlist.getStore();
                        store.setData(obj);
                        store.sync();

                        tabPanel.setActiveItem(1);
                    },

                    failure: function (response, opts) {
                        Ext.Msg.alert('Ошибка', 'Ни одной записи не найдено', Ext.emptyFn);
                    }
                });
            }
        }
    ],
});
