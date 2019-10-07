/**
 * This view is an example list of people.
 */
Ext.define('CitizenRegisterFront.view.main.Report', {
    extend: 'Ext.window.Window',
    xtype: 'report-window',
    
    title: 'Генерация отчета',

    height: 350,
    width: 500,
    modal: true,
    
    items: [
        {
            xtype: 'numberfield',
            padding: '14 0 0 14',
            id: 'count',
            fieldLabel: 'Количество записей ',
            name: 'count',
            allowBlank: true
        }, {
            xtype: 'numberfield',
            padding: '0 0 0 14',
            id: 'offset',
            fieldLabel: 'Начиная с ',
            name: 'offset',
            allowBlank: true
        }
    ],

    buttons: [

        {
            text: 'Скачать PDF',
            handler: function () {
                var mainlist = Ext.ComponentQuery.query('mainlist')[0];
                var formData = mainlist.formData;

                var count = Ext.getCmp('count').getValue();
                var offset = Ext.getCmp('offset').getValue();

                var query = [];

                if (formData.name !== "") query.push(Ext.String.format("name={0}", formData.name));
                if (formData.surname !== "") query.push(Ext.String.format("surname={0}", formData.surname));
                if (formData.middleName !== "") query.push(Ext.String.format("middleName={0}", formData.middleName));
                if (formData.birthDateAfter !== "") query.push(Ext.String.format("birthDateAfter={0}", formData.birthDateAfter));
                if (formData.birthDateBefore !== "") query.push(Ext.String.format("birthDateBefore={0}", formData.birthDateBefore));

                var queryStr = "";

                for (i = 0; i < query.length; i++)
                {
                    queryStr += "&"+query[i];
                }

                location.href = Ext.String.format('/api/citizen/report?offset={0}&count={1}', offset, count) + queryStr ;
            }
        }
    ],

    init: function () {
        Ext.getCmp('count').setValue(100);
        Ext.getCmp('offset').setValue(0);
    }
});
